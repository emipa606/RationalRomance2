using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace RationalRomance_Code;

public class ThoughtWorker_SharedBedRRR : ThoughtWorker
{
    public override ThoughtState CurrentStateInternal(Pawn p)
    {
        if (!p.Spawned)
        {
            return ThoughtState.Inactive;
        }

        if (!p.RaceProps.Humanlike)
        {
            return ThoughtState.Inactive;
        }

        if (!LovePartnerRelationUtility.HasAnyLovePartner(p))
        {
            return ThoughtState.Inactive;
        }

        if (p.ownership.OwnedBed == null)
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

        var partnerspartners = SexualityUtilities.getAllLoverPawnsFirstRemoved(p);

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
                        if (partnerspartners.Contains(otherPawn))
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

        if (partnerCount > 1)
        {
            //Polycule Bed
            return ThoughtState.ActiveAtStage(2);
        }

        if (partnerCount > 0 && otherPartners > 0)
        {
            //Partner of Polycule Bed
            return ThoughtState.ActiveAtStage(1);
        }

        return ThoughtState.Inactive;
    }
}