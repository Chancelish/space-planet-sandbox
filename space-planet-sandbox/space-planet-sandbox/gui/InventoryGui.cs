using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using space_planet_sandbox.entities.items;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using space_planet_sandbox.entities.player;

namespace space_planet_sandbox.gui
{
    public class InventoryGui
    {

        private Texture2D background;
        private PlayerInventory inventory;
        private PlayerCharacter player;
        
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

        public void Open(PlayerInventory playerInventory, PlayerCharacter activePlayer)
        {
            isOpen = true;
            player = activePlayer;
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
            clicked = false;
            inventory.UpdateHotbar();
            if (isOpen)
            {
                UpdateInternal();
            }
            if (InputUtils.LeftMouse)
            {
                var mousePosition = InputUtils.GetMouseScreenPosition();
                clicked = MouseInItemGrid(mousePosition) || MouseOverHotbar(mousePosition) || grabbedIndex != -1;
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
                    int _y = (int) (mousePosition.Y - (position.Y + 16)) / 32;
                    grabbedIndex = _x + 10 * _y;
                    grabbedItem = inventory.GetItemAt(grabbedIndex);
                }
            }
            if (InputUtils.LeftMouseReleased && grabbedIndex != -1)
            {
                if (MouseInItemGrid(mousePosition))
                {
                    int _x = (int)(mousePosition.X - position.X) / 32;
                    int _y = (int)(mousePosition.Y - (position.Y + 16)) / 32;
                    var releasedIndex = _x + 10 * _y;
                    if (releasedIndex != grabbedIndex) inventory.Swap(inventory.selectedTab, grabbedIndex, releasedIndex);
                }
                for (int i = 0; i < 10; i++)
                {
                    var xi = (480 + 32 * i);
                    if (mousePosition.X > xi && mousePosition.X < xi + 32 && mousePosition.Y > 660 && mousePosition.Y < 692) inventory.PlaceOnHotbar(i, grabbedItem);
                }
                grabbedItem = null;
                grabbedIndex = -1;
            }
            if (InputUtils.RightMouseClicked && grabbedIndex != -1)
            {
                player.DropItem(grabbedItem);
                inventory.Remove(grabbedItem.tab, grabbedIndex);
                inventory.RemoveFromHotbar(grabbedItem);
                grabbedItem = null;
            }
            if (InputUtils.GetKeyPressed("escape")) Close();
        }

        private bool MouseInItemGrid(Point mousePosition)
        {
            return mousePosition.X > position.X && mousePosition.Y > position.Y + 16 && mousePosition.X < position.X + 320 && mousePosition.Y < position.Y + 208 && isOpen;
        }

        private bool MouseOverHotbar(Point mousePosition)
        {
            return mousePosition.X > 480 && mousePosition.Y > 660 && mousePosition.X < 800 && mousePosition.Y < 692;
        }
    }
}
