using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox
{
    public sealed class InputUtils
    {

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
            foreach (string key in keyBindings.Keys)
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
