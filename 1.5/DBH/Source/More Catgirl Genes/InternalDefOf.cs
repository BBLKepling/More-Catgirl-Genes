using RimWorld;
using Verse;

namespace More_Catgirl_Genes
{
    [DefOf]
    public static class InternalDefOf
    {
        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }

        public static BodyPartDef Stomach;
        public static BodyPartDef Tongue;
        public static HediffDef BionicTongue;

        public static HediffDef BBLK_Hairball;
        public static JobDef BBLK_LickSelf;
        public static GeneDef BBLK_LitterTrained;
        public static GeneDef BBLK_WildShitter;
        public static GeneDef BBLK_Tongue_Cleaning;
        public static ThingDef BBLK_Filth_Hairball;

        [MayRequire("silkcircuit.carryabarfbag")]
        public static ThingDef BarfBag;

        [MayRequire("bblkepling.hairballmod")]
        public static HediffDef BBLK_CatLax;
    }
}
