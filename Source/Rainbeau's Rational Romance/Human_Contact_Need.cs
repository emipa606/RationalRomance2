using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace RationalRomance_Code;

public class Human_Contact_Need : Need_Seeker
{
    private const float fallRate = 0.5f;
    public float lonely_threshold = 0.50f;

    public float touch_deprived_threshold = 0.05f;

    public Human_Contact_Need(Pawn pawn) : base(pawn)
    {
        threshPercents = new List<float>
        {
            touch_deprived_threshold,
            lonely_threshold
        };
    }

    private bool isInvisible => pawn.Map == null;

    public float getInRelationshipModifier(IEnumerable<Pawn> partners)
    {
        if (partners.Count() > 1)
        {
            return 3f;
        }

        return 1f;
    }

    public override void NeedInterval() //150 ticks between each calls
    {
        if (AndroidsCompatibility.IsAndroid(pawn) || !Controller.Settings.need_contact)
        {
            pawn.needs.AllNeeds.Remove(this);
            CurLevel = 1;
            return;
        }
        //if (isInvisible)
        //	return;

        var partners = from r in pawn.relations.PotentiallyRelatedPawns
            where LovePartnerRelationUtility.LovePartnerRelationExists(pawn, r)
            select r;

        var fallPerInterval = 150 * (fallRate / 5000000f) * getInRelationshipModifier(partners);

        if (partners.Count() > 1)
        {
            if (pawn.InBed())
            {
                var partnersInBed =
                    from r in partners where pawn.CurrentBed().OwnersForReading.Contains(r) select r;
                if (partnersInBed.Count() > 1)
                {
                    CurLevel += 10 * fallPerInterval;
                }
            }
        }

        CurLevel -= fallPerInterval;
        if (CurLevel > 1)
        {
            CurLevel = 1;
        }
    }
}