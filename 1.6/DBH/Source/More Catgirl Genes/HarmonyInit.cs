using DubsBadHygiene;
using HarmonyLib;
using RimWorld;
using Verse;

namespace More_Catgirl_Genes
{
    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        static HarmonyInit()
        {
            Harmony harmonyInstance = new Harmony("BBLKepling.MoreCatgirlGenes");
            harmonyInstance.Patch(typeof(JobGiver_UseToilet).GetMethod("TryGiveJob"), prefix: new HarmonyMethod(typeof(JobGiver_UseToilet_Patch).GetMethod("Prefix")));
            if (ModLister.HasActiveModWithName("Dubs Bad Hygiene")) harmonyInstance.Patch(typeof(JobGiver_HaveWash).GetMethod("TryGiveJob"), prefix: new HarmonyMethod(typeof(JobGiver_HaveWash_Patch).GetMethod("PrefixReg")));
            if (ModLister.HasActiveModWithName("Dubs Bad Hygiene Lite")) harmonyInstance.Patch(typeof(JobGiver_HaveWash).GetMethod("TryGiveJob"), prefix: new HarmonyMethod(typeof(JobGiver_HaveWash_Patch).GetMethod("PrefixLite")));
            if (ModLister.HasActiveModWithName("Carry A Barf Bag"))
            {
                harmonyInstance.Patch(AccessTools.Method(typeof(JobDriver_Vomit), "MakeNewToils"), prefix: new HarmonyMethod(typeof(JobDriver_Vomit_Patch).GetMethod("PrefixBarfBag")), postfix: new HarmonyMethod(typeof(JobDriver_Vomit_Patch).GetMethod("PostfixBarfBag")));
            }
            else harmonyInstance.Patch(AccessTools.Method(typeof(JobDriver_Vomit), "MakeNewToils"), postfix: new HarmonyMethod(typeof(JobDriver_Vomit_Patch).GetMethod("PostfixVanilla")));
        }
    }
}
