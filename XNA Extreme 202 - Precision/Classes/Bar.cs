using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class Bar
    {
        internal enum BarAlignment { Left, Center, Right };
        #region Fields
        protected Vector2 position;
        protected int width;
        protected int height;
        protected float percent;

        protected BarAlignment alignment;
        protected Color color;
        protected List<Color> colors;
      

        internal static Texture2D texture;
        internal static List<Bar> bars;

        #endregion

        #region Properties
        internal Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        internal int Height
        {
            get { return height; }
            set { height = value; }
        }

        internal int Width
        {
            get { return width; }
            set { width = value; }
        }

        internal BarAlignment Alignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        internal float Percent        
        {
            get { return percent; }
            set { percent = value; }
        }

        internal bool isVisible
        {
            get { return percent > 0f && position.X >= 0f && position.Y >= 0f; }
        }
        #endregion

        #region Constructors
        static Bar()
        {
            bars = new List<Bar>();
        }

        internal Bar(int width, int height, List<Color> colors)
        {
            this.width = width;
            this.height = height;
            this.percent = 0f;
            this.colors = colors;
            bars.Add(this);
        }
        #endregion

        #region Methods
        #region DrawBars
        internal static void DrawBars(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bars.Count; i++)
            {
                BackgroundBar backgroundBar = bars[i] as BackgroundBar;
                if (backgroundBar == null)
                    bars[i].Draw(spriteBatch);
            }
        }
        #endregion

        #region DrawBackgroundBars
        internal static void DrawBackgroundBars(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bars.Count; i++)
            {
                BackgroundBar backgroundBar = bars[i] as BackgroundBar;
                if (backgroundBar != null)
                    backgroundBar.Draw(spriteBatch);
            }
        }
        #endregion

        #region  Draw
        internal void Draw(SpriteBatch spriteBatch)
        {
           this.color = PrecisionGame.GetLifeBarColor(colors, percent);

            Vector2 origin = new Vector2((float)this.alignment, 0);
            Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, (int)(this.width * percent), height);
            
            spriteBatch.Draw(Bar.texture, destRect, null, this.color, 0f, origin, SpriteEffects.None, 0f);
            
        }
        #endregion

      
        #endregion
    }
}
