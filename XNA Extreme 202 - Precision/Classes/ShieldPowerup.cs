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
            this.frequency = Config.SHIELD_POWERUP_FREQUENCY;
            this.startLevel = Config.SHIELD_POWERUP_START_LEVEL;
        }
        #endregion

        #region Methods
        #region override Activate
        protected override void Activate()
        {
           
            PrecisionGame.player.InvincibilityTime = Config.SHIELD_POWERUP_DURATION;
            PrecisionGame.player.overlayTextures.Add(this.texture);

            if (PrecisionGame.isCoopMode)
            {
                PrecisionGame.player2.InvincibilityTime = Config.SHIELD_POWERUP_DURATION;
                PrecisionGame.player2.overlayTextures.Add(this.texture);
            }
        }
        #endregion

        #region override Deactivate
        protected override void Deactivate()
        {
          
            PrecisionGame.player.InvincibilityTime = 0;
            PrecisionGame.player.overlayTextures.Remove(this.texture);

            if (PrecisionGame.isCoopMode)
            {
                PrecisionGame.player2.InvincibilityTime = 0;
                PrecisionGame.player2.overlayTextures.Remove(this.texture);
            }
        }
        #endregion
        #endregion
    }
}
