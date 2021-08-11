using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using Microsoft.Xna.Framework.Input;

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
            sprite = Game1.loadedTextures["unknown"];
        }
        public Rectangle BoundingBox()
        {
            return hurtBox;
        }

        public void Update(GameTime time)
        {
            var kstate = Keyboard.GetState();
            float deltaY = 0;
            float deltaX = 0;

            if (kstate.IsKeyDown(Keys.Up))
                deltaY -= speed * (float)time.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                deltaY += speed * (float)time.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                deltaX -= speed * (float)time.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                deltaX += speed * (float)time.ElapsedGameTime.TotalSeconds;


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
