using Microsoft.Xna.Framework.Graphics;
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
        private InventoryTab selectedTab = InventoryTab.Equipment;

        Dictionary<InventoryTab, InventoryItem[]> inventorySlots = new Dictionary<InventoryTab, InventoryItem[]>();

        public PlayerInventory()
        {
            inventorySlots.Add(InventoryTab.Equipment, new InventoryItem[60]);
            inventorySlots.Add(InventoryTab.Furniture, new InventoryItem[60]);
            inventorySlots.Add(InventoryTab.Blocks, new InventoryItem[60]);
            inventorySlots.Add(InventoryTab.Consumables, new InventoryItem[60]);
            inventorySlots.Add(InventoryTab.Materials, new InventoryItem[60]);
        }

        public bool IsFull(InventoryTab tab)
        {
            for (int i = 0; i < inventorySlots[tab].Length; i++)
            {
                if (inventorySlots[tab][i] == null) return false;
            }
            return true;
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

        public void Render(SpriteBatch graphics, int x, int y)
        {
            foreach (InventoryItem item in inventorySlots[selectedTab])
            {
                if (item != null) item.Render(graphics, 0, 0);
            }
        }
    }
}
