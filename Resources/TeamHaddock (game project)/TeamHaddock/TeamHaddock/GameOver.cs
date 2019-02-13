using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TeamHaddock
{
    // Created by Noble 12-11 
    internal static class GameOver
    {
        // Background for game over
        private static Texture2D background;

        private static string name;
        private static Vector2 namePosition = new Vector2(600, 550);
        private static int score;
        private static Vector2 scorePosition = new Vector2(340, 280);


        //Edited by Noble 12-11
        public static void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>(@"Textures/Backgrounds/GameOver");
        }

        public static void UpdatePlayerScore(string name, int score)
        {
            GameOver.name = name;
            GameOver.score = score;
        }

        //Edited by Noble 12-11
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Draw background
            spriteBatch.Draw(background, new Rectangle(0, 0, Game1.ScreenBounds.X, Game1.ScreenBounds.Y), Color.White);
            // Draw Name
            spriteBatch.DrawString(Game1.NormalMenuFont, name, namePosition, Color.White);
            // Draw Score
            spriteBatch.DrawString(Game1.NormalMenuFont, score.ToString(), scorePosition, Color.White);
            spriteBatch.End();
        }
    }
}
