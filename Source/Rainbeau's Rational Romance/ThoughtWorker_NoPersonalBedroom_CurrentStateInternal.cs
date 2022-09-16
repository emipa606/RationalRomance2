using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RationalRomance_Code;

[HarmonyPatch(typeof(ThoughtWorker_NoPersonalBedroom), "CurrentStateInternal")]
public static class ThoughtWorker_NoPersonalBedroom_CurrentStateInternal
{
    // CHANGE: Allowed for polyamory.
    public static void Postfix(ref ThoughtState __result, Pawn p)
    {
        var loveTree = SexualityUtilities.getAllLoverPawnsFirstRemoved(p);
        var hasStranger = false;
        if (p.ownership.OwnedBed != null && p.ownership.OwnedBed.GetRoom() != null)
        {
            foreach (var bed in p.ownership.OwnedBed.GetRoom().ContainedBeds)
            {
                foreach (var pawn in bed.OwnersForReading)
                {
                    if (loveTree.Contains(pawn) || pawn == p)
                    {
                        continue;
                    }

                    hasStranger = true;
                    break;
                }
            }
        }

        if (hasStranger)
        {
            return;
        }

        if (p.IsPrisoner)
        {
            return;
        }

        var memB = p.needs.mood.thoughts.memories.GetFirstMemoryOfDef(ThoughtDefOf.SleptInBarracks);
        if (memB == null)
        {
            return;
        }

        if (p.needs.mood.thoughts.memories.GetFirstMemoryOfDef(ThoughtDefOf.SleptInBedroom) == null)
        {
            p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInBedroom);
        }

        if (ThoughtDefOf.SleptInBedroom.stages.Count >= memB.CurStageIndex &&
            ThoughtDefOf.SleptInBedroom.stages[memB.CurStageIndex] != null)
        {
            var mem = p.needs.mood.thoughts.memories.GetFirstMemoryOfDef(ThoughtDefOf.SleptInBedroom);
            mem?.SetForcedStage(memB.CurStageIndex);
        }

        __result = ThoughtState.Inactive;
        p.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInBarracks);
    }
}