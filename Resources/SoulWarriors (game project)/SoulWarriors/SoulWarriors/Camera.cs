using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoulWarriors
{
    public class Camera2D
    {
        private readonly Viewport _viewport;

        public float Zoom { get; set; } = 1f;
        public Vector2 Location { get; set; } = Vector2.Zero;
        public float Rotation { get; set; } = 0f;
        public Vector2 Origin { get; private set; }

        /// <summary>
        /// The rectangle of what the camera shows
        /// </summary>
        public Rectangle CameraWorldRect => new Rectangle(
            Convert.ToInt32(Location.X - Origin.X / Zoom),
            Convert.ToInt32(Location.Y - Origin.Y / Zoom),
            Convert.ToInt32(_viewport.Width / Zoom),
            Convert.ToInt32(_viewport.Height / Zoom));

        /// <summary>
        /// the origin coordinate manipulated by the zoom factor
        /// </summary>
        public Vector2 ZoomedOrigin => Origin / Zoom;

        public Matrix TransformMatrix =>  Matrix.CreateTranslation(new Vector3(-Location.X, -Location.Y, 0)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom) *
            Matrix.CreateTranslation(new Vector3(Origin, 0));

        public Camera2D(Viewport viewport)
        {
            Origin = new Vector2(viewport.Width, viewport.Height) / 2f;
            _viewport = viewport;
        }
    }
}
