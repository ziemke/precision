using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Precision.Classes
{
    /// <summary>
    /// Handles the Basic Input, atm: Keyboard + GamePad (1 Player only)
    /// </summary>
    class Input
    {
        #region Fields
        /// <summary>
        /// Keyboard states
        /// </summary>
        static KeyboardState keyboardState = Keyboard.GetState(), keyboardStateLastFrame;

        /// <summary>
        /// Game Pad States
        /// </summary>
        static GamePadState gamePadState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One), gamePadStateLastFrame;
        #endregion

        #region Properties
        #region Keyboard
        internal static KeyboardState KeyboardState
        {
            get { return keyboardState; }
        }

        internal static bool KeyboardEnterPressed
        {
            get { return keyboardState.IsKeyDown(Keys.Enter); }
        }

        internal static bool KeyboardEnterJustPressed
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Enter)
              && keyboardStateLastFrame.IsKeyUp(Keys.Enter);
            }
        }


        internal static bool KeyboardBackPressed
        {
            get { return keyboardState.IsKeyDown(Keys.Back); }
        }

        internal static bool KeyboardBackJustPressed
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Back)
              && keyboardStateLastFrame.IsKeyUp(Keys.Back);
            }
        }



        internal static bool KeyboardUpPressed
        {
            get { return keyboardState.IsKeyDown(Keys.Up); }
        }

        internal static bool KeyboardUpJustPressed
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Up)
              && keyboardStateLastFrame.IsKeyUp(Keys.Up);
            }
        }


        internal static bool KeyboardDownPressed
        {
            get { return keyboardState.IsKeyDown(Keys.Down); }
        }

        internal static bool KeyboardDownJustPressed
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Down)
              && keyboardStateLastFrame.IsKeyUp(Keys.Down);
            }
        }


        internal static bool KeyboardLeftPressed
        {
            get { return keyboardState.IsKeyDown(Keys.Left); }
        }

        internal static bool KeyboardLeftJustPressed
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Left)
              && keyboardStateLastFrame.IsKeyUp(Keys.Left);
            }
        }


        internal static bool KeyboardRightPressed
        {
            get { return keyboardState.IsKeyDown(Keys.Right); }
        }

        internal static bool KeyboardRightJustPressed
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Right)
              && keyboardStateLastFrame.IsKeyUp(Keys.Right);
            }
        }


        #endregion

        #region Game Pad
        internal static GamePadState GamePadState
        {
            get { return gamePadState; }
        }

        internal static bool GamePadStartPressed
        {
            get { return gamePadState.Buttons.Start == ButtonState.Pressed; }
        }

        internal static bool GamePadStartJustPressed
        {
            get
            {
                return gamePadState.Buttons.Start == ButtonState.Pressed
              && gamePadStateLastFrame.Buttons.Start == ButtonState.Released;
            }
        }


        internal static bool GamePadBackPressed
        {
            get { return gamePadState.Buttons.Back == ButtonState.Pressed; }
        }


        internal static bool GamePadBackJustPressed
        {
            get
            {
                return gamePadState.Buttons.Back == ButtonState.Pressed
              && gamePadStateLastFrame.Buttons.Back == ButtonState.Released;
            }
        }


        internal static bool GamePadXPressed
        {
            get { return gamePadState.Buttons.X == ButtonState.Pressed; }
        }

        internal static bool GamePadXJustPressed
        {
            get
            {
                return gamePadState.Buttons.X == ButtonState.Pressed
              && gamePadStateLastFrame.Buttons.X == ButtonState.Released;
            }
        }


        internal static bool GamePadAPressed
        {
            get { return gamePadState.Buttons.A == ButtonState.Pressed; }
        }

        internal static bool GamePadAJustPressed
        {
            get
            {
                return gamePadState.Buttons.A == ButtonState.Pressed
              && gamePadStateLastFrame.Buttons.A == ButtonState.Released;
            }
        }


        internal static bool GamePadBPressed
        {
            get { return gamePadState.Buttons.B == ButtonState.Pressed; }
        }

        internal static bool GamePadBJustPressed
        {
            get
            {
                return gamePadState.Buttons.B == ButtonState.Pressed
              && gamePadStateLastFrame.Buttons.B == ButtonState.Released;
            }
        }


        internal static bool GamePadYPressed
        {
            get { return gamePadState.Buttons.Y == ButtonState.Pressed; }
        }

        internal static bool GamePadYJustPressed
        {
            get
            {
                return gamePadState.Buttons.Y == ButtonState.Pressed
              && gamePadStateLastFrame.Buttons.Y == ButtonState.Released;
            }
        }


        internal static bool GamePadLeftShoulderPressed
        {
            get { return gamePadState.Buttons.LeftShoulder == ButtonState.Pressed; }
        }

        internal static bool GamePadLeftShoulderJustPressed
        {
            get
            {
                return gamePadState.Buttons.LeftShoulder == ButtonState.Pressed
              && gamePadStateLastFrame.Buttons.LeftShoulder == ButtonState.Released;
            }
        }


        internal static bool GamePadRightShoulderPressed
        {
            get { return gamePadState.Buttons.RightShoulder == ButtonState.Pressed; }
        }

        internal static bool GamePadRightShoulderJustPressed
        {
            get
            {
                return gamePadState.Buttons.RightShoulder == ButtonState.Pressed
              && gamePadStateLastFrame.Buttons.RightShoulder == ButtonState.Released;
            }
        }

        internal static bool GamePadUpPressed
        {
            get
            {
                return gamePadState.DPad.Up == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.Y > 0.75f;
            }
        }

        internal static bool GamePadUpJustPressed
        {
            get
            {
                return (gamePadState.DPad.Up == ButtonState.Pressed
                    && gamePadStateLastFrame.DPad.Up == ButtonState.Released)
                    || (gamePadState.ThumbSticks.Left.Y > 0.75f && gamePadStateLastFrame.ThumbSticks.Left.Y < 0.75f);
            }
        }


        internal static bool GamePadDownPressed
        {
            get
            {
                return gamePadState.DPad.Down == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.Y < -0.75f;
            }
        }

        internal static bool GamePadDownJustPressed
        {
            get
            {
                return (gamePadState.DPad.Down == ButtonState.Pressed
                    && gamePadStateLastFrame.DPad.Down == ButtonState.Released)
                    || (gamePadState.ThumbSticks.Left.Y < -0.75f && gamePadStateLastFrame.ThumbSticks.Left.Y > -0.75f);
            }
        }


        internal static bool GamePadLeftPressed
        {
            get
            {
                return gamePadState.DPad.Left == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.X < -0.75f;
            }
        }

        internal static bool GamePadLeftJustPressed
        {
            get
            {
                return (gamePadState.DPad.Left == ButtonState.Pressed
                    && gamePadStateLastFrame.DPad.Left == ButtonState.Released)
                    || (gamePadState.ThumbSticks.Left.X < -0.75f && gamePadStateLastFrame.ThumbSticks.Left.X > -0.75f);
            }
        }


        internal static bool GamePadRightPressed
        {
            get
            {
                return gamePadState.DPad.Right == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.X > -0.75f;
            }
        }

        internal static bool GamePadRightJustPressed
        {
            get
            {
                return (gamePadState.DPad.Right == ButtonState.Pressed
                    && gamePadStateLastFrame.DPad.Right == ButtonState.Released)
                    || (gamePadState.ThumbSticks.Left.X > 0.75f && gamePadStateLastFrame.ThumbSticks.Left.X < 0.75f);
            }
        }

        #endregion
        #endregion

        #region Methods
        #region Update
        /// <summary>
        /// Refreshes everything every frame
        /// </summary>
        internal static void Update()
        {
            keyboardStateLastFrame = keyboardState;
            keyboardState = Keyboard.GetState();

            gamePadStateLastFrame = gamePadState;
            gamePadState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
        }
        #endregion

        #region KeyboardKeyPressed
        /// <summary>
        /// Checks if the specified key is pressed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static bool KeyboardKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }
        #endregion

        #region KeyboardKeyJustPressed
        /// <summary>
        /// Checks if the specified key was just pressed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static bool KeyboardKeyJustPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && keyboardStateLastFrame.IsKeyUp(key);
        }
        #endregion
        #endregion

    }
}
