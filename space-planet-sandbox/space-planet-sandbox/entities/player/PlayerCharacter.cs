using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using Microsoft.Xna.Framework.Input;
using space_planet_sandbox.world;

namespace space_planet_sandbox.entities.player
{
    class PlayerCharacter : CollidableEntity
    {
        private HitBox hurtBox;
        private float speed = 100.0f;
        private Texture2D sprite;

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

        public void Update(GameTime time, TileMap world)
        {
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

            int xCheck = (int) (deltaX > 0 ? Math.Ceiling(deltaX) : Math.Floor(deltaX));
            int yCheck = (int) (deltaY > 0 ? Math.Ceiling(deltaY) : Math.Floor(deltaY));

            if (Collide(world, xCheck, yCheck))
            {
                deltaX = 0; deltaY = 0;
            }

            x += (float) deltaX;
            y += (float) deltaY;
            hurtBox.MoveTo((int) x, (int) y);
        }

        public void Render(SpriteBatch graphics)
        {
            graphics.Draw(sprite,
                new Vector2((float) x, (float) y),
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
