using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// Class Created by Alexander 11-07
namespace TeamHaddock
{
    /// <summary>
    ///     An object used for easy control in menus using the keyboard as input
    /// </summary>
    internal class MenuControls
    {
        /// <summary>
        ///     Number of menu options
        /// </summary>
        private readonly Vector2 selectionMax;


        /// <summary>
        ///     Creates a new instance of MenuControls
        /// </summary>
        /// <param name="selectionMax">Number of menu options</param>
        public MenuControls(Vector2 selectionMax)
        {
            this.selectionMax = selectionMax;
        }

        public bool IsEnterDown { get; private set; }

        public bool IsEscapeDown { get; private set; }

        /// <summary>
        ///     Updates selected menu option in menus
        /// </summary>
        /// <returns>vector2 passed by reference to change</returns>
        public void UpdateSelected(ref Vector2 selected)
        {
            // When W or UP arrow keys are pressed
            if (UtilityClass.SingleActivationKey(Keys.W) || UtilityClass.SingleActivationKey(Keys.Up))
            {
                // And selected.Y is GREATER THAN 0, preventing it from exiting the selection range, 
                if (selected.Y > 0)
                {
                    // Then move selected
                    selected.Y--;
                }
            }

            // When A or Left arrow keys are pressed
            if (UtilityClass.SingleActivationKey(Keys.A) || UtilityClass.SingleActivationKey(Keys.Left))
            {
                // And selected.X is GREATER THAN 0, preventing it from exiting the selection range, 
                if (selected.X > 0)
                {
                    // Then move selected
                    selected.X--;
                }
            }

            // When S or Down arrow keys are pressed
            if (UtilityClass.SingleActivationKey(Keys.S) || UtilityClass.SingleActivationKey(Keys.Down))
            {
                // And selected.Y is LESS THAN selectionMax.Y, preventing it from exceeding maximum Y selection range, 
                if (selected.Y < selectionMax.Y)
                {
                    // Then move selected
                    selected.Y++;
                }
            }
            // When D or Right arrow keys are pressed
            if (UtilityClass.SingleActivationKey(Keys.D) || UtilityClass.SingleActivationKey(Keys.Right))
            {
                // And selected.X is LESS THAN selectionMax.X, preventing it from exceeding maximum X selection range, 
                if (selected.X < selectionMax.X)
                {
                    // Then move selected
                    selected.X++;
                }

            }
                // Update Enter key
                IsEnterDown = UtilityClass.SingleActivationKey(Keys.Enter);

                // Update escape key
                IsEscapeDown = UtilityClass.SingleActivationKey(Keys.Escape);
        }
    }
}