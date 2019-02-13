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
    public class Knight : Player
    {
        public Knight(Texture2D texture, List<Animation> animations)
            : base(texture, new Vector2(100f, 500f), new PlayerControlScheme(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.RightControl, Keys.RightShift, Keys.Enter, Keys.Back), animations)
        {
            
        }
    }
}
