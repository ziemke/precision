using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class ScorePowerup : enduringPowerup
    {
        #region Constructors
        internal ScorePowerup(Texture2D texture, Color activeTimeBarColor, List<Color> pickupTimeBarColors)
            : base(texture, activeTimeBarColor, pickupTimeBarColors)
        {
            this.Position = -this.Origin;
            this.RespawnTime = Config.SCORE_POWERUP_RESPAWN_TIME;
            this.ActiveTime = Config.SCORE_POWERUP_DURATION;
            this.DefaultPickupTime = Config.SCORE_POWERUP_PICKUP_TIME;
            this.activeTimeBar.Position = new Vector2(PrecisionGame.SCREEN_WIDTH / 2, 3 * Config.ACTIVE_TIMEBAR_HEIGHT);
            this.startLevel = Config.SCORE_POWERUP_START_LEVEL;
            this.frequency = Config.SCORE_POWERUP_FREQUENCY;
        }
        #endregion

        #region Methods
        #region override Activate
        protected override void Activate()
        {
            PrecisionGame.scoreMultiplicator *= 2;
            base.Activate();
        }
        #endregion

        #region override Deactivate
        protected override void Deactivate()
        {
            PrecisionGame.scoreMultiplicator /= 2;
            base.Deactivate();
        }
        
        #endregion
        #endregion
    }
}
