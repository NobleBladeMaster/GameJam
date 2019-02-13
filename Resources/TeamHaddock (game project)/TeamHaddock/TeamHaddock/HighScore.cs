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

namespace TeamHaddock
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
    }

    [Serializable]
    public struct SaveData
    {
        public List<HighScoreEntry> Scores;

        public SaveData(List<HighScoreEntry> newHighScoreEntries)
        {
            Scores = newHighScoreEntries;
        }
    }

    // Created by Elias 11-29 // Edited By Noble 12-11 
    public static class HighScore
    {
        private static Texture2D background;
        private static SpriteFont scoreFont;

        private static readonly string FileName = "save.dat";

        private static SaveData currentData;

        public static void Initilize()
        {
            if (!File.Exists(FileName))
            {
                SaveData data = new SaveData(new List<HighScoreEntry>(5) { new HighScoreEntry("default", 0)});
                currentData = data;
                DoSave(data, FileName);
            }
            else
            {
                currentData = LoadData(FileName);
            }
        }

        public static void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>(@"Textures/Backgrounds/HighScoreBackGround");
            scoreFont = content.Load<SpriteFont>(@"Fonts/CreditsTitleFont");
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

        private static SaveData LoadData(string FileName)
        {
            SaveData data;

            FileStream stream = File.Open(FileName, FileMode.Open, FileAccess.ReadWrite);
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
            if (currentData.Scores.Count >= 5)
            {
                // And newScore is smaller than the smallest score
                if (newScore.score < currentData.Scores[4].score)
                {
                    // Do not add score
                    return;
                }
            }
            // add newScore
            currentData.Scores.Add(newScore);
            // sort scores
            for (int write = 0; write < currentData.Scores.Count; write++)
            {
                for (int sort = 0; sort < currentData.Scores.Count - 1; sort++)
                {
                    if (currentData.Scores[sort].score > currentData.Scores[sort + 1].score)
                    {
                        HighScoreEntry temp = currentData.Scores[sort + 1];
                        currentData.Scores[sort + 1] = currentData.Scores[sort];
                        currentData.Scores[sort] = temp;
                    }
                }
            }

            currentData.Scores.Reverse();
            DoSave(currentData, FileName);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Draw background
            spriteBatch.Draw(background, new Rectangle(0, 0, Game1.ScreenBounds.X, Game1.ScreenBounds.Y), Color.White);
            // Draw score
            for (int i = 0; i < currentData.Scores.Count; i++ )
            {
                // Draw Name
                spriteBatch.DrawString(scoreFont, currentData.Scores[i].name, new Vector2(Game1.ScreenBounds.X * 0.25f, i * 60 + 120), Color.White);
                // Draw score
                spriteBatch.DrawString(scoreFont, currentData.Scores[i].score.ToString(), new Vector2(Game1.ScreenBounds.X * 0.75f, i * 60 + 120), Color.White);
            }
            spriteBatch.End();
        }
    }
}
