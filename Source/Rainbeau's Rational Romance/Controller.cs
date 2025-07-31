using System.Reflection;
using HarmonyLib;
using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace RationalRomance_Code;

public class Controller : Mod
{
    public static Settings Settings;
    public static string currentVersion;
    public static readonly FieldInfo MelaninFieldInfo = AccessTools.Field(typeof(Pawn_StoryTracker), "melanin");

    public Controller(ModContentPack content) : base(content)
    {
        //HarmonyInstance harmony = HarmonyInstance.Create("net.rainbeau.rimworld.mod.rationalromance");
        var harmony = new Harmony("net.rainbeau.rimworld.mod.rationalromance");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        Settings = GetSettings<Settings>();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
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