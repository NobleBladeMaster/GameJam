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

namespace GameJam
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        PlayerManager player;
        ShotManager shotManager;
        Sprite spriteManager;
        Texture2D playerPic;
        DynamicLight light;
        Texture2D Background;
        Texture2D BackgroundNormals;



        private List<Light> Lights = new List<Light>();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public enum GameStates : byte
        {
            MainMenu,
            InGame,
            HighScore,
            Tutorial,
            Credits,
            Exit,
            GameOver,
            ChooseDifficulty,
        }

        public static Point ScreenBounds { get; } = new Point(1280, 720);

        public static GameStates gameState = GameStates.MainMenu;
        public static SpriteFont NormalMenuFont;
        public static SpriteFont BoldMenuFont;

        public static SpriteFont CreditsFont;
        public static SpriteFont BoldCreditsFont;
        public static SpriteFont CreditsTitleFont;

        public delegate void FinalActionsDelegate();

        public static FinalActionsDelegate finalActionsDelegate = () => { Console.WriteLine("Started Delegate"); };

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            Rectangle screenBounds = new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);

            player = new PlayerManager(Content.Load<Texture2D>(@"Textures/Avilda - Front"), 1, 42, 92, screenBounds);

            NormalMenuFont = Content.Load<SpriteFont>(@"Fonts/NormalMenuFont");
            BoldMenuFont = Content.Load<SpriteFont>(@"Fonts/BoldMenuFont");

            CreditsFont = Content.Load<SpriteFont>(@"Fonts/CreditsFont");
            BoldCreditsFont = Content.Load<SpriteFont>(@"Fonts/BoldCreditsFont");
            CreditsTitleFont = Content.Load<SpriteFont>(@"Fonts/CreditsTitleFont");
            Background = Content.Load<Texture2D>(@"Background");
            BackgroundNormals = Content.Load<Texture2D>(@"Background Normal");


            Lights.Add(new PointLight()
            {
                IsEnabled = true,
                Color = new Vector4(2.0f, 2.0f, 2.0f, 2.0f),

                Power = 0.7f,
                LightDecay = 300,
                Position = new Vector3(100, 100, 80)

            });

            MainMenu.LoadContent(Content);
            InGame.LoadContent(Content);
            Tutorial.LoadContent(Content);
            Credits.LoadContent(Content);
            ChooseDifficulty.LoadContent(Content);
            HighScore.LoadContent(Content);
            GameOver.LoadContent(Content);

            gameState = GameStates.MainMenu;

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            UtilityClass.Update();

            switch (gameState)
            {
                case GameStates.MainMenu:
                    MainMenu.Update(gameTime);
                    break;
                case GameStates.InGame:
                    InGame.Update(gameTime);
                    break;
                case GameStates.Tutorial:
                    InGame.Update(gameTime);
                    break;
                case GameStates.Credits:
                    InGame.Update(gameTime);
                    break;
                case GameStates.Exit:
                    this.Exit();
                    break;
                case GameStates.ChooseDifficulty:
                    ChooseDifficulty.Update(gameTime);
                    break;
                case GameStates.HighScore:
                    HighScore.Update(gameTime);
                    break;
                case GameStates.GameOver:
                    GameOver.Update(gameTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (UtilityClass.SingleActivationKey(Keys.Escape))
            {
                LoadContent();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            switch (gameState)
            {
                case GameStates.MainMenu:
                    MainMenu.Draw(spriteBatch);
                    break;
                case GameStates.InGame:
                    InGame.Draw(spriteBatch);
                    break;
                case GameStates.Tutorial:
                    InGame.Draw(spriteBatch);
                    break;
                case GameStates.Credits:
                    InGame.Draw(spriteBatch);
                    break;
                case GameStates.Exit:
                    this.Exit();
                    break;
                case GameStates.ChooseDifficulty:
                    ChooseDifficulty.Draw(spriteBatch);
                    break;
                case GameStates.HighScore:
                    HighScore.Draw(spriteBatch);
                    break;
                case GameStates.GameOver:
                    GameOver.Draw(spriteBatch);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            if (!finalActionsDelegate.Equals(new FinalActionsDelegate(() => { })))
            {

                finalActionsDelegate();

                finalActionsDelegate = () => { };
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }

       
    }
}
