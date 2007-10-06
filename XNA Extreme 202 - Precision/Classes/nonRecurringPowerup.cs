using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class nonRecurringPowerup : Powerup
    {
        #region Constructors
        internal nonRecurringPowerup(Texture2D texture, Color pickupTimeBarColor)
            : base(texture, pickupTimeBarColor)
        {
        }
        #endregion

        #region Methods
        #region Pickup
        internal override void Pickup()
        {
            this.respawnTimeRemaining = this.respawnTime + Game1.Range(-respawnTimeRemainingRandomModifier, respawnTimeRemainingRandomModifier);

            base.Pickup();
        }
        #endregion
        #endregion
    }
}
