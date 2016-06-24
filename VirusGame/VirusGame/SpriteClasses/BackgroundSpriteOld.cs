using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VirusGame.SpriteClasses
{
    /// <summary>
    /// Background sprite is for a parallax scrolling background.
    /// The backgrounds are formed in a grid of 3x3 tiles.
    /// In the center of the grid is the viewport.
    /// </summary>
    public class BackgroundSprite : SpriteClasses.BaseSprite
    {
        float venesSpeed = 0.5f;
        Texture2D venesTexture;
        #region Declaration

        //for changing direction of the scrolling on the X or Y axis.
        protected Vector2 xDirection = new Vector2(-1, 0);
        protected Vector2 yDirection = new Vector2(0, -1);

        //scroll speed variables for the background
        public float speed;
        public float speedConstant;
        public float screenWidth;
        public float screenHeight;

        private bool movingUp;
        private bool movingDown;
        private bool movingLeft;
        private bool movingRight;



        #endregion


        #region image position variables

        private Vector2 venesPosition;

        //positions of the images top row.
        private Vector2 positionImage1;
        private Vector2 positionImage2;
        private Vector2 positionImage3;

        //positions of the images center row.
        private Vector2 positionImage4;
        private Vector2 positionImage5;
        private Vector2 positionImage6;

        //positions of the images bottom row.
        private Vector2 positionImage7;
        private Vector2 positionImage8;
        private Vector2 positionImage9;

        #endregion


        /// <summary>
        /// Creates a BG sprite for parallax scrolling square of 3x3.
        /// </summary>
        /// <param name="sprite">basesprite for texture and position.</param>
        /// <param name="_speed">float value for the scrolling speed.</param>
        /// <param name="_speedConstant">float value for extra constant scrolling speed</param>
        public BackgroundSprite(Texture2D _texture, Texture2D _venesTexture, Vector2 _position, float _speed, float _speedConstant, float _screenWidth, float _screenHeight)
        {
            speed = _speed;
            speedConstant = _speedConstant;
            texture = _texture;
            venesTexture = _venesTexture;
            position = _position;
            screenHeight = _screenHeight;
            screenWidth = _screenWidth;

            venesPosition = new Vector2(-1024, -1024);
            //top row
            positionImage1 = new Vector2(position.X - texture.Width, position.Y - texture.Height);
            positionImage2 = new Vector2(position.X, position.Y - texture.Height);
            positionImage3 = new Vector2(position.X + texture.Width , position.Y - texture.Height);

            //center row
            positionImage4 = new Vector2(position.X - texture.Width , position.Y);
            positionImage5 = position;
            positionImage6 = new Vector2(position.X + texture.Width, position.Y);

            //bottom row
            positionImage7 = new Vector2(position.X - texture.Width, position.Y + texture.Height);
            positionImage8 = new Vector2(position.X, position.Y + texture.Height);
            positionImage9 = new Vector2(position.X + texture.Width, position.Y + texture.Height);



        }

        //public BackgroundSprite(Texture2D _texture, Vector2 _position, float _speed, float _speedConstant, float _screenWidth, float _screenHeight, Vector2 _direction)
        //{
        //    speed = _speed;
        //    speedConstant = _speedConstant;
        //    texture = _texture;
        //    position = _position;
        //    screenHeight = _screenHeight;
        //    screenWidth = _screenWidth;



        //    positionImage5 = position;



        //}


        /// <summary>
        /// Moves the bg in a positive horizontal position(left).
        /// </summary>
        /// <param name="_timeDelta">gameTime total elapsed seconds.</param>
        public void Update(GameTime gameTime, Controls cont, Vector2 _centerPosition)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (cont.moveRight())
            {
                movingLeft = false;
                movingRight = true;
                venesPosition -= new Vector2(0.5f, 0);
            }
            else if (cont.moveLeft())
            {
                movingLeft = true;
                movingRight = false;
                venesPosition += new Vector2(0.5f, 0);
            }
            else if (cont.moveUp())
            {
                movingUp = true;
                movingDown = false;
                venesPosition += new Vector2(0, 0.5f);
            }
            else if (cont.moveDown())
            {
                movingUp = false;
                movingDown = true;
                venesPosition -= new Vector2(0, 0.5f);
            }

            //else
            //{
            //    movingUp = false;
            //    movingDown = false;
            //}

            /*
            if (movingUp)
                moveUp(timeDelta);
            if (movingDown)
                moveDown(timeDelta);
            if (movingLeft)
                moveLeft(timeDelta);
            if (movingRight)
                moveRight(timeDelta);

             */

            #region BG Repeater Right Movement

            if (positionImage1.X < (-texture.Width / 2) + _centerPosition.X - (texture.Width))
            {
                positionImage1.X = positionImage1.X + (texture.Width * 2);
                positionImage4.X = positionImage4.X + (texture.Width * 2);
                positionImage7.X = positionImage7.X + (texture.Width * 2);
            }

            if (positionImage2.X < (-texture.Width / 2) + _centerPosition.X - (texture.Width))
            {
                positionImage2.X = positionImage2.X + (texture.Width * 2);
                positionImage5.X = positionImage5.X + (texture.Width * 2);
                positionImage8.X = positionImage8.X + (texture.Width * 2);
            }


            if (positionImage3.X < (-texture.Width / 2) + _centerPosition.X - (texture.Width))
            {
                positionImage3.X = positionImage3.X - (texture.Width * 2);
                positionImage6.X = positionImage6.X - (texture.Width * 2);
                positionImage9.X = positionImage9.X - (texture.Width * 2);
            }

            #region w/o camera old code
            /////////////////////////////////////////////////////////
            //////////////////////  second row  //////////////////////
            /////////////////////////////////////////////////////////

            //if (positionImage4.X < -1 * (texture.Width))
            //{
            //    positionImage4.X = positionImage4.X + (texture.Width * 2);
            //}

            //if (positionImage5.X < -1 * (texture.Width))
            //{
            //    positionImage5.X = positionImage5.X + (texture.Width * 2);
            //}
            //if (positionImage6.X < -1 * (texture.Width))
            //{
            //    positionImage6.X = positionImage6.X - (texture.Width * 2);
            //}

            /////////////////////////////////////////////////////////
            //////////////////////  third row  //////////////////////
            /////////////////////////////////////////////////////////

            //if (positionImage7.X < -1 * (texture.Width))
            //{
            //    positionImage7.X = positionImage7.X + (texture.Width * 2);
            //}

            //if (positionImage8.X < -1 * (texture.Width))
            //{
            //    positionImage8.X = positionImage8.X + (texture.Width * 2);
            //}
            //if (positionImage9.X < -1 * (texture.Width))
            //{
            //    positionImage9.X = positionImage9.X - (texture.Width * 2);
            //}
            #endregion
            #endregion

            #region BG Repeater Left Movement


            if (positionImage1.X > _centerPosition.X - (texture.Width / 2) + (texture.Width))
            {
                positionImage1.X = positionImage1.X - (texture.Width * 2);
                positionImage4.X = positionImage4.X - (texture.Width * 2);
                positionImage7.X = positionImage7.X - (texture.Width * 2);
            }

            if (positionImage2.X > _centerPosition.X - (texture.Width / 2) + (texture.Width))
            {
                positionImage2.X = positionImage2.X - (texture.Width * 2);
                positionImage5.X = positionImage5.X - (texture.Width * 2);
                positionImage8.X = positionImage8.X - (texture.Width * 2);
            }

            if (positionImage3.X > _centerPosition.X - (texture.Width / 2) + (texture.Width))
            {
                positionImage3.X = positionImage3.X - (texture.Width * 2);
                positionImage6.X = positionImage6.X - (texture.Width * 2);
                positionImage9.X = positionImage9.X - (texture.Width * 2);
            }
            #region w/o camera old code
            /////////////////////////////////////////////////////////
            //////////////////////  second row  //////////////////////
            /////////////////////////////////////////////////////////

            //if (positionImage4.X > (texture.Width))
            //{
            //    positionImage4.X = positionImage4.X - (texture.Width * 2);
            //}

            //if (positionImage5.X > (texture.Width))
            //{
            //    positionImage5.X = positionImage5.X - (texture.Width * 2);
            //}

            //if (positionImage6.X > (texture.Width))
            //{
            //    positionImage6.X = positionImage6.X - (texture.Width * 2);
            //}

            /////////////////////////////////////////////////////////
            //////////////////////  third row  //////////////////////
            /////////////////////////////////////////////////////////

            //if (positionImage7.X > (texture.Width))
            //{
            //    positionImage7.X = positionImage7.X - (texture.Width * 2);
            //}

            //if (positionImage8.X > (texture.Width))
            //{
            //    positionImage8.X = positionImage8.X - (texture.Width * 2);
            //}

            //if (positionImage9.X > (texture.Width))
            //{
            //    positionImage9.X = positionImage9.X - (texture.Width * 2);
            //}

            #endregion
            #endregion

            #region BG Repeater Up Movement


            if (positionImage1.Y < (_centerPosition.Y - (texture.Width / 2)) - (texture.Height))
            {
                positionImage1.Y = positionImage1.Y + (texture.Height * 2);
                positionImage2.Y = positionImage2.Y + (texture.Height * 2);
                positionImage3.Y = positionImage3.Y + (texture.Height * 2);
            }

            if (positionImage4.Y < (_centerPosition.Y - (texture.Width / 2)) - (texture.Height))
            {
                positionImage4.Y = positionImage4.Y + (texture.Height * 2);
                positionImage5.Y = positionImage5.Y + (texture.Height * 2);
                positionImage6.Y = positionImage6.Y + (texture.Height * 2);
            }
            if (positionImage7.Y < (_centerPosition.Y - (texture.Width / 2)) - (texture.Height * 2))
            {
                positionImage7.Y = positionImage7.Y + (texture.Height * 2);
                positionImage8.Y = positionImage8.Y + (texture.Height * 2);
                positionImage9.Y = positionImage9.Y + (texture.Height * 2);
            }

            #region old code w/o camera

            /////////////////////////////////////////////////////////
            //////////////////////  second row  //////////////////////
            /////////////////////////////////////////////////////////

            //if (positionImage4.Y < -1 * (texture.Height))
            //{
            //    positionImage4.Y = positionImage4.Y + (texture.Height * 2);
            //}

            //if (positionImage5.Y < -1 * (texture.Height))
            //{
            //    positionImage5.Y = positionImage5.Y + (texture.Height * 2);
            //}
            //if (positionImage6.Y < -1 * (texture.Height))
            //{
            //    positionImage6.Y = positionImage6.Y - (texture.Height * 2);
            //}

            /////////////////////////////////////////////////////////
            //////////////////////  third row  //////////////////////
            /////////////////////////////////////////////////////////

            //if (positionImage7.Y < -1 * (texture.Height))
            //{
            //    positionImage7.Y = positionImage7.Y + (texture.Height * 2);
            //}

            //if (positionImage8.Y < -1 * (texture.Height))
            //{
            //    positionImage8.Y = positionImage8.Y + (texture.Height * 2);
            //}
            //if (positionImage9.Y < -1 * (texture.Height))
            //{
            //    positionImage9.Y = positionImage9.Y - (texture.Height * 2);
            //}

            #endregion
            #endregion

            #region BG Repeater Down Movement

            if (positionImage1.Y > (_centerPosition.Y - (texture.Width / 2)) + (texture.Height))
            {
                positionImage1.Y = positionImage1.Y - (texture.Height * 2);
                positionImage2.Y = positionImage2.Y - (texture.Height * 2);
                positionImage3.Y = positionImage3.Y - (texture.Height * 2);
            }

            if (positionImage4.Y > (_centerPosition.Y - (texture.Width / 2)) + (texture.Height))
            {
                positionImage4.Y = positionImage4.Y - (texture.Height * 2);
                positionImage5.Y = positionImage5.Y - (texture.Height * 2);
                positionImage6.Y = positionImage6.Y - (texture.Height * 2);
            }
            if (positionImage7.Y > (_centerPosition.Y - (texture.Width / 2)) + (texture.Height * 2))
            {
                positionImage7.Y = positionImage7.Y - (texture.Height * 2);
                positionImage8.Y = positionImage8.Y - (texture.Height * 2);
                positionImage9.Y = positionImage9.Y - (texture.Height * 2);
            }

            #region old code w/o camera
            /////////////////////////////////////////////////////////
            //////////////////////  second row  //////////////////////
            /////////////////////////////////////////////////////////

            //if (positionImage4.Y > texture.Height)
            //{
            //    positionImage4.Y = positionImage4.Y - (texture.Height * 2);
            //}

            //if (positionImage5.Y > texture.Height)
            //{
            //    positionImage5.Y = positionImage5.Y - (texture.Height * 2);
            //}
            //if (positionImage6.Y > texture.Height)
            //{
            //    positionImage6.Y = positionImage6.Y + (texture.Height * 2);
            //}


            /////////////////////////////////////////////////////////
            //////////////////////  third row  //////////////////////
            /////////////////////////////////////////////////////////

            //if (positionImage7.Y > texture.Height)
            //{
            //    positionImage7.Y = positionImage7.Y - (texture.Height * 2);
            //}

            //if (positionImage8.Y > texture.Height)
            //{
            //    positionImage8.Y = positionImage8.Y - (texture.Height * 2);
            //}
            //if (positionImage9.Y > texture.Height)
            //{
            //    positionImage9.Y = positionImage9.Y + (texture.Height * 2);
            //}

            #endregion

            #endregion
        }


        #region MOVEMENT
        /// <summary>
        /// Moves the bg in a negative verticle position(up).
        /// </summary>
        /// <param name="_timeDelta">gameTime total elapsed seconds.</param>
        public void moveUp(float _timeDelta)
        {
            venesPosition += -1 * yDirection * venesSpeed * speedConstant * _timeDelta;

            positionImage1 += -1 * yDirection * speed * speedConstant * _timeDelta;
            positionImage2 += -1 * yDirection * speed * speedConstant * _timeDelta;
            positionImage3 += -1 * yDirection * speed * speedConstant * _timeDelta;
            positionImage4 += -1 * yDirection * speed * speedConstant * _timeDelta;
            positionImage5 += -1 * yDirection * speed * speedConstant * _timeDelta;
            positionImage6 += -1 * yDirection * speed * speedConstant * _timeDelta;
            positionImage7 += -1 * yDirection * speed * speedConstant * _timeDelta;
            positionImage8 += -1 * yDirection * speed * speedConstant * _timeDelta;
            positionImage9 += -1 * yDirection * speed * speedConstant * _timeDelta;
        }

        /// <summary>
        /// Moves the bg in a negative positive position(down).
        /// </summary>
        /// <param name="_timeDelta">gameTime total elapsed seconds.</param>
        public void moveDown(float _timeDelta)
        {
            venesPosition += yDirection * venesSpeed * speedConstant * _timeDelta;

            positionImage1 += yDirection * speed * speedConstant * _timeDelta;
            positionImage2 += yDirection * speed * speedConstant * _timeDelta;
            positionImage3 += yDirection * speed * speedConstant * _timeDelta;
            positionImage4 += yDirection * speed * speedConstant * _timeDelta;
            positionImage5 += yDirection * speed * speedConstant * _timeDelta;
            positionImage6 += yDirection * speed * speedConstant * _timeDelta;
            positionImage7 += yDirection * speed * speedConstant * _timeDelta;
            positionImage8 += yDirection * speed * speedConstant * _timeDelta;
            positionImage9 += yDirection * speed * speedConstant * _timeDelta;
        }

        /// <summary>
        /// Moves the bg in a negative horizontal position(right).
        /// </summary>
        /// <param name="_timeDelta">gameTime total elapsed seconds.</param>
        public void moveRight(float _timeDelta)
        {
            venesPosition += xDirection * venesSpeed * speedConstant * _timeDelta;

            positionImage1 += xDirection * speed * speedConstant * _timeDelta;
            positionImage2 += xDirection * speed * speedConstant * _timeDelta;
            positionImage3 += xDirection * speed * speedConstant * _timeDelta;
            positionImage4 += xDirection * speed * speedConstant * _timeDelta;
            positionImage5 += xDirection * speed * speedConstant * _timeDelta;
            positionImage6 += xDirection * speed * speedConstant * _timeDelta;
            positionImage7 += xDirection * speed * speedConstant * _timeDelta;
            positionImage8 += xDirection * speed * speedConstant * _timeDelta;
            positionImage9 += xDirection * speed * speedConstant * _timeDelta;
        }

        /// <summary>
        /// Moves the bg in a positive horizontal position(left).
        /// </summary>
        /// <param name="_timeDelta">gameTime total elapsed seconds.</param>
        public void moveLeft(float _timeDelta)
        {
            venesPosition += -1 * xDirection * venesSpeed * speedConstant * _timeDelta;

            positionImage1 += -1 * xDirection * speed * speedConstant * _timeDelta;
            positionImage2 += -1 * xDirection * speed * speedConstant * _timeDelta;
            positionImage3 += -1 * xDirection * speed * speedConstant * _timeDelta;
            positionImage4 += -1 * xDirection * speed * speedConstant * _timeDelta;
            positionImage5 += -1 * xDirection * speed * speedConstant * _timeDelta;
            positionImage6 += -1 * xDirection * speed * speedConstant * _timeDelta;
            positionImage7 += -1 * xDirection * speed * speedConstant * _timeDelta;
            positionImage8 += -1 * xDirection * speed * speedConstant * _timeDelta;
            positionImage9 += -1 * xDirection * speed * speedConstant * _timeDelta;
        }
        #endregion


        /// <summary>
        /// Draws the 9 bg textures.
        /// </summary>
        /// <param name="spriteBatch">Takes a spritebatch to draw the textures.</param>
        public void Draw(SpriteBatch spriteBatch)
        {

            //top row
            spriteBatch.Draw(this.texture, this.positionImage1, Color.White);
            spriteBatch.Draw(this.texture, this.positionImage2, Color.White);
            spriteBatch.Draw(this.texture, this.positionImage3, Color.White);
            //middle row
            spriteBatch.Draw(this.texture, this.positionImage4, Color.White);
            spriteBatch.Draw(this.texture, this.positionImage5, Color.White);
            spriteBatch.Draw(this.texture, this.positionImage6, Color.White);
            //bottom row
            spriteBatch.Draw(this.texture, this.positionImage7, Color.White);
            spriteBatch.Draw(this.texture, this.positionImage8, Color.White);
            spriteBatch.Draw(this.texture, this.positionImage9, Color.White);

            spriteBatch.Draw(venesTexture, venesPosition, Color.White);

        }
    }

}
