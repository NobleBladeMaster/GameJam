using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace SoulWarriors
{
    [Serializable]
    public struct HighScoreEntry
    {
        public string name;
        public int score;

        public HighScoreEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }

        public static HighScoreEntry GenerateDefaultHighScoreEntry()
        {
            return new HighScoreEntry("Default", 0);
        }
    }

    [Serializable]
    public struct SaveData
    {
        public List<HighScoreEntry> CampaingScores;
        public List<HighScoreEntry> EndlessScores;

        public SaveData(List<HighScoreEntry> newCampaingScoreEntries, List<HighScoreEntry> newEndlessScores)
        {
            CampaingScores = newCampaingScoreEntries;
            EndlessScores = newEndlessScores;
        }

        public static SaveData GenerateDefaultSaveData()
        {
            return new SaveData(
                new List<HighScoreEntry>(5) {HighScoreEntry.GenerateDefaultHighScoreEntry()},
                new List<HighScoreEntry>(5) {HighScoreEntry.GenerateDefaultHighScoreEntry()});
        }
    }

    
    public static class HighScore
    {
        private enum ShowScoreStates
        {
            Campaing,
            Endless
        }

        private static ShowScoreStates showScoreState = ShowScoreStates.Campaing;

        private static Menu menu;
        private static Viewport _viewport;
        private static SpriteFont scoreFont;

        private const string FileName = "save.dat";

        private static SaveData currentData;

        public static void Initilize()
        {
            if (!File.Exists(FileName))
            {
                SaveData data = SaveData.GenerateDefaultSaveData();
                currentData = data;
                DoSave(data, FileName);
            }
            else
            {
                currentData = LoadData(FileName);
            }
        }

        public static void LoadContent(ContentManager content, Viewport viewport)
        {
            // Create a new menu
            menu = new Menu(content.Load<Texture2D>(@"Textures/Menu/MainMenuBackground"),
                new Button[]
                {
                    // Back button
                    new Button(new Point(0,0),
                        new Rectangle(100,100,50,50),
                        content.Load<Texture2D>(@"Textures/Menu/Button1"),
                        content.Load<Texture2D>(@"Textures/Menu/Menu0_button1"),
                        () => Game1.CurrentGameState = Game1.GameState.MainMenu),
                    // Show camping scores
                    new Button(new Point(0,1),
                        new Rectangle(100, 400, 100, 50),
                        content.Load<Texture2D>(@"Textures/Menu/Button1"),
                        content.Load<Texture2D>(@"Textures/Menu/Menu0_button1"),
                        () => showScoreState = ShowScoreStates.Campaing),
                    // Show Endless scores
                    new Button(new Point(0,2),
                        new Rectangle(100, 500, 100, 50),
                        content.Load<Texture2D>(@"Textures/Menu/Button1"),
                        content.Load<Texture2D>(@"Textures/Menu/Menu0_button1"),
                        () => showScoreState = ShowScoreStates.Endless),
                },
                new MenuControlScheme(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Enter),
                viewport);
            _viewport = viewport;
            scoreFont = content.Load<SpriteFont>(@"Fonts/DebugFont"); // TODO: Add high score font
        }

        public static void Update()
        {
            menu.Update();
        }

        /// <summary>
        /// Opens file and saves data
        /// </summary>
        /// <param name="data">data to write</param>
        /// <param name="filename">name of file to write to</param>
        private static void DoSave(SaveData data, string filename)
        {
            // Open or create file
            FileStream stream = File.Open(filename, FileMode.OpenOrCreate);
            try
            {
                // Make to XML and try to open filestream
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close file
                stream.Close();
            }
        }

        private static SaveData LoadData(string fileName)
        {
            SaveData data;

            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                data = (SaveData)serializer.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }

            return data;
        }

        public static void SaveHighScore(HighScoreEntry newScore)
        {
            // If there are already 10 scores
            if (currentData.CampaingScores.Count >= 5)
            {
                // And newScore is smaller than the smallest score
                if (newScore.score < currentData.CampaingScores[4].score)
                {
                    // Do not add score
                    return;
                }
            }
            // Add newScore
            currentData.CampaingScores.Add(newScore);
            // sort scores
            for (int write = 0; write < currentData.CampaingScores.Count; write++)
            {
                for (int sort = 0; sort < currentData.CampaingScores.Count - 1; sort++)
                {
                    if (currentData.CampaingScores[sort].score > currentData.CampaingScores[sort + 1].score)
                    {
                        HighScoreEntry temp = currentData.CampaingScores[sort + 1];
                        currentData.CampaingScores[sort + 1] = currentData.CampaingScores[sort];
                        currentData.CampaingScores[sort] = temp;
                    }
                }
            }

            currentData.CampaingScores.Reverse();
            DoSave(currentData, FileName);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);

            spriteBatch.Begin();

            spriteBatch.DrawString(scoreFont, showScoreState == ShowScoreStates.Campaing ? "Campaing High Scores" : "Endless High Scores", new Vector2(_viewport.Width / 2f, _viewport.Height / 10f), Color.Black);

            // Draw score
            for (int i = 0; i < currentData.CampaingScores.Count; i++ )
            {
                // Draw Name
                spriteBatch.DrawString(scoreFont, showScoreState == ShowScoreStates.Campaing ? currentData.CampaingScores[i].name : currentData.EndlessScores[i].name, new Vector2(_viewport.Width / 2f - 100, i * 60 + 180), Color.Black);
                // Draw score
                spriteBatch.DrawString(scoreFont, showScoreState == ShowScoreStates.Campaing ? currentData.CampaingScores[i].score.ToString() : currentData.EndlessScores[i].score.ToString(), new Vector2(_viewport.Width / 2f + 100, i * 60 + 180), Color.Black);
            }
            spriteBatch.End();
        }
    }
}
