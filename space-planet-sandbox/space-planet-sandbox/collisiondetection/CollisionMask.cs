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

        public void MoveTo(int x, int y);
    }

    public class HitBox : ICollisionMask
    {
        private Rectangle boundingBox;

        public HitBox(int x, int y, int width, int height)
        {
            boundingBox = new Rectangle(x, y, width, height);
        }

        public bool Collide(ICollisionMask _other, int _xOffset, int _yOffset)
        {
            var displacedRectangle = new Rectangle(boundingBox.X + _xOffset, boundingBox.Y + _yOffset, boundingBox.Width, boundingBox.Height);
            return _other.Collide(displacedRectangle);
        }

        public bool Collide(Point point)
        {
            return boundingBox.Contains(point);
        }

        public bool Collide(Rectangle rectangle)
        {
            return boundingBox.Intersects(rectangle);
        }

        public bool Collide(CollisionGrid grid)
        {
            return grid.Intersects(boundingBox);
        }

        public void MoveTo(int x, int y)
        {
            boundingBox.X = x;
            boundingBox.Y = y;
        }

        public void Resize(int width, int height)
        {
            boundingBox.Width = width;
            boundingBox.Height = height;
        }

        public Point Size()
        {
            return boundingBox.Size;
        }
    }

    public class PointMask : ICollisionMask
    {
        private Point pointLocation;

        public PointMask(int x, int y)
        {
            pointLocation = new Point(x, y);
        }

        public bool Collide(ICollisionMask _other, int _xOffset, int _yOffset)
        {
            var displacedPoint = new Point(pointLocation.X + _xOffset, pointLocation.Y + _yOffset);
            return _other.Collide(displacedPoint);
        }

        public bool Collide(Point point)
        {
            return pointLocation == point;
        }

        public bool Collide(Rectangle rectangle)
        {
            return rectangle.Contains(pointLocation);
        }

        public bool Collide(CollisionGrid grid)
        {
            return grid.Contains(pointLocation);
        }

        public void MoveTo(int x, int y)
        {
            pointLocation.X = x;
            pointLocation.Y = y;
        }
    }

    public class CollisionGrid
    {
        public int tileSize;
        public int[,] solidTiles;
        public readonly int gridHeight;
        public readonly int gridWidth;
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
            int ix = Math.Max(0, upperLeft.X / tileSize);
            int fx = Math.Min(gridWidth - 1, (lowerRight.X - 1) / tileSize);
            int iy = Math.Max(0, upperLeft.Y / tileSize);
            int fy = Math.Min(gridHeight - 1, (lowerRight.Y - 1) / tileSize);
            for (int i = ix; i <= fx; i++)
            {
                for (int j = iy; j <= fy; j++)
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

    public class GridMask : ICollisionMask
    {
        private CollisionGrid grid;

        public GridMask(int tileSize, int width, int height, int x = 0, int y = 0)
        {
            grid = new CollisionGrid(tileSize, width, height, x, y);
        }

        public bool Collide(ICollisionMask _other, int _xOffset, int _yOffset)
        {
            var displacedGrid = new CollisionGrid(grid.tileSize, grid.gridWidth, grid.gridHeight, grid.zeroOrigin.X + _xOffset, grid.zeroOrigin.Y + _yOffset);
            return Collide(displacedGrid);
        }

        public bool Collide(Point point)
        {
            return grid.Contains(point);
        }

        public bool Collide(Rectangle rectangle)
        {
            return grid.Intersects(rectangle);
        }

        public bool Collide(CollisionGrid grid)
        {
            return false; // NO, just NO!
        }

        // Although supported, please don't move grids, it makes things complicated.
        public void MoveTo(int x, int y)
        {
            grid.zeroOrigin.X = x;
            grid.zeroOrigin.Y = y;
        }

        public void ChangeTile(int x, int y, int tileType)
        {
            grid.solidTiles[x, y] = tileType;
        }
    }
}
