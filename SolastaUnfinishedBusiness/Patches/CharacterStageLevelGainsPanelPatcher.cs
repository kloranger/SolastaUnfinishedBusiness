﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using SolastaUnfinishedBusiness.Api.Helpers;
using SolastaUnfinishedBusiness.Models;

namespace SolastaUnfinishedBusiness.Patches;

public static class CharacterStageLevelGainPanelPatcher
{
    [HarmonyPatch(typeof(CharacterStageLevelGainsPanel), "EnterStage")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class EnterStage_Patch
    {
        // ReSharper disable once UnusedMember.Global
        public static void GetLastAssignedClassAndLevel(
            ICharacterBuildingService characterBuildingService,
            RulesetCharacterHero hero,
            out CharacterClassDefinition lastClassDefinition,
            out int level)
        {
            if (LevelUpContext.IsLevelingUp(hero))
            {
                //PATCH: mark we are beyond selecting classes (MULTICLASS)
                LevelUpContext.SetIsClassSelectionStage(hero, false);

                //PATCH: gets my own class and level for level up (MULTICLASS)
                lastClassDefinition = LevelUpContext.GetSelectedClass(hero);
                level = hero.ClassesHistory.Count;
            }
            else
            {
                characterBuildingService.GetLastAssignedClassAndLevel(hero, out lastClassDefinition, out level);
            }
        }

        [NotNull]
        public static IEnumerable<CodeInstruction> Transpiler([NotNull] IEnumerable<CodeInstruction> instructions)
        {
            var getLastAssignedClassAndLevelMethod =
                typeof(ICharacterBuildingService).GetMethod("GetLastAssignedClassAndLevel");
            var customGetLastAssignedClassAndLevelMethod =
                typeof(EnterStage_Patch).GetMethod("GetLastAssignedClassAndLevel");

            return TranspileHelper.ReplaceCodeCall(instructions, getLastAssignedClassAndLevelMethod,
                new CodeInstruction(OpCodes.Call, customGetLastAssignedClassAndLevelMethod));
        }
    }

    [HarmonyPatch(typeof(CharacterStageLevelGainsPanel), "RefreshSpellcastingFeatures")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class RefreshSpellcastingFeatures_Patch
    {
        //PATCH: only displays spell casting features from the current class (MULTICLASS)
        private static List<RulesetSpellRepertoire> SpellRepertoires(
            [NotNull] RulesetCharacterHero rulesetCharacterHero)
        {
            if (LevelUpContext.IsLevelingUp(rulesetCharacterHero) && LevelUpContext.IsMulticlass(rulesetCharacterHero))
            {
                return rulesetCharacterHero.SpellRepertoires
                    .Where(x => LevelUpContext.IsRepertoireFromSelectedClassSubclass(rulesetCharacterHero, x))
                    .ToList();
            }

            return rulesetCharacterHero.SpellRepertoires;
        }

        [NotNull]
        public static IEnumerable<CodeInstruction> Transpiler([NotNull] IEnumerable<CodeInstruction> instructions)
        {
            var spellRepertoiresMethod = typeof(RulesetCharacter).GetMethod("get_SpellRepertoires");
            var filteredSpellRepertoiresMethod =
                new Func<RulesetCharacterHero, List<RulesetSpellRepertoire>>(SpellRepertoires).Method;

            return TranspileHelper.ReplaceCodeCall(instructions, spellRepertoiresMethod,
                new CodeInstruction(OpCodes.Call, filteredSpellRepertoiresMethod));
        }
    }
}
