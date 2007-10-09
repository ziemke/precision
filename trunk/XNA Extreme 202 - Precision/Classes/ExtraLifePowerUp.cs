using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class ExtraLifePowerUp : nonRecurringPowerup
    {
        #region Constructors
        internal ExtraLifePowerUp(Texture2D texture, List<Color> pickupTimeBarColors)
            : base(texture, pickupTimeBarColors)
        {
            this.frequency = Config.EXTRALIFE_POWERUP_FREQUENCY;
            this.RespawnTime = Config.EXTRALIFE_POWERUP_RESPAWN_TIME;
            this.Position = -this.Origin;
            this.DefaultPickupTime = Config.EXTRALIFE_POWERUP_PICKUP_TIME;
            this.startLevel = Config.EXTRALIFE_POWERUP_START_LEVEL;
            this.frequency = Config.EXTRALIFE_POWERUP_FREQUENCY;
            
        }
        #endregion

        #region Methods
        #region override PickUp
        internal override void Pickup()
        {
            PrecisionGame.AddExtraLife();
            base.Pickup();
        }
        #endregion
        #endregion

    }
}
