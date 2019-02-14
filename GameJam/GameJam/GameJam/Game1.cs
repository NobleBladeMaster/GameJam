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
        Texture2D level1pic;
        Rectangle screenBounds;
       Vector2 level1pos = new Vector2(0, 0);
        
        //private PlayerManager playerSprite;


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

        public static Point ScreenBounds { get; } = new Point(1040, 620);

        public static GameStates gameState = GameStates.MainMenu;
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

            // Set window size to ScreenBounds
            graphics.PreferredBackBufferWidth = ScreenBounds.X;
            graphics.PreferredBackBufferHeight = ScreenBounds.Y;
            graphics.ApplyChanges();

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
            player = new PlayerManager(Content.Load<Texture2D>(@"Textures/Test"), 1, 40, 60, this.screenBounds);
            Rectangle screenBounds = new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            level1pic = Content.Load<Texture2D>(@"Textures/level1");
           
            //playerPic = Content.Load<Texture2D>(@"Textures/Avilda - Front");
            Console.WriteLine(player.Velocity);
           
            MainMenu.LoadContent(Content);
            InGame.LoadContent(Content);
            Tutorial.LoadContent(Content);
            Credits.LoadContent(Content);
            ChooseDifficulty.LoadContent(Content);
            HighScore.LoadContent(Content);
            GameOver.LoadContent(Content);

            player.Position = new Vector2(0,0);

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
            KeyboardState keyBoard = new KeyboardState();

            if (keyBoard.IsKeyDown(Keys.End))
            {
                this.Exit();
            }
            Console.WriteLine(player.Position);

            
            switch (gameState)
            {
                case GameStates.MainMenu:
                    MainMenu.Update(gameTime);
                    player.HandleSpriteMovement(gameTime);
                    player.Update(gameTime);

                    if (player.position.X > 10 && player.position.X < 137 && player.position.Y == 144)
                    {
                        player.position.Y -= player.walkingSpeed;


                    }

                    if (player.position.X > 10 && player.position.X < 137 && player.position.Y == 168)
                    {
                        player.position.Y += player.walkingSpeed;


                    }



                    if (player.position.X > 10 && player.position.X < 137 && player.position.Y == 342)
                    {
                        player.position.Y -= player.walkingSpeed;


                    }


                    if (player.position.X > 10 && player.position.X < 137 && player.position.Y == 373)
                    {
                        player.position.Y += player.walkingSpeed;


                    }

                    if (player.position.X > 184 && player.position.X < 482 && player.position.Y == 341)
                    {
                        player.position.Y -= player.walkingSpeed;


                    }

                    if (player.position.X > 184 && player.position.X < 482 && player.position.Y == 140)
                    {
                        player.position.Y -= player.walkingSpeed;


                    }

                    if (player.position.X > 184 && player.position.X < 482 && player.position.Y == 371)
                    {
                        player.position.Y += player.walkingSpeed;


                    }

                    if (player.position.X > 184 && player.position.X < 482 && player.position.Y == 160)
                    {
                        player.position.Y += player.walkingSpeed;


                    }

                    if (player.position.Y > 84 && player.position.Y < 422 && player.position.X == 312)
                    {
                        player.position.X -= player.walkingSpeed;
                    }


                    if (player.position.Y > 84 && player.position.Y < 422 && player.position.X == 346)
                    {
                        player.position.X += player.walkingSpeed;
                    }

                    if (player.position.Y > -15 && player.position.Y < 14 && player.position.X == 346)
                    {
                        player.position.X += player.walkingSpeed;
                    }

                    if (player.position.Y > -15 && player.position.Y < 14 && player.position.X == 312)
                    {
                        player.position.X -= player.walkingSpeed;
                    }


                    //player.AnimateLeft(gameTime);
                    // player.AnimateDown(gameTime);
                    // player.AnimateRight(gameTime);
                    // player.AnimateUp(gameTime);
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



            // TODO: Add your update logic here

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            
            spriteBatch.Draw(level1pic, level1pos, Color.White);
            spriteBatch.Draw(player.Texture, player.Position, player.SourceRect, Color.White);
            player.Draw(spriteBatch);




            //switch (gameState)
            //{
            //    case GameStates.MainMenu:
            //        MainMenu.Draw(spriteBatch);
            //        player.HandleSpriteMovement(gameTime);
            //        player.Update(gameTime);
            //        break;
            //    case GameStates.InGame:
            //        InGame.Draw(spriteBatch);
            //        break;
            //    case GameStates.Tutorial:
            //        InGame.Draw(spriteBatch);
            //        break;
            //    case GameStates.Credits:
            //        InGame.Draw(spriteBatch);
            //        break;
            //    case GameStates.Exit:
            //        this.Exit();
            //        break;
            //    case GameStates.ChooseDifficulty:
            //        ChooseDifficulty.Draw(spriteBatch);
            //        break;
            //    case GameStates.HighScore:
            //        HighScore.Draw(spriteBatch);
            //        break;
            //    case GameStates.GameOver:
            //        GameOver.Draw(spriteBatch);
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}



            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
