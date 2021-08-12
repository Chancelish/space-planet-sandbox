using Microsoft.Xna.Framework;
using System;

namespace space_planet_sandbox.collisiondetection
{
    public interface ICollisionMask
    {
        public bool Collide(ICollisionMask _other, int _xOffset, int _yOffset);

        public bool Collide(Point point);

        public bool Collide(Rectangle rectangle);

        public bool Collide(CollisionGrid grid);
    }

    public abstract class IHitBox : ICollisionMask
    {
        public abstract Rectangle BoundingBox();

        public bool Collide(ICollisionMask _other, int _xOffset, int _yOffset)
        {
            var displacedRectangle = new Rectangle(BoundingBox().X - _xOffset, BoundingBox().Y - _yOffset, BoundingBox().Width, BoundingBox().Height);
            return _other.Collide(displacedRectangle);
        }

        public bool Collide(Point point)
        {
            return BoundingBox().Contains(point);
        }

        public bool Collide(Rectangle rectangle)
        {
            return BoundingBox().Intersects(rectangle);
        }

        public bool Collide(CollisionGrid grid)
        {
            return grid.Intersects(BoundingBox());
        }
    }

    public abstract class IPointMask : ICollisionMask
    {
        public abstract Point Location();
        public bool Collide(ICollisionMask _other, int _xOffset, int _yOffset)
        {
            var displacedPoint = new Point(Location().X - _xOffset, Location().Y - _yOffset);
            return _other.Collide(displacedPoint);
        }

        public bool Collide(Point point)
        {
            return Location() == point;
        }

        public bool Collide(Rectangle rectangle)
        {
            return rectangle.Contains(Location());
        }

        public bool Collide(CollisionGrid grid)
        {
            return grid.Contains(Location());
        }
    }

    public class CollisionGrid
    {
        public int tileSize;
        public int[,] solidTiles;
        public int gridHeight;
        public int gridWidth;
        public Point zeroOrigin;

        public CollisionGrid(int tSize, int width, int height, int x, int y)
        {
            tileSize = tSize;
            gridHeight = height;
            gridWidth = width;
            solidTiles = new int[width, height];
            zeroOrigin = new Point(x, y);
        }

        public bool Contains(Point point)
        {
            var relativePoint = point - zeroOrigin;
            if (relativePoint.X < 0 || relativePoint.Y < 0) {
                return false;
            }
            int xIndex = relativePoint.X / tileSize;
            int yIndex = relativePoint.Y / tileSize;
            if (xIndex >=  gridWidth || yIndex >= gridHeight)
            {
                return false;
            }
            return solidTiles[xIndex, yIndex] == 1;
        }

        public bool Intersects(Rectangle rectangle)
        {
            var upperLeft = rectangle.Location - zeroOrigin;
            var lowerRight = upperLeft + rectangle.Size;
            upperLeft.X /= tileSize;
            upperLeft.Y /= tileSize;
            lowerRight.X /= tileSize;
            lowerRight.Y /= tileSize;
            for (int i = Math.Max(upperLeft.X, 0); i < Math.Min(lowerRight.X, gridWidth); i++)
            {
                for (int j = Math.Max(upperLeft.Y, 0); j < Math.Min(lowerRight.Y, gridHeight); j++)
                {
                    if (solidTiles[i,j] == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    public abstract class IGridMask : ICollisionMask
    {
        public abstract CollisionGrid GetCollisionGrid();

        public bool Collide(ICollisionMask _other, int _xOffset, int _yOffset)
        {
            CollisionGrid g = GetCollisionGrid();
            var displacedGrid = new CollisionGrid(g.tileSize, g.gridWidth, g.gridHeight, g.zeroOrigin.X - _xOffset, g.zeroOrigin.Y);
            return Collide(displacedGrid);
        }

        public bool Collide(Point point)
        {
            return GetCollisionGrid().Contains(point);
        }

        public bool Collide(Rectangle rectangle)
        {
            return GetCollisionGrid().Intersects(rectangle);
        }

        public bool Collide(CollisionGrid grid)
        {
            return false; // NO, just NO!
        }
    }
}
