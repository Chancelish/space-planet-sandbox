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
        private readonly int tileSize;

        private readonly int tileWorldWidth;
        public readonly int pixelWorldWidth;
        private readonly int tileWorldHeight;
        public readonly int pixelWorldHeight;

        private TileMap[,] chunks;
        private List<CollidableEntity>[,] activeRegion;

        private PlayerCharacter player;

        private int xChunkMain;
        private int yChunkMain;

        public World(int width, int height, int size = 32)
        {
            chunkWidth = width;
            chunkHeight = height;
            chunkSize = size;
            tileSize = 16;

            tileWorldWidth = width * size;
            tileWorldHeight = height * size;

            pixelWorldWidth = tileWorldWidth * tileSize;
            pixelWorldHeight = tileWorldHeight * tileSize;

            player = new PlayerCharacter(50, 50);

            entities = new List<CollidableEntity>();
            entities.Add(player);

            chunks = new TileMap[width, height];

            activeRegion = new List<CollidableEntity>[7, 7];

            for (int ix = 0; ix < 7; ix ++)
            {
                for (int iy = 0; iy < 7; iy++)
                {
                    activeRegion[ix, iy] = new List<CollidableEntity>();
                }
            }

            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = 0; iy < height; iy++)
                {
                    chunks[ix, iy] = new TileMap(tileSize, size, size, ix * size * tileSize, iy * size * tileSize);
                }
            }
        }

        public bool PlaceTile(int x, int y, string blockName)
        {
            int xChunk = x / (chunkSize * tileSize);
            int yChunk = y / (chunkSize * tileSize);
            if (x >= 0 && xChunk < chunkWidth && y >= 0 && yChunk < chunkHeight)
            {
                return chunks[xChunk, yChunk].AddTile(x, y, TileDataDictionary.GetTile(blockName));
            }
            return false;
        }

        public bool MineTile(int x, int y, float miningPower)
        {
            int xChunk = x / (chunkSize * tileSize);
            int yChunk = y / (chunkSize * tileSize);
            if (x >= 0 && xChunk < chunkWidth && y >= 0 && yChunk < chunkHeight)
            {
                return chunks[xChunk, yChunk].RemoveTile(x, y);
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            PreUpdate();
            
            if (InputUtils.LeftMouse)
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
            int x1 = ((int) player.Position().X) / (chunkSize * tileSize);
            int y1 = ((int) player.Position().Y) / (chunkSize * tileSize);
            int x2 = ((int) (player.Position().X + tileSize)) / (chunkSize * tileSize);
            int y2 = ((int) (player.Position().Y + 32)) / (chunkSize * tileSize);
            for (int i = Math.Max(x1, 0); i <= Math.Min(x2, chunkWidth - 1); i++)
            {
                for (int j = Math.Max(y1, 0); j <= Math.Min(y2, chunkHeight - 1); j++)
                {
                    player.interSectingChunks.Add(chunks[i, j]);
                }
            }

            foreach (var entity in entities)
            {
                if (entity.isActive) entity.Update(gameTime);
            }

            PostUpdate();
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

        private void PreUpdate()
        {
            var playerPostion = player.Position();
            xChunkMain = (int) Math.Floor(playerPostion.X / (chunkSize * tileSize));
            yChunkMain = (int) Math.Floor(playerPostion.Y / (chunkSize * tileSize));
            for (int j = 0; j < 7; j++)
            {
                int jthIndex = yChunkMain - 3 + j;
                if (jthIndex < 0 || jthIndex >= chunkHeight)
                {
                    continue;
                }
                for (int i = 0; i < 7; i++)
                {
                    int ithIndex = xChunkMain - 3 + i;
                    if (ithIndex < 0) ithIndex += chunkWidth;
                    if (ithIndex >= chunkWidth) ithIndex -= chunkWidth;
                    activeRegion[i, j].Add(chunks[ithIndex, jthIndex]);
                }
            }
            foreach (CollidableEntity entity in entities)
            {
                entity.isActive = IsEntityActive(entity);
                
            }
        }

        private void PostUpdate()
        {
            for (int ix = 0; ix < 7; ix++)
            {
                for (int iy = 0; iy < 7; iy++)
                {
                    activeRegion[ix, iy].Clear();
                }
            }
        }

        private bool IsEntityActive(CollidableEntity entity)
        {
            int leftChunk = (int)Math.Floor(entity.Position().X / (chunkSize * tileSize));
            int topChunk = (int)Math.Floor(entity.Position().Y / (chunkSize * tileSize));
            int rightChunk = (int)Math.Floor((entity.Position().X + entity.GetWidth().X) / (chunkSize * tileSize));
            int bottomChunk = (int)Math.Floor((entity.Position().Y + entity.GetWidth().Y) / (chunkSize * tileSize));
            if (rightChunk >= xChunkMain - 3
                && leftChunk <= xChunkMain + 3
                && topChunk <= Math.Min(chunkHeight - 1, yChunkMain + 3)
                && bottomChunk >= Math.Max(0, yChunkMain - 3))
            {
                int leftIndex = leftChunk - xChunkMain + 3;
                int rightIndex = rightChunk - xChunkMain + 3;
                int topIndex = topChunk - yChunkMain + 3;
                int bottomIndex = bottomChunk - yChunkMain + 3;
                for (int i = leftIndex; i <= rightIndex; i++)
                {
                    for (int j = topIndex; j <= bottomIndex; j++)
                    {
                        activeRegion[i, j].Add(entity);
                    }
                }
                return true;
            }
            return false;
        }
    }
}
