﻿using space_planet_sandbox.entities;
using space_planet_sandbox;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using space_planet_sandbox.entities.player;
using System;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.entities.environment;

namespace space_planet_sandbox.world
{
    public class World
    {
        private List<CollidableEntity> entities;

        private readonly int chunkWidth;
        private readonly int chunkHeight;
        private readonly int chunkSize;
        private readonly int tileSize;
        private readonly int chunkPixelSize;

        private readonly int tileWorldWidth;
        public readonly int pixelWorldWidth;
        private readonly int tileWorldHeight;
        public readonly int pixelWorldHeight;

        private TileMap[,] chunks;
        private List<CollidableEntity>[,] activeRegion;

        private PlayerCharacter player;
        private BlockPreview blockPreview;

        private int xChunkMain;
        private int yChunkMain;

        public World(int width, int height, int size = 32)
        {
            chunkWidth = width;
            chunkHeight = height;
            chunkSize = size;
            tileSize = 16;
            chunkPixelSize = (chunkSize * tileSize);

            tileWorldWidth = width * size;
            tileWorldHeight = height * size;

            pixelWorldWidth = tileWorldWidth * tileSize;
            pixelWorldHeight = tileWorldHeight * tileSize;

            player = new PlayerCharacter(640, 50);
            blockPreview = new BlockPreview(tileSize);
            blockPreview.SetWorld(this);

            entities = new List<CollidableEntity>();
            AddEntity(player);

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
                    chunks[ix, iy].SetWorld(this);
                }
            }
        }

        public bool IsPlacementBlocked()
        {
            return blockPreview.placementBlocked;
        }

        public bool PlaceTile(int x, int y, string blockName)
        {
            int xChunk = x / chunkPixelSize;
            int yChunk = y / chunkPixelSize;
            if (x >= 0 && xChunk < chunkWidth && y >= 0 && yChunk < chunkHeight)
            {
                return chunks[xChunk, yChunk].AddTile(x, y, TileDataDictionary.GetTile(blockName));
            }
            return false;
        }

        public bool MineTile(int x, int y, float miningPower)
        {
            int xChunk = x / chunkPixelSize;
            int yChunk = y / chunkPixelSize;
            if (x >= 0 && xChunk < chunkWidth && y >= 0 && yChunk < chunkHeight)
            {
                return chunks[xChunk, yChunk].RemoveTile(x, y);
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            PreUpdate();

            var mousePosition = Mouse.GetState().Position;
            int realMouseX = (int) (mousePosition.X / SandboxGame.renderScale) + SandboxGame.camera.x;
            int realMouseY = (int) (mousePosition.Y / SandboxGame.renderScale) + SandboxGame.camera.y;
            
            if (SandboxGame.gui.GetHighlightedItem() != null)
            {
                blockPreview.UpdatePosition(realMouseX, realMouseY);
                blockPreview.Update(gameTime);
                if (InputUtils.LeftMouse)
                {
                    SandboxGame.gui.GetHighlightedItem().OnUse(new Point(realMouseX, realMouseY), player, this);
                }
            }

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
                    chunks[ithIndex, jthIndex].Update(gameTime);
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

            foreach (var entity in entities)
            {
                if (entity.isActive)
                {
                    entity.Render(graphics);
                }
            }
            blockPreview.Render(graphics);
        }

        public void AddEntity(CollidableEntity entity)
        {
            entities.Add(entity);
            entity.SetWorld(this);
        }

        public Dictionary<String, HashSet<CollidableEntity>> GetPotentialCollisions(int x, int y, int width, int height)
        {
            Dictionary<String, HashSet<CollidableEntity>> possibleCollisions = new Dictionary<String, HashSet<CollidableEntity>>();
            var x1Chunk = Math.Max(x / chunkPixelSize - xChunkMain + 3, 0);
            var x2Chunk = Math.Min((x + width) / chunkPixelSize - xChunkMain + 3, 6);
            var y1Chunk = Math.Max(y / chunkPixelSize - yChunkMain + 3, 0);
            var y2Chunk = Math.Min((y + height) / chunkPixelSize - yChunkMain + 3, 6);
            for (int i = x1Chunk; i <= x2Chunk; i++)
            {
                for (int j = y1Chunk; j <= y2Chunk; j++)
                {
                    foreach (CollidableEntity entity in activeRegion[i, j])
                    {
                        if (!possibleCollisions.ContainsKey(entity.collisionGroup))
                        {
                            possibleCollisions.Add(entity.collisionGroup, new HashSet<CollidableEntity>());
                        }
                        possibleCollisions[entity.collisionGroup].Add(entity);
                    }
                }
            }
            return possibleCollisions;
        }

        public TileDataAbridged GetTileAt(int x, int y)
        {
            int xChunk = x / chunkPixelSize;
            int yChunk = y / chunkPixelSize;
            if (x >= 0 && xChunk < chunkWidth && y >= 0 && yChunk < chunkHeight)
            {
                return chunks[xChunk, yChunk].GetTileAt(x, y);
            }
            return TileDataDictionary.GetTile("empty").Abridge();
        }

        private void PreUpdate()
        {
            var playerPostion = player.Position();
            xChunkMain = (int) Math.Floor(playerPostion.X / chunkPixelSize);
            yChunkMain = (int) Math.Floor(playerPostion.Y / chunkPixelSize);
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
            entities.RemoveAll(entity => entity.flaggedForRemoval);
        }

        private bool IsEntityActive(CollidableEntity entity)
        {
            int leftChunk = (int)Math.Floor(entity.Position().X / chunkPixelSize);
            int topChunk = (int)Math.Floor(entity.Position().Y / chunkPixelSize);
            int rightChunk = (int)Math.Floor((entity.Position().X + entity.GetSize().X) / chunkPixelSize);
            int bottomChunk = (int)Math.Floor((entity.Position().Y + entity.GetSize().Y) / chunkPixelSize);
            if (rightChunk >= xChunkMain - 3
                && leftChunk <= xChunkMain + 3
                && topChunk <= Math.Min(chunkHeight - 1, yChunkMain + 3)
                && bottomChunk >= Math.Max(0, yChunkMain - 3))
            {
                int leftIndex = Math.Max(leftChunk - xChunkMain + 3, 0);
                int rightIndex = Math.Min(rightChunk - xChunkMain + 3, 6);
                int topIndex = Math.Max(topChunk - yChunkMain + 3, 0);
                int bottomIndex = Math.Min(bottomChunk - yChunkMain + 3, 6);
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
