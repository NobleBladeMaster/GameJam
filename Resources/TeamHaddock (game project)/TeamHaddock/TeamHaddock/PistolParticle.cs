using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Created by Alexander 11-21
namespace TeamHaddock
{
    public class PistolParticle
    {
        private CollidableObject collidableObject;
        private float velocity;
        private Vector2 direction;

        /// <summary>
        /// Creates a new pistol bullet/particle
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position">The spawn position of the object</param>
        /// <param name="velocity"></param>
        /// <param name="rotation"></param>
        public PistolParticle(Texture2D texture, Vector2 position, float velocity, float rotation)
        {
            // Create a new collidableobject
            collidableObject = new CollidableObject(texture, position) {Rotation = rotation};
            // Set velocity
            this.velocity = velocity;
            // Set direction
            direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        }

        /// <summary>
        /// Updates particle logic
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // If bullet is colliding with player
            if (collidableObject.IsColliding(InGame.player.collidableObject))
            {
                // Deal damage to player
                InGame.player.TakeDamage(damageType);
                // Kill this particle
                RemoveFromList();
                return;
            }
            // If particle is outside of screen
            if (collidableObject.Position.X < 0 || collidableObject.Position.Y < 0 || collidableObject.Position.X > Game1.ScreenBounds.X)
            {
                RemoveFromList();
                return;
            }
            // Update position
            collidableObject.Position += direction * velocity * gameTime.ElapsedGameTime.Milliseconds;
        }

        /// <summary>
        /// Removes this object from the particles list
        /// </summary>
        private void RemoveFromList()
        {
            Game1.finalActionsDelegate += () => { InGame.particles.Remove(this); };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(collidableObject.Texture, collidableObject.Position, collidableObject.SourceRectangle, Color.White, collidableObject.Rotation, collidableObject.Origin, 1.0f, SpriteEffects.None, 0.0f);
        }

    }
}
