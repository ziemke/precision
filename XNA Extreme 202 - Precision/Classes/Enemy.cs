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
            this.BaseRotateSpeed = Game1.Range(-ENEMY_MAX_ROTATE_BASE_SPEED, ENEMY_MAX_ROTATE_BASE_SPEED);
        
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


            base.Update(gameTime);
        }
        #endregion

        #region SetRandomMove
        private void SetRandomMove() {

            this.pointA = this.Position;
            this.pointB = Game1.GetRandomScreenPosition(this.Radius);

            this.moveTime = baseSpeed + Game1.Range(-speedVariation, speedVariation);
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
                enemy.Position = Game1.GetRandomScreenPosition(enemy.Radius);
            }
        }
        #endregion

        #endregion
    }
}
