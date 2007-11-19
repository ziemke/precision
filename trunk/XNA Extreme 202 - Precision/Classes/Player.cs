using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class Player : Actor
    {
        #region Fields
        internal static byte playersCount;
        float invincibilityTime;
        internal List<Texture2D> overlayTextures;
        float slowDown = 1f; 
        private byte playerID;
        #endregion

        #region Properties
        public byte PlayerID
        {
            get { return playerID; }
        }
	
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
           this.overlayTextures = new List<Texture2D>();
            playersCount++;
            this.playerID = playersCount;

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

        #region override Draw
        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (PrecisionGame.isCoopMode && this.Visible) spriteBatch.Draw(PrecisionGame.graphicsSetsContents[PrecisionGame.selectedGraphicsSet]["overlay"], new Rectangle((int)this.Position.X - (int)this.Origin.X, (int)this.Position.Y - (int)this.Origin.Y - 5, (int)this.texture.Width, 4), (this.playerID == 1) ? Color.Blue : Color.White);
            base.Draw(spriteBatch);
        }
        #endregion

        #region Draw overlay Textures
        internal void DrawOverlayTextures(SpriteBatch spriteBatch)
        {
                foreach (Texture2D overlayTexture in overlayTextures)
                    spriteBatch.Draw(overlayTexture, this.Position, null, this.tint, this.rotate, new Vector2(overlayTexture.Width / 2, overlayTexture.Height / 2), 1f, SpriteEffects.None, 1f);
        }
        #endregion

        #region Reset
        internal void Reset(float invincibilityTime)
        {
            if (PrecisionGame.isCoopMode)
            {
                if (this.PlayerID == 1)
                       this.Position = new Vector2(PrecisionGame.SCREEN_WIDTH / 4 , PrecisionGame.SCREEN_HEIGHT / 2);
                else
                       this.Position = new Vector2(PrecisionGame.SCREEN_WIDTH - PrecisionGame.SCREEN_WIDTH / 4, PrecisionGame.SCREEN_HEIGHT / 2);
            }
            else
                this.Position = new Vector2(PrecisionGame.SCREEN_WIDTH / 2 , PrecisionGame.SCREEN_HEIGHT / 2);
            this.invincibilityTime = invincibilityTime;
            this.Flicker = true;
        }
        #endregion
        #endregion
    }
}
