using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SoulWarriors
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public enum GameState
        {
            InGame,
            MainMenu,
            HighScore,
            Exit
        }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

#if DEBUG
        public static SpriteFont DebugFont;
#endif

        public static GameState CurrentGameState = GameState.MainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsFixedTimeStep = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Set window size to 1920 * 1080
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();

            this.IsMouseVisible = true;
            
            HighScore.Initilize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            InGame.LoadContent(Content, GraphicsDevice.Viewport);
            Main.LoadContent(Content, GraphicsDevice.Viewport);
            HighScore.LoadContent(Content, GraphicsDevice.Viewport);
#if DEBUG
            DebugFont = Content.Load<SpriteFont>(@"Fonts/DebugFont");
#endif

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.End) || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //Fullscreen
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            switch (CurrentGameState)
            {
                case GameState.InGame:
                    if (IsMouseVisible == true)
                    {
                        IsMouseVisible = false;
                    }
                    InGame.Update(gameTime);

                    break;

                case GameState.MainMenu:
                    Main.Update();

                    break;

                case GameState.HighScore:
                    HighScore.Update();
                    break;

                case GameState.Exit:
                    this.Exit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (CurrentGameState)
            {
                case GameState.InGame:
                    InGame.Draw(spriteBatch);
                    break;
                case GameState.MainMenu:
                    Main.Draw(spriteBatch);
                    break;
                case GameState.Exit:
                    break;
                case GameState.HighScore:
                    HighScore.Draw(spriteBatch);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.Draw(gameTime);
        }
    }
}
