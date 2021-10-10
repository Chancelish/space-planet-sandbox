using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.entities.items
{
    public enum InventoryTab
    {
        Equipment,
        Furniture,
        Blocks,
        Consumables,
        Materials
    }

    public class PlayerInventory
    {
        public readonly int slotCount = 60;
        public InventoryTab selectedTab = InventoryTab.Equipment;

        private Dictionary<InventoryTab, InventoryItem[]> inventorySlots = new Dictionary<InventoryTab, InventoryItem[]>();

        private InventoryItem[] itemsOnHotbar = new InventoryItem[10];
        private int selectedItem;
        private Texture2D hotbarSprite;

        private int hotbarX = 480;
        private int hotbarY = 660;

        public PlayerInventory()
        {
            inventorySlots.Add(InventoryTab.Equipment, new InventoryItem[60]);
            inventorySlots.Add(InventoryTab.Furniture, new InventoryItem[60]);
            inventorySlots.Add(InventoryTab.Blocks, new InventoryItem[60]);
            inventorySlots.Add(InventoryTab.Consumables, new InventoryItem[60]);
            inventorySlots.Add(InventoryTab.Materials, new InventoryItem[60]);
            // TODO: Load from file.
            hotbarSprite = SandboxGame.loadedTextures["hotbarframe"];
            selectedItem = 0;
            inventorySlots[InventoryTab.Equipment][1] = itemsOnHotbar[1] = new InventoryPickaxe();
            inventorySlots[InventoryTab.Equipment][2] = itemsOnHotbar[2] = new InventoryBlock("ground_tiles_and_plants", 1000, 64);
            inventorySlots[InventoryTab.Equipment][3] = itemsOnHotbar[3] = new InventoryBlock("sand", 1000, 32);
        }

        public bool IsFull(InventoryTab tab)
        {
            for (int i = 0; i < inventorySlots[tab].Length; i++)
            {
                if (inventorySlots[tab][i] == null) return false;
            }
            return true;
        }

        public InventoryItem GetItemAt(int index)
        {
            return inventorySlots[selectedTab][index];
        }

        public InventoryItem GetItemAt(InventoryTab tab, int index)
        {
            return inventorySlots[tab][index];
        }

        public void Remove(InventoryTab tab, int index)
        {
            inventorySlots[tab][index] = null;
        }

        public bool Add(InventoryItem item)
        {
            int slot = FindFirstEmpty(item.tab);
            if (slot == -1) return false;
            inventorySlots[item.tab][slot] = item;
            return true;
        }

        public void Swap(InventoryTab tab, int index1, int index2)
        {
            var temp = inventorySlots[tab][index1];
            inventorySlots[tab][index1] = inventorySlots[tab][index2];
            inventorySlots[tab][index2] = temp;
        }

        public int FindFirstEmpty(InventoryTab tab)
        {
            for (int i = 0; i <= inventorySlots[tab].Length; i++)
            {
                if (inventorySlots[tab][i] == null) return i;
            }
            return -1;
        }

        public void RenderInventory(SpriteBatch graphics, int x, int y)
        {
            for (int i = 0; i < inventorySlots[selectedTab].Length; i++)
            {
                if (inventorySlots[selectedTab][i] != null) inventorySlots[selectedTab][i].Render(graphics, x + 32 * (i % 10), y + 16 + 32 * (i / 10));
            }
        }

        public void RemoveFromHotbar(int index)
        {
            itemsOnHotbar[index] = null;
        }

        public void RemoveFromHotbar(InventoryItem item)
        {
            for (int i = 0; i < itemsOnHotbar.Length; i++)
            {
                if (item == itemsOnHotbar[i]) itemsOnHotbar[i] = null;
            }
        }

        public void PlaceOnHotbar(int index, InventoryItem usable)
        {
            if (usable == null) return;
            if (usable.usable) itemsOnHotbar[index] = usable; 
        }

        public InventoryItem GetHighlightedItem()
        {
            return itemsOnHotbar[selectedItem];
        }

        public void RenderHotbar(SpriteBatch graphics)
        {
            for (int i = 0; i < 10; i++)
            {
                var xi = hotbarX + 32 * i;
                var location = new Vector2(xi, hotbarY);
                var color = i == selectedItem ? Color.LimeGreen : Color.White;
                graphics.Draw(hotbarSprite, location, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                String label = i == 9 ? "0" : (i + 1).ToString();

                if (null != itemsOnHotbar[i])
                {
                    itemsOnHotbar[i].Render(graphics, xi, hotbarY);
                }

                TextUtils.RenderOutlinedText(graphics, TextUtils.defaultFont, label, hotbarX + 22 + 32 * i, hotbarY + 18);
            }
        }

        public void UpdateHotbar()
        {
            for (int i = 0; i < 10; i++)
            {
                if (InputUtils.GetKeyState("Hotbar " + i))
                    selectedItem = i - 1;

                if (InputUtils.LeftMouseClicked)
                {
                    var mouseLocation = InputUtils.GetMouseScreenPosition();
                    var xi = (hotbarX + 32 * i);
                    if (mouseLocation.X > xi && mouseLocation.X < xi + 32 && mouseLocation.Y > hotbarY && mouseLocation.Y < hotbarY + 32)
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
    }
}
