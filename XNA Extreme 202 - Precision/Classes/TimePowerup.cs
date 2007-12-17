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
            this.frequency = Config.TIME_POWERUP_FREQUENCY;
            this.RespawnTime = Config.TIME_POWERUP_RESPAWN_TIME;
            this.ActiveTime = Config.TIME_POWERUP_DURATION;
            this.SlowDownPercent = Config.TIME_POWERUP_SLOWDOWN;
            this.DefaultPickupTime = Config.TIME_POWERUP_PICKUP_TIME;
            this.startLevel = Config.TIME_POWERUP_START_LEVEL;
        }
        #endregion

        #region Methods
        #region override Activate
        protected override void Activate()
        {
            Actor.TimeScale = this.slowDownPercent;
            PrecisionGame.showSlowMotionEffect = true;
            Audio.Play(Audio.Cue.Powerup_Time);
        }
        #endregion

        #region override Deactivate
        protected override void Deactivate()
        {
            Actor.TimeScale = 1f;
            PrecisionGame.showSlowMotionEffect = false;
            Audio.StopCategory("Powerup_Time");
        }
        
        #endregion
        #endregion
    }
}
