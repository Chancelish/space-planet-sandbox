using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace space_planet_sandbox.world
{
    class TileMap
    {
        private int tileWidth;
        private int tileHeight;
        private int[,] tileData;

        public TileMap(int _width, int _height)
        {
            tileWidth = _width;
            tileHeight = _height;
            tileData = new int[_width, _height];

            Random tileChooser = new Random();
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    tileData[x,y] = tileChooser.Next(0, 4);
                }
            }
        }

        public void Render(SpriteBatch _graphics)
        {
            for (int x = 0; x < tileWidth; x++)
            {
                for (int y = 0; y < tileHeight; y++)
                {
                    if (tileData[x,y] == 1)
                    {
                        _graphics.Draw(Game1.loadedTextures["ground_tiles_and_plants"],
                            new Rectangle(x * 16, y * 16, 16, 16),
                            new Rectangle(0, 0, 16, 16),
                            Color.White);
                    }
                }
            }
        }
    }
}
