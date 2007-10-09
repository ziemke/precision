using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class enduringPowerup : Powerup
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
                }
            }

            base.Update(gameTime);
        }

        #endregion

        #region Pickup
        internal override void Pickup()
        {
            this.activeTimeRemaining = this.activeTime;
            this.Activate();

            base.Pickup();
        }
        #endregion

        #region Activate
        protected virtual void Activate()
        { 
            this.activeTimeBar.Position = GetActiveTimeBarPosition();
        }
        #endregion

        #region Deactivate
        protected virtual void Deactivate()
        { }
        #endregion

        #region Get activeTimeBar Position
        private Vector2 GetActiveTimeBarPosition()
        {
            foreach (Bar bar in Bar.bars)
            {
                Vector2 subOrigin = new Vector2(0, Config.ACTIVE_TIMEBAR_ICON_SIZE - 10);

                if (Bar.bars.Count != 1)
                {
                    for (int i = Bar.bars.Count - 1; i > 0; i--)
                    {
                        Vector2 desiredPosition = new Vector2(PrecisionGame.SCREEN_WIDTH / 2, (Config.ACTIVE_TIMEBAR_ICON_SIZE + 10) * (Bar.bars.Count - i));

                        BackgroundBar backgroundBar = bar as BackgroundBar;
                        if (backgroundBar != null) break;

                        if (bar != this.activeTimeBar)
                            if (bar.Percent == 0f && bar.Position == desiredPosition - subOrigin || bar.Position != desiredPosition - subOrigin)
                                return desiredPosition - subOrigin;
                    }


                }
                else
                    return (new Vector2(PrecisionGame.SCREEN_WIDTH / 2, Config.ACTIVE_TIMEBAR_ICON_SIZE + 10) - subOrigin);
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
