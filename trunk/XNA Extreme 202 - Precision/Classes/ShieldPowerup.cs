using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class ShieldPowerup : enduringPowerup
    {
        #region Fields
        const float INVINCIBLE_POWERUP_RESPAWN_TIME = 0f;
        const float INVINCIBLE_POWERUP_DURATION = 3f;

        #endregion
        #region Constructors
        internal ShieldPowerup(Texture2D texture, Color activeTimeBarColor, Color pickupTimeBarColor)
            : base(texture, activeTimeBarColor, pickupTimeBarColor)
        {
            activeTimeBar.Position += new Vector2(0, Config.ACTIVE_TIMEBAR_HEIGHT + Config.ACTIVE_TIMEBAR_HEIGHT / 2);   
        }
        #endregion
        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #region Methods
        #region override Activate
        protected override void Activate()
        {
            for (int i = actors.Count - 1; i >= 0; i--)
            {

                if (actors[i] is Player)
                {
                    Player player = actors[i] as Player;
                    player.InvincibilityTime = 3f;
                    player.Flicker = true;
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
                    player.Flicker = false;
                    break;
                }
            }
            base.Deactivate();
        }
        #endregion


        #region Insert Invincible Powerups
        internal static void InsertShieldPowerups(Texture2D texture, int num)
        {
            for (int i = 0; i < num; i++)
            {
                ShieldPowerup ShieldPowerup = new ShieldPowerup(texture, Color.Green, Color.White);
                ShieldPowerup.Position = -ShieldPowerup.Origin;
                ShieldPowerup.RespawnTime = INVINCIBLE_POWERUP_RESPAWN_TIME;
                ShieldPowerup.ActiveTime = INVINCIBLE_POWERUP_DURATION;
                ShieldPowerup.Position = Game1.GetRandomScreenPosition(texture.Width / 2);
                ShieldPowerup = null;
             }
        }
        #endregion

        #endregion
    }
}
