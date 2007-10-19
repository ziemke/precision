using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class ScaleDownPowerUp : nonRecurringPowerup
    {
        #region Constructors
        internal ScaleDownPowerUp(Texture2D texture, List<Color> pickupTimeBarColors)
            : base(texture, pickupTimeBarColors)
        {
            this.frequency = Config.SCALE_DOWN_POWERUP_FREQUENCY;
            this.RespawnTime = Config.SCALE_DOWN_POWERUP_RESPAWN_TIME;
            this.Position = -this.Origin;
            this.DefaultPickupTime = Config.SCALE_DOWN_POWERUP_PICKUP_TIME;
            this.startLevel = Config.SCALE_DOWN_POWERUP_START_LEVEL;
            this.frequency = Config.SCALE_DOWN_POWERUP_FREQUENCY;
            
        }
        #endregion

        #region Methods
        #region override PickUp
        internal override void Pickup()
        {
            PrecisionGame.ScaleEnemiesDown(Config.SCALE_DOWN_POWERUP_SCALE);
            base.Pickup();
        }
        #endregion
        #endregion

    }
}
