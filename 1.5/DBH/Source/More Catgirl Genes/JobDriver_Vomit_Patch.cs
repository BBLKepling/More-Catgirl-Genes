using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace More_Catgirl_Genes
{
    public class JobDriver_Vomit_Patch
    {
        [HarmonyPostfix]
        public static IEnumerable<Toil> PostfixVanilla(IEnumerable<Toil> oldToils, JobDriver_Vomit __instance)
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
        [HarmonyPrefix]
        [HarmonyBefore("com.carryabarfbag.preventvomit")]
        public static void PrefixBarfBag(out bool __state, JobDriver_Vomit __instance) => __state = __instance.pawn?.inventory?.innerContainer?.FirstOrDefault(item => item.def == InternalDefOf.BarfBag) is Thing;
        [HarmonyPostfix]
        public static IEnumerable<Toil> PostfixBarfBag(IEnumerable<Toil> oldToils, JobDriver_Vomit __instance, bool __state)
        {
            Pawn pawn = __instance.pawn;
            if (!(pawn?.health?.hediffSet is HediffSet hediffSet) || !hediffSet.TryGetHediff(InternalDefOf.BBLK_Hairball, out Hediff hediff) || hediff.Severity < 0.5 || Rand.Range(1, 101) > hediff.Severity * 100)
            {
                foreach (Toil toil in oldToils) yield return toil;
                yield break;
            }
            if (!__state)
            {
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
                yield break;
            }
            bool patched = false;
            foreach (Toil toil in oldToils)
            {
                if (!patched)
                {
                    toil.AddFinishAction(delegate
                    {
                        pawn.health.RemoveHediff(hediff);
                    });
                    patched = true;
                }
                yield return toil;
            }
        }
    }
}
