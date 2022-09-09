﻿using System;
using SolastaCommunityExpansion.Builders;
using SolastaCommunityExpansion.Builders.Features;
using SolastaCommunityExpansion.CustomDefinitions;
using static FeatureDefinitionAttributeModifier.AttributeModifierOperation;
using static RuleDefinitions;
using static SolastaCommunityExpansion.Builders.Features.AutoPreparedSpellsGroupBuilder;
using static SolastaCommunityExpansion.Api.DatabaseHelper;
using static SolastaCommunityExpansion.Api.DatabaseHelper.CharacterSubclassDefinitions;
using static SolastaCommunityExpansion.Api.DatabaseHelper.FeatureDefinitionPowers;
using static SolastaCommunityExpansion.Api.DatabaseHelper.SpellDefinitions;

namespace SolastaCommunityExpansion.Subclasses.Druid;

internal sealed class CircleOfTheForestGuardian : AbstractSubclass
{
    private const string DruidForestGuardianName = "DruidForestGuardian";
    private static readonly Guid BaseGuid = new("45a7595b-5d5f-4351-b7f1-cb78c9d0a136");
    private CharacterSubclassDefinition Subclass;

    internal override FeatureDefinitionSubclassChoice GetSubclassChoiceList()
    {
        return FeatureDefinitionSubclassChoices.SubclassChoiceDruidCircle;
    }

    internal override CharacterSubclassDefinition GetSubclass()
    {
        return Subclass ??= BuildAndAddSubclass();
    }

    private static CharacterSubclassDefinition BuildAndAddSubclass()
    {
        // Create Auto-prepared Spell list
        var druidForestGuardianMagic = FeatureDefinitionAutoPreparedSpellsBuilder
            .Create("AutoPreparedSpellsForestGuardian", BaseGuid)
            .SetGuiPresentation("DruidForestGuardianMagic", Category.Feature)
            .SetPreparedSpellGroups(
                BuildSpellGroup(2, Shield, FogCloud),
                BuildSpellGroup(3, Blur, FlameBlade),
                BuildSpellGroup(5, ProtectionFromEnergy, DispelMagic),
                BuildSpellGroup(7, FireShield, DeathWard),
                BuildSpellGroup(9, HoldMonster, GreaterRestoration))
            .SetSpellcastingClass(CharacterClassDefinitions.Druid)
            .AddToDB();

        var extraAttack = FeatureDefinitionAttributeModifierBuilder
            .Create("AttributeModifierDruidForestGuardianExtraAttack", BaseGuid)
            .SetGuiPresentation("DruidForestGuardianExtraAttack", Category.Feature)
            .SetModifier(Additive, AttributeDefinitions.AttacksNumber, 1)
            .AddToDB();

        var sylvanResistance = FeatureDefinitionAttributeModifierBuilder
            .Create("AttributeModifierDruidForestGuardianSylvanDurability", BaseGuid)
            .SetGuiPresentation("DruidForestGuardianSylvanDurability", Category.Feature)
            .SetModifier(Additive, AttributeDefinitions.HitPointBonusPerLevel, 1)
            .AddToDB();

        // Create Sylvan War Magic
        var sylvanWarMagic = FeatureDefinitionMagicAffinityBuilder
            .Create(FeatureDefinitionMagicAffinitys.MagicAffinityBattleMagic,
                "MagicAffinityDruidForestGuardianSylvanWarMagic",
                BaseGuid)
            .SetGuiPresentation(Category.Feature)
            .AddToDB();

        var (barkWard, improvedBarkWard, superiorBarkWard) = CreateBarkWard();

        return CharacterSubclassDefinitionBuilder
            .Create(DruidForestGuardianName, BaseGuid)
            .SetGuiPresentation(Category.Subclass, MartialMountaineer.GuiPresentation.SpriteReference)
            .AddFeatureAtLevel(druidForestGuardianMagic, 2)
            .AddFeatureAtLevel(sylvanResistance, 2)
            .AddFeatureAtLevel(sylvanWarMagic, 2)
            .AddFeatureAtLevel(barkWard, 2)
            .AddFeatureAtLevel(extraAttack, 6)
            .AddFeatureAtLevel(improvedBarkWard, 10)
            .AddFeatureAtLevel(superiorBarkWard, 14)
            .AddToDB();
    }

    // Create Bark Ward Wild Shape Power (and the two higher variants, improved and superior)
    private static (FeatureDefinitionPowerSharedPool barkWard, FeatureDefinitionPowerSharedPool improvedBarkWard,
        FeatureDefinitionPowerSharedPool superiorBarkWard) CreateBarkWard()
    {
        var tempHpEffect = EffectFormBuilder
            .Create()
            .SetTempHPForm(4, DieType.D1, 0)
            .SetLevelAdvancement(EffectForm.LevelApplianceType.MultiplyBonus, LevelSourceType.ClassLevel)
            .CreatedByCharacter()
            .Build();

        var barkWardBuff = EffectFormBuilder
            .Create()
            .SetConditionForm(CreateConditionBarkWard(), ConditionForm.ConditionOperation.Add, true, true)
            .Build();

        var improvedBarkWardBuff = EffectFormBuilder
            .Create()
            .SetConditionForm(CreateConditionConditionImprovedBarkWard(), ConditionForm.ConditionOperation.Add, true,
                true)
            .Build();

        var superiorBarkWardBuff = EffectFormBuilder
            .Create()
            .SetConditionForm(CreateConditionConditionSuperiorBarkWard(), ConditionForm.ConditionOperation.Add, true,
                true)
            .Build();

        var barkWardEffectDescription = EffectDescriptionBuilder
            .Create()
            .SetTargetingData(Side.Ally, RangeType.Self, 1, TargetType.Self)
            .SetCreatedByCharacter()
            .SetDurationData(DurationType.Minute, 10, TurnOccurenceType.EndOfTurn)
            .AddEffectForm(tempHpEffect)
            .AddEffectForm(barkWardBuff)
            .SetEffectAdvancement(EffectIncrementMethod.None)
            .Build();

        var improvedBarkWardEffectDescription = EffectDescriptionBuilder
            .Create()
            .SetTargetingData(Side.Ally, RangeType.Self, 1, TargetType.Self)
            .SetCreatedByCharacter()
            .SetDurationData(DurationType.Minute, 10, TurnOccurenceType.EndOfTurn)
            .AddEffectForm(tempHpEffect)
            .AddEffectForm(improvedBarkWardBuff)
            .SetEffectAdvancement(EffectIncrementMethod.None)
            .Build();

        var superiorBarkWardEffectDescription = EffectDescriptionBuilder
            .Create()
            .SetTargetingData(Side.Ally, RangeType.Self, 1, TargetType.Self)
            .SetCreatedByCharacter()
            .SetDurationData(DurationType.Minute, 10, TurnOccurenceType.EndOfTurn)
            .AddEffectForm(tempHpEffect)
            .AddEffectForm(superiorBarkWardBuff)
            .SetEffectAdvancement(EffectIncrementMethod.None)
            .Build();

        var barkWard = FeatureDefinitionPowerSharedPoolBuilder
            .Create("PowerSharedPoolDruidForestGuardianBarkWard", BaseGuid)
            .SetGuiPresentation(Category.Feature, PowerDruidWildShape.GuiPresentation.SpriteReference)
            .Configure(
                PowerDruidWildShape, RechargeRate.ShortRest, ActivationTime.BonusAction, 1, false, false,
                AttributeDefinitions.Wisdom, barkWardEffectDescription, true)
            .AddToDB();

        var improvedBarkWard = FeatureDefinitionPowerSharedPoolBuilder
            .Create("PowerSharedForestGuardianImprovedBarkWard", BaseGuid)
            .SetGuiPresentation(Category.Feature, PowerDruidWildShape.GuiPresentation.SpriteReference)
            .Configure(
                PowerDruidWildShape, RechargeRate.ShortRest, ActivationTime.BonusAction, 1, false, false,
                AttributeDefinitions.Wisdom, improvedBarkWardEffectDescription, true)
            .SetOverriddenPower(barkWard)
            .AddToDB();

        var superiorBarkWard = FeatureDefinitionPowerSharedPoolBuilder
            .Create("PowerSharedForestGuardianSuperiorBarkWard", BaseGuid)
            .SetGuiPresentation(Category.Feature, PowerDruidWildShape.GuiPresentation.SpriteReference)
            .Configure(
                PowerDruidWildShape, RechargeRate.ShortRest, ActivationTime.BonusAction, 1, false, false,
                AttributeDefinitions.Wisdom, superiorBarkWardEffectDescription, true)
            .SetOverriddenPower(improvedBarkWard)
            .AddToDB();

        return (barkWard, improvedBarkWard, superiorBarkWard);

        static ConditionDefinition CreateConditionBarkWard()
        {
            return ConditionDefinitionBuilder
                .Create(ConditionDefinitions.ConditionBarkskin, "ConditionBarkWard", BaseGuid)
                .SetOrUpdateGuiPresentation("ConditionBarkWard", Category.Condition)
                .ClearFeatures()
                .SetAllowMultipleInstances(false)
                .SetDuration(DurationType.Minute, 10)
                .SetTurnOccurence(TurnOccurenceType.EndOfTurn)
                .AddToDB();
        }

        static ConditionDefinition CreateConditionConditionImprovedBarkWard()
        {
            var damageEffect = EffectFormBuilder
                .Create()
                .SetDamageForm(false, DieType.D8, "DamagePiercing", 0, DieType.D8, 2)
                .CreatedByCondition()
                .Build();

            var improvedBarkWardRetaliationEffect = EffectDescriptionBuilder
                .Create()
                .AddEffectForm(damageEffect)
                .Build();

            var improvedBarkWardDamageRetaliate = FeatureDefinitionPowerBuilder
                .Create("PowerImprovedBarkWardRetaliate", BaseGuid)
                .SetGuiPresentationNoContent()
                .Configure(
                    0, UsesDetermination.Fixed, AttributeDefinitions.Wisdom, ActivationTime.NoCost,
                    0, RechargeRate.AtWill, false, false, AttributeDefinitions.Wisdom,
                    improvedBarkWardRetaliationEffect, true)
                .AddToDB();

            var improvedBarkWardDamage = FeatureDefinitionDamageAffinityBuilder
                .Create("DamageAffinityImprovedBarkWardRetaliation", BaseGuid)
                .SetGuiPresentationNoContent()
                .SetDamageAffinityType(DamageAffinityType.None)
                .SetDamageType(DamageTypePoison)
                .SetRetaliate(improvedBarkWardDamageRetaliate, 1, true)
                .SetAncestryDefinesDamageType(false)
                .AddToDB();

            return ConditionDefinitionBuilder
                .Create(ConditionDefinitions.ConditionBarkskin, "ConditionImprovedBarkWard", BaseGuid)
                .SetOrUpdateGuiPresentation("ConditionConditionImprovedBarkWard", Category.Condition)
                .SetAllowMultipleInstances(false)
                .SetDuration(DurationType.Minute, 10)
                .SetTurnOccurence(TurnOccurenceType.EndOfTurn)
                .SetFeatures(improvedBarkWardDamage)
                .AddToDB();
        }

        static ConditionDefinition CreateConditionConditionSuperiorBarkWard()
        {
            var damageEffect = EffectFormBuilder
                .Create()
                .SetDamageForm(false, DieType.D8, "DamagePiercing", 0, DieType.D8, 3)
                .CreatedByCondition()
                .Build();

            var superiorBarkWardRetaliationEffect = EffectDescriptionBuilder
                .Create()
                .AddEffectForm(damageEffect)
                .Build();

            var PowerSuperiorBarkWardRetaliatePower = FeatureDefinitionPowerBuilder
                .Create("PowerSuperiorBarkWardRetaliate", BaseGuid)
                .SetGuiPresentationNoContent()
                .Configure(
                    0, UsesDetermination.Fixed, AttributeDefinitions.Wisdom, ActivationTime.NoCost,
                    0, RechargeRate.AtWill, false, false,
                    AttributeDefinitions.Wisdom, superiorBarkWardRetaliationEffect, true)
                .AddToDB();

            var PowerSuperiorBarkWardRetaliateDamageAffinity = FeatureDefinitionDamageAffinityBuilder
                .Create("DamageAffinitySuperiorBarkWardRetaliation", BaseGuid)
                .SetGuiPresentationNoContent()
                .SetDamageAffinityType(DamageAffinityType.Immunity)
                .SetDamageType(DamageTypePoison)
                .SetRetaliate(PowerSuperiorBarkWardRetaliatePower, 1, true)
                .SetAncestryDefinesDamageType(false)
                .AddToDB();

            return ConditionDefinitionBuilder
                .Create(ConditionDefinitions.ConditionBarkskin, "ConditionSuperiorBarkWard", BaseGuid)
                .SetOrUpdateGuiPresentation("ConditionConditionSuperiorBarkWard", Category.Condition)
                .SetFeatures(PowerSuperiorBarkWardRetaliateDamageAffinity)
                .SetAllowMultipleInstances(false)
                .SetDuration(DurationType.Minute, 10)
                .SetTurnOccurence(TurnOccurenceType.EndOfTurn)
                .AddToDB();
        }
    }
}
