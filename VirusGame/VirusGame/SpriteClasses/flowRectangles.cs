using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses
{
    /// <summary>
    /// Flow Rectangle is so we can create a rectangle that will produce
    /// a "gravity" or "flow" in the game. To give resistance or momentum.
    /// </summary>
    public class flowRectangles
    {
        #region Declaration

        public Rectangle rectangle;
        private Vector2 flowVelocity;

        #endregion

        /// <summary>
        /// flow rectangles used to simulate a current/gravity
        /// </summary>
        /// <param name="_rect">rectangle</param>
        /// <param name="_flowVelocity">velocity of current/gravity</param>
        public flowRectangles(Rectangle _rect, Vector2 _flowVelocity)
        {
            rectangle = _rect;
            flowVelocity = _flowVelocity;
        }

        /// <summary>
        /// returns the flowVelocity.
        /// </summary>
        public Vector2 FlowVelocity
        {
            get { return flowVelocity; }
        }
    }
}
