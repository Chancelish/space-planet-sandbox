using Microsoft.Xna.Framework;
using space_planet_sandbox.collisiondetection;
using space_planet_sandbox.world;

namespace space_planet_sandbox.entities
{
    public abstract class CollidableEntity
    {
        protected float x;
        protected float y;

        protected World myWorld;
        public bool isActive;

        public abstract ICollisionMask GetCollisionMask();

        public abstract void Update(GameTime time);

        public bool Collide(CollidableEntity other, int xOffset, int yOffset)
        {
            return GetCollisionMask().Collide(other.GetCollisionMask(), xOffset, yOffset);
        }

        public Vector2 Position()
        {
            return new Vector2(x, y);
        }

        public abstract Point GetWidth();

        public void setWorld(World world)
        {
            myWorld = world;
        }
    }
}
