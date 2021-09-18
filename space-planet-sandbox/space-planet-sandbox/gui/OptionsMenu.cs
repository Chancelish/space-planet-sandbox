using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        private Texture2D backGround;
        private Texture2D textFrame;
        private Texture2D closeIcon;

        private float backgroundX;
        private float backgroundY;

        public bool isOpen { get; private set; }
        private bool keyboardMode;
        private bool justOpened = true;
        private int mousedOverBox = -1;
        private int keyboardSelect = -1;

        public OptionsMenu(SpriteBatch graphics)
        {
            backGround = new Texture2D(graphics.GraphicsDevice, 1, 1);
            backGround.SetData(new[] { Color.White });
            textFrame = SandboxGame.loadedTextures["menu_frame"];
            closeIcon = SandboxGame.loadedTextures["close_icon_v1"];

            backgroundX = 380 * SandboxGame.renderScale;
            backgroundY = 170 * SandboxGame.renderScale;

            options = new string[] { video, sound, controls, quit };
        }

        public void Update()
        {
            UpdateBasedOnMousePosition();
            UpdateBasedOnKeypress();
        }

        public void Render(SpriteBatch graphics)
        {
            graphics.Draw(backGround, new Rectangle((int) backgroundX, (int) backgroundY, (int) (300 * SandboxGame.renderScale), (int) (300 * SandboxGame.renderScale)), Color.DarkBlue);
            var closeIconPosition = new Vector2(backgroundX + 240 * SandboxGame.renderScale, backgroundY + 12 * SandboxGame.renderScale);
            graphics.Draw(closeIcon, closeIconPosition, null, Color.White, 0f, Vector2.Zero, SandboxGame.renderScale, SpriteEffects.None, 0f);
            for (int i = 0; i < options.Length; i++)
            {
                var color = i == keyboardSelect || i == mousedOverBox ? Color.LimeGreen : Color.White;
                var boxPosition = new Vector2(backgroundX + 32 * SandboxGame.renderScale, backgroundY + (44 + 48 * i) * SandboxGame.renderScale);
                graphics.Draw(textFrame, boxPosition, null, color, 0f, Vector2.Zero, SandboxGame.renderScale, SpriteEffects.None, 0f);
                var textLocation = new Vector2(backgroundX + 60 * SandboxGame.renderScale, backgroundY + (52 + 48 * i) * SandboxGame.renderScale);
                graphics.DrawString(SandboxGame.dialogFont, options[i], textLocation, Color.White, 0f, Vector2.Zero, SandboxGame.renderScale, SpriteEffects.None, 0f);
            }
        }

        public void Open()
        {
            mousedOverBox = -1;
            keyboardSelect = -1;
            justOpened = true;
            isOpen = true;
        }

        private void UpdateBasedOnMousePosition()
        {
            var mousePosition = Mouse.GetState().Position;
            var xLocLeft = backgroundX + 32 * SandboxGame.renderScale;
            var xLocRight = backgroundX + 240 * SandboxGame.renderScale;
            for (int i = 0; i < options.Length; i++)
            {
                var yLocTop = backgroundY + (44 + 48 * i) * SandboxGame.renderScale;
                var yLocBottom = backgroundY + (92 + 48 * i) * SandboxGame.renderScale;
                if (mousePosition.X > xLocLeft && mousePosition.X < xLocRight && mousePosition.Y > yLocTop && mousePosition.Y < yLocBottom)
                {
                    mousedOverBox = i;
                    return;
                }
            }
            mousedOverBox = -1;
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
            if (InputUtils.GetKeyPressed("directionUp") || InputUtils.GetKeyPressed("upArrow"))
            {
                keyboardMode = true;
                keyboardSelect--;
            }
            else if (InputUtils.GetKeyPressed("directionDown") || InputUtils.GetKeyPressed("downArrow"))
            {
                keyboardMode = true;
                keyboardSelect++;
            }
            if (keyboardSelect < 0 && keyboardMode) keyboardSelect += options.Length;
            if (keyboardSelect >= options.Length && keyboardMode) keyboardSelect -= options.Length;
        }
    }
}
