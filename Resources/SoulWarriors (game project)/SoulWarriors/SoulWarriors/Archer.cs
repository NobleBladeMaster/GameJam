using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SoulWarriors
{
    public class Archer : Player
    {
        public List<Projectile> arrows = new List<Projectile>();
        private Texture2D _arrowTexture;

        public Archer(Texture2D texture, Texture2D arrowTexture, List<Animation> animations) 
            : base(texture, new Vector2(500f), new PlayerControlScheme(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space, Keys.LeftAlt, Keys.X, Keys.C), animations)
        {
            _arrowTexture = arrowTexture;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateArrows(gameTime);
        }

        private void UpdateArrows(GameTime gameTime)
        {
            List<int> arrowsToBeRemoved = new List<int>();
            for (int i = 0; i < arrows.Count; i++)
            {
                // Update arrow
                arrows[i].Update(gameTime);
                // If arrow is no longer active add its index number to the arrowsToBeRemoved list
                if (arrows[i].Active == false)
                {
                    arrowsToBeRemoved.Add(i);
                }
            }
            // Sort arrows arrowsToBeRemoved in ascending order
            arrowsToBeRemoved.Sort();
            // Reverse arrowsToBeRemoved to descending order
            arrowsToBeRemoved.Reverse();
            // Try to remove arrows
            try
            {
                // Remove arrows
                for (int index = 0; index < arrowsToBeRemoved.Count; index++)
                {
                    arrows.RemoveAt(arrowsToBeRemoved[index]);
                }
            }
            finally
            {
                // Always clear arrowsToBeRemoved
                arrowsToBeRemoved.Clear();
            }

        }

        /// <summary>
        /// Fire Arrow
        /// </summary>
        protected override void Action1()
        {
                arrows.Add(new Projectile(_arrowTexture, CollidableObject.Position, InGame.MousePos, 0.3f, InGame.PlayArea));
        }
        protected override void Action2()
        {
        }
        protected override void Action3()
        {
        }
        protected override void Action4()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Projectile arrow in arrows)
            {
                arrow.Draw(spriteBatch);
            }
        }
    }
}
