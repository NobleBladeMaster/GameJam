using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SoulWarriors
{

    public enum SpawnAreas
    {
        Left,
        Middle,
        Right
    }

    public enum Targets
    {
        Door,
        Archer,
        Knight
    }

    public enum AiTypes
    {
        /// <summary>
        /// Always prioritize Player
        /// </summary>
        Brute,
        /// <summary>
        /// Prioritize Player if within radius else Door, if already at Door stay at Door
        /// </summary>
        Smart,
        /// <summary>
        /// Always prioritize Door
        /// </summary>
        Rush
    }

    public abstract class Enemy
    {

        public CollidableObject CollidableObject;

        private AiTypes _aiType;

        protected Targets Target;
        protected Vector2 TargetPosition;

        private const float targetingRange = 150f;
        private const float attackingRange = 40f;

        private Rectangle spawnLeft = new Rectangle(100, 300, 500, 700);
        private Rectangle spawnMiddle = new Rectangle(700, 800, 1200, 1050);
        private Rectangle spawnRight = new Rectangle(1600, 600, 1800, 800);
        private Random random = new Random();
        private int spawnX;
        private int spawnY;


        protected bool attacking = false; // TODO: create a way to decide which direction to attack in.

        /// <summary>
        /// movement speed in pixels per millisecond
        /// </summary>
        protected float speed;

        protected Enemy(Texture2D texture, SpawnAreas spawnArea, AiTypes aiType, float speed)
        {
            switch (spawnArea)
            {
                case SpawnAreas.Left:
                    spawnX = random.Next(spawnLeft.X + texture.Width / 2, spawnLeft.Width - texture.Width / 2);
                    spawnY = random.Next(spawnLeft.Y + texture.Height / 2, spawnLeft.Height - texture.Height / 2);
                    break;
                case SpawnAreas.Middle:
                    spawnX = random.Next(spawnMiddle.X + texture.Width / 2, spawnMiddle.Width - texture.Width / 2);
                    spawnY = random.Next(spawnMiddle.Y + texture.Height / 2, spawnMiddle.Height - texture.Height / 2);
                    break;
                case SpawnAreas.Right:
                    spawnX = random.Next(spawnRight.X + texture.Width / 2, spawnRight.Width - texture.Width / 2);
                    spawnY = random.Next(spawnRight.Y + texture.Height / 2, spawnRight.Height - texture.Height / 2);
                    break;
            }
            
            CollidableObject = new CollidableObject(texture, new Vector2(spawnX, spawnY), new Rectangle(0, 0, 100, 100), 0f);
            _aiType = aiType;
            this.speed = speed;
            // Set initial targets
            switch (aiType)
            {
                case AiTypes.Brute:
                    SetTarget(Targets.Knight);
                    break;
                case AiTypes.Smart:
                    SetTarget(Targets.Door);
                    break;
                case AiTypes.Rush:
                    SetTarget(Targets.Door);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(aiType), aiType, null);
            }
        }


        public virtual void Update(GameTime gameTime)
        {
            switch (_aiType)
            {
                case AiTypes.Brute:
                    UpdateBruteAi();
                    break;
                case AiTypes.Smart:
                    UpdateSmartAi();
                    break;
                case AiTypes.Rush:
                    UpdateRushAi();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            MovementAi(gameTime);
        }


        private void UpdateBruteAi()
        {
            // Get distances to Targets
            float distanceToKnight = Vector2.Distance(CollidableObject.Position, InGame.Knight.CollidableObject.Position);
            float distanceToArcher = Vector2.Distance(CollidableObject.Position, InGame.Archer.CollidableObject.Position);

            attacking = false;

            // If the knight is closer than the archer
            if (distanceToKnight < distanceToArcher)
            {
                // Set target to knight
                SetTarget(Targets.Knight);
                // If Knight is within attacking range
                if (distanceToKnight < attackingRange)
                {
                    attacking = true;
                }
                return;
            }
            // Else change to Archer
            else
            {
                // Set target to Archer
                SetTarget(Targets.Archer);
                // If Archer is within attacking range
                if (distanceToArcher < attackingRange)
                {
                    attacking = true;
                }
                return;
            }


        }

        private void UpdateSmartAi()
        {
            // Get distances to Targets
            float distanceToKnight = Vector2.Distance(CollidableObject.Position, InGame.Knight.CollidableObject.Position);
            float distanceToArcher = Vector2.Distance(CollidableObject.Position, InGame.Archer.CollidableObject.Position);
            float distanceToDoor = Vector2.Distance(CollidableObject.Position, new Vector2(960, 250));

            attacking = false;

            // If Door is within attacking range
            if (distanceToDoor < attackingRange)
            {
                SetTarget(Targets.Door);
                attacking = true;
            }

            // If the knight is within 150 units and is not already targeting Archer
            if (distanceToKnight < targetingRange && Target != Targets.Archer)
            {
                // Set target to knight
                SetTarget(Targets.Knight);
                // If Knight is within attacking range
                if (distanceToKnight < attackingRange)
                {
                    attacking = true;
                }
                return;
            }

            // If the Archer is within 150 units and is not already targeting Knight
            if (distanceToArcher < targetingRange && Target != Targets.Knight)
            {
                // Set target to Archer
                SetTarget(Targets.Archer);
                // If Archer is within attacking range
                if (distanceToArcher < attackingRange)
                {
                    attacking = true;
                }
                return;
            }

            // Else no players are within 150 units
            // Set target to Door
            SetTarget(Targets.Door);
        }

        private void UpdateRushAi()
        {
            float distanceToDoor = Vector2.Distance(CollidableObject.Position, new Vector2(960, 250));
            attacking = false;

            // If Door is within attacking range
            if (distanceToDoor < attackingRange)
            {
                SetTarget(Targets.Door);
                attacking = true;
            }
        }


        /// <summary>
        /// Sets target and targetPosition to a new target
        /// </summary>
        /// <param name="newTarget"></param>
        protected void SetTarget(Targets newTarget)
        {
            this.Target = newTarget;
            switch (newTarget)
            {
                case Targets.Door:
                    TargetPosition = new Vector2(960, 250);
                    break;
                case Targets.Archer:
                    TargetPosition = InGame.Archer.CollidableObject.Position;
                    break;
                case Targets.Knight:
                    TargetPosition = InGame.Knight.CollidableObject.Position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newTarget), newTarget, null);
            }
        }

        private void MovementAi(GameTime gameTime)
        {
            // If attacking then don´t move
            if (attacking) { return; }

            // Reset displacement
            Vector2 displacement = Vector2.Zero;

            // Reset bools
            bool up = false,
                right = false,
                down = false,
                left = false,
                rightUp = false,
                rightDown = false,
                leftDown = false,
                leftUp = false;

            const int margin = 12;

            // If target is within the margin in X-axis
            if (TargetPosition.X - margin <= CollidableObject.Position.X &&
                CollidableObject.Position.X <= TargetPosition.X + margin)
            {
                // if target is above
                up = TargetPosition.Y < CollidableObject.Position.Y;
                // If target is below
                down = CollidableObject.Position.Y < TargetPosition.Y;
            }

            // If target is within the margin in Y-axis
            if (TargetPosition.Y - margin <= CollidableObject.Position.Y &&
                CollidableObject.Position.Y <= TargetPosition.Y + margin)
            {
                // if target is to the left
                left = TargetPosition.X < CollidableObject.Position.X;
                // if target is to the right
                right = CollidableObject.Position.X < TargetPosition.X;
            }

            rightUp = TargetPosition.Y < CollidableObject.Position.Y && CollidableObject.Position.X < TargetPosition.X && !up && !right;
            rightDown = CollidableObject.Position.X < TargetPosition.X && CollidableObject.Position.Y < TargetPosition.Y && !right && !down;
            leftDown = CollidableObject.Position.Y < TargetPosition.Y && TargetPosition.X < CollidableObject.Position.X && !down && !left;
            leftUp = TargetPosition.X < CollidableObject.Position.X && TargetPosition.Y < CollidableObject.Position.Y && !left && !up;

            float sqrt2 = (float)Math.Sqrt(2);

            if (up)
            {
                displacement.Y -= speed * gameTime.ElapsedGameTime.Milliseconds;
            }

            if (rightUp)
            {
                displacement.Y -= (speed / sqrt2) * gameTime.ElapsedGameTime.Milliseconds;
                displacement.X += (speed / sqrt2) * gameTime.ElapsedGameTime.Milliseconds;
            }

            if (right)
            {
                displacement.X += speed * gameTime.ElapsedGameTime.Milliseconds;
            }

            if (rightDown)
            {
                displacement.X += (speed / sqrt2) * gameTime.ElapsedGameTime.Milliseconds;
                displacement.Y += (speed / sqrt2) * gameTime.ElapsedGameTime.Milliseconds;
            }

            if (down)
            {
                displacement.Y += speed * gameTime.ElapsedGameTime.Milliseconds;
            }

            if (leftDown)
            {
                displacement.Y += (speed / sqrt2) * gameTime.ElapsedGameTime.Milliseconds;
                displacement.X -= (speed / sqrt2) * gameTime.ElapsedGameTime.Milliseconds;
            }

            if (left)
            {
                displacement.X -= speed * gameTime.ElapsedGameTime.Milliseconds;
            }

            if (leftUp)
            {
                displacement.X -= (speed / sqrt2) * gameTime.ElapsedGameTime.Milliseconds;
                displacement.Y -= (speed / sqrt2) * gameTime.ElapsedGameTime.Milliseconds;
            }

            AddToPosition(displacement);
        }

        /// <summary>
        /// adds velocity to position while clamping to PlayArea
        /// </summary>
        private void AddToPosition(Vector2 valueToAdd)
        {
            CollidableObject.Position = Vector2.Clamp(
                CollidableObject.Position + valueToAdd,
                new Vector2(InGame.PlayArea.Left, InGame.PlayArea.Top),
                new Vector2(InGame.PlayArea.Right, InGame.PlayArea.Bottom));
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw Enemy
            spriteBatch.Draw(CollidableObject.Texture,
                CollidableObject.Position,
                CollidableObject.SourceRectangle,
                Color.White,
                CollidableObject.Rotation,
                CollidableObject.origin,
                1,
                SpriteEffects.None,
                0);

        }
    }
}
