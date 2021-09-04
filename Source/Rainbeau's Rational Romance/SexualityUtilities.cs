﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RationalRomance_Code
{
    public static class SexualityUtilities
    {
        public static void updateMetamours(Pawn pawn)
        {
            var partners = from r in pawn.relations.PotentiallyRelatedPawns
                where LovePartnerRelationUtility.LovePartnerRelationExists(pawn, r)
                select r;
            foreach (var p in partners)
            {
                updateMetamours(pawn, p);
            }
        }

        public static void updateMetamours(Pawn pawn, Pawn secondPawn)
        {
            var partners = from r in pawn.relations.PotentiallyRelatedPawns
                where LovePartnerRelationUtility.LovePartnerRelationExists(secondPawn, r)
                select r;
            var metamours = getAllLoverPawnsFirstRemoved(pawn);
            foreach (var meta in partners)
            {
                if (!metamours.Contains(meta))
                {
                    if (pawn.relations.GetDirectRelation(RRRRelationsDefsOf.Metamour, meta) == null)
                    {
                        continue;
                    }

                    pawn.relations.RemoveDirectRelation(RRRRelationsDefsOf.Metamour, meta);
                    meta.relations.RemoveDirectRelation(RRRRelationsDefsOf.Metamour, pawn);
                }
                else
                {
                    if (pawn.relations.GetDirectRelation(RRRRelationsDefsOf.Metamour, meta) != null)
                    {
                        continue;
                    }

                    pawn.relations.AddDirectRelation(RRRRelationsDefsOf.Metamour, meta);
                    meta.relations.AddDirectRelation(RRRRelationsDefsOf.Metamour, pawn);
                }
            }
        }

        public static IEnumerable<Pawn> getAllLoverPawnsFirstRemoved(Pawn p)
        {
            //Log.Message("testfor " + p.Name);
            var list = new List<Pawn>();
            var loveTree = from r in p.relations.PotentiallyRelatedPawns
                where LovePartnerRelationUtility.LovePartnerRelationExists(p, r)
                select r;
            foreach (var newPawn in loveTree)
            {
                if (!list.Contains(newPawn))
                {
                    list.Add(newPawn);
                }

                //Log.Message("--Hello " + newPawn.Name);
                var loveTree2 = from r in newPawn.relations.PotentiallyRelatedPawns
                    where LovePartnerRelationUtility.LovePartnerRelationExists(newPawn, r)
                    select r;
                foreach (var secondPawn in loveTree2)
                {
                    if (secondPawn == p)
                    {
                        continue;
                    }

                    //Log.Message("-------"+secondPawn.Name);
                    if (!list.Contains(secondPawn))
                    {
                        //if(p.relations.GetDirectRelation(PawnRelationDefOf.Spouse, newPawn) != null || p.relations.GetDirectRelation(PawnRelationDefOf.Fiance, newPawn) != null)
                        //Make sure the two pawns are married first in order to be a meta
                        //	if (p.relations.GetDirectRelation(RRRRelationsDefsOf.Metamour, secondPawn) == null &&
                        //	p.relations.GetDirectRelation(PawnRelationDefOf.Lover, secondPawn) == null &&
                        //	p.relations.GetDirectRelation(PawnRelationDefOf.Spouse, secondPawn) == null &&
                        //	p.relations.GetDirectRelation(PawnRelationDefOf.Fiance, secondPawn) == null 
                        //	)
                        //	{
                        //	//Pawn is a meta, but not given a relationship tag
                        //	p.relations.AddDirectRelation(RRRRelationsDefsOf.Metamour, secondPawn);
                        //	}
                        list.Add(secondPawn);
                    }
                }
            }

            return list;
        }

        public static Pawn FindAttractivePawn(Pawn p1)
        {
            Pawn result;
            if (p1.story.traits.HasTrait(TraitDefOf.Asexual))
            {
                result = null;
            }
            else
            {
                IEnumerable<Pawn> enumerable = p1.Map.mapPawns.FreeColonistsSpawned;
                enumerable = enumerable.Except(from p in enumerable
                    where (p.story.traits.HasTrait(TraitDefOf.Asexual) || !p.RaceProps.Humanlike ||
                           p.story.traits.HasTrait(TraitDefOf.Gay) && p.gender != p1.gender ||
                           p.story.traits.HasTrait(RRRTraitDefOf.Straight) && p.gender == p1.gender) && Rand.Value < 0.8
                    select p);
                enumerable = from p in enumerable
                    where p.Map == p1.Map && p.Faction == p1.Faction
                    select p;
                var pawns = enumerable as Pawn[] ?? enumerable.ToArray();
                if (!pawns.Any())
                {
                    result = null;
                }
                else
                {
                    pawns.TryRandomElementByWeight(
                        x => p1.relations.SecondaryRomanceChanceFactor(x) *
                             p1.relations.SecondaryRomanceChanceFactor(x), out var pawn);
                    if (pawn == null)
                    {
                        result = null;
                    }
                    else if (pawn == p1)
                    {
                        result = null;
                    }
                    else if (LovePartnerRelationUtility.HasAnyLovePartner(pawn) && Rand.Value < 0.85f)
                    {
                        result = null;
                    }
                    else if (pawn == LovePartnerRelationUtility.ExistingLovePartner(p1))
                    {
                        result = null;
                    }
                    else if (p1.relations.SecondaryRomanceChanceFactor(pawn) < 0.05)
                    {
                        result = null;
                    }
                    else
                    {
                        result = pawn;
                    }
                }
            }

            return result;
        }

        public static Building_Bed FindHookupBed(Pawn p1, Pawn p2)
        {
            Building_Bed result;
            if (p1.ownership.OwnedBed != null)
            {
                if (p1.ownership.OwnedBed.OwnersForReading.Capacity > 1)
                {
                    result = p1.ownership.OwnedBed;
                    return result;
                }
            }

            if (p2.ownership.OwnedBed != null)
            {
                if (p2.ownership.OwnedBed.OwnersForReading.Capacity <= 1)
                {
                    return null;
                }

                result = p2.ownership.OwnedBed;
                return result;
            }

            foreach (var current in RestUtility.AllBedDefBestToWorst)
            {
                if (!RestUtility.CanUseBedEver(p1, current))
                {
                    continue;
                }

                var building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(p1.Position, p1.Map,
                    ThingRequest.ForDef(current),
                    PathEndMode.OnCell, TraverseParms.For(p1), 9999f, _ => true,
                    null, 0, -1, false, RegionType.Normal);
                if (building_Bed == null)
                {
                    building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(p1.Position, p1.Map,
                        ThingRequest.ForDef(current),
                        PathEndMode.OnCell, TraverseParms.For(p1), 9999f, _ => true);
                }

                if (building_Bed == null)
                {
                    continue;
                }

                if (building_Bed.SleepingSlotsCount <= 1)
                {
                    continue;
                }

                result = building_Bed;
                return result;
            }

            return null;
        }

        public static bool HasNonPolyPartner(Pawn p)
        {
            foreach (var current in p.relations.DirectRelations)
            {
                if (current.def != PawnRelationDefOf.Lover && current.def != PawnRelationDefOf.Fiance &&
                    current.def != PawnRelationDefOf.Spouse)
                {
                    continue;
                }

                if (current.otherPawn.story.traits.HasTrait(RRRTraitDefOf.Polyamorous))
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public static bool IsHookupAppealing(Pawn pSubject, Pawn pObject)
        {
            bool result;
            if (PawnUtility.WillSoonHaveBasicNeed(pSubject))
            {
                result = false;
            }
            else
            {
                var human_Contact_Need = pSubject.needs.TryGetNeed<Human_Contact_Need>();
                var num = 0f;
                num += pSubject.relations.SecondaryRomanceChanceFactor(pObject);
                if (human_Contact_Need != null)
                {
                    num /= Math.Min(0.1f, human_Contact_Need.CurLevel);
                }
                else
                {
                    num /= 1.5f;
                }

                num *= Mathf.InverseLerp(-100f, 0f, pSubject.relations.OpinionOf(pObject));
                result = Rand.Range(0.05f, 1f) < num;
            }

            return result;
        }

        public static bool WillPawnTryHookup(Pawn p1)
        {
            bool result;
            if (p1.story.traits.HasTrait(TraitDefOf.Asexual))
            {
                result = false;
            }
            else
            {
                var pawn = LovePartnerRelationUtility.ExistingMostLikedLovePartner(p1, false);
                if (pawn != null)
                {
                    float num = p1.relations.OpinionOf(pawn);
                    float num2;
                    if (p1.story.traits.HasTrait(RRRTraitDefOf.Philanderer))
                    {
                        num2 = p1.Map == pawn.Map
                            ? Mathf.InverseLerp(70f, 15f, num)
                            : Mathf.InverseLerp(100f, 50f, num);
                    }
                    else
                    {
                        num2 = Mathf.InverseLerp(30f, -80f, num);
                    }

                    if (p1.story.traits.HasTrait(RRRTraitDefOf.Faithful))
                    {
                        num2 = 0f;
                    }

                    num2 /= 2f;
                    result = Rand.Range(0f, 1f) < num2;
                }
                else
                {
                    result = true;
                }
            }

            return result;
        }

        public static string GetAdultCulturalAdjective(Pawn p)
        {
            var result = "Colonial";
            if (p.story.adulthood == null)
            {
                return result;
            }

            if (p.story.adulthood.spawnCategories.Contains("Tribal"))
            {
                result = "Tribal";
            }
            else if (p.story.adulthood.title.Contains("medieval") ||
                     p.story.adulthood.baseDesc.IndexOf("Medieval", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     p.story.adulthood.baseDesc.IndexOf("Village", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                result = "Medieval";
            }
            else if (p.story.adulthood.title.Contains("glitterworld") ||
                     p.story.adulthood.baseDesc.IndexOf("Glitterworld", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (p.story.adulthood.title != "adventurer")
                {
                    result = "Glitterworld";
                }
            }
            else if (p.story.adulthood.title.Contains("urbworld") || p.story.adulthood.title.Contains("vatgrown") ||
                     p.story.adulthood.baseDesc.IndexOf("Urbworld", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     p.story.adulthood.baseDesc.IndexOf("Urbworld", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                result = "Urbworld";
            }
            else if (p.story.adulthood.title.Contains("midworld") ||
                     p.story.adulthood.baseDesc.IndexOf("Midworld", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                result = "Midworld";
            }
            else if (p.story.adulthood.baseDesc.IndexOf("Tribe", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                result = "Tribal";
            }
            else if (p.story.adulthood.title.Contains("imperial") ||
                     p.story.adulthood.baseDesc.IndexOf("Imperial", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                result = "Imperial";
            }

            return result;
        }
    }
}