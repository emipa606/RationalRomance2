﻿using HarmonyLib;
using RimWorld;
using Verse;

namespace RationalRomance_Code;

[HarmonyPatch(typeof(PawnGenerator), nameof(PawnGenerator.GenerateTraits), null)]
public static class PawnGenerator_GenerateTraits
{
    // CHANGE: Add orientation trait after other traits are selected.
    public static void Postfix(Pawn pawn)
    {
        //Removes existing sexualities if existed.
        Trait tempTrait = null;
        foreach (var trait in pawn.story.traits.allTraits)
        {
            tempTrait = trait;
            if (tempTrait.Label.Equals("Asexual"))
            {
                break;
            }

            if (tempTrait.Label.Equals("Bisexual"))
            {
                break;
            }

            if (tempTrait.Label.Equals("Gay"))
            {
                break;
            }

            if (tempTrait.Label.Equals("Straight"))
            {
                break;
            }

            tempTrait = null;
        }

        if (tempTrait != null)
        {
            //TODO: If another trait was rerolled, find way to add another random trait.
            pawn.story.traits.allTraits.Remove(tempTrait);
            pawn.skills?.Notify_SkillDisablesChanged();

            if (!pawn.Dead && pawn.RaceProps.Humanlike)
            {
                pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
            }

            //TODO: Get better system for generating "Random" traits
            if (!pawn.story.traits.HasTrait(TraitDefOf.Nudist))
            {
                pawn.story.traits.GainTrait(new Trait(TraitDefOf.Nudist));
            }
            else if (!pawn.story.traits.HasTrait(TraitDefOf.Psychopath))
            {
                pawn.story.traits.GainTrait(new Trait(TraitDefOf.Psychopath));
            }
            else
            {
                pawn.story.traits.GainTrait(new Trait(TraitDefOf.Kind));
            }
        }


        if (pawn.story.traits.HasTrait(TraitDefOf.Asexual) || pawn.story.traits.HasTrait(TraitDefOf.Bisexual) ||
            pawn.story.traits.HasTrait(TraitDefOf.Gay) || pawn.story.traits.HasTrait(RRRTraitDefOf.Straight))
        {
            return;
        }

        ExtraTraits.AssignOrientation(pawn);
    }
}