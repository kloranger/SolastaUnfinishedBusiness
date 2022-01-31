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
    [TargetType(typeof(DestinationLocationDescription))]
    public static partial class DestinationLocationDescriptionExtensions
    {
        public static T SetDisplayedTitle<T>(this T entity, System.String value)
            where T : DestinationLocationDescription
        {
            entity.DisplayedTitle = value;
            return entity;
        }

        public static T SetEntranceIndex<T>(this T entity, System.Int32 value)
            where T : DestinationLocationDescription
        {
            entity.EntranceIndex = value;
            return entity;
        }

        public static T SetLocationDefinition<T>(this T entity, LocationDefinition value)
            where T : DestinationLocationDescription
        {
            entity.LocationDefinition = value;
            return entity;
        }

        public static T SetUserLocationName<T>(this T entity, System.String value)
            where T : DestinationLocationDescription
        {
            entity.UserLocationName = value;
            return entity;
        }
    }
}