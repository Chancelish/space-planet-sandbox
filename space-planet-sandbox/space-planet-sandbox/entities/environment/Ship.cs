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
        private Texture2D shipGraphic;
        private Point size;

        /* style is the general shape of the ship level is how big. */
        public Ship(string style, int level)
        {
            x = 16 * 32;
            y = 16 * 32;
            shipWalls = new GridMask(16, 28, 9, (int) x, (int) y);
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (j < 3 || j >= 7 || i < 3 || i >= 26 || (j < 4 && i >= 22))
                    {
                        shipWalls.ChangeTile(i, j, 1);
                    }
                }
            }
            shipGraphic = SandboxGame.loadedTextures["small_ship"];
            size = new Point(28 * 16, 9 * 16);
            collisionGroup = "solid";
        }

        public override ICollisionMask GetCollisionMask()
        {
            return shipWalls;
        }

        public override Point GetSize()
        {
            return size;
        }

        public override void Render(SpriteBatch graphics, float xDisplacement = 0, float yDisplacement = 0)
        {
            graphics.Draw(shipGraphic, new Vector2(x + xDisplacement + 6, y + yDisplacement + 5), Color.White);
        }

        public override void Update(GameTime time)
        {
            
        }
    }
}
