using System;
using UnityEngine;
using Verse;

namespace RationalRomance_Code;

public class Settings : ModSettings
{
    public float alienLoveChance = 33f;
    public float asexualChance = 10f;
    public float BigotCorrectionRate = 50f;
    public float bisexualChance = 50f;
    public float dateRate = 100f;
    public float gayChance = 20f;
    public bool generateSexualities = true;

    public float hookupMaxRange = 100;
    public float hookupRate = 100f;
    public bool need_contact;

    public bool polyamorousDebuff = true;
    public float polyamorousLoverAttachmentCoefficient = 1f;

    public float polyamorousNewPartnerChanceCoefficient = 1f;
    public float polyChance = 15f;

    public float secondaryLovinChanceCoefficient = 1f;
    public float straightChance = 20f;

    public void DoWindowContents(Rect canvas)
    {
        var list = new Listing_Standard
        {
            ColumnWidth = (int)(canvas.width / 2.1)
        };
        list.Begin(canvas);
        list.Gap(2);
        Text.Font = GameFont.Tiny;
        list.Label("RRR.Overview".Translate());
        Text.Font = GameFont.Small;
        list.Gap();
        list.Label("RRR.StraightChance".Translate() + "  " + (int)straightChance + "%");
        straightChance = list.Slider(straightChance, 0f, 100.99f);
        if (straightChance > 100.99f - bisexualChance - gayChance)
        {
            straightChance = 100.99f - bisexualChance - gayChance;
        }

        list.Gap();
        list.Label("RRR.BisexualChance".Translate() + "  " + (int)bisexualChance + "%");
        bisexualChance = list.Slider(bisexualChance, 0f, 100.99f);
        if (bisexualChance > 100.99f - straightChance - gayChance)
        {
            bisexualChance = 100.99f - straightChance - gayChance;
        }

        list.Gap();
        list.Label("RRR.GayChance".Translate() + "  " + (int)gayChance + "%");
        gayChance = list.Slider(gayChance, 0f, 100.99f);
        if (gayChance > 100.99f - straightChance - bisexualChance)
        {
            gayChance = 100.99f - straightChance - bisexualChance;
        }

        list.Gap();
        asexualChance = 100 - (int)straightChance - (int)bisexualChance - (int)gayChance;
        list.Label("RRR.AsexualChance".Translate() + "  " + asexualChance + "%");
        list.Gap(40);
        list.GapLine();
        list.Label("RRR.PolyamoryChance".Translate() + "  " + (int)polyChance + "%", -1f,
            "RRR.PolyamoryChanceTip".Translate());
        polyChance = list.Slider(polyChance, 0f, 100.99f);
        list.Gap(2);
        Text.Font = GameFont.Tiny;
        list.Label("RRR.DateRate".Translate() + "  " + (int)dateRate + "%");
        dateRate = list.Slider(dateRate, 0f, 200.99f);
        list.Gap(2);
        list.Label("RRR.HookupRate".Translate() + "  " + (int)hookupRate + "%");
        hookupRate = list.Slider(hookupRate, 0f, 200.99f);
        list.Gap(2);
        list.Label("RRR.BigotCorrectionRate".Translate() + "  " + (int)BigotCorrectionRate + "%", -1f,
            "RRR.BigotCorrectionRateTip".Translate());
        BigotCorrectionRate = list.Slider(BigotCorrectionRate, 0f, 100.99f);
        list.Gap(2);
        list.Label("RRR.AlienLoveChance".Translate() + "  " + (int)alienLoveChance + "%", -1f,
            "RRR.AlienLoveChanceTip".Translate());
        alienLoveChance = list.Slider(alienLoveChance, 0f, 100.99f);
        list.Gap(2);
        list.CheckboxLabeled("RRR.PolyamorousDebuff".Translate(), ref polyamorousDebuff,
            "RRR.PolyamorousDebuffTip".Translate());
        list.Gap(2);
        list.CheckboxLabeled("RRR.GenerateSexualities".Translate(), ref generateSexualities,
            "RRR.GenerateSexualitiesTip".Translate());
        list.Gap(2);
        list.CheckboxLabeled("RRR.NeedContact".Translate(), ref need_contact, "RRR.NeedContact".Translate());


        list.Gap(2);
        list.Label(
            "RRR.PolyamorousNewPartnerChance".Translate() + "  " +
            Math.Round(polyamorousNewPartnerChanceCoefficient, 1) + ".", -1f,
            "RRR.PolyamorousNewPartnerChanceTip".Translate());
        polyamorousNewPartnerChanceCoefficient = list.Slider(polyamorousNewPartnerChanceCoefficient, -2.99f, 2.99f);
        list.Gap(2);
        list.Label(
            "RRR.polyamorousLoverAttachment".Translate() + "  " +
            Math.Round(polyamorousLoverAttachmentCoefficient, 1) + ".", -1f,
            "RRR.polyamorousLoverAttachmentTip".Translate());
        polyamorousLoverAttachmentCoefficient = list.Slider(polyamorousLoverAttachmentCoefficient, 0, 2.99f);
        list.Gap(2);
        list.Label(
            "RRR.secondLovinCoefficent".Translate() + "  " + Math.Round(secondaryLovinChanceCoefficient, 2) + ".",
            -1f, "RRR.secondLovinCoefficentTip".Translate());
        secondaryLovinChanceCoefficient = list.Slider(secondaryLovinChanceCoefficient, 0, 5.99f);
        list.Gap(2);
        list.Label("RRR.hookupMaxRange".Translate() + "  " + (int)hookupMaxRange + ".", -1f,
            "RRR.hookupMaxRangeTip".Translate());
        hookupMaxRange = list.Slider(hookupMaxRange, 0, 300.99f);


        list.Gap(100);
        if (list.ButtonText("Reset"))
        {
            asexualChance = 10f;
            bisexualChance = 50f;
            gayChance = 20f;
            straightChance = 20f;
            polyChance = 15f;
            alienLoveChance = 33f;
            dateRate = 100f;
            hookupRate = 100f;
            BigotCorrectionRate = 50f;
            polyamorousDebuff = true;
            generateSexualities = true;
            need_contact = false;
            polyamorousLoverAttachmentCoefficient = 1;
            polyamorousNewPartnerChanceCoefficient = 1;
            secondaryLovinChanceCoefficient = 1;
            hookupMaxRange = 100;
        }

        if (Controller.currentVersion != null)
        {
            list.Gap();
            GUI.contentColor = Color.gray;
            list.Label("RRR.CurrentModVersion".Translate(Controller.currentVersion));
            GUI.contentColor = Color.white;
        }

        list.End();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref asexualChance, "asexualChance", 10.0f);
        Scribe_Values.Look(ref bisexualChance, "bisexualChance", 50.0f);
        Scribe_Values.Look(ref gayChance, "gayChance", 20.0f);
        Scribe_Values.Look(ref straightChance, "straightChance", 20.0f);
        Scribe_Values.Look(ref polyChance, "polyChance");
        Scribe_Values.Look(ref alienLoveChance, "alienLoveChance", 33.0f);
        Scribe_Values.Look(ref dateRate, "dateRate", 100.0f);
        Scribe_Values.Look(ref hookupRate, "hookupRate", 100.0f);
        Scribe_Values.Look(ref BigotCorrectionRate, "BigotCorrectionRate", 50.0f);
        Scribe_Values.Look(ref polyamorousDebuff, "polyamorousDebuff", true);
        Scribe_Values.Look(ref generateSexualities, "generateSexualities", true);
        Scribe_Values.Look(ref need_contact, "needcontact", true);
        Scribe_Values.Look(ref polyamorousLoverAttachmentCoefficient, "polyamorousLoverAttachmentCoefficient", 1);
        Scribe_Values.Look(ref polyamorousNewPartnerChanceCoefficient, "polyamorousNewPartnerChanceCoefficient", 1);
        Scribe_Values.Look(ref secondaryLovinChanceCoefficient, "secondaryLovinChanceCoefficient", 1);
        Scribe_Values.Look(ref hookupMaxRange, "hookupMaxRange", 100);
    }
}