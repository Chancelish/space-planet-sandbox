using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using space_planet_sandbox.world;
using System.Collections.Generic;

namespace space_planet_sandbox.entities.player
{
    class PlayerCharacter : CollidableEntity
    {
        private HitBox hurtBox;
        private float speed = 200.0f;
        private Texture2D sprite;
        public List<TileMap> interSectingChunks;

        public PlayerCharacter(int startX, int startY)
        {
            hurtBox = new HitBox(startX, startY, 16, 32);
            sprite = SandboxGame.loadedTextures["unknown"];
            x = startX;
            y = startY;
        }
        public override ICollisionMask GetCollisionMask()
        {
            return hurtBox;
        }

        public override void Update(GameTime time)
        {
            SandboxGame.camera.Follow(this, 1280, 720);
            
            double deltaY = 0;
            double deltaX = 0;

            if (InputUtils.GetKeyState("directionUp"))
                deltaY -= speed * time.ElapsedGameTime.TotalSeconds;

            if (InputUtils.GetKeyState("directionDown"))
                deltaY += speed * time.ElapsedGameTime.TotalSeconds;

            if (InputUtils.GetKeyState("directionLeft"))
                deltaX -= speed * time.ElapsedGameTime.TotalSeconds;

            if (InputUtils.GetKeyState("directionRight"))
                deltaX += speed * time.ElapsedGameTime.TotalSeconds;

            int xCheck = (int) (deltaX + Math.Sign(deltaX));
            int yCheck = (int) (deltaY + Math.Sign(deltaY));

            foreach (var chunk in interSectingChunks)
            {
                if (Collide(chunk, xCheck, 0))
                {
                    deltaX = 0;
                    break;
                }
            }
            foreach (var chunk in interSectingChunks)
            {
                if (Collide(chunk, 0, yCheck))
                {
                    deltaY = 0;
                    break;
                }
            }

            x += (float) deltaX;
            y += (float) deltaY;
            hurtBox.MoveTo((int) x, (int) y);
        }

        public void Render(SpriteBatch graphics)
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
