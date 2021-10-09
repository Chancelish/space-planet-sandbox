using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.entities.items
{
    public class InventoryPickaxe : InventoryItem
    {
        float miningPower;
        float attackPower;

        public InventoryPickaxe()
        {
            maxStack = 1;
            quantity = 1;
            miningPower = 2.5f;
            attackPower = 2f;
            icon = SandboxGame.loadedTextures["temppickaxeicon"];
            usable = true;
        }

        public override void OnDrop(Point cursorLocation, CollidableEntity callingEntity, World targetWorld)
        {
            
        }

        public override void OnUse(Point cursorLocation, CollidableEntity callingEntity, World targetWorld)
        {
            targetWorld.MineTile(cursorLocation.X, cursorLocation.Y, miningPower);
        }

        public override void Render(SpriteBatch graphics, float x, float y)
        {
            graphics.Draw(icon, new Vector2(x, y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
