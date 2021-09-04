using System;
using Verse;

namespace RationalRomance_Code
{
    /// <summary>
    ///     Helper class for ChJees's Androids mod.
    /// </summary>
    [StaticConstructorOnStartup]
    public static class AndroidsCompatibility
    {
        public static Type androidCompatType;
        public static readonly string typeName = "Androids.SexualizeAndroidRJW";
        private static readonly bool foundType;

        static AndroidsCompatibility()
        {
            try
            {
                androidCompatType = Type.GetType(typeName);
                foundType = true;
                //Log.Message("Found Type: Androids.SexualizeAndroidRJW");
            }
            catch
            {
                foundType = false;
                //Log.Message("Did NOT find Type: Androids.SexualizeAndroidRJW");
            }
        }

        public static bool IsAndroid(ThingDef def)
        {
            if (def == null || !foundType)
            {
                return false;
            }

            return def.modExtensions != null &&
                   def.modExtensions.Any(extension => extension.GetType().FullName == typeName);
        }

        public static bool IsAndroid(Thing thing)
        {
            return IsAndroid(thing.def);
        }
    }
}