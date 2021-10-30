using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using space_planet_sandbox.world;
using System.Collections.Generic;

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

        public abstract void Render(SpriteBatch graphics, float xDisplacement = 0, float yDisplacement = 0);

        public bool Collide(CollidableEntity other, int xOffset, int yOffset)
        {
            return GetCollisionMask().Collide(other.GetCollisionMask(), xOffset, yOffset);
        }

        public Vector2 Position()
        {
            return new Vector2(x, y);
        }

        public void Displace(float _x, float _y)
        {
            x += _x;
            y += _y;
            if (y < 0) y = 0;
            GetCollisionMask().MoveTo((int) x, (int) y);
        }

        public abstract Point GetSize();

        public void SetWorld(World world)
        {
            myWorld = world;
        }

        public static HashSet<CollidableEntity> ExtractByCollisionGroup(Dictionary<string, HashSet<CollidableEntity>> collisions, CollidableEntity requester = null, params string[] groups)
        {
            HashSet<CollidableEntity> returnValue = new HashSet<CollidableEntity>();
            foreach (string group in groups)
            {
                if (collisions.ContainsKey(group))
                {
                    returnValue.UnionWith(collisions[group]);
                }
            }
            returnValue.RemoveWhere(entity => entity == requester);
            return returnValue;
        }

        protected bool WorldWrapCollisionCheck(CollidableEntity other, int xOffset, int yOffset)
        {
            var width = GetSize().X;
            if (x + xOffset + width > myWorld.pixelWorldWidth)
            {
                return Collide(other, xOffset - myWorld.pixelWorldWidth, yOffset);
            }
            if (x + xOffset <= 0)
            {
                return Collide(other, xOffset + myWorld.pixelWorldWidth, yOffset);
            }
            return false;
        }
    }
}
