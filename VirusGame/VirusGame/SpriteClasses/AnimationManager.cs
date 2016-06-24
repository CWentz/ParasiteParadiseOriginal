using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VirusGame.SpriteClasses
{

    /// <summary>
    /// AnimationManager manages all the animations. It has a dictionary of animations.
    /// The dictionary will use a string reference to load a animation.
    /// </summary>
    public class AnimationManager
    {

        #region Declarations

        public Vector2 Position = Vector2.Zero;
        protected Dictionary<string, Animation> Animations = new Dictionary<string, Animation>();
        protected int FrameIndex = 0;
        protected Vector2 origin;

        // default to 20 frames per second
        private float timeToUpdate = 0.05f;     //timeToUpdate is the FPS for the animation. 
        private int height;                     //height of a frame
        private int width;                      //width of a frame
        private int frames;                     //how many frames hoizontally
        private int rows;                       //how many rows or animations
        private string animation;               //stores the current animation string dictionary reference. 
        private float depth = 0f;
        private float timeElapsed;
        private float rotation;
        private int currentFrame;

        #endregion


        #region Get/Set Methods

        public int frameIndex
        {
            set { FrameIndex = value; }
            get { return FrameIndex; }
        }

        /// <summary>
        /// Get method returns the animation string.
        /// Set sets animation to a new value and sets the frameIndex back to the first frame.
        /// </summary>
        public string Animation
        {
            get { return animation; }
            set
            {
                animation = value;
                FrameIndex = 0;
            }
        }

        public string setAnimation
        {
            get { return animation; }
            set { animation = value; }
        }


        /// <summary>
        /// Get and sets the height value.
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        ///// <summary>
        ///// Get and sets the color value for the current animation.
        ///// </summary>
        //public Color color
        //{
        //    get { return Animations[Animation].Color; }
        //    set { Animations[Animation].Color = value; }
        //}

        /// <summary>
        /// Get and sets the width value.
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Get and sets the frame value.
        /// </summary>
        public int Frames
        {
            get { return frames; }
            set { frames = value; }
        }


        /// <summary>
        /// Get and sets the current frame value.
        /// </summary>
        public int CurrentFrame
        {
            set { currentFrame = value; }
            get { return currentFrame; }
        }


        /// <summary>
        /// Get and sets the rows value.
        /// </summary>
        public int Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        /// <summary>
        /// Get/Set the origin value.
        /// </summary>
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        /// <summary>
        /// Get/Set the depth value.
        /// </summary>
        public float Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        /// <summary>
        /// Set the Frames Per Second for the animations.
        /// </summary>
        public int FramesPerSecond
        {
            set { timeToUpdate = (1f / value); }
        }


        #endregion


        /// <summary>
        /// Animation manager is used to store animations in a dictionary.
        /// Will set the frame height, width, and the origin.
        /// </summary>
        /// <param name="_texture">texture for animation.</param>
        /// <param name="_frames">How many frames horizontally.</param>
        /// <param name="_rows">How many rows(animations).</param>
        //public AnimationManager(Texture2D _texture, int _frames, int _rows)
        //{
        //    frames = _frames;
        //    rows = _rows;
        //    width = _texture.Width / _frames;
        //    height = _texture.Height / _rows;
        //    Origin = new Vector2(width / 2, height / 2);
        //}


        /// <summary>
        /// Add an animation to the dictionary of this manager. 
        /// Creates an array of rectangles to store a row as an animation.
        /// Stores the animation in the dictionary with a string reference.
        /// </summary>
        /// <param name="_name">String value name of animation</param>
        /// <param name="_row">what row for the animation</param>
        /// <param name="_frames">how many frames in the row</param>
        /// <param name="_animation">Animation to be stored in the manager</param>
        public void AddAnimation(string _name, int _row, int _frames, Animation _animation)
        {
            Rectangle[] recs = new Rectangle[frames];
            for (int i = 0; i < frames; i++)
            {
                if (currentFrame != 0 && currentFrame < frames && currentFrame > 0)
                {
                    i = currentFrame;
                    currentFrame = 0;
                }
                recs[i] = new Rectangle(i * width, (_row - 1) * height, width, height);
            }
            _animation.Frames = _frames;
            _animation.Rectangles = recs;
            Animations.Add(_name, _animation);
        }


        /// <summary>
        /// Updates the animation based on the frames per second.
        /// IsLooping is also tested.
        /// </summary>
        /// <param name="gameTime">for elapsed time</param>
        public void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;

                
                if (FrameIndex < Animations[Animation].Frames - 1)
                    FrameIndex++;
                else if (Animations[Animation].IsLooping)
                    FrameIndex = 0;
            }
        }


        /// <summary>
        /// Draws the animation. 
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="_texture">texture of sprite</param>
        /// <param name="_position">position of sprite</param>
        /// <param name="_rotation">rotation of sprite</param>
        public void Draw(SpriteBatch spriteBatch, Texture2D _texture, Vector2 _position, float _rotation)
        {
            rotation = _rotation;
            try
            {
                spriteBatch.Draw(_texture, _position,
                    Animations[Animation].Rectangles[FrameIndex],
                    Animations[Animation].Color,
                    _rotation, Origin,
                    Animations[Animation].Scale,
                    Animations[Animation].SpriteEffect, Animations[Animation].Depth);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }

        }


        public void Draw(SpriteBatch spriteBatch, Texture2D _texture, Color _color, Vector2 _position, float _rotation)
        {
            rotation = _rotation;
            try
            {
            spriteBatch.Draw(_texture, _position,
                Animations[Animation].Rectangles[FrameIndex],
                _color,
                _rotation, Origin,
                Animations[Animation].Scale,
                Animations[Animation].SpriteEffect, Animations[Animation].Depth);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
