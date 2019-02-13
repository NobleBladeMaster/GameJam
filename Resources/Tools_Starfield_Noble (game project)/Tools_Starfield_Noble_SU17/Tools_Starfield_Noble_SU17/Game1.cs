using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tools_Starfield_Noble_SU17
{
    /// <summary>
    ///     This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private CollisionManager collisionManager;

        private EnemyManager enemyManager;

        private ExplosionManager explosionManager;

        private SpriteFont font;
        private readonly GraphicsDeviceManager graphics;

        private readonly Vector2 lifeDisplayPosition = new Vector2(10, 20);
        private Texture2D mixedSprites;

        private PlayerManager playerSprite;
        private SpriteBatch spriteBatch;

        private Starfield starField;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            mixedSprites = Content.Load<Texture2D>(@"Images/Mixed");

            var screenBounds = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);

            playerSprite = new PlayerManager(Content.Load<Texture2D>(@"Images/Mixed"), 1, 40, 60, screenBounds);

            enemyManager = new EnemyManager(mixedSprites, new Rectangle(0, 262, 40, 19), 1, playerSprite, screenBounds);

            playerSprite.Position = new Vector2(400, 300);

            starField = new Starfield(Window.ClientBounds.Width, Window.ClientBounds.Height, 200,
                new Vector2(100, 300f), mixedSprites, new Rectangle(0, 80, 2, 5));

            font = Content.Load<SpriteFont>(@"Fonts/font");

            explosionManager = new ExplosionManager(
                mixedSprites,
                new Rectangle(555, 134, 175, 74),
                3,
                new Rectangle(0, 450, 2, 2));

            collisionManager = new CollisionManager(playerSprite, explosionManager, enemyManager);
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Tryck på Back eller Esc för att stänga ner spelet
            var gamePad = GamePad.GetState(PlayerIndex.One);
            var keyboard = Keyboard.GetState();
            // Back eller escape quittar spelet
            if (gamePad.Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
                Exit();
            // Tryck på F för att gå Full-Screen
            if (keyboard.IsKeyDown(Keys.F))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            // TODO: Add your update logic here

            starField.Update(gameTime);
            playerSprite.HandleSpriteMovement(gameTime);
            playerSprite.Update(gameTime);
            enemyManager.Update(gameTime);
            explosionManager.Update(gameTime);
            collisionManager.CheckCollision();

            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            spriteBatch.Begin();
            starField.Draw(spriteBatch);
            playerSprite.Draw(spriteBatch);
            enemyManager.Draw(spriteBatch);
            explosionManager.Draw(spriteBatch);
            spriteBatch.Draw(playerSprite.Texture, playerSprite.Position, playerSprite.SourceRect, Color.White);
            spriteBatch.DrawString(font, playerSprite.livesRemaining.ToString(), lifeDisplayPosition, Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}