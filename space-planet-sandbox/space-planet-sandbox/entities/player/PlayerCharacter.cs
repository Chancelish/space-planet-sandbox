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

        private Texture2D boxOutline;

        public PlayerCharacter(int startX, int startY)
        {
            hurtBox = new HitBox(startX, startY, 16, 32);
            sprite = SandboxGame.loadedTextures["unknown"];
            x = startX;
            y = startY;
            collisionGroup = "player";
        }
        public override ICollisionMask GetCollisionMask()
        {
            return hurtBox;
        }

        public override void Update(GameTime time)
        {
            SandboxGame.camera.Follow(this, 1280, 720);

            var possibleCollisions = myWorld.GetPotentialCollisions((int) x, (int) y, hurtBox.Size().X, hurtBox.Size().Y);
            
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

            if (possibleCollisions.ContainsKey("tiles"))
            {
                foreach (var chunk in possibleCollisions["tiles"])
                {
                    if (Collide(chunk, xCheck, 0))
                    {
                        while (xCheck != 0)
                        {
                            xCheck = xCheck > 0 ? xCheck - 1 : xCheck + 1;
                            deltaX = xCheck;
                            if (!Collide(chunk, xCheck, 0))
                            {
                                break;
                            }
                        }
                        break;
                    }
                }
                foreach (var chunk in possibleCollisions["tiles"])
                {
                    if (Collide(chunk, 0, yCheck))
                    {
                        while (yCheck != 0)
                        {
                            yCheck = yCheck > 0 ? yCheck - 1 : yCheck + 1;
                            deltaY = yCheck;
                            if (!Collide(chunk, 0, yCheck))
                            {
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            x += (float) deltaX;
            y += (float) deltaY;
            hurtBox.MoveTo((int) x, (int) y);
        }

        public override void Render(SpriteBatch graphics)
        {
            if (boxOutline == null)
            {
                boxOutline = new Texture2D(graphics.GraphicsDevice, 1, 1);
                boxOutline.SetData(new[] { Color.White });
            }
            graphics.Draw(boxOutline, new Rectangle((int) x, (int) y, 16, 32), Color.LightGreen);

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

        public override Point GetSize()
        {
            return hurtBox.Size();
        }
    }
}
