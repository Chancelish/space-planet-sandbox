using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using space_planet_sandbox.world;
using space_planet_sandbox.rendering;
using space_planet_sandbox.gui;
using System;

namespace space_planet_sandbox
{
    public class SandboxGame : Game
    {
        public static Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();
        public static SpriteFont defaultFont { get; private set; }
        public static SpriteFont dialogFont { get; private set; }
        public static SpriteFont smolFont { get; private set; }
        public static PlayerGui gui { get; private set; }

        public static bool flagToQuit;
        public static float renderScale { get; private set; } = 1;

        public static Camera camera = new Camera();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D actionLayer;
        private RenderTarget2D guiLayer;

        private Texture2D gorilla;

        private World testWorld;

        public SandboxGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
        }

        protected override void Initialize()
        {
            base.Initialize();

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            InputUtils.SetDefaultKeys();

            var menuBackDrop = new Texture2D(graphics.GraphicsDevice, 1, 1);

            gui = new PlayerGui(new OptionsMenu(menuBackDrop, new ControlsMenu(Window, menuBackDrop)));
            testWorld = new World(70, 20, 32);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TileDataDictionary.LoadTiles();

            // TODO: use this.Content to load your game content here
            gorilla = Content.Load<Texture2D>("pink_mountain_bg");
            LoadTexture("unknown");
            LoadTexture("ground_tiles_and_plants");
            LoadTexture("close_icon_v1");
            LoadTexture("inventory_core");
            LoadTexture("hotbarframe");
            LoadTexture("sand");
            LoadTexture("temppickaxeicon");
            LoadTexture("forbidden_x");
            LoadTexture("menu_frame");
            LoadTexture("menu_frame_condensed");

            defaultFont = Content.Load<SpriteFont>("default");
            dialogFont = Content.Load<SpriteFont>("dialog");
            smolFont = Content.Load<SpriteFont>("smolprint");

            actionLayer = new RenderTarget2D(GraphicsDevice, 1280, 720);
            guiLayer = new RenderTarget2D(GraphicsDevice, 1280, 720);
        }

        private void LoadTexture(string textureName)
        {
            SandboxGame.loadedTextures.Add(textureName, Content.Load<Texture2D>(textureName));
        }

        protected override void Update(GameTime gameTime)
        {
            if (flagToQuit) Exit();
            
            InputUtils.PreUdate();

            gui.Update();
            testWorld.Update(gameTime);

            base.Update(gameTime);

            InputUtils.PostUpdate();
        }

        protected override void Draw(GameTime gameTime)
        {
            renderScale = 1F / (720F / graphics.GraphicsDevice.Viewport.Height);
            GraphicsDevice.SetRenderTarget(actionLayer);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, transformMatrix: camera.Transform);
            int x1 = FindParallaxPosition(camera.x, 4);
            int x2 = FindParallaxPosition(camera.x, 4, 1280);
            spriteBatch.Draw(gorilla, new Vector2(x1, camera.y / 4), Color.White);
            spriteBatch.Draw(gorilla, new Vector2(x2, camera.y / 4), Color.White);

            testWorld.Render(spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(guiLayer);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap);
            spriteBatch.Draw(actionLayer, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            gui.Render(spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            
            spriteBatch.Draw(guiLayer, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private int FindParallaxPosition(int cameraX, float ratio, int displacement = 0)
        {
            int x1 = (int) (camera.x / ratio) + displacement;
            while (x1 > camera.x + 1280)
            {
                x1 -= 2560;
            }
            while (x1 < camera.x - 1280)
            {
                x1 += 2560;
            }
            return x1;
        }

        private void OnResize(Object sender, EventArgs e)
        {

        }
    }
}
