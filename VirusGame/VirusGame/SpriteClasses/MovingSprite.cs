using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System.Collections;

namespace VirusGame.SpriteClasses
{
    public abstract class MovingSprite : SpriteClasses.BaseSprite
    {

        #region Declarations
        protected SpriteClasses.Menu.Points pointList;

        public Vector2 velocity;
        public Vector2 oldPosition;
        protected Vector2 spawn;
        protected bool testCollision = false;

        protected AnimationManager aniM = new AnimationManager();
        public Animation animation = new Animation().Copy();

        public bool credit;
        public bool firstRun;
        protected float depth = 0f;


        protected World level;
        public Body body;
        //public Body wheel;
        //public Fixture fixture;
        private float centerOffset;

        private int points = 0;
        private bool killed = false;
        protected bool bodyRemoved;

        public Vector2 getPosition;


        //physics
        public float mass = 1f;
        public float COR = .4f;
        public float friction = .2f;
        public float tangentialVelocity = 1f;
        public int frames;
        public int rows;
        public bool rotates = true;
        private bool hasMoved;
        protected Vector2 playerPosition;


        #endregion

        #region get/set

        public ArrayList getPoints
        {
            get { return pointList.getInfo(); }
        }

        public Vector2 PlayerPosition
        {
            set { playerPosition = value; }
        }


        public bool HasMoved
        {
            get { return hasMoved; }
            set { hasMoved = value; }
        }

        public bool BodyRemoved
        {
            get { return bodyRemoved; }
        }


        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public AnimationManager animationManager
        {
            get { return aniM; }
            set { aniM = value; }
        }

        public float Depth
        {
            set { depth = value; }
        }

        public Vector2 Position
        {
            get
            {
                return (ConvertUnits.ToDisplayUnits(body.Position) + Vector2.UnitY * centerOffset);
            }
        }

        #endregion

        /// <summary>
        /// Creates moving sprite without animations.
        /// </summary>
        /// <param name="_texture">texture</param>
        /// <param name="_position">position</param>
        /// <param name="_velocity">velocity</param>
        public MovingSprite(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _radius = 20)
        {
            texture = _texture;
            position = _position;
            oldPosition = position;
            velocity = _velocity;
            level = _level;

            spawn = _position;
            //rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            //circle = new Circle(texture.Height / 2, position);
        }
        public MovingSprite(Vector2 _position, float _rotation, int _distance, float _speed)
        {

        }
        public MovingSprite(Texture2D _texture, float _speed)
        {
            
        }


        /// <summary>
        /// Creates a moving sprite with animations.
        /// </summary>
        /// <param name="_texture">texture</param>
        /// <param name="_position">position</param>
        /// <param name="_velocity">velocity</param>
        /// <param name="_frames">frames horizontally</param>
        /// <param name="_animations">rows/animations</param>
        public MovingSprite(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations, int _radius = 20)
        {
            texture = _texture;
            position = _position;
            oldPosition = position;
            velocity = _velocity;
            aniM.Frames = frames = _frames;
            aniM.Rows = rows = _animations;
            aniM.Width = _texture.Width / _frames;
            aniM.Height = _texture.Height / rows;
            aniM.Origin = origin = new Vector2(aniM.Width, aniM.Height) / 2;
            spawn = _position;

            level = _level;

            //farseer

            body = BodyFactory.CreateCircle(level, ConvertUnits.ToSimUnits(_radius * Globals.GlobalScale), 1.0f, Globals.getWorldPosition(position));
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 0.1f;
            body.Friction = 5f;
            //body.Position = new Vector2(ConvertUnits.ToSimUnits(position.X), ConvertUnits.ToSimUnits(position.Y));
            body.IgnoreGravity = true;
            centerOffset = position.Y - (float)ConvertUnits.ToDisplayUnits(body.Position.Y); //remember the offset from the center for drawing
            body.SleepingAllowed = true;
        }


        /// <summary>
        /// Animated update: rectangle position, old position and adds velocity.
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        public virtual void Update(GameTime gameTime)
        {

            if (position != oldPosition)
                HasMoved = true;
            else HasMoved = false;

            this.oldPosition = this.position;
            this.position += this.velocity;

            this.aniM.Update(gameTime);

        }

        public virtual void Update(GameTime gameTime, Vector2 _position)
        {
            position = _position;
            if (position != oldPosition)
                HasMoved = true;
            else HasMoved = false;

            this.oldPosition = this.position;
            this.position += this.velocity;

            this.aniM.Update(gameTime);

        }




        public int GivePoints()
        {
            if (!killed)
            {
                killed = true;
                credit = true;
                return points;
            }
            return 0;
        }

        /// <summary>
        /// Animated Draw
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
                aniM.Draw(spriteBatch, texture, position, this.rotation);
        }
    }
}
