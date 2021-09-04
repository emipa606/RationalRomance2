using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace RationalRomance_Code
{
    public class JobDriver_LeadHookup : JobDriver
    {
        public bool successfulPass = true;

        public bool wasSuccessfulPass => successfulPass;

        private Pawn actor => GetActor();

        private Pawn TargetPawn => TargetThingA as Pawn;

        private Building_Bed TargetBed => TargetThingB as Building_Bed;

        private TargetIndex TargetPawnIndex => TargetIndex.A;

        private TargetIndex TargetBedIndex => TargetIndex.B;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        private bool DoesTargetPawnAcceptAdvance()
        {
            return !PawnUtility.WillSoonHaveBasicNeed(TargetPawn) &&
                   !PawnUtility.EnemiesAreNearby(TargetPawn) &&
                   TargetPawn.CurJob.def != JobDefOf.LayDown &&
                   TargetPawn.CurJob.def != JobDefOf.BeatFire &&
                   TargetPawn.CurJob.def != JobDefOf.Arrest &&
                   TargetPawn.CurJob.def != JobDefOf.Capture &&
                   TargetPawn.CurJob.def != JobDefOf.EscortPrisonerToBed &&
                   TargetPawn.CurJob.def != JobDefOf.ExtinguishSelf &&
                   TargetPawn.CurJob.def != JobDefOf.FleeAndCower &&
                   TargetPawn.CurJob.def != JobDefOf.MarryAdjacentPawn &&
                   TargetPawn.CurJob.def != JobDefOf.PrisonerExecution &&
                   TargetPawn.CurJob.def != JobDefOf.ReleasePrisoner &&
                   TargetPawn.CurJob.def != JobDefOf.Rescue &&
                   TargetPawn.CurJob.def != JobDefOf.SocialFight &&
                   TargetPawn.CurJob.def != JobDefOf.SpectateCeremony &&
                   TargetPawn.CurJob.def != JobDefOf.TakeToBedToOperate &&
                   TargetPawn.CurJob.def != JobDefOf.TakeWoundedPrisonerToBed &&
                   TargetPawn.CurJob.def != JobDefOf.UseCommsConsole &&
                   TargetPawn.CurJob.def != JobDefOf.Vomit &&
                   TargetPawn.CurJob.def != JobDefOf.Wait_Downed &&
                   SexualityUtilities.WillPawnTryHookup(TargetPawn) &&
                   SexualityUtilities.IsHookupAppealing(TargetPawn, GetActor());
        }

        private bool IsTargetPawnOkay()
        {
            return !TargetPawn.Dead && !TargetPawn.Downed;
        }

        private bool IsTargetPawnFreeForHookup()
        {
            var freeOtherwise = !PawnUtility.WillSoonHaveBasicNeed(TargetPawn) &&
                                !PawnUtility.EnemiesAreNearby(TargetPawn) &&
                                TargetPawn.CurJob.def != JobDefOf.LayDown &&
                                TargetPawn.CurJob.def != JobDefOf.BeatFire &&
                                TargetPawn.CurJob.def != JobDefOf.Arrest &&
                                TargetPawn.CurJob.def != JobDefOf.Capture &&
                                TargetPawn.CurJob.def != JobDefOf.EscortPrisonerToBed &&
                                TargetPawn.CurJob.def != JobDefOf.ExtinguishSelf &&
                                TargetPawn.CurJob.def != JobDefOf.FleeAndCower &&
                                TargetPawn.CurJob.def != JobDefOf.MarryAdjacentPawn &&
                                TargetPawn.CurJob.def != JobDefOf.PrisonerExecution &&
                                TargetPawn.CurJob.def != JobDefOf.ReleasePrisoner &&
                                TargetPawn.CurJob.def != JobDefOf.Rescue &&
                                TargetPawn.CurJob.def != JobDefOf.SocialFight &&
                                TargetPawn.CurJob.def != JobDefOf.SpectateCeremony &&
                                TargetPawn.CurJob.def != JobDefOf.TakeToBedToOperate &&
                                TargetPawn.CurJob.def != JobDefOf.TakeWoundedPrisonerToBed &&
                                TargetPawn.CurJob.def != JobDefOf.UseCommsConsole &&
                                TargetPawn.CurJob.def != JobDefOf.Vomit &&
                                TargetPawn.CurJob.def != JobDefOf.Wait_Downed;
            //Taget is vomiting or something and our pawn should not attempt hookup.
            if (!freeOtherwise)
            {
                return false;
            }

            //CHANGE: There is no need to try to hook up with pawns that are already hooking up with someone else.
            if (TargetPawn.CurJob.def != JobDefOf.Lovin && TargetPawn.CurJob.def != RRRJobDefOf.DoLovinCasual)
            {
                return true;
            }

            if (TargetPawn.story.traits.HasTrait(RRRTraitDefOf.Polyamorous) &&
                GetActor().story.traits.HasTrait(RRRTraitDefOf.Polyamorous))
            {
                //If both are poly, I'll let the pawn be ballsy enough to attempt a threesome.
                return true;
            }

            return false;
        }

        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
            //Don't try to hook up with yourself!
            if (actor == TargetPawn)
            {
                yield break;
            }


            foreach (var queuedJob in pawn.jobs.jobQueue.ToList())
            {
                if (queuedJob.job.def == RRRJobDefOf.LeadHookup)
                {
                    yield break;
                }
            }


            if (!IsTargetPawnFreeForHookup())
            {
                yield break;
            }

            yield return Toils_Goto.GotoThing(TargetPawnIndex, PathEndMode.Touch);
            var TryItOn = new Toil();
            TryItOn.AddFailCondition(() => !IsTargetPawnOkay());
            TryItOn.defaultCompleteMode = ToilCompleteMode.Delay;
            TryItOn.initAction = delegate
            {
                ticksLeftThisToil = 50;
                FleckMaker.ThrowMetaIcon(GetActor().Position, GetActor().Map, FleckDefOf.Heart);
            };
            yield return TryItOn;
            var AwaitResponse = new Toil
            {
                defaultCompleteMode = ToilCompleteMode.Instant,
                initAction = delegate
                {
                    var list = new List<RulePackDef>();
                    successfulPass = DoesTargetPawnAcceptAdvance();
                    if (successfulPass)
                    {
                        FleckMaker.ThrowMetaIcon(TargetPawn.Position, TargetPawn.Map, FleckDefOf.Heart);
                        list.Add(RRRMiscDefOf.HookupSucceeded);
                    }
                    else
                    {
                        FleckMaker.ThrowMetaIcon(TargetPawn.Position, TargetPawn.Map, FleckDefOf.IncapIcon);
                        GetActor().needs.mood.thoughts.memories
                            .TryGainMemory(RRRThoughtDefOf.RebuffedMyHookupAttempt, TargetPawn);
                        TargetPawn.needs.mood.thoughts.memories.TryGainMemory(RRRThoughtDefOf.FailedHookupAttemptOnMe,
                            GetActor());
                        list.Add(RRRMiscDefOf.HookupFailed);
                    }

                    Find.PlayLog.Add(new PlayLogEntry_Interaction(RRRMiscDefOf.TriedHookupWith, pawn, TargetPawn,
                        list));
                }
            };
            AwaitResponse.AddFailCondition(() => !wasSuccessfulPass);
            yield return AwaitResponse;
            if (wasSuccessfulPass)
            {
                yield return new Toil
                {
                    defaultCompleteMode = ToilCompleteMode.Instant,
                    initAction = delegate
                    {
                        if (!wasSuccessfulPass)
                        {
                            return;
                        }

                        GetActor().jobs.jobQueue.EnqueueFirst(new Job(RRRJobDefOf.DoLovinCasual, TargetPawn,
                            TargetBed, TargetBed.GetSleepingSlotPos(0)));
                        TargetPawn.jobs.jobQueue.EnqueueFirst(new Job(RRRJobDefOf.DoLovinCasual, GetActor(),
                            TargetBed, TargetBed.GetSleepingSlotPos(1)));
                        //TEST: If we swap to regular lovin, does RiceRiceBaby still work.
                        //this.GetActor().jobs.jobQueue.EnqueueFirst(new Job(JobDefOf.Lovin, this.TargetPawn, this.TargetBed, this.TargetBed.GetSleepingSlotPos(0)), null);
                        //this.TargetPawn.jobs.jobQueue.EnqueueFirst(new Job(JobDefOf.Lovin, this.GetActor(), this.TargetBed, this.TargetBed.GetSleepingSlotPos(1)), null);
                        GetActor().jobs.EndCurrentJob(JobCondition.InterruptOptional);
                        if (TargetPawn == null)
                        {
                            return;
                        }

                        if (TargetPawn.jobs == null)
                        {
                            return;
                        }

                        if (TargetPawn.jobs.IsCurrentJobPlayerInterruptible())
                        {
                            TargetPawn.jobs.EndCurrentJob(JobCondition.InterruptOptional);
                        }
                    }
                };
            }
        }
    }
}