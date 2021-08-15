using Microsoft.Xna.Framework;
using space_planet_sandbox.collisiondetection;

namespace space_planet_sandbox.entities
{
    public abstract class CollidableEntity
    {
        public abstract ICollisionMask GetCollisionMask();

        protected float x;
        protected float y;

        public bool Collide(CollidableEntity other, int xOffset, int yOffset)
        {
            return GetCollisionMask().Collide(other.GetCollisionMask(), xOffset, yOffset);
        }

        public Vector2 Position()
        {
            return new Vector2(x, y);
        }
    }
}
