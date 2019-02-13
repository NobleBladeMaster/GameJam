using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TeamHaddock
{
    public class LampPost
    {
        private static Texture2D texture, normalMap;
        private Vector2 position;

        public static void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"Textures/ActiveObjects/LampPost");
            normalMap = content.Load<Texture2D>(@"Textures/ActiveObjects/LampPostNormalMap");
        }
        // Edited by Noble 12-09 
        public LampPost(Vector2 position)
        {
            this.position = position;

            InGame.dynamicLight.lights.Add(new PointLight
            {
                IsEnabled = true,
                Color = new Vector4(0.1f, 0.2f, 0.8f, 1f),
                Power = 2.0f,
                LightDecay = 500,
                Position = new Vector3(position.X, position.Y - texture.Height + 38, 20)
            });
        }

        public void DrawColorMap(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height), Vector2.One, SpriteEffects.None, 0);
        }

        public void DrawNormalMap(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(normalMap, position, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height), Vector2.One, SpriteEffects.None, 0);
        }
    }
}
