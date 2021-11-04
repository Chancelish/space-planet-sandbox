using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.entities.environment
{
    public class Ship : CollidableEntity
    {
        private GridMask shipWalls;



        public override ICollisionMask GetCollisionMask()
        {
            return shipWalls;
        }

        public override Point GetSize()
        {
            throw new NotImplementedException();
        }

        public override void Render(SpriteBatch graphics, float xDisplacement = 0, float yDisplacement = 0)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime time)
        {
            
        }
    }
}
