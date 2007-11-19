using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class DestructionPowerup : enduringPowerup
    {
        #region Fields
        Texture2D playerTexture;
        #endregion

        #region Constructors
        internal DestructionPowerup(Texture2D texture, Texture2D playerTexture, Color activeTimeBarColor, List<Color> pickupTimeBarColors)
            : base(texture, activeTimeBarColor, pickupTimeBarColors)
        {
            this.Position = -this.Origin;
            this.RespawnTime = Config.DESTRUCTION_POWERUP_RESPAWN_TIME;
            this.ActiveTime = Config.DESTRUCTION_POWERUP_DURATION;
            this.DefaultPickupTime = Config.DESTRUCTION_POWERUP_PICKUP_TIME;
            this.playerTexture = playerTexture;
            this.frequency = Config.DESTRUCTION_POWERUP_FREQUENCY;
            this.startLevel = Config.DESTRUCTION_POWERUP_START_LEVEL;
        }
        #endregion

        #region Methods
        #region override Update
        internal override void Update(GameTime gameTime)
        {
            if (activeTimeRemaining > 0)
            {
                for (int i = Actor.actors.Count - 1; i >= 0; i--)
                {
                    Actor actor = Actor.actors[i];

                    Enemy enemy = actor as Enemy;
                    if (enemy != null)
                    {
                        if (!enemy.IsDying && Actor.CheckCollision(enemy, PrecisionGame.player) || (Actor.CheckCollision(enemy, PrecisionGame.player2) && PrecisionGame.isCoopMode))
                        {
                            Vibration.SetVibration(0.3f, 0.3f, new System.TimeSpan(0, 0, 0, 0, 200));
                            enemy.Kill();
                        }

                        continue;
                    }

                    //Cell cell = actor as Cell;
                    //if (cell != null)
                    //{

                    //  if (Actor.CheckCollision(cell, PrecisionGame.player) || (Actor.CheckCollision(cell, PrecisionGame.player2) && PrecisionGame.isCoopMode))
                    //        cell.Kill();
                    //}
                }
            }
            base.Update(gameTime);
        }
        #endregion

        #region override Activate
        protected override void Activate()
        {
            PrecisionGame.player.InvincibilityTime = Config.DESTRUCTION_POWERUP_DURATION;
            PrecisionGame.player.overlayTextures.Add(this.playerTexture);
            if (PrecisionGame.isCoopMode)
            {
                PrecisionGame.player2.InvincibilityTime = Config.DESTRUCTION_POWERUP_DURATION;
                PrecisionGame.player2.overlayTextures.Add(this.playerTexture);
            }

            base.Activate();
        }
        #endregion

        #region override Deactivate
        protected override void Deactivate()
        {
            PrecisionGame.player.overlayTextures.Remove(this.playerTexture);
            if (PrecisionGame.isCoopMode) PrecisionGame.player2.overlayTextures.Remove(this.playerTexture);
            base.Deactivate();
        }
        #endregion
        #endregion
    }
}
