using System.Linq;
using RimWorld;
using Verse;

namespace RationalRomance_Code;

public class ThoughtWorker_NeedHumanContact : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        if (!Controller.Settings.need_contact || p.story.traits.HasTrait(TraitDefOf.Asexual))
        {
            return ThoughtState.Inactive;
        }

        var touchNeed = p.needs.TryGetNeed<Human_Contact_Need>();
        if (touchNeed == null)
        {
            return ThoughtState.Inactive;
        }

        var lev = touchNeed.CurLevel;
        if (lev <= touchNeed.touch_deprived_threshold)
            //deprived
        {
            return ThoughtState.ActiveAtStage(0);
        }

        if (!(lev <= touchNeed.lonely_threshold))
        {
            return ThoughtState.Inactive;
        }

        var partners = from r in p.relations.PotentiallyRelatedPawns
            where LovePartnerRelationUtility.LovePartnerRelationExists(p, r)
            select r;
        return ThoughtState.ActiveAtStage(!partners.Any()
            ?
            //singel and lonely
            1
            : 2);

        //lonely
    }
}