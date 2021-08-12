using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using Microsoft.Xna.Framework.Input;
using space_planet_sandbox.world;

namespace space_planet_sandbox.entities.player
{
    class PlayerCharacter : IHitBox
    {
        private Rectangle hurtBox;
        private Vector2 location;
        private float speed = 100.0f;
        private Texture2D sprite;

        public PlayerCharacter(int startX, int startY)
        {
            hurtBox = new Rectangle(startX, startY, 16, 32);
            location = new Vector2(startX, startY);
        }
        public override Rectangle BoundingBox()
        {
            return hurtBox;
        }

        public void Update(GameTime time, TileMap world)
        {
            var kstate = Keyboard.GetState();
            float deltaY = 0;
            float deltaX = 0;
            sprite = Game1.loadedTextures["unknown"];

            if (kstate.IsKeyDown(Keys.Up))
                deltaY -= speed * (float)time.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                deltaY += speed * (float)time.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                deltaX -= speed * (float)time.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                deltaX += speed * (float)time.ElapsedGameTime.TotalSeconds;

            if (((IHitBox)this).Collide(world, (int) deltaX, (int) deltaY))
            {
                deltaX = 0; deltaY = 0;
            }

            location.X += deltaX;
            location.Y += deltaY;
            hurtBox.Location = location.ToPoint();
        }

        public void Render(SpriteBatch graphics)
        {
            graphics.Draw(sprite,
                location,
                null,
                Color.White,
                0f,
                new Vector2(sprite.Width / 2, sprite.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);
        }
    }
}
