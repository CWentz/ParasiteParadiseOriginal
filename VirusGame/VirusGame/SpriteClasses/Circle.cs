using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame.SpriteClasses
{
    public class Circle
    {

        #region Declarations 

        public Vector2 position;
        public bool added = false;
        public float radius;
        private Rectangle qtRect;
        

        #endregion

        public Rectangle Rect
        {
            get { return qtRect; }
            set { qtRect = value; }
        }


        /// <summary>
        /// circle with the position and radius
        /// </summary>
        /// <param name="_radius">radius</param>
        /// <param name="_position">position</param>
        public Circle(float _radius, Vector2 _position)
        {
            position = _position;
            radius = _radius;
            
        }

        /// <summary>
        /// updates the position of the circle
        /// </summary>
        /// <param name="_position">position</param>
        public void Update(Vector2 _position)
        {
            position = _position;
            qtRect = new Rectangle((int)(position.X - radius * 1.5f), (int)(position.Y - radius * 1.5f), (int)(radius * 3), (int)(radius * 3));

        }

        /// <summary>
        /// collision detection between circles
        /// </summary>
        /// <param name="target">circle to test against</param>
        /// <returns>true if collision occurs</returns>
        public bool collision(Circle target)
        {
            if (Vector2.Distance(position, target.position) < target.radius + radius)
                return true;
            else return false;
        }


        /// <summary>
        /// calculates the distance between 2 points and moves the object in the
        /// opposite direction of the circles center.
        /// </summary>
        /// <param name="_object">position of object you wish to move</param>
        /// <returns>Vector2</returns>
        public Vector2 pushFromCenter(Vector2 _object)
        {
            _object.X += (_object.X - position.X) / 10;
            _object.Y += (_object.Y - position.Y) / 10;

            return _object;
        }


        /// <summary>
        /// Get/Set the radius value.
        /// </summary>
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }
    }

    
}
