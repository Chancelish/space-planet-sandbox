using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using space_planet_sandbox.world;
using space_planet_sandbox.entities.items;

namespace space_planet_sandbox.entities.player
{
    public class PlayerCharacter : CollidableEntity
    {
        private HitBox hurtBox;
        private float speed = 200.0f;
        private double xVelocity = 0;
        private double yVelocity = 0;
        private Texture2D sprite;
        private double timeSinceDropped = 1;
        public float lastFrameTime { get; private set; }
        private bool lookingRight = false;

        private Texture2D boxOutline;
        private PlayerInventory inventory;

        public PlayerCharacter(int startX, int startY)
        {
            hurtBox = new HitBox(startX, startY, 16, 32);
            sprite = SandboxGame.loadedTextures["unknown"];
            x = startX;
            y = startY;
            collisionGroup = "player";
            inventory = new PlayerInventory();
            SandboxGame.gui.SetInventory(inventory);
            lastFrameTime = 0.5f;
        }
        public override ICollisionMask GetCollisionMask()
        {
            return hurtBox;
        }

        public override void Update(GameTime time)
        {
            lastFrameTime =(float) time.ElapsedGameTime.TotalSeconds;
            SandboxGame.camera.Follow(this, 1280, 720);

            var possibleCollisions = myWorld.GetPotentialCollisions((int) x, (int) y, hurtBox.Size().X, hurtBox.Size().Y);

            xVelocity = 0;
            yVelocity = 0;

            if (SandboxGame.gui.guiState != gui.GuiState.OptionsMenu)
            {
                CheckPlayerInput(time);
            }

            int xCheck = (int) (xVelocity + Math.Sign(xVelocity));
            int yCheck = (int) (yVelocity + Math.Sign(yVelocity));

            var tiles = ExtractByCollisionGroup(possibleCollisions, this, "tiles");
            foreach (var chunk in tiles)
            {
                if (Collide(chunk, xCheck, 0))
                {
                    while (xCheck != 0)
                    {
                        xCheck = xCheck > 0 ? xCheck - 1 : xCheck + 1;
                        xVelocity = xCheck;
                        if (!Collide(chunk, xCheck, 0))
                        {
                            break;
                        }
                    }
                    break;
                }
            }
            foreach (var chunk in tiles)
            {
                if (Collide(chunk, 0, yCheck))
                {
                    while (yCheck != 0)
                    {
                        yCheck = yCheck > 0 ? yCheck - 1 : yCheck + 1;
                        yVelocity = yCheck;
                        if (!Collide(chunk, 0, yCheck))
                        {
                            break;
                        }
                    }
                    break;
                }
            }

            var items = ExtractByCollisionGroup(possibleCollisions, this, "item");
            if (timeSinceDropped > 0)
            {
                timeSinceDropped -= time.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                foreach (var item in items)
                {
                    if (Collide(item, 0, 0))
                    {
                        (item as WorldItem).Collect(this, inventory);
                    }
                }
            }

            x += (float) xVelocity;
            y += (float) yVelocity;
            if (y < 0) y = 0;
            hurtBox.MoveTo((int) x, (int) y);
        }

        public override void Render(SpriteBatch graphics)
        {
            if (boxOutline == null)
            {
                boxOutline = new Texture2D(graphics.GraphicsDevice, 1, 1);
                boxOutline.SetData(new[] { Color.White });
            }
            graphics.Draw(boxOutline, new Rectangle((int) x, (int) y, 16, 32), Color.LightGreen);

            graphics.Draw(sprite,
                new Vector2(x, y),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                lookingRight ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0f);
        }

        public override Point GetSize()
        {
            return hurtBox.Size();
        }

        public void DropItem(InventoryItem item)
        {
            item.OnDrop(InputUtils.GetMouseWorldPosition(), this, myWorld);
            timeSinceDropped = 1f;
        }

        private void CheckPlayerInput(GameTime time)
        {
            var mouseWorldPosition = InputUtils.GetMouseWorldPosition();

            if (InputUtils.LeftMouse && inventory.GetHighlightedItem() != null && !SandboxGame.gui.clicked)
            {
                SandboxGame.gui.GetHighlightedItem().OnUse(new Point(mouseWorldPosition.X, mouseWorldPosition.Y), this, myWorld);
            }

            if (InputUtils.GetKeyState("Inventory") && !InputUtils.GetLastFrameKeyState("Inventory")) SandboxGame.gui.OpenIventory(inventory, this);

            if (InputUtils.GetKeyState("Up"))
                yVelocity -= speed * time.ElapsedGameTime.TotalSeconds;

            if (InputUtils.GetKeyState("Down"))
                yVelocity += speed * time.ElapsedGameTime.TotalSeconds;

            if (InputUtils.GetKeyState("Left")) {
                xVelocity -= speed * time.ElapsedGameTime.TotalSeconds;
                lookingRight = false;
            }
                
            if (InputUtils.GetKeyState("Right")) {
                xVelocity += speed * time.ElapsedGameTime.TotalSeconds;
                lookingRight = true;
                // gotta look where ya going!
            }
                
        }
    }
}
