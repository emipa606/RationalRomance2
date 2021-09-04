using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace RationalRomance_Code
{
    public class JoyGiver_Date : JoyGiver
    {
        public static float percentRate = Controller.Settings.dateRate / 4;
        private readonly Dictionary<Pawn, long> dateCooldown = new Dictionary<Pawn, long>();

        public override Job TryGiveJob(Pawn pawn)
        {
            if (!dateCooldown.ContainsKey(pawn))
            {
                dateCooldown[pawn] = 0;
            }

            var cooldown = dateCooldown[pawn];
            if (Find.TickManager.TicksGame - cooldown < 60 * 100 && cooldown > 1)
            {
                return null;
            }


            Job result;
            if (!InteractionUtility.CanInitiateInteraction(pawn))
            {
                result = null;
            }
            else if (!LovePartnerRelationUtility.HasAnyLovePartner(pawn))
            {
                result = null;
            }
            else
            {
                var pawn2 = LovePartnerRelationUtility.ExistingLovePartner(pawn);
                if (!pawn2.Spawned)
                {
                    result = null;
                }
                else if (!pawn2.Awake())
                {
                    result = null;
                }
                else if (!JoyUtility.EnjoyableOutsideNow(pawn))
                {
                    result = null;
                }
                else if (PawnUtility.WillSoonHaveBasicNeed(pawn))
                {
                    result = null;
                }
                else if (100f * Rand.Value > percentRate && percentRate > 1)
                {
                    result = null;
                }
                else
                {
                    var cooldownTime = 1 * Find.TickManager.TicksGame;
                    dateCooldown[pawn] = cooldownTime;
                    result = new Job(def.jobDef, pawn2);
                }
            }

            return result;
        }
    }
}