using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Collision;


namespace VirusGame.SpriteClasses.NPCSprites
{
    public class MakrophageCell : VirusGame.SpriteClasses.MovingSprite
    {
        #region declarations

        public bool screamPlayed;
        public bool playScream;
        public int chasedistance = 400;
        private bool hasImploded = false;
        private int health = 100;
        public  bool proximity = false;
        private int attacked = 20;
        bool rotated = false;
        public bool chasing = false;
        
        private bool start = true;
        #endregion

        /// <summary>
        /// Makrophage Cells
        /// </summary>
        /// <param name="_texture"></param>
        /// <param name="_position"></param>
        /// <param name="_velocity"></param>
        /// <param name="_frames"></param>
        /// <param name="_animations"></param>
        public MakrophageCell(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations, int _radius = 60)
            : base(_level, _texture, _position, _velocity, _frames, _animations, _radius)
        {
            animation.Scale = .4f;
            animation.Depth = 0.05f;
            aniM.FramesPerSecond = 5;
            aniM.AddAnimation("float", 1, _frames, animation.Copy());
            aniM.Animation = "float";
            body.FixtureList[0].UserData = "Makrophage";
            Type = "Makrophage";
            rotates = false;
            body.Mass = 2f;
            body.Restitution = .2f;
            //slows the object when no force applied
            body.LinearDamping = .5f;

            body.CollisionCategories = Category.Cat29;
            body.CollidesWith = Category.All;
        }


        public override void Update(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


            Vector2 tempVelocity = new Vector2(0, 0);
            if (!proximity)
            {
                
                //if (start)
                //{
                //    chase(new Vector2(position.X, position.Y + 4));
                //    start = false;
                //}
                //else chase(start);
            }

            tempVelocity += new Vector2(velocity.X, velocity.Y);
            if (screamPlayed && playScream)
                screamPlayed = true;

            //0 no bounce 1 bounce set from 0.0 to 1.0
            
            body.ApplyForce(tempVelocity);


            getPosition = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));

            position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));
            //this.rectangle.X = (int)this.position.X - (this.rectangle.Width / 2);
            //this.rectangle.Y = (int)this.position.Y - (this.rectangle.Height / 2);

            if (position != oldPosition)
                HasMoved = true;
            else HasMoved = false;

            this.oldPosition = this.position;
            //this.position += this.velocity;
            //body.Position = new Vector2((float)ConvertUnits.ToSimUnits(position.X), (float)ConvertUnits.ToSimUnits(position.Y));
            
            //this.circle.Update(position);
            this.aniM.Update(gameTime);
            if (rotates)
            {
                //if (rotation - .1f < (float)((Math.PI / -2.0) + Math.Atan2(targetPosition.Y - position.Y, targetPosition.X - position.X)))
                //    rotation += .1f;
                //if (rotation + .1f > (float)((Math.PI / -2.0) + Math.Atan2(targetPosition.Y - position.Y, targetPosition.X - position.X)))
                //    rotation -= .1f;

                //this.rotation = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);
                //if(rotated)
                //    rotation = rotation - ((float)Math.PI / 2f);
            }
            if (testCollision)
            {
                body.OnCollision += body_OnCollision;
            }
            testCollision = !testCollision;
        }


        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            switch (fixtureB.UserData as string)
            {
                case "PlasmaShot":
                    {
                        return false;
                    }
                case "Prop":
                    {

                        return false;
                    }
                case "Makrophage":
                    {
                        return true;
                    }


                case "flow":
                    {
                        return false;
                    }


                case "collectable":
                    {
                        return false;
                    }

                case "Wall":
                    {
                        return true;
                    }
                case "BloodCellDespawner":
                    {
                        return false;
                    }

                default: return true;

            }

        }

        public void Implode()
        {
            if (!hasImploded)
            {

                Vector2 helperPos = Globals.getWorldPosition(position);

                Vector2 min = helperPos - new Vector2(1f, 1f);
                Vector2 max = helperPos + new Vector2(1f, 1f);

                min = min * Globals.GlobalScale;
                max = max * Globals.GlobalScale;

                AABB aabb = new AABB(ref min, ref max);



                level.QueryAABB(fixture =>
                {

                    Vector2 fv = (fixture.Body.Position - helperPos);
                    fv.Normalize();
                    fv *= -.5f;
                    //fixture.Body.ApplyLinearImpulse(ref fv);
                    if ((string)fixture.UserData == "Player")
                    {
                        fixture.Body.ApplyLinearImpulse(ref fv);
                        return false;
                    }

                    return true;
                }, ref aabb);


                //hasExploded = true;
            }

        }
        /// <summary>
        /// Chase AI
        /// </summary>
        /// <param name="targetPosition">position of target</param>
        public int chase(Vector2 targetPosition, bool soundAbfrage = false)
        {
            if (Vector2.Distance(position, targetPosition) < 500 && Vector2.Distance(position, targetPosition) > 400)
                proximity = true;
            if (Vector2.Distance(position, targetPosition) < chasedistance)
            {
                if (targetPosition.X < position.X)
                    velocity.X = -4;
                if (targetPosition.X > position.X)
                    velocity.X = 4;
                if (targetPosition.Y < position.Y)
                    velocity.Y = -4;
                if (targetPosition.Y > position.Y)
                    velocity.Y = 4;
                chasing = true;
                playScream = true;
                if (Vector2.Distance(position, targetPosition) < 200)
                {
                    
                    if (Vector2.Distance(position, targetPosition) < 75)
                    {
                        Implode();
                        if (!rotated)
                        {
                            rotated = true;
                        }
                        body.CollidesWith = Category.All;
                        if (Vector2.Distance(position, targetPosition) < 50 && attacked < 0)
                        {
                            if (!soundAbfrage)
                                attacked = 20;
                            return 4;
                        }
                    }
                }
                else
                {
                    rotates = false;
                    rotation = 0;
                    rotated = false;
                }


                
            }
            if (rotates)
            {
                if (rotation - .1f < (float)((Math.PI / -2.0) + Math.Atan2(targetPosition.Y - position.Y, targetPosition.X - position.X)))
                    rotation += .1f;
                if (rotation + .1f > (float)((Math.PI / -2.0) + Math.Atan2(targetPosition.Y - position.Y, targetPosition.X - position.X)))
                    rotation -= .1f;
                body.Rotation = rotation;
                //this.rotation = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);
                //if(rotated)
                //    rotation = rotation - ((float)Math.PI / 2f);
            }
            else chasing = false;
            if (!soundAbfrage)
                attacked--;
            return 0;
        }
        /// <summary>
        /// Chase AI
        /// </summary>
        /// <param name="targetPosition">position of target</param>
        public void chase(bool playerdead)
        {
            if (!playerdead)
            {
                rotates = false;
                rotation = 0;
                if (spawn.X < position.X)
                    velocity.X = -5;
                if (spawn.X > position.X)
                    velocity.X = 5;
                if (spawn.Y < position.Y)
                    velocity.Y = -5;
                if (spawn.Y > position.Y)
                    velocity.Y = 5;
            }

        }
    }
}
