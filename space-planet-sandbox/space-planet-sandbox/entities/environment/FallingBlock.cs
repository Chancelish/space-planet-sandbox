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
        private float terminalVelocity = 420f;
        private float gravity = 100f;

        public FallingBlock(float initX, float initY, string blockName)
        {
            x = initX;
            y = initY;
            sprite = SandboxGame.loadedTextures[blockName];
            this.blockName = blockName;
            collisionGroup = "solid";
            hitBox = new HitBox((int)x, (int)y, 15, 16);
        }

        public override ICollisionMask GetCollisionMask()
        {
            return hitBox;
        }

        public override Point GetSize()
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
            var solidCollisions = ExtractByCollisionGroup(possibleCollisions, this, "tiles", "solid");
            foreach (var solid in solidCollisions)
            {
                if (Collide(solid, 0, 0))
                {
                    flaggedForRemoval = true;
                    myWorld.PlaceTile((int)x, (int)y, blockName);
                }
            }
        }

        public override void Render(SpriteBatch graphics, float xDisplacement = 0, float yDisplacement = 0)
        {
            graphics.Draw(sprite,
                new Vector2(x + xDisplacement, y + yDisplacement),
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
