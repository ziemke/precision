using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class ShieldPowerup : enduringPowerup
    {
        #region Constructors
        internal ShieldPowerup(Texture2D texture, Color activeTimeBarColor, Color pickupTimeBarColor)
            : base(texture, activeTimeBarColor, pickupTimeBarColor)
        {
            this.Position = -this.Origin;
            this.RespawnTime = Config.SHIELD_POWERUP_RESPAWN_TIME;
            this.ActiveTime = Config.SHIELD_POWERUP_DURATION;
            this.DefaultPickupTime = Config.TIME_POWERUP_PICKUP_TIME;
            this.activeTimeBar.Position = new Vector2(Game1.SCREEN_WIDTH / 2, 3 * Config.ACTIVE_TIMEBAR_HEIGHT);
        }
        #endregion

        #region Methods
        #region override Activate
        protected override void Activate()
        {
            for (int i = actors.Count - 1; i >= 0; i--)
            {

                if (actors[i] is Player)
                {
                    Player player = actors[i] as Player;
                    player.InvincibilityTime = Config.SHIELD_POWERUP_DURATION; ;
                    //player.Flicker = true;
                    player.overlayTextures.Add(this.texture);
                    break;
                }
            }
            base.Activate();
        }
        #endregion

        #region override Deactivate
        protected override void Deactivate()
        {
            for (int i = actors.Count - 1; i >= 0; i--)
            {

                if (actors[i] is Player)
                {
                    Player player = actors[i] as Player;
                    player.InvincibilityTime = 0;
                    //player.Flicker = false;
                    player.overlayTextures.Remove(this.texture);
                    break;
                }
            }
            base.Deactivate();
        }
        #endregion
        #endregion
    }
}
