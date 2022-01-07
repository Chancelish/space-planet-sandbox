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
        public static PlayerGui gui { get; private set; }

        public static bool flagToQuit;
        public static float renderScale { get; private set; } = 1;

        public static Camera camera = new Camera();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D actionLayer;
        private RenderTarget2D guiLayer;

        private static World activeWorld;

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
            activeWorld = new World(70, 20, 32);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TileDataDictionary.LoadTiles();

            // TODO: use this.Content to load your game content here
            LoadTexture("unknown");
            LoadTexture("dirt");
            LoadTexture("close_icon_v1");
            LoadTexture("inventory_core");
            LoadTexture("hotbarframe");
            LoadTexture("sand");
            LoadTexture("temppickaxeicon");
            LoadTexture("forbidden_x");
            LoadTexture("menu_frame");
            LoadTexture("menu_frame_condensed");
            LoadTexture("mining_overlay");
            LoadTexture("mountain_1");
            LoadTexture("small_ship");
            LoadTexture("to_planet");
            LoadTexture("to_ship");

            var whitePixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            whitePixel.SetData(new[] { Color.White });

            loadedTextures.Add("white_pixel", whitePixel);

            TextUtils.LoadFonts(Content);

            actionLayer = new RenderTarget2D(GraphicsDevice, 1280, 720);
            guiLayer = new RenderTarget2D(GraphicsDevice, 1280, 720);
        }

        public static void GoToWorld(World world)
        {
            activeWorld = world;
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
            activeWorld.Update(gameTime);

            base.Update(gameTime);

            InputUtils.PostUpdate();
        }

        protected override void Draw(GameTime gameTime)
        {
            renderScale = 1F / (720F / graphics.GraphicsDevice.Viewport.Height);
            GraphicsDevice.SetRenderTarget(actionLayer);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, transformMatrix: camera.Transform);

            activeWorld.Render(spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(guiLayer);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(actionLayer, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            gui.Render(spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);
            
            spriteBatch.Draw(guiLayer, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void OnResize(Object sender, EventArgs e)
        {

        }
    }
}
