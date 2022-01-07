using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.entities.player;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.entities.items
{
    public class WorldBlock : WorldItem
    {
        private float gravity = 250f;
        private float terminalVelocity = 500f;
        private float airResistance = 0.03f;
        private float groundFriction = 5f;
        private float xVelocity;
        private float yVelocity;
        private bool onGround;
        private Texture2D icon;

        public WorldBlock(float _x, float _y, int quantity, string block)
        {
            name = block;
            associatedItem = new InventoryBlock(block, 1000, quantity);
            yVelocity = -50f;
            icon = SandboxGame.loadedTextures[block];
            var rand = new Random();
            xVelocity = (float) (rand.NextDouble() - 0.5) * 100f;
            x = _x;
            y = _y;
            hitBox = new collisiondetection.HitBox((int) _x, (int) _y, 12, 12);
            collisionGroup = "item";
        }

        public WorldBlock(float _x, float _y, int quantity, string block, int mouseX)
        {
            name = block;
            associatedItem = new InventoryBlock(block, 1000, quantity);
            yVelocity = -80f;
            icon = SandboxGame.loadedTextures[block];
            xVelocity = mouseX > _x ? 120f : -120f;
            x = _x;
            y = _y;
            hitBox = new collisiondetection.HitBox((int) _x, (int) _y, 12, 12);
            collisionGroup = "item";
        }

        public override void Collect(PlayerCharacter entity, PlayerInventory inventory)
        {
            for (int i = 0; i < 60; i++)
            {
                var item = inventory.GetItemAt(associatedItem.tab, i);
                if (item != null && item.name.Equals(name))
                {
                    if (1000 - item.quantity > associatedItem.quantity)
                    {
                        item.quantity += associatedItem.quantity;
                        associatedItem.quantity = 0;
                        flaggedForRemoval = true;
                        return;
                    }
                    else
                    {
                        associatedItem.quantity -= 1000 - item.quantity;
                        item.quantity = 1000;
                    }
                }
            }
            if (inventory.Add(associatedItem))
            {
                flaggedForRemoval = true;
            }
        }

        public override Point GetSize()
        {
            return hitBox.Size();
        }

        public override void Render(SpriteBatch graphics, float xDisplacement = 0, float yDisplacement = 0)
        {
            var cornerOfTexture = new Rectangle(0, 0, 16, 16);
            graphics.Draw(icon, new Vector2(x + xDisplacement, y + yDisplacement), cornerOfTexture, Color.White);
            if (associatedItem.quantity > 1)
            {
                graphics.Draw(icon, new Vector2(x + xDisplacement  - 3, y + yDisplacement - 3), cornerOfTexture, Color.White);
            }
        }

        public override void Update(GameTime time)
        {
            var possibleCollisions = myWorld.GetPotentialCollisions((int)x, (int)y, hitBox.Size().X, hitBox.Size().Y);

            ManagePhysics(time, possibleCollisions);

            if (possibleCollisions.ContainsKey("item"))
            {
                foreach (var item in possibleCollisions["item"])
                {
                    var block = item as WorldBlock;
                    if (block != null && block.name.Equals(name) && !Equals(block) && Collide(block, 0, 0))
                    {
                        AttemptMerge(block);
                    }
                }
            }
        }

        public bool AttemptMerge(WorldBlock block)
        {
            if (block.associatedItem.quantity + associatedItem.quantity < 1000 && !(flaggedForRemoval || block.flaggedForRemoval))
            {
                block.associatedItem.quantity += associatedItem.quantity;
                associatedItem.quantity = 0;
                flaggedForRemoval = true;
            }
            return false;
        }

        private void ManagePhysics(GameTime time, Dictionary<string, HashSet<CollidableEntity>> possibleCollisions)
        {
            var deltaT = (float) time.ElapsedGameTime.TotalSeconds;
            yVelocity += gravity * deltaT;
            if (yVelocity > terminalVelocity) yVelocity = terminalVelocity;
            var friction = onGround ? groundFriction : airResistance;
            if (Math.Abs(xVelocity) < friction * 2f)
            {
                xVelocity = 0;
            }
            else
            {
                xVelocity -= xVelocity * deltaT * friction;
            }

            var deltaX = xVelocity * deltaT;
            var deltaY = yVelocity * deltaT;

            int xCheck = (int)(deltaX + Math.Sign(xVelocity));
            int yCheck = (int)(deltaY + Math.Sign(yVelocity));

            var tiles = ExtractByCollisionGroup(possibleCollisions, null, "tiles");

            foreach (var chunk in tiles)
            {
                if (Collide(chunk, xCheck, 0) || WorldWrapCollisionCheck(chunk, xCheck, 0))
                {
                    while (xCheck != 0)
                    {
                        xCheck = xCheck > 0 ? xCheck - 1 : xCheck + 1;
                        deltaX = xCheck;
                        if (!Collide(chunk, xCheck, 0) && !WorldWrapCollisionCheck(chunk, xCheck, 0))
                        {
                            xVelocity = deltaX / deltaT;
                            break;
                        }
                    }
                    xVelocity = deltaX / deltaT;
                    break;
                }
            }
            foreach (var chunk in tiles)
            {
                if (Collide(chunk, 0, yCheck) || WorldWrapCollisionCheck(chunk, 0, yCheck))
                {
                    while (yCheck != 0)
                    {
                        yCheck = yCheck > 0 ? yCheck - 1 : yCheck + 1;
                        deltaY = yCheck;
                        if (!(Collide(chunk, 0, yCheck) || WorldWrapCollisionCheck(chunk, 0, yCheck)))
                        {
                            yVelocity = deltaY / deltaT;
                            break;
                        }
                    }
                    onGround = true;
                    yVelocity = deltaY / deltaT;
                    break;
                }
                else onGround = false;
            }

            Displace(deltaX, deltaY);
        }
    }
}
