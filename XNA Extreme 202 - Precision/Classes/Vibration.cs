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
        #endregion

        #region Contructors
        static Vibration()
        {
            vibrations = new List<Vibration>();
        }

        public Vibration(float speedLeft, float speedRight, TimeSpan duration)
        {
            vibrations.Add(this);

            this.speedLeft = speedLeft;
            this.speedRight = speedRight;
            this.duration = duration;
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
            float overallSpeedLeft = 0f, overallSpeedRight = 0f;

            for (int i = vibrations.Count - 1; i >= 0; i--)
            {
                overallSpeedLeft += vibrations[i].SpeedLeft;
                overallSpeedRight += vibrations[i].SpeedRight;
            }

            overallSpeedLeft = MathHelper.Clamp(overallSpeedLeft, 0f, 1f);
            overallSpeedRight = MathHelper.Clamp(overallSpeedRight, 0f, 1f);

            GamePad.SetVibration(PlayerIndex.One, overallSpeedLeft, overallSpeedRight);
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
        internal static void SetVibration(float speedLeft, float speedRight, TimeSpan duration)
        {
            new Vibration(speedLeft, speedRight, duration);
        }
        #endregion
        #endregion
    }
}
