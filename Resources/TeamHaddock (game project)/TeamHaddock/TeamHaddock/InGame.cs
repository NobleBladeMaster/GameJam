using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// Class created by Alexander 11-07
namespace TeamHaddock
{
    /// <summary>
    /// Class for InGame GameState
    /// </summary>
    public static class InGame 
    {
        public static DynamicLight dynamicLight;

        public static Player player;

        private static Texture2D backgroundColorMap;
        private static Texture2D backgroundNormalMap;

        private static Texture2D groundColorMap;
        private static Texture2D groundNormalMap;
        public static Rectangle groundRectangle;

        private static int baseSpawnInterval = 3000;
        private static int timeSinceLastSpawn;
        private static Vector2 defaultSpawnPosition;
        private static Random random = new Random();

        public static int totalTimeElapsed;
        public static float difficultyModifier;

        public static List<IEnemy> enemies = new List<IEnemy>();
        private static List<LampPost> lampPosts = new List<LampPost>();

        //Edited by Noble 12-10, Alexander 12-11
        public static void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            dynamicLight = new DynamicLight();
            dynamicLight.LoadContent(content, graphicsDevice);

            player = new Player();
            player.LoadContent(content);

            backgroundColorMap = content.Load<Texture2D>(@"Textures/Backgrounds/InGameBackground");
            backgroundNormalMap = content.Load<Texture2D>(@"Textures/Backgrounds/InGameBackgroundNormalMap");

            groundColorMap = content.Load<Texture2D>(@"Textures/ActiveObjects/Ground");
            groundNormalMap = content.Load<Texture2D>(@"Textures/ActiveObjects/GroundNormalMap");
            groundRectangle = new Rectangle(0, Game1.ScreenBounds.Y - groundColorMap.Height, Game1.ScreenBounds.X, groundColorMap.Height);

            defaultSpawnPosition = new Vector2(Game1.ScreenBounds.X, groundRectangle.Top - 100);

            totalTimeElapsed = 0;
            difficultyModifier = 0;

            enemies.Clear();
            lampPosts.Clear();

            UserInterface.LoadContent(content);

            MeleeEnemy.LoadContent(content);
            CivilianEnemy.LoadContent(content);

            LampPost.LoadContent(content);

            lampPosts.Add(new LampPost(new Vector2(Game1.ScreenBounds.X - 250, groundRectangle.Top)));
            lampPosts.Add(new LampPost(new Vector2(250, groundRectangle.Top)));
        }

        public static void Update(GameTime gameTime)
        {
            totalTimeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            difficultyModifier = (totalTimeElapsed / 30000f)+1f; // double difficulty per 1 minute

            UpdateSpawning(gameTime);
            // Update player logic
            player.Update(gameTime);

            // Update enemy logic
            foreach (IEnemy enemy in enemies)
            {
                enemy.Update(gameTime);
            }
        }

        private static void UpdateSpawning(GameTime gameTime)
        {
            // Update timer
            timeSinceLastSpawn += gameTime.ElapsedGameTime.Milliseconds;
            // if timer has reached max
            if (timeSinceLastSpawn >= baseSpawnInterval / difficultyModifier)
            {
                // Spawn a new enemy
                if (random.Next(2) == 1)
                {
                    // %50 chance to spawn meleeEnemy
                    enemies.Add(new MeleeEnemy(random.Next(2) == 1 ? defaultSpawnPosition : new Vector2(0, defaultSpawnPosition.Y)));
                }
                else
                {
                    // %50 chance to spawn civilianEnemy
                    enemies.Add(random.Next(2) == 1 ? new CivilianEnemy(new Vector2(0, defaultSpawnPosition.Y + 200), true) : new CivilianEnemy(defaultSpawnPosition, false));
                }
                // Reset timer
                timeSinceLastSpawn = 0;
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            // Draw color map
            dynamicLight.DrawColorMap(graphicsDevice);
            DrawColorMap(spriteBatch);

            // Clear all render targets
            graphicsDevice.SetRenderTarget(null);

            // Draw normals
            dynamicLight.DrawNormalMap(graphicsDevice);
            DrawNormalMap(spriteBatch);

            // Clear all render targets
            graphicsDevice.SetRenderTarget(null);

            dynamicLight.GenerateShadowMap(graphicsDevice);

            graphicsDevice.Clear(Color.Black);

            dynamicLight.DrawCombinedMaps(spriteBatch);
            // Draw UI
            UserInterface.Draw(spriteBatch);
        }

        private static void DrawColorMap(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Draw Background
            spriteBatch.Draw(backgroundColorMap, new Rectangle(0, 0, Game1.ScreenBounds.X, Game1.ScreenBounds.Y), Color.White);
            // Draw Platforms and ground
            spriteBatch.Draw(groundColorMap, groundRectangle, Color.White);
            // Draw LampPosts
            foreach (LampPost lampPost in lampPosts)
            {
                lampPost.DrawColorMap(spriteBatch);
            }

            // Draw the enemies
            foreach (IEnemy enemy in enemies)
            {
                enemy.DrawColorMap(spriteBatch);
            }
            // Draw player
            player.DrawColorMap(spriteBatch);

            spriteBatch.End();
        }

        private static void DrawNormalMap(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw Background
            spriteBatch.Draw(backgroundNormalMap, new Rectangle(0, 0, Game1.ScreenBounds.X, Game1.ScreenBounds.Y), Color.White);
            // Draw Platforms and ground
            spriteBatch.Draw(groundNormalMap, groundRectangle, Color.White);
            // Draw LampPosts
            foreach (LampPost lampPost in lampPosts)
            {
                lampPost.DrawNormalMap(spriteBatch);
            }

            // Draw the enemies
            foreach (IEnemy enemy in enemies)
            {
                enemy.DrawNormalMap(spriteBatch);
            }
            // Draw player
            player.DrawNormalMap(spriteBatch);


            spriteBatch.End();
        }
    }
}
