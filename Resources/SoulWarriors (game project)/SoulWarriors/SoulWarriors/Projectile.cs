using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Created by Alexander 11-21
namespace SoulWarriors
{
    public class Projectile
    {
        private readonly CollidableObject _collidableObject;
        private readonly float _velocity;
        private readonly Vector2 _direction;
        private readonly Rectangle _bounds;
        public bool Active;

        /// <summary>
        /// Creates a new Projectile
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position">The spawn position of the object</param>
        /// <param name="velocity"></param>
        /// <param name="rotation"></param>
        /// <param name="bounds">Play area bounds for where the arrow can be</param>
        public Projectile(Texture2D texture, Vector2 position, float velocity, float rotation, Rectangle bounds)
        {
            // Create a new collidableobject
            _collidableObject = new CollidableObject(texture, position) {Rotation = rotation};
            // Set velocity
            _velocity = velocity;
            // Set direction
            _direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            _bounds = bounds;
            Active = true;
        }

        public Projectile(Texture2D texture, Vector2 from, Vector2 to, float velocity, Rectangle bounds)
        {
            _direction = Vector2.Normalize(new Vector2(to.X - from.X, to.Y - from.Y));
            _collidableObject = new CollidableObject(texture, from) {Rotation = (float)Math.Atan(_direction.Y / _direction.X)};
            _velocity = velocity;
            _bounds = bounds;
            Active = true;
        }
        /// <summary>
        /// Updates particle logic
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // If this arrow is not active, return.
            if (!Active) return;
            // If Projectile is outside of bounds
            if (!_bounds.Contains(_collidableObject.BoundingRectangle))
            {
                // Remove Projectile from list
                Active = false;
                return;
            }

            // Update position
            _collidableObject.Position += _direction * _velocity * gameTime.ElapsedGameTime.Milliseconds;
            // Check collision TODO: Add collision
            foreach (Enemy enemy in InGame.Enemies)
            {
                enemy.CollidableObject.IsColliding(_collidableObject);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // If this arrow is not active, return.
            if (!Active) return;
            spriteBatch.Draw(_collidableObject.Texture, _collidableObject.Position, _collidableObject.SourceRectangle, Color.White, _collidableObject.Rotation, _collidableObject.origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
