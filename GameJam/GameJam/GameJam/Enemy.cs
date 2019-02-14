using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam
{
    public class Enemy
    {
        public Sprite enemySprite;
        public Vector2 gunOffset = new Vector2(25, 25);
        private Queue<Vector2> waypoints = new Queue<Vector2>();
        private Vector2 currentWaypoint = Vector2.Zero;
        private float speed = 120f;
        public bool destroyed = false;
        private int enemyRadius = 15;
        private Vector2 previousPosition = Vector2.Zero;

        // Constructor
        public Enemy(Texture2D texture, Vector2 position, Rectangle initialFrame, int frameCount)
        {
            enemySprite = new Sprite(position, texture, initialFrame, Vector2.Zero);

            for (int x = 1; x < frameCount; x++)
            {
                enemySprite.AddFrame(new Rectangle(initialFrame.X = (initialFrame.Width * x), initialFrame.Y, initialFrame.Width, initialFrame.Height));
            }
            previousPosition = position;
            currentWaypoint = position;
            enemySprite.CollisionRadius = enemyRadius;
        }

        public void AddWaypoint(Vector2 waypoint)
        {
            waypoints.Enqueue(waypoint);
        }

        public bool WaypointReached()
        {
            if (Vector2.Distance(enemySprite.Position, currentWaypoint) < (float)enemySprite.Source.Width / 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsActive()
        {
            if (destroyed)
            {
                return false;
            }

            if (waypoints.Count > 0)
            {
                return true;
            }

            if (WaypointReached())
            {
                return false;
            }

            return true;
        }

        // Update
        public void Update(GameTime gameTime)
        {
            if (IsActive())
            {
                Vector2 heading = currentWaypoint - enemySprite.Position;
                if (heading != Vector2.Zero)
                {
                    heading.Normalize();
                }
                heading *= speed;
                enemySprite.Velocity = heading;
                previousPosition = enemySprite.Position;
                enemySprite.Update(gameTime);
                enemySprite.Rotation = (float)Math.Atan2(enemySprite.Position.Y - previousPosition.Y, enemySprite.Position.X - previousPosition.X);

                if (WaypointReached())
                {
                    if (waypoints.Count > 0)
                    {
                        currentWaypoint = waypoints.Dequeue();
                    }
                }
            }
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive())
            {
                enemySprite.Draw(spriteBatch);
            }
        }
    }
}
