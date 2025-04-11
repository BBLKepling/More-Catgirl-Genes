using RimWorld;
using Verse;
using Verse.AI;

namespace More_Catgirl_Genes
{
    public class Toils_GoToLickSpot
    {
        public static Toil GoToLickSpot(Pawn pawn)
        {
            Toil toil = ToilMaker.MakeToil("GoToLickSpot");
            toil.initAction = delegate
            {
                Pawn actor = toil.actor;
                IntVec3 cell = IntVec3.Invalid;
                Thing thing = null;
                thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(actor), 30, (Thing t) => BaseChairValidator(t) && t.Position.GetDangerFor(pawn, t.Map) == Danger.None);
                if (thing == null)
                {
                    RCellFinder.TryFindNearbyEmptyCell(actor, out cell);
                    Danger lickSpotDanger = cell.GetDangerFor(pawn, actor.Map);
                    if (lickSpotDanger != Danger.None)
                    {
                        thing = GenClosest.ClosestThingReachable(actor.Position, actor.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(actor), 30, (Thing t) => BaseChairValidator(t) && (int)t.Position.GetDangerFor(pawn, t.Map) <= (int)lickSpotDanger);
                    }
                }
                if (thing != null && !Toils_Ingest.TryFindFreeSittingSpotOnThing(thing, actor, out cell))
                {
                    Log.Error("Could not find sitting spot on licking chair! This is not supposed to happen - we looked for a free spot in a previous check!");
                }
                actor.ReserveSittableOrSpot(cell, actor.CurJob);
                actor.Map.pawnDestinationReservationManager.Reserve(actor, actor.CurJob, cell);
                actor.pather.StartPath(cell, PathEndMode.OnCell);
                bool BaseChairValidator(Thing t)
                {
                    if (t.def.building == null || !t.def.building.isSittable || !Toils_Ingest.TryFindFreeSittingSpotOnThing(t, actor, out var cell2) || t.IsForbidden(pawn) || (actor.IsColonist && t.Position.Fogged(t.Map)) || !actor.CanReserve(t) || !t.IsSociallyProper(actor) || t.IsBurning() || t.HostileTo(pawn))
                    {
                        return false;
                    }
                    return true;
                }
            };
            toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
            return toil;
        }
    }
}
