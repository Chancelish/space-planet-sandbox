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
        private readonly int tileWidth;
        private readonly int tileHeight;
        private readonly int tileSize;
        private int originX;
        private int originY;
        private readonly TileData[,] tileData;
        private GridMask grid;

        private Dictionary<string, int> biomeScore;
        private string dominantBiome;
        private int biomeThreshold;

        private static readonly string EMPTY = "empty";

        public TileMap(int size, int width, int height, int xOrigin = 0, int yOrigin = 0)
        {
            tileWidth = width;
            tileHeight = height;
            tileSize = size;
            x = originX = xOrigin;
            y = originY = yOrigin;
            tileData = new TileData[width, height];
            grid = new GridMask(size, width, height, xOrigin, yOrigin);
            biomeScore = new Dictionary<string, int>();
            biomeScore.Add("sky", 0);
            dominantBiome = "sky";
            biomeThreshold = 2 * (width + height);

            Random tileChooser = new Random();
            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = 0; iy < height; iy++)
                {
                    if ((iy * 16) + yOrigin < 320)
                    {
                        tileData[ix, iy].tileName = EMPTY;
                    }
                    else if ((iy * 16) + yOrigin == 320)
                    {
                        tileData[ix, iy].tileName = "ground_tiles_and_plants";
                        tileData[ix, iy].tileIndex = 1;
                        grid.ChangeTile(ix, iy, 1);
                    }
                    else
                    {
                        tileData[ix, iy].tileName = "ground_tiles_and_plants";
                        tileData[ix, iy].tileIndex = 3;
                        grid.ChangeTile(ix, iy, 1);
                    }
                    tileData[ix, iy].behaviorTag = "basic";
                    tileData[ix, iy].biomeTags = new HashSet<string>();
                }
            }
        }

        public void Update(int mouseX, int mouseY)
        {
            int xTile = (mouseX - originX) / tileSize;
            int yTile = (mouseY - originY)/ tileSize;
            if (xTile < 0 || yTile < 0 || xTile >= tileWidth || yTile >= tileHeight)
            {
                return;
            }
            if (tileData[xTile, yTile].tileName.Equals(EMPTY))
            {
                tileData[xTile, yTile].tileName = "ground_tiles_and_plants";
                grid.ChangeTile(xTile, yTile, 1);
            }
            else
            {
                tileData[xTile, yTile].tileName = EMPTY;
                grid.ChangeTile(xTile, yTile, 0);
            }
        }

        public override void Update(GameTime time)
        {
            
        }
        public void Render(SpriteBatch graphics)
        {
            for (int ix = 0; ix < tileWidth; ix++)
            {
                for (int iy = 0; iy < tileHeight; iy++)
                {
                    if (!tileData[ix,iy].tileName.Equals(EMPTY))
                    {
                        graphics.Draw(SandboxGame.loadedTextures[tileData[ix,iy].tileName],
                            new Rectangle(originX + ix * tileSize, originY + iy * tileSize, tileSize, tileSize),
                            new Rectangle(tileData[ix, iy].tileIndex * tileSize, 0, tileSize, tileSize),
                            Color.White);
                    }
                }
            }
        }

        public override ICollisionMask GetCollisionMask()
        {
            return grid;
        }

        public string GetBiome()
        {
            return biomeScore[dominantBiome] > biomeThreshold ? dominantBiome : "sky";
        }

        public void AddTile(int xLoc, int yLoc, TileData tile)
        {
            // can only place a block if the tile is empty,
            // here or else there must be some indicator of whether a tile can be placed
            if (tileData[xLoc, yLoc].tileName.Equals(EMPTY))
            {
                tileData[xLoc, yLoc] = tile;
                grid.ChangeTile(xLoc, yLoc, 1);
                AlterBiomeOnAddition(xLoc, yLoc);
            }
        }

        public void RemoveTile(int xLoc, int yLoc)
        {
            // shouldn't get here on empty tile, final safeguard
            if (tileData[xLoc, yLoc].tileName.Equals(EMPTY)) return;

            foreach (string tag in tileData[xLoc, yLoc].biomeTags)
            {
                {
                    biomeScore[tag]--;
                    if (tag.Equals(dominantBiome))
                    {
                        foreach (string biome in biomeScore.Keys)
                        {
                            if (biomeScore[biome] > biomeScore[tag])
                            {
                                dominantBiome = biome;
                                break;
                            }
                        }
                    }
                }
            }
            tileData[xLoc, yLoc].tileName = EMPTY;
            grid.ChangeTile(xLoc, yLoc, 0);
            // dropItemFromTile();
        }

        private void ComputeInitialBiome()
        {
            for (int ix = 0; ix < tileWidth; ix++)
            {
                for (int iy = 0; iy < tileHeight; iy++)
                {
                    if (tileData[ix, iy].tileName.Equals(EMPTY)) continue;

                    AlterBiomeOnAddition(ix, iy);
                }
            }
        }

        private void AlterBiomeOnAddition(int xLoc, int yLoc)
        {
            foreach (string tag in tileData[xLoc, yLoc].biomeTags)
            {
                if (biomeScore.ContainsKey(tag))
                {
                    biomeScore[tag]++;
                    dominantBiome = biomeScore[tag] > biomeScore[dominantBiome] ? tag : dominantBiome;
                }
                else
                {
                    biomeScore.Add(tag, 1);
                }
            }
        }
    }
}
