using DubsBadHygiene;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace More_Catgirl_Genes
{
    public class JobDriver_LickSelf : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
        protected override IEnumerable<Toil> MakeNewToils()
        {
            job.targetA = pawn;
            bool bionicTongue = pawn.health.hediffSet.HasHediff(InternalDefOf.BionicTongue);
            yield return Toils_GoToLickSpot.GoToLickSpot(pawn);
            Toil toil = new Toil
            {
                debugName = "MakeNewToils",
                defaultDuration = 1000,
                defaultCompleteMode = ToilCompleteMode.Delay
            };
            toil.WithEffect(DubDef.WashingEffect, TargetIndex.A);
            toil.AddEndCondition(() => (pawn.needs.TryGetNeed<Need_Hygiene>().CurLevel < (bionicTongue ? 0.75f : 0.65f)) ? JobCondition.Ongoing : JobCondition.Succeeded);
            toil.tickAction = delegate
            {
                pawn.needs.TryGetNeed<Need_Hygiene>().clean(bionicTongue ? 0.002f : 0.001f);
                if (ticksLeftThisToil % 60 != 0) return;
                if (Rand.Range(1, 6) == 1)
                {
                    FilthMaker.TryMakeFilth(pawn.Position, pawn.Map, ThingDefOf.Filth_AnimalFilth, pawn.LabelIndefinite(), 1, FilthSourceFlags.Pawn);
                    return;
                }
                HediffSet hediffset = pawn.health.hediffSet;
                if (hediffset.TryGetHediff(InternalDefOf.BBLK_Hairball, out Hediff hediff))
                {
                    hediff.Severity += (Rand.Range(1, 4) * .01f);
                    return;
                }
                BodyPartRecord stomach = hediffset.GetBodyPartRecord(InternalDefOf.Stomach);
                if (!hediffset.PartOrAnyAncestorHasDirectlyAddedParts(stomach) && hediffset.PartIsMissing(stomach)) return;
                pawn.health.AddHediff(InternalDefOf.BBLK_Hairball, stomach);
                return;
            };
            toil.AddFinishAction(delegate
            {
                pawn.CleanContamination();
                SanitationUtil.ContaminationFromCellForPawn(pawn, pawn.CurJob.GetTarget(TargetIndex.A).Cell);
            });
            yield return toil;
        }
    }
}
