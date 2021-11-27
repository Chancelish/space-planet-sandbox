using space_planet_sandbox.entities;
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
        private HashSet<CollidableEntity> entitiesToAdd;

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

            Initialize();

            player = new PlayerCharacter(640, 150);
            AddEntity(player);
            SandboxGame.gui.SetWarpButton(SandboxGame.loadedTextures["to_ship"]);
        }

        public World(int width, int height, int size, bool isShip)
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

            Initialize();
            if (isShip)
            {
                AddEntity(new Ship("human", 0));
                player = new PlayerCharacter(0, 0);
                AddEntity(player);
                SandboxGame.gui.SetWarpButton(SandboxGame.loadedTextures["to_planet"]);
                player.Displace(16 * 32 + 48, 16 * 32 + 64);
            }
            else
            {
                SandboxGame.gui.SetWarpButton(SandboxGame.loadedTextures["to_ship"]);
                player = new PlayerCharacter(640, 150);
                AddEntity(player);
            }
        }

        private void Initialize()
        {
            blockPreview = new BlockPreview(tileSize);
            blockPreview.SetWorld(this);

            entities = new List<CollidableEntity>();
            entitiesToAdd = new HashSet<CollidableEntity>();

            chunks = new TileMap[chunkWidth, chunkHeight];

            activeRegion = new List<CollidableEntity>[7, 7];

            for (int ix = 0; ix < 7; ix++)
            {
                for (int iy = 0; iy < 7; iy++)
                {
                    activeRegion[ix, iy] = new List<CollidableEntity>();
                }
            }

            for (int ix = 0; ix < chunkWidth; ix++)
            {
                for (int iy = 0; iy < chunkHeight; iy++)
                {
                    chunks[ix, iy] = new TileMap(tileSize, chunkSize, chunkSize, ix * chunkSize * tileSize, iy * chunkSize * tileSize);
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

        public bool MineTile(int x, int y)
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

            var mouseWorldPosition = InputUtils.GetMouseWorldPosition();
            
            if (SandboxGame.gui.GetHighlightedItem() != null)
            {
                blockPreview.UpdatePosition(mouseWorldPosition.X, mouseWorldPosition.Y);
                blockPreview.Update(gameTime);
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
                if (entity.isActive)
                {
                    entity.Update(gameTime);
                    if (entity.Position().X < 0) entity.Displace(pixelWorldWidth, 0f);
                    else if (entity.Position().X > pixelWorldWidth) entity.Displace(-pixelWorldWidth, 0f);
                }
            }
            PostUpdate();
        }

        public void Render(SpriteBatch graphics)
        {
            int x1 = FindParallaxPosition(SandboxGame.camera.x, 4);
            int x2 = FindParallaxPosition(SandboxGame.camera.x, 4, 1280);
            graphics.Draw(SandboxGame.loadedTextures["mountain_1"], new Vector2(x1, 160 + SandboxGame.camera.y / 4), Color.White);
            graphics.Draw(SandboxGame.loadedTextures["mountain_1"], new Vector2(x2, 160 + SandboxGame.camera.y / 4), Color.White);

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
                    if (ithIndex < 0)
                    {
                        ithIndex += chunkWidth;
                        chunks[ithIndex, jthIndex].Render(graphics, -pixelWorldWidth, 0);
                    }
                    else if (ithIndex >= chunkWidth)
                    {
                        ithIndex -= chunkWidth;
                        chunks[ithIndex, jthIndex].Render(graphics, pixelWorldWidth, 0);
                    }
                    chunks[ithIndex, jthIndex].Render(graphics);
                }
            }

            foreach (var entity in entities)
            {
                if (entity.isActive)
                {
                    entity.Render(graphics);
                    if (entity.Position().X < 1280) entity.Render(graphics, pixelWorldWidth, 0);
                    else if (entity.Position().X > pixelWorldWidth - 1280) entity.Render(graphics, -pixelWorldWidth, 0);
                }
            }
            blockPreview.Render(graphics);
        }

        public void AddEntity(CollidableEntity entity)
        {
            entitiesToAdd.Add(entity);
            entity.SetWorld(this);
        }

        private void ResolveAdded()
        {
            entities.AddRange(entitiesToAdd);
            entitiesToAdd.Clear();
        }

        public Dictionary<String, HashSet<CollidableEntity>> GetPotentialCollisions(int x, int y, int width, int height)
        {
            Dictionary<String, HashSet<CollidableEntity>> possibleCollisions = new Dictionary<String, HashSet<CollidableEntity>>();
            var x1Chunk = ComputeBoundaryChunk(x) - xChunkMain + 3;
            var x2Chunk = ComputeBoundaryChunk(x + width) - xChunkMain + 3;
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

        private int FindParallaxPosition(int cameraX, float ratio, int displacement = 0)
        {
            int x1 = (int)(cameraX / ratio) + displacement;
            while (x1 > cameraX + 1280)
            {
                x1 -= 2560;
            }
            while (x1 < cameraX - 1280)
            {
                x1 += 2560;
            }
            return x1;
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
            ResolveAdded();
        }

        private bool IsEntityActive(CollidableEntity entity)
        {
            int leftChunk = ComputeBoundaryChunk(entity.Position().X);
            int topChunk = (int)Math.Floor(entity.Position().Y / chunkPixelSize);
            int rightChunk = ComputeBoundaryChunk(entity.Position().X + entity.GetSize().X);
            int bottomChunk = (int)Math.Floor((entity.Position().Y + entity.GetSize().Y) / chunkPixelSize);
            if (rightChunk >= xChunkMain - 3
                && leftChunk <= xChunkMain + 3
                && topChunk <= Math.Min(chunkHeight - 1, yChunkMain + 3)
                && bottomChunk >= Math.Max(0, yChunkMain - 3))
            {
                int leftIndex = leftChunk - xChunkMain + 3;
                int rightIndex = rightChunk - xChunkMain + 3;
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

        private int ComputeBoundaryChunk(float xPos)
        {
            int chunk = (int)Math.Floor(xPos / chunkPixelSize);
            if (xChunkMain < 3 && chunk > chunkWidth - 3) chunk -= chunkWidth;
            else if (xChunkMain > chunkWidth - 3 && chunk < 3) chunk += chunkWidth;
            return chunk;
        }
    }
}
