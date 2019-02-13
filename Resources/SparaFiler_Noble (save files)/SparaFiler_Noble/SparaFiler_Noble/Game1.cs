#region Using 
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
using System.Xml.Serialization;
using System.IO;
#endregion
namespace SparaFiler_Noble
{
    #region Summary Game1
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
#endregion
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // This names the filename
        public readonly string Filename = "saveFile.dat";

        // These are the names used to save the score to 
        String[] names = new string[] { "Kalle", "Olaf", "Kerstin" };

        // These are static names used in draw
        string kalle = "Kalle";
        string olaf = "Olaf";
        string kerstin = "Kerstin";
        
        // These are the name positions in the main menu 
        Vector2 kalleNamePos = new Vector2(170, 200);
        Vector2 olafNamePos = new Vector2(345, 200);
        Vector2 kerstinNamePos = new Vector2(520, 200);

        // These are the score positions for each correlating name
        Vector2 kalleScorePos;
        Vector2 olafScorePos; 
        Vector2 kerstinScorePos;

        // These are the names and their position in the High-Score list
        Vector2 highScoreP1 = new Vector2(275, 150);
        Vector2 highScoreP2;
        Vector2 highScoreP3;

        // These are the scores correlating to each name in the High-Score list
        Vector2 highScoreP1Score;
        Vector2 highScoreP2Score;
        Vector2 highScoreP3Score;

        // This is used later on when I define the position of the highScoreP1 to have everything be one variable instead of individual assigned numbers
        int highScorePlayerDistance = 150; 

        // These are all the helper-texts to make it easier to know what to do
        string leftOrRightMessage = "Press Left or Right to change the selected name";
        string upOrDownMessage = "Press Up or Down to change the score of the selected name";
        string escAndFullscreenMessage = "Press ESC to exit the program, Press F to go Fullscreen"; 
        string menuChangingMessage = "Press H to go to the High-Score list, and press M to go back to the menu";
        string savingMessage = "Press S to save the score, remember to save on a name before you change the name";
        string resetMessage = "Press R to reset the High-Score list";

        // This is used to show how much score you saved to a person 
        string savedPerson1 = "You have saved ";
        string savedPerson2 = " score to ";

        // These are used to define where the message on how much score you saved and on which person you saved it on is
        bool kalleIsTrue = false;
        Vector2 kalleBoolPos = new Vector2(0, 420);

        bool olafIsTrue = false;
        Vector2 olafBoolPos = new Vector2(0, 440);

        bool kerstinIsTrue = false;
        Vector2 kerstinBoolPos = new Vector2 (0, 460); 

        // These are used to define the positions of all the helper-texts
        Vector2 helpfulMessagePos1 = new Vector2(0, 5);
        Vector2 helpfulMessagePos2;
        Vector2 helpfulMessagePos3;
        Vector2 helpfulMessagePos4;
        Vector2 helpfulMessagePos5;
        Vector2 helpfulMessagePos6;

        // This is used later on when I define the positions of the helpful messages to have everything be one variable instead of individual assigned numbers
        int helpfulMessagePosDistance = 20;

        // These are the 3 different fonts I use in spriteBatch.drawString
        SpriteFont font;
        SpriteFont smallFont;
        SpriteFont largeFont;

        // This is so I can make a random number if the need for it ever arises 
        Random numberGen = new Random();

        // This is used to save the score of the highlighted names you press --> S <-- on 
        int playerScore = 0;

        // These are used to separate the scores so that when I save a highlighted names score, it doesn't save on all of the other names
        int player1Score = 0;
        int player2Score = 0;
        int player3Score = 0;

        // This is used to help differentiate between which name is assigned and which name is not
        int playerCharacter = 0;

        // This is used to not make everything go every tick, instead it is a set delay in milliseconds 
        int delay = 0; 

        // This is used to change between the different gameStates
        enum GameState { mainMenu, highScoreList };

        // This says that the starting gameState is the main menu
        GameState gameState = GameState.mainMenu;

        // !!! Attention !!! Down here where I have written down what you told me to write down on paper, I am guessing what I believe these different lines of code do 

        // This helps save the two variables PlayerName and Score to the file 
        [Serializable]
        public struct SaveData
        {
            public string[] PlayerName;
            public int[] Score;

            public int Count;

            public SaveData(int count)
            {
                PlayerName = new string[count];
                Score = new int[count];

                Count = count;
            }
        }

        // This loads the data of the "Filename" file 
        public static SaveData LoadData(string Filename)
        {
            SaveData data;

            string fullpath = Filename;

            FileStream stream = File.Open(fullpath, FileMode.Open,
                FileAccess.Read);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                data = (SaveData)serializer.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }

            return (data);
        }

        // This saves the data and filename to the file
        public static void DoSave(SaveData data, String filename)
        {
            FileStream stream = File.Open(filename, FileMode.Create);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                stream.Close();
            }
        }

        // This saves the score and name to the file 
        private void SaveHighScore()
        {
            SaveData data = LoadData(Filename);

            int scoreIndex = -1;

            for (int x = 0; x < data.Count; x++)
            {
                if (playerScore > data.Score[x])
                {
                    scoreIndex = x;
                    break;
                }
            }

            if (scoreIndex > -1)
            {
                for (int x = data.Count - 1; x > scoreIndex; x--)
                {
                    data.Score[x] = data.Score[x - 1];
                }


                data.Score[scoreIndex] = playerScore;

                if (playerCharacter == 0)
                {
                    data.PlayerName[scoreIndex] = names[0];
                }
                if (playerCharacter == 1)
                {
                    data.PlayerName[scoreIndex] = names[1];
                }
                if (playerCharacter == 2)
                {
                    data.PlayerName[scoreIndex] = names[2];
                }
                    

                DoSave(data, Filename);
            }
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #region Summary Initialize
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 
#endregion 
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // This says that if the file does not exist, create a new file with these requirements that are listed down below
            if (!File.Exists(Filename))
            {
                SaveData data = new SaveData(3);
                data.PlayerName[0] = names[0];
                data.PlayerName[1] = names[1];
                data.PlayerName[2] = names[2];

                data.Score[0] = 0;
                data.Score[1] = 0;
                data.Score[2] = 0;

                DoSave(data, Filename);
            }

            base.Initialize();
        }
        #region Summary LoadContent
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 
#endregion
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // These are all the different fonts I use in spriteBatch.drawString
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // These are all the different fonts I use in spriteBatch.drawString
            font = Content.Load<SpriteFont>(@"SpriteFont1");

            smallFont = Content.Load<SpriteFont>(@"SpriteFont2");

            largeFont = Content.Load<SpriteFont>(@"SpriteFont3");

            // TODO: use this.Content to load your game content here
        }
        #region Summary UnloadContent
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        /// 
#endregion
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #region Summary Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
#endregion
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            // Defines gamePad and keyboard
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();

            // All of these values the different positions to be based upon an original positions X any Y

            // The main menu score positions 
            kalleScorePos.Y = kalleNamePos.Y + 50;
            olafScorePos.Y = olafNamePos.Y + 50;
            kerstinScorePos.Y = kerstinNamePos.Y + 50;

            kalleScorePos.X = kalleNamePos.X + 1;
            olafScorePos.X = olafNamePos.X + 1;
            kerstinScorePos.X = kerstinNamePos.X + 1;

            // These are the high-score list name positions
            highScoreP2.X = highScoreP1.X + 1;
            highScoreP2.Y = highScoreP1.Y + 100;
            
            highScoreP3.X = highScoreP2.X + 1;
            highScoreP3.Y = highScoreP2.Y + 100;

            // These are the high-score list score positions 
            highScoreP1Score.X = highScoreP1.X + highScorePlayerDistance;
            highScoreP1Score.Y = highScoreP1.Y;

            highScoreP2Score.X = highScoreP2.X + highScorePlayerDistance;
            highScoreP2Score.Y = highScoreP2.Y;

            highScoreP3Score.X = highScoreP3.X + highScorePlayerDistance;
            highScoreP3Score.Y = highScoreP3.Y;

            // These are the instruction messages in the main menu positions
            helpfulMessagePos2.X = helpfulMessagePos1.X;
            helpfulMessagePos2.Y = helpfulMessagePos1.Y + helpfulMessagePosDistance;

            helpfulMessagePos3.X = helpfulMessagePos2.X;
            helpfulMessagePos3.Y = helpfulMessagePos2.Y + helpfulMessagePosDistance;

            helpfulMessagePos4.X = helpfulMessagePos3.X;
            helpfulMessagePos4.Y = helpfulMessagePos3.Y + helpfulMessagePosDistance;

            helpfulMessagePos5.X = helpfulMessagePos4.X;
            helpfulMessagePos5.Y = helpfulMessagePos4.Y + helpfulMessagePosDistance;

            helpfulMessagePos6.X = helpfulMessagePos5.X;
            helpfulMessagePos6.Y = helpfulMessagePos5.Y + helpfulMessagePosDistance;

            


            // Press escape or the back button to close down the game
            if (gamePad.Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }        

            // Press f to go full-screen mode
            if (keyboard.IsKeyDown(Keys.F))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            // This is a switch that is used to help with the gameStates 
            switch (gameState)
            {
                case GameState.mainMenu:

                    SaveData data = new SaveData(1);
                    
                    // This is used to make everything not go every tick, as explained above
                    delay -= gameTime.ElapsedGameTime.Milliseconds;

                    // This is used to make the left to right scrolling system be able to go to the other side 
                    if (playerCharacter <= -1)
                    {
                        playerCharacter = 2;
                    }

                    if (playerCharacter >= 3)
                    {
                        playerCharacter = 0;
                    }

                    // The left to right scrolling system to change between the chosen names 
                    if (keyboard.IsKeyDown(Keys.Left) && delay <= 0)
                    {
                        playerCharacter--;
                        delay = 200; 
                    }

                    if (keyboard.IsKeyDown(Keys.Right) && delay <= 0)
                    {
                        playerCharacter++;
                        delay = 200; 
                    }

                    // This is used to change the score of the selected names. Up makes the value go up, and down makes the value go down
                    if (keyboard.IsKeyDown(Keys.Up) && delay <= 0)
                    {                   
                        delay = 10; 
                        if (playerCharacter == 0)
                        {
                            player1Score++;
                        }
                        if (playerCharacter == 1)
                        {
                            player2Score++;
                        }
                        if (playerCharacter == 2)
                        {
                            player3Score++;
                        }
                    }
                    if (keyboard.IsKeyDown(Keys.Down) && delay <= 0)
                    {
                      

                        delay = 10; 
                        if (playerCharacter == 0)
                        {
                            player1Score--;
                        }
                        if (playerCharacter == 1)
                        {
                            player2Score--;
                        }
                        if (playerCharacter == 2)
                        {
                            player3Score--;
                        }
                    }

                    // Changes the menu to the high-score list, with other things done
                    if (keyboard.IsKeyDown(Keys.H) && delay <= 0)
                    {
                        gameState = GameState.highScoreList;          
                        
                        delay = 250;
                    }

                    // Saves the score of the chosen name 
                    if (keyboard.IsKeyDown(Keys.S) && delay <= 0)
                    {
                        if (playerCharacter == 0)
                        {
                            playerScore = player1Score;
                            SaveHighScore();
                            delay = 250;
                            kalleIsTrue = true;
                        }
                        if (playerCharacter == 1)
                        {
                            playerScore = player2Score;
                            SaveHighScore();
                            delay = 250;
                            olafIsTrue = true; 
                        }
                        if (playerCharacter == 2)
                        {
                            playerScore = player3Score;
                            SaveHighScore();
                            delay = 250;
                            kerstinIsTrue = true; 
                        }
                    }
                    break;
                case GameState.highScoreList:

                    delay -= gameTime.ElapsedGameTime.Milliseconds;

                    playerCharacter = 0;

                    // This says "if you press M, go to the main menu gameState" 
                    if (keyboard.IsKeyDown(Keys.M) && delay <= 0)
                    {
                        gameState = GameState.mainMenu;

                        delay = 250; 
                    }          
                    break;                
            }

            // This is used to reset the file so that you don't have to delete the file itself to reset the high-score list 
            if (keyboard.IsKeyDown(Keys.R))
            {

            }

            base.Update(gameTime);
        }
        #region Summary Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
#endregion
        protected override void Draw(GameTime gameTime)
        {
            
            // TODO: Add your drawing code here

            // You and I both know what this does 
            spriteBatch.Begin();

            // If the chosen gameState is the main menu, do various things 
            if (gameState == GameState.mainMenu)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                // Bad code (should have used for loops instead of copy pasting) that is used to display the helpful texts in the main menu 
                spriteBatch.DrawString(smallFont, leftOrRightMessage.ToString(), helpfulMessagePos1, Color.Black);

                spriteBatch.DrawString(smallFont, upOrDownMessage.ToString(), helpfulMessagePos2, Color.Black);

                spriteBatch.DrawString(smallFont, escAndFullscreenMessage.ToString(), helpfulMessagePos3, Color.Black);

                spriteBatch.DrawString(smallFont, menuChangingMessage.ToString(), helpfulMessagePos4, Color.Black);

                spriteBatch.DrawString(smallFont, savingMessage.ToString(), helpfulMessagePos5, Color.Black);

                spriteBatch.DrawString(smallFont, resetMessage.ToString(), helpfulMessagePos6, Color.Black);

                // These are all used to reveal which person you have selected 
                if (playerCharacter == 0)
                {
                    spriteBatch.DrawString(font, kalle.ToString(), kalleNamePos, Color.White);
                }  
                else
                {
                    spriteBatch.DrawString(font, kalle.ToString(), kalleNamePos, Color.Black);
                }
                
                // These all draws the score in correlation to their name 
                spriteBatch.DrawString(font, player1Score.ToString(), kalleScorePos, Color.Black);

                if (kalleIsTrue == true)
                {
                    spriteBatch.DrawString(smallFont, savedPerson1 + player1Score + savedPerson2 + names[0].ToString(), kalleBoolPos, Color.Black);
                }

                if (playerCharacter == 1)
                {
                    spriteBatch.DrawString(font, olaf.ToString(), olafNamePos, Color.White);
                }
                else
                {
                    spriteBatch.DrawString(font, olaf.ToString(), olafNamePos, Color.Black);
                }
                
                spriteBatch.DrawString(font, player2Score.ToString(), olafScorePos, Color.Black);

                if (olafIsTrue == true)
                {
                    spriteBatch.DrawString(smallFont, savedPerson1 + player2Score + savedPerson2 + names[1].ToString(), olafBoolPos, Color.Black);
                }

                if (playerCharacter == 2)
                {
                    spriteBatch.DrawString(font, kerstin.ToString(), kerstinNamePos, Color.White);
                }
                else
                {
                    spriteBatch.DrawString(font, kerstin.ToString(), kerstinNamePos, Color.Black);
                }
                
                spriteBatch.DrawString(font, player3Score.ToString(), kerstinScorePos, Color.Black);

                if (kerstinIsTrue == true)
                {
                    spriteBatch.DrawString(smallFont, savedPerson1 + player3Score + savedPerson2 + names[2].ToString(), kerstinBoolPos, Color.Black);
                }
            }

            // These all loads the data from the file and draws them in the high-score list 
            if (gameState == GameState.highScoreList)
            {            
                spriteBatch.DrawString(font, LoadData(Filename).PlayerName[0].ToString(), highScoreP1, Color.Black);
                spriteBatch.DrawString(font, LoadData(Filename).Score[0].ToString(), highScoreP1Score, Color.Black);

                spriteBatch.DrawString(font, LoadData(Filename).PlayerName[1].ToString(), highScoreP2, Color.Black);
                spriteBatch.DrawString(font, LoadData(Filename).Score[1].ToString(), highScoreP2Score, Color.Black);

                spriteBatch.DrawString(font, LoadData(Filename).PlayerName[2].ToString(), highScoreP3, Color.Black);
                spriteBatch.DrawString(font, LoadData(Filename).Score[2].ToString(), highScoreP3Score, Color.Black);

                GraphicsDevice.Clear(Color.Wheat);
            }
                spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
