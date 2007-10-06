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
#endregion

namespace Precision
{
    enum GameState { Title, LevelChange, Playing, Died, GameOver, Pause };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    internal class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Fields
        GraphicsDeviceManager graphics;
        ContentManager content;
        SpriteBatch spriteBatch;
       internal static SpriteFont spriteFont;
        Color overlayColorDeath = new Color(200, 0, 0, 128);
        Color overlayColorPause = new Color(255, 255, 255, 128);
        static Random random;
        GameState gameState;

        internal const int SCREEN_WIDTH = 800;
        internal const int SCREEN_HEIGHT = 600;

        const float PLAYER_SPEED_SLOW = 5f;
        const float PLAYER_SPEED_FAST = 10f;
        const float PLAYER_INVINCIBILITY_TIME = 1f;

        const float ENEMY_BASE_SPEED = 3f;
        const float ENEMY_SPEED_VARIATION = 1f;

        const float CELL_DEATH_TIME = 2f;
        private static Color cellHealthyColor = new Color(124, 207, 86);
        private static Color cellDeadColor = new Color(115, 95, 141);
        private static Color cellBarHealthyColor = new Color(0, 255, 0);
        private static Color cellBarDeadColor = Color.Red;
        double timeUntilAttack;
        const double cellAttackInterval = 1d;
        
        private int level;
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

        internal static SlowMoEffect slowMoEffect;

        static float titleScreenFlick = 0;

        Player player;
        static Texture2D backgroundTexture, titleScreenTexture, titleScreenTextureNormal, titleScreenRolloverTexture, playerTexture, cellTexture, enemyTexture, overlayTexture, levelChangeTexture, timeTexture, extralifeTexture, shieldTexture, slowmoeffectTexture, scoreTexture;
        #endregion

        #region Properties
        private int levelCellCount
        {
            get { return BASE_NUM_CELLS * level; }
        }

        private int levelEnemyCount
        {
            get { return BASE_NUM_ENEMIES * level; }
        }
        #endregion


        internal Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
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
                spriteFont = content.Load<SpriteFont>("Content/Font1");

                backgroundTexture = content.Load<Texture2D>("Content/Background");
                titleScreenTextureNormal = content.Load<Texture2D>("Content/TitleScreen");
                titleScreenRolloverTexture = content.Load<Texture2D>("Content/TitleScreenRollover");
                levelChangeTexture = content.Load<Texture2D>("Content/LevelChangeScreen");
                overlayTexture = content.Load<Texture2D>("Content/Fill22");
                playerTexture = content.Load<Texture2D>("Content/Player");
                cellTexture = content.Load<Texture2D>("Content/Cell");
                enemyTexture = content.Load<Texture2D>("Content/Enemy");
                Bar.texture = content.Load<Texture2D>("Content/Fill22");
                timeTexture = content.Load<Texture2D>("Content/TimePowerup");
                shieldTexture = content.Load<Texture2D>("Content/ShieldPowerup");
                extralifeTexture = content.Load<Texture2D>("Content/ExtraLifePowerup");
                slowmoeffectTexture = content.Load<Texture2D>("Content/SlowMoEffect");
                scoreTexture = content.Load<Texture2D>("Content/ScorePowerUp");

                titleScreenTexture = titleScreenTextureNormal;

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
            if (gamePadState.Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            
             
            switch (gameState)
            {
                case GameState.Title:
                    titleScreenFlick += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (titleScreenFlick > Config.TITLE_SCREEN_FLICK_DURATION)
                    {
                        titleScreenFlick = 0f;
                        if (titleScreenTexture == titleScreenTextureNormal)
                            titleScreenTexture = titleScreenRolloverTexture;
                        else
                            titleScreenTexture = titleScreenTextureNormal;
                    }

                    if (gamePadState.Buttons.Start == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Enter))
                    {
                        this.BeginGame();
                        this.gameState = GameState.LevelChange;
                    }

                    break;

                case GameState.LevelChange:
                    if (gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A))
                    {
                        this.BeginLevel();
                        this.gameState = GameState.Playing;
                    }
                    break;

                case GameState.Playing:

                    timeUntilAttack -= gameTime.ElapsedGameTime.TotalSeconds;                 

                    if (Cell.cellCount <= 0)
                    {
                        this.level++;
                        this.gameState = GameState.LevelChange;
                        this.EndLevel();
                        break;
                    }

                    if (gamePadState.Buttons.Start == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Enter))
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
                                actor.Position += direction * PLAYER_SPEED_FAST;
                            else
                                actor.Position += direction * PLAYER_SPEED_SLOW;

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
                                actor.Position += directionKeyboard * PLAYER_SPEED_FAST;
                            else
                                actor.Position += directionKeyboard * PLAYER_SPEED_SLOW;

                            actor.Position += direction;

                            Vector2 playerPos = actor.Position;
                            if (playerPos.X < actor.Origin.X)
                                playerPos.X = actor.Origin.X;
                            else if (playerPos.X > SCREEN_WIDTH - actor.Origin.X)
                                playerPos.X = SCREEN_WIDTH - actor.Origin.X;
                            if (playerPos.Y < actor.Origin.Y)
                                playerPos.Y = actor.Origin.Y;
                            else if (playerPos.Y > SCREEN_HEIGHT - actor.Origin.Y)
                                playerPos.Y = SCREEN_HEIGHT - actor.Origin.Y;

                            actor.Position = playerPos;

                            slowMoEffect.Position = playerPos;

                            continue;
                        }

                        Enemy enemy = actor as Enemy;
                        if (enemy != null)
                        {
                            if (Actor.CheckCollision(this.player, enemy) && enemy.IsHarmful && !player.IsInvincible)
                            {
                                lives--;
                                if (lives > 0)
                                {
                                    this.gameState = GameState.Died;

                                }
                                else
                                    this.gameState = GameState.GameOver;

                                player.Reset(PLAYER_INVINCIBILITY_TIME);

                            }
                            continue;
                        }

                        Cell cell = actor as Cell;
                        if (cell != null)
                        {

                            if (timeUntilAttack <= 0 && cell.State == Cell.CellState.Healthy)
                            {
                                cell.SetAttacked(CELL_DEATH_TIME);
                                timeUntilAttack = cellAttackInterval;
                            }

                            if (cell.State == Cell.CellState.Attacked && Actor.CheckCollision(cell, player))
                            {
                                cell.Saved();
                                this.score += currentScoreValue * scoreMultiplicator;
                            }
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

                    if (gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A))
                        gameState = GameState.Playing;

                    break;

                case GameState.GameOver:
                    if (gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A))
                        gameState = GameState.Title;
                    break;

                case GameState.Pause:
                    if (gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.A))
                        gameState = GameState.Playing;
                    break;
                default:
                    break;
            }

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

                case GameState.LevelChange:
                    DrawLevelChangeScreen();
                    break;

                case GameState.Playing:
                case GameState.Died:
                case GameState.GameOver:
                case GameState.Pause:
                    spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);

                    Actor.DrawActors(spriteBatch);
                    Bar.DrawBars(spriteBatch);
                    Powerup.DrawPowerups(spriteBatch);

                    spriteBatch.DrawString(spriteFont, "SCORE: " + this.score, new Vector2(4, 0),Color.White);
                    spriteBatch.DrawString(spriteFont, "VALUE: " + currentScoreValue * scoreMultiplicator, new Vector2(4, 20), Color.White);
                    spriteBatch.DrawString(spriteFont, "LIVES: " + lives, new Vector2(4, 40), Color.White);

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

            base.Draw(gameTime);
        }

        #region Gameplay methods
        #region Begin Game
        private void BeginGame()
        {
            this.level = 1;
            lives = STARTING_LIVES;
            this.score = 0;

            Powerup.powerups.Clear();
            Bar.bars.Clear();


            new TimePowerup(timeTexture, Config.timePowerupActiveTimeBarColor, Config.timePowerupPickupTimeBarColor);
            new ScorePowerup(scoreTexture, Config.scorePowerupActiveTimeBarColor, Config.scorePowerupPickupTimeBarColor);
            new ExtraLifePowerup(extralifeTexture, Config.extralivePowerupPickupTimeBarColor);

        }
        #endregion

        #region Begin Level
        private void BeginLevel()
        {
            Actor.TimeScale = 1f;
            Actor.actors.Clear();

            for (int i = 0; i < Powerup.powerups.Count; i++)
            {
                Powerup.powerups[i].BeginLevel();
            }

            Cell.cellCount = 0;

            
            Cell.AddCells(levelCellCount, cellTexture, cellHealthyColor, cellDeadColor, cellBarHealthyColor, cellBarDeadColor);
            Enemy.AddEnemies(levelEnemyCount, enemyTexture, ENEMY_BASE_SPEED, ENEMY_SPEED_VARIATION);

            this.player = new Player(playerTexture);
            this.player.Reset(PLAYER_INVINCIBILITY_TIME);

            this.timeUntilAttack = cellAttackInterval;

            currentScoreValue = INITIAL_SCORE_VALUE * level;

            slowMoEffect = new SlowMoEffect(slowmoeffectTexture);
        }
        #endregion

        #region Ende Level
        private void EndLevel()
        {
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
            return new Vector2(random.Next(padding, SCREEN_WIDTH - padding), random.Next(padding, SCREEN_HEIGHT - padding));
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
            Enemy enemy = new Enemy(enemyTexture, ENEMY_BASE_SPEED, ENEMY_SPEED_VARIATION);
            enemy.Position = position;
        }

        #endregion

        #region Add extra live
        internal static void AddExtraLife()
        {
            lives++;
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
            spriteBatch.Draw(overlayTexture, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), overlayColorDeath);
            spriteBatch.DrawString(spriteFont, "You are dead.", new Vector2(SCREEN_WIDTH / 2 - spriteFont.MeasureString("You are dead.").X / 2, SCREEN_HEIGHT / 2 - spriteFont.LineSpacing), Color.White);
            spriteBatch.DrawString(spriteFont, "Press A to continue..", new Vector2(470, 390), Color.White);
        }
        #endregion

        #region Draw Game Over Screen
        private void DrawGameOverScreen()
        {
            spriteBatch.Draw(overlayTexture, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), overlayColorDeath);
            spriteBatch.DrawString(spriteFont, "Game Over", new Vector2(SCREEN_WIDTH / 2 - spriteFont.MeasureString("Game Over").X / 2, SCREEN_HEIGHT / 2 - spriteFont.LineSpacing), Color.White);
            spriteBatch.DrawString(spriteFont, "Press A to continue..", new Vector2(470, 390), Color.White);
        }
        #endregion

        #region Draw Level Change Screen
        private void DrawLevelChangeScreen()
        {
            Vector2 position = new Vector2(270f, 230f);
            spriteBatch.Draw(levelChangeTexture, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
            spriteBatch.DrawString(spriteFont, "LEVEL " + this.level, position, Color.White);

            spriteBatch.Draw(cellTexture, position + new Vector2(0f, 30f), cellHealthyColor);
            spriteBatch.DrawString(spriteFont, "x" + this.levelCellCount, position + new Vector2(70f, 50f), Color.White);

            spriteBatch.Draw(enemyTexture, position + new Vector2(0f, 90f), Color.White);
            spriteBatch.DrawString(spriteFont, "x" + this.levelEnemyCount, position + new Vector2(70f, 110f), Color.White);

            spriteBatch.DrawString(spriteFont, "Press A!", new Vector2(470, 390), Color.White);

        }
        #endregion

        #region Draw Pause Screen
        private void DrawPauseScreen()
        {
            spriteBatch.Draw(overlayTexture, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), overlayColorPause);
            spriteBatch.DrawString(spriteFont, "Paused", new Vector2(SCREEN_WIDTH / 2 - spriteFont.MeasureString("Pause").X / 2, SCREEN_HEIGHT / 2 - spriteFont.LineSpacing), Color.White);
            spriteBatch.DrawString(spriteFont, "Press A to continue.", new Vector2(470, 390), Color.White);
        }
        #endregion
        #endregion
    }
}
