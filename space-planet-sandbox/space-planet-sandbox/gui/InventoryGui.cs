using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.entities.items;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.gui
{
    public class InventoryGui
    {

        private Texture2D background;
        private PlayerInventory inventory;

        public bool isOpen { get; private set; }

        public InventoryGui()
        {
            background = SandboxGame.loadedTextures["inventory_core"];
        }

        public void Open(PlayerInventory playerInventory)
        {
            inventory = playerInventory;
            isOpen = true;
        }
    }
}
