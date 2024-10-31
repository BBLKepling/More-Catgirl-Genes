using DubsBadHygiene;
using HarmonyLib;
using Verse.AI;
using Verse;

namespace More_Catgirl_Genes
{
    [HarmonyPatch(typeof(JobGiver_HaveWash), nameof(JobGiver_HaveWash.TryGiveJob))]
    class JobGiver_HaveWash_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(ref Job __result, Pawn pawn, JobGiver_HaveWash __instance)
        {
            if (!pawn.genes.HasActiveGene(InternalDefOf.BBLK_Tongue_Cleaning) || 
                !(pawn?.health?.hediffSet?.GetBodyPartRecord(InternalDefOf.Tongue) is BodyPartRecord tongue) || 
                (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(tongue) && pawn.health.hediffSet.PartIsMissing(tongue))) return true;
            if (pawn.needs?.TryGetNeed<Need_Thirst>().CurLevel <= 0.30f || __instance.GetPriority(pawn) == 0f)
            {
                __result = null;
                return false;
            }
            __result = JobMaker.MakeJob(InternalDefOf.BBLK_LickSelf);
            return false;
        }
    }
}