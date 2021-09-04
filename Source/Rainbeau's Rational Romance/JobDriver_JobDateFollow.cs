﻿using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using Verse;
using Verse.AI;

namespace RationalRomance_Code
{
    public class JobDriver_JobDateFollow : JobDriver
    {
        private readonly TargetIndex PartnerInd = TargetIndex.A;

        private Pawn actor => GetActor();

        private Pawn Partner => (Pawn)(Thing)job.GetTarget(PartnerInd);

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        public override RandomSocialMode DesiredSocialMode()
        {
            return RandomSocialMode.SuperActive;
        }

        //private bool IsPartnerNearby() {
        //	return this.actor.Position.InHorDistOf(this.Partner.Position, 2f);
        //}
        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
            var FollowPartner = new Toil
            {
                defaultCompleteMode = ToilCompleteMode.Delay
            };
            FollowPartner.AddFailCondition(() => !Partner.Spawned);
            FollowPartner.AddFailCondition(() => Partner.Dead);
            FollowPartner.AddFailCondition(() => Partner.CurJob.def != RRRJobDefOf.JobDateLead);
            FollowPartner.initAction = delegate
            {
                ticksLeftThisToil = 200;
                actor.pather.StartPath(Partner, PathEndMode.Touch);
            };
            FollowPartner.tickAction = delegate { actor.needs.joy.GainJoy(0.0001f, RRRMiscDefOf.Social); };
            for (var i = 0; i < 100; i++)
            {
                yield return FollowPartner;
            }
        }
    }
}