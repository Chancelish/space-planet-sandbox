using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using space_planet_sandbox.collisiondetection;
using space_planet_sandbox.entities.items;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.entities.environment
{
    public class BlockPreview : CollidableEntity
    {
        private HitBox hitBox;
        private Texture2D sprite;

        public bool placementBlocked { get; private set; }

        public BlockPreview(int tileSize)
        {
            hitBox = new HitBox(0, 0, tileSize, tileSize);
            x = 0f;
            y = 0f;
            sprite = SandboxGame.loadedTextures["forbidden_x"];
            collisionGroup = "none";
        }

        public override ICollisionMask GetCollisionMask()
        {
            return hitBox;
        }

        public override Point GetSize()
        {
            return hitBox.Size();
        }

        public override void Render(SpriteBatch graphics, float xDisplacement = 0, float yDisplacement = 0)
        {
            if (SandboxGame.gui.GetHighlightedItem() != null && SandboxGame.gui.GetHighlightedItem() is InventoryBlock)
            {
                var block = (InventoryBlock)SandboxGame.gui.GetHighlightedItem();
                var cornerOfTexture = new Rectangle(0, 0, 16, 16);
                var position = new Vector2(x + xDisplacement, y + yDisplacement);
                Color transparency = new Color(1.0f, 1.0f, 1.0f, 0.3f);
                graphics.Draw(SandboxGame.loadedTextures[block.name], position, cornerOfTexture, transparency, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                if (placementBlocked)
                {
                    graphics.Draw(sprite, position, transparency);
                }
            }
        }

        public void UpdatePosition(int xLoc, int yLoc)
        {
            int xTileLoc = xLoc / 16 * 16;
            int yTileLoc = yLoc / 16 * 16;
            x = xTileLoc;
            y = yTileLoc;
            hitBox.MoveTo(xTileLoc, yTileLoc);
        }

        public override void Update(GameTime time)
        {
            var possibleCollisions = myWorld.GetPotentialCollisions((int)x, (int)y, 16, 16);
            foreach (var entity in CollidableEntity.ExtractByCollisionGroup(possibleCollisions, this, "player", "solid", "npc", "monster"))
            {
                if (Collide(entity, 0, 0))
                {
                    placementBlocked = true;
                    return;
                }
            }
            placementBlocked = false;
        }
    }
}
