using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoulWarriors
{
    static class Main
    {
        private static Menu _menu;

        public static void LoadContent(ContentManager content, Viewport viewport)
        {
            _menu = new Menu(content.Load<Texture2D>(@"Textures/Menu/MainMenuBackground"),
                new Button[]
                {
                    new Button(new Point(0, 0),
                        new Rectangle(100, 200, 100, 100),
                        content.Load<Texture2D>(@"Textures/Menu/Button1"),
                        content.Load<Texture2D>(@"Textures/Menu/Menu0_button1"),
                        () => Game1.CurrentGameState = Game1.GameState.InGame),

                    new Button(new Point(1, 0),
                        new Rectangle(300, 200, 100, 100),
                        content.Load<Texture2D>(@"Textures/Menu/Button1"),
                        content.Load<Texture2D>(@"Textures/Menu/Menu0_button1"),
                        () => Game1.CurrentGameState = Game1.GameState.HighScore),

                    new Button(new Point(2, 0),
                        new Rectangle(500, 200, 100, 100),
                        content.Load<Texture2D>(@"Textures/Menu/Button1"),
                        content.Load<Texture2D>(@"Textures/Menu/Menu0_button1"),
                        () => Console.Beep()),

                    new Button(new Point(3, 0),
                        new Rectangle(700, 200, 100, 100),
                        content.Load<Texture2D>(@"Textures/Menu/Button1"),
                        content.Load<Texture2D>(@"Textures/Menu/Menu0_button1"),
                        () => Game1.CurrentGameState = Game1.GameState.Exit),
                },
                new MenuControlScheme(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Enter),
                viewport);
        }
         
        public static void Update()
        {
            _menu.Update();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            _menu.Draw(spriteBatch);
#if DEBUG
            spriteBatch.Begin();
            //spriteBatch.DrawString(Game1.DebugFont, $"{}");
            spriteBatch.End();
#endif
        }
    }
}
