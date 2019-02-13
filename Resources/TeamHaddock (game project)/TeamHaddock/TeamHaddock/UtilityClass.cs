using System;
using Microsoft.Xna.Framework.Input;

// Class created by Alexander 11-07
namespace TeamHaddock
{
    /// <summary>
    /// Contains small utility methods
    /// </summary>
    internal static class UtilityClass
    {
        /// <summary>
        /// KeyboardState during this update
        /// </summary>
        private static KeyboardState currentKeyboardState;

        /// <summary>
        /// KeyboardState during last update
        /// </summary>
        private static KeyboardState previousKeyboardState;

        /// <summary>
        /// Update UtilityClass logic
        /// </summary>
        public static void Update()
        {
            // Set previous KeyboardState
            previousKeyboardState = currentKeyboardState;
            // Get current KeyboardState 
            currentKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Check if key is pressed now but not one update ago
        /// </summary>
        /// <param name="key">key to check</param>
        /// <returns></returns>
        public static bool SingleActivationKey(Keys key)
        {
            // If key is down but was up before
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        public static float Truncate(this float value, int digits)
        {
            double mult = Math.Pow(10.0, digits);
            double result = Math.Truncate(mult * value) / mult;
            return (float)result;
        }
    }
}