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
        }
        public override ICollisionMask GetCollisionMask()
        {
            return hurtBox;
        }

        public override void Update(GameTime time)
        {
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

            if (possibleCollisions.ContainsKey("tiles"))
            {
                foreach (var chunk in possibleCollisions["tiles"])
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
                foreach (var chunk in possibleCollisions["tiles"])
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
                SpriteEffects.None,
                0f);
        }

        public override Point GetSize()
        {
            return hurtBox.Size();
        }

        private void CheckPlayerInput(GameTime time)
        {
            var mouseWorldPosition = InputUtils.GetMouseWorldPosition();

            if (InputUtils.LeftMouse && inventory.GetHighlightedItem() != null && !SandboxGame.gui.clicked)
            {
                SandboxGame.gui.GetHighlightedItem().OnUse(new Point(mouseWorldPosition.X, mouseWorldPosition.Y), this, myWorld);
            }

            if (InputUtils.GetKeyState("Inventory") && !InputUtils.GetLastFrameKeyState("Inventory")) SandboxGame.gui.OpenIventory(inventory);

            if (InputUtils.GetKeyState("Up"))
                yVelocity -= speed * time.ElapsedGameTime.TotalSeconds;

            if (InputUtils.GetKeyState("Down"))
                yVelocity += speed * time.ElapsedGameTime.TotalSeconds;

            if (InputUtils.GetKeyState("Left"))
                xVelocity -= speed * time.ElapsedGameTime.TotalSeconds;

            if (InputUtils.GetKeyState("Right"))
                xVelocity += speed * time.ElapsedGameTime.TotalSeconds;
        }
    }
}
