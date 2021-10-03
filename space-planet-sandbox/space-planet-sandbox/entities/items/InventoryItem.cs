using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.world;

namespace space_planet_sandbox.entities.items
{
    public abstract class InventoryItem
    {
        public int maxStack { get; protected set; }
        public InventoryTab tab { get; protected set; }
        private int _quantity;
        public int quantity 
        {
            get { return _quantity; }
            set {
                if (value > maxStack) _quantity = maxStack;
                else if (value > 0) _quantity = value;
                else _quantity = 0;
            }
        }
        protected Texture2D icon;

        public abstract void OnUse(Point cursorLocation, CollidableEntity callingEntity, World targetWorld);

        public abstract void OnDrop(Point cursorLocation, CollidableEntity callingEntity, World targetWorld);

        public abstract void Render(SpriteBatch graphics, float x, float y);
    }
}
