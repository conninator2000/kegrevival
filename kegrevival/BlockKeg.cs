using System;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace kegrevivedagain
{
    public class BlockKeg : BlockLiquidContainerTopOpened
    {
        public override float CapacityLitres => 50f;

        protected override string meshRefsCacheKey => "kegMeshRefs";

        protected override AssetLocation emptyShapeLoc => new AssetLocation("kegrevivedagain:shapes/block/wood/keg/empty.json");

        protected override AssetLocation contentShapeLoc => new AssetLocation("kegrevivedagain:shapes/block/wood/keg/contents.json");

        protected override float liquidMaxYTranslate => 7f / 16f;

        public override void OnLoaded(ICoreAPI api) => base.OnLoaded(api);

        public override bool DoPlaceBlock(
          IWorldAccessor world,
          IPlayer byPlayer,
          BlockSelection blockSel,
          ItemStack byItemStack)
        {
            bool flag = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);
            if (flag && world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityKeg blockEntity)
            {
                BlockPos blockPos = blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position;
                float num1 = (float)Math.Atan2(((EntityPos)((Entity)byPlayer.Entity).Pos).X - ((double)blockPos.X + blockSel.HitPosition.X), ((EntityPos)((Entity)byPlayer.Entity).Pos).Z - ((double)blockPos.Z + blockSel.HitPosition.Z));
                float num2 = 0.3926991f;
                float num3 = (float)(int)Math.Round((double)num1 / (double)num2) * num2;
                blockEntity.MeshAngle = num3;
            }
            return flag;
        }
    }
}