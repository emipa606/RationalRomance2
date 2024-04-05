﻿using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RationalRomance_Code;

[HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), nameof(InteractionWorker_RomanceAttempt.RandomSelectionWeight),
    null)]
public static class InteractionWorker_RomanceAttempt_RandomSelectionWeight
{
    // CHANGE: Updated with new orientation options and traits.
    // CHANGE: Women are no less likely than men to initiate romance, when from colonist or glitterworld cultures.
    // CHANGE: Women are more forward when from tribal or imperial; men when from medieval or urbworld cultures. 
    // CHANGE: Pawn in mental break or who is already lover of initiator can't be targeted.
    // CHANGE: Pawn can't perform romance attempt if recently rebuffed.
    // CHANGE: Pawn can't target others or be targeted if current lover is good enough.
    // CHANGE: Allowed for polyamory.
    // CHANGE: Misandrists and Misogynists are less likely to successfully romance the other gender.
    public static bool Prefix(Pawn initiator, Pawn recipient, ref float __result)
    {
        if (!initiator.story.traits.HasTrait(TraitDefOf.Asexual) &&
            !initiator.story.traits.HasTrait(TraitDefOf.Bisexual) &&
            !initiator.story.traits.HasTrait(TraitDefOf.Gay) &&
            !initiator.story.traits.HasTrait(RRRTraitDefOf.Straight))
        {
            ExtraTraits.AssignOrientation(initiator);
        }

        if (!recipient.story.traits.HasTrait(TraitDefOf.Asexual) &&
            !recipient.story.traits.HasTrait(TraitDefOf.Bisexual) &&
            !recipient.story.traits.HasTrait(TraitDefOf.Gay) &&
            !recipient.story.traits.HasTrait(RRRTraitDefOf.Straight))
        {
            ExtraTraits.AssignOrientation(recipient);
        }

        if (recipient.InMentalState || LovePartnerRelationUtility.LovePartnerRelationExists(initiator, recipient))
        {
            __result = 0f;
            return false;
        }

        if (initiator.needs.mood.thoughts.memories.NumMemoriesOfDef(ThoughtDefOf.RebuffedMyRomanceAttempt) > 0)
        {
            __result = 0f;
            return false;
        }

        var initiator_partner = LovePartnerRelationUtility.ExistingMostLikedLovePartner(initiator, false);
        if (initiator_partner != null && initiator.relations.OpinionOf(initiator_partner) >= 33)
        {
            if (!initiator.story.traits.HasTrait(RRRTraitDefOf.Polyamorous) &&
                !initiator.story.traits.HasTrait(RRRTraitDefOf.Philanderer))
            {
                __result = 0f;
                return false;
            }
        }

        var recipient_partner = LovePartnerRelationUtility.ExistingMostLikedLovePartner(recipient, false);
        if (recipient_partner != null && recipient.relations.OpinionOf(recipient_partner) >= 33)
        {
            if (!recipient.story.traits.HasTrait(RRRTraitDefOf.Polyamorous) &&
                !recipient.story.traits.HasTrait(RRRTraitDefOf.Philanderer))
            {
                __result = 0f;
                return false;
            }
        }

        var romanceChance = initiator.relations.SecondaryRomanceChanceFactor(recipient);
        if (romanceChance < 0.25f)
        {
            __result = 0f;
            return false;
        }

        var opinionOfTarget = initiator.relations.OpinionOf(recipient);
        if (opinionOfTarget < 5)
        {
            __result = 0f;
            return false;
        }

        if (recipient.relations.OpinionOf(initiator) < 5)
        {
            __result = 0f;
            return false;
        }

        var cheatChance = 1f;
        var pawn = LovePartnerRelationUtility.ExistingMostLikedLovePartner(initiator, false);
        if (pawn != null)
        {
            float opinionOfPartner = initiator.relations.OpinionOf(pawn);
            if (initiator.story.traits.HasTrait(RRRTraitDefOf.Polyamorous))
            {
                if (SexualityUtilities.HasNonPolyPartner(initiator))
                {
                    if (initiator.story.traits.HasTrait(RRRTraitDefOf.Philanderer))
                    {
                        cheatChance = initiator.Map == pawn.Map
                            ? Mathf.InverseLerp(75f, 5f, opinionOfPartner)
                            : Mathf.InverseLerp(100f, 50f, opinionOfPartner);
                    }
                    else
                    {
                        cheatChance =
                            Mathf.InverseLerp(
                                25f + (25 * Controller.Settings.polyamorousLoverAttachmentCoefficient),
                                -50f + (25 * Controller.Settings.polyamorousNewPartnerChanceCoefficient),
                                opinionOfPartner);
                    }

                    if (initiator.story.traits.HasTrait(RRRTraitDefOf.Faithful))
                    {
                        cheatChance = 0f;
                    }
                }
                else
                {
                    cheatChance = 1f;
                }
            }
            else
            {
                if (initiator.story.traits.HasTrait(RRRTraitDefOf.Philanderer))
                {
                    cheatChance = initiator.Map == pawn.Map
                        ? Mathf.InverseLerp(75f, 5f, opinionOfPartner)
                        : Mathf.InverseLerp(100f, 50f, opinionOfPartner);
                }
                else
                {
                    cheatChance = Mathf.InverseLerp(50f, -50f, opinionOfPartner);
                }

                if (initiator.story.traits.HasTrait(RRRTraitDefOf.Faithful))
                {
                    cheatChance = 0f;
                }
            }
        }

        float genderAggressiveness;
        var backgroundCulture = SexualityUtilities.GetAdultCulturalAdjective(initiator);
        if (backgroundCulture == "Urbworld")
        {
            genderAggressiveness = initiator.gender != Gender.Male ? 0.5f : 1f;
        }
        else if (backgroundCulture == "Imperial")
        {
            genderAggressiveness = initiator.gender != Gender.Female ? 0.5f : 1f;
        }
        else if (backgroundCulture == "Tribal")
        {
            genderAggressiveness = initiator.gender != Gender.Female ? 0.125f : 1f;
        }
        else if (backgroundCulture == "Medieval")
        {
            genderAggressiveness = initiator.gender != Gender.Male ? 0.125f : 1f;
        }
        else
        {
            genderAggressiveness = 1f;
        }

        var romanceChancePercent = Mathf.InverseLerp(0.25f, 1f, romanceChance);
        var opinionPercent = Mathf.InverseLerp(5f, 100f, opinionOfTarget);
        var orientationMatch = 1f;
        if (initiator.story.traits.HasTrait(TraitDefOf.Asexual) &&
            recipient.story.traits.HasTrait(TraitDefOf.Asexual))
        {
            //Ace people may not be Aro. They can have non-sexual relationships as well.
            //orientationMatch = 1f;
        }
        else if (initiator.story.traits.HasTrait(TraitDefOf.Asexual) ||
                 recipient.story.traits.HasTrait(TraitDefOf.Asexual))
        {
            orientationMatch = 0.15f;
        }
        else if (initiator.gender != recipient.gender)
        {
            if ((initiator.story.traits.HasTrait(TraitDefOf.Asexual) ||
                 initiator.story.traits.HasTrait(TraitDefOf.Gay)) &&
                (recipient.story.traits.HasTrait(TraitDefOf.Gay) ||
                 recipient.story.traits.HasTrait(TraitDefOf.Asexual)))
            {
                //If neigther of them want to be in a relationship with the other. Set it to 0.01
                orientationMatch = 0.01f;
            }

            if (initiator.story.traits.HasTrait(TraitDefOf.Gay) || recipient.story.traits.HasTrait(TraitDefOf.Gay))
            {
                orientationMatch = 0.15f;
                //Atleast one of them is not into it
            }
        }
        else if (initiator.gender == recipient.gender)
        {
            genderAggressiveness = 1f;
            if ((initiator.story.traits.HasTrait(TraitDefOf.Asexual) ||
                 initiator.story.traits.HasTrait(RRRTraitDefOf.Straight)) &&
                (recipient.story.traits.HasTrait(RRRTraitDefOf.Straight) ||
                 recipient.story.traits.HasTrait(TraitDefOf.Asexual)))
            {
                //"Woah dude! We're straight. Why are we hitting on eachother?"
                orientationMatch = 0.01f;
            }

            if (initiator.story.traits.HasTrait(RRRTraitDefOf.Straight) ||
                recipient.story.traits.HasTrait(RRRTraitDefOf.Straight))
            {
                orientationMatch = 0.15f;
                //Atleast one of them is gay. Not a healthy relationship, but what can you do.
            }
        }

        //If a misogynist tries to romance a woman, it most likely isn't going to go well. It can, but not as well as someone who respects women.
        float bigotRomanceChancePercent = 1;
        if (initiator.story.traits.HasTrait(TraitDefOf.DislikesMen) && recipient.gender == Gender.Male)
        {
            bigotRomanceChancePercent *= 0.6f;
        }
        else if (initiator.story.traits.HasTrait(TraitDefOf.DislikesWomen) && recipient.gender == Gender.Female)
        {
            bigotRomanceChancePercent *= 0.6f;
        }

        if (recipient.story.traits.HasTrait(TraitDefOf.DislikesMen) && initiator.gender == Gender.Male)
        {
            bigotRomanceChancePercent *= 0.6f;
        }
        else if (recipient.story.traits.HasTrait(TraitDefOf.DislikesMen) && initiator.gender == Gender.Male)
        {
            bigotRomanceChancePercent *= 0.6f;
        }


        __result = 1.15f * genderAggressiveness * romanceChancePercent * opinionPercent * cheatChance *
                   orientationMatch * bigotRomanceChancePercent;
        return false;
    }
}