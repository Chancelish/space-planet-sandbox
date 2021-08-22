﻿using space_planet_sandbox.entities;
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

        public void Update(GameTime gameTime)
        {
            if (InputUtils.LeftMouseClicked)
            {
                var mousePosition = Mouse.GetState().Position;
                int xChunk = mousePosition.X / (chunkSize * 16);
                int yChunk = mousePosition.Y / (chunkSize * 16);
                chunks[xChunk, yChunk].Update(mousePosition.X, mousePosition.Y);
            }

            player.interSectingChunks = new List<TileMap>();
            int x1 = ((int) player.Position().X) / (chunkSize * 16);
            int y1 = ((int) player.Position().Y) / (chunkSize * 16);
            int x2 = ((int) (player.Position().X + 16)) / (chunkSize * 16);
            int y2 = ((int) (player.Position().X + 32)) / (chunkSize * 16);
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