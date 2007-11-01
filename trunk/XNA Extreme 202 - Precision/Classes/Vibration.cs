using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Precision.Classes
{
    /// <summary>
    /// Handles multiple parallel Vibrations, with variable duration
    /// ATM only one Player support
    /// </summary>
    class Vibration
    {
        #region Fields
        /// <summary>
        /// Holds a list of all active vibrations
        /// </summary>
        internal static List<Vibration> vibrations;

        /// <summary>
        /// Duration of the vibration
        /// </summary>
        private TimeSpan duration;

        /// <summary>
        /// Intensities of the vibrations
        /// </summary>
        private float speedLeft, speedRight;

        /// <summary>
        /// Target Player
        /// </summary>
        private PlayerIndex playerIndex;

        #endregion

        #region Properties
        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public float SpeedLeft
        {
            get { return speedLeft; }
            set { speedLeft = value; }
        }

        public float SpeedRight
        {
            get { return speedRight; }
            set { speedRight = value; }
        }

        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
            set { playerIndex = value; }
        }
	
        #endregion

        #region Contructors
        static Vibration()
        {
            vibrations = new List<Vibration>();
        }

        public Vibration(PlayerIndex playerIndex, float speedLeft, float speedRight, TimeSpan duration)
        {
            this.playerIndex = playerIndex;
            this.speedLeft = speedLeft;
            this.speedRight = speedRight;
            this.duration = duration;

            vibrations.Add(this);
        }
        #endregion

        #region Methods
        #region Update Vibrations
        /// <summary>
        /// Updates all vibrations
        /// Should be Called in Game.Update()
        /// </summary>
        internal static void UpdateVibrations(GameTime gameTime)
        {
            // First Update them all
            for (int i = vibrations.Count - 1; i >= 0; i--)
            {
                vibrations[i].Update(gameTime);   
            }

            //Now calucalte the overall speeds
            float overallSpeedLeftPlayer1 = 0f, overallSpeedRightPlayer1 = 0f,
                overallSpeedLeftPlayer2 = 0f, overallSpeedRightPlayer2 = 0f,
                overallSpeedLeftPlayer3 = 0f, overallSpeedRightPlayer3 = 0f,
                overallSpeedLeftPlayer4 = 0f, overallSpeedRightPlayer4 = 0f;

            for (int i = vibrations.Count - 1; i >= 0; i--)
            {
                switch (vibrations[i].playerIndex)
                {
                    
                    case PlayerIndex.One:
                        overallSpeedLeftPlayer1 += vibrations[i].SpeedLeft;
                        overallSpeedRightPlayer1 += vibrations[i].SpeedRight;
                        break;
                    case PlayerIndex.Two:
                        overallSpeedLeftPlayer2 += vibrations[i].SpeedLeft;
                        overallSpeedRightPlayer2 += vibrations[i].SpeedRight;
                        break;
                    case PlayerIndex.Three:
                        overallSpeedLeftPlayer3 += vibrations[i].SpeedLeft;
                        overallSpeedRightPlayer3 += vibrations[i].SpeedRight;
                        break;
                    case PlayerIndex.Four:
                        overallSpeedLeftPlayer4 += vibrations[i].SpeedLeft;
                        overallSpeedRightPlayer4 += vibrations[i].SpeedRight;
                        break;
                    default:
                        break;
                }
               
            }

            //Clamp them all
            overallSpeedLeftPlayer1 = MathHelper.Clamp(overallSpeedLeftPlayer1, 0f, 1f);
            overallSpeedRightPlayer1 = MathHelper.Clamp(overallSpeedRightPlayer1, 0f, 1f);

            overallSpeedLeftPlayer2 = MathHelper.Clamp(overallSpeedLeftPlayer2, 0f, 1f);
            overallSpeedRightPlayer2 = MathHelper.Clamp(overallSpeedRightPlayer2, 0f, 1f);

            overallSpeedLeftPlayer3 = MathHelper.Clamp(overallSpeedLeftPlayer3, 0f, 1f);
            overallSpeedRightPlayer3 = MathHelper.Clamp(overallSpeedRightPlayer3, 0f, 1f);

            overallSpeedLeftPlayer4 = MathHelper.Clamp(overallSpeedLeftPlayer4, 0f, 1f);
            overallSpeedRightPlayer4 = MathHelper.Clamp(overallSpeedRightPlayer4, 0f, 1f);


            GamePad.SetVibration(PlayerIndex.One, overallSpeedLeftPlayer1, overallSpeedRightPlayer1);
            GamePad.SetVibration(PlayerIndex.Two, overallSpeedLeftPlayer2, overallSpeedRightPlayer2);
            GamePad.SetVibration(PlayerIndex.Three, overallSpeedLeftPlayer3, overallSpeedRightPlayer3);
            GamePad.SetVibration(PlayerIndex.Four, overallSpeedLeftPlayer4, overallSpeedRightPlayer4);

        }
        #endregion

        #region Update
        /// <summary>
        /// Updates a specified vibration
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime)
        {

            //Handle duration stuff
            if (this.duration > TimeSpan.Zero)
            {
                this.duration -= gameTime.ElapsedGameTime;
                
                //Vibration timed out? delete it!
                if (this.duration <= TimeSpan.Zero)
                    vibrations.Remove(this);

            }
        }
        #endregion

        #region SetVibration
        /// <summary>
        /// Creates a new Vibration
        /// </summary>
        /// <param name="speedLeft"></param>
        /// <param name="speedRight"></param>
        /// <param name="duration"></param>
        internal static void SetVibration(PlayerIndex playerIndex, float speedLeft, float speedRight, TimeSpan duration)
        {
            new Vibration(playerIndex, speedLeft, speedRight, duration);
        }
        #endregion
        #endregion
    }
}
