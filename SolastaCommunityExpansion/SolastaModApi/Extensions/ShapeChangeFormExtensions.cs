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
    [TargetType(typeof(ShapeChangeForm))]
    public static partial class ShapeChangeFormExtensions
    {
        public static T SetKeepMentalAbilityScores<T>(this T entity, System.Boolean value)
            where T : ShapeChangeForm
        {
            entity.SetField("keepMentalAbilityScores", value);
            return entity;
        }

        public static T SetShapeChangeType<T>(this T entity, ShapeChangeForm.Type value)
            where T : ShapeChangeForm
        {
            entity.SetField("shapeChangeType", value);
            return entity;
        }

        public static T SetSpecialSubstituteCondition<T>(this T entity, ConditionDefinition value)
            where T : ShapeChangeForm
        {
            entity.SetField("specialSubstituteCondition", value);
            return entity;
        }
    }
}