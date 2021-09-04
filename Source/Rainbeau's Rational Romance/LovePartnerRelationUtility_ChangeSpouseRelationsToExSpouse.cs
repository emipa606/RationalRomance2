﻿using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RationalRomance_Code
{
    [HarmonyPatch(typeof(LovePartnerRelationUtility), "ChangeSpouseRelationsToExSpouse", null)]
    public static class LovePartnerRelationUtility_ChangeSpouseRelationsToExSpouse
    {
        // CHANGE: Allowed for polyamory.
        public static bool Prefix(Pawn pawn)
        {
            if (!pawn.story.traits.HasTrait(RRRTraitDefOf.Polyamorous))
            {
                return true;
            }

            var spouses = from p in pawn.relations.RelatedPawns
                where pawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, p)
                select p;
            foreach (var spousePawn in spouses)
            {
                if (spousePawn.story.traits.HasTrait(RRRTraitDefOf.Polyamorous))
                {
                    continue;
                }

                pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Spouse, spousePawn);
                pawn.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, spousePawn);
                //SexualityUtilities.updateMetamours(pawn,spousePawn);
                //SexualityUtilities.updateMetamours(spousePawn,pawn);
            }

            return false;
        }
    }
}