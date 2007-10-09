using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class nonRecurringPowerup : Powerup
    {
        #region Constructors
        internal nonRecurringPowerup(Texture2D texture, List<Color> pickupTimeBarColors)
            : base(texture, pickupTimeBarColors)
        {
        }
        #endregion

        #region Methods
        #region Pickup
        internal override void Pickup()
        {
            this.respawnTimeRemaining = this.respawnTime + PrecisionGame.Range(-frequency, frequency);

            base.Pickup();
        }
        #endregion
        #endregion
    }
}
