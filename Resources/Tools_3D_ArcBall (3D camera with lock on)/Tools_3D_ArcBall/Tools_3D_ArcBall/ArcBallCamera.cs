using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_3D_ArcBall
{
    /// <summary>
    /// Class that is used to create a new camera with a lock-on component that fixes its view onto an object 
    /// </summary>
    public class ArcBallCamera : Camera
    {
        /// <summary>
        /// Used to rotate the camera in the X axis by a set amount
        /// </summary>
        public float RotationX { get; set; }

        /// <summary>
        /// Used to rotate the camera in the Y axis by a set amount
        /// </summary>
        public float RotationY { get; set; }

        /// <summary>
        /// Sets the minimum amount of available rotation for use in the Y axis for the camera 
        /// </summary>
        public float MinRotationY { get; set; }

        /// <summary>
        /// Sets the maximum amount of available rotation for use in the Y axis for the camera 
        /// </summary>
        public float MaxRotationY { get; set; }

        /// <summary>
        /// Used to set the amount of distance between an object and a camera 
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// Sets the minimum amount of distance that a target can be located in accordance to the camera 
        /// </summary>
        public float MinDistance { get; set; }

        /// <summary>
        /// Sets the maximum amount of distance that a target can be located in accordance to the camera 
        /// </summary>
        public float MaxDistance { get; set; }

        /// <summary>
        /// The position of an object in the world space 
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// The rotation of an object in X, Y and Z axis to be able make the object look in one direction  
        /// </summary>
        public Vector3 Target { get; set; }

        /// <summary>
        /// Creates a new instance of a camera with a lock on target, rotation X, rotation Y, minimum rotation Y, max rotation y, distance from target, minimum distance, maximum distance, and rendering for the camera 
        /// </summary>
        /// <param name="Target">The position which the camera is looking</param>
        /// <param name="RotationX">The amount of rotation that the camera starts with and can change with in the X axis</param>
        /// <param name="RotationY">The amount of rotation that the camera starts with and can change with in the Y axis</param>
        /// <param name="MinRotationY">The set minimum amount of rotation that the camera can perform every time it is rotated in the Y axis</param>
        /// <param name="MaxRotationY">The set maximum amount of rotation that the camera can perform every time it is rotated in the Y axis</param>
        /// <param name="Distance">The set amount of initial distance between a target and the camera</param>
        /// <param name="MinDistance">The minimum distance that the camera can have between the set target</param>
        /// <param name="MaxDistance">The maximum distance that the camera can have between the set target</param>
        /// <param name="graphicsDevice">The graphics device used to perform rendering for the various objects</param>
        /// <param name=""></param>
        public ArcBallCamera(Vector3 Target, float RotationX, float RotationY, float MinRotationY, float MaxRotationY,
            float Distance, float MinDistance, float MaxDistance, GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            // Equals the variable outside of the constructor to the variable inside the parameters of the constructor, so its logic in the class can be inherited by the variable in the parameter
            this.Target = Target;
            
            this.MinRotationY = MinRotationY;
            this.MaxRotationY = MaxRotationY;                        
            
            this.RotationX = RotationX;
           
            this.MinDistance = MinDistance;
            this.MaxDistance = MaxDistance;

            // Sets the variables to a minimum and a max capacity so that an object cannot perform an action to little or too much 
            this.RotationY = MathHelper.Clamp(RotationY, MinRotationY, MaxRotationY);
            this.Distance = MathHelper.Clamp(Distance, MinDistance, MaxDistance);


        }

        /// <summary>
        /// Sets the amount of distance that the camera is supposed is supposed to move in one direction in accordance to the target
        /// </summary>
        /// <param name="DistanceChange">The amount of distance that should be changed</param>
        public void Move(float DistanceChange)
        {
            // Sets distance equal to the distance change towards or from the target 
            this.Distance += DistanceChange;

            // Clamps the distance for the camera between a minimum and a maximum amount
            this.Distance = MathHelper.Clamp(Distance, MinDistance, MaxDistance);

        }

        /// <summary>
        /// Rotates the camera in the X and Y axis. The Y axis is clamped between min rotation Y and max rotation Y 
        /// </summary>
        /// <param name="RotationXChange">The amount of camera rotation in the X axis</param>
        /// <param name="RotationYChange">The amount of camera rotation in the Y axis</param>
        public void Rotate(float RotationXChange, float RotationYChange)
        {
            // Makes the rotation for the camera in the X or Y axis change in accordance to the rotation change 
            this.RotationX += RotationXChange;
            this.RotationY += RotationYChange;

            // Clamps the Y rotation for the camera between a minimum and a maximum amount 
            this.RotationY = MathHelper.Clamp(RotationY, MinRotationY, MaxRotationY);
        }


        /// <summary>
        /// Translates the position of the camera in the X, Y or Z axis 
        /// </summary>
        /// <param name="PositionChange">The direction and amount of change that the camera travels in the X, Y and Z axis</param>
        public void Translate(Vector3 PositionChange)
        {
            // Sets the position of the camera to the amount of position change in one or more directions  
            this.Position += PositionChange;
        }

        /// <summary>
        /// The update for the arcball class which overrides the camera update in camera.cs 
        /// </summary>
        public override void Update()
        {
            // Creates a rotation matrix that keeps track of the rotation of the camera 
            Matrix rotation = Matrix.CreateFromYawPitchRoll(RotationX, -RotationY, 0);

            // Creates a new 3D vector that translates the position of the camera   
            Vector3 translation = new Vector3(0, 0, Distance);

            // Transforms the created 3D vector by the rotation matrix 
            translation = Vector3.Transform(translation, rotation);

            // Makes the position of the camera equal to the direction which it is looking plus the position and rotation of the camera
            Position = Target + translation;

            // Makes the up in the world become the cameras top side 
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Creates a new instance of the various camera logic that equals the position, target, and the up in the world for the camera 
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}

