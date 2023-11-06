using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace kegrevival 
{
    public class BlockEntityKeg : BlockEntityLiquidContainer
    {
        private ICoreAPI api;
        private MeshData currentMesh;
        private BlockKeg ownBlock;
        public float MeshAngle;

        public override string InventoryClassName => "keg";

        public BlockEntityKeg() => this.inventory = new InventoryGeneric(1, (string)null, (ICoreAPI)null, (NewSlotDelegate)null);

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
            this.api = api;
            this.ownBlock = this.Block as BlockKeg;
            if (api.Side != EnumAppSide.Client) return;
            this.currentMesh = GenMesh();
            MarkDirty(true, (IPlayer)null);
        }

        protected override float Inventory_OnAcquireTransitionSpeed(
          EnumTransitionType transType,
          ItemStack stack,
          float baseMul)
        {
            if (transType == EnumTransitionType.Dry)
            {
                return (room != null ? (room.ExitCount == 0 ? 1 : 0) : 0) != 0 ? 2f : 0.5f;
            }
            if (transType != EnumTransitionType.Perish || transType != EnumTransitionType.Ripen)
                return 1f;
            float perishRate = GetPerishRate();
            return transType == EnumTransitionType.Ripen ? GameMath.Clamp((float)((1.0 - (double)perishRate - 0.5) * 3.0), 0.0f, 1f) : baseMul * (perishRate - 0.1f);
        }

        public override void OnBlockBroken(IPlayer byPlayer = null)
        {
        }

        public override void OnBlockPlaced(ItemStack byItemStack = null)
        {
            base.OnBlockPlaced(byItemStack);
            if (api.Side != EnumAppSide.Client)
                return;
            currentMesh =GenMesh();
            MarkDirty(true, (IPlayer)null);
        }

        internal MeshData GenMesh()
        {
            if (ownBlock == null)
                return (MeshData)null;
            MeshData meshData = this.ownBlock.GenMesh(api as ICoreClientAPI, this.GetContent(), Pos);
            if (meshData.CustomInts != null)
            {
                for (int index = 0; index < meshData.CustomInts.Count; ++index)
                {
                    meshData.CustomInts.Values[index] |= 134217728;
                    meshData.CustomInts.Values[index] |= 67108864;
                }
            }
            return meshData;
        }

        public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
        {
            mesher?.AddMeshData(currentMesh?.Clone()?.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0.0f, MeshAngle, 0.0f), 1);
            return true;
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
        {
            base.FromTreeAttributes(tree, worldForResolving);
            MeshAngle = tree.GetFloat("meshAngle", MeshAngle);
            if (api == null || api.Side != EnumAppSide.Client)
                return;
            currentMesh = GenMesh();
            MarkDirty(true, null);
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            tree.SetFloat("meshAngle", MeshAngle);
        }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            ItemSlot itemSlot = inventory[0];
            if (itemSlot.Empty)
                dsc.AppendLine(Lang.Get("Empty", Array.Empty<object>()));
            else
                dsc.AppendLine(Lang.Get("Contents: {0}x{1}", new object[2]
                {
                    itemSlot.Itemstack.StackSize,
                    itemSlot.Itemstack.GetName()
                }));
        }
    }
}