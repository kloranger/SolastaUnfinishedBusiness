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
    [TargetType(typeof(SlotTypeDefinition))]
    public static partial class SlotTypeDefinitionExtensions
    {
        public static T SetAlwaysLocked<T>(this T entity, System.Boolean value)
            where T : SlotTypeDefinition
        {
            entity.SetField("alwaysLocked", value);
            return entity;
        }

        public static T SetBodySlot<T>(this T entity, System.Boolean value)
            where T : SlotTypeDefinition
        {
            entity.SetField("bodySlot", value);
            return entity;
        }

        public static T SetCanDisplayArmor<T>(this T entity, System.Boolean value)
            where T : SlotTypeDefinition
        {
            entity.SetField("canDisplayArmor", value);
            return entity;
        }

        public static T SetCanDisplayArmorForPhoto<T>(this T entity, System.Boolean value)
            where T : SlotTypeDefinition
        {
            entity.SetField("canDisplayArmorForPhoto", value);
            return entity;
        }

        public static T SetCanDisplayLight<T>(this T entity, System.Boolean value)
            where T : SlotTypeDefinition
        {
            entity.SetField("canDisplayLight", value);
            return entity;
        }

        public static T SetCanStack<T>(this T entity, System.Boolean value)
            where T : SlotTypeDefinition
        {
            entity.SetField("canStack", value);
            return entity;
        }

        public static T SetDisplayArmorSortingIndex<T>(this T entity, System.Int32 value)
            where T : SlotTypeDefinition
        {
            entity.SetField("displayArmorSortingIndex", value);
            return entity;
        }

        public static T SetHasDefaultVisual<T>(this T entity, System.Boolean value)
            where T : SlotTypeDefinition
        {
            entity.SetField("hasDefaultVisual", value);
            return entity;
        }

        public static T SetLockedInBattle<T>(this T entity, System.Boolean value)
            where T : SlotTypeDefinition
        {
            entity.SetField("lockedInBattle", value);
            return entity;
        }
    }
}