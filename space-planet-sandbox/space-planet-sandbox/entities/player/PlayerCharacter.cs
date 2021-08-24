using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using Microsoft.Xna.Framework.Input;
using space_planet_sandbox.world;
using System.Collections.Generic;

namespace space_planet_sandbox.entities.player
{
    class PlayerCharacter : CollidableEntity
    {
        private HitBox hurtBox;
        private float speed = 100.0f;
        private Texture2D sprite;
        public List<TileMap> interSectingChunks;

        public PlayerCharacter(int startX, int startY)
        {
            hurtBox = new HitBox(startX, startY, 16, 32);
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
            
            var kstate = Keyboard.GetState();
            double deltaY = 0;
            double deltaX = 0;
            sprite = SandboxGame.loadedTextures["unknown"];

            if (kstate.IsKeyDown(Keys.Up))
                deltaY -= speed * time.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                deltaY += speed * time.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                deltaX -= speed * time.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                deltaX += speed * time.ElapsedGameTime.TotalSeconds;

            int xCheck = (int) (deltaX + Math.Sign(deltaX));
            int yCheck = (int) (deltaY + Math.Sign(deltaY));

            foreach (var chunk in interSectingChunks)
            {
                if (Collide(chunk, xCheck, yCheck))
                {
                    deltaX = 0; deltaY = 0;
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
