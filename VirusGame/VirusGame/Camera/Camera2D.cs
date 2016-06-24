using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame.Camera
{
    public class Camera2D
    {

        #region Declarations

        private Vector2 cameraClampMax = new Vector2(535, 415);
        private Vector2 cameraClampMin = new Vector2(-535, -415);
        private Matrix transform;
        private Vector2 center;
        private Viewport viewport;
        private float zoom = 1;
        private float rotation = 0;

        #endregion


        /// <summary>
        /// Constructor creates camera needs a viewport
        /// </summary>
        /// <param name="_viewport">viewport</param>
        public Camera2D(Viewport _viewport)
        {
            viewport = _viewport;
        }

        #region Get/Sets

        public Matrix Transform
        {
            get { return transform; }
        }

        public float X
        {
            get { return center.X; }
            set { center.X = value; }
        }

        public float Y
        {
            get { return center.Y; }
            set { center.Y = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; 
                if (zoom < 0.1f) //sets so zoom will not zoom too far.
                    zoom = 0.1f; 
            }
        }

        
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        #endregion

        public Matrix camSimUnit;
        public void Update(Vector2 _position)
        {
            center = new Vector2(_position.X, _position.Y);

            if (center.Y < cameraClampMin.Y)    // * (Math.Pow((double)zoom, 6.0)))
                center.Y = cameraClampMin.Y;   // * (float)(Math.Pow((double)zoom, 6.0));
            if (center.Y > cameraClampMax.Y)    // * (Math.Pow((double)zoom, 6.0)))
                center.Y = cameraClampMax.Y;   // * (float)(Math.Pow((double)zoom, 6.0));

            if (center.X < cameraClampMin.X)    // * (Math.Pow((double)zoom, 6.0)))
                center.X = cameraClampMin.X;   // * (float)(Math.Pow((double)zoom, 6.0));
            if (center.X > cameraClampMax.X)    // * (Math.Pow((double)zoom, 6.0)))
                center.X = cameraClampMax.X;   // * (float)(Math.Pow((double)zoom, 6.0));
            Globals.CameraPosition = center;

            transform = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
                Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));

            camSimUnit = Matrix.CreateTranslation(new Vector3((float)ConvertUnits.ToSimUnits(-center.X), (float)ConvertUnits.ToSimUnits(-center.Y), 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
                Matrix.CreateTranslation(new Vector3((float)ConvertUnits.ToSimUnits(viewport.Width / 2), (float)ConvertUnits.ToSimUnits(viewport.Height / 2), 0));

            
            
        }

    }

}
