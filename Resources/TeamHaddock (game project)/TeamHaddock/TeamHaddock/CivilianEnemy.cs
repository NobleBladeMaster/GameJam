using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TeamHaddock
{
    class CivilianEnemy : IEnemy
    {
        private CollidableObject collidableObject;
        private static Texture2D colorMap;
        private static Texture2D normalMap;

        public CollidableObject CollidableObject => collidableObject;

        private bool onGround, walkingDirection;

        private Animation moveLeftAnimation;
        private Animation moveRightAnimation;

        private Vector2 velocity;
        private const int defaultHealth = 500;
        private int health;
        private int invulnerabilityFrames;

        private const float baseWalkingSpeed = 0.15f;
        private readonly Vector2 maxMovementSpeed = new Vector2(0.5f, 100f);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="walkingDirection">false for left true for right</param>
        public CivilianEnemy(Vector2 position, bool walkingDirection)
        {
            collidableObject = new CollidableObject(colorMap, position, new Rectangle(120, 0, 98, 114), 0);
            this.walkingDirection = walkingDirection;
            health = (int)(defaultHealth * InGame.difficultyModifier);

            int walkingTime = 200;

            // Load all frames into Animation // Edited by Noble 12-11
            moveRightAnimation = new Animation(new List<Frame>
                {
                    new Frame(new Rectangle(0, 0, 53, 142), walkingTime),
                    new Frame(new Rectangle(53, 0, 53, 142), walkingTime),
                    new Frame(new Rectangle(0, 0, 53, 142), walkingTime),
                    new Frame(new Rectangle(106, 0, 53, 142 ), walkingTime),             
                }
            );
            moveLeftAnimation = new Animation(new List<Frame>
                {
                    new Frame(new Rectangle(0, 143, 53, 142), walkingTime),
                    new Frame(new Rectangle(53, 143, 53, 142), walkingTime),
                    new Frame(new Rectangle(0, 143, 53, 142), walkingTime),
                    new Frame(new Rectangle(106, 143, 53, 142 ), walkingTime),
                }
            );
        }

        public static void LoadContent(ContentManager content)
        {
            colorMap = content.Load<Texture2D>(@"Textures/Characters/Civillian");
            normalMap = content.Load<Texture2D>(@"Textures/Characters/CivillianNormalMap");
        }


        public void Update(GameTime gameTime)
        {
            invulnerabilityFrames -= gameTime.ElapsedGameTime.Milliseconds;
            UpdateAI(gameTime);
            UpdatePosition(gameTime);

            if (collidableObject.Position.X <= -collidableObject.SourceRectangle.Width || collidableObject.Position.X >= Game1.ScreenBounds.X + collidableObject.SourceRectangle.Width)
            {
                RemoveFromList();
            }
        }

        private void UpdateAI(GameTime gameTime)
        {
            // Update ground
            onGround = collidableObject.Position.Y >= Game1.ScreenBounds.Y - (collidableObject.origin.Y + InGame.groundRectangle.Height);

            if (walkingDirection)
            {
                MoveRight(gameTime);
            }
            else
            {
                MoveLeft(gameTime);
            }
            
            Fall(gameTime);
        }

        private void Fall(GameTime gameTime)
        {
            if (!onGround)
            {
                // Add gravity
                AddForce(new Vector2(0, 0.01f * gameTime.ElapsedGameTime.Milliseconds));
            }
        }


        private void MoveLeft(GameTime gameTime)
        {
            // Animate left
            moveLeftAnimation.Animate(ref collidableObject.SourceRectangle, gameTime);
            // Set velocity
            velocity.X = MathHelper.Clamp(-baseWalkingSpeed * InGame.difficultyModifier, -maxMovementSpeed.X, maxMovementSpeed.X);
        }

        private void MoveRight(GameTime gameTime)
        {
            // Animate right
            moveRightAnimation.Animate(ref collidableObject.SourceRectangle, gameTime);
            // Set velocity
            velocity.X = MathHelper.Clamp(baseWalkingSpeed * InGame.difficultyModifier, -maxMovementSpeed.X, maxMovementSpeed.X) ;
        }

        /// <summary>
        /// Adds a force to velocity while clamping velocity to maxMovementSpeed
        /// </summary>
        /// <param name="force"></param>
        private void AddForce(Vector2 force)
        {
            velocity.X = MathHelper.Clamp(velocity.X + force.X, -maxMovementSpeed.X, maxMovementSpeed.X);
            velocity.Y = MathHelper.Clamp(velocity.Y + force.Y, -maxMovementSpeed.Y, maxMovementSpeed.Y);
        }

        public bool TakeDamage(int damageTaken)
        {
            bool tookDamage = false;
            if (invulnerabilityFrames <= 0)
            {
                health -= damageTaken;
                tookDamage = true;
                invulnerabilityFrames += 650;
            }
            // If health reaches 0, kill this enemy.
            if (health <= 0)
            {
                RemoveFromList();
            }

            return tookDamage;
        }

        /// <summary>
        /// Updates position while limiting position to the floor and ceiling in Y and the window sides + origin. 
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdatePosition(GameTime gameTime)
        {
            // Clamp X position + velocity to not go beyond the window + texture
            collidableObject.Position.X = MathHelper.Clamp(
                collidableObject.Position.X + (velocity.X * gameTime.ElapsedGameTime.Milliseconds),
                -collidableObject.origin.X,
                Game1.ScreenBounds.X + collidableObject.SourceRectangle.Width);

            // Clamp Y position + velocity to not go beyond the window - texture
            collidableObject.Position.Y = MathHelper.Clamp(
                collidableObject.Position.Y + (velocity.Y * gameTime.ElapsedGameTime.Milliseconds),
                collidableObject.origin.Y,
                InGame.groundRectangle.Top - collidableObject.origin.Y);
        }

        /// <summary>
        /// Removes this object from the particles list
        /// </summary>
        private void RemoveFromList()
        {
            Game1.finalActionsDelegate += () => { InGame.enemies.Remove(this); };
        }


        public void DrawColorMap(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(colorMap, collidableObject.Position, collidableObject.SourceRectangle, invulnerabilityFrames > 0 ? Color.Red : Color.White, collidableObject.Rotation, collidableObject.origin, 1.0f, SpriteEffects.None, 0.0f);
        }

        public void DrawNormalMap(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(normalMap, collidableObject.Position, collidableObject.SourceRectangle, Color.White, collidableObject.Rotation, collidableObject.origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
