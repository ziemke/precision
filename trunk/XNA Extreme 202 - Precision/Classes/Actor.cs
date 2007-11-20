using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Precision.Classes
{
    internal class Actor
    {
        #region Fields
        internal static List<Actor> actors;

        protected Texture2D texture;
        protected Color tint;

        const float FLICKER_FREQUENCY = 15f;
        protected bool visible;
        bool flicker;
        double nextFlickerUpdate;

        protected float scale;
        float scaleTime;
        float scalePerSecond;

        static float timeScale = 1f;

        protected float rotate;
        #endregion

        #region Properties
        internal Vector2 Position { get; set; }

        internal Vector2 Origin
        {
            get { return new Vector2(texture.Width / 2, texture.Height / 2); }
        }

        internal int Radius
        {
            get { return texture.Width / 2; }
        }

        internal bool Flicker
        {
            set
            {
                flicker = value;
                visible = !value;
            }
        }

        internal float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        internal bool isScaling
        {
            get { return scaleTime > 0.0f; }
        }


        internal  static float TimeScale
        {
            get { return timeScale; }
            set { timeScale = value; }
        }

        internal bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
	
        #endregion

        #region Constructors
        static Actor()
        {
            actors = new List<Actor>();
        }

        internal Actor(Texture2D texture)
        {
            if (!(this is Powerup)) 
                actors.Add(this);
            this.texture = texture;
            this.tint = Color.White;
            this.visible = true;
            this.scale = 1f;
            this.rotate = 0f;
        }
        #endregion

        #region Methods
        #region Update
        internal virtual void Update(GameTime gameTime) 
        {
            if (this.flicker)
            {
                if (gameTime.TotalGameTime.TotalSeconds > nextFlickerUpdate)
                {
                    this.visible = !this.visible;
                    this.nextFlickerUpdate = gameTime.TotalGameTime.TotalSeconds + 1 / FLICKER_FREQUENCY;
                }
            }

            if (isScaling)
            {
                this.scale += scalePerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
                this.scaleTime -= (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
            }
        }
        #endregion

        #region DrawActors
        internal static void DrawActors(SpriteBatch spriteBatch)
        {
            foreach (Actor actor in actors)
            {
                if (!(actor is Powerup))
                    actor.Draw(spriteBatch);
            }
        }
        #endregion

        #region Draw
        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.visible)
            {
                spriteBatch.Draw(texture, this.Position, null, this.tint, this.rotate, this.Origin, scale, SpriteEffects.None, 0f);

                Player player = this as Player;
                if (player != null) player.DrawOverlayTextures(spriteBatch);
            }
        }
        #endregion

        #region CheckCollision
        internal static bool CheckCollision(Actor actorA, Actor actorB)
        {
            float distance = Vector2.Distance(actorA.Position, actorB.Position);

            return distance < actorA.Radius + actorB.Radius;
        }
        #endregion

        #region CheckCollisionWithAny
        internal bool CheckCollisionWithAny()
        {

            foreach (Actor actor in actors)
            {
                if (actor != (Actor)this)
                    if (CheckCollision((Actor)this, actor))
                        return true;
            }

            foreach (Powerup powerup in Powerup.powerups)
            {
                if (powerup != (Actor)this)
                    if (CheckCollision((Actor)this, (Actor)powerup))
                        return true;
            }
            return false;
        }
        #endregion
        
        #region StartScale
        internal void StartScale(float targetScale, float scaleTime)
        {
            if (isScaling)
                return;

            this.scaleTime = scaleTime;
            this.scalePerSecond = (targetScale - this.scale) / scaleTime;
        }
        #endregion 
        #endregion
    }
}
