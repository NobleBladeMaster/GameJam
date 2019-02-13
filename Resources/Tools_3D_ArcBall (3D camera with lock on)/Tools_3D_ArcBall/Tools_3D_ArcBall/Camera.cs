using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace Tools_3D_ArcBall
{
    /// <summary>
    /// Used to create a camera with various properties that renders 3D objects onto the 2D screen
    /// </summary>
    public abstract class Camera
    {
        
        /// <summary>
        /// The view matrix that is used as a camera to show objects using a orthographic view 
        /// </summary>
        Matrix view;

        /// <summary>
        /// The projection matrix that is used to combine the view and world matrix and project the 3D objects and render them onto the 2D computer screen
        /// </summary>
        Matrix projection;

        /// <summary>
        /// The projection matrix for the camera 
        /// </summary>
        public Matrix Projection
        {           
            // Gets the projection matrix values
            get { return projection; }
            
            // Sets the value in this and any derived class 
            protected set
            {
                // Sets the value of the projection matrix
                projection = value;

                // Generates the frustum for the camera 
                generateFrustum();
            }
        }

        /// <summary>
        /// The view matrix for the camera 
        /// </summary>
        public Matrix View
        {           
            // Gets the view matrix values 
            get { return view; }

            // Sets the value in this and any derived class
            protected set
            {                
                // Sets the value of the view matrix 
                view = value;

                // Generates the frustum for the camera  
                generateFrustum(); 
            }
        }

        /// <summary>
        /// The area between the near and far plane 
        /// </summary>
        public BoundingFrustum Frustum { get; private set; }

        /// <summary>
        /// Used to perform the rendering for various objects
        /// </summary>
        protected GraphicsDevice GraphicsDevice { get; set; }

        /// <summary>
        /// The constructor for the class that generates the view 
        /// </summary>
        /// <param name="graphicsDevice">The rendering device for the object</param>
        public Camera(GraphicsDevice graphicsDevice)
        {            
            // Sets the global variable to the method variable 
            this.GraphicsDevice = graphicsDevice;

            // Generates the perspective of the projection matrix with a field-of-view set to pi over 4 (pi divided by four) 
            generatePerspectiveProjectionMatrix(MathHelper.PiOver4);
        }
          
        /// <summary>
        /// Generates the perspective of the camera 
        /// </summary>
        /// <param name="FieldOfView">The field of view that the player is supposed to view in-game</param>
        private void generatePerspectiveProjectionMatrix(float FieldOfView)
        {           
            // Renders an object with a graphics device within its presentation parameters 
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            // Defines the aspect ratio as the width divided by the height 
            float aspectRatio = (float)pp.BackBufferWidth / (float)pp.BackBufferHeight;

            // Used to generate the projection regarding the field-of-view, the aspect ratio, and the near and far plane
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000000.0f);
        }

        /// <summary>
        /// The update for the class that is used to insert various values in here 
        /// </summary>
        public virtual void Update()
        {


        }

        /// <summary>
        /// Generates the distance in-between the near and far plane
        /// </summary>
        private void generateFrustum()
        {
            // Multiplies the view and projection matrix to define the area in-between the near and far plane 
            Matrix viewProjection = View * Projection;

            // Creates a new frustum that is dependent on the set view and projection properties
            Frustum = new BoundingFrustum(viewProjection);
        }

        /// <summary>
        /// Returns a value if the cameras frustum overlaps with a sphere 
        /// </summary>
        /// <param name="sphere">The sphere object</param>
        /// <returns></returns>
        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            // Return a value if the frustum contains the sphere
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        /// <summary>
        /// Returns a value if the cameras frustum overlaps with a box 
        /// </summary>
        /// <param name="box">The box object</param>
        /// <returns></returns>
        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            // Return a value if the frustum contains the box 
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }
}
