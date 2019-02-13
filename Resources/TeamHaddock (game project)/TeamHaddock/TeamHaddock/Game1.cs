using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

// Class created by Alexander 11-07 // Edited by Noble 12-11
namespace TeamHaddock
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        /// <summary>
        /// List of menu states
        /// </summary>
        public enum GameStates : byte
        {
            MainMenu,
            InGame,
            HighScore,
            Tutorial, 
            Credits,
            Exit,
            GameOver, 
        }

        /// <summary>
        /// Current GameState
        /// </summary>
        public static GameStates GameState = GameStates.MainMenu;

        public static SpriteFont NormalMenuFont;
        public static SpriteFont BoldMenuFont;

        public static SpriteFont CreditsFont;
        public static SpriteFont BoldCreditsFont;
        public static SpriteFont CreditsTitleFont;

        /// <summary>
        /// Size of game window
        /// </summary>
        public static Point ScreenBounds { get; } = new Point(1280, 720);

        public delegate void FinalActionsDelegate();

        public static FinalActionsDelegate finalActionsDelegate = () => { Console.WriteLine("Started Delegate");};

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Set window size to ScreenBounds
            graphics.PreferredBackBufferWidth = ScreenBounds.X;
            graphics.PreferredBackBufferHeight = ScreenBounds.Y;
            graphics.ApplyChanges();

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


            NormalMenuFont = Content.Load<SpriteFont>(@"Fonts/NormalMenuFont");
            BoldMenuFont = Content.Load<SpriteFont>(@"Fonts/BoldMenuFont");

            CreditsFont = Content.Load<SpriteFont>(@"Fonts/CreditsFont");
            BoldCreditsFont = Content.Load<SpriteFont>(@"Fonts/BoldCreditsFont");
            CreditsTitleFont = Content.Load<SpriteFont>(@"Fonts/CreditsTitleFont");

            MainMenu.LoadContent(Content);
            InGame.LoadContent(Content, GraphicsDevice);
            Tutorial.LoadContent(Content, GraphicsDevice);
            HighScore.LoadContent(Content);
            Credits.LoadContent(Content, GraphicsDevice);
            GameOver.LoadContent(Content);

            GameState = GameStates.MainMenu;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }
        // Edited by Noble 12-10
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || UtilityClass.SingleActivationKey(Keys.End)) { this.Exit(); }
                
            UtilityClass.Update();

            switch (GameState)
            {
                case GameStates.MainMenu:
                    MainMenu.Update();
                    break;
                case GameStates.InGame:
                    InGame.Update(gameTime);
                    break;
                case GameStates.Tutorial:
                    Tutorial.Update(gameTime);
                    break;
                case GameStates.Credits:
                    Credits.Update(gameTime);
                    break;
                case GameStates.Exit:
                    this.Exit();
                    break;
                case GameStates.HighScore:
                    break;
                case GameStates.GameOver:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (UtilityClass.SingleActivationKey(Keys.Escape))
            {
                LoadContent();
            }

            base.Update(gameTime);
        }
        // Edited by Noble 12-10 
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (GameState)
            {
                case GameStates.MainMenu:
                    MainMenu.Draw(spriteBatch);
                    break;
                case GameStates.InGame:
                    InGame.Draw(spriteBatch, GraphicsDevice);
                    break;
                case GameStates.HighScore:
                    HighScore.Draw(spriteBatch);
                    break;
                case GameStates.Tutorial:
                    Tutorial.Draw(spriteBatch, GraphicsDevice);
                    break;
                case GameStates.Credits:
                    Credits.Draw(spriteBatch, GraphicsDevice);
                    break;
                case GameStates.GameOver:
                    GameOver.Draw(spriteBatch);
                    break;
                case GameStates.Exit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Run final actions
            if (!finalActionsDelegate.Equals(new FinalActionsDelegate(() => { })))
            {
                // Run delegate
                finalActionsDelegate();
                // Clear delegate
                finalActionsDelegate = () => { };
            }
            base.Draw(gameTime);
        }
    }
}
