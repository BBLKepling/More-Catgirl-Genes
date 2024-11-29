using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace More_Catgirl_Genes
{
    [HarmonyPatch(typeof(JobDriver_Vomit))]
    [HarmonyPatch("MakeNewToils")]
    public class JobDriver_Vomit_Patch
    {
        [HarmonyPostfix]
        public static IEnumerable<Toil> Postfix(IEnumerable<Toil> oldToils, JobDriver_Vomit __instance)
        {
            Pawn pawn = __instance.pawn;
            if (!(pawn?.health?.hediffSet is HediffSet hediffSet) || !hediffSet.TryGetHediff(InternalDefOf.BBLK_Hairball, out Hediff hediff) || hediff.Severity < 0.5 || Rand.Range(1, 101) > hediff.Severity * 100)
            {
                foreach (Toil toil in oldToils) yield return toil;
                yield break;
            }
            foreach (Toil toil in oldToils)
            {
                if (toil.debugName != "MakeNewToils") yield return toil;
                else
                {
                    toil.AddFinishAction(delegate
                    {
                        FilthMaker.TryMakeFilth(__instance.job.targetA.Cell, pawn.Map, InternalDefOf.BBLK_Filth_Hairball, pawn.LabelIndefinite());
                        pawn.health.RemoveHediff(hediff);
                    });
                    yield return toil;
                }
            }
        }
    }
}
