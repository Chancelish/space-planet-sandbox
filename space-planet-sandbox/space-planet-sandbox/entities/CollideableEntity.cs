using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public bool flaggedForRemoval;

        public string collisionGroup { get; protected set; }

        public abstract ICollisionMask GetCollisionMask();

        public abstract void Update(GameTime time);

        public abstract void Render(SpriteBatch graphics);

        public bool Collide(CollidableEntity other, int xOffset, int yOffset)
        {
            return GetCollisionMask().Collide(other.GetCollisionMask(), xOffset, yOffset);
        }

        public Vector2 Position()
        {
            return new Vector2(x, y);
        }

        public abstract Point GetWidth();

        public void SetWorld(World world)
        {
            myWorld = world;
        }
    }
}
