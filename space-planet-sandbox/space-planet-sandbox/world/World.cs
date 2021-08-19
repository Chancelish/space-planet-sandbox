using space_planet_sandbox.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.world
{
    public class World
    {
        private List<CollidableEntity> entities;

        private readonly int chunkWidth;
        private readonly int chunkHeight;
        private readonly int chunkSize;

        private readonly int tileWorldWidth;
        private readonly int pixelWorldWidth;
        private readonly int tileWorldHeight;
        private readonly int pixelWorldHeight;

        private TileMap[,] chunks;

        private World(int width, int height, int size = 32)
        {
            chunkWidth = width;
            chunkHeight = height;
            chunkSize = size;

            tileWorldWidth = width * size;
            tileWorldHeight = height * size;

            pixelWorldWidth = tileWorldWidth * 16;
            pixelWorldHeight = tileWorldHeight * 16;

            chunks = new TileMap[width, height];
        }
    }
}
