using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using space_planet_sandbox.world;
using space_planet_sandbox.rendering;
using space_planet_sandbox.gui;

namespace space_planet_sandbox
{
    public class SandboxGame : Game
    {
        public static Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();
        public static SpriteFont defaultFont { get; private set; }
        public static SpriteFont dialogFont { get; private set; }
        public static PlayerGui gui { get; private set; }
        public static float renderScale { get; private set; } = 1;

        public static Camera camera = new Camera();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D renderTarget;  

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

            InputUtils.SetDefaultKeys();
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
            LoadTexture("hotbarframe");
            LoadTexture("sand");
            LoadTexture("temppickaxeicon");
            LoadTexture("forbidden_x");
            LoadTexture("menu_frame");

            defaultFont = Content.Load<SpriteFont>("default");
            dialogFont = Content.Load<SpriteFont>("dialog");

            renderTarget = new RenderTarget2D(GraphicsDevice, 1280, 720);
            gui = new PlayerGui(new OptionsMenu(spriteBatch));
            testWorld = new World(7, 7, 32);
        }

        private void LoadTexture(string textureName)
        {
            SandboxGame.loadedTextures.Add(textureName, Content.Load<Texture2D>(textureName));
        }

        protected override void Update(GameTime gameTime)
        {
            InputUtils.PreUdate();

            testWorld.Update(gameTime);
            gui.Update();

            base.Update(gameTime);

            InputUtils.PostUpdate();
        }

        protected override void Draw(GameTime gameTime)
        {
            renderScale = 1F / (720F / graphics.GraphicsDevice.Viewport.Height);
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, transformMatrix: camera.Transform);
            spriteBatch.Draw(gorilla, Vector2.Zero, Color.White);

            testWorld.Render(spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
            gui.Render(spriteBatch);
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
        public static int previousScrollWheelValue { get; private set; }

        private static Dictionary<string, Keys> keyBindings = new Dictionary<string, Keys>();
        private static Dictionary<string, bool> boundKeyStates = new Dictionary<string, bool>();
        private static Dictionary<string, bool> boundKeyPrevious = new Dictionary<string, bool>();

        public static void PreUdate()
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            LeftMouse = mouseState.LeftButton == ButtonState.Pressed;
            RightMouse = mouseState.RightButton == ButtonState.Pressed;
            LeftMouseClicked = LeftMouse && !LeftMouseLast;
            RightMouseClicked = RightMouse && !RightMouseLast;
            LeftMouseReleased = !LeftMouse && LeftMouseLast;
            RightMouseReleased = !RightMouse && RightMouseLast;
            foreach(string key in keyBindings.Keys)
            {
                boundKeyStates[key] = keyboardState.IsKeyDown(keyBindings[key]);
            }
        }

        public static void PostUpdate()
        {
            RightMouseLast = RightMouse;
            LeftMouseLast = LeftMouse;
            previousScrollWheelValue = Mouse.GetState().ScrollWheelValue;
            foreach (string key in keyBindings.Keys)
            {
                boundKeyPrevious[key] = boundKeyStates[key];
            }
        }

        public static bool GetKeyState(string keyBinding)
        {
            if (boundKeyStates.ContainsKey(keyBinding))
            {
                return boundKeyStates[keyBinding];
            }
            return false;
        }

        public static bool GetLastFrameKeyState(string keyBinding)
        {
            if (boundKeyPrevious.ContainsKey(keyBinding))
            {
                return boundKeyPrevious[keyBinding];
            }
            return false;
        }

        public static bool GetKeyPressed(string keyBinding)
        {
            return GetKeyState(keyBinding) && !GetLastFrameKeyState(keyBinding);
        }

        public static void SetDefaultKeys()
        {
            keyBindings.Clear();
            keyBindings.Add("directionUp", Keys.W);
            keyBindings.Add("directionLeft", Keys.A);
            keyBindings.Add("directionDown", Keys.S);
            keyBindings.Add("directionRight", Keys.D);
            keyBindings.Add("hotbar1", Keys.D1);
            keyBindings.Add("hotbar2", Keys.D2);
            keyBindings.Add("hotbar3", Keys.D3);
            keyBindings.Add("hotbar4", Keys.D4);
            keyBindings.Add("hotbar5", Keys.D5);
            keyBindings.Add("hotbar6", Keys.D6);
            keyBindings.Add("hotbar7", Keys.D7);
            keyBindings.Add("hotbar8", Keys.D8);
            keyBindings.Add("hotbar9", Keys.D9);
            keyBindings.Add("hotbar0", Keys.D0);
            // Reserved Keys, do not allow remapping
            keyBindings.Add("escape", Keys.Escape);
            keyBindings.Add("enter", Keys.Enter);
            keyBindings.Add("upArrow", Keys.Up);
            keyBindings.Add("downArrow", Keys.Down);
            keyBindings.Add("leftArrow", Keys.Left);
            keyBindings.Add("rightArrow", Keys.Right);
            boundKeyStates.Clear();
            boundKeyPrevious.Clear();
            foreach (string key in keyBindings.Keys)
            {
                boundKeyStates.Add(key, false);
                boundKeyPrevious.Add(key, false);
            }
        }
    }
}
