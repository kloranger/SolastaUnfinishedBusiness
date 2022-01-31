using SolastaModApi.Infrastructure;
using AK.Wwise;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System;
using System.Text;
using TA.AI;
using TA;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using  static  ActionDefinitions ;
using  static  TA . AI . DecisionPackageDefinition ;
using  static  TA . AI . DecisionDefinition ;
using  static  RuleDefinitions ;
using  static  BanterDefinitions ;
using  static  Gui ;
using  static  BestiaryDefinitions ;
using  static  CursorDefinitions ;
using  static  AnimationDefinitions ;
using  static  CharacterClassDefinition ;
using  static  CreditsGroupDefinition ;
using  static  CampaignDefinition ;
using  static  GraphicsCharacterDefinitions ;
using  static  GameCampaignDefinitions ;
using  static  TooltipDefinitions ;
using  static  BaseBlueprint ;
using  static  MorphotypeElementDefinition ;

namespace SolastaModApi.Extensions
{
    /// <summary>
    /// This helper extensions class was automatically generated.
    /// If you find a problem please report at https://github.com/SolastaMods/SolastaModApi/issues.
    /// </summary>
    [TargetType(typeof(LootPackDefinition))]
    public static partial class LootPackDefinitionExtensions
    {
        public static T SetInDungeonEditor<T>(this T entity, System.Boolean value)
            where T : LootPackDefinition
        {
            entity.InDungeonEditor = value;
            return entity;
        }

        public static T SetLootChallengeMode<T>(this T entity, LootPackDefinition.LootChallenge value)
            where T : LootPackDefinition
        {
            entity.SetField("lootChallengeMode", value);
            return entity;
        }

        public static T SetLootMagnitudeMode<T>(this T entity, LootPackDefinition.LootMagnitude value)
            where T : LootPackDefinition
        {
            entity.SetField("lootMagnitudeMode", value);
            return entity;
        }

        public static T SetLootSpawnMode<T>(this T entity, LootPackDefinition.LootSpawn value)
            where T : LootPackDefinition
        {
            entity.SetField("lootSpawnMode", value);
            return entity;
        }

        public static T SetUserLootPack<T>(this T entity, System.Boolean value)
            where T : LootPackDefinition
        {
            entity.SetField("<UserLootPack>k__BackingField", value);
            return entity;
        }
    }
}