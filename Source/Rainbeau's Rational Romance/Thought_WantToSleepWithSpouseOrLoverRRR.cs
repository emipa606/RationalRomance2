using RimWorld;
using Verse;

namespace RationalRomance_Code
{
    public class Thought_WantToSleepWithSpouseOrLoverRRR : ThoughtWorker_WantToSleepWithSpouseOrLover
    {
        public override string PostProcessLabel(Pawn p, string label)
        {
            if (p.story.traits.HasTrait(RRRTraitDefOf.Polyamorous))
            {
                return string.Format(label, "my partners").CapitalizeFirst();
            }

            var directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, false);
            return string.Format(label, directPawnRelation.otherPawn.LabelShort).CapitalizeFirst();
        }
    }
}