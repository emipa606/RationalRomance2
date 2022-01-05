using RimWorld;
using Verse;

namespace RationalRomance_Code;

public class PawnRelationWorker_Lover_NRE : PawnRelationWorker_Lover
{
    public override void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
    {
        Log.Message("RR ===  NEW RELATIONSHIP FEELS");
        secondPawn.needs.mood.thoughts.memories.TryGainMemory(RRRThoughtDefOf.NewRelationshipEnergy, firstPawn);
        firstPawn.needs.mood.thoughts.memories.TryGainMemory(RRRThoughtDefOf.NewRelationshipEnergy, secondPawn);
        base.OnRelationCreated(firstPawn, secondPawn);
    }
}