﻿namespace SolastaUnfinishedBusiness.CustomBehaviors;

//NOTE: Patch in RulesetActorPatcher for this feature is currently disabled, since this handler is not used anywhere at the moment
internal delegate void ProcessConditionInterruptionHandler(
    RulesetActor actor,
    RuleDefinitions.ConditionInterruption interruption,
    int amount);
