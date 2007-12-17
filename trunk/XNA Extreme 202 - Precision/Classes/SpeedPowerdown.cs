using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class SpeedPowerdown : enduringPowerup
    {
        #region Fields
        Texture2D playerTexture;
        #endregion

        #region Constructors
        internal SpeedPowerdown(Texture2D texture, Texture2D playerTexture, Color activeTimeBarColor, List<Color> pickupTimeBarColors)
            : base(texture, activeTimeBarColor, pickupTimeBarColors)
        {
            this.Position = -this.Origin;
            this.RespawnTime = Config.SPEED_POWERDOWN_RESPAWN_TIME;
            this.ActiveTime = Config.SPEED_POWERDOWN_DURATION;
            this.DefaultPickupTime = Config.SPEED_POWERDOWN_PICKUP_TIME;
            this.playerTexture = playerTexture;
            this.frequency = Config.SPEED_POWERDOWN_FREQUENCY;
            this.startLevel = Config.SPEED_POWERDOWN_START_LEVEL;
        }
        #endregion

        #region Methods
        #region override Activate
        protected override void Activate()
        {

            PrecisionGame.player.SlowDown = Config.SPEED_POWERDOWN_SLOWDOWN;
            PrecisionGame.player.overlayTextures.Add(this.playerTexture);

            if (PrecisionGame.isCoopMode)
            {
                PrecisionGame.player2.SlowDown = Config.SPEED_POWERDOWN_SLOWDOWN;
                PrecisionGame.player2.overlayTextures.Add(this.playerTexture);
            }
        }
        #endregion

        #region override Deactivate
        protected override void Deactivate()
        {

            PrecisionGame.player.SlowDown = 1f;
            PrecisionGame.player.overlayTextures.Remove(this.playerTexture);

            if (PrecisionGame.isCoopMode)
            {
                PrecisionGame.player2.SlowDown = 1f;
                PrecisionGame.player2.overlayTextures.Remove(this.playerTexture);
            }
        }
        #endregion
        #endregion
    }
}
