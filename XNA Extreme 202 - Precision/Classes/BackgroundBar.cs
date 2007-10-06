using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class BackgroundBar : Bar
    {
        #region Fields
        const float BAR_COLOR_SHADE = 0.7f;
        #endregion

        #region Constructors
        internal BackgroundBar(int width, int height, List<Color> colors)
            : base(width, height, colors)
        {
            this.alignment = BarAlignment.Left;
        }
        #endregion


        #region Methods
        #region new Draw
        new internal void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this.color = GetLifeBarColor(colors, percent);

            Vector2 origin = new Vector2((float)this.alignment, 0);
            Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, (int)(this.width * percent), height);
            Rectangle destBackgroundRect = new Rectangle((int)position.X - 1, (int)position.Y - 1, (int)this.width + 2, height + 2);

            spriteBatch.Draw(Bar.texture, destBackgroundRect, null, new Color((byte)(this.color.R * BAR_COLOR_SHADE), (byte)(this.color.G * BAR_COLOR_SHADE), (byte)(this.color.B * BAR_COLOR_SHADE)), 0f, origin, SpriteEffects.None, 0f);
            spriteBatch.Draw(Bar.texture, destRect, null, this.color, 0f, origin, SpriteEffects.None, 0f);
        }
        #endregion
        #endregion

    }
}
