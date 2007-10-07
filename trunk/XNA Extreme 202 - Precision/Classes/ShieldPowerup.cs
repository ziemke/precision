using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class ShieldPowerup : enduringPowerup
    {
        #region Constructors
        internal ShieldPowerup(Texture2D texture, Color activeTimeBarColor, List<Color> pickupTimeBarColors)
            : base(texture, activeTimeBarColor, pickupTimeBarColors)
        {
            this.Position = -this.Origin;
            this.RespawnTime = Config.SHIELD_POWERUP_RESPAWN_TIME;
            this.ActiveTime = Config.SHIELD_POWERUP_DURATION;
            this.DefaultPickupTime = Config.SHIELD_POWERUP_PICKUP_TIME;
        }
        #endregion

        #region Methods
        #region override Activate
        protected override void Activate()
        {
           
            Game1.player.InvincibilityTime = Config.SHIELD_POWERUP_DURATION;
            Game1.player.overlayTextures.Add(this.texture);

            base.Activate();
        }
        #endregion

        #region override Deactivate
        protected override void Deactivate()
        {
          
            Game1.player.InvincibilityTime = 0;
            Game1.player.overlayTextures.Remove(this.texture);

            base.Deactivate();
        }
        #endregion
        #endregion
    }
}
