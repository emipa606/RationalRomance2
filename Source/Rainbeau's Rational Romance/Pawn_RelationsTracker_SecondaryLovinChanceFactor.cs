using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RationalRomance_Code;

[HarmonyPatch(typeof(Pawn_RelationsTracker), "SecondaryLovinChanceFactor", null)]
public static class Pawn_RelationsTracker_SecondaryLovinChanceFactor
{
    // CHANGE: Updated with new orientation options.
    // CHANGE: Gender age preferences are now the same, except for mild cultural variation.
    // CHANGE: Pawns with Ugly trait are less uninterested romantically in other ugly pawns.

    public static bool Prefix(Pawn otherPawn, ref float __result, ref Pawn_RelationsTracker __instance, Pawn ___pawn)
    {
        if (___pawn == otherPawn)
        {
            __result = 0f;
            return false;
        }

        if ((!___pawn.RaceProps.Humanlike || !otherPawn.RaceProps.Humanlike) && ___pawn.def != otherPawn.def)
        {
            __result = 0f;
            return false;
        }

        float crossSpecies = 1;
        if (___pawn.def != otherPawn.def)
        {
            crossSpecies = Controller.Settings.alienLoveChance / 100;
        }

        if (Rand.ValueSeeded(___pawn.thingIDNumber ^ 3273711) >= 0.015f)
        {
            if (___pawn.RaceProps.Humanlike && ___pawn.story.traits.HasTrait(TraitDefOf.Asexual))
            {
                __result = 0f;
                return false;
            }

            if (___pawn.RaceProps.Humanlike && ___pawn.story.traits.HasTrait(TraitDefOf.Gay))
            {
                if (otherPawn.gender != ___pawn.gender)
                {
                    __result = 0f;
                    return false;
                }
            }

            if (___pawn.RaceProps.Humanlike && ___pawn.story.traits.HasTrait(RRRTraitDefOf.Straight))
            {
                if (otherPawn.gender == ___pawn.gender)
                {
                    __result = 0f;
                    return false;
                }
            }
        }

        var ageBiologicalYearsFloat = ___pawn.ageTracker.AgeBiologicalYearsFloat;
        var targetAge = otherPawn.ageTracker.AgeBiologicalYearsFloat;
        if (targetAge < 16f)
        {
            __result = 0f;
            return false;
        }

        var youngestTargetAge = Mathf.Max(16f, ageBiologicalYearsFloat - 30f);
        var youngestReasonableTargetAge = Mathf.Max(20f, ageBiologicalYearsFloat, ageBiologicalYearsFloat - 10f);
        var targetAgeLikelihood = GenMath.FlatHill(0.15f, youngestTargetAge, youngestReasonableTargetAge,
            ageBiologicalYearsFloat + 7f, ageBiologicalYearsFloat + 30f, 0.15f, targetAge);
        var targetBaseCapabilities = 1f;
        targetBaseCapabilities *=
            Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Talking));
        targetBaseCapabilities *=
            Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation));
        targetBaseCapabilities *=
            Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
        var initiatorBeauty = 0;
        var targetBeauty = 0;
        if (otherPawn.RaceProps.Humanlike)
        {
            initiatorBeauty = ___pawn.story.traits.DegreeOfTrait(TraitDefOf.Beauty);
        }

        if (otherPawn.RaceProps.Humanlike)
        {
            targetBeauty = otherPawn.story.traits.DegreeOfTrait(TraitDefOf.Beauty);
        }

        var targetBeautyMod = 1f;
        switch (targetBeauty)
        {
            case -2:
                targetBeautyMod = initiatorBeauty >= 0 ? 0.3f : 0.8f;
                break;
            case -1:
                targetBeautyMod = initiatorBeauty >= 0 ? 0.75f : 0.9f;
                break;
            case 1:
                targetBeautyMod = 1.7f;
                break;
            case 2:
                targetBeautyMod = 2.3f;
                break;
        }

        var backgroundCulture = SexualityUtilities.GetAdultCulturalAdjective(___pawn);
        var ageDiffPref = 1f;
        if (backgroundCulture is "Urbworld" or "Medieval")
        {
            if (___pawn.gender == Gender.Male && otherPawn.gender == Gender.Female)
            {
                ageDiffPref = ageBiologicalYearsFloat <= targetAge ? 0.8f : 1.2f;
            }
            else if (___pawn.gender == Gender.Female && otherPawn.gender == Gender.Male)
            {
                ageDiffPref = ageBiologicalYearsFloat <= targetAge ? 1.2f : 0.8f;
            }
        }

        if (backgroundCulture is "Tribal" or "Imperial")
        {
            if (___pawn.gender == Gender.Male && otherPawn.gender == Gender.Female)
            {
                ageDiffPref = ageBiologicalYearsFloat <= targetAge ? 1.2f : 0.8f;
            }
            else if (___pawn.gender == Gender.Female && otherPawn.gender == Gender.Male)
            {
                ageDiffPref = ageBiologicalYearsFloat <= targetAge ? 0.8f : 1.2f;
            }
        }

        var initiatorYoung = Mathf.InverseLerp(15f, 18f, ageBiologicalYearsFloat);
        var targetYoung = Mathf.InverseLerp(15f, 18f, targetAge);
        __result = targetAgeLikelihood * ageDiffPref * targetBaseCapabilities * initiatorYoung * targetYoung *
                   targetBeautyMod * crossSpecies * Controller.Settings.secondaryLovinChanceCoefficient;
        return false;
    }
}