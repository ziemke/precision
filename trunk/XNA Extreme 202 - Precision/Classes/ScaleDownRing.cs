using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class ScaleDownRing : Actor
    {
        #region Constructors
		internal ScaleDownRing(Texture2D texture) : base(texture)
        {
            this.scale = PrecisionGame.ScaleToFit(new Vector2(this.texture.Width, this.texture.Height), new Vector2(PrecisionGame.player.Radius * 2));
            this.Position = PrecisionGame.player.Position;
            this.tint = Color.LightBlue;
            this.StartScale(PrecisionGame.ScaleToFit(new Vector2(this.texture.Width, this.texture.Height), new Vector2(PrecisionGame.SCREEN_WIDTH * 2.5f)), Config.SCALE_DOWN_POWERUP_SCALE_TIME);
        }
	    #endregion 

        #region Methods
        #region override Update
        internal override void Update(GameTime gameTime)
        {
            if (!this.isScaling) actors.Remove(this);

            for (int i = actors.Count - 1; i >= 0; i--)
            {
                Enemy enemy = actors[i] as Enemy;
                if (enemy != null)
                {
                    Rectangle ringRectangle = new Rectangle((int)(this.Position.X - this.Origin.X * this.scale), (int)(this.Position.Y - this.Origin.Y * this.scale), this.texture.Width, this.texture.Height);
                    Rectangle enemyRectangle = new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, 1, 1);
                    if (ringRectangle.Contains(enemyRectangle))
                    {
                        enemy.StartScale(Config.SCALE_DOWN_POWERUP_SCALE, Config.SCALE_DOWN_POWERUP_SCALE_TIME);
                    }
                }
            }
            base.Update(gameTime);
        }
        #endregion
        #endregion
    }
}
