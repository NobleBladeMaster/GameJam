using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TeamHaddock
{
    public class Enemy
    {
        public CollidableObject collidableObject;

        private Color Color { get; set; } = Color.White;

        public Enemy(Texture2D texture, Vector2 position)
        {
            collidableObject = new CollidableObject(texture, position, new Rectangle(120, 0, 60, 120), 0);
        }


        public void Update(GameTime gameTime)
        {
            Color = collidableObject.IsColliding(InGame.player.collidableObject) ? Color.Red : Color.White;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(collidableObject.Texture, collidableObject.Position, collidableObject.SourceRectangle, Color, collidableObject.Rotation, collidableObject.Origin, 1.0f, SpriteEffects.None, 0.0f);
        } 
    }
}
