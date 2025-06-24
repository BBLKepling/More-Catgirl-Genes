using DubsBadHygiene;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace More_Catgirl_Genes
{
    public class JobDriver_LickSelf : JobDriver
    {
        private bool bionicTongue;
        private bool catLax;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_GoToLickSpot.GoToLickSpot(pawn);
            Toil toil = new Toil
            {
                debugName = "MakeNewToils",
                defaultDuration = 1000,
                defaultCompleteMode = ToilCompleteMode.Delay
            };
            toil.WithEffect(DubDef.WashingEffect, TargetIndex.A);
            if (ModLister.HasActiveModWithName("Dubs Bad Hygiene"))
            {
                toil.AddEndCondition(() => (pawn.needs.TryGetNeed<Need_Hygiene>().CurLevel < (bionicTongue ? 0.75f : 0.65f) && pawn.needs?.TryGetNeed<Need_Thirst>().CurLevel >= 0.30f) ? JobCondition.Ongoing : JobCondition.Succeeded);
                toil.tickAction = delegate
                {
                    pawn.needs.TryGetNeed<Need_Hygiene>().clean(bionicTongue ? 0.002f : 0.001f);
                    if (ticksLeftThisToil % 3 != 0) return;
                    pawn.needs.TryGetNeed<Need_Thirst>().Drink(-0.001f);
                    if (ticksLeftThisToil % 60 != 0) return;
                    if (Rand.Range(1, 6) == 1)
                    {
                        FilthMaker.TryMakeFilth(pawn.Position, pawn.Map, ThingDefOf.Filth_AnimalFilth, pawn.LabelIndefinite(), 1, FilthSourceFlags.Pawn);
                        return;
                    }
                    if (!(pawn.health?.hediffSet is HediffSet hediffset)) return;
                    if (hediffset.TryGetHediff(InternalDefOf.BBLK_Hairball, out Hediff hediff))
                    {
                        hediff.Severity += catLax ? (Rand.Range(1, 4) * -.01f) : (Rand.Range(1, 4) * .01f);
                        if (catLax && hediff.Severity <= .01f) pawn.health.RemoveHediff(hediff);
                        return;
                    }
                    if (catLax) return;
                    BodyPartRecord stomach = hediffset.GetBodyPartRecord(InternalDefOf.Stomach);
                    if (!hediffset.PartOrAnyAncestorHasDirectlyAddedParts(stomach) && hediffset.PartIsMissing(stomach)) return;
                    pawn.health.AddHediff(InternalDefOf.BBLK_Hairball, stomach);
                };
            }
            else
            {
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
                    if (!(pawn.health?.hediffSet is HediffSet hediffset)) return;
                    if (hediffset.TryGetHediff(InternalDefOf.BBLK_Hairball, out Hediff hediff))
                    {
                        hediff.Severity += catLax ? (Rand.Range(1, 4) * -.01f) : (Rand.Range(1, 4) * .01f);
                        if (catLax && hediff.Severity <= .01f) pawn.health.RemoveHediff(hediff);
                        return;
                    }
                    if (catLax) return;
                    BodyPartRecord stomach = hediffset.GetBodyPartRecord(InternalDefOf.Stomach);
                    if (!hediffset.PartOrAnyAncestorHasDirectlyAddedParts(stomach) && hediffset.PartIsMissing(stomach)) return;
                    pawn.health.AddHediff(InternalDefOf.BBLK_Hairball, stomach);
                };
            }
            toil.AddFinishAction(delegate
            {
                pawn.CleanContamination();
                SanitationUtil.ContaminationFromCellForPawn(pawn, pawn.CurJob.GetTarget(TargetIndex.A).Cell);
            });
            yield return toil;
        }
        public override void Notify_Starting()
        {
            base.Notify_Starting();
            job.count = 1;
            job.targetA = pawn;
            bionicTongue = pawn.health.hediffSet.HasHediff(InternalDefOf.BionicTongue);
            catLax = ModLister.HasActiveModWithName("Kepling's Hairball Mod") && pawn.health.hediffSet.HasHediff(InternalDefOf.BBLK_CatLax);
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref bionicTongue, "bionicTongue", defaultValue: false);
            Scribe_Values.Look(ref catLax, "catLax", defaultValue: false);
        }
    }
}
