using space_planet_sandbox.collisiondetection;

namespace space_planet_sandbox.entities
{
    public abstract class CollidableEntity
    {
        public abstract ICollisionMask GetCollisionMask();

        public bool Collide(CollidableEntity other, int xOffset, int yOffset)
        {
            return GetCollisionMask().Collide(other.GetCollisionMask(), xOffset, yOffset);
        }
    }
}
