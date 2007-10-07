using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class TimePowerup : enduringPowerup
    {
        #region Fields
        float slowDownPercent;
        #endregion

        #region Properties
        internal float SlowDownPercent
        {
            get { return slowDownPercent; }
            set { slowDownPercent = value; }
        }

        #endregion

        #region Constructors
        internal TimePowerup(Texture2D texture, Color activeTimeBarColor, List<Color> pickupTimeBarColors)
            : base(texture, activeTimeBarColor, pickupTimeBarColors)
        {
            this.Position = -this.Origin;
            this.respawnTimeRemainingRandomModifier = Config.TIME_POWERUP_RESPAWN_TIME_RANDOM_MODIFIER;
            this.RespawnTime = Config.TIME_POWERUP_RESPAWN_TIME;
            this.ActiveTime = Config.TIME_POWERUP_DURATION;
            this.SlowDownPercent = Config.TIME_POWERUP_SLOWDOWN;
            this.DefaultPickupTime = Config.TIME_POWERUP_PICKUP_TIME;
            this.activeTimeBar.Position = new Vector2(Game1.SCREEN_WIDTH / 2, Config.ACTIVE_TIMEBAR_HEIGHT);
        }
        #endregion

        #region Methods
        #region override Activate
        protected override void Activate()
        {
            Actor.TimeScale = this.slowDownPercent;
            //Game1.slowMoEffect.Visible = true;
            Game1.showSlowMotionEffect = true;
            base.Activate();
        }
        #endregion

        #region override Deactivate
        protected override void Deactivate()
        {
            Actor.TimeScale = 1f;
            //Game1.slowMoEffect.Visible = false;
            Game1.showSlowMotionEffect = false;
            base.Deactivate();
        }
        
        #endregion
        #endregion
    }
}
