#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Precision.Classes;
using Mahdi.Khodadadi.PostProcessing;
using System.IO;
using System.Threading;
#endregion

namespace Precision
{
    enum GameState { Title, Start, Options, HighScore, Help, LevelChange, Playing, Died, GameOver, Pause};
    enum ButtonIcons { BumperLeft, BumperRight, A, B, Back, Start, X, Y, DPadDown, DPadLeft, DPadRight, DPadUp, DPadLeftRight, StickLeft, StickRight, TriggerLeft, TriggerRight};

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    internal class PrecisionGame : Microsoft.Xna.Framework.Game
    {
        #region Fields
        GraphicsDeviceManager graphics;
        ContentManager content;
        SpriteBatch spriteBatch;
        internal static SpriteFont spriteFontXirod1;
        internal static SpriteFont spriteFontXirod2;
        internal static SpriteFont spriteFontArial1;
        Color overlayColorDeath = new Color(200, 0, 0, 128);
        Color overlayColorPause = new Color(255, 255, 255, 128);
        static Random random;
        GameState gameState;
        PostProcess ppe;


        internal const int SCREEN_WIDTH = 800;
        internal const int SCREEN_HEIGHT = 600;

        const float PLAYER_SPEED_SLOW = 5f;
        const float PLAYER_SPEED_FAST = 10f;
        const float PLAYER_INVINCIBILITY_TIME = 1f;

        const float ENEMY_BASE_SPEED = 3f;
        const float ENEMY_SPEED_VARIATION = 1f;

       
        private static Color cellHealthyColor = new Color(124, 207, 86);
        private static Color cellDeadColor = new Color(115, 95, 141);
        private static Color cellBarHealthyColor = new Color(0, 255, 0);
        private static Color cellBarDeadColor = Color.Red;
        double timeUntilAttack;
        const double cellAttackInterval = 1d;
        
        internal static int level;
        const int BASE_NUM_CELLS = 2;
        const int BASE_NUM_ENEMIES = 1;

        static int lives;
        const int STARTING_LIVES = 3;

        private int score;
        const int INITIAL_SCORE_VALUE = 10;
        internal const int CELL_DEATH_PENALTY = 2;
        internal static int currentScoreValue;
        internal static int scoreMultiplicator = 1;

        
        const int EXTRA_LIVE_POWERUP_PROBABILITY = 8;

        static float titleScreenFlick = 0;

        internal static bool showSlowMotionEffect = false;

        internal static Player player;
        Texture2D titleScreenTexture;
        internal static List<String> graphicsSetsFolderNames;
        internal static Dictionary<string, Dictionary<string, Texture2D>> graphicsSetsContents;

        internal static string selectedGraphicsSet;

        bool remUpPressed = false, remDownPressed = false, remStartPressed = false, remAPressed = false, remXPressed = false, remBPressed = false, remYPressed = false, remBackOrEscPressed = false, remLeftPressed = false, remRightPressed = false;
        bool gamePadUp = false, gamePadDown = false, gamePadLeft = false, gamePadRight = false;
        int currentMenuItem = 0;

        internal static int difficulty = 1;
        List<string> difficulties;


        int currentDifficulty;
        int currentGraphicsSet;
        int currentHighScoreDifficulty;
        info.aspspider.HighScore[] highScores;

        Thread getHighScoresThread;

        Texture2D textureButtonBumperLeft, textureButtonBumperRight, textureButtonA, textureButtonB, textureButtonBack, textureButtonStart, textureButtonX, textureButtonY, textureButtonDPadDown, textureButtonDPadLeft, textureButtonDPadRight, textureButtonDPadUp, textureButtonDPadLeftRight, textureButtonStickLeft, textureButtonStickRight, textureButtonTriggerLeft, textureButtonTriggerRight;
        #endregion

        #region Properties
        private int levelCellCount
        {
            get { return BASE_NUM_CELLS * level; }
        }

        private int levelEnemyCount
        {
            get { return BASE_NUM_ENEMIES * level * difficulty; }
        }
        #endregion

        internal PrecisionGame()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;

            //graphics.IsFullScreen = true;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            random = new Random();
            gameState = GameState.Title;

            currentDifficulty = 0;
            currentGraphicsSet = 0;
            currentHighScoreDifficulty = 0;

            graphicsSetsFolderNames = new List<string>();
            graphicsSetsContents = new Dictionary<string, Dictionary<string, Texture2D>>();


            String[] directories = Directory.GetDirectories(@"Content\graphicsSets");
            for (int i = 0; i < directories.Length; i++)
                graphicsSetsFolderNames.Add(directories[i]);

            selectedGraphicsSet = @"Content\graphicsSets\3DBuzz";

            difficulties = new List<string>();
            difficulties.Add("Easy");
            difficulties.Add("Normal");
            difficulties.Add("Hard");
            difficulties.Add("Insane");

            highScores = new info.aspspider.HighScore[10];
            base.Initialize();
        }


        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                spriteFontXirod1 = content.Load<SpriteFont>("Content/Xirod1");
                spriteFontXirod2 = content.Load<SpriteFont>("Content/Xirod2");
                spriteFontArial1 = content.Load<SpriteFont>("Content/Arial1");

                textureButtonBumperLeft = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/bumper_left");
                textureButtonBumperRight = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/bumper_right");
                textureButtonA = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/button_a");
                textureButtonB = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/button_b");
                textureButtonBack = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/bumper_left");
                textureButtonStart = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/button_start");
                textureButtonX = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/button_x");
                textureButtonY = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/button_y");
                textureButtonDPadDown = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/bumper_left");
                textureButtonDPadLeft = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/bumper_left");
                textureButtonDPadRight = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/bumper_left");
                textureButtonDPadUp = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/bumper_left");
                textureButtonDPadLeftRight = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/dpad_leftright");
                textureButtonStickLeft = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/bumper_left");
                textureButtonStickRight = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/bumper_left");
                textureButtonTriggerLeft = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/bumper_left");
                textureButtonTriggerRight = content.Load<Texture2D>("Content/XBOX360 Controller Buttons/bumper_left");


                foreach (String directory in graphicsSetsFolderNames)
                {
                    Dictionary<string, Texture2D> graphicsSet = new Dictionary<string, Texture2D>();
                    graphicsSet.Add("background", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/Background.png"));
                    graphicsSet.Add("titleScreenNormal", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/TitleScreen.png"));
                    graphicsSet.Add("titleScreenRollover", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/TitleScreenRollover.png"));
                    graphicsSet.Add("levelChange", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/LevelChangeScreen.png"));
                    graphicsSet.Add("overlay", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/Fill22.png"));
                    graphicsSet.Add("player", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/Player.png"));
                    graphicsSet.Add("cell", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/Cell.png"));
                    graphicsSet.Add("enemy", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/Enemy.png"));
                    graphicsSet.Add("time", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/TimePowerup.png"));
                    graphicsSet.Add("shield", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/ShieldPowerup.png"));
                    graphicsSet.Add("extraLife", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/ExtraLifePowerup.png"));
                    graphicsSet.Add("score", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/ScorePowerUp.png"));
                    graphicsSet.Add("destruction", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/DestructionPowerup.png"));
                    graphicsSet.Add("destructionPlayer", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/DestructionPowerupPlayer.png"));
                    graphicsSet.Add("speedPlayer", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/SpeedPowerdownPlayer.png"));
                    graphicsSet.Add("HUD", Texture2D.FromFile(graphics.GraphicsDevice, directory + "/HUD.png"));

                    graphicsSetsContents.Add(directory, graphicsSet);
                   
                }


                titleScreenTexture = graphicsSetsContents[selectedGraphicsSet]["titleScreenNormal"];
                ppe = new PostProcess(graphics.GraphicsDevice);

            }

        }


        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
            {
                content.Unload();
            }
        }

       
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            // Allows the game to exit
            if (gamePadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            gamePadUp = gamePadState.DPad.Up == ButtonState.Pressed || gamePadState.ThumbSticks.Left.Y > 0.5f;
            gamePadDown = gamePadState.DPad.Down == ButtonState.Pressed || gamePadState.ThumbSticks.Left.Y < -0.5f;
            gamePadLeft = gamePadState.DPad.Left == ButtonState.Pressed || gamePadState.ThumbSticks.Left.X < -0.5f;
            gamePadRight = gamePadState.DPad.Right == ButtonState.Pressed || gamePadState.ThumbSticks.Left.X > 0.5f;

            switch (gameState)
            {
                case GameState.Title:
                    titleScreenFlick += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (titleScreenFlick > Config.TITLE_SCREEN_FLICK_DURATION)
                    {
                        titleScreenFlick = 0f;
                        if (titleScreenTexture == graphicsSetsContents[selectedGraphicsSet]["titleScreenNormal"])
                            titleScreenTexture = graphicsSetsContents[selectedGraphicsSet]["titleScreenRollover"];
                        else
                            titleScreenTexture = graphicsSetsContents[selectedGraphicsSet]["titleScreenNormal"];
                    }

                    if (gamePadState.Buttons.Start == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Enter))
                    {
                        this.gameState = GameState.Start;
                    }

                    break;

                case GameState.Start:

                    if ((gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A)) && currentMenuItem == 0 && !remAPressed)
                    {
                        this.BeginGame();
                        this.gameState = GameState.LevelChange;
                    }

                    if ((gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A)) && currentMenuItem == 1 && !remAPressed)
                    {
                        this.currentMenuItem = 0;
                        this.currentDifficulty = difficulty;
                        this.currentGraphicsSet = graphicsSetsFolderNames.IndexOf(selectedGraphicsSet);
                        this.gameState = GameState.Options;
                    }


                    if ((gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A)) && currentMenuItem == 2 && !remAPressed)
                    {
                        this.currentMenuItem = 0;
                        this.GetHighScores(currentHighScoreDifficulty);
                        this.gameState = GameState.HighScore;
                    }

                    if ((gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A)) && currentMenuItem == 4 && !remAPressed)
                        this.Exit();

                    if((gamePadUp || keyboardState.IsKeyDown(Keys.Up)) && !remUpPressed)
                        this.currentMenuItem = (currentMenuItem + 4) % 5;
                    else if ((gamePadDown || keyboardState.IsKeyDown(Keys.Down)) && !remDownPressed)
                        this.currentMenuItem = (currentMenuItem + 1) % 5;
                    
                    break;

                case GameState.Options:
                    if ((gamePadUp || keyboardState.IsKeyDown(Keys.Up)) && !remUpPressed)
                        this.currentMenuItem = (currentMenuItem + 2) % 3;
                    else if ((gamePadDown || keyboardState.IsKeyDown(Keys.Down)) && !remDownPressed)
                        this.currentMenuItem = (currentMenuItem + 1) % 3;

                    if ((gamePadLeft || keyboardState.IsKeyDown(Keys.Left)) && !remLeftPressed)
                    {
                        if (currentMenuItem == 0) this.currentDifficulty = (currentDifficulty + this.difficulties.Count - 1) % this.difficulties.Count;
                        else if (currentMenuItem == 1) this.currentGraphicsSet = (currentGraphicsSet + graphicsSetsFolderNames.Count - 1) % graphicsSetsFolderNames.Count;
                    }
                    else if ((gamePadRight || keyboardState.IsKeyDown(Keys.Right)) && !remRightPressed)
                    {
                        if (currentMenuItem == 0)
                            this.currentDifficulty = (currentDifficulty + 1) % this.difficulties.Count;
                        else if (currentMenuItem == 1) this.currentGraphicsSet = (currentGraphicsSet + 1) % graphicsSetsFolderNames.Count;
                    }

                    if ((gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A)) && currentMenuItem == 2 && !remAPressed)
                    {
                        this.gameState = GameState.Start;
                        currentMenuItem = 1;
                    }

                    difficulty = currentDifficulty;
                    selectedGraphicsSet = graphicsSetsFolderNames[currentGraphicsSet];
                    break;

                case GameState.HighScore:
                    if ((gamePadLeft || keyboardState.IsKeyDown(Keys.Left)) && !remLeftPressed)
                    {
                        this.currentHighScoreDifficulty = (currentHighScoreDifficulty + this.difficulties.Count - 1) % this.difficulties.Count;
                        this.GetHighScores(currentHighScoreDifficulty);
                    }
                    else if ((gamePadRight || keyboardState.IsKeyDown(Keys.Right)) && !remRightPressed)
                    {
                        this.currentHighScoreDifficulty = (currentHighScoreDifficulty + 1) % this.difficulties.Count;
                        this.GetHighScores(currentHighScoreDifficulty);
                    }


                    if ((gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A)) && !remAPressed)
                    {
                        this.currentMenuItem = 2;
                        this.gameState = GameState.Start;
                    }

                    if ((gamePadState.Buttons.X == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.X)) && !remXPressed)
                        this.GetHighScores(currentHighScoreDifficulty);

                    break;

                case GameState.Help:
                    break;

                case GameState.LevelChange:
                    if ((gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A)) && !remAPressed)
                    {
                        this.BeginLevel();
                        this.gameState = GameState.Playing;
                    }
                    break;

                case GameState.Playing:                     

                    timeUntilAttack -= gameTime.ElapsedGameTime.TotalSeconds;                 

                    if (Cell.cellCount <= 0)
                    {
                        level++;
                        this.gameState = GameState.LevelChange;
                        this.EndLevel();
                        break;
                    }

                    if ((gamePadState.Buttons.Start == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Enter)) && !remStartPressed)
                        this.gameState = GameState.Pause;

                    for (int i = Actor.actors.Count - 1; i >= 0; i--)
                    {
                        Actor actor = Actor.actors[i];
                        actor.Update(gameTime);

                        if (actor is Player)
                        {
                            Vector2 direction = new Vector2(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y);

                            if (direction.Length() > 0f)
                                direction.Normalize();
                            else
                                direction = Vector2.Zero;

                            if (gamePadState.Buttons.B == ButtonState.Pressed)
                                actor.Position += direction * PLAYER_SPEED_FAST * player.SlowDown;
                            else
                                actor.Position += direction * PLAYER_SPEED_SLOW * player.SlowDown;

                            Vector2 directionKeyboard = Vector2.Zero;

                            if (keyboardState.IsKeyDown(Keys.Right))
                                directionKeyboard.X = 1f;
                            else if (keyboardState.IsKeyDown(Keys.Left))
                                directionKeyboard.X = -1f;
                            if (keyboardState.IsKeyDown(Keys.Up))
                                directionKeyboard.Y = -1f;
                            else if (keyboardState.IsKeyDown(Keys.Down))
                                directionKeyboard.Y = 1f;

                            if (directionKeyboard.Length() > 0f)
                                directionKeyboard.Normalize();
                            else
                                directionKeyboard = Vector2.Zero;

                            if (keyboardState.IsKeyDown(Keys.Y))
                                actor.Position += directionKeyboard * PLAYER_SPEED_FAST * player.SlowDown;
                            else
                                actor.Position += directionKeyboard * PLAYER_SPEED_SLOW * player.SlowDown;

                            int HudHeight = graphicsSetsContents[selectedGraphicsSet]["HUD"].Height;

                            Vector2 playerPos = actor.Position;
                            if (playerPos.X < actor.Origin.X)
                                playerPos.X = actor.Origin.X;
                            else if (playerPos.X > SCREEN_WIDTH - actor.Origin.X)
                                playerPos.X = SCREEN_WIDTH - actor.Origin.X;
                            if (playerPos.Y < actor.Origin.Y)
                                playerPos.Y = actor.Origin.Y;
                            else if (playerPos.Y > SCREEN_HEIGHT - actor.Origin.Y - HudHeight + 7)
                                playerPos.Y = SCREEN_HEIGHT - actor.Origin.Y - HudHeight + 7;

                            actor.Position = playerPos;

                            continue;
                        }

                        Enemy enemy = actor as Enemy;
                        if (enemy != null)
                        {
                            if (Actor.CheckCollision(player, enemy) && enemy.IsHarmful && !player.IsInvincible)
                            {
                                lives--;
                                if (lives > 0)
                                {
                                    this.gameState = GameState.Died;
                                    showSlowMotionEffect = false;
                                }
                                else
                                {
                                    this.gameState = GameState.GameOver;
                                    showSlowMotionEffect = false;
                                }

                                player.Reset(PLAYER_INVINCIBILITY_TIME);

                            }
                            continue;
                        }

                        Cell cell = actor as Cell;
                        if (cell != null)
                        {

                            if (timeUntilAttack <= 0 && cell.State == Cell.CellState.Healthy)
                            {
                                cell.SetAttacked(Config.CELL_DEATH_TIME);
                                timeUntilAttack = cellAttackInterval;
                            }

                            if (cell.State == Cell.CellState.Attacked && Actor.CheckCollision(cell, player))
                            {
                                cell.Saved();
                                this.score += currentScoreValue * scoreMultiplicator;
                            }

                            continue;
                        }

                        Powerup powerup = actor as Powerup;
                        if (powerup != null)
                        {

                            if (Actor.CheckCollision(powerup, player))
                                powerup.Pickup();

                        }
                    }

                    for (int i = Powerup.powerups.Count - 1; i >= 0; i--)
                    {
                        Powerup powerup = Powerup.powerups[i];
                        powerup.Update(gameTime);

                        if (Actor.CheckCollision((Actor)powerup, (Actor)player))
                            powerup.Pickup();
                    }
                    break;

                case GameState.Died:

                    if ((gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A)) && !remAPressed)
                        gameState = GameState.Playing;

                    break;

                case GameState.GameOver:
                    if ((gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A)) && !remAPressed)
                        gameState = GameState.Title;
                    break;

                case GameState.Pause:
                    if ((gamePadState.Buttons.Start == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Enter)) && !remStartPressed)
                        gameState = GameState.Playing;
                    break;
                default:
                    break;
            }

            #region Remember if important buttons were pressed
            remUpPressed = gamePadState.ThumbSticks.Left.Y > 0.5f || keyboardState.IsKeyDown(Keys.Up) || gamePadState.DPad.Up == ButtonState.Pressed;
            remDownPressed = gamePadState.ThumbSticks.Left.Y < -0.5f || keyboardState.IsKeyDown(Keys.Down) || gamePadState.DPad.Down == ButtonState.Pressed;
            remLeftPressed = gamePadState.ThumbSticks.Left.X < -0.5f || keyboardState.IsKeyDown(Keys.Left) || gamePadState.DPad.Left == ButtonState.Pressed;
            remRightPressed = gamePadState.ThumbSticks.Left.X > 0.5f || keyboardState.IsKeyDown(Keys.Right) || gamePadState.DPad.Right == ButtonState.Pressed;

            remStartPressed = gamePadState.Buttons.Start == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Enter);
            remBackOrEscPressed = gamePadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape);

            remAPressed = gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A);
            remBPressed = gamePadState.Buttons.B == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.B);
            remXPressed = gamePadState.Buttons.X == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.X);
            remYPressed = gamePadState.Buttons.Y == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Y);
            #endregion

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Orange);


            spriteBatch.Begin();
            switch (gameState)
            {
                case GameState.Title:
                    DrawTitleScreen();
                    break;

                case GameState.Start:
                    spriteBatch.Draw(graphicsSetsContents[selectedGraphicsSet]["levelChange"], new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);

                    spriteBatch.DrawString(spriteFontXirod1, "Start", new Vector2(SCREEN_WIDTH / 2 - spriteFontXirod1.MeasureString("Start").X / 2, 200), (currentMenuItem == 0) ? Color.Orange : Color.White);
                    spriteBatch.DrawString(spriteFontXirod1, "Options", new Vector2(SCREEN_WIDTH / 2 - spriteFontXirod1.MeasureString("Options").X / 2, 250), (currentMenuItem == 1) ? Color.Orange : Color.White);
                    spriteBatch.DrawString(spriteFontXirod1, "High Score", new Vector2(SCREEN_WIDTH / 2 - spriteFontXirod1.MeasureString("High Score").X / 2, 300), (currentMenuItem == 2) ? Color.Orange : Color.White);
                    spriteBatch.DrawString(spriteFontXirod1, "Help", new Vector2(SCREEN_WIDTH / 2 - spriteFontXirod1.MeasureString("Help").X / 2, 350), (currentMenuItem == 3) ? Color.Orange : Color.White);
                    spriteBatch.DrawString(spriteFontXirod1, "Exit", new Vector2(SCREEN_WIDTH / 2 - spriteFontXirod1.MeasureString("Exit").X / 2, 400), (currentMenuItem == 4) ? Color.Orange : Color.White);
                    break;

                case GameState.Options:
                    spriteBatch.Draw(graphicsSetsContents[selectedGraphicsSet]["levelChange"], new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);

                    spriteBatch.DrawString(spriteFontXirod1, "Difficulty:", new Vector2(SCREEN_WIDTH / 2 - spriteFontXirod1.MeasureString("Difficulty:").X/* / 2*/, 200), (currentMenuItem == 0) ? Color.Orange : Color.White);
                    spriteBatch.DrawString(spriteFontXirod1, " " + this.difficulties[this.currentDifficulty], new Vector2(SCREEN_WIDTH / 2 /*+ spriteFontXirod1.MeasureString("Difficulty: ").X / 2*/, 200), (currentMenuItem == 0) ? Color.Orange : Color.White);

                    spriteBatch.DrawString(spriteFontXirod1, "Graphics Set:", new Vector2(SCREEN_WIDTH / 2 - spriteFontXirod1.MeasureString("Graphics Set:").X /*/ 2*/, 250), (currentMenuItem == 1) ? Color.Orange : Color.White);
                    spriteBatch.DrawString(spriteFontXirod1, " " + graphicsSetsFolderNames[currentGraphicsSet].Replace(@"Content\graphicsSets\", ""), new Vector2(SCREEN_WIDTH / 2 /*+ spriteFontXirod1.MeasureString("Graphics Set: ").X / 2*/, 250), (currentMenuItem == 1) ? Color.Orange : Color.White);

                    spriteBatch.DrawString(spriteFontXirod1, "Back", new Vector2(SCREEN_WIDTH / 2 - spriteFontXirod1.MeasureString("Back").X / 2, 300), (currentMenuItem == 2) ? Color.Orange : Color.White);

                     break;

                case GameState.HighScore:
                    spriteBatch.Draw(graphicsSetsContents[selectedGraphicsSet]["levelChange"], new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);

                    spriteBatch.DrawString(spriteFontXirod1, "High Scores", new Vector2(SCREEN_WIDTH / 2 - 200, 130), Color.White);
                    spriteBatch.DrawString(spriteFontArial1, "Difficulty: " + difficulties[currentHighScoreDifficulty], new Vector2(SCREEN_WIDTH / 2 - 200, 150), Color.White);

                    spriteBatch.DrawString(spriteFontArial1, "#", new Vector2(SCREEN_WIDTH / 2 - 200, 200), Color.White);
                    spriteBatch.DrawString(spriteFontArial1, "Name", new Vector2(SCREEN_WIDTH / 2 - 200 + spriteFontArial1.MeasureString("#   ").X, 200), Color.White);
                    spriteBatch.DrawString(spriteFontArial1, "Level", new Vector2(SCREEN_WIDTH / 2 - 200 + spriteFontArial1.MeasureString("#   Name  ").X, 200), Color.White);
                    spriteBatch.DrawString(spriteFontArial1, "Score", new Vector2(SCREEN_WIDTH / 2 - 200 + spriteFontArial1.MeasureString("#   Name  Level  ").X, 200), Color.White);
                    spriteBatch.DrawString(spriteFontArial1, "Date", new Vector2(SCREEN_WIDTH / 2 - 200 + spriteFontArial1.MeasureString("#   Name  Level  Score   ").X, 200), Color.White);


                    for (int i = 0; i < highScores.Length; i++)
                    {
                        spriteBatch.DrawString(spriteFontArial1, (i + 1) + "", new Vector2(SCREEN_WIDTH / 2 - 200, 220 + i * 20), Color.White);
                        spriteBatch.DrawString(spriteFontArial1, highScores[i].Name, new Vector2(SCREEN_WIDTH / 2 - 200 + spriteFontArial1.MeasureString("#   ").X, 220 + i * 20), Color.White);
                        spriteBatch.DrawString(spriteFontArial1, highScores[i].Level.ToString(), new Vector2(SCREEN_WIDTH / 2 - 200 + spriteFontArial1.MeasureString("#   Name  ").X, 220 + i * 20), Color.White);
                        spriteBatch.DrawString(spriteFontArial1, highScores[i].Score.ToString(), new Vector2(SCREEN_WIDTH / 2 - 200 + spriteFontArial1.MeasureString("#   Name  Level  ").X, 220 + i * 20), Color.White);
                        spriteBatch.DrawString(spriteFontArial1, (highScores[i].DateTime.Ticks <= 1 ? "-" : highScores[i].DateTime.ToString("MM.dd.yyyy")), new Vector2(SCREEN_WIDTH / 2 - 200 + spriteFontArial1.MeasureString("#   Name  Level  Score   ").X, 220 + i * 20), Color.White);
                    }

                    //spriteBatch.DrawString(spriteFontXirod2, "Press A to continue", new Vector2(430, 480), Color.White);
                    this.DrawPressButtonText(ButtonIcons.A, spriteBatch, spriteFontXirod2, "Back", new Vector2(430, 480), Color.White);
                    //spriteBatch.DrawString(spriteFontXirod2, "Press X to refresh", new Vector2(430, 500), Color.White);
                    this.DrawPressButtonText(ButtonIcons.X, spriteBatch, spriteFontXirod2, "Refresh", new Vector2(430, 505), Color.White);
                    //spriteBatch.DrawString(spriteFontXirod2, "Press L/R to switch diffi", new Vector2(430, 520), Color.White);
                    this.DrawPressButtonText(ButtonIcons.DPadLeftRight, spriteBatch, spriteFontXirod2, "Switch Difficulty", new Vector2(430, 528), Color.White);
                    break;

                case GameState.Help:
                    break;

                case GameState.LevelChange:
                    DrawLevelChangeScreen();
                    break;

                case GameState.Playing:
                case GameState.Died:
                case GameState.GameOver:
                case GameState.Pause:
                    spriteBatch.Draw(graphicsSetsContents[selectedGraphicsSet]["background"], new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);

                    Bar.texture = graphicsSetsContents[selectedGraphicsSet]["overlay"];

                    Bar.DrawBackgroundBars(spriteBatch);
                    Actor.DrawActors(spriteBatch);
                    Bar.DrawBars(spriteBatch);
                    Powerup.DrawPowerups(spriteBatch);

                    this.DrawHud(spriteBatch);
                    

                    if (this.gameState == GameState.Died)
                        DrawDeathScreen();
                    else if (this.gameState == GameState.GameOver)
                        DrawGameOverScreen();
                    else if (this.gameState == GameState.Pause)
                        DrawPauseScreen();
                    break;

                default:
                    break;
            }
            spriteBatch.End();

            if (showSlowMotionEffect)
            {
                ppe.ResolveBackBuffer();

                //ppe.ApplyDownSample();
                ppe.ApplyDownSample();

                //ppe.ApplyUpSample();
                ppe.ApplyUpSample();
                

                //ppe.ApplyRadialBlur();

                ppe.Present(null);
            }

            base.Draw(gameTime);
        }

      

        #region Gameplay methods
        #region Begin Game
        private void BeginGame()
        {
            level = 1;
            lives = STARTING_LIVES;
            this.score = 0;

            Powerup.powerups.Clear();
            Bar.bars.Clear();

            showSlowMotionEffect = false;
        }
        #endregion

        #region Begin Level
        private void BeginLevel()
        {
            Actor.TimeScale = 1f;
            scoreMultiplicator = 1;
            Actor.actors.Clear();
            Powerup.powerups.Clear();
            Bar.bars.Clear();

            Cell.cellCount = 0;

            
            Cell.AddCells(levelCellCount, graphicsSetsContents[selectedGraphicsSet]["cell"], cellHealthyColor, cellDeadColor, cellBarHealthyColor, cellBarDeadColor);
            Enemy.AddEnemies(levelEnemyCount, graphicsSetsContents[selectedGraphicsSet]["enemy"], ENEMY_BASE_SPEED, ENEMY_SPEED_VARIATION);

            player = new Player(graphicsSetsContents[selectedGraphicsSet]["player"]);
            player.Reset(PLAYER_INVINCIBILITY_TIME);

            this.timeUntilAttack = cellAttackInterval;

            currentScoreValue = INITIAL_SCORE_VALUE * level;

            showSlowMotionEffect = false;

            List<Color> timePowerupPickupTimeBarColors = new List<Color>();
            timePowerupPickupTimeBarColors.Add(Config.timePowerupPickupTimeBarColor);
            timePowerupPickupTimeBarColors.Add(Color.Gray);
            new TimePowerup(graphicsSetsContents[selectedGraphicsSet]["time"], Config.timePowerupActiveTimeBarColor, timePowerupPickupTimeBarColors);

            List<Color> scorePowerupPickupTimeBarColors = new List<Color>();
            scorePowerupPickupTimeBarColors.Add(Config.scorePowerupPickupTimeBarColor);
            new ScorePowerup(graphicsSetsContents[selectedGraphicsSet]["score"], Config.scorePowerupActiveTimeBarColor, scorePowerupPickupTimeBarColors);

            List<Color> extralifePowerupPickupTimeBarColors = new List<Color>();
            extralifePowerupPickupTimeBarColors.Add(Config.extralifePowerupPickupTimeBarColor);
            new ExtraLifePowerUp(graphicsSetsContents[selectedGraphicsSet]["extraLife"], extralifePowerupPickupTimeBarColors);

            List<Color> shieldPowerupPickupTimeBarColors = new List<Color>();
            shieldPowerupPickupTimeBarColors.Add(Config.shieldPowerupPickupTimeBarColor);
            new ShieldPowerup(graphicsSetsContents[selectedGraphicsSet]["shield"], Config.shieldPowerupActiveTimeBarColor, shieldPowerupPickupTimeBarColors);

            List<Color> destructionPowerupPickupTimeBarColors = new List<Color>();
            destructionPowerupPickupTimeBarColors.Add(Config.destructionPowerupPickupTimeBarColor);
            new DestructionPowerup(graphicsSetsContents[selectedGraphicsSet]["destruction"], graphicsSetsContents[selectedGraphicsSet]["destructionPlayer"], Config.destructionPowerupActiveTimeBarColor, destructionPowerupPickupTimeBarColors);

            new SpeedPowerdown(graphicsSetsContents[selectedGraphicsSet]["cell"], graphicsSetsContents[selectedGraphicsSet]["speedPlayer"], Config.speedPowerdownActiveTimeBarColor, Config.speedPowerdownPickupTimeBarColors);
        }
        #endregion

        #region Ende Level
        private void EndLevel()
        {
            showSlowMotionEffect = false;

            for (int i = 0; i < Cell.actors.Count; i++)
            {
                Cell cell = Cell.actors[i] as Cell;
                if (cell != null) cell.EndLevel();
            }
        }
        #endregion
        #endregion

        #region Utitity
        #region Get random screen position
        internal static Vector2 GetRandomScreenPosition(int padding)
        {
            return new Vector2(random.Next(padding, SCREEN_WIDTH - padding), random.Next(padding, SCREEN_HEIGHT - padding - graphicsSetsContents[selectedGraphicsSet]["HUD"].Height));
        }
        #endregion

        #region Range
        internal static float Range(float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }
        #endregion

        #region Add enemy at
        internal static void AddEnemyAdd(Vector2 position)
        {
            Enemy enemy = new Enemy(graphicsSetsContents[selectedGraphicsSet]["enemy"], ENEMY_BASE_SPEED, ENEMY_SPEED_VARIATION);
            enemy.Position = position;
        }

        #endregion

        #region Add extra live
        internal static void AddExtraLife()
        {
            lives++;
        }

        #endregion

        #region Scale To Fit
        internal static float ScaleToFit(Vector2 source, Vector2 target)
        {
            float scale = 1f;
            if (source.X < target.X || source.Y < target.Y)
            {
                while (source.X * scale < target.X && source.Y * scale < target.Y)
                    scale += 0.01f;
            }
            else
            {
                while ((source.X * scale > target.X) && (source.Y * scale > target.Y))
                    scale -= 0.01f;
            }
            return scale;
        }
        #endregion

        #region Get Life Bar Color
        internal static Color GetLifeBarColor(List<Color> colors, float healthPercent)
        {
            if (colors.Count == 1)
                return colors[0];

            if (colors.Count >= 2)
            {
                byte startColor = (byte)((colors.Count - 1) * healthPercent);
                byte endColor = (byte)(startColor + 1);

                if (endColor > colors.Count - 1)
                    endColor = (byte)(colors.Count - 1);

                byte r = (byte)(MathHelper.Lerp(colors[startColor].R, colors[endColor].R, healthPercent));
                byte g = (byte)(MathHelper.Lerp(colors[startColor].G, colors[endColor].G, healthPercent));
                byte b = (byte)(MathHelper.Lerp(colors[startColor].B, colors[endColor].B, healthPercent));

                return new Color(r, g, b);
            }

            return Config.lifeBarColorDefault;
        }
        #endregion

        #region GetHighScores
        private void GetHighScores(int difficulty)
        {
            for (int i = 0; i < highScores.Length; i++)
            {
                highScores[i] = new Precision.info.aspspider.HighScore();
                highScores[i].Name = "-";
                highScores[i].Score = 0;
                highScores[i].Level = 0;
                highScores[i].DateTime = new DateTime(1);
            }

            if (getHighScoresThread != null) getHighScoresThread.Abort();

            getHighScoresThread = new Thread(new ThreadStart(
                delegate
                {
                    try
                    {
                        highScores = new info.aspspider.Service1().GetHighScores(difficulty);
                    }
                    catch
                    {

                    }
                }
                ));

            getHighScoresThread.Start();

        }
        #endregion

        #region Scale Enemies down
        internal static void ScaleEnemiesDown(float scale)
        {
            foreach (Actor actor in Actor.actors)
            {
                Enemy enemy = actor as Enemy;
                if (enemy != null)
                    enemy.StartScale(scale, 3f);
            }
        }
        #endregion

        #region DrawPressButtonText
        public void DrawPressButtonText(ButtonIcons buttonIcon, SpriteBatch spriteBatch, SpriteFont spriteFont, String text, Vector2 position, Color color)
        {
            Texture2D buttonTexture;
            switch (buttonIcon)
            {
                case ButtonIcons.BumperLeft:
                    buttonTexture = textureButtonBumperLeft;
                    break;
                case ButtonIcons.BumperRight:
                    buttonTexture = textureButtonBumperRight;
                    break;
                case ButtonIcons.A:
                    buttonTexture = textureButtonA;
                    break;
                case ButtonIcons.B:
                    buttonTexture = textureButtonB;
                    break;
                case ButtonIcons.Back:
                    buttonTexture = textureButtonBack;
                    break;
                case ButtonIcons.Start:
                    buttonTexture = textureButtonStart;
                    break;
                case ButtonIcons.X:
                    buttonTexture = textureButtonX;
                    break;
                case ButtonIcons.Y:
                    buttonTexture = textureButtonY;
                    break;
                case ButtonIcons.DPadDown:
                    buttonTexture = textureButtonDPadDown;
                    break;
                case ButtonIcons.DPadLeft:
                    buttonTexture = textureButtonDPadLeft;
                    break;
                case ButtonIcons.DPadRight:
                    buttonTexture = textureButtonDPadRight;
                    break;
                case ButtonIcons.DPadUp:
                    buttonTexture = textureButtonDPadUp;
                    break;
                case ButtonIcons.DPadLeftRight:
                    buttonTexture = textureButtonDPadLeftRight;
                    break;
                case ButtonIcons.StickLeft:
                    buttonTexture = textureButtonStickLeft;
                    break;
                case ButtonIcons.StickRight:
                    buttonTexture = textureButtonStickRight;
                    break;
                case ButtonIcons.TriggerLeft:
                    buttonTexture = textureButtonTriggerLeft;
                    break;
                case ButtonIcons.TriggerRight:
                    buttonTexture = textureButtonTriggerRight;
                    break;
                default:
                    buttonTexture = textureButtonA;
                    break;
            }

            float scale = ScaleToFit(new Vector2(buttonTexture.Width, buttonTexture.Height), new Vector2(spriteFont.MeasureString(text).Y, spriteFont.MeasureString(text).Y));

            spriteBatch.Draw(buttonTexture, position, null, Color.White, 0f, new Vector2(buttonTexture.Width/2, buttonTexture.Height / 2), scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(spriteFont, text, position + new Vector2((buttonTexture.Width * scale) * 0.8f, 0), color, 0, new Vector2(0, spriteFont.MeasureString(text).Y / 2), 1f, SpriteEffects.None, 0f);
        }
        #endregion
        #endregion

        #region Screens
        #region Draw Title Screen
        private void DrawTitleScreen()
        {
            spriteBatch.Draw(titleScreenTexture, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
        }
        #endregion

        #region Draw Death Screen
        private void DrawDeathScreen()
        {
            spriteBatch.Draw(graphicsSetsContents[selectedGraphicsSet]["overlay"], new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), overlayColorDeath);
            spriteBatch.DrawString(spriteFontXirod1, "You are dead.", new Vector2(SCREEN_WIDTH / 2 - spriteFontXirod1.MeasureString("You are dead.").X / 2, SCREEN_HEIGHT / 2 - spriteFontXirod1.LineSpacing), Color.White);
            //spriteBatch.DrawString(spriteFontXirod2, "Press A to continue..", new Vector2(470, 390), Color.White);
            this.DrawPressButtonText(ButtonIcons.A, spriteBatch, spriteFontXirod2, "Continue", new Vector2(470, 390), Color.White); 
        }
        #endregion

        #region Draw Game Over Screen
        private void DrawGameOverScreen()
        {
            spriteBatch.Draw(graphicsSetsContents[selectedGraphicsSet]["overlay"], new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), overlayColorDeath);
            spriteBatch.DrawString(spriteFontXirod1, "Game Over", new Vector2(SCREEN_WIDTH / 2 - spriteFontXirod1.MeasureString("Game Over").X / 2, SCREEN_HEIGHT / 2 - spriteFontXirod1.LineSpacing), Color.White);
            this.DrawPressButtonText(ButtonIcons.A, spriteBatch, spriteFontXirod2, "Continue", new Vector2(470, 390), Color.White);
        }
        #endregion

        #region Draw Level Change Screen
        private void DrawLevelChangeScreen()
        {
            Vector2 position = new Vector2(270f, 230f);
            spriteBatch.Draw(graphicsSetsContents[selectedGraphicsSet]["levelChange"], new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
            spriteBatch.DrawString(spriteFontXirod1, "LEVEL " + level, position, Color.White);

            spriteBatch.Draw(graphicsSetsContents[selectedGraphicsSet]["cell"], position + new Vector2(0f, 30f), cellHealthyColor);
            spriteBatch.DrawString(spriteFontXirod1, "x" + levelCellCount, position + new Vector2(70f, 50f), Color.White);

            spriteBatch.Draw(graphicsSetsContents[selectedGraphicsSet]["enemy"], position + new Vector2(0f, 90f), Color.White);
            spriteBatch.DrawString(spriteFontXirod1, "x" + levelEnemyCount, position + new Vector2(70f, 110f), Color.White);

          //  spriteBatch.DrawString(spriteFontXirod2, "Press A!", new Vector2(470, 390), Color.White);
            this.DrawPressButtonText(ButtonIcons.A, spriteBatch, spriteFontXirod2, "GO!", new Vector2(470, 390), Color.White);

        }
        #endregion

        #region Draw Pause Screen
        private void DrawPauseScreen()
        {
            spriteBatch.Draw(graphicsSetsContents[selectedGraphicsSet]["overlay"], new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), overlayColorPause);
            spriteBatch.DrawString(spriteFontXirod1, "Paused", new Vector2(SCREEN_WIDTH / 2 - spriteFontXirod1.MeasureString("Pause").X / 2, SCREEN_HEIGHT / 2 - spriteFontXirod1.LineSpacing), Color.White);
            this.DrawPressButtonText(ButtonIcons.Start, spriteBatch, spriteFontXirod2, "Resume", new Vector2(470, 390), Color.White);
        }
        #endregion

        #region Draw Hud
        private void DrawHud(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(graphicsSetsContents[selectedGraphicsSet]["HUD"], new Vector2(SCREEN_WIDTH - graphicsSetsContents[selectedGraphicsSet]["HUD"].Width, SCREEN_HEIGHT - graphicsSetsContents[selectedGraphicsSet]["HUD"].Height), Color.White);
            spriteBatch.DrawString(spriteFontArial1, "" + currentScoreValue * scoreMultiplicator, new Vector2(189 - spriteFontArial1.MeasureString("" + currentScoreValue * scoreMultiplicator).X, SCREEN_HEIGHT - 25), new Color(58, 58, 58));
            spriteBatch.DrawString(spriteFontArial1, this.score.ToString(), new Vector2(557 - spriteFontArial1.MeasureString(this.score.ToString()).X, SCREEN_HEIGHT - 25), new Color(58, 58, 58));
            spriteBatch.DrawString(spriteFontArial1, lives.ToString(), new Vector2(785 - spriteFontArial1.MeasureString(lives.ToString()).X, SCREEN_HEIGHT - 25), new Color(58, 58, 58));
        }
        #endregion
        #endregion
    }
}

