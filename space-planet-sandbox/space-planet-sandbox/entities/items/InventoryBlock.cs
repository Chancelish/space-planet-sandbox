using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.entities.player;
using space_planet_sandbox.world;

namespace space_planet_sandbox.entities.items
{
    public class InventoryBlock : InventoryItem
    {

        public InventoryBlock(string blockName, int max, int initialValue)
        {
            maxStack = max;
            quantity = initialValue;
            name = blockName;
            icon = SandboxGame.loadedTextures[blockName];
            usable = true;
            tab = InventoryTab.Equipment; //TODO: put things in correct tab once tabs work.
        }

        public override void OnUse(Point cursorLocation, PlayerCharacter callingEntity, World targetWorld)
        {
            int x = cursorLocation.X;
            int y = cursorLocation.Y;
            if (x < 0) x += targetWorld.pixelWorldWidth;
            else if (x >= targetWorld.pixelWorldWidth) x -= targetWorld.pixelWorldWidth;
            if (!targetWorld.IsPlacementBlocked())
            {
                if (targetWorld.PlaceTile(x, y, name))
                {
                    callingEntity.cooldown = 0.05;
                    quantity--;
                }
            }
        }

        public override void OnDrop(Point cursorLocation, PlayerCharacter callingEntity, World targetWorld)
        {
            targetWorld.AddEntity(new WorldBlock(callingEntity.Position().X + callingEntity.GetSize().X / 2,
                callingEntity.Position().Y,
                quantity,
                name,
                cursorLocation.X));
        }

        public override void Render(SpriteBatch graphics, float x, float y)
        {
            var cornerOfTexture = new Rectangle(0, 0, 16, 16);
            var xActual = x + 8f;
            var yActual = y + 8f;
            graphics.Draw(icon, new Vector2(xActual, yActual), cornerOfTexture, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            TextUtils.RenderOutlinedText(graphics, TextUtils.smolFont, quantity.ToString(), (int) x + 1, (int) y + 1);
        }
    }
}
