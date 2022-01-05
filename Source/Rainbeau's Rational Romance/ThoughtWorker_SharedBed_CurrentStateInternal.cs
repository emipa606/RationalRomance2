using HarmonyLib;
using RimWorld;
using Verse;

namespace RationalRomance_Code;

[HarmonyPatch(typeof(ThoughtWorker_SharedBed), "CurrentStateInternal")]
public static class ThoughtWorker_SharedBed_CurrentStateInternal
{
    // CHANGE: Allowed for polyamory.
    public static void Postfix(ref ThoughtState __result, Pawn p)
    {
        __result = ThoughtState.Inactive;
    }
}