using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.world
{
    public struct TileData
    {
        public int tileIndex; // the tile in the tileset to draw, if index is somehow larger than the tilesheet, should draw tile
        public double maxDurability; // the total durability of the tile
        public double tileDurability; // the total durability of the tile
        public double tileToughness; // mining power required to deplete durability
        public string tileName; // the texture group to pull from
        public HashSet<String> behaviorTags; // for now, ore, crumble, or constructed
        public HashSet<String> biomeTags; // any biomes this block can associated, unknown biomes default to plains
        public bool richDrop; // true drop 3 blocks instead of one (ores only)
    }
}
