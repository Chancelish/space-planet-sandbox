using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.world
{
    struct TileData
    {
        int tileIndex; // the tile in the tileset to draw, if index is somehow larger than the tilesheet, should draw tile
        double tileDurability; // the total durability of the tile
        double tileToughness; // mining power required to deplete durability
        string tileName; // the texture group to pull from
        string[] tags; // for now, ore, environment, crumble, or constructed
        bool richDrop; // true drop 3 blocks instead of one (ores only)
    }
}
