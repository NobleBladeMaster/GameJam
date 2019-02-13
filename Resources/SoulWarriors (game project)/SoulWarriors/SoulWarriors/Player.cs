using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoulWarriors
{
    public struct PlayerControlScheme
    {
        public Keys MoveUp { get; set; }
        public Keys MoveDown { get; set; }
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        public Keys Action1 { get; set; }
        public Keys Action2 { get; set; }
        public Keys Action3 { get; set; }
        public Keys Action4 { get; set; }

        public PlayerControlScheme(Keys moveUp, Keys moveDown, Keys moveLeft, Keys moveRight, Keys action1, Keys action2, Keys action3, Keys action4)
        {
            MoveUp = moveUp;
            MoveDown = moveDown;
            MoveLeft = moveLeft;
            MoveRight = moveRight;
            Action1 = action1;
            Action2 = action2;
            Action3 = action3;
            Action4 = action4;
        }
    }

    public abstract class Player
    {
        private static KeyboardState _currentKeyboardState;
        private const float MaxChainLength = 640f; // TODO: Give MaxChainLength a more descriptive name

        private readonly PlayerControlScheme _controlScheme;
        public readonly AnimationSet _animationSet;

        public CollidableObject CollidableObject;

        /// <summary>
        /// movement speed in pixels per millisecond
        /// </summary>
        private float speed = .2f;

        protected Player(Texture2D texture, Vector2 spawnPosition, PlayerControlScheme controlScheme, List<Animation> animations)
        {
            _controlScheme = controlScheme;
            _animationSet = new AnimationSet(animations, AnimationStates.Idle, AnimationDirections.Down);

            Rectangle initialSourceRectangle = Rectangle.Empty;
            // Set initialSourceRectangle to the first frame in the first animation
            animations[0].SetToFrame(ref initialSourceRectangle, 0);
            CollidableObject = new CollidableObject(texture, spawnPosition, initialSourceRectangle, 0f);
        }

        public virtual void Update(GameTime gameTime)
        {
            GetInput(gameTime);
            _animationSet.UpdateAnimation(ref CollidableObject.SourceRectangle, ref CollidableObject.origin, gameTime);
        }
        
        private void GetInput(GameTime gameTime)
        {
            GetMovement(gameTime);
            GetActions();
        }

        private void GetMovement(GameTime gameTime)
        {
            // Update keyboard state
            _currentKeyboardState = Keyboard.GetState();
            // Reset displacement
            Vector2 displacement = Vector2.Zero;

            // Get input
            if (_currentKeyboardState.IsKeyDown(_controlScheme.MoveUp))
            {
                displacement.Y -= speed * gameTime.ElapsedGameTime.Milliseconds;
                //_animationSet.AnimationDirection = AnimationDirections.Up;
            }

            if (_currentKeyboardState.IsKeyDown(_controlScheme.MoveLeft))
            {
                displacement.X -= speed * gameTime.ElapsedGameTime.Milliseconds;
                //_animationSet.AnimationDirection = AnimationDirections.Left;
            }

            if (_currentKeyboardState.IsKeyDown(_controlScheme.MoveRight))
            {
                displacement.X += speed * gameTime.ElapsedGameTime.Milliseconds;
                //_animationSet.AnimationDirection = AnimationDirections.Right;
            }

            if (_currentKeyboardState.IsKeyDown(_controlScheme.MoveDown))
            {
                displacement.Y += speed * gameTime.ElapsedGameTime.Milliseconds;
                //_animationSet.AnimationDirection = AnimationDirections.Down;
            }

            // if nothing happened set _animationState to Idle and return.
            if (displacement == Vector2.Zero)
            {
                _animationSet.AnimationState = AnimationStates.Idle;
                return;
            }
            // if something happened set _animationState to Walk
            //_animationSet.AnimationState = AnimationStates.Walk;

            // Limit distance from other player
            // If the new distance is greater than MaxChainLength
            if (Vector2.Distance(CollidableObject.Position + displacement, InGame.GetOtherPlayerPosition(CollidableObject.Position)) > MaxChainLength)
            {
                // Don´t add displacement to position
                return; // TODO: make the remaining length used too
            }

            // Add displacement to position
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

        private void GetActions()
        {
            if (_currentKeyboardState.IsKeyDown(_controlScheme.Action1))
            {
                Action1();
                // Overwrite the previous animation state (Walk) with Action1
                //_animationSet.AnimationState = AnimationStates.Action1;
                // Return so the player does not make any other move
                return;
            }
            if (_currentKeyboardState.IsKeyDown(_controlScheme.Action2))
            {
                Action2();
                //_animationSet.AnimationState = AnimationStates.Action2;
                return;
            }
            if (_currentKeyboardState.IsKeyDown(_controlScheme.Action3))
            {
                Action3();
                //_animationSet.AnimationState = AnimationStates.Action3;
                return;
            }
            if (_currentKeyboardState.IsKeyDown(_controlScheme.Action4))
            {
                Action4();
                //_animationSet.AnimationState = AnimationStates.Action4;
                return;
            }
            // If no action and no movement was made AnimationState will remain Idle
        }

        protected virtual void Action1()
        {
        }

        protected virtual void Action2()
        {
        }

        protected virtual void Action3()
        {
        }

        protected virtual void Action4()
        {
        }

        /// <summary>
        /// Draw Player
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
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
