using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using space_planet_sandbox.collisiondetection;
using space_planet_sandbox.entities;

namespace space_planet_sandbox.world
{
    public class TileMap : CollidableEntity
    {
        private int tileWidth;
        private int tileHeight;
        private TileData[,] tileData;
        private GridMask grid;

        public TileMap(int width, int height)
        {
            tileWidth = width;
            tileHeight = height;
            tileData = new TileData[width, height];
            grid = new GridMask(16, width, height, 0, 0);

            Random tileChooser = new Random();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (y < 20)
                    {
                        tileData[x, y].tileName = "empty";
                        tileData[x, y].tags = new List<String>();
                    }
                    else if (y == 20)
                    {
                        tileData[x, y].tileName = "ground_tiles_and_plants";
                        tileData[x, y].tileIndex = 1;
                        grid.ChangeTile(x, y, 1);
                    }
                    else
                    {
                        tileData[x, y].tileName = "ground_tiles_and_plants";
                        tileData[x, y].tileIndex = 3;
                        grid.ChangeTile(x, y, 1);
                    }
                }
            }
        }

        public void Update(int x, int y)
        {
            int xTile = x / 16;
            int yTile = y / 16;
            if (xTile < 0 || yTile < 0 || xTile >= tileWidth || yTile >= tileHeight)
            {
                return;
            }
            if (tileData[xTile, yTile].tileName.Equals("empty"))
            {
                tileData[xTile, yTile].tileName = "ground_tiles_and_plants";
                grid.ChangeTile(xTile, yTile, 1);
            }
            else
            {
                tileData[xTile, yTile].tileName = "empty";
                grid.ChangeTile(xTile, yTile, 0);
            }
        }
        public void Render(SpriteBatch graphics)
        {
            for (int x = 0; x < tileWidth; x++)
            {
                for (int y = 0; y < tileHeight; y++)
                {
                    if (!tileData[x,y].tileName.Equals("empty"))
                    {
                        graphics.Draw(Game1.loadedTextures[tileData[x,y].tileName],
                            new Rectangle(x * 16, y * 16, 16, 16),
                            new Rectangle(tileData[x, y].tileIndex * 16, 0, 16, 16),
                            Color.White);
                    }
                }
            }
        }

        public override ICollisionMask GetCollisionMask()
        {
            return grid;
        }
    }
}
