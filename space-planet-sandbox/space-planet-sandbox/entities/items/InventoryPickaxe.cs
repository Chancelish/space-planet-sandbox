using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using space_planet_sandbox.entities.environment;
using space_planet_sandbox.entities.player;
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

        private PointMask pointCheck;

        public InventoryPickaxe()
        {
            maxStack = 1;
            quantity = 1;
            miningPower = 2.5f;
            attackPower = 2f;
            icon = SandboxGame.loadedTextures["temppickaxeicon"];
            usable = true;
            tab = InventoryTab.Equipment;
            name = "pickaxe";
            pointCheck = new PointMask(0, 0);
        }

        public override void OnDrop(Point cursorLocation, PlayerCharacter callingEntity, World targetWorld)
        {

        }

        public override void OnUse(Point cursorLocation, PlayerCharacter callingEntity, World targetWorld)
        {
            pointCheck.MoveTo(cursorLocation.X, cursorLocation.Y);
            var block = targetWorld.GetTileAt(cursorLocation.X, cursorLocation.Y);
            if (block.tileName != "empty")
            {
                if (!SustainMining(cursorLocation, callingEntity, targetWorld))
                {
                    InitiateMining(cursorLocation, callingEntity, targetWorld, block.tileName);
                }
            }
        }

        public override void Render(SpriteBatch graphics, float x, float y)
        {
            graphics.Draw(icon, new Vector2(x, y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        private bool SustainMining(Point cursorLocation, PlayerCharacter callingEntity, World targetWorld)
        {
            var collisions = CollidableEntity.ExtractByCollisionGroup(targetWorld.GetPotentialCollisions(cursorLocation.X, cursorLocation.Y, 1, 1),
                null,
                "mining_object"
                );
            foreach (var block in collisions)
            {
                if (block.GetCollisionMask().Collide(pointCheck, 0, 0))
                {
                    (block as MinedBlockOverlay).Mine(miningPower, callingEntity.lastFrameTime);
                    return true;
                }
            }
            return false;
        }

        private void InitiateMining(Point cursorLocation, PlayerCharacter callingEntity, World targetWorld, string tileName)
        {
            var mineOverlay = new MinedBlockOverlay((cursorLocation.X / 16) * 16, (cursorLocation.Y/ 16) * 16, tileName);
            targetWorld.AddEntity(mineOverlay);
        }
    }
}
