using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.gui
{
    public class OptionsMenu
    {
        private string sound = "Sound";
        private string video = "Video";
        private string controls = "Controls";
        private string quit = "Save and Quit";

        private string[] options;

        private readonly Texture2D backGround;
        private readonly Texture2D textFrame;
        private readonly Texture2D closeIcon;
        private readonly ControlsMenu controlsSubMenu;

        private float backgroundX;
        private float backgroundY;

        public bool isOpen { get; private set; }
        private bool keyboardMode;
        private bool justOpened = true;
        private bool mouseOverClose;
        private int mousedOverBox = -1;
        private int keyboardSelect = -1;

        public OptionsMenu(Texture2D texture, ControlsMenu controlsMenu)
        {
            backGround = texture;
            backGround.SetData(new[] { Color.White });
            textFrame = SandboxGame.loadedTextures["menu_frame"];
            closeIcon = SandboxGame.loadedTextures["close_icon_v1"];

            backgroundX = 380;
            backgroundY = 170;

            options = new string[] { video, sound, controls, quit };

            controlsSubMenu = controlsMenu;
        }

        public void Update()
        {
            if (controlsSubMenu.isOpen)
            {
                controlsSubMenu.Update();
                return;
            }
            DetermineMouseOver();
            UpdateBasedOnKeypress();
            CheckClick();
        }

        public void Render(SpriteBatch graphics)
        {
            if (controlsSubMenu.isOpen)
            {
                controlsSubMenu.Render(graphics);
                return;
            }
            graphics.Draw(backGround, new Rectangle((int) backgroundX, (int) backgroundY, 300, 300), Color.DarkBlue);
            var closeIconPosition = new Vector2(backgroundX + 240, backgroundY + 12);
            graphics.Draw(closeIcon, closeIconPosition, null, mouseOverClose ? Color.LimeGreen : Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            for (int i = 0; i < options.Length; i++)
            {
                var color = i == keyboardSelect || i == mousedOverBox ? Color.LimeGreen : Color.White;
                var boxPosition = new Vector2(backgroundX + 32, backgroundY + (44 + 48 * i));
                graphics.Draw(textFrame, boxPosition, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                var textLocation = new Vector2(backgroundX + 60, backgroundY + (56 + 48 * i));
                graphics.DrawString(TextUtils.dialogFont, options[i], textLocation, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        public void Open()
        {
            mousedOverBox = -1;
            keyboardSelect = -1;
            justOpened = true;
            isOpen = true;
        }

        private void DetermineMouseOver()
        {
            var mousePosition = InputUtils.GetMouseScreenPosition();
            var xLocLeft = backgroundX + 32;
            var xLocRight = backgroundX + 240;
            for (int i = 0; i < options.Length; i++)
            {
                var yLocTop = backgroundY + (44 + 48 * i);
                var yLocBottom = backgroundY + (92 + 48 * i);
                if (mousePosition.X > xLocLeft && mousePosition.X < xLocRight && mousePosition.Y > yLocTop && mousePosition.Y < yLocBottom)
                {
                    mousedOverBox = i;
                    return;
                }
            }
            mouseOverClose = mousePosition.X > backgroundX + 240 && mousePosition.Y > backgroundY + 12 && mousePosition.X < backgroundX + 272 && mousePosition.Y < backgroundY + 44;
            mousedOverBox = -1;
        }

        private void CheckClick()
        {
            if (InputUtils.LeftMouseClicked)
            {
                switch (mousedOverBox)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        controlsSubMenu.isOpen = true;
                        break;
                    case 3:
                        SandboxGame.flagToQuit = true;
                        break;
                }
                var mousePosition = InputUtils.GetMouseScreenPosition();
                if (mouseOverClose)
                {
                    keyboardMode = false;
                    isOpen = false;
                    return;
                }
            }
        }

        private void UpdateBasedOnKeypress()
        {
            if (InputUtils.GetKeyPressed("escape") && !justOpened)
            {
                keyboardMode = false;
                isOpen = false;
                return;
            }
            else if (!InputUtils.GetKeyState("escape") && justOpened)
            {
                justOpened = false;
            }
            if (InputUtils.GetKeyPressed("Up") || InputUtils.GetKeyPressed("upArrow"))
            {
                keyboardMode = true;
                keyboardSelect--;
            }
            else if (InputUtils.GetKeyPressed("Down") || InputUtils.GetKeyPressed("downArrow"))
            {
                keyboardMode = true;
                keyboardSelect++;
            }
            if (InputUtils.GetKeyPressed("enter"))
            {
                switch (keyboardSelect)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        controlsSubMenu.isOpen = true;
                        break;
                    case 3:
                        SandboxGame.flagToQuit = true;
                        break;
                }
            }
            if (keyboardSelect < 0 && keyboardMode) keyboardSelect += options.Length;
            if (keyboardSelect >= options.Length && keyboardMode) keyboardSelect -= options.Length;
        }
    }
}
