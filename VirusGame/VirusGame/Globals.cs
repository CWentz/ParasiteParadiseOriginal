using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame
{
    public static class Globals
    {
        private static Texture2D pixel;
        private static Vector2 worldPosition;
        private static Vector2 displayPosition;
        private static float globalScale = 1f;
        private static Vector2 cameraPosition;
        private static SpriteClasses.TextureManager textureManager;
        public static Texture2D particles;

        /// <summary>
        /// pixel is used for debugging.
        /// </summary>
        public static Texture2D Pixel
        {
            get
            {
                return pixel;
            }
            set
            {
                pixel = value;
            }
        }

        public static Vector2 CameraPosition
        {
            get
            {
                return cameraPosition;
            }
            set
            {
                cameraPosition = value;
            }
        }

        public static float GlobalScale
        {
            set { globalScale = value; }
            get { return globalScale; }
        }

        
        public static SpriteClasses.TextureManager TextureManager
        {
            set { textureManager = value; }
            get { return textureManager; }
        }

        
        /// <summary>
        /// Give a display position and it will return the world position.
        /// </summary>
        /// <param name="_targetsDisplayPosition"></param>
        /// <returns></returns>
        public static Vector2 getWorldPosition(Vector2 _targetsDisplayPosition)
        {
            worldPosition = new Vector2(ConvertUnits.ToSimUnits(_targetsDisplayPosition.X), ConvertUnits.ToSimUnits(_targetsDisplayPosition.Y));
            return worldPosition;
        }


        /// <summary>
        /// give the world position and it will return the display position.
        /// </summary>
        /// <param name="_targetsWorldPosition"></param>
        /// <returns></returns>
        public static Vector2 getDisplayPosition(Vector2 _targetsWorldPosition)
        {
            displayPosition = new Vector2(ConvertUnits.ToDisplayUnits(_targetsWorldPosition.X), ConvertUnits.ToDisplayUnits(_targetsWorldPosition.Y));
            return displayPosition;
        }

    }
}
