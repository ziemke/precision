using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    class Cell : Actor
    {
       internal enum CellState { Healthy, Attacked, Saved, Dead };

        #region Fields
        const float CELL_SAVING_TIME = 0.5f;
        const int CELL_PLACEMENT_RETRYCOUNT = 100;

        internal static int cellCount;
        CellState state;

        Color healthyColor;
        Color deadColor;

        List<Color> lifeBarColorList;


        float lifePercent;
        float deathTime;

        const int LIFEBAR_HEIGHT = 8;
        private BackgroundBar lifeBar;
        
        #endregion

        #region Properties
        internal CellState State
        {
            get { return state; }
        }

        internal int CellCount
        {
            get { return cellCount; }
        }
	
        #endregion

        #region Constructors
        internal Cell(Texture2D texture, Color healthyColor, Color deadColor)
            : base(texture)
        {
            this.healthyColor = healthyColor;
            this.deadColor = deadColor;
            System.Random random = new System.Random();
            this.lifePercent = 1f;
            this.state = CellState.Healthy;
            this.tint = healthyColor;
            cellCount++;

            lifeBarColorList = new List<Color>();
            this.lifeBarColorList.Add(Color.Red);
            this.lifeBarColorList.Add(Color.Yellow);
            this.lifeBarColorList.Add(Color.Green);
        }
        #endregion

        #region Methods
        #region override Update
        internal override void Update(GameTime gameTime)
        {
            if (this.state == CellState.Attacked)
            {
                this.lifePercent -= (float)gameTime.ElapsedGameTime.TotalSeconds / this.deathTime * TimeScale;
                this.lifeBar.Percent = this.lifePercent;

                if (lifePercent <= 0)
                {
                    this.state = CellState.Dead;
                    cellCount--;
                    Bar.bars.Remove(this.lifeBar);
                    this.lifeBar = null;
                    PrecisionGame.AddEnemyAdd(this.Position);
                    PrecisionGame.currentScoreValue -= PrecisionGame.CELL_DEATH_PENALTY;
                }

                this.tint = this.Lerp(deadColor, healthyColor, lifePercent);
            }

            if (this.state == CellState.Saved && !this.isScaling)
            {
                Actor.actors.Remove(this);
                cellCount--;
            }
            base.Update(gameTime);
        }
        #endregion

        #region SetAttacked
        internal void SetAttacked(float deathTime) {
            if (this.state == CellState.Healthy)
            {
                lifeBar = new BackgroundBar(this.texture.Width, LIFEBAR_HEIGHT, lifeBarColorList);
                lifeBar.Percent = 1f;
                this.deathTime = deathTime;
                this.state = CellState.Attacked;
                this.lifeBar.Position = new Vector2(this.Position.X - this.Origin.X, this.Position.Y + this.Origin.Y + 2);
            }
        }
        #endregion

        #region Saved
        internal void Saved() {
            if (this.state != CellState.Saved)
            {
               this.state = CellState.Saved;
               this.StartScale(0f, CELL_SAVING_TIME);

                Bar.bars.Remove(this.lifeBar);
                this.lifeBar = null;
            }
        }
        #endregion

        #region static AddCells
        internal static void AddCells(int numCells, Texture2D texture, Color healthyColor, Color deadColor, Color barHealthyColor, Color barDeadColor)
        {
            for (int i = 0; i < numCells; i++)
            {
                Cell cell = new Cell(texture, healthyColor, deadColor);

                int c = 0;
                do
                {
                    cell.Position = PrecisionGame.GetRandomScreenPosition(cell.Radius);
                    c++;
                }
                while (cell.CheckCollisionWithAny() && c < CELL_PLACEMENT_RETRYCOUNT);
            }
        }
        #endregion

        #region Lerp
        internal Color Lerp(Color color1, Color color2, float amount) {
            byte r = (byte)MathHelper.Lerp(color1.R, color2.R, amount);
            byte g = (byte)MathHelper.Lerp(color1.G, color2.G, amount);
            byte b = (byte)MathHelper.Lerp(color1.B, color2.B, amount);
            byte a = (byte)MathHelper.Lerp(color1.A, color2.A, amount);

            return new Color(r, g, b, a);
        }
        #endregion

        #region End Level
        internal void EndLevel()
        {
            Bar.bars.Remove(this.lifeBar);
            this.lifeBar = null;
        }
        #endregion

        //#region Kill
        //internal void Kill()
        //{
        //    if (this.state == CellState.Dead) return;

        //    this.state = CellState.Dead;
        //    cellCount--;
        //    Bar.bars.Remove(this.lifeBar);
        //    this.lifeBar = null;
        //    PrecisionGame.currentScoreValue -= PrecisionGame.CELL_DEATH_PENALTY;
        //    this.lifePercent = -0.1f;
        //    this.tint = deadColor;
        //}
        //#endregion
        #endregion
    }
}
