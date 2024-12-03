using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RationalRomance_Code;

[HarmonyPatch(typeof(InteractionWorker_RomanceAttempt),
    nameof(InteractionWorker_RomanceAttempt.BreakLoverAndFianceRelations), null)]
public static class InteractionWorker_RomanceAttempt_BreakLoverAndFianceRelations
{
    // CHANGE: Allowed for polyamory.
    public static bool Prefix(Pawn pawn, ref List<Pawn> oldLoversAndFiances)
    {
        if (pawn.story.traits.HasTrait(RRRTraitDefOf.Polyamorous))
        {
            oldLoversAndFiances = [];
            foreach (DirectPawnRelation relation in pawn.relations.DirectRelations.Where(relation => relation.def == PawnRelationDefOf.Lover || relation.def == PawnRelationDefOf.Fiance).ToList())
            {
                if (relation.otherPawn.story.traits.HasTrait(RRRTraitDefOf.Polyamorous))
                {
                    continue;
                }
                else if (relation.def == PawnRelationDefOf.Lover) {
                        pawn.relations.AddDirectRelation(PawnRelationDefOf.ExLover, relation.otherPawn);
                        pawn.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, relation.otherPawn);
                }
                else if (relation.def == PawnRelationDefOf.Fiance)
                {
                    pawn.relations.AddDirectRelation(PawnRelationDefOf.ExLover, relation.otherPawn);
                    pawn.relations.TryRemoveDirectRelation(PawnRelationDefOf.Fiance, relation.otherPawn);
                }
            }
            return false;
        }
        return true;
    }
}