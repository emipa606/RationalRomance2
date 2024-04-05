﻿using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RationalRomance_Code;

[HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), nameof(InteractionWorker_RomanceAttempt.SuccessChance), null)]
public static class InteractionWorker_RomanceAttempt_SuccessChance
{
    // CHANGE: Updated with new orientation options and traits.
    // CHANGE: Pawns are more likely to rebuff non-ideal partners.
    // CHANGE: Allowed for polyamory.
    public static bool Prefix(Pawn initiator, Pawn recipient, ref float __result)
    {
        if (!recipient.story.traits.HasTrait(TraitDefOf.Asexual) &&
            !recipient.story.traits.HasTrait(TraitDefOf.Bisexual) &&
            !recipient.story.traits.HasTrait(TraitDefOf.Gay) &&
            !recipient.story.traits.HasTrait(RRRTraitDefOf.Straight))
        {
            ExtraTraits.AssignOrientation(recipient);
        }

        var single = 0.6f;
        single *= recipient.relations.SecondaryRomanceChanceFactor(initiator);
        single *= Mathf.InverseLerp(5f, 100f, recipient.relations.OpinionOf(initiator));
        var single1 = 1f;

        var isPoly = recipient.story.traits.HasTrait(RRRTraitDefOf.Polyamorous);

        //FIX: Makes sure partners cant romance the same f'n person again.
        if (initiator.relations.GetDirectRelation(PawnRelationDefOf.Lover, recipient) != null)
        {
            __result = 0f;
            return false;
        }

        if (initiator.relations.GetDirectRelation(PawnRelationDefOf.Spouse, recipient) != null)
        {
            __result = 0f;
            return false;
        }

        if (isPoly && !SexualityUtilities.HasNonPolyPartner(recipient))
        {
        }
        else
        {
            Pawn firstDirectRelationPawn = null;
            if (recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, x => !x.Dead) != null)
            {
                firstDirectRelationPawn = recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover);
                single1 = 0.6f;
                if (recipient.story.traits.HasTrait(RRRTraitDefOf.Faithful))
                {
                    single1 = 0f;
                }
            }
            else if (recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, x => !x.Dead) != null)
            {
                firstDirectRelationPawn = recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance);
                single1 = 0.1f;
                if (recipient.story.traits.HasTrait(RRRTraitDefOf.Faithful))
                {
                    single1 = 0f;
                }
            }
            else if (recipient.GetFirstSpouse() != null && !recipient.GetFirstSpouse().Dead)
            {
                firstDirectRelationPawn = recipient.GetFirstSpouse();
                single1 = 0.3f;
                if (recipient.story.traits.HasTrait(RRRTraitDefOf.Faithful))
                {
                    single1 = 0f;
                }
            }

            if (firstDirectRelationPawn != null)
            {
                single1 *= Mathf.InverseLerp(100f, 0f, recipient.relations.OpinionOf(firstDirectRelationPawn));
                if (recipient.story.traits.HasTrait(RRRTraitDefOf.Philanderer))
                {
                    single1 *= 1.6f;
                    if (firstDirectRelationPawn.Map != recipient.Map)
                    {
                        single1 *= 2f;
                    }
                }

                single1 *= Mathf.Clamp01(1f -
                                         recipient.relations.SecondaryRomanceChanceFactor(firstDirectRelationPawn));
            }
        }

        single *= single1;
        __result = Mathf.Clamp01(single);
        if (initiator.gender == recipient.gender && recipient.story.traits.HasTrait(RRRTraitDefOf.Straight))
        {
            __result *= 0.6f;
        }

        if (initiator.gender != recipient.gender && recipient.story.traits.HasTrait(TraitDefOf.Gay))
        {
            __result *= 0.6f;
        }

        if (recipient.story.traits.HasTrait(TraitDefOf.Asexual))
        {
            __result *= 0.3f;
        }

        return false;
    }
}