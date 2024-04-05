using System.Linq;
using RimWorld;
using Verse;

namespace RationalRomance_Code;

public class ThoughtWorker_NeedHumanContact : ThoughtWorker
{
    public override ThoughtState CurrentStateInternal(Pawn p)
    {
        if (!Controller.Settings.need_contact)
        {
            return ThoughtState.Inactive;
        }

        if (p.story.traits.HasTrait(TraitDefOf.Asexual))
        {
            return ThoughtState.Inactive;
        }

        var touch_need = p.needs.TryGetNeed<Human_Contact_Need>();
        if (touch_need == null)
        {
            return ThoughtState.Inactive;
        }

        var lev = touch_need.CurLevel;
        if (lev <= touch_need.touch_deprived_threshold)
            //deprived
        {
            return ThoughtState.ActiveAtStage(0);
        }

        if (!(lev <= touch_need.lonely_threshold))
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