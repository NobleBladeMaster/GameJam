using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeamHaddock
{
    public interface IEnemy
    {
        CollidableObject CollidableObject { get; }
        void DrawColorMap(SpriteBatch spriteBatch);
        void DrawNormalMap(SpriteBatch spriteBatch);
        bool TakeDamage(int damageTaken);
        void Update(GameTime gameTime);
    }
}