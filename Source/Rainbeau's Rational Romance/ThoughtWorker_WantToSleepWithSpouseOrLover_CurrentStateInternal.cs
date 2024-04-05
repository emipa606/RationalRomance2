using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RationalRomance_Code;

[HarmonyPatch(typeof(ThoughtWorker_WantToSleepWithSpouseOrLover),
    nameof(ThoughtWorker_WantToSleepWithSpouseOrLover.CurrentStateInternal))]
public static class ThoughtWorker_WantToSleepWithSpouseOrLover_CurrentStateInternal
{
    public static void Prefix(ref ThoughtState __result, Pawn p)
    {
        if (__result.StageIndex == ThoughtState.Inactive.StageIndex)
        {
            return;
        }

        if (p.ownership.OwnedBed == null)
        {
            return;
        }

        //DirectPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, false);
        var partners = from r in p.relations.PotentiallyRelatedPawns
            where LovePartnerRelationUtility.LovePartnerRelationExists(p, r)
            select r;
        var partnersInBed = from r in partners
            where p.ownership.OwnedBed.OwnersForReading.Contains(r)
            select r;
        var multiplePartners = partners.Count() > 1;


        if (partnersInBed.Any() && multiplePartners)
        {
            __result = ThoughtState.Inactive;
            return;
        }

        if (!p.story.traits.HasTrait(RRRTraitDefOf.Polyamorous))
        {
            return;
        }

        if (p.ownership.OwnedBed.GetRoom() == null)
        {
            return;
        }

        foreach (var bed in p.ownership.OwnedBed.GetRoom().ContainedBeds)
        {
            foreach (var pawn in bed.OwnersForReading)
            {
                if (!partners.Contains(pawn))
                {
                    continue;
                }

                __result = ThoughtState.Inactive;
                break;
            }

            if (!__result.Active)
            {
                break;
            }
        }
    }
}