using Microsoft.Xna.Framework.Graphics;

namespace space_planet_sandbox.entities.items
{
    public abstract class Item
    {
        public readonly bool burnsInLava;
        public readonly int maxStack;
        readonly Texture2D icon;

        public abstract void onUse();
    }
}
