using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace space_planet_sandbox.gui
{
    class PlayerGui
    {
        private Texture2D hotbarSprite;
        private int selectedItem;

        public PlayerGui()
        {
            hotbarSprite = SandboxGame.loadedTextures["hotbarframe"];
            selectedItem = 0;
        }

        public void Render(SpriteBatch graphics, float renderScale)
        {
            for (int i = 0; i < 10; i++)
            {
                var location = new Vector2((480 + 32 * i) * renderScale, 670 * renderScale);
                var color = i == selectedItem ? Color.YellowGreen : Color.White;
                graphics.Draw(hotbarSprite, location, null, color, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
                String label = i == 9 ? "0" : (i + 1).ToString();
                var textLocation = new Vector2((500 + 32 * i) * renderScale, 685 * renderScale);
                graphics.DrawString(SandboxGame.defaultFont, label, textLocation, Color.Black, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
                var textLocation2 = new Vector2((502 + 32 * i) * renderScale, 687 * renderScale);
                graphics.DrawString(SandboxGame.defaultFont, label, textLocation2, Color.Black, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
                var textLocation3 = new Vector2((501 + 32 * i) * renderScale, 686 * renderScale);
                graphics.DrawString(SandboxGame.defaultFont, label, textLocation, Color.White, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
            }
        }

        public void Update()
        {
            for (int i = 0; i < 10; i++)
            {
                if (InputUtils.GetKeyState("hotbar" + i))
                {
                    selectedItem = i - 1;
                }
            }

            if (InputUtils.previousScrollWheelValue > Mouse.GetState().ScrollWheelValue)
            {
                selectedItem++;
            }
            else if (InputUtils.previousScrollWheelValue < Mouse.GetState().ScrollWheelValue)
            {
                selectedItem--;
            }

            if (selectedItem < 0)
            {
                selectedItem += 10;
            }
            if (selectedItem > 10)
            {
                selectedItem -= 10;
            }
        }
    }
}
