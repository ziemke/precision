using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class ExtraLifePowerup : nonRecurringPowerup
    {
        #region Constructors
        internal ExtraLifePowerup(Texture2D texture, Color pickupTimeBarColor)
            : base(texture, pickupTimeBarColor)
        {
            this.respawnTimeRemainingRandomModifier = Config.EXTRALIFE_POWERUP_RESPAWN_TIME_RANDOM_MODIFIER;
            this.RespawnTime = Config.EXTRALIFE_POWERUP_RESPAWN_TIME;
            this.Position = -this.Origin;
            this.DefaultPickupTime = Config.EXTRALIFE_POWERUP_PICKUP_TIME;
            
        }
        #endregion

        #region Methods
        #region override PickUp
        internal override void Pickup()
        {
            Game1.AddExtraLife();
            base.Pickup();
        }
        #endregion
        #endregion

    }
}
