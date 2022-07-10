﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using SolastaCommunityExpansion.Utils;
using TA;
using UnityEngine;
using static SolastaCommunityExpansion.Api.DatabaseHelper.GadgetBlueprints;

namespace SolastaCommunityExpansion.Models;

internal static class GameUiContext
{
    private const int ExitsWithGizmos = 2;

    private static readonly GadgetBlueprint[] GadgetExits =
    {
        VirtualExit, VirtualExitMultiple, Exit, ExitMultiple, TeleporterIndividual, TeleporterParty
    };

    private static bool EnableDebugCamera { get; set; }

    internal static bool IsGadgetExit(GadgetBlueprint gadgetBlueprint, bool onlyWithGizmos = false)
    {
        return Array.IndexOf(GadgetExits, gadgetBlueprint) >= (onlyWithGizmos ? ExitsWithGizmos : 0);
    }

    internal static void Load()
    {
        var inputService = ServiceRepository.GetService<IInputService>();

        // Dungeon Maker
        inputService.RegisterCommand(InputCommands.Id.EditorRotate, (int)KeyCode.R, (int)KeyCode.LeftShift);

        // HUD
        inputService.RegisterCommand(Hotkeys.CtrlShiftC, (int)KeyCode.C, (int)KeyCode.LeftShift,
            (int)KeyCode.LeftControl);
        inputService.RegisterCommand(Hotkeys.CtrlShiftL, (int)KeyCode.L, (int)KeyCode.LeftShift,
            (int)KeyCode.LeftControl);
        inputService.RegisterCommand(Hotkeys.CtrlShiftM, (int)KeyCode.M, (int)KeyCode.LeftShift,
            (int)KeyCode.LeftControl);
        inputService.RegisterCommand(Hotkeys.CtrlShiftP, (int)KeyCode.P, (int)KeyCode.LeftShift,
            (int)KeyCode.LeftControl);
        inputService.RegisterCommand(Hotkeys.CtrlShiftH, (int)KeyCode.H, (int)KeyCode.LeftShift,
            (int)KeyCode.LeftControl);

        // Debug Overlay
        inputService.RegisterCommand(Hotkeys.CtrlShiftD, (int)KeyCode.D, (int)KeyCode.LeftShift,
            (int)KeyCode.LeftControl);

        // Export Character
        inputService.RegisterCommand(Hotkeys.CtrlShiftE, (int)KeyCode.E, (int)KeyCode.LeftShift,
            (int)KeyCode.LeftControl);

        // Spawn Encounter
        inputService.RegisterCommand(Hotkeys.CtrlShiftS, (int)KeyCode.S, (int)KeyCode.LeftShift,
            (int)KeyCode.LeftControl);

        // Teleport
        inputService.RegisterCommand(Hotkeys.CtrlShiftT, (int)KeyCode.T, (int)KeyCode.LeftShift,
            (int)KeyCode.LeftControl);

        // Zoom Camera
        inputService.RegisterCommand(Hotkeys.CtrlShiftZ, (int)KeyCode.Z, (int)KeyCode.LeftShift,
            (int)KeyCode.LeftControl);

        SwitchControlScheme();
    }

    internal static void SwitchControlScheme()
    {
        var inputManager = ServiceRepository.GetService<IInputService>();
        var controlScheme = Main.Settings.EnableGamepad
            ? InputDefinitions.ControlSchemeGamepad
            : InputDefinitions.ControlSchemeKeyboardMouse;

        inputManager.SwitchControlScheme(controlScheme);
    }

    internal static void HandleInput(GameLocationBaseScreen gameLocationBaseScreen, InputCommands.Id command)
    {
        if (Main.Settings.EnableHotkeyToggleIndividualHud)
        {
            switch (command)
            {
                case Hotkeys.CtrlShiftC:
                    GameHud.ShowCharacterControlPanel(gameLocationBaseScreen);
                    return;

                case Hotkeys.CtrlShiftL:
                    GameHud.TogglePanelVisibility(Gui.GuiService.GetScreen<GuiConsoleScreen>());
                    return;

                case Hotkeys.CtrlShiftM:
                    GameHud.TogglePanelVisibility(GetTimeAndNavigationPanel());
                    return;

                case Hotkeys.CtrlShiftP:
                    GameHud.TogglePanelVisibility(GetInitiativeOrPartyPanel());
                    return;
            }
        }

        if (Main.Settings.EnableHotkeyToggleHud && command == Hotkeys.CtrlShiftH)
        {
            GameHud.ShowAll(gameLocationBaseScreen, GetInitiativeOrPartyPanel(), GetTimeAndNavigationPanel());
        }
        else if (Main.Settings.EnableHotkeyDebugOverlay && command == Hotkeys.CtrlShiftD)
        {
            ServiceRepository.GetService<IDebugOverlayService>()?.ToggleActivation();
        }
        else if (Main.Settings.EnableTeleportParty && command == Hotkeys.CtrlShiftT)
        {
            Teleporter.ConfirmTeleportParty();
        }
        else if (Main.Settings.EnableHotkeyZoomCamera && command == Hotkeys.CtrlShiftZ)
        {
            ToggleZoomCamera();
        }
        else if (EncountersSpawnContext.EncounterCharacters.Count > 0 && command == Hotkeys.CtrlShiftS)
        {
            EncountersSpawnContext.ConfirmStageEncounter();
        }

        void ToggleZoomCamera()
        {
            var cameraService = ServiceRepository.GetService<ICameraService>();

            if (cameraService != null)
            {
                EnableDebugCamera = !EnableDebugCamera;
                cameraService.DebugCameraEnabled = EnableDebugCamera;
            }
        }

        [SuppressMessage("Minor Code Smell", "IDE0066:Use switch expression", Justification = "Prefer switch here")]
        [CanBeNull]
        GuiPanel GetInitiativeOrPartyPanel()
        {
            return gameLocationBaseScreen switch
            {
                GameLocationScreenExploration gameLocationScreenExploration => gameLocationScreenExploration
                    .partyControlPanel,
                GameLocationScreenBattle gameLocationScreenBattle => gameLocationScreenBattle.initiativeTable,
                _ => null
            };
        }

        [SuppressMessage("Minor Code Smell", "IDE0066:Use switch expression", Justification = "Prefer switch here")]
        [CanBeNull]
        TimeAndNavigationPanel GetTimeAndNavigationPanel()
        {
            return gameLocationBaseScreen switch
            {
                GameLocationScreenExploration gameLocationScreenExploration => gameLocationScreenExploration
                    .timeAndNavigationPanel,
                GameLocationScreenBattle gameLocationScreenBattle => gameLocationScreenBattle.timeAndNavigationPanel,
                _ => null
            };
        }
    }

    internal static class GameHud
    {
        internal static void ShowAll([NotNull] GameLocationBaseScreen gameLocationBaseScreen,
            GuiPanel initiativeOrPartyPanel,
            TimeAndNavigationPanel timeAndNavigationPanel)
        {
            var guiConsoleScreen = Gui.GuiService.GetScreen<GuiConsoleScreen>();
            var anyVisible = guiConsoleScreen.Visible || gameLocationBaseScreen.CharacterControlPanel.Visible ||
                             initiativeOrPartyPanel.Visible || timeAndNavigationPanel.Visible;

            ShowCharacterControlPanel(gameLocationBaseScreen, anyVisible);
            TogglePanelVisibility(guiConsoleScreen, anyVisible);
            TogglePanelVisibility(initiativeOrPartyPanel);
            TogglePanelVisibility(timeAndNavigationPanel, anyVisible);
        }

        internal static void ShowCharacterControlPanel([NotNull] GameLocationBaseScreen gameLocationBaseScreen,
            bool forceHide = false)
        {
            var characterControlPanel = gameLocationBaseScreen.CharacterControlPanel;

            if (characterControlPanel.Visible || forceHide)
            {
                characterControlPanel.Hide();
                characterControlPanel.Unbind();
            }
            else
            {
                var gameLocationSelectionService = ServiceRepository.GetService<IGameLocationSelectionService>();

                if (gameLocationSelectionService.SelectedCharacters.Count <= 0)
                {
                    return;
                }

                characterControlPanel.Bind(gameLocationSelectionService.SelectedCharacters[0],
                    gameLocationBaseScreen.ActionTooltipDock);
                characterControlPanel.Show();
            }
        }

        internal static void TogglePanelVisibility(GuiPanel guiPanel, bool forceHide = false)
        {
            if (guiPanel == null)
            {
                return;
            }

            if (guiPanel.Visible || forceHide)
            {
                guiPanel.Hide();
            }
            else
            {
                guiPanel.Show();
            }
        }

        public static void RefreshCharactrControlPanel()
        {
            if (Gui.CurrentLocationScreen != null && Gui.CurrentLocationScreen is GameLocationBaseScreen location)
            {
                location.CharacterControlPanel.RefreshNow();
            }
        }
    }

    private static class Teleporter
    {
        internal static void ConfirmTeleportParty()
        {
            var position = GetEncounterPosition();

            Gui.GuiService.ShowMessage(
                MessageModal.Severity.Attention2,
                "Message/&TeleportPartyTitle",
                Gui.Format("Message/&TeleportPartyDescription", position.x.ToString(), position.x.ToString()),
                "Message/&MessageYesTitle", "Message/&MessageNoTitle",
                () => TeleportParty(position),
                null);
        }

        private static int3 GetEncounterPosition()
        {
            var gameLocationService = ServiceRepository.GetService<IGameLocationService>();

            var x = (int)gameLocationService.GameLocation.LastCameraPosition.x;
            var z = (int)gameLocationService.GameLocation.LastCameraPosition.z;

            return new int3(x, 0, z);
        }

        private static void TeleportParty(int3 position)
        {
            var gameLocationActionService = ServiceRepository.GetService<IGameLocationActionService>();
            var gameLocationCharacterService = ServiceRepository.GetService<IGameLocationCharacterService>();
            var gameLocationPositioningService = ServiceRepository.GetService<IGameLocationPositioningService>();
            var formationPositions = new List<int3>();
            var partyAndGuests = new List<GameLocationCharacter>();
            var positions = new List<int3>();

            for (var iy = 0; iy < 4; iy++)
            {
                for (var ix = 0; ix < 2; ix++)
                {
                    formationPositions.Add(new int3(ix, 0, iy));
                }
            }

            partyAndGuests.AddRange(gameLocationCharacterService.PartyCharacters);
            partyAndGuests.AddRange(gameLocationCharacterService.GuestCharacters);

            gameLocationPositioningService.ComputeFormationPlacementPositions(partyAndGuests, position,
                LocationDefinitions.Orientation.North, formationPositions, CellHelpers.PlacementMode.Station,
                positions, new List<RulesetActor.SizeParameters>(), 25);

            for (var index = 0; index < positions.Count; index++)
            {
                partyAndGuests[index].LocationPosition = positions[index];

                // rotates the characters in position to force the game to redrawn them
                gameLocationActionService.MoveCharacter(partyAndGuests[index],
                    positions[(index + 1) % positions.Count], LocationDefinitions.Orientation.North, 0,
                    ActionDefinitions.MoveStance.Walk);
            }
        }
    }
}
