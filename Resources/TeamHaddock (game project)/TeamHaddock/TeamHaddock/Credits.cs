using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Class created by Alexander 11-07 // Class edited by Adam 12-10 // Class edited by Noble 12-11
namespace TeamHaddock
{
    internal static class Credits
    {

        private static Texture2D backGround;

        private static float creditsTime;

        // Edited by Noble 12-11 
        public static void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            backGround = content.Load<Texture2D>(@"Textures/Backgrounds/Credits");
        }
        
        // Edited by Noble 12-11 
        public static void Update(GameTime gameTime)
        {
            if (UtilityClass.SingleActivationKey(Keys.Escape))
            {
                Game1.GameState = Game1.GameStates.MainMenu; 
            }

            creditsTime += gameTime.ElapsedGameTime.Milliseconds / 2; 
        }
        
        // Edited by Noble 12-11 
        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Begin();

            //spriteBatch.Draw(creditsBackground, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(backGround, new Rectangle(0, 0, Game1.ScreenBounds.X, Game1.ScreenBounds.Y), Color.White);


            spriteBatch.DrawString(Game1.CreditsTitleFont, "Credits", new Vector2(50, 30), Color.White);



            spriteBatch.DrawString(Game1.CreditsFont, "Loading", new Vector2(20, 120), Color.White);


            if (creditsTime > 200)
            {
                spriteBatch.DrawString(Game1.CreditsFont, ".", new Vector2(140, 120), Color.White);
            }

            if (creditsTime > 500)
            {
                spriteBatch.DrawString(Game1.CreditsFont, ".", new Vector2(155, 120), Color.White);
            }

            if (creditsTime > 800)
            {
                spriteBatch.DrawString(Game1.CreditsFont, ".", new Vector2(170, 120), Color.White);
            }

            if (creditsTime > 1100)
            {
                spriteBatch.DrawString(Game1.CreditsFont, ".", new Vector2(185, 120), Color.White);
            }

            if (creditsTime > 1600)
            {
                spriteBatch.DrawString(Game1.BoldCreditsFont, "Graphics", new Vector2(20, 180), Color.White);
            }

            if (creditsTime > 1800)
            {
                spriteBatch.DrawString(Game1.CreditsFont, "Adam ", new Vector2(20, 230), Color.White);
            }

            if (creditsTime > 2000)
            {
                spriteBatch.DrawString(Game1.CreditsFont, "Alexander ", new Vector2(20, 270), Color.White);
            }

            if (creditsTime > 2200)
            {
                spriteBatch.DrawString(Game1.CreditsFont, "Noble ", new Vector2(20, 310), Color.White);
            }

            if (creditsTime > 2400)
            {
                spriteBatch.DrawString(Game1.CreditsFont, "Elias ", new Vector2(20, 350), Color.White);
            }

            if (creditsTime > 2600)
            {
                spriteBatch.DrawString(Game1.BoldCreditsFont, "Code", new Vector2(20, 390), Color.White);
            }

            if (creditsTime > 2800)
            {
                spriteBatch.DrawString(Game1.CreditsFont, "Adam ", new Vector2(20, 430), Color.White);
            }

            if (creditsTime > 3000)
            {
                spriteBatch.DrawString(Game1.CreditsFont, "Alexander ", new Vector2(20, 470), Color.White);
            }

            if (creditsTime > 3200)
            {
                spriteBatch.DrawString(Game1.CreditsFont, "Elias ", new Vector2(20, 510), Color.White);
            }

            if (creditsTime > 3400)
            {
                spriteBatch.DrawString(Game1.CreditsFont, "Noble ", new Vector2(20, 550), Color.White);
            }

            if (creditsTime > 3600)
            {
                spriteBatch.DrawString(Game1.BoldCreditsFont, "Project Manager ", new Vector2(400, 180), Color.White);
            }

            if (creditsTime > 3800)
            {
                spriteBatch.DrawString(Game1.CreditsFont, "Noble ", new Vector2(400, 240), Color.White);
            }

            if (creditsTime > 4000)
            {
                spriteBatch.DrawString(Game1.BoldCreditsFont, "Game Designer ", new Vector2(400, 390), Color.White);
            }

            if (creditsTime > 4200)
            {
                spriteBatch.DrawString(Game1.CreditsFont, "Noble ", new Vector2(400, 450), Color.White);
            }

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
        }
    
    }
}
