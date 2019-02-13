using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoulWarriors
{
    public class Chain
    {
        private readonly Texture2D _texture;

        public Vector2 StartPosition { get; set; }
        public Vector2 EndPosition { get; set; }
        public float Rotation => (float)Math.Atan2(EndPosition.Y - StartPosition.Y, EndPosition.X - StartPosition.X);
        public float Length => Vector2.Distance(StartPosition, EndPosition);

        public Chain(Texture2D texture)
        {
            _texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, InGame.Camera.TransformMatrix);
            spriteBatch.Draw(
                _texture,
                StartPosition,
                new Rectangle(0,0, (int)Length, _texture.Height),
                Color.White,
                Rotation,
                new Vector2(0f, (float)_texture.Height / 2),
                1,
                SpriteEffects.None,
                0f);
            spriteBatch.End();
        }
    }
}