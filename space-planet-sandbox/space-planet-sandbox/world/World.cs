using space_planet_sandbox.entities;
using space_planet_sandbox;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using space_planet_sandbox.entities.player;
using System;
using Microsoft.Xna.Framework.Graphics;

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

        private PlayerCharacter player;

        public World(int width, int height, int size = 32)
        {
            chunkWidth = width;
            chunkHeight = height;
            chunkSize = size;

            tileWorldWidth = width * size;
            tileWorldHeight = height * size;

            pixelWorldWidth = tileWorldWidth * 16;
            pixelWorldHeight = tileWorldHeight * 16;

            player = new PlayerCharacter(50, 50);

            entities = new List<CollidableEntity>();
            entities.Add(player);

            chunks = new TileMap[width, height];

            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = 0; iy < height; iy++)
                {
                    chunks[ix, iy] = new TileMap(16, size, size, ix * size * 16, iy * size * 16);
                }
            }
        }

        public bool PlaceTile(int x, int y, string blockName)
        {
            int xChunk = x / (chunkSize * 16);
            int yChunk = y / (chunkSize * 16);
            if (x >= 0 && xChunk < chunkWidth && y >= 0 && yChunk < chunkHeight)
            {
                return chunks[xChunk, yChunk].AddTile(x, y, TileDataDictionary.GetTile(blockName));
            }
            return false;
        }

        public bool MineTile(int x, int y, float miningPower)
        {
            int xChunk = x / (chunkSize * 16);
            int yChunk = y / (chunkSize * 16);
            if (x >= 0 && xChunk < chunkWidth && y >= 0 && yChunk < chunkHeight)
            {
                return chunks[xChunk, yChunk].RemoveTile(x, y);
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            if (InputUtils.LeftMouseClicked)
            {
                var mousePosition = Mouse.GetState().Position;
                int realMouseX = mousePosition.X + SandboxGame.camera.x;
                int realMouseY = mousePosition.Y + SandboxGame.camera.y;
                if (SandboxGame.gui.GetHighlightedItem() != null)
                {
                    SandboxGame.gui.GetHighlightedItem().OnUse(new Point(realMouseX, realMouseY), player, this);
                }
            }

            player.interSectingChunks = new List<TileMap>();
            int x1 = ((int) player.Position().X) / (chunkSize * 16);
            int y1 = ((int) player.Position().Y) / (chunkSize * 16);
            int x2 = ((int) (player.Position().X + 16)) / (chunkSize * 16);
            int y2 = ((int) (player.Position().Y + 32)) / (chunkSize * 16);
            for (int i = Math.Max(x1, 0); i <= Math.Min(x2, chunkWidth - 1); i++)
            {
                for (int j = Math.Max(y1, 0); j <= Math.Min(y2, chunkHeight - 1); j++)
                {
                    player.interSectingChunks.Add(chunks[i, j]);
                }
            }

            foreach (var entity in entities)
            {
                entity.Update(gameTime);
            }
        }

        public void Render(SpriteBatch graphics)
        {
            for (int i = 0; i < chunkWidth; i++)
            {
                for (int j = 0; j < chunkHeight; j++)
                {
                    chunks[i, j].Render(graphics);
                }
            }

            player.Render(graphics);
        }
    }
}
