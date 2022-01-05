using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RationalRomance_Code;

public class InteractionWorker_NullWorker : InteractionWorker
{
    public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks,
        out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
    {
        base.Interacted(initiator, recipient, extraSentencePacks, out letterText, out letterLabel, out letterDef,
            out lookTargets);
        letterLabel = null;
        letterText = null;
        letterDef = null;
    }

    public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
    {
        return 0f;
    }
}