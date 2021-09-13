using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.entities.environment
{
    class FallingBlock : CollidableEntity
    {
        private HitBox hitBox;
        private Texture2D sprite;
        private string blockName;
        private float speed = 0f;
        private float terminalVelocity = 400f;
        private float gravity = 60f;

        public FallingBlock(float initX, float initY, string blockName)
        {
            x = initX;
            y = initY;
            sprite = SandboxGame.loadedTextures[blockName];
            this.blockName = blockName;
            collisionGroup = "solid";
            hitBox = new HitBox((int)x, (int)y, 16, 16);
        }

        public override ICollisionMask GetCollisionMask()
        {
            return hitBox;
        }

        public override Point GetWidth()
        {
            return hitBox.Size();
        }

        public override void Update(GameTime time)
        {
            float deltaT = (float)time.ElapsedGameTime.TotalSeconds;
            speed += gravity * deltaT;
            if (speed > terminalVelocity)
            {
                speed = terminalVelocity;
            }
            y += speed * deltaT;
            hitBox.MoveTo((int)x, (int)y);
            var possibleCollisions = myWorld.GetPotentialCollisions((int)x, (int)y, hitBox.Size().X, hitBox.Size().Y);
            if (possibleCollisions.ContainsKey("tiles"))
            {
                foreach (var chunk in possibleCollisions["tiles"])
                {
                    if (Collide(chunk, 0, 0))
                    {
                        flaggedForRemoval = true;
                        myWorld.PlaceTile((int)x, (int)y, blockName);
                    }
                }
            }
        }

        public override void Render(SpriteBatch graphics)
        {
            graphics.Draw(sprite,
                new Vector2(x, y),
                null,
                Color.White,
                0f,
                new Vector2(0, 0),
                Vector2.One,
                SpriteEffects.None,
                0f);
        }
    }
}
