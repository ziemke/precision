using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    abstract class enduringPowerup : Powerup
    {
        #region Fields
        float activeTime;
        protected float activeTimeRemaining;
        internal Bar activeTimeBar;
        float iconScale;
        #endregion

        #region Properties
        internal float ActiveTime
        {
            get { return activeTime; }
            set { activeTime = value; }
        }
        #endregion

        #region Constructors
        internal enduringPowerup(Texture2D texture, Color activeTimeBarColor, List<Color> pickupTimeBarColors)
            : base(texture, pickupTimeBarColors)
        {
            List<Color> activeTimeBarColorList = new List<Color>();
            activeTimeBarColorList.Add(activeTimeBarColor);
            this.activeTimeBar = new Bar(Config.ACTIVE_TIMEBAR_WIDTH, Config.ACTIVE_TIMEBAR_HEIGHT, activeTimeBarColorList);
            this.activeTimeBar.Alignment = Bar.BarAlignment.Center;
            this.activeTimeBar.Percent = 0f;
            iconScale = PrecisionGame.ScaleToFit(new Vector2(this.texture.Width, this.texture.Height), new Vector2(Config.ACTIVE_TIMEBAR_ICON_SIZE));
        }
        #endregion

        #region Methods
        #region Update
        internal override void Update(GameTime gameTime)
        {
            if (this.activeTimeRemaining > 0f)
            {
                this.activeTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.activeTimeBar.Percent = this.activeTimeRemaining / this.activeTime;
                if (this.activeTimeRemaining <= 0f)
                {
                    this.Deactivate();
                    this.activeTimeBar.Percent = 0f;
                    this.respawnTimeRemaining = this.RespawnTime;
                }
            }

            if(this.pickupTimeRemaining <= 0 && this.activeTimeRemaining <= 0 && this.respawnTimeRemaining <= 0)
                this.respawnTimeRemaining = this.RespawnTime;

            base.Update(gameTime);
        }

        #endregion

        #region Pickup
        internal override void Pickup()
        {
            this.activeTimeRemaining = this.activeTime;
            this.activeTimeBar.Position = GetActiveTimeBarPosition();
            this.Activate();

            base.Pickup();
        }
        #endregion

        #region Activate
        protected abstract void Activate();
        #endregion

        #region Deactivate
        protected abstract void Deactivate();
        #endregion

        #region Get activeTimeBar Position
        private Vector2 GetActiveTimeBarPosition()
        {
            Vector2 desiredPositionStart = new Vector2(PrecisionGame.SCREEN_WIDTH / 2, (Config.ACTIVE_TIMEBAR_ICON_SIZE + 16));
            Vector2 desiredPosition = new Vector2();
            Vector2 subOrigin = new Vector2(0, Config.ACTIVE_TIMEBAR_ICON_SIZE - 16);

            for (int c = 0; c < Bar.bars.Count + 1; c++)
  			{
                desiredPosition = desiredPositionStart;
                desiredPosition.Y = (c + 1) * desiredPositionStart.Y;
                bool desiredPositionTaken = false;
                foreach (Bar bar in Bar.bars)
                {
                  BackgroundBar backgroundBar = bar as BackgroundBar;
                  if (backgroundBar != null) continue;
                  if (bar.Percent == 0f) continue;
                  if (bar != this.activeTimeBar && bar.Position == desiredPosition - subOrigin)
                      desiredPositionTaken = true;                    
                 }
                 if (!desiredPositionTaken) return desiredPosition - subOrigin;
           }

            return Vector2.Zero;
        }
        #endregion

        #region override Draw
        internal override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 iconPosition = new Vector2(this.activeTimeBar.Position.X, this.activeTimeBar.Position.Y + activeTimeBar.Height / 2);
            if (this.activeTimeRemaining > 0f) 
                spriteBatch.Draw(this.texture, iconPosition, null, Color.White, 0f, this.Origin, iconScale, SpriteEffects.None, 0);
            base.Draw(spriteBatch);
        }
        #endregion
        #endregion
    }
}
