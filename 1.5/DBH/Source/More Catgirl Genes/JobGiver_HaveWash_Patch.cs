using DubsBadHygiene;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace More_Catgirl_Genes
{
    public class JobGiver_HaveWash_Patch
    {
        [HarmonyPrefix]
        public static bool PrefixReg(ref Job __result, Pawn pawn, JobGiver_HaveWash __instance)
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
        [HarmonyPrefix]
        public static bool PrefixLite(ref Job __result, Pawn pawn, JobGiver_HaveWash __instance)
        {
            if (!pawn.genes.HasActiveGene(InternalDefOf.BBLK_Tongue_Cleaning) ||
                !(pawn?.health?.hediffSet?.GetBodyPartRecord(InternalDefOf.Tongue) is BodyPartRecord tongue) ||
                (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(tongue) && pawn.health.hediffSet.PartIsMissing(tongue))) return true;
            if (__instance.GetPriority(pawn) == 0f)
            {
                __result = null;
                return false;
            }
            __result = JobMaker.MakeJob(InternalDefOf.BBLK_LickSelf);
            return false;
        }
    }
}