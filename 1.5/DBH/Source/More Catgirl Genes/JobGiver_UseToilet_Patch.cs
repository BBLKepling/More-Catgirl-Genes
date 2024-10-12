using DubsBadHygiene;
using HarmonyLib;
using Verse.AI;
using Verse;
using RimWorld;

namespace More_Catgirl_Genes
{
    [HarmonyPatch(typeof(JobGiver_UseToilet), nameof(JobGiver_UseToilet.TryGiveJob))]
    class JobGiver_UseToilet_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(ref Job __result, Pawn pawn, JobGiver_UseToilet __instance)
        {
            if (pawn.RaceProps.Animal || !pawn.genes.HasActiveGene(InternalDefOf.BBLK_WildShitter)) return true;
            if (__instance.GetPriority(pawn) == 0f)
            {
                __result = null;
                return false;
            }
            if (pawn.genes.HasActiveGene(InternalDefOf.BBLK_LitterTrained) && pawn.Faction.IsPlayer)
            {
                Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(DubDef.LitterBox), PathEndMode.OnCell, TraverseParms.For(pawn), 30f, Val);
                if (thing != null)
                {
                    __result = JobMaker.MakeJob(DubDef.UseToilet, thing);
                    return false;
                }
            }
            IntVec3 intVec = SanitationUtil.TryFindPoopCell(pawn);
            if (intVec.IsValid)
            {
                __result = JobMaker.MakeJob(DubDef.haveWildPoo, intVec);
                return false;
            }
            if (RCellFinder.TryFindNearbyEmptyCell(pawn, out IntVec3 cell) && cell.IsValid)
            {
                __result = JobMaker.MakeJob(DubDef.haveWildPoo, cell);
                return false;
            }
            __result = null;
            return false;
            bool Val(Thing x)
            {
                if (pawn.CanReserve(x))
                {
                    return !x.IsForbidden(pawn);
                }
                return false;
            }
        }
    }
}