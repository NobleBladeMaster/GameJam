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

namespace Tools_3D_FreeCamera
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// Creates a list of the CustomModel type that is used to add models to a list and draw every box according to each property for each box in the list 
        /// </summary>
        List<CustomModel> models = new List<CustomModel>();

        /// <summary>
        /// Creates a new instance of the camera class 
        /// </summary>
        private Camera camera;

        /// <summary>
        /// The mouse that is used to move the camera around in the world matrix 
        /// </summary>
        private MouseState LastMouseState;

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

            // Adds instances of a model object that contains a 3D model, position, rotation, scale and rendering for the object 
            models.Add(new CustomModel(Content.Load<Model>("Models/test"), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(100f), GraphicsDevice));
            models.Add(new CustomModel(Content.Load<Model>("Models/Ground"), Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice));

            // Creates a new free camera with no lock on with a position, amount of yaw (y) rotation, amount of pitch (x) rotation, and rendering for the camera
            camera = new FreeCamera(new Vector3(100, 1000, -2000), MathHelper.ToRadians(0), MathHelper.ToRadians(0), GraphicsDevice);

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
            // Creates a KeyboardState which is used to get information regarding the current inputs by the keyboard
            KeyboardState keyBoard = Keyboard.GetState();

            // Creates a MouseState which is used to get information regarding the current inputs by the mouse 
            MouseState lastMouseState = Mouse.GetState();

            // Pressing "End" closes the solution down
            if (keyBoard.IsKeyDown(Keys.End))
            {
                this.Exit();
            }

            // Pressing "F" makes the window go full screen
            if (keyBoard.IsKeyDown(Keys.F))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            // Updates the camera via game time 
            UpdateCamera(gameTime);


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        void UpdateCamera(GameTime gameTime)
        {
            // Creates a mouse and keyboard state to for updating the controls 
            MouseState currentMouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            // Creates a delta value for the mouse movement in the X and Y axis that equals the last mouse state minus the current mouse state
            float deltaX = (float)LastMouseState.X - (float)currentMouseState.X;
            float deltaY = (float)LastMouseState.Y - (float)currentMouseState.Y;

            // Makes the arc ball camera rotate by the delta mouse values in the X and Y axis, and a set value in the Z axis 
            ((FreeCamera)camera).Rotate(deltaX * .01f, deltaY * .01f);

            // Creates a camera position translation that changes the cameras coordinates
            Vector3 translation = Vector3.Zero;

            // Creates a delta value for the scroll wheel that equals the last mouse state minus the current mouse state 
            float scrollDelta = (float)LastMouseState.ScrollWheelValue - (float)currentMouseState.ScrollWheelValue;

            // Moves the camera up, down, left or right depending on the button pressed
            if (keyState.IsKeyDown(Keys.Up)) translation += Vector3.Forward;
            if (keyState.IsKeyDown(Keys.Down)) translation += Vector3.Backward;
            if (keyState.IsKeyDown(Keys.Left)) translation += Vector3.Left;
            if (keyState.IsKeyDown(Keys.Right)) translation += Vector3.Right;

            // Sets the translation speed according to a base value times the amount of updates in milliseconds 
            translation *= 3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds; 

            // Makes the arc ball camera move by the delta scroll wheel value 
            ((FreeCamera)camera).Move(translation);

            // Updates the camera 
            camera.Update();

            // Sets the last mouse state equals the current mouse state 
            LastMouseState = currentMouseState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Sets the color of the background to black 
            GraphicsDevice.Clear(Color.Black);

            // For every model in the models list 
            foreach (CustomModel model in models)
            {
                // Draw each model in the view and projection matrix 
                model.Draw(camera.View, camera.Projection);
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
