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

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D renderTarget;

        private float renderScale = 1;

        public float RenderScale
        {
            get => renderScale;
            set
            {
                if (value > 0 && value < 10) {
                    renderScale = value;
                }
            }
        }

        private Texture2D gorilla;
        private TileMap tileMap;

        private PlayerCharacter character;

        private ButtonState leftMouseLast = ButtonState.Released;
        private ButtonState leftMouseCurrent = ButtonState.Released;

        private ButtonState rightMouseLast = ButtonState.Released;
        private ButtonState rightMouseCurrent = ButtonState.Released;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            gorilla = Content.Load<Texture2D>("morshu");
            LoadTexture("unknown");
            LoadTexture("ground_tiles_and_plants");

            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);

            tileMap = new TileMap(60, 30);
            character = new PlayerCharacter(50, 50);
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
                tileMap.Update((int) (mouseState.X / RenderScale), (int) (mouseState.Y / RenderScale));
            }

            base.Update(gameTime);
            leftMouseLast = leftMouseCurrent;
            rightMouseLast = rightMouseCurrent;
        }

        protected override void Draw(GameTime gameTime)
        {
            RenderScale = 1F / (1080F / graphics.GraphicsDevice.Viewport.Width);
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap);
            spriteBatch.Draw(gorilla, new Vector2(0, 0), Color.White);
            tileMap.Render(spriteBatch);
            character.Render(spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, RenderScale, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
