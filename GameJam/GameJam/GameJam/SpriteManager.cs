using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam
{
  public class SpriteManager
    {
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;

        public int CollisionRadius = 0;
        private int currentFrame;
        private readonly int frameHeight;

        protected List<Rectangle> frames = new List<Rectangle>();
        private float frameTime = 0.1f;

        private readonly int frameWidth;

        protected Vector2 position = Vector2.Zero;
        private float rotation;

        public Texture2D texture;
        private float timeForCurrentFrame;

        protected Vector2 velocity = Vector2.Zero;

        public SpriteManager
        (Vector2 position,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
            this.texture = texture;

            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Color TintColor { get; set; } = Color.White;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

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

        public Rectangle Source => frames[currentFrame];

        public Rectangle Destination => new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

        public Vector2 Center => position + new Vector2(frameWidth / 2, frameHeight / 2);

        public Rectangle BoundingBoxRect => new Rectangle((int)position.X + BoundingXPadding,
            (int)position.Y + BoundingYPadding, frameWidth - BoundingXPadding * 2, frameHeight - BoundingYPadding * 2);

        public bool IsBoxColliding(Rectangle OtherBox)
        {
            return BoundingBoxRect.Intersects(OtherBox);
        }

        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            if (Vector2.Distance(Center, otherCenter) < CollisionRadius + otherRadius)
                return true;
            return false;
        }

        public void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        public virtual void Update(GameTime gameTime)
        {
            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeForCurrentFrame += elapsed;

            if (timeForCurrentFrame >= FrameTime)
            {
                currentFrame = (currentFrame + 1) % frames.Count;
                timeForCurrentFrame = 0.0f;
            }

            position += velocity * elapsed;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Center, Source, TintColor, rotation, new Vector2(frameWidth / 2, frameHeight / 2),
                1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
