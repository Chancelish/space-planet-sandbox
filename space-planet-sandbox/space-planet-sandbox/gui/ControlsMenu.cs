using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.gui
{
    public class ControlsMenu
    {

        private List<ConfigurableControl> configurableControls;

        private string selectedKeyBinding;
        private bool selectionMode;
        private int controlsIndex;
        private readonly Texture2D backGround;
        private readonly Texture2D closeIcon;

        public bool isOpen;
        private bool mouseOverClose;

        public ControlsMenu(GameWindow window, Texture2D color)
        {
            selectionMode = false;
            selectedKeyBinding = null;

            configurableControls = new List<ConfigurableControl>();
            closeIcon = SandboxGame.loadedTextures["close_icon_v1"];

            window.KeyDown += bindKey;
            backGround = color;

            GetCurrentBindings();
        }

        private void bindKey(Object sender, InputKeyEventArgs eventArgs) {
            if (selectionMode && selectedKeyBinding != null)
            {
                selectionMode = false;
                if (eventArgs.Key != Keys.Escape)
                {
                    InputUtils.SetKeyBinding(selectedKeyBinding, eventArgs.Key);
                    configurableControls[controlsIndex].boundKey = eventArgs.Key;
                }
                selectedKeyBinding = null;
            }
        }

        public void Update()
        {
            if (!selectionMode)
            {
                UpdateMain();
            }
        }

        private void UpdateMain()
        {
            var mousePosition = InputUtils.GetMouseScreenPosition();
            for (int i = 0; i < configurableControls.Count; i++)
            {
                configurableControls[i].Update(mousePosition);
                if (configurableControls[i].Clicked())
                {
                    selectionMode = true;
                    selectedKeyBinding = configurableControls[i].controlLabel;
                    controlsIndex = i;
                    break;
                }
            }
            mouseOverClose = mousePosition.X > 1214 && mousePosition.X < 1246 && mousePosition.Y > 50 && mousePosition.Y < 82;
            if (InputUtils.GetKeyPressed("escape") || (mouseOverClose && InputUtils.LeftMouseClicked)) isOpen = false;
        }

        public void Render(SpriteBatch graphics)
        {
            graphics.Draw(backGround, new Rectangle(32, 48, 1216, 400), Color.DarkBlue);
            graphics.Draw(closeIcon, new Vector2(1214, 50), null, mouseOverClose ? Color.LimeGreen : Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            for (int i = 0; i < configurableControls.Count; i++)
            {
                configurableControls[i].Render(graphics);
            }
            if (selectionMode)
            {
                graphics.Draw(backGround, new Rectangle(160, 96, 1008, 288), Color.Black);
                string instructions = "Press a key to bind it to the '" + selectedKeyBinding + "' action.";
                string noEscape = "Or press 'Escape' to cancel the binding."; 
                graphics.DrawString(TextUtils.dialogFont, instructions, new Vector2(192, 128), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                graphics.DrawString(TextUtils.dialogFont, noEscape, new Vector2(192, 152), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        private void GetCurrentBindings()
        {
            var bindings = InputUtils.getCurrentBindings();
            int iterator = 0;
            foreach (var binding in bindings)
            {
                var configurableControl = new ConfigurableControl(binding);
                configurableControl.SetPosition(64 + 400 * (iterator % 3), 88 + 56 * (iterator / 3));
                configurableControls.Add(configurableControl);
                iterator++;
            }
        }
    }

    class ConfigurableControl
    {
        public string controlLabel;
        private Keys _key;
        private string keyLabel;
        public Keys boundKey {
            get { return _key; }
            set {
                _key = value;
                keyLabel = _key.ToString();
                if (keyLabel.StartsWith('D') && keyLabel.Length == 2)
                {
                    keyLabel = keyLabel[1..];
                }
                if (keyLabel.Equals("OemSemicolon"))
                {
                    keyLabel = ";";
                }
            }
        }
        public int x;
        public int y;

        private bool mousedOver;

        public ConfigurableControl(Tuple<string, Keys> binding)
        {
            controlLabel = binding.Item1;
            boundKey = binding.Item2;
        }

        public void Render(SpriteBatch graphics)
        {
            var color = mousedOver ? Color.LimeGreen : Color.White;
            graphics.Draw(SandboxGame.loadedTextures["menu_frame"], new Vector2(x, y), color);
            graphics.Draw(SandboxGame.loadedTextures["menu_frame_condensed"], new Vector2(x + 240, y), color);
            graphics.DrawString(TextUtils.dialogFont, controlLabel, new Vector2(x + 32, y + 12), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            graphics.DrawString(TextUtils.dialogFont, keyLabel, new Vector2(x + 252, y + 12), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void Update(Point mousePosition)
        {
            mousedOver = (mousePosition.X > x && mousePosition.X < x + 374 && mousePosition.Y > y && mousePosition.Y < y + 48);
        }

        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Clicked()
        {
            return mousedOver && InputUtils.LeftMouseClicked;
        }
    }
}
