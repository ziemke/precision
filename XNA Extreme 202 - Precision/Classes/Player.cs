using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class Player : Actor
    {
        #region Fields
        float invincibilityTime;
        internal List<Texture2D> overlayTextures;
        float slowDown = 1f; 
        #endregion

        #region Properties
        internal bool IsInvincible
        {
            get { return invincibilityTime > 0f; }
        }

        internal float InvincibilityTime
        {
            get { return invincibilityTime; }
            set { invincibilityTime = value; }
        }

        internal float SlowDown
        {
            get { return slowDown; }
            set { slowDown = value; }
        }
	
	
        #endregion

        #region Constructos
        internal Player(Texture2D texture) : base(texture)
        {
            overlayTextures = new List<Texture2D>();
        }
        #endregion

        #region Methods
        #region override Update
        internal override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.IsInvincible)
            {
                this.invincibilityTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (!this.IsInvincible)
                    this.Flicker = false;
            }
            base.Update(gameTime);
        }
        #endregion
        
        #region Draw overlay Textures
        internal void DrawOverlayTextures(SpriteBatch spriteBatch)
        {
                foreach (Texture2D overlayTexture in overlayTextures)
                    spriteBatch.Draw(overlayTexture, this.Position, null, this.tint, this.rotate, this.Origin, Game1.ScaleToFit(new Vector2(overlayTexture.Width, overlayTexture.Height), new Vector2(this.texture.Width, this.texture.Height)), SpriteEffects.None, 1f);
        }
        #endregion

        #region Reset
        internal void Reset(float invincibilityTime)
        {
            this.Position = new Vector2(Game1.SCREEN_WIDTH / 2, Game1.SCREEN_HEIGHT / 2);
            this.invincibilityTime = invincibilityTime;
            this.Flicker = true;
        }
        #endregion
        #endregion
    }
}
