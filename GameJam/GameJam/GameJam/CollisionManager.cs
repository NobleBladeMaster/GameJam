using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameJam
{
    class CollisionManager
    {

        private PlayerManager playerManager;
        private EnemyManager enemyManager;
        //private ExplosionManager explosionManager;
        private Vector2 offScreen = new Vector2(-500, -500);

        public CollisionManager(PlayerManager playerSprite, /*ExplosionManager explosionManager,*/ EnemyManager enemyManager)
        {
            this.playerManager = playerSprite;
            //this.explosionManager = explosionManager;
            this.enemyManager = enemyManager;
        }

        private void CheckShotToEnemyCollisions()
        {
            foreach (Sprite shot in playerManager.PlayerShotManager.Shots)
            {
                foreach (Enemy enemy in enemyManager.Enemies)
                {
                    if (shot.IsCircleColliding(enemy.enemySprite.Center, enemy.enemySprite.CollisionRadius))
                    {
                        shot.Position = offScreen;
                        enemy.destroyed = true;

                        //explosionManager.AddExplosion(enemy.enemySprite.Center, enemy.enemySprite.Velocity / 10);
                    }
                }
            }
        }

        private void CheckShotToPlayerCollisions()
        {
            foreach (Sprite shot in enemyManager.enemyShotManager.Shots)
            {
                if (shot.IsCircleColliding(playerManager.Center, playerManager.collisionRadius))
                {
                    shot.Position = offScreen;
                    playerManager.destroyed = true;
                    //explosionManager.AddExplosion(playerManager.Center, Vector2.Zero);
                }
            }
        }

        private void CheckEnemyToPlayerCollisions()
        {
            foreach (Enemy enemy in enemyManager.Enemies)
            {
                if (enemy.enemySprite.IsCircleColliding(playerManager.Position, playerManager.collisionRadius))
                {
                    enemy.destroyed = true;

                    //explosionManager.AddExplosion(enemy.enemySprite.Center, enemy.enemySprite.Velocity / 10);

                    playerManager.destroyed = true;

                    //explosionManager.AddExplosion(playerManager.Position, Vector2.Zero);
                }
            }
        }

        public void CheckCollision()
        {
            CheckShotToEnemyCollisions();

            if (!playerManager.destroyed)
            {
                CheckShotToEnemyCollisions();
                CheckEnemyToPlayerCollisions();
            }
        }
    }
}
