﻿using SolastaCommunityExpansion.Builders;
using SolastaModApi;
using SolastaModApi.Extensions;
using System.Collections.Generic;

namespace SolastaCommunityExpansion.Classes.Tinkerer
{
    // TODO see if there's interest in moving this to CE builders
    class CancelConditionPowerBuilder : BaseDefinitionBuilder<FeatureDefinitionPower>
    {
        public CancelConditionPowerBuilder(string name, string guid, GuiPresentation presentation, ConditionDefinition condition) : base(name, guid)
        {
            Definition.GuiPresentation = presentation;
            Definition.SetFixedUsesPerRecharge(1);
            Definition.SetUsesDetermination(RuleDefinitions.UsesDetermination.Fixed);
            Definition.SetRechargeRate(RuleDefinitions.RechargeRate.AtWill);
            Definition.SetActivationTime(RuleDefinitions.ActivationTime.NoCost);
            EffectDescriptionBuilder effectDescriptionBuilder = new EffectDescriptionBuilder();
            effectDescriptionBuilder.SetTargetingData(RuleDefinitions.Side.Ally, RuleDefinitions.RangeType.Self, 1, RuleDefinitions.TargetType.Self, 1, 1, ActionDefinitions.ItemSelectionType.None);
            effectDescriptionBuilder.AddEffectForm(new EffectFormBuilder()
                .SetConditionForm(condition, ConditionForm.ConditionOperation.Remove, false, false, new List<ConditionDefinition>()).Build());
            Definition.SetEffectDescription(effectDescriptionBuilder.Build());
        }
    }
}
