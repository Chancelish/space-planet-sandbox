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

            options = new string[] { video, sound, controls, quit };
        }

        public void Update()
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
        }

        public void Render(SpriteBatch graphics)
        {
            var backgroundX = 380 * SandboxGame.renderScale;
            var backgroundY = 170 * SandboxGame.renderScale;
            graphics.Draw(backGround, new Rectangle((int) backgroundX, (int) backgroundY, (int) (300 * SandboxGame.renderScale), (int) (300 * SandboxGame.renderScale)), Color.DarkBlue);
            var closeIconPosition = new Vector2(backgroundX + 240 * SandboxGame.renderScale, backgroundY + 12 * SandboxGame.renderScale);
            graphics.Draw(closeIcon, closeIconPosition, null, Color.White, 0f, Vector2.Zero, SandboxGame.renderScale, SpriteEffects.None, 0f);
            for (int i = 0; i < options.Length; i++)
            {
                var boxPosition = new Vector2(backgroundX + 32 * SandboxGame.renderScale, backgroundY + (44 + 48 * i) * SandboxGame.renderScale);
                graphics.Draw(textFrame, boxPosition, null, Color.White, 0f, Vector2.Zero, SandboxGame.renderScale, SpriteEffects.None, 0f);
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
    }
}
