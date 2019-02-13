using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tools_Starfield_Noble_SU17
{

    public class PlayerManager
    {
        public int collisionRadius = 0;
        public bool destroyed = false;
        public int livesRemaining = 3;
        private int playerRadius = 15; 

        Texture2D playerSprite;
        float timer = 0f;
        float interval = 200f;
        int currentFrame = 0;
        int spriteWidth = 40;
        int spriteHeight = 60;
        int spriteSpeed = 2;
        Rectangle sourceRect;
        Vector2 position;
        Vector2 velocity;

        private Vector2 gunOffset = new Vector2(25, 10);
        private float shotTimer = 0.0f;
        private float minShotTimer = 0.2f;
        public ShotManager PlayerShotManager;
        Rectangle screenBounds;

        public Vector2 Center
        {
            get
            {
                return position + new Vector2(spriteWidth / 2, spriteHeight / 2);
            }
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

        public Texture2D Texture
        {
            get { return playerSprite; }
            set { playerSprite = value; }
        }

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public PlayerManager(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight, Rectangle screenBounds)
        {
            this.playerSprite = texture;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;

            this.screenBounds = screenBounds;

            collisionRadius = playerRadius;

            PlayerShotManager = new ShotManager(texture, new Rectangle(0, 80, 2, 2), 4, 2, 250f, screenBounds);
        }

        public PlayerManager(int livesRemaining)
        {
            this.livesRemaining = livesRemaining;
        }

        // Keyboard kontroll
        KeyboardState currentKBState;
        KeyboardState previousKBState;

        private void FireShotDown()
        {
            if (shotTimer >= minShotTimer)
            {
                PlayerShotManager.FireShot(position + gunOffset, new Vector2(0, 1), true);
                shotTimer = 0.0f;
            }
        }

        private void FireShotLeft()
        {
            if (shotTimer >= minShotTimer)
            {
                PlayerShotManager.FireShot(position + gunOffset, new Vector2(-1, 0), true);
                shotTimer = 0.0f;
            }
        }

        private void FireShotRight()
        {
            if (shotTimer >= minShotTimer)
            {
                PlayerShotManager.FireShot(position + gunOffset, new Vector2(0, -1), true);
                shotTimer = 0.0f;
            }
        }

        private void FireShotUp()
        {
            if (shotTimer >= minShotTimer)
            {
                PlayerShotManager.FireShot(position + gunOffset, new Vector2(1, 0), true);
                shotTimer = 0.0f;
            }
        }

        public void HandleSpriteMovement(GameTime gameTime)
        {
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);

            if (currentKBState.GetPressedKeys().Length == 0)
            {
                if (currentFrame > 0 && currentFrame < 4)
                {
                    currentFrame = 0;
                }
                if (currentFrame > 4 && currentFrame < 8)
                {
                    currentFrame = 4;
                }
                if (currentFrame > 8 && currentFrame < 12)
                {
                    currentFrame = 8;
                }
                if (currentFrame > 12 && currentFrame < 16)
                {
                    currentFrame = 12;
                }
            }

            if (currentKBState.IsKeyDown(Keys.Space) && currentFrame >= 0 && currentFrame < 4)
            {
                FireShotDown();
            }

            if (currentKBState.IsKeyDown(Keys.Space) && currentFrame >= 4 && currentFrame < 8)
            {
                FireShotLeft();
            }

            if (currentKBState.IsKeyDown(Keys.Space) && currentFrame >= 8 && currentFrame < 12)
            {
                FireShotUp();
            }

            if (currentKBState.IsKeyDown(Keys.Space) && currentFrame >= 12 && currentFrame < 16)
            {
                FireShotRight();
            }

            if (currentKBState.IsKeyDown(Keys.Right) == true)
            {
                AnimateRight(gameTime);
                if (position.X < 780)
                {
                    position.X += spriteSpeed;
                }
            }

            if (currentKBState.IsKeyDown(Keys.Left) == true)
            {
                AnimateLeft(gameTime);
                if (position.X > 20)
                {
                    position.X -= spriteSpeed;
                }
            }

            if (currentKBState.IsKeyDown(Keys.Down) == true)
            {
                AnimateDown(gameTime);
                if (position.Y < 575)
                {
                    position.Y += spriteSpeed;
                }
            }

            if (currentKBState.IsKeyDown(Keys.Up) == true)
            {
                AnimateUp(gameTime);
                if (position.Y > 25)
                {
                    position.Y -= spriteSpeed;
                }
            }

            velocity = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }

        public void AnimateRight(GameTime gameTime)
        {
            if (currentKBState != previousKBState)
            {
                currentFrame = 9;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 11)
                {
                    currentFrame = 8;
                }
                timer = 0f;
            }
        }

        public void AnimateUp(GameTime gameTime)
        {
            if (currentKBState != previousKBState)
            {
                currentFrame = 13;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 15)
                {
                    currentFrame = 12;
                }
                timer = 0f;
            }
        }

        public void AnimateDown(GameTime gameTime)
        {
            if (currentKBState != previousKBState)
            {
                currentFrame = 1;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 3)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void AnimateLeft(GameTime gameTime)
        {
            if (currentKBState != previousKBState)
            {
                currentFrame = 5;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame > 7)
                {
                    currentFrame = 4;
                }
                timer = 0f;
            }
        }

        // Update
        public void Update(GameTime gameTime)
        {
            PlayerShotManager.Update(gameTime);
            if (!destroyed)
            {
                shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerShotManager.Draw(spriteBatch);
        }
    }
}


