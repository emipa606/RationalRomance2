using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace RationalRomance_Code;

public class JoyGiver_Date : JoyGiver
{
    public static readonly float percentRate = Controller.Settings.dateRate / 4;
    private readonly Dictionary<Pawn, long> dateCooldown = new Dictionary<Pawn, long>();

    public override Job TryGiveJob(Pawn pawn)
    {
        dateCooldown.TryAdd(pawn, 0);

        var cooldown = dateCooldown[pawn];
        if (Find.TickManager.TicksGame - cooldown < 60 * 100 && cooldown > 1)
        {
            return null;
        }


        if (!InteractionUtility.CanInitiateInteraction(pawn))
        {
            return null;
        }

        if (!LovePartnerRelationUtility.HasAnyLovePartner(pawn))
        {
            return null;
        }

        var pawn2 = LovePartnerRelationUtility.ExistingLovePartners(pawn, false).RandomElement();
        if (!pawn2.otherPawn.Spawned)
        {
            return null;
        }

        if (!pawn2.otherPawn.Awake())
        {
            return null;
        }

        if (!JoyUtility.EnjoyableOutsideNow(pawn))
        {
            return null;
        }

        if (PawnUtility.WillSoonHaveBasicNeed(pawn))
        {
            return null;
        }

        if (100f * Rand.Value > percentRate && percentRate > 1)
        {
            return null;
        }

        var cooldownTime = 1 * Find.TickManager.TicksGame;
        dateCooldown[pawn] = cooldownTime;
        return new Job(def.jobDef, pawn2.otherPawn);
    }
}