using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class ScorePowerup : enduringPowerup
    {
        #region Constructors
        internal ScorePowerup(Texture2D texture, Color activeTimeBarColor, Color pickupTimeBarColor)
            : base(texture, activeTimeBarColor, pickupTimeBarColor)
        {
            this.Position = -this.Origin;
            this.RespawnTime = Config.SCORE_POWERUP_RESPAWN_TIME;
            this.ActiveTime = Config.SCORE_POWERUP_DURATION;
            this.DefaultPickupTime = Config.TIME_POWERUP_PICKUP_TIME;
            this.activeTimeBar.Position = new Vector2(Game1.SCREEN_WIDTH / 2, 3 * Config.ACTIVE_TIMEBAR_HEIGHT);
        }
        #endregion

        #region Methods
        #region override Activate
        protected override void Activate()
        {
            Game1.scoreMultiplicator *= 2;
            base.Activate();
        }
        #endregion

        #region override Deactivate
        protected override void Deactivate()
        {
            Game1.scoreMultiplicator /= 2;
            base.Deactivate();
        }
        
        #endregion
        #endregion
    }
}
