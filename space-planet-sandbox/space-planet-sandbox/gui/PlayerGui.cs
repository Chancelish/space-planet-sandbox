using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Texture2D warpButton;
        private Color color;
        private Vector2 warpButtonPosition;

        public bool clicked { get; private set; }

        public PlayerGui(OptionsMenu options)
        {
            optionsMenu = options;
            guiState = GuiState.Basic;
            inventoryGui = new InventoryGui();
            warpButton = SandboxGame.loadedTextures["to_ship"];
            color = new Color(30, 45, 55);
            warpButtonPosition = new Vector2(1036, 646);
        }

        public void Render(SpriteBatch graphics)
        {
            graphics.Draw(SandboxGame.loadedTextures["white_pixel"], new Rectangle(0, 640, 1280, 80), color);
            graphics.Draw(warpButton, warpButtonPosition, Color.White);
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
                    clicked = inventoryGui.clicked || InputUtils.GetMouseScreenPosition().Y > 640;
                    WarpClicked();
                    if (InputUtils.GetKeyPressed("escape"))
                    {
                        guiState = GuiState.OptionsMenu;
                        optionsMenu.Open();
                    }
                    break;
                case GuiState.Inventory:
                    inventoryGui.Update();
                    clicked = inventoryGui.clicked || InputUtils.GetMouseScreenPosition().Y > 640;
                    WarpClicked();
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

        public void SetWarpButton(Texture2D image)
        {
            warpButton = image;
        }

        public void WarpClicked()
        {
            if (InputUtils.LeftMouseClicked)
            {
                var cursor = InputUtils.GetMouseScreenPosition();
                if (cursor.X > warpButtonPosition.X && cursor.X < warpButtonPosition.X + 24 && cursor.Y > warpButtonPosition.Y && cursor.Y < warpButtonPosition.Y + 24)
                {
                    if (warpButton.Equals(SandboxGame.loadedTextures["to_ship"])) SandboxGame.GoToWorld(new world.World(7, 7, 32, true));
                    else SandboxGame.GoToWorld(new world.World(100, 20, 32));
                }
            }
        }
    }
}
