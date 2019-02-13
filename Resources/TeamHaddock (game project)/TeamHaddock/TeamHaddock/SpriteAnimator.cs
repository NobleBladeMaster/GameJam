using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TeamHaddock
{
    class SpriteAnimator
    {

      
                   public Texture2D Texture;
                   

                   private int frameWidth = 0;
                   private int frameHeight = 0;
                   private int currentFrame;
                   private float frameTime = 0.1f;
                   private float timeForCurrentFrame = 0.0f;

                    private Color tintColor = Color.White;
                    private float rotation = 0.0f;
        

        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;

        protected Vector2 position = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;

        protected List<Rectangle> frames = new List<Rectangle>();

        public SpriteAnimator(Vector2 position, Texture2D texture, Rectangle initialFrame, Vector2 velocity)
        {
            this.position = position;
            Texture = texture;
            this.velocity = velocity;


            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
        }
        //returns the position and then sets a value on the position  
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        //returns velocity and sets gives it a value
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        //returns TintColor and sets gives it a value
        public Color TintColor
        {
            get { return tintColor; }
            set { tintColor = value; }
        }
        //returns rotation and gives it a value 
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }
        //currentFrame restricts it with in a specific range, value range 0 and frames,count -1 
        public int Frame
        {
            get { return currentFrame; }
            set { currentFrame = (int)MathHelper.Clamp(value, 0, frames.Count - 1); }
        }

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }

        public Rectangle Source
        {
            get { return frames[currentFrame]; }
        }

        public Rectangle Destination
        {
            get { return new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight); }
        }

        public Vector2 Center
        {
            get { return position + new Vector2(frameWidth / 2, frameHeight / 2); }
        }

        public Rectangle BoundingBoxRect
        {
            get { return new Rectangle((int)position.X + BoundingXPadding, (int)position.Y + BoundingYPadding, frameWidth - (BoundingXPadding * 2), frameHeight - (BoundingYPadding * 2)); }
        }

        public bool IsBoxColliding(Rectangle OtherBox)
        {
            return BoundingBoxRect.Intersects(OtherBox);
        }

        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            if (Vector2.Distance(Center, otherCenter) < (CollisionRadius + otherRadius))
                return true;
            else
                return false;
        }

        public void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        public virtual void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeForCurrentFrame += elapsed;

            if (timeForCurrentFrame >= FrameTime)
            {
                currentFrame = (currentFrame + 1) % (frames.Count);
                timeForCurrentFrame = 0.0f;
            }

            position += (velocity * elapsed);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Center, Source, tintColor, rotation, new Vector2(frameWidth / 2, frameHeight / 2), 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
     
    

