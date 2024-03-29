using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class Enemy : Actor
    {
        #region Fields
        const float ENEMY_SCALE_TIME = 0.5f;
        const float ENEMY_MAX_ROTATE_BASE_SPEED = 8f;

        float baseSpeed;
        float speedVariation;
        private float BaseRotateSpeed;

        Vector2 pointA, pointB;

        private float amount;
        float moveTime;

        bool isDying;
        #endregion

        #region Properties
        internal bool IsHarmful
        {
            get { return !this.isScaling; }
        }

        internal float Amount
        {
            get { return amount; }
        }

        internal bool IsDying
        {
            get { return isDying; }
            set { isDying = value; }
        }
	
        #endregion

        #region Constructors
        internal Enemy(Texture2D texture, float baseSpeed, float speedVariation) : base(texture)
        {
            this.baseSpeed = baseSpeed;
            this.speedVariation = speedVariation;

            System.Random random = new System.Random();
            this.Position = new Vector2(random.Next(300), random.Next(300));

            this.Scale = 0f;
            this.StartScale(1f, ENEMY_SCALE_TIME);
            this.BaseRotateSpeed = PrecisionGame.Range(-ENEMY_MAX_ROTATE_BASE_SPEED, ENEMY_MAX_ROTATE_BASE_SPEED);

            isDying = false;
        
        }
        #endregion

        #region Methods
        #region override Update
        internal override void Update(GameTime gameTime)
        {
            if (moveTime <= 0)
                this.SetRandomMove();

            if (this.IsHarmful)
            {
                amount += (float)gameTime.ElapsedGameTime.TotalSeconds / moveTime * TimeScale;
                this.Position = Vector2.SmoothStep(pointA, pointB, this.amount);
            }

            UpdateRotate(gameTime);

            if (this.amount >= 1f)
                this.SetRandomMove();

            if (this.scale < 0)
                Actor.actors.Remove(this);

            base.Update(gameTime);
        }
        #endregion

        #region SetRandomMove
        private void SetRandomMove() {

            this.pointA = this.Position;
            this.pointB = PrecisionGame.GetRandomScreenPosition(this.Radius);

            this.moveTime = baseSpeed + PrecisionGame.Range(-speedVariation, speedVariation);
            this.amount = 0;
        }
        #endregion

        #region Update Rotate
        private void UpdateRotate(GameTime gameTime)
        {
            this.rotate += this.BaseRotateSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds / moveTime * TimeScale;
        }
        #endregion

        #region static AddEnemies
        internal static void AddEnemies(int numEnemies, Texture2D texture, float baseSpeed, float speedVariation)
        {

            for (int i = 0; i < numEnemies; i++)
            {
                Enemy enemy = new Enemy(texture, baseSpeed, speedVariation);
                enemy.Position = PrecisionGame.GetRandomScreenPosition(enemy.Radius);
            }
        }
        #endregion

        #region Kill
        internal void Kill()
        {
            StartScale(-1, 3f);
            isDying = true;
            Audio.Play(Audio.Cue.Enemy_Kill);
        }
        #endregion
        #endregion
    }
}
