﻿using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using static SolastaUnfinishedBusiness.Api.DatabaseHelper.CharacterClassDefinitions;

namespace SolastaUnfinishedBusiness.Patches;

public static class ShapeOptionItemPatcher
{
    [HarmonyPatch(typeof(ShapeOptionItem), "Bind")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Patch")]
    public static class Bind_Patch
    {
        public static void Postfix(
            ShapeOptionItem __instance,
            RulesetCharacter shifter,
            int requiredLevel)
        {
            //PATCH: uses class level when offering wildshape
            if (shifter is not RulesetCharacterHero rulesetCharacterHero ||
                !rulesetCharacterHero.ClassesAndLevels.TryGetValue(Druid, out var levels))
            {
                return;
            }

            var isShapeOptionAvailable = requiredLevel <= levels;

            __instance.levelLabel.TMP_Text.color = isShapeOptionAvailable
                ? __instance.validLevelColor
                : __instance.invalidLevelColor;
            __instance.toggle.interactable = isShapeOptionAvailable;
            __instance.canvasGroup.alpha = isShapeOptionAvailable ? 1f : 0.3f;
        }
    }
}
