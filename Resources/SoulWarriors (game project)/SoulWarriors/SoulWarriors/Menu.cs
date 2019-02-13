using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoulWarriors
{
    public delegate void ButtonActionDelegate();

    public struct MenuControlScheme
    {
        public Keys MoveUp { get; set; }
        public Keys MoveDown { get; set; }
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        public Keys ActivateButton { get; set; }

        public MenuControlScheme(Keys moveUp, Keys moveDown, Keys moveLeft, Keys moveRight, Keys activateButton)
        {
            MoveUp = moveUp;
            MoveDown = moveDown;
            MoveLeft = moveLeft;
            MoveRight = moveRight;
            ActivateButton = activateButton;
        }
    }

    // TODO: Comment Button
    public struct Button
    {
        public enum ButtonStates
        {
            Released,
            Selected,
        }

        private ButtonActionDelegate Action { get; }

        private Texture2D ReleasedTexture { get; }
        private Texture2D SelectedTexture { get; }

        public Rectangle Destination { get; set; }
        /// <summary>
        /// Place on menu for keyboard control
        /// </summary>
        public Point MenuPosition { get; }

        public ButtonStates State { get; set; }

        /// <summary>
        /// Instantiates a new instance of Button
        /// </summary>
        /// <param name="menuPosition">Position of buttons in the menu</param>
        /// <param name="destination">Destination rectangle</param>
        /// <param name="releasedTexture">Texture when not selected</param>
        /// <param name="selectedTexture">Texture when selected</param>
        /// <param name="action">Custom code for this button</param>
        public Button(Point menuPosition, Rectangle destination, Texture2D releasedTexture, Texture2D selectedTexture, ButtonActionDelegate action)
        {
            MenuPosition = menuPosition;
            Destination = destination;

            ReleasedTexture = releasedTexture;
            SelectedTexture = selectedTexture;

            Action = action;

            State = ButtonStates.Released;
        }

        /// <summary>
        /// Attempts to run the Action delegate
        /// </summary>
        /// <returns>If the execution of delegate was successful or not</returns>
        public bool RunAction()
        {
            try
            {
                Action();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Draws this button with the texture and destination rectangle
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(State == ButtonStates.Released ? ReleasedTexture : SelectedTexture, Destination, Color.White);
        }
    }



    //TODO: Comment Menu
    public class Menu
    {
        private KeyboardState _previousKeyboardState;
        private MouseState _previousMouseState;
        private readonly MenuControlScheme _controlScheme;
        private readonly Viewport _viewport;
        private readonly Texture2D _background;

        private readonly Button[] _buttons;

        private Point? _selectedMenuPosition = Point.Zero;
        private Point _nonNullableSelectedPoint = Point.Zero; // TODO: Change name for _nonNullableSelectedPoint
        private readonly Point _minSelected;
        private readonly Point _maxSelected;
        private Point? _previousMousePoint;
        private Point _previousKeyboardPoint;

        public Menu(Texture2D background, Button[] buttons, MenuControlScheme keyboardControlScheme, Viewport viewport)
        {
            _background = background;
            _buttons = buttons;
            _controlScheme = keyboardControlScheme;
            _viewport = viewport;

            // Calculate min and max selected
            foreach (Button button in buttons)
            {
                if (button.MenuPosition.X < _minSelected.X)
                {
                    _minSelected.X = button.MenuPosition.X;
                }
                else if (button.MenuPosition.X > _maxSelected.X)
                {
                    _maxSelected.X = button.MenuPosition.X;
                }

                if (button.MenuPosition.Y < _minSelected.Y)
                {
                    _minSelected.Y = button.MenuPosition.Y;
                }
                else if (button.MenuPosition.Y > _maxSelected.Y)
                {
                    _maxSelected.Y = button.MenuPosition.Y;
                }
            }

            _previousKeyboardState = Keyboard.GetState();
            _previousMouseState = Mouse.GetState();
        }

        public virtual void Update()
        {
            UpdateButtonSelection();
            UpdateButtonAction();

            // Set _previousKeyboardState to current keyboard state
            _previousKeyboardState = Keyboard.GetState();
            _previousMouseState = Mouse.GetState();
        }

        private void UpdateButtonSelection()
        {
            // Get new selected from mouse and keyboard
            Point? mousePoint = UpdateMouse();
            Point keyboardPoint = UpdateKeyboard();

            // If mouse point has changed
            if (mousePoint != _previousMousePoint)
            {
                // Set _selectedMenuPosition to mouse point
                _selectedMenuPosition = mousePoint;
                // Update button states with the new _selectedMenuPosition
                UpdateButtonStates();
            }
            // If keyboardPoint has changed and mouse did not change
            else if(keyboardPoint != _previousKeyboardPoint)
            {
                // Set _selectedMenuPosition to the new selected point
                _selectedMenuPosition = keyboardPoint;
            }

            if (_selectedMenuPosition != null)
            {
                _nonNullableSelectedPoint = (Point) _selectedMenuPosition;
            }

            // Update button states with the new _selectedMenuPosition
            UpdateButtonStates();
            _previousMousePoint = mousePoint;
            _previousKeyboardPoint = keyboardPoint;
        }

        private Point? UpdateMouse()
        {
            // Update Mouse Position
            Point mousePosition = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            // For every button in Buttons
            foreach (Button button in _buttons)
            {
                // If the mouse is on this button
                if (button.Destination.Contains(mousePosition))
                {
                    // Return this button´s MenuPosition
                    return button.MenuPosition;
                }
            }
            // The mouse is not on any button and will return null
            return null;
        }

        private Point UpdateKeyboard() // TODO: Add jump to nearest button code for when the menu points are not correct
        {
            Point selected = _nonNullableSelectedPoint;
            // Update keyboard
            KeyboardState keyboard = Keyboard.GetState();

            // Get input
            if (keyboard.SingleActivationKey(_previousKeyboardState, _controlScheme.MoveUp)) { selected.Y--;}

            if (keyboard.SingleActivationKey(_previousKeyboardState, _controlScheme.MoveDown)) { selected.Y++;}

            if (keyboard.SingleActivationKey(_previousKeyboardState, _controlScheme.MoveLeft)) { selected.X--;}

            if (keyboard.SingleActivationKey(_previousKeyboardState, _controlScheme.MoveRight)) { selected.X++;}

            //Clamp selected
            selected.X = (int)MathHelper.Clamp(selected.X, _minSelected.X, _maxSelected.X);
            selected.Y = (int)MathHelper.Clamp(selected.Y, _minSelected.Y, _maxSelected.Y);

            return selected;
        }

        private void UpdateButtonStates()
        {
            // If no button is selected set all button states to Released
            if (_selectedMenuPosition == null)
            {
                for (int i = 0; i < _buttons.Length; i++)
                {
                    _buttons[i].State = Button.ButtonStates.Released;
                }
                return;
            }
            // Set the selected button´s state to Selected, and every else to Released
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].State = _buttons[i].MenuPosition == _selectedMenuPosition ? Button.ButtonStates.Selected : Button.ButtonStates.Released;
            }
        }

        /// <summary>
        /// Check if left mouse button or first ActivateButton key 
        /// </summary>
        private void UpdateButtonAction()
        {
            // If left mouse key or ActivateButton key is pressed
            if (Mouse.GetState().SingleActivationLeftClick(_previousMouseState) || Keyboard.GetState().SingleActivationKey(_previousKeyboardState, _controlScheme.ActivateButton))
            {
                foreach (Button button in _buttons)
                {
                    if (button.MenuPosition == _selectedMenuPosition)
                    {
                        button.RunAction();
                    }
                }
            }
        }

        /// <summary>
        /// Draws the background and buttons
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Draw background
            spriteBatch.Draw(_background, _viewport.Bounds, Color.White);

            // Draw buttons
            foreach (Button button in _buttons)
            {
                button.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }

    public static class MenuExtensions
    {
        /// <summary> 
        /// Check if key is pressed now but not one update ago 
        /// </summary> 
        /// <param name="currentKeyboardState"></param> 
        /// <param name="previousKeyboardState">KeyboardState to compare to</param> 
        /// <param name="key">key to check</param> 
        /// <returns></returns> 
        public static bool SingleActivationKey(this KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, Keys key)
        {
            // If key is down but was up before 
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Check if left mouse button is pressed now but not one update ago     
        /// </summary>
        /// <param name="currentMouseState"></param>
        /// <param name="previousMouseState">MouseState to compare to</param>
        /// <returns></returns>
        public static bool SingleActivationLeftClick(this MouseState currentMouseState, MouseState previousMouseState)
        {
            // If Left mouse button is Pressed but was Released before
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        }
    }
}
