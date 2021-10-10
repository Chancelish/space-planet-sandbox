﻿using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.entities.items;

namespace space_planet_sandbox.gui
{
    public enum GuiState
    {
        None,
        Basic,
        OptionsMenu,
        Inventory
    }
    
    public class PlayerGui
    {
        public GuiState guiState { get; private set; }

        private OptionsMenu optionsMenu;
        private InventoryGui inventoryGui;

        public bool clicked { get; private set; }

        public PlayerGui(OptionsMenu options)
        {
            optionsMenu = options;
            guiState = GuiState.Basic;
            inventoryGui = new InventoryGui();
        }

        public void Render(SpriteBatch graphics)
        {
            inventoryGui.Render(graphics);
            if (guiState == GuiState.OptionsMenu)
            {
                optionsMenu.Render(graphics);
            }
        }

        public void Update()
        {
            clicked = false;
            switch (guiState)
            {
                case GuiState.OptionsMenu:
                    optionsMenu.Update();
                    if (!optionsMenu.isOpen)
                    {
                        guiState = GuiState.Basic;
                    }
                    break;
                case GuiState.Basic:
                    inventoryGui.Update();
                    clicked = inventoryGui.clicked;
                    if (InputUtils.GetKeyPressed("escape"))
                    {
                        guiState = GuiState.OptionsMenu;
                        optionsMenu.Open();
                    }
                    break;
                case GuiState.Inventory:
                    inventoryGui.Update();
                    clicked = inventoryGui.clicked;
                    if (!inventoryGui.isOpen) guiState = GuiState.Basic;
                    break;
            }
        }

        public InventoryItem GetHighlightedItem()
        {
            return inventoryGui.GetHighlightedItem();
        }

        public void OpenIventory(PlayerInventory inventory, entities.player.PlayerCharacter player)
        {
            if (inventoryGui.isOpen)
            {
                inventoryGui.Close();
                return;
            }
            else
            {
                inventoryGui.Open(inventory, player);
                guiState = GuiState.Inventory;
            }
        }

        public void SetInventory(PlayerInventory inventory)
        {
            inventoryGui.SetInventory(inventory);
        }
    }
}
