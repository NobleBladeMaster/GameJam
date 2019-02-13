﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_Starfield_Noble_SU17
{
    class EnemyManager
    {
        private Texture2D texture;
        private Rectangle initialFrame;
        private int frameCount;

        public List<Enemy> Enemies = new List<Enemy>();

        public ShotManager enemyShotManager;
        private PlayerManager playerManager;

        public int minShipsPerWave = 5;
        public int maxShipsPerWave = 8;
        private float nextWaveTimer = 0.0f;
        private float nextWaveMinTimer = 8.0f;
        private float shipSpawnTimer = 0.0f;
        private float shipSpawnWaitTime = 0.5f;

        private float shipShotChance = 0.2f;

        private List<List<Vector2>> pathWaypoints = new List<List<Vector2>>();

        private Dictionary<int, int> waveSpawns = new Dictionary<int, int>();

        public bool Active = true;

        private Random rand = new Random();

        //Alla listor
        private void setUpWaypoints()
        {
            List<Vector2> path0 = new List<Vector2>();
            path0.Add(new Vector2(850, 300));
            path0.Add(new Vector2(-100, 300));
            pathWaypoints.Add(path0);
            waveSpawns[0] = 0;

            List<Vector2> path1 = new List<Vector2>();
            path1.Add(new Vector2(-50, 225));
            path1.Add(new Vector2(885, 225));
            pathWaypoints.Add(path1);
            waveSpawns[1] = 0;

            List<Vector2> path2 = new List<Vector2>();
            path2.Add(new Vector2(-100, 50));
            path2.Add(new Vector2(150, 50));
            path2.Add(new Vector2(200, 75));
            path2.Add(new Vector2(200, 125));
            path2.Add(new Vector2(150, 150));
            path2.Add(new Vector2(150, 175));
            path2.Add(new Vector2(200, 200));
            path2.Add(new Vector2(600, 200));
            path2.Add(new Vector2(850, 600));
            pathWaypoints.Add(path2);
            waveSpawns[2] = 0;

            List<Vector2> path3 = new List<Vector2>();
            path3.Add(new Vector2(600, -100));
            path3.Add(new Vector2(600, 250));
            path3.Add(new Vector2(580, 275));
            path3.Add(new Vector2(500, 250));
            path3.Add(new Vector2(500, 200));
            path3.Add(new Vector2(450, 175));
            path3.Add(new Vector2(400, 150));
            path3.Add(new Vector2(-100, 150));
            pathWaypoints.Add(path3);
            waveSpawns[3] = 0;
        }

        public EnemyManager(Texture2D texture, Rectangle initialFrame, int frameCount, PlayerManager playerSprite, Rectangle screenBounds)
        {
            this.texture = texture;
            this.initialFrame = initialFrame;
            this.frameCount = frameCount;
            this.playerManager = playerSprite;

            enemyShotManager = new ShotManager(texture, new Rectangle(0, 286, 11, 1), 1, 1, 150f, screenBounds);

            setUpWaypoints();
        }

        public void SpawnEnemy(int path)
        {
            Enemy thisEnemy = new Enemy(texture, pathWaypoints[path][0], initialFrame, frameCount);
            for (int i = 0; i < pathWaypoints[path].Count(); i++)
            {
                thisEnemy.AddWaypoint(pathWaypoints[path][i]);
            }
            Enemies.Add(thisEnemy);
        }

        public void SpawnWave(int waveType)
        {
            waveSpawns[waveType] += rand.Next(minShipsPerWave, maxShipsPerWave + 1); 
        }

        private void updateWaveSpawns(GameTime gameTime)
        {
            shipSpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shipSpawnTimer > shipSpawnWaitTime)
            {
                for (int i = waveSpawns.Count - 1; i >= 0; i--)
                {
                    if (waveSpawns[i] > 0)
                    {
                        waveSpawns[i]--;
                        SpawnEnemy(i);
                    }
                }
                shipSpawnTimer = 0f;
            }

            nextWaveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (nextWaveTimer > nextWaveMinTimer)
                {
                    SpawnWave(rand.Next(0, pathWaypoints.Count));
                    nextWaveTimer = 0f;
                }
            
        }

            public void Update(GameTime gameTime)
        {
            enemyShotManager.Update(gameTime);

            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                Enemies[i].Update(gameTime);
                if (Enemies[i].IsActive() == false)
                {
                    Enemies.RemoveAt(i);
                }
                else
                {
                    if ((float)rand.Next(0, 1000) / 10 <= shipShotChance)
                    {
                        Vector2 fireLoc = Enemies[i].enemySprite.Position;
                        fireLoc += Enemies[i].gunOffset;

                        Vector2 shotDirection = playerManager.Position - fireLoc;

                        shotDirection.Normalize();

                        enemyShotManager.FireShot(fireLoc, shotDirection, false);
                    }
                }
            }    

            if (Active)
            {
                updateWaveSpawns(gameTime);
            }
         }

        //Draw
        public void Draw (SpriteBatch spriteBatch)
        {
            enemyShotManager.Draw(spriteBatch);

            foreach (Enemy enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }
        } 
    }
}
