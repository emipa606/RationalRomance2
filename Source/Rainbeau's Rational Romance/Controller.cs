using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RationalRomance_Code;

public class Controller : Mod
{
    public static Settings Settings;

    public Controller(ModContentPack content) : base(content)
    {
        //HarmonyInstance harmony = HarmonyInstance.Create("net.rainbeau.rimworld.mod.rationalromance");
        var harmony = new Harmony("net.rainbeau.rimworld.mod.rationalromance");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        Settings = GetSettings<Settings>();
    }

    public override string SettingsCategory()
    {
        return "RRR.RationalRomance".Translate();
    }

    public override void DoSettingsWindowContents(Rect canvas)
    {
        Settings.DoWindowContents(canvas);
    }
}

//
// HARMONY PATCHES
//

//
// PawnRelationWorker_Child "CreateRelation"
//

//
// PawnRelationWorker_Sibling "CreateRelation"
//

//
// PawnRelationWorker_Sibling "GenerateParent"	
//
/*
[HarmonyPatch(typeof(ThoughtWorker_OpinionOfMyLover), "CurrentStateInternal")]
public static class ThoughtWorker_OpinionOfMyLover_CurrentStateInternal
    {
    // CHANGE: Allowed for polyamory.
    public static void Postfix(ref ThoughtState __result, Pawn p)
        {
        if (__result.StageIndex != ThoughtState.Inactive.StageIndex)
            {
            if (p.story.traits.HasTrait(RRRTraitDefOf.Polyamorous))
                {
                    ((ThoughtWorker_OpinionOfMyLover)null).

                /*List<Thought_Memory> thoughts = new List<Thought_Memory>();
                IEnumerable<Pawn> loveTree = (from r in p.relations.PotentiallyRelatedPawns where LovePartnerRelationUtility.LovePartnerRelationExists(p, r) select r);
                foreach (Pawn pawn in loveTree)
                    foreach (Thought_Memory mem in p.needs.mood.thoughts.memories.NumMemoriesOfDef.Memories)
                        {
                            Log.Message(pawn.Name+" mem " + mem.LabelCap);

                        }* /

                /*if (p.needs.mood.thoughts.memories.NumMemoriesOfDef() == null)
                    {
                    p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInBedroom);
                    Thought_Memory mem = p.needs.mood.thoughts.memories.GetFirstMemoryOfDef(ThoughtDefOf.SleptInBedroom);
                    if (mem != null)
                        {
                        mem.SetForcedStage(__result.StageIndex);
                        }
                    }
                }
                }
            }
        }
    }*/

//PawnRelationWorker_Lover
/*[HarmonyPatch(typeof(PawnRelationWorker_Lover), "OnRelationCreated")]
public static class PawnRelationWorker_OnRelationCreated
    {
    public static void Postfix(Pawn firstPawn, Pawn secondPawn)
        {
        //((PawnRelationWorker_Lover)null).OnRelationCreated
        Log.Message("RR ===             NEW RELATIONSHIP FEELS");
        secondPawn.needs.mood.thoughts.memories.TryGainMemory(RRRThoughtDefOf.NewRelationshipEnergy, firstPawn);
        firstPawn.needs.mood.thoughts.memories.TryGainMemory(RRRThoughtDefOf.NewRelationshipEnergy, secondPawn);
        }
    }*/

//
// NEW CODE
//