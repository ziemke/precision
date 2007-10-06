using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class Player : Actor
    {
        #region Fields
        float invincibilityTime;

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
	
        #endregion

        #region Constructos
        internal Player(Texture2D texture) : base(texture)
        {
            Position = new Vector2(400, 300);
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
