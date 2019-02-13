using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace Tools_3D_ArcBall
{
    /// <summary>
    /// Used to create a new 3D model with various properties 
    /// </summary>
    public class CustomModel
    {
        /// <summary>
        /// The 3D position in the world for the model 
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>-
        /// The rotation for the 3D model regarding the yaw (Y), pitch (X) and roll (Z) 
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// The 3D scale for the model in the world 
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// The model which represents a 3D objects various information to use for creating a custom model 
        /// </summary>
        public Model Model { get; private set; }

        /// <summary>
        /// Creates a matrix used to keep track of all the transformation aspects regarding the models (rotation, scale and position) 
        /// </summary>
        private Matrix[] modelTransforms;

        /// <summary>
        /// Used to perform the rendering for all of the individual models 
        /// </summary>
        private GraphicsDevice graphicsDevice; 

        /// <summary>
        /// Creates a new 3D object that has a model, in world-position, a rotation, a scale for the object, and rendering for the object  
        /// </summary>
        /// <param name="Model">The model that represents a 3D objects various information</param>
        /// <param name="Position">The in-world coordinates for the 3D object</param>
        /// <param name="Rotation">The yaw, pitch and roll for the 3D object</param>
        /// <param name="Scale">The chosen size of the 3D object</param>
        /// <param name="graphicsDevice">Performs the basic rendering for the object</param>
        public CustomModel(Model Model, Vector3 Position, Vector3 Rotation, Vector3 Scale, GraphicsDevice graphicsDevice)
        {
            // Creates a new instance in the matrix that keeps track of all the various bones (a collection of nodes which connect to some nodes to rotate parts of a model) 
            modelTransforms = new Matrix[Model.Bones.Count];

            // Copies all of the objects set bones (a bunch of nodes with lines in-between some of them to rotate specific parts of an object) information to the model transformation  
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            // Equals the variable outside of the constructor to the variable inside the parameters of the constructor, so its logic in the class can be inherited by the variable in the parameter 
            this.Model = Model;
         
            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;

            this.graphicsDevice = graphicsDevice; 
        }

        /// <summary>
        /// Translates all of the 3D information in the view and projection matrix onto the 2D screen  
        /// </summary>
        /// <param name="View">The view matrix for the method</param>
        /// <param name="Projection">The projection matrix for the method</param>
        public void Draw(Matrix View, Matrix Projection)
        {
            // Creates a world matrix that contains an objects information in the world, with a scale which is timed to the rotation of an object, and timed to the position of an object 
            Matrix baseWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);

            // For every mesh in a model (a mesh is a collection of vertices, edges, and faces that describe the shape of a 3D object) 
            foreach (ModelMesh mesh in Model.Meshes)
            {
                // Transform the models mesh bones onto a coordinate in the world times the base world 
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld; 

                // For each part in a mesh 
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Sets effect that is used to renders each mesh part in a 3D object 
                    BasicEffect effect = (BasicEffect)meshPart.Effect;

                    // Renders all of the various information regarding all of the 3D objects in the world, view and projection matrix 
                    effect.World = localWorld;
                    effect.View = View;
                    effect.Projection = Projection;

                    // Enables a lightning effect 
                    effect.EnableDefaultLighting();
                }

                // Draws all of the various meshes that are stored in the world 
                mesh.Draw();
            }
        }
    }
}
