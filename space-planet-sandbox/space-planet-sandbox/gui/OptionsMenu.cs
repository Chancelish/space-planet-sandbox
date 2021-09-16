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
        private string video = "video";
        private string controls = "controls";

        private Texture2D backGround;
        private Texture2D textFrame;

        public OptionsMenu(SpriteBatch graphics)
        {
            backGround = new Texture2D(graphics.GraphicsDevice, 1, 1);
            backGround.SetData(new[] { Color.White });
            textFrame = SandboxGame.loadedTextures["menu_frame"];
        }

        public void Update()
        {

        }

        public void Render(SpriteBatch graphics)
        {

        }
    }
}
