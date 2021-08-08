using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace space_planet_sandbox.world
{
    class TileMap
    {
        private int tileWidth;
        private int tileHeight;
        private TileData[,] tileData;

        public TileMap(int _width, int _height)
        {
            tileWidth = _width;
            tileHeight = _height;
            tileData = new TileData[_width, _height];

            Random tileChooser = new Random();
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
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
                    }
                    else
                    {
                        tileData[x, y].tileName = "ground_tiles_and_plants";
                        tileData[x, y].tileIndex = 3;
                    }
                }
            }
        }

        public void Update(int x, int y)
        {
            int xTile = x / 16;
            int yTile = y / 16;
            if (tileData[xTile, yTile].tileName.Equals("empty"))
            {
                tileData[xTile, yTile].tileName = "ground_tiles_and_plants"; 
            }
            else
            {
                tileData[xTile, yTile].tileName = "empty";
            }
        }
        public void Render(SpriteBatch _graphics)
        {
            for (int x = 0; x < tileWidth; x++)
            {
                for (int y = 0; y < tileHeight; y++)
                {
                    if (!tileData[x,y].tileName.Equals("empty"))
                    {
                        _graphics.Draw(Game1.loadedTextures[tileData[x,y].tileName],
                            new Rectangle(x * 16, y * 16, 16, 16),
                            new Rectangle(tileData[x, y].tileIndex * 16, 0, 16, 16),
                            Color.White);
                    }
                }
            }
        }
    }
}
