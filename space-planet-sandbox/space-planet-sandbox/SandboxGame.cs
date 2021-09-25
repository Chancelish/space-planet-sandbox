﻿using Microsoft.Xna.Framework;
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
            LoadTexture("menu_frame_condensed");

            defaultFont = Content.Load<SpriteFont>("default");
            dialogFont = Content.Load<SpriteFont>("dialog");

            actionLayer = new RenderTarget2D(GraphicsDevice, 1280, 720);
            guiLayer = new RenderTarget2D(GraphicsDevice, 1280, 720);
            testWorld = new World(70, 20, 32);
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
        private static HashSet<string> reservedKeys = new HashSet<string>();
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

        public static Point GetMouseWorldPosition()
        {
            var mousePosition = Mouse.GetState().Position;
            int realMouseX = (int)(mousePosition.X / SandboxGame.renderScale) + SandboxGame.camera.x;
            int realMouseY = (int)(mousePosition.Y / SandboxGame.renderScale) + SandboxGame.camera.y;
            return new Point(realMouseX, realMouseY);
        }

        public static Point GetMouseScreenPosition()
        {
            var mousePosition = Mouse.GetState().Position;
            int realMouseX = (int)(mousePosition.X / SandboxGame.renderScale);
            int realMouseY = (int)(mousePosition.Y / SandboxGame.renderScale);
            return new Point(realMouseX, realMouseY);
        }

        public static void SetDefaultKeys()
        {
            keyBindings.Clear();
            keyBindings.Add("Up", Keys.W);
            keyBindings.Add("Left", Keys.A);
            keyBindings.Add("Down", Keys.S);
            keyBindings.Add("Right", Keys.D);
            keyBindings.Add("Jump", Keys.Space);
            keyBindings.Add("Inventory", Keys.E);
            keyBindings.Add("Hotbar 1", Keys.D1);
            keyBindings.Add("Hotbar 2", Keys.D2);
            keyBindings.Add("Hotbar 3", Keys.D3);
            keyBindings.Add("Hotbar 4", Keys.D4);
            keyBindings.Add("Hotbar 5", Keys.D5);
            keyBindings.Add("Hotbar 6", Keys.D6);
            keyBindings.Add("Hotbar 7", Keys.D7);
            keyBindings.Add("Hotbar 8", Keys.D8);
            keyBindings.Add("Hotbar 9", Keys.D9);
            keyBindings.Add("Hotbar 0", Keys.D0);
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
            reservedKeys.Add("escape");
            reservedKeys.Add("enter");
            reservedKeys.Add("upArrow");
            reservedKeys.Add("downArrow");
            reservedKeys.Add("leftArrow");
            reservedKeys.Add("rightArrow");
        }

        public static void SetKeyBinding(string binding, Keys key)
        {
            if (reservedKeys.Contains(binding))
            {
                return;
            }
            if (keyBindings.ContainsKey(binding))
            {
                keyBindings[binding] = key;
            }
            else
            {
                keyBindings.Add(binding, key);
            }
        }

        public static List<Tuple<string, Keys>> getCurrentBindings()
        {
            List<Tuple<string, Keys>> currentBindings = new List<Tuple<string, Keys>>();
            currentBindings.Add(new Tuple<string, Keys>("Up", keyBindings["Up"]));
            currentBindings.Add(new Tuple<string, Keys>("Left", keyBindings["Left"]));
            currentBindings.Add(new Tuple<string, Keys>("Right", keyBindings["Right"]));
            currentBindings.Add(new Tuple<string, Keys>("Down", keyBindings["Down"]));
            currentBindings.Add(new Tuple<string, Keys>("Jump", keyBindings["Jump"]));
            currentBindings.Add(new Tuple<string, Keys>("Inventory", keyBindings["Inventory"]));
            currentBindings.Add(new Tuple<string, Keys>("Hotbar 1", keyBindings["Hotbar 1"]));
            currentBindings.Add(new Tuple<string, Keys>("Hotbar 2", keyBindings["Hotbar 2"]));
            currentBindings.Add(new Tuple<string, Keys>("Hotbar 3", keyBindings["Hotbar 3"]));
            currentBindings.Add(new Tuple<string, Keys>("Hotbar 4", keyBindings["Hotbar 4"]));
            currentBindings.Add(new Tuple<string, Keys>("Hotbar 5", keyBindings["Hotbar 5"]));
            currentBindings.Add(new Tuple<string, Keys>("Hotbar 6", keyBindings["Hotbar 6"]));
            currentBindings.Add(new Tuple<string, Keys>("Hotbar 7", keyBindings["Hotbar 7"]));
            currentBindings.Add(new Tuple<string, Keys>("Hotbar 8", keyBindings["Hotbar 8"]));
            currentBindings.Add(new Tuple<string, Keys>("Hotbar 9", keyBindings["Hotbar 9"]));
            currentBindings.Add(new Tuple<string, Keys>("Hotbar 0", keyBindings["Hotbar 0"]));
            return currentBindings;
        }
    }
}
