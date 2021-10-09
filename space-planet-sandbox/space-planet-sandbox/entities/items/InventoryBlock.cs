using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.world;

namespace space_planet_sandbox.entities.items
{
    public class InventoryBlock : InventoryItem
    {

        public readonly string blockName;

        public InventoryBlock(string blockName, int max, int initialValue)
        {
            maxStack = max;
            quantity = initialValue;
            this.blockName = blockName;
            icon = SandboxGame.loadedTextures[blockName];
            usable = true;
        }

        public override void OnUse(Point cursorLocation, CollidableEntity callingEntity, World targetWorld)
        {
            if (!targetWorld.IsPlacementBlocked())
            {
                if (targetWorld.PlaceTile(cursorLocation.X, cursorLocation.Y, blockName)) quantity--;
            }
        }

        public override void OnDrop(Point cursorLocation, CollidableEntity callingEntity, World targetWorld)
        {
            
        }

        public override void Render(SpriteBatch graphics, float x, float y)
        {
            var cornerOfTexture = new Rectangle(0, 0, 16, 16);
            var xActual = x + 8f;
            var yActual = y + 8f;
            graphics.Draw(icon, new Vector2(xActual, yActual), cornerOfTexture, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            TextUtils.RenderOutlinedText(graphics, SandboxGame.smolFont, quantity.ToString(), (int) x + 1, (int) y + 1);
        }
    }
}
