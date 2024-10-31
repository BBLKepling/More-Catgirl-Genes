using HarmonyLib;
using Verse;

namespace More_Catgirl_Genes
{
    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        static HarmonyInit()
        {
            Harmony harmonyInstance = new Harmony("BBLKepling.MoreCatgirlGenes");
            harmonyInstance.PatchAll();
        }
    }
}
