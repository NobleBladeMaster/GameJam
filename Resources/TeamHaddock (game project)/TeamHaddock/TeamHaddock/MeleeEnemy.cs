using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// Created by Alexander 11-28 
namespace TeamHaddock
{
    public class MeleeEnemy
    {
        private CollidableObject collidableObject;
        private static Texture2D colorMap;
        private static Texture2D normalMap;
        private static Texture2D collisionMap;

        public CollidableObject CollidableObject => collidableObject;

        private Animation moveLeftAnimation;
        private Animation moveRightAnimation;

        private Vector2 velocity;
        private Vector2 direction;
        private const int defaultHealth = 1000;
        private int health;

        private const float baseWalkingSpeed = 0.17f, baseJumpStrength = -1.35f;
        private readonly Vector2 maxMovementSpeed = new Vector2(0.5f, 100f);
        private int timeSinceLastJump;
        private bool onGround;

        private int invulnerabilityFrames;

        // Edited by Noble 12-10, Alexander 12-11
        public MeleeEnemy(Vector2 position)
        {
            collidableObject = new CollidableObject(collisionMap, position, new Rectangle(120, 0, 98, 114), 0);
            health = (int)(defaultHealth * InGame.difficultyModifier);
            int walkingTime = 200; 
            
            // Load all frames into Animation // Edited by Noble 12-10 
            moveRightAnimation = new Animation(new List<Frame>
                {
                    new Frame(new Rectangle(0, 0, 98, 114), walkingTime),
                    new Frame(new Rectangle(99, 0, 98, 114), walkingTime),
                    new Frame(new Rectangle(198, 0, 98, 114), walkingTime),
                    new Frame(new Rectangle(99, 0, 98, 114), walkingTime),
                }
            );
            moveLeftAnimation = new Animation(new List<Frame>
                {
                    new Frame(new Rectangle(0, 115, 98, 113), walkingTime),
                    new Frame(new Rectangle(99, 115, 98, 113), walkingTime),
                    new Frame(new Rectangle(198, 115, 98, 113), walkingTime),
                    new Frame(new Rectangle(99, 115, 98, 113), walkingTime),
                }
            );
        }

        public static void LoadContent(ContentManager content)
        {
            colorMap = content.Load<Texture2D>(@"Textures/Characters/BatonPolice");
            normalMap = content.Load<Texture2D>(@"Textures/Characters/BatonPoliceNormalMap");
            collisionMap = content.Load<Texture2D>(@"Textures/CollisionMaps/BatonPoliceCollisionMap");
        }

        public void Update(GameTime gameTime)
        {
            invulnerabilityFrames -= gameTime.ElapsedGameTime.Milliseconds;
            UpdateAI(gameTime);
            UpdatePosition(gameTime);
            UpdateAttack(gameTime);
        }

        /// <summary>
        /// Moves enemy closer to the player depending their position
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateAI(GameTime gameTime)
        {
            // Update ground
            onGround = collidableObject.Position.Y >=
                       Game1.ScreenBounds.Y - (collidableObject.origin.Y + InGame.groundRectangle.Height);

            // Move left when player is to the left
            if (collidableObject.Position.X > InGame.player.collidableObject.Position.X -
                (InGame.player.collidableObject.origin.X + collidableObject.origin.X))
            {
                MoveLeft(gameTime);
            }

            // Move right when player is to the right
            if (collidableObject.Position.X < InGame.player.collidableObject.Position.X +
                (InGame.player.collidableObject.origin.X + collidableObject.origin.X))
            {
                MoveRight(gameTime);
            }

            // when enemy is near the player 
            if (collidableObject.Position.X > InGame.player.collidableObject.Position.X -
                (InGame.player.collidableObject.origin.X + collidableObject.origin.X) && collidableObject.Position.X <
                InGame.player.collidableObject.Position.X +
                (InGame.player.collidableObject.origin.X + collidableObject.origin.X))
            {
                // player is above enemy and jump is not complete
                if (InGame.player.collidableObject.Position.Y < collidableObject.Position.Y)
                {
                    // Start jump
                    // If jumpTime is reset and is on ground
                    if (timeSinceLastJump > 1000 && onGround)
                    {
                        Jump();
                        timeSinceLastJump = 0;
                    }
                }
            }


                // Move left when player is to the left
                if (collidableObject.Position.X > InGame.player.collidableObject.Position.X -
                    (InGame.player.collidableObject.origin.X + collidableObject.origin.X))
                {
                    MoveLeft(gameTime);
                }

                // Move right when player is to the right
                if (collidableObject.Position.X < InGame.player.collidableObject.Position.X +
                    (InGame.player.collidableObject.origin.X + collidableObject.origin.X))
                {
                    MoveRight(gameTime);
                }

                // Stop when enemy is near the player 
                if (collidableObject.Position.X > InGame.player.collidableObject.Position.X -
                    (InGame.player.collidableObject.origin.X + collidableObject.origin.X)
                    && collidableObject.Position.X < InGame.player.collidableObject.Position.X +
                    (InGame.player.collidableObject.origin.X + collidableObject.origin.X))
                {
                    StopMoving();
                }
            }
        }

        // Created by Noble 11-21, Edited by Noble 11-28 , Edited by Alexander 12-06
        private void Jump()
        {
            velocity.Y = baseJumpStrength;
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
            // Set direction to left
            direction.X = -1;
            // Set velocity
            velocity.X = MathHelper.Clamp(-baseWalkingSpeed * InGame.difficultyModifier, -maxMovementSpeed.X, maxMovementSpeed.X);
        }

        private void MoveRight(GameTime gameTime)
        {
            // Animate right
            moveRightAnimation.Animate(ref collidableObject.SourceRectangle, gameTime);
            // Set direction to right
            direction.X = 1;
            // Set velocity
            velocity.X = MathHelper.Clamp(baseWalkingSpeed * InGame.difficultyModifier, -maxMovementSpeed.X, maxMovementSpeed.X);
        }

        /// <summary>
        /// resets animation and velocity
        /// </summary>
        private void StopMoving()
        {
            // Set velocity to 0
            velocity.X = 0f;
            // If direction is right
            if (direction.X > 0)
            {
                // Set to idle frame of right animation
                moveRightAnimation.SetToFrame(ref collidableObject.SourceRectangle, 0);
            }
            // Else direction is left
            else
            {
                // Set to idle frame of left animation
                moveLeftAnimation.SetToFrame(ref collidableObject.SourceRectangle, 0);
            }
        }

        /// <summary>
        /// Checks for a collision between the player and this enemy and deals damage to player when true
        /// </summary>
        private void UpdateAttack(GameTime gameTime)
        {
            if (collidableObject.IsColliding(InGame.player.collidableObject))
            {
                InGame.player.TakeDamage((int)(850 * InGame.difficultyModifier), gameTime);
            }
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
        /// Removes this object from the particles list
        /// </summary>
        private void RemoveFromList()
        {
            Game1.finalActionsDelegate += () => { InGame.enemies.Remove(this); };
        }


        // Created by Alexander 11-22
        /// <summary>
        /// Updates position while limiting position to the floor and ceiling in Y and the window sides + origin. 
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdatePosition(GameTime gameTime)
        {
            // Clamp X position + velocity to not go beyond the window + texture
            collidableObject.Position.X = MathHelper.Clamp(
                collidableObject.Position.X + (velocity.X * gameTime.ElapsedGameTime.Milliseconds),
                0 - collidableObject.origin.X,
                Game1.ScreenBounds.X + collidableObject.origin.X);

            // Clamp Y position + velocity to not go beyond the window - texture
            collidableObject.Position.Y = MathHelper.Clamp(
                collidableObject.Position.Y + (velocity.Y * gameTime.ElapsedGameTime.Milliseconds),
                0 + collidableObject.origin.Y,
                InGame.groundRectangle.Top - collidableObject.origin.Y);
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
