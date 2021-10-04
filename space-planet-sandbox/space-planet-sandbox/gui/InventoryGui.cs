using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using space_planet_sandbox.entities.items;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace space_planet_sandbox.gui
{
    public class InventoryGui
    {

        private Texture2D background;
        private PlayerInventory inventory;
        
        private Vector2 position;

        private int grabbedIndex;
        private InventoryItem grabbedItem = null;
        
        public bool clicked { get; private set; }

        public bool isOpen { get; private set; }

        public InventoryGui()
        {
            background = SandboxGame.loadedTextures["inventory_core"];
            
            position = new Vector2(480, 430);
            
            grabbedIndex = -1;
            
        }

        public void Open(PlayerInventory playerInventory)
        {
            isOpen = true;
            SetInventory(playerInventory);
        }

        public void SetInventory(PlayerInventory playerInventory)
        {
            inventory = playerInventory;
        }

        public void Close()
        {
            isOpen = false;
        }

        public void Update()
        {
            inventory.UpdateHotbar();
            if (isOpen)
            {
                UpdateInternal();
            }
        }

        public void Render(SpriteBatch graphics)
        {
            inventory.RenderHotbar(graphics);
            if (isOpen)
            {
                graphics.Draw(background, position, null, Color.White);
                inventory.RenderInventory(graphics, (int) position.X, (int) position.Y);
            }
            if (grabbedItem != null)
            {
                grabbedItem.Render(graphics, InputUtils.GetMouseScreenPosition().X, InputUtils.GetMouseScreenPosition().Y);
            }
        }

        public InventoryItem GetHighlightedItem()
        {
            return inventory.GetHighlightedItem();
        }

        private void UpdateInternal()
        {
            var mousePosition = InputUtils.GetMouseScreenPosition();
            if (InputUtils.LeftMouseClicked)
            {
                if (MouseInItemGrid(mousePosition))
                {
                    int _x = (int) (mousePosition.X - position.X) / 32;
                    int _y = (int) (mousePosition.Y - position.Y) / 32;
                    grabbedIndex = _x + 10 * _y;
                    grabbedItem = inventory.GetItemAt(grabbedIndex);
                    clicked = true;
                }
            }
            if (InputUtils.LeftMouseReleased)
            {
                if (MouseInItemGrid(mousePosition))
                {
                    int _x = (int)(mousePosition.X - position.X) / 32;
                    int _y = (int)(mousePosition.Y - position.Y) / 32;
                    var releasedIndex = _x + 10 * _y;
                    if (releasedIndex != grabbedIndex) inventory.Swap(inventory.selectedTab, grabbedIndex, releasedIndex);
                }
                grabbedItem = null;
            }
            if (InputUtils.GetKeyPressed("escape")) Close();
        }

        private bool MouseInItemGrid(Point mousePosition)
        {
            return mousePosition.X > position.X && mousePosition.Y > position.Y + 16 && mousePosition.X < position.X + 320 && mousePosition.Y < position.Y + 208;
        }
    }
}
