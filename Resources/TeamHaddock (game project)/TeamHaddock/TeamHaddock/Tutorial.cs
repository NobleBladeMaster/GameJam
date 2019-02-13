using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TeamHaddock
{
    // Class created by Noble 12-11 
    internal static class Tutorial
    {

        
        private static Texture2D backGround;
       
        //Edited by Noble 12-11
        public static void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            backGround = content.Load<Texture2D>(@"Textures/Backgrounds/Tutorial");
        }

        //Edited by Noble 12-11
        public static void Update(GameTime gameTime)
        {
            if (UtilityClass.SingleActivationKey(Keys.Escape))
            {
                Game1.GameState = Game1.GameStates.MainMenu; 
            }
        }

        //Edited by Noble 12-11
        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            // Draw
            spriteBatch.Begin();
            spriteBatch.Draw(backGround, new Rectangle(0, 0, Game1.ScreenBounds.X, Game1.ScreenBounds.Y), Color.White);
            spriteBatch.End();
            // Clear all render targets
            graphicsDevice.SetRenderTarget(null);

        }
    }
}