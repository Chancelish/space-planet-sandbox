using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using space_planet_sandbox.world;
using space_planet_sandbox.entities.player;

namespace space_planet_sandbox
{
    public class Game1 : Game
    {
        public static Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D gorilla;
        private TileMap tileMap;

        private PlayerCharacter character;

        private ButtonState leftMouseLast = ButtonState.Released;
        private ButtonState leftMouseCurrent = ButtonState.Released;

        private ButtonState rightMouseLast = ButtonState.Released;
        private ButtonState rightMouseCurrent = ButtonState.Released;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            tileMap = new TileMap(60, 30);
            character = new PlayerCharacter(50, 50);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            gorilla = Content.Load<Texture2D>("morshu");
            LoadTexture("unknown");
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

            character.Update(gameTime, tileMap);

            var mouseState = Mouse.GetState();
            leftMouseCurrent = mouseState.LeftButton;
            rightMouseCurrent = mouseState.RightButton;

            var leftMouseClicked = leftMouseCurrent == ButtonState.Pressed && leftMouseLast == ButtonState.Released;
           if (leftMouseClicked)
            {
                tileMap.Update(mouseState.X, mouseState.Y);
            }

            base.Update(gameTime);
            leftMouseLast = leftMouseCurrent;
            rightMouseLast = rightMouseCurrent;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(gorilla, new Vector2(0, 0), Color.White);
            tileMap.Render(_spriteBatch);
            character.Render(_spriteBatch);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
