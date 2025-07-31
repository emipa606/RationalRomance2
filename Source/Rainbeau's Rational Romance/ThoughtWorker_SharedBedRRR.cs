using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace RationalRomance_Code;

public class ThoughtWorker_SharedBedRRR : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        if (!p.Spawned || !p.RaceProps.Humanlike || !LovePartnerRelationUtility.HasAnyLovePartner(p) ||
            p.ownership.OwnedBed == null)
        {
            return ThoughtState.Inactive;
        }

        var lovers = new List<Pawn>();
        var directRelations = p.relations.DirectRelations;
        foreach (var rel in directRelations)
        {
            if (LovePartnerRelationUtility.IsLovePartnerRelation(rel.def) && !rel.otherPawn.Dead)
            {
                lovers.Add(rel.otherPawn);
            }
        }

        var partnersPartners = SexualityUtilities.getAllLoverPawnsFirstRemoved(p);

        var partnerCount = 0;
        var otherPartners = 0;
        var stranger = false;

        if (lovers.Count < 1)
        {
            if (p.ownership.OwnedBed.OwnersForReading.Count > 1)
            {
                foreach (var otherPawn in p.ownership.OwnedBed.OwnersForReading)
                {
                    if (otherPawn == p)
                    {
                        continue;
                    }

                    stranger = true;
                }
            }
        }
        else
        {
            if (p.ownership.OwnedBed.OwnersForReading.Count > 1)
            {
                foreach (var otherPawn in p.ownership.OwnedBed.OwnersForReading)
                {
                    if (otherPawn == p)
                    {
                        continue;
                    }

                    if (!lovers.Contains(otherPawn))
                    {
                        if (partnersPartners.Contains(otherPawn))
                        {
                            otherPartners++;
                        }
                        else
                        {
                            stranger = true;
                        }
                    }
                    else
                    {
                        partnerCount++;
                    }
                }
            }
        }

        if (stranger)
        {
            //Stranger bed
            return ThoughtState.ActiveAtStage(0);
        }

        switch (partnerCount)
        {
            case > 1:
                //Polycule Bed
                return ThoughtState.ActiveAtStage(2);
            case > 0 when otherPartners > 0:
                //Partner of Polycule Bed
                return ThoughtState.ActiveAtStage(1);
            default:
                return ThoughtState.Inactive;
        }
    }
}