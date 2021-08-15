using Microsoft.Xna.Framework;
using space_planet_sandbox.entities;

namespace space_planet_sandbox.rendering
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        public int x;
        public int y;

        public void Follow(CollidableEntity target, int width, int height)
        {
            var tartetLocation = target.Position();
            x = (int) tartetLocation.X - width / 2;
            y = (int) tartetLocation.Y - height / 2;
            var position = Matrix.CreateTranslation(-tartetLocation.X, -tartetLocation.Y, 0);
            var offset = Matrix.CreateTranslation(width / 2, height / 2, 0);
            Transform = position * offset;
        }
    }
}
