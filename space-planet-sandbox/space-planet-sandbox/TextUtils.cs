using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace space_planet_sandbox
{
    public sealed class TextUtils
    {
        public static void RenderOutlinedText(SpriteBatch graphics, SpriteFont font, string text, int x, int y)
        {
            var textLocation = new Vector2(x - 1, y - 1);
            graphics.DrawString(font, text, textLocation, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            var textLocation2 = new Vector2(x + 1, y + 1);
            graphics.DrawString(font, text, textLocation2, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            var textLocation3 = new Vector2(x + 1, y - 1);
            graphics.DrawString(font, text, textLocation3, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            var textLocation4 = new Vector2(x - 1, y + 1);
            graphics.DrawString(font, text, textLocation4, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            var textLocation5 = new Vector2(x, y);
            graphics.DrawString(font, text, textLocation5, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
