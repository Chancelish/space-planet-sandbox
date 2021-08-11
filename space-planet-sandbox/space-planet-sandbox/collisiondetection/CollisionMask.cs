using Microsoft.Xna.Framework;
using System;

namespace space_planet_sandbox.collisiondetection
{
    public interface ICollisionMask
    {
        public virtual bool Collide(ICollisionMask _other, int _xOffset, int _yOffset)
        {
            throw new NotImplementedException("You Must Override Base Collision Mask");
        }

        public virtual bool Collide(Point point)
        {
            throw new NotImplementedException("You Must Override Base Collision Mask");
        }

        public virtual bool Collide(Rectangle rectangle)
        {
            throw new NotImplementedException("You Must Override Base Collision Mask");
        }
    }

    public interface IHitBox : ICollisionMask
    {
        public abstract Rectangle BoundingBox();

        public new bool Collide(ICollisionMask _other, int _xOffset, int _yOffset)
        {
            var displacedRectangle = new Rectangle(BoundingBox().X - _xOffset, BoundingBox().Y - _yOffset, BoundingBox().Width, BoundingBox().Height);
            return _other.Collide(displacedRectangle);
        }

        public new bool Collide(Point point)
        {
            return BoundingBox().Contains(point);
        }

        public new bool Collide(Rectangle rectangle)
        {
            return BoundingBox().Intersects(rectangle);
        }
    }

    public interface IPointMask : ICollisionMask
    {
        public abstract Point Location();
        public new bool Collide(ICollisionMask _other, int _xOffset, int _yOffset)
        {
            var displacedPoint = new Point(Location().X - _xOffset, Location().Y - _yOffset);
            return _other.Collide(displacedPoint);
        }

        public new bool Collide(Point point)
        {
            return Location() == point;
        }

        public new bool Collide(Rectangle rectangle)
        {
            return rectangle.Contains(Location());
        }
    }

    interface IGridMask : ICollisionMask
    {

    }
}
