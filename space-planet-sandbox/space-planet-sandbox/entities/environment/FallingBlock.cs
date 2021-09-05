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
        private float speed = 0f;
        private float terminalVelocity = 250f;
        private float gravity = 30f;

        public FallingBlock(float initX, float initY, string blockName)
        {
            x = initX;
            y = initY;
            sprite = SandboxGame.loadedTextures[blockName];
        }

        public override ICollisionMask GetCollisionMask()
        {
            return hitBox;
        }

        public override void Update(GameTime time)
        {
            float deltaT = (float)time.ElapsedGameTime.TotalSeconds;
            speed += gravity * deltaT;
            if (speed > terminalVelocity)
            {
                speed = terminalVelocity;
            }
            y += terminalVelocity;
            // if collide solid, go back to static block.
        }
    }
}
