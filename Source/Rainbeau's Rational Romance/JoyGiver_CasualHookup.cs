using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace RationalRomance_Code;

public class JoyGiver_CasualHookup : JoyGiver
{
    public static readonly float percentRate = Controller.Settings.hookupRate / 25;


    private readonly Dictionary<Pawn, long> hookupCooldown = new Dictionary<Pawn, long>();

    public override Job TryGiveJob(Pawn pawn)
    {
        hookupCooldown.TryAdd(pawn, 0);

        Job result;

        var tickTime = hookupCooldown.TryGetValue(pawn);
        long currentTime = Find.TickManager.TicksGame;
        if (currentTime - tickTime < 300)
        {
            hookupCooldown.Remove(pawn);
            hookupCooldown.Add(pawn, currentTime);
            return null;
        }

        if (!InteractionUtility.CanInitiateInteraction(pawn))
        {
            result = null;
        }
        else if (!SexualityUtilities.WillPawnTryHookup(pawn))
        {
            result = null;
        }
        else if (PawnUtility.WillSoonHaveBasicNeed(pawn))
        {
            result = null;
        }
        else
        {
            foreach (var job in pawn.jobs.jobQueue.ToList())
            {
                if (job.job.def == RRRJobDefOf.DoLovinCasual /* this.def.jobDef.GetType()*/)
                {
                    return null;
                }
            }

            var pawn2 = SexualityUtilities.FindAttractivePawn(pawn);
            if (pawn2 == null)
            {
                result = null;
            }
            else
            {
                if (100f * Rand.Value > percentRate)
                {
                    if (Controller.Settings.need_contact)
                    {
                        var touch_need = pawn.needs.TryGetNeed<Human_Contact_Need>();
                        if (touch_need != null)
                        {
                            var lev = touch_need.CurLevel;
                            if (lev <= touch_need.touch_deprived_threshold)
                            {
                                if (100f * Rand.Value < lev)
                                {
                                    return null;
                                }
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                var building_Bed = SexualityUtilities.FindHookupBed(pawn, pawn2);
                if (building_Bed == null)
                {
                    return null;
                }

                double distanceSqquared = pawn2.Position.DistanceToSquared(pawn.Position);
                if (distanceSqquared > Controller.Settings.hookupMaxRange * Controller.Settings.hookupMaxRange)
                {
                    return null;
                }

                result = new Job(def.jobDef, pawn, building_Bed);
                pawn.jobs.jobQueue.EnqueueFirst(new Job(def.jobDef, pawn2, building_Bed));
            }
        }

        return result;
    }
}