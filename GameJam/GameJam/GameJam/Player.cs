﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// Class created by Alexander 11-07
namespace GameJam
{
    /// <summary>
    ///     Class responsible for Player movement, drawing etc
    /// </summary>
    public class Player
    {
        // Variables
        public int collisionRadius;
        private int currentFrame;

        private KeyboardState currentKBState;
        private KeyboardState previousKBState;


        public bool destroyed = false;

        private readonly Vector2 gunOffset = new Vector2(25, 10);
        private readonly float interval = 200f;
        public int livesRemaining = 3;
        private readonly float minShotTimer = 0.2f;
        public Texture2D playerPicture;

        public Vector2 playerPosition;
        private readonly int playerRadius = 15;
        public ShotManager PlayerShotManager;

        public float walkingSpeed = 1.0f;

        private Vector2 position;
        
        private Rectangle screenBounds;
        private float shotTimer;
        private readonly int spriteHeight = 60;
        private readonly int spriteWidth = 40;
        private float timer;

        public Player
        (Texture2D texture,
            int currentFrame,
            int spriteWidth,
            int spriteHeight,
            Rectangle screenBounds)
        {
            Texture = texture;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;

            this.screenBounds = screenBounds;

            collisionRadius = playerRadius;

            PlayerShotManager = new ShotManager(texture, new Rectangle(0, 80, 2, 2), 4, 2, 250f, screenBounds);
        }

        public Vector2 Center => position + new Vector2(spriteWidth / 2, spriteHeight / 2);

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Velocity { get; set; }

        public Texture2D Texture { get; set; }

        public Rectangle SourceRect { get; set; }

        public void PlayerCharacter(GameTime gameTime)
        {
            var playerCharacter = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, playerPicture.Width,
                playerPicture.Height);
        }

        private void FireShotRight()
        {
            if (shotTimer >= minShotTimer)
            {
                PlayerShotManager.FireShot(position + gunOffset, new Vector2(0, -1), true);
                shotTimer = 0.0f;
            }
        }

        public void HandleSpriteMovement(GameTime gameTime)
        {
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();
            SourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);

            if (currentKBState.GetPressedKeys().Length == 0)
            {
                if (currentFrame > 0 && currentFrame < 4) currentFrame = 0;
                if (currentFrame > 4 && currentFrame < 8) currentFrame = 4;
                if (currentFrame > 8 && currentFrame < 12) currentFrame = 8;
                if (currentFrame > 12 && currentFrame < 16) currentFrame = 12;
            }

            if (currentKBState.IsKeyDown(Keys.X) && currentFrame >= 12 && currentFrame < 16) FireShotRight();


            if (UtilityClass.SingleActivationKey(Keys.Up))
            {
                AnimateUp(gameTime);
                position.Y -= walkingSpeed;
            }

            if (UtilityClass.SingleActivationKey(Keys.Down))        
            {
                AnimateDown(gameTime);
                position.Y += walkingSpeed;
            }

            if (UtilityClass.SingleActivationKey(Keys.Left))
            {
                AnimateLeft(gameTime);
                position.X -= walkingSpeed;
            }

            if (UtilityClass.SingleActivationKey(Keys.Right))
            {
                AnimateRight(gameTime);
                position.X += walkingSpeed;
            }
            
            Velocity = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);
        }

        public void AnimateUp(GameTime gameTime)
        {
            if (currentKBState != previousKBState) currentFrame = 13;
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame > 15) currentFrame = 12;
                timer = 0f;
            }
        }

        public void AnimateDown(GameTime gameTime)
        {
            if (currentKBState != previousKBState) currentFrame = 1;
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame > 3) currentFrame = 0;
                timer = 0f;
            }
        }

        public void AnimateLeft(GameTime gameTime)
        {
            if (currentKBState != previousKBState) currentFrame = 5;
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame > 7) currentFrame = 4;
                timer = 0f;
            }
        }

        public void AnimateRight(GameTime gameTime)
        {
            if (currentKBState != previousKBState) currentFrame = 9;

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame > 11) currentFrame = 8;
                timer = 0f;
            }
        }

        // Update
        public void Update(GameTime gameTime)
        {
            PlayerShotManager.Update(gameTime);
            if (!destroyed) shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerShotManager.Draw(spriteBatch);
        }
    }
}