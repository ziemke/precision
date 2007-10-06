using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class Powerup : Actor
    {
        #region Fields
        internal static List<Powerup> powerups;
        protected BackgroundBar pickupTimeBar;

        protected float respawnTime;
        protected float respawnTimeRemaining;
        protected float respawnTimeRemainingRandomModifier;
        protected float pickupTime;
        protected float pickupTimeRemaining;
        #endregion

        #region Properties
        internal float RespawnTime
        {
            get { return respawnTime; }
            set { respawnTime = value;
            respawnTimeRemaining = value + Game1.Range(0, respawnTimeRemainingRandomModifier);
            }
        }

        internal float PickupTime
        {
            get { return pickupTime; }
            set
            {
                pickupTime = value;
                pickupTimeRemaining = value;
            }
        }

        internal float DefaultPickupTime
        {
            get { return pickupTime; }
            set { pickupTime = value; }
        }
	    #endregion

        #region Constructors
        internal Powerup(Texture2D texture, Color pickupTimeBarColor)
            : base(texture)
        {
            powerups.Add(this);
            List<Color> pickupTimeBarColorList = new List<Color>();
            pickupTimeBarColorList.Add(pickupTimeBarColor);
            this.pickupTimeBar = new BackgroundBar(this.texture.Width, Config.PICKUP_TIMEBAR_HEIGHT, pickupTimeBarColorList);
            this.pickupTimeBar.Position = -this.Origin;
            this.pickupTimeBar.Alignment = Bar.BarAlignment.Left;
        }

        static Powerup()
        {
            powerups = new List<Powerup>();
        }
        #endregion

        #region Methods
        #region Update
        internal override void  Update(GameTime gameTime)
        {
            if (this.respawnTimeRemaining > 0f && pickupTimeRemaining <= 0f)
            {
                this.respawnTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;

                if (this.respawnTimeRemaining <= 0f)
                {
                    int c = 0;
                    do
                    {
                        this.Position = Game1.GetRandomScreenPosition(this.Radius + Config.PICKUP_TIMEBAR_HEIGHT / 2 + 2);
                        c++;
                    }
                    while (this.CheckCollisionWithAny() && c < Config.POWERUP_PLACEMENT_RETRYCOUNT);

                    
                    this.pickupTimeRemaining = this.pickupTime;
                    this.pickupTimeBar.Percent = 1f;
                    this.pickupTimeBar.Position = GetPickupTimePosition();
                }
            }

            if (this.pickupTimeRemaining > 0f)
            {
                this.pickupTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
                this.pickupTimeBar.Percent = this.pickupTimeRemaining / this.pickupTime;
                if (this.pickupTimeRemaining <= 0f)
                {
                    this.respawnTimeRemaining = this.respawnTime + Game1.Range(0, respawnTimeRemainingRandomModifier);
                    this.pickupTimeBar.Position = -this.Origin;
                    this.Position = -this.Origin;
                }
            }

 	         base.Update(gameTime);
        }

        #endregion

        #region Pickup
        internal virtual void Pickup()
        {
            this.Position = -this.Origin;
            this.pickupTimeBar.Position = -this.Origin;
            this.pickupTimeRemaining = 0f;
            
        }
        #endregion

        #region Draw Powerups
        internal static void DrawPowerups(SpriteBatch spriteBatch)
        {
            foreach (Actor powerup in powerups)
            {

                powerup.Draw(spriteBatch);
            }
        }
        #endregion

        #region Get pickupTime Position
        private Vector2 GetPickupTimePosition()
        {
            return new Vector2(this.Position.X - this.Origin.X, this.Position.Y - this.Origin.Y - Config.PICKUP_TIMEBAR_HEIGHT - 2);
        }
        #endregion

        #region Begin Level
        internal virtual void BeginLevel()
        {
            this.Position = -this.Origin;
            this.pickupTimeBar.Position = -this.Origin;
            if (this.respawnTimeRemaining <= 0f) this.respawnTimeRemaining = this.respawnTime + Game1.Range(0, respawnTimeRemainingRandomModifier);
            this.pickupTimeRemaining = 0f;
        }
        #endregion
        #endregion
    }
}
