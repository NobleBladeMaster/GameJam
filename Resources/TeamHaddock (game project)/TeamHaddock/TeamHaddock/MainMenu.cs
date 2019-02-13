using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// Class created by Alexander 11-07 // Edited by Noble 12-11
namespace TeamHaddock
{
    /// <summary>
    ///     Draws and does logic for the gameState mainMenu
    /// </summary>
    internal static class MainMenu
    {

        /// <summary>
        ///     String array of menu option names
        /// </summary>
        private static readonly string[] MenuOptionsStr = {"Play", "Tutorial", "High Score", "Credits", "Exit"};

        /// <summary>
        ///     selected menu option
        /// </summary>
        private static Vector2 selected;

        /// <summary>
        ///     MainMenu background image
        /// </summary>
        private static Texture2D background;

        /// <summary>
        ///     Controls keyboard actions in menus
        /// </summary>
        private static readonly MenuControls menuControl = new MenuControls(new Vector2(MenuOptionsStr.Length - 1, 0));

        /// <summary>
        /// Load MainMenu Textures, e.g the background
        /// </summary>
        /// <param name="content"></param>
        public static void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>(@"Textures/Backgrounds/MainMenuBackground");
        }

        // Edited by Noble 12-10 
        /// <summary>
        ///     Updates MainMenu gamestate logic
        /// </summary>
        public static void Update()
        {
            menuControl.UpdateSelected(ref selected); // Updates selected menu option

            // If enter is not pressed Then return
            if (!menuControl.IsEnterDown) return;
      
            // Else (enter is pressed) Then change gamestate and playstate
            switch ((int)selected.X)
            {
                // Play
                case 0:
                    Game1.GameState = Game1.GameStates.InGame;
                    break;
                // Tutorial
                case 1:
                    Game1.GameState = Game1.GameStates.Tutorial; 
                    break;
                // HighScore 
                case 2:
                    Game1.GameState = Game1.GameStates.HighScore;
                    break;
                // Credits
                case 3:
                    Game1.GameState = Game1.GameStates.Credits;
                    break;
                // Exit
                case 4:
                    Game1.GameState = Game1.GameStates.Exit;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        // Edited by Noble 12-10, 12-11
        /// <summary>
        ///     Draws the MainMenu gamestate
        /// </summary>
        /// <param name="spriteBatch">Enables a group of sprites to be drawn using the same settings.</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw background in whole window
            spriteBatch.Draw(background, new Rectangle(0, 0, Game1.ScreenBounds.X, Game1.ScreenBounds.Y), Color.White);

            // Iterate through every entry in menuOptionsStr array
            for (int i = 0; i < MenuOptionsStr.Length; i++)
            {
                // If selected menu option is int i have bold font else normal font
                spriteBatch.DrawString((int)selected.X == i ? Game1.BoldMenuFont : Game1.NormalMenuFont, MenuOptionsStr[i], new Vector2(200 + 200 * i, 400), Color.White);
            }

            spriteBatch.End();
        }
    }
}