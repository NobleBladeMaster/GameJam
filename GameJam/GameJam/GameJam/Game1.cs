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
        CollidableObject collidableObject;
        Player player;
        ShotManager shotManager;
        SpriteManager spriteManager;
        Texture2D playerPic;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // asdlkdsalkjsadlkjsdajl


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

        public static Point ScreenBounds { get; } = new Point(1280, 720);

        public static GameStates GameState = GameStates.MainMenu;
        public static SpriteFont NormalMenuFont;
        public static SpriteFont BoldMenuFont;

        public static SpriteFont CreditsFont;
        public static SpriteFont BoldCreditsFont;
        public static SpriteFont CreditsTitleFont;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            player = new Player(Content.Load<Texture2D>(@"Textures/Avilda - Front"), 1, 42, 92, screenBounds);

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

            switch (GameState)
            {
                case GameStates.MainMenu:
                    MainMenu.Update();
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



            // TODO: Add your update logic here

            base.Update(gameTime);


            player.Update(gameTime);


            player.HandleSpriteMovement(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(pl)

            player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
