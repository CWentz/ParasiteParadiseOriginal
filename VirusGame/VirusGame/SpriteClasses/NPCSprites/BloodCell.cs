using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace VirusGame.SpriteClasses.NPCSprites
{
    public class BloodCell : SpriteClasses.MovingSprite
    {

        #region Declarations

        private static Random rand = new Random(1);
        private int timer = 200;
        private int FPStimer = 30;
        private Vector2 initialVelocity;
        public bool despawnerCollide = false;
        private Vector2 origionalPosition;
        private bool reset = false;
        private int animationNumber;

        #endregion

        #region get/set

        public Vector2 InitialVelocity
        {
            get
            {
                return initialVelocity;
            }
        }
        public int Timer
        {
            set { timer = value; }
            get { return timer; }
        }

        public int AnimationNumber
        {
            get { return animationNumber; }
        }

        public AnimationManager AnimationMan
        {
            set { aniM = value; }
        }



        public float Mass
        {
            set { body.Mass = value; }
        }

        #endregion

        public BloodCell(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            origionalPosition = position;
            animation.Depth = 0.074f;
            position = new Vector2((float)rand.Next((int)_position.X - 30, (int)_position.X + 30), (float)rand.Next((int)_position.Y - 30, (int)_position.Y + 30));
            initialVelocity = velocity;
            body.FixtureList[0].UserData = "bloodcell";
            Type = "bloodcell";
            animation.Scale = .25f;
            aniM.FramesPerSecond = rand.Next(6, 10);
            aniM.AddAnimation("spin", 1, _frames, animation.Copy());
            aniM.Animation = "spin";
            body.SleepingAllowed = true;
            body.Restitution = 0f;
            body.Mass = 10f;
            animationNumber = rand.Next(0, 4);

            body.CollidesWith = Category.All &~Category.Cat2 &~Category.Cat15;

            body.IgnoreCCD = true;
            body.IsBullet = true;
            //body.CollisionGroup = -1;
            testCollision = true;

            //body.IgnoreCCD = true;

            this.texture = null;
        }


        public override void Update(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (reset)
            {
                timer = 200;
                body.Position = Globals.getWorldPosition(origionalPosition);
                body.LinearVelocity = new Vector2(0, 0);
                reset = false;
            }

            if (IsVisible)
            {
                Vector2 tempVelocity = new Vector2(0, 0);

                tempVelocity += new Vector2(velocity.X, velocity.Y);

                if (rotates)
                    this.rotation = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);

                body.ApplyForce(tempVelocity);

                if (testCollision)
                {
                    body.OnCollision += OnCollision;
                    //position = position + (position - oldPosition);
                    position += (velocity / 5f);
                }
                else
                {
                    position = Globals.getDisplayPosition(body.Position);
                }
                
                //position = (body.Position);
                

                testCollision = !testCollision;

                timer--;

                if (timer <= 0)
                {
                    IsVisible = false;
                }

                //changes the FPS of the animation to a random between 4-10.
                //FPS timer is the time between the change.
                FPStimer--;
                if (FPStimer <= 0)
                {
                    aniM.FramesPerSecond = rand.Next(4, 10);
                    FPStimer = 30;
                }


                if (position != oldPosition)
                    HasMoved = true;
                else HasMoved = false;
                oldPosition = position;

                //animationManager.Update(gameTime);
            }

            if (!IsVisible && !bodyRemoved)
            {
                bodyRemoved = true;
                body.Dispose(); //level.RemoveBody(body);
                
            }
        }

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            switch (fixtureB.UserData as string)
            {
                case "BloodCellDespawner":
                    {
                        if (despawnerCollide)
                        {
                            timer = 0;
                            return true;
                        }
                        else return true;
                    }
                case "Makrophage":
                    {
                        return true;
                    }
                case "Wall":
                    {
                        return true;
                    }
                case "Player":
                    {
                        return true;
                    }
                case "Prop":
                    {
                        return false;
                    }
                case "valve":
                    {

                        timer = 0;
                        return false;
                    }
                case "bloodcell":
                    {
                        return false;
                    }
                default :
                    return false;

            }
        }


        public void speedGovernor()
        {
            //if (velocity.X > 10f)
            //    velocity.X = 10f;
            //if (velocity.X < -10f)
            //    velocity.X = -10f;
            //if (velocity.Y > 10f)
            //    velocity.Y = 10f;
            //if (velocity.Y < -10f)
            //    velocity.Y = -10f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
                aniM.Draw(spriteBatch, Globals.TextureManager.Sprites(12), position, this.rotation);
        }
    }
}
