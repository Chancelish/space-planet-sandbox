using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.entities.environment
{
    public class MinedBlockOverlay : CollidableEntity
    {
        private HitBox hitBox;
        private float toughness;
        private float durability;
        private float maxDurability;
        private readonly float regenRate;
        private double timeDelay;
        private int frame;
        private Texture2D sprite;
        private bool beingMined;
        private string name;

        public MinedBlockOverlay(int _x, int _y, string blockName)
        {
            x = _x;
            y = _y;
            name = blockName;
            var blockTemplate = world.TileDataDictionary.GetTile(blockName);
            toughness = blockTemplate.toughness;
            maxDurability = durability = blockTemplate.maxDurability;
            regenRate = maxDurability / 8;
            sprite = SandboxGame.loadedTextures["mining_overlay"];
            hitBox = new HitBox(_x, _y, 15, 15);
            frame = 0;
            collisionGroup = "mining_object";
        }

        public void Mine(float miningPower, double time)
        {
            beingMined = true;
            float mineRate = miningPower - toughness;
            if (mineRate > 0)
            {
                durability -= mineRate * (float) time;
                // play *chip* noise
            }
            else
            {
                // play *clink* noise
            }
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
            graphics.Draw(sprite, new Vector2(x + xDisplacement, y + yDisplacement), new Rectangle(16 * frame, 0, 16, 16), Color.White);
        }

        public override void Update(GameTime time)
        {
            if (beingMined)
            {
                timeDelay = 0.5;
                if (durability <= 0)
                {
                    flaggedForRemoval = true;
                    myWorld.MineTile((int) x + 1, (int) y + 1);
                    myWorld.AddEntity(new items.WorldBlock(x, y, 1, name));
                }
                beingMined = false;
            }
            else
            {
                timeDelay -= time.ElapsedGameTime.TotalSeconds;
                if (timeDelay <= 0)
                {
                    timeDelay += 0.5;
                    durability += regenRate;
                    if (durability > maxDurability) flaggedForRemoval = true;
                }
            }
            frame = (int) ((1f - durability / maxDurability) * 4);
        }
    }
}
