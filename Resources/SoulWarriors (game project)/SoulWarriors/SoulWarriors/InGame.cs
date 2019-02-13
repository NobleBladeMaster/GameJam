using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoulWarriors
{
    public static class InGame
    {
        // Players
        public static Archer Archer;
        public static Knight Knight;

        public static List<Enemy> Enemies;

        public static Camera2D Camera;

        public static Chain Chain;
        private static Texture2D _backgroundTexture;
        
        private static Random random = new Random();
        

        public static Vector2 MousePos => new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

#if DEBUG
        public static Texture2D mousepostest;
#endif

        /// <summary>
        /// The area in _backgroundTexture that is ground
        /// </summary>
        public static Rectangle PlayArea => new Rectangle(32, 272, _backgroundTexture.Width - 64, _backgroundTexture.Height - 272);

        public static Vector2 GetOtherPlayerPosition(Vector2 yourPosition)
        {
            if (yourPosition == Archer.CollidableObject.Position)
            {
                return Knight.CollidableObject.Position;
            }
            else if (yourPosition == Knight.CollidableObject.Position)
            {
                return Archer.CollidableObject.Position;
            }
            else
            {
                throw new ArgumentException();
            }
        }


        public static void LoadContent(ContentManager content, Viewport viewport)
        {
            Camera = new Camera2D(viewport);

            _backgroundTexture = content.Load<Texture2D>(@"Textures/WorldBackground");

            LoadPlayers(content);
            LoadEnemies(content);

            Chain = new Chain(content.Load<Texture2D>(@"Textures/Chain"));

            mousepostest = content.Load<Texture2D>(@"Textures/Chain");
        }

        private static void LoadPlayers(ContentManager content)
        {
            // Load archer spritesheet
            Texture2D archerTexture = content.Load<Texture2D>(@"Textures/ArcherSpriteSheet");
            // Load archer animations
            List<Animation> archerAnimations = new List<Animation>()
            {
                new Animation(AnimationStates.Idle.ToString() + AnimationDirections.Down.ToString(), new List<Frame>()
                {
                    new Frame(new Rectangle(0,0,100,100), new Vector2(0,0), Int32.MaxValue)
                })
            };
            // Load arrow Texture
            Texture2D arrowTexture = content.Load<Texture2D>(@"Textures/ArcherSpriteSheet");
            // Create archer with archer sprite sheet, arrow texture and archer 
            Archer = new Archer(archerTexture, arrowTexture, archerAnimations);

            Texture2D knightTexture = content.Load<Texture2D>(@"Textures/KnightSpriteSheet");
            List<Animation> knightAnimations = new List<Animation>()
            {
                new Animation(AnimationStates.Idle.ToString() + AnimationDirections.Down.ToString(), new List<Frame>()
                {
                    new Frame(new Rectangle(0,0,100,100), Int32.MaxValue)
                })
            };

            Knight = new Knight(knightTexture, knightAnimations);
        }

        private static void LoadEnemies(ContentManager content)
        {
            Enemies = new List<Enemy>();
            Goblin.LoadContent(content);
            Enemies.Add(new Goblin(SpawnAreas.Left));
            Enemies.Add(new Goblin(SpawnAreas.Middle));
            Enemies.Add(new Goblin(SpawnAreas.Right));
        }

        public static void Update(GameTime gameTime)
        {
            Archer.Update(gameTime);
            Knight.Update(gameTime);
            UpdateChainAndCamera();
            ClampMouse();
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Enemies.Add(new Goblin(SpawnAreas.Left));
                Enemies.Add(new Goblin(SpawnAreas.Middle));
                Enemies.Add(new Goblin(SpawnAreas.Right));
            }
            else
#endif
                foreach (Enemy enemy in Enemies)
                {
                    enemy.Update(gameTime);
                }
        }

        private static void UpdateChainAndCamera()
        {
            Chain.StartPosition = Archer.CollidableObject.Position;
            Chain.EndPosition = Knight.CollidableObject.Position;

            // Update camera location while clamping to bounds of _backgroundTexture.Height and interpolating between old and new position
            Camera.Location =
                // Clamp to background bounds
                Vector2.Clamp(
                    // Interpolate to smoothen movement
                    Vector2.SmoothStep(
                        // Vector to interpolate
                        Camera.Location,
                        // Location to interpolate camera to.
                        new Vector2(
                            // Unit vector from the chains rotation
                            (float)Math.Cos(Chain.Rotation), (float)Math.Sin(Chain.Rotation))
                        // multiplied by half of chains length
                        * (Chain.Length / 2f)
                        // plus the chain´s stating position
                        + Chain.StartPosition,
                        // Interpolation speed
                        0.2f),
                    // Min
                    Camera.ZoomedOrigin,
                    // Max
                    new Vector2(_backgroundTexture.Width - Camera.ZoomedOrigin.X, _backgroundTexture.Height - Camera.ZoomedOrigin.Y));

        }

        private static void ClampMouse()
        {
            if (Camera.CameraWorldRect.Contains(new Point((int)MousePos.X, (int)MousePos.Y)) == false)
            {
                Mouse.SetPosition(
                    (int)MathHelper.Clamp(MousePos.X, (float)Camera.CameraWorldRect.Left, (float)Camera.CameraWorldRect.Right),
                    (int)MathHelper.Clamp(MousePos.Y, (float)Camera.CameraWorldRect.Top, (float)Camera.CameraWorldRect.Bottom)); 
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            // Begin spriteBatch with the camera transform
            // Here the drawings effected by the camera shall be put (Enemies, player, etc.)
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.TransformMatrix);

            // Draw World
            spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
            // Draw Enemies
            foreach (Enemy enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }
            spriteBatch.End();
            // Draw chain between players
            Chain.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.TransformMatrix);
            // Draw Player
            Archer.Draw(spriteBatch);
            Knight.Draw(spriteBatch);

            spriteBatch.Draw(mousepostest, MousePos, null, Color.Wheat, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);

            spriteBatch.End();

            // Begin new spriteBatch without a transform
            // Here things not effected by the camera shall be put (UI etc.)
            spriteBatch.Begin();
            // Draw UI
#if DEBUG
            spriteBatch.DrawString(Game1.DebugFont,
                $" Camera:{Camera.Location}\n Archer:{Archer.CollidableObject.Position}\n Knight:{Knight.CollidableObject.Position}\n Mouse:{MousePos}\n ArchAniIdentifier:{Archer._animationSet.AnimationState.ToString() + Archer._animationSet.AnimationDirection}\n KnigAniIdentifier:{Knight._animationSet.AnimationState.ToString() + Knight._animationSet.AnimationDirection}  Arrows:{Archer.arrows.Count}",
                Vector2.Zero,
                Color.White);
#endif

            spriteBatch.End();
        }
    }
}
