using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using space_planet_sandbox.world;

namespace space_planet_sandbox
{
    public class Game1 : Game
    {
        public static Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D gorilla;
        private Texture2D character;
        private TileMap tileMap;
        Vector2 characterPosition;
        float speed;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            characterPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
_graphics.PreferredBackBufferHeight / 2);
            speed = 100.0f;

            tileMap = new TileMap(120, 60);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            gorilla = Content.Load<Texture2D>("morshu");
            character = Content.Load<Texture2D>("unknown");
            LoadTexture("ground_tiles_and_plants");
        }

        private void LoadTexture(string textureName)
        {
            Game1.loadedTextures.Add(textureName, Content.Load<Texture2D>(textureName));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
                characterPosition.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                characterPosition.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                characterPosition.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                characterPosition.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(gorilla, new Vector2(0, 0), Color.White);
            tileMap.Render(_spriteBatch);
            _spriteBatch.Draw(character,
                characterPosition,
                null,
                Color.White,
                0f,
                new Vector2(character.Width / 2, character.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
