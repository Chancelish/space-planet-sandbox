using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.world
{
    public enum TileBehavior
    {
        None,
        Basic,
        Platform,
        Sand,
        Liquid,
        Ore
    }

    public struct TileData
    {
        public int tileIndex; // the tile in the tileset to draw, if index is somehow larger than the tilesheet, should draw tile
        public float maxDurability; // the total durability of the tile
        public float durability; // the total durability of the tile
        public float toughness; // mining power required to deplete durability
        public TileBehavior behaviorTag; 
        public string tileName; // the texture group to pull from
        public HashSet<String> biomeTags; // any biomes this block can associated, unknown biomes default to plains

        public TileData(float durability, float toughness, TileBehavior behavior, string name, params string[] biomes)
        {
            tileIndex = 0;
            this.durability = maxDurability = durability;
            this.toughness = toughness;
            tileName = name;
            behaviorTag = behavior;
            biomeTags = new HashSet<string>();
            foreach (string biome in biomes)
            {
                biomeTags.Add(biome);
            }
        }

        public TileDataAbridged Abridge()
        {
            var tileDataAbridged = new TileDataAbridged();
            tileDataAbridged.behaviorTag = behaviorTag;
            tileDataAbridged.tileName = tileName;
            tileDataAbridged.tileIndex = 0;
            return tileDataAbridged;
        }
    }

    public struct TileDataAbridged //excludes biome tags to cute down om memory use.
    {
        public int tileIndex;
        public string tileName;
        public TileBehavior behaviorTag;
    }

    public sealed class TileDataDictionary
    {
        private static readonly Dictionary<string, TileData> knownTiles = new Dictionary<string, TileData>();

        public static void LoadTiles() // in the future load from json file.
        {
            knownTiles.Add("empty", new TileData(0, 0, TileBehavior.None, "empty"));
            knownTiles.Add("dirt", new TileData(10, 0, TileBehavior.Basic, "dirt", "forest", "plains"));
            knownTiles.Add("sand", new TileData(8, 0, TileBehavior.Sand, "sand", "desert", "beach"));
        }

        public static TileData GetTile(string tileType)
        {
            return knownTiles.ContainsKey(tileType) ? knownTiles[tileType] : knownTiles["empty"];
        }
    }
}
