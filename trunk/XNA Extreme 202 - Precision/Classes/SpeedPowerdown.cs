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
        }
        #endregion

        #region Methods
        #region override Activate
        protected override void Activate()
        {

            Game1.player.SlowDown = Config.SPEED_POWERDOWN_SLOWDOWN;
            Game1.player.overlayTextures.Add(this.playerTexture);

            base.Activate();
        }
        #endregion

        #region override Deactivate
        protected override void Deactivate()
        {

            Game1.player.SlowDown = 1f;
            Game1.player.overlayTextures.Remove(this.playerTexture);

            base.Deactivate();
        }
        #endregion
        #endregion
    }
}
