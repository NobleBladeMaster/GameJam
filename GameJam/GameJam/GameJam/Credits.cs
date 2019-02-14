using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJam
{
    public static class Credits
    {

        private static Texture2D credits;

        public static void LoadContent(ContentManager content/*, GraphicsDevice graphicsDevice*/)
        {
            credits = content.Load<Texture2D>(@"Textures/TestBackgrounds/Credits");
        }

        public static void Update(GameTime gameTime)
        {
            if (UtilityClass.SingleActivationKey(Keys.Escape))
            {
                Game1.gameState = Game1.GameStates.MainMenu;
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {

            spriteBatch.Draw(credits, new Rectangle(0, 0, Game1.ScreenBounds.X, Game1.ScreenBounds.Y), Color.White);

            spriteBatch.DrawString(Game1.CreditsTitleFont, "Credits", new Vector2(50, 30), Color.Black);

            spriteBatch.DrawString(Game1.BoldCreditsFont, "Graphics", new Vector2(20, 180), Color.White);

            graphicsDevice.SetRenderTarget(null);

        }
    }
}
