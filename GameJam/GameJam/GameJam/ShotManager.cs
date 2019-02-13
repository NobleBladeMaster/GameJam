using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam
{
    class ShotManager
    {
        // Variables
        private static Texture2D Texture;
        private static Rectangle InitialFrame;
        private static int FrameCount;
        private static int CollisionRadius;
        private readonly float shotSpeed;
        private Rectangle screenBounds;
        public List<SpriteManager> Shots = new List<SpriteManager>();

        // Constructor
        public ShotManager(Texture2D texture, Rectangle initialFrame, int frameCount, int collisionRadius,
            float shotSpeed, Rectangle screenBounds)
        {
            Texture = texture;
            InitialFrame = initialFrame;
            FrameCount = frameCount;
            CollisionRadius = collisionRadius;
            this.shotSpeed = shotSpeed;
            this.screenBounds = screenBounds;
        }

        // Fireshot
        public void FireShot(Vector2 position, Vector2 velocity, bool playerFired)
        {
            var thisShot = new SpriteManager(position, Texture, InitialFrame, velocity);

            thisShot.Velocity *= shotSpeed;

            for (var i = 1; i < FrameCount; i++)
                thisShot.AddFrame(new Rectangle(InitialFrame.X + InitialFrame.Width * i, InitialFrame.Y,
                    InitialFrame.Width, InitialFrame.Height));
            thisShot.CollisionRadius = CollisionRadius;
            Shots.Add(thisShot);
        }

        // Update 
        public void Update(GameTime gameTime)
        {
            for (var i = Shots.Count - 1; i >= 0; i--)
            {
                Shots[i].Update(gameTime);
                if (!screenBounds.Intersects(Shots[i].Destination)) Shots.RemoveAt(i);
            }
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var shot in Shots) shot.Draw(spriteBatch);
        }
    }
}
    

