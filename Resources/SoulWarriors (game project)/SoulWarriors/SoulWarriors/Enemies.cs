using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoulWarriors
{
    public sealed class Goblin : Enemy
    {
        private static Texture2D texture;
        private static List<Animation> animations;

        public Goblin(SpawnAreas spawnAreas) : base(texture, spawnAreas, AiTypes.Smart, 0.15f)
        {
        }
         
        public static void LoadContent(ContentManager content)
        {
            // Load goblin texture
            texture = content.Load<Texture2D>(@"Textures/ArcherSpriteSheet");
            animations = new List<Animation>()
            {
                new Animation("IdleDown", new List<Frame>() { new Frame(Rectangle.Empty, Int32.MaxValue)})
            };
        }
    }
}
