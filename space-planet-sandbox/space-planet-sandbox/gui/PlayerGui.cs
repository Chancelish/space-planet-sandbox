using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;
using System.Text;
using space_planet_sandbox.entities.items;

namespace space_planet_sandbox.gui
{
    public class PlayerGui
    {
        private Texture2D hotbarSprite;
        private int selectedItem;
        private InventoryItem[] itemsOnHotbar = new InventoryItem[10];

        public PlayerGui()
        {
            hotbarSprite = SandboxGame.loadedTextures["hotbarframe"];
            selectedItem = 0;
            itemsOnHotbar[1] = new InventoryPickaxe();
            itemsOnHotbar[2] = new InventoryBlock("ground_tiles_and_plants", 1000, 64);
            itemsOnHotbar[3] = new InventoryBlock("sand", 1000, 32);
        }

        public void Render(SpriteBatch graphics, float renderScale)
        {
            for (int i = 0; i < 10; i++)
            {
                var xi = 480 + 32 * i;
                var location = new Vector2(xi * renderScale, 670 * renderScale);
                var color = i == selectedItem ? Color.YellowGreen : Color.White;
                graphics.Draw(hotbarSprite, location, null, color, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
                String label = i == 9 ? "0" : (i + 1).ToString();

                if (null != itemsOnHotbar[i])
                {
                    itemsOnHotbar[i].Render(graphics, xi, 670, renderScale);
                }

                var textLocation = new Vector2((500 + 32 * i) * renderScale, 685 * renderScale);
                graphics.DrawString(SandboxGame.defaultFont, label, textLocation, Color.Black, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
                var textLocation2 = new Vector2((502 + 32 * i) * renderScale, 687 * renderScale);
                graphics.DrawString(SandboxGame.defaultFont, label, textLocation2, Color.Black, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
                var textLocation3 = new Vector2((502 + 32 * i) * renderScale, 685 * renderScale);
                graphics.DrawString(SandboxGame.defaultFont, label, textLocation3, Color.Black, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
                var textLocation4 = new Vector2((500 + 32 * i) * renderScale, 687 * renderScale);
                graphics.DrawString(SandboxGame.defaultFont, label, textLocation4, Color.Black, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
                var textLocation5 = new Vector2((501 + 32 * i) * renderScale, 686 * renderScale);
                graphics.DrawString(SandboxGame.defaultFont, label, textLocation5, Color.White, 0f, Vector2.Zero, renderScale, SpriteEffects.None, 0f);
            }
        }

        public void Update()
        {
            for (int i = 0; i < 10; i++)
            {
                if (InputUtils.GetKeyState("hotbar" + i))
                    selectedItem = i - 1;

                if (InputUtils.LeftMouseClicked)
                {
                    var mouseLocation = Mouse.GetState().Position;
                    var xi = 480 + 32 * i;
                    if (mouseLocation.X > xi && mouseLocation.X < xi + 32 && mouseLocation.Y > 670 && mouseLocation.Y < 702)
                    {
                        selectedItem = i;
                    }
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
            if (selectedItem >= 10)
            {
                selectedItem -= 10;
            }
        }

        public void RemoveFromHotbar(int index)
        {
            itemsOnHotbar[index] = null;
        }

        public InventoryItem GetHighlightedItem()
        {
            return itemsOnHotbar[selectedItem];
        }
    }
}
