using RimWorld;
using Verse;

namespace RationalRomance_Code;

public static class ExtraTraits
{
    public static bool hasSexualTrait(Pawn pawn)
    {
        if (pawn.story.traits.GetTrait(TraitDefOf.Bisexual) != null)
        {
            return true;
        }

        if (pawn.story.traits.GetTrait(RRRTraitDefOf.Straight) != null)
        {
            return true;
        }

        if (pawn.story.traits.GetTrait(TraitDefOf.Gay) != null)
        {
            return true;
        }

        if (pawn.story.traits.GetTrait(TraitDefOf.Asexual) != null)
        {
            return true;
        }

        return false;
    }


    public static void AssignOrientation(Pawn pawn)
    {
        var orientation = Rand.Value;
        if (pawn.gender == Gender.None)
        {
            return;
        }

        if (hasSexualTrait(pawn))
        {
            return;
        }

        if (Controller.Settings.generateSexualities)
        {
            if (pawn.kindDef.race.defName.ToLower().Contains("droid") && !AndroidsCompatibility.IsAndroid(pawn))
            {
                pawn.story.traits.GainTrait(new Trait(TraitDefOf.Asexual));
                return;
            }

            var likesOwn = false;
            var likesOther = false;

            if (orientation < Controller.Settings.asexualChance / 100 && Controller.Settings.asexualChance >= 1)
            {
                if (LovePartnerRelationUtility.HasAnyLovePartnerOfTheOppositeGender(pawn) ||
                    LovePartnerRelationUtility.HasAnyExLovePartnerOfTheOppositeGender(pawn))
                {
                    likesOther = true;
                }

                if (LovePartnerRelationUtility.HasAnyLovePartnerOfTheSameGender(pawn) ||
                    LovePartnerRelationUtility.HasAnyExLovePartnerOfTheSameGender(pawn))
                {
                    likesOwn = true;
                }

                if (pawn.story.traits.HasTrait(RRRTraitDefOf.Philanderer))
                {
                    likesOther = true;
                    likesOwn = true;
                }
                else
                {
                    pawn.story.traits.GainTrait(new Trait(TraitDefOf.Asexual));
                    return;
                }
            }

            if (!hasSexualTrait(pawn))
            {
                if (LovePartnerRelationUtility.HasAnyLovePartnerOfTheOppositeGender(pawn) ||
                    LovePartnerRelationUtility.HasAnyExLovePartnerOfTheOppositeGender(pawn))
                {
                    likesOther = true;
                }

                if (LovePartnerRelationUtility.HasAnyLovePartnerOfTheSameGender(pawn) ||
                    LovePartnerRelationUtility.HasAnyExLovePartnerOfTheSameGender(pawn))
                {
                    likesOwn = true;
                }

                var hatesOwnGender = false;
                var hatesOtherGender = false;
                if (pawn.story.traits.HasTrait(TraitDefOf.DislikesMen))
                {
                    if (pawn.gender == Gender.Male)
                    {
                        hatesOwnGender = true;
                    }
                    else
                    {
                        hatesOtherGender = true;
                    }
                }

                if (pawn.story.traits.HasTrait(TraitDefOf.DislikesWomen))
                {
                    if (pawn.gender == Gender.Female)
                    {
                        hatesOwnGender = true;
                    }
                    else
                    {
                        hatesOtherGender = true;
                    }
                }

                else if (orientation <
                         (Controller.Settings.asexualChance + Controller.Settings.bisexualChance) / 100 &&
                         Controller.Settings.bisexualChance >= 1)
                {
                    likesOther = true;
                    likesOwn = true;
                    //bi
                }
                else if (orientation < (Controller.Settings.asexualChance + Controller.Settings.bisexualChance +
                                        Controller.Settings.gayChance) / 100 && Controller.Settings.gayChance >= 1)
                {
                    //Makes it so misogynists and misandrists are less likely to have to romance the "hated sex".
                    if (hatesOwnGender && Rand.Value < Controller.Settings.BigotCorrectionRate)
                    {
                        if (Rand.Value < 100F - Controller.Settings.straightChance)
                        {
                            likesOther = true;
                            //Likes own too
                        }
                        else
                        {
                            likesOther = true;
                            likesOwn = false;
                        }
                    }
                    else
                    {
                        likesOwn = true;
                    }
                }
                else
                {
                    //Makes it so misogynists and misandrists are less likely to have to romance the "hated sex".
                    if (hatesOtherGender && Rand.Value < Controller.Settings.BigotCorrectionRate)
                    {
                        if (Rand.Value < 100F - Controller.Settings.gayChance)
                        {
                            likesOwn = true;
                            //Likes other too.
                        }
                        else
                        {
                            likesOwn = true;
                            likesOther = false;
                        }
                    }
                    else
                    {
                        likesOther = true;
                    }
                }

                switch (likesOther)
                {
                    case true when likesOwn:
                        pawn.story.traits.GainTrait(new Trait(TraitDefOf.Bisexual));
                        break;
                    case true:
                        pawn.story.traits.GainTrait(new Trait(RRRTraitDefOf.Straight));
                        break;
                    default:
                    {
                        pawn.story.traits.GainTrait(likesOwn
                            ? new Trait(TraitDefOf.Gay)
                            : new Trait(TraitDefOf.Asexual));

                        break;
                    }
                }
            }
        }

        if (pawn.story.traits.HasTrait(TraitDefOf.Asexual) || pawn.story.traits.HasTrait(RRRTraitDefOf.Polyamorous))
        {
            return;
        }

        if (Rand.Value < Controller.Settings.polyChance / 100 && Controller.Settings.polyChance > 1)
        {
            pawn.story.traits.GainTrait(new Trait(RRRTraitDefOf.Polyamorous));
        }
    }
}