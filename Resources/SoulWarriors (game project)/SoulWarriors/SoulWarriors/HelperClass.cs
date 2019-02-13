using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoulWarriors
{
    static class HelperClass
    {
        /// <summary>
        /// Draws a texture between two Vector2
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void DrawLine(this SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end)
        {
            spriteBatch.Draw(texture, start, null, Color.White,
                (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                new Vector2(0f, (float)texture.Height / 2),
                new Vector2(Vector2.Distance(start, end), texture.Height),
                SpriteEffects.None, 0f);
        }

        ///// <summary> 
        ///// Check if key is pressed now but not one update ago 
        ///// </summary> 
        ///// <param name="currentKeyboardState"></param> 
        ///// <param name="previousKeyboardState"></param> 
        ///// <param name="key">key to check</param> 
        ///// <returns></returns> 
        //public static bool SingleActivationKey(this KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, Keys key)
        //{
        //    // If key is down but was up before 
        //    return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        //}
    }
}
