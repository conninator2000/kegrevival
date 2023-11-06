using Vintagestory.API.Common;

namespace kegrevival
{
    public class kegrevivalModSystem : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockClass("BlockKeg", typeof(BlockKeg));
            api.RegisterBlockEntityClass("BlockKegEntity", typeof(BlockEntityKeg));
        }
    }
}
