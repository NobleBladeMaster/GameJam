using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// Class created by Alexander 11-07
namespace GameJam
{


        public static class InGame
    {
        private static Texture2D level1;

        public static void LoadContent(ContentManager content)
        {

            level1 = content.Load<Texture2D>(@"Textures/TestBackgrounds/Level1");


        }

        public static void Update(GameTime gameTime)
        {
          
        }

        public static void Draw(SpriteBatch spriteBatch)
        {


            //spriteBatch.Draw(level1, new Rectangle(0, 0, Game1.ScreenBounds.X, Game1.ScreenBounds.Y), Color.White);


        }
       
    

    

        
    }
}
