using Vintagestory.API.Common;

namespace kegrevivedagain
{
    public class kegrevivedagainModSystem : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockClass("BlockKeg", typeof(BlockKeg));
            api.RegisterBlockEntityClass("BlockKegEntity", typeof(BlockEntityKeg));
        }
    }
}
