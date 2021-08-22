using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using space_planet_sandbox.world;
using space_planet_sandbox.entities.player;
using space_planet_sandbox.rendering;

namespace space_planet_sandbox
{
    public class SandboxGame : Game
    {
        public static Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D renderTarget;
        public static Camera camera = new Camera();

        private float renderScale = 1;

        private Texture2D gorilla;

        private World testWorld;

        public SandboxGame()
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

            renderTarget = new RenderTarget2D(GraphicsDevice, 1280, 720);

            testWorld = new World(7, 7, 32);
        }

        private void LoadTexture(string textureName)
        {
            SandboxGame.loadedTextures.Add(textureName, Content.Load<Texture2D>(textureName));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputUtils.PreUdate();

            testWorld.Update(gameTime);

            base.Update(gameTime);

            InputUtils.PostUpdate();
        }

        protected override void Draw(GameTime gameTime)
        {
            renderScale = 1F / (720F / graphics.GraphicsDevice.Viewport.Height);
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, transformMatrix: camera.Transform);
            spriteBatch.Draw(gorilla, new Vector2(0, 0), Color.White);

            testWorld.Render(spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public sealed class InputUtils {

        public static bool LeftMouseClicked { get; private set; }
        public static bool LeftMouseReleased { get; private set; }

        public static bool RightMouseClicked { get; private set; }
        public static bool RightMouseReleased { get; private set; }
        public static bool LeftMouse { get; private set; }
        public static bool RightMouse { get; private set; }
        public static bool LeftMouseLast { get; private set; }
        public static bool RightMouseLast { get; private set; }

        public static void PreUdate()
        {
            var mouseState = Mouse.GetState();
            LeftMouse = mouseState.LeftButton == ButtonState.Pressed;
            RightMouse = mouseState.RightButton == ButtonState.Pressed;
            LeftMouseClicked = LeftMouse && !LeftMouseLast;
            RightMouseClicked = RightMouse && !RightMouseLast;
            LeftMouseReleased = !LeftMouse && LeftMouseLast;
            RightMouseReleased = !RightMouse && RightMouseLast;
        }

        public static void PostUpdate()
        {
            RightMouseLast = RightMouse;
            LeftMouseLast = LeftMouse;
        }
    }
}
