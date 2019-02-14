using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam
{
  public class ShotManager
    {
        public List<Sprite> Attacks = new List<Sprite>();
        private Rectangle screenBounds;

            


        private static Texture2D Texture;
        private static Rectangle InitialFrame;
        private static int FrameCount;
        private float attackSpeed;
        private static int CollisionRadius;

        // Constructor
        public ShotManager(Texture2D texture, Rectangle initialFrame, int frameCount, int collisionRadius, float shotSpeed, Rectangle screenBounds)
        {
            Texture = texture;
            InitialFrame = initialFrame;
            FrameCount = frameCount;
            CollisionRadius = collisionRadius;
            this.attackSpeed = attackSpeed;
            this.screenBounds = screenBounds;
        }

        // Fireshot
        public void Attack(Vector2 position, Vector2 velocity, bool playerFired)
        {
            Sprite thisAttack = new Sprite(position, Texture, InitialFrame, velocity);

            thisAttack.Velocity *= attackSpeed;

            for (int i = 1; i < FrameCount; i++)
            {
                thisAttack.AddFrame(new Rectangle(InitialFrame.X + (InitialFrame.Width * i), InitialFrame.Y, InitialFrame.Width, InitialFrame.Height));
            }
            thisAttack.CollisionRadius = CollisionRadius;
            Attacks.Add(thisAttack);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = Attacks.Count - 1; i >= 0; i--)
            {
                Attacks[i].Update(gameTime);
                if (!screenBounds.Intersects(Attacks[i].Destination))
                {
                    Attacks.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite attack in Attacks)
            {
                attack.Draw(spriteBatch);
            }
        }
    }
}
    

