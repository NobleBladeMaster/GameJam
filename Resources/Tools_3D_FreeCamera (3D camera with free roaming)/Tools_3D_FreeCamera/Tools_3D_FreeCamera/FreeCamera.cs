using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace Tools_3D_FreeCamera
{
    /// <summary>
    /// Class that is used to create a new free-roaming camera  
    /// </summary>
    public class FreeCamera : Camera
    {

        /// <summary>
        /// The Yaw (Y) rotation for the camera
        /// </summary>
        public float Yaw { get; set; }

        /// <summary>
        /// The Pitch (X) rotation for the camera
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// The position for the camera
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The direction that the camera is looking
        /// </summary>
        public Vector3 Target { get; set; }

        /// <summary>
        /// The position change for the camera 
        /// </summary>
        private Vector3 translation;

        /// <summary>
        /// Creates a new camera with no lock-on with a position, Yaw (Y) rotation, Pitch (X) rotation, and the rendering for the camera  
        /// </summary>
        /// <param name="Position">The position of the camera</param>
        /// <param name="Yaw">The amount of Yaw (Y) rotation the camera currently has</param>
        /// <param name="Pitch">The amount of Pitch (X) rotation the camera currently has</param>
        /// <param name="graphicsDevice">The rendering for the camera</param>
        public FreeCamera(Vector3 Position, float Yaw, float Pitch, GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            // Equals the variable outside of the constructor to the variable inside the parameters of the constructor, so its logic in the class can be inherited by the variable in the parameter 
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;

            // Sets all of the translations 3D components to zero
            translation = Vector3.Zero;
        }

        /// <summary>
        /// The rotation for the camera in the X and Y axis 
        /// </summary>
        /// <param name="YawChange">The current amount of change in the Yaw (Y) rotation</param>
        /// <param name="PitchChange">The current amount of change in the Pitch (X) rotation</param>
        public void Rotate(float YawChange, float PitchChange)
        {
            // Increases or decreases the floats value according to the change set for the values 
            this.Yaw += YawChange;
            this.Pitch += PitchChange; 

        }

        /// <summary>
        /// Moves the free camera according to the set translation value 
        /// </summary>
        /// <param name="Translation">The amount of position change</param>
        public void Move(Vector3 Translation)
        {
            // Sets the global translation to the parameter translation 
            this.translation += Translation; 
        }

        /// <summary>
        /// The update for the free camera class which overrides the camera update in camera.cs 
        /// </summary>
        public override void Update()
        {
            // Creates a rotation matrix that keeps track of the rotation of the camera in the Y and X axis  
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);

            // Creates a new 3D vector that translates the position of the camera
            translation = Vector3.Transform(translation, rotation);

            // Sets the position of the camera too the translation of it 
            Position += translation;

            // Sets all of the translations 3D components to zero
            translation = Vector3.Zero;

            // Creates a new 3D vector which is transformed into the forward direction and the rotation matrix 
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);

            // Sets the direction the camera is facing to the position of the camera and the forward direction (with rotation) 
            Target = Position + forward;

            // Creates a new 3D which is transformed into the up rotation for the camera with its rotation in mind 
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);
            
            // Creates a view matrix which has the position of the camera, and a target for the cameras direction (with the up direction defined) 
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
