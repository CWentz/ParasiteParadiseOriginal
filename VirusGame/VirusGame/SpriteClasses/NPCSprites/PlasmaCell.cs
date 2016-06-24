using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Collision;
using FarseerPhysics.Factories;

namespace VirusGame.SpriteClasses.NPCSprites
{
    public class PlasmaCell : SpriteClasses.MovingSprite
    {
        public bool playingSound;
        public bool proximity = false;
        public bool proximityShoot = false;
        private bool movingX = false;
        private bool movingY = false;
        private bool maxDistance = false;
        private bool start = true;
        private int attacked = 20;
        private Char direction;
        private int shotAmount = 8;
        private int timer = 40;
        private NPCSprites.PlasmaShot plasmaShot;
        
        

        public List<NPCSprites.PlasmaShot> shotList;


        public PlasmaCell(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations, Char _direction)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            
            animation.Scale = .5f;
            animation.Depth = 0.1f;

            aniM.FramesPerSecond = 5;
            aniM.AddAnimation("swimVerticalDown", 1, _frames, animation.Copy());
            
            //animation.IsLooping = true;
            aniM.AddAnimation("explodeVerticalDown", 4, _frames, animation.Copy());
            aniM.AddAnimation("glowVerticalDown", 6, _frames, animation.Copy());
            aniM.AddAnimation("explodeHorzontalRight", 3, _frames, animation.Copy());
            aniM.AddAnimation("glowHorizontalRight", 5, _frames, animation.Copy());
            //animation.IsLooping = false;

            aniM.AddAnimation("swimHorzontalRight", 2, _frames, animation.Copy());

            animation.SpriteEffect = SpriteEffects.FlipHorizontally;
            aniM.AddAnimation("swimHorzontalLeft", 2, _frames, animation.Copy());

            //animation.IsLooping = true;
            aniM.AddAnimation("explodeHorzontalLeft", 3, _frames, animation.Copy());
            aniM.AddAnimation("glowHorizontalLeft", 5, _frames, animation.Copy());
            //animation.IsLooping = false;

            animation.SpriteEffect = SpriteEffects.FlipVertically;
            aniM.AddAnimation("swimVerticalUp", 1, _frames, animation.Copy());
            
            //animation.IsLooping = true;
            aniM.AddAnimation("explodeVerticalUp", 4, _frames, animation.Copy());
            aniM.AddAnimation("glowVerticalUp", 6, _frames, animation.Copy());
            //animation.IsLooping = false;
            body.FixtureList[0].UserData = "PlasmaCell";
            aniM.Origin = new Vector2(aniM.Width * .5f, aniM.Height *.75f);
            
            
            Type = "Plasma";
            rotates = false;
            direction = _direction;
            shotList = new List<PlasmaShot>();
            faceDirection();
            Points = 100;
            velocity = _velocity;
            pointList = new Menu.Points(Type, Points, false);
            //JointFactory.CreateFixedRevoluteJoint(level, body, 
            //    new Vector2((float)ConvertUnits.ToSimUnits(0), (float)ConvertUnits.ToSimUnits(20)),
            //    new Vector2((float)ConvertUnits.ToSimUnits(position.X), (float)ConvertUnits.ToSimUnits(position.Y)));

        }



        public override void Update(GameTime gameTime)
        {
            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            

            int index = -1;

            foreach (NPCSprites.PlasmaShot ps in shotList)
            {
                ps.Update(gameTime);

                if (!ps.IsVisible && ps.BodyRemoved)
                    index = shotList.IndexOf(ps);

                if (proximity)
                {
                    ps.IsVisible = false;
                }
            }

            
            if (IsVisible)
            {
                chase(playerPosition, false);
                returnToSpawn();

                if (testCollision)
                {
                    body.OnCollision += OnCollision;
                }

                testCollision = !testCollision;

                Vector2 tempVelocity = new Vector2(0, 0);

                


                tempVelocity = velocity;


                body.Mass = .5f;
                body.Restitution = 0f;
                body.LinearDamping = .25f;

                body.ApplyForce(tempVelocity);


                position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));


                
                #region direction animation changes
                if (proximity)
                {
                    aniM.FramesPerSecond = 10;
                }

                switch (direction)
                {
                    case 'd':
                        {
                            if (proximity && proximityShoot)
                            {
                                timer--;
                                if (timer > 0)
                                    timer = 0;
                                if (aniM.Animation != "explodeVerticalDown")
                                    aniM.Animation = "explodeVerticalDown";

                                if (timer == -30)
                                    IsVisible = false;
                            }
                            else
                            {
                                if (!proximityShoot && aniM.Animation != "swimVerticalDown")
                                {
                                    aniM.Animation = "swimVerticalDown";
                                    timer = 40;
                                }

                                if (proximityShoot && shotList.Count < shotAmount)
                                {
                                    timer--;
                                    if (aniM.Animation != "glowVerticalDown" && this.timer <= 20)
                                        aniM.Animation = "glowVerticalDown";

                                    if (timer <= 0)
                                    {
                                        addShot();
                                    }
                                }
                            }

                        }
                        break;


                    case 'u':
                        {
                            if (proximity)
                            {
                                if (timer > 0)
                                    timer = 0;
                                if (aniM.Animation != "explodeVerticalUp")
                                    aniM.Animation = "explodeVerticalUp";

                                if (timer == -30)
                                    IsVisible = false;
                            }
                            else
                            {
                                if (timer > 80 && timer < 90 && aniM.Animation != "swimVerticalUp")
                                    aniM.Animation = "swimVerticalUp";


                                if (shotList.Count < shotAmount && this.timer <= 20)
                                {
                                    if (aniM.Animation != "glowVerticalUp")
                                        aniM.Animation = "glowVerticalUp";
                                    if (timer <= 0)
                                    {
                                        addShot();
                                        this.timer = 150;
                                    }
                                }
                            }

                        }
                        break;


                    case 'l':
                        {
                            if (proximity)
                            {
                                if (timer > 0)
                                    timer = 0;
                                if (aniM.Animation != "explodeHorzontalLeft")
                                    aniM.Animation = "explodeHorzontalLeft";

                                if (timer == -30)
                                    IsVisible = false;
                            }
                            else
                            {
                                if (timer > 80 && timer < 90 && aniM.Animation != "swimHorzontalLeft")
                                    aniM.Animation = "swimHorzontalLeft";


                                if (shotList.Count < shotAmount && this.timer <= 20)
                                {
                                    if (aniM.Animation != "glowHorizontalLeft")
                                        aniM.Animation = "glowHorizontalLeft";
                                    if (timer <= 0)
                                    {
                                        addShot();

                                    }
                                }
                            }

                        }
                        break;


                    case 'r':
                        {
                            if (proximity)
                            {
                                if (timer > 0)
                                    timer = 0;
                                if (aniM.Animation != "explodeHorzontalRight")
                                    aniM.Animation = "explodeHorzontalRight";

                                if (timer == -30)
                                    IsVisible = false;
                            }
                            else
                            {
                                if (timer > 80 && timer < 90 && aniM.Animation != "swimHorzontalRight")
                                    aniM.Animation = "swimHorzontalRight";


                                if (shotList.Count < shotAmount && this.timer <= 20)
                                {
                                    if (aniM.Animation != "glowHorizontalRight")
                                        aniM.Animation = "glowHorizontalRight";
                                    if (timer <= 0)
                                    {
                                        addShot();
                                        this.timer = 150;
                                    }
                                }
                            }

                        }
                        break;

                    default:
                        {
                            direction = 'd';
                        }
                        break;


                }
                #endregion

                if (position != oldPosition)
                    HasMoved = true;
                else HasMoved = false;

                this.oldPosition = this.position;


                body.Rotation = rotation;
                pointList.Update(gameTime);
                this.aniM.Update(gameTime);
            }



            if (index != -1)
                shotList.RemoveAt(index);

            if (!IsVisible && !bodyRemoved)
            {
                bodyRemoved = true;
                body.Dispose(); //level.RemoveBody(body);
            }
        }

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.UserData as string == "Player")
            {
                return true;
            }
            if (fixtureB.UserData as string == "Pattack")
            {
                if (!credit)
                {
                    pointList.Credit = credit = true;
                    proximity = true;
                    return true;
                }
                return false;
            }
            if (fixtureB.UserData as string == "PlasmaShot")
            {
                return false;
            }
            return true;
        }



        public void addShot()
        {
            //if (direction == 'd')
            //    plasmaShot = SpriteClasses.SpriteManager.addPlasmaShot(position + ((new Vector2((float)Math.Cos(rotation + (MathHelper.Pi / 2f)), (float)Math.Sin(rotation + (MathHelper.Pi / 2f))))), new Vector2(0, 0), direction);
            //if (direction == 'u')
            //    plasmaShot = SpriteClasses.SpriteManager.addPlasmaShot(position + ((new Vector2((float)Math.Cos(rotation - (MathHelper.Pi / 2f)), (float)Math.Sin(rotation - (MathHelper.Pi / 2f))))), new Vector2(0, 0), direction);
            //if (direction == 'l')
            //    plasmaShot = SpriteClasses.SpriteManager.addPlasmaShot(position + ((new Vector2((float)Math.Cos(rotation + MathHelper.Pi), (float)Math.Sin(rotation + MathHelper.Pi)))), new Vector2(0, 0), direction);
            //if (direction == 'r')
            //    plasmaShot = SpriteClasses.SpriteManager.addPlasmaShot(position + ((new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)))), new Vector2(0, 0), direction);

            
            
            //plasmaShot.position = );
            if (proximityShoot)
                if (IsVisible && !proximity)
                {

                    for (int a = 0; a < 3; a++)
                    {
                        if (a == 0)
                        {
                            plasmaShot = SpriteClasses.SpriteManager.addPlasmaShot(position + ((new Vector2((float)Math.Cos(rotation + (MathHelper.Pi / 2f)), (float)Math.Sin(rotation + (MathHelper.Pi / 2f))))), new Vector2(0, 0), direction);
                            plasmaShot.rotation = rotation + .2f;
                            shotList.Add(plasmaShot);
                        }
                        if (a == 1)
                        {

                            plasmaShot = SpriteClasses.SpriteManager.addPlasmaShot(position + ((new Vector2((float)Math.Cos(rotation + (MathHelper.Pi / 2f)), (float)Math.Sin(rotation + (MathHelper.Pi / 2f))))), new Vector2(0, 0), direction);
                            plasmaShot.rotation = rotation - .2f;
                            shotList.Add(plasmaShot);
                        }
                        if (a == 2)
                        {

                            plasmaShot = SpriteClasses.SpriteManager.addPlasmaShot(position + ((new Vector2((float)Math.Cos(rotation + (MathHelper.Pi / 2f)), (float)Math.Sin(rotation + (MathHelper.Pi / 2f))))), new Vector2(0, 0), direction);
                            plasmaShot.rotation = rotation;
                            shotList.Add(plasmaShot);
                        }
                        //if (a == 3)
                        //{
                        //    plasmaShot = SpriteClasses.SpriteManager.addPlasmaShot(position + ((new Vector2((float)Math.Cos(rotation + (MathHelper.Pi / 2f)), (float)Math.Sin(rotation + (MathHelper.Pi / 2f))))), new Vector2(0, 0), direction);
                        //    plasmaShot.rotation = rotation - .3f;
                        //    shotList.Add(plasmaShot);
                        //}

                    }
                    //plasmaShot.rotation = rotation + .1f;
                    //shotList.Add(plasmaShot);
                    //plasmaShot.rotation = rotation + .3f;
                    //shotList.Add(plasmaShot);
                    //plasmaShot.rotation = rotation - .1f;
                    //shotList.Add(plasmaShot);
                    //plasmaShot.rotation = rotation - .3f;
                    //shotList.Add(plasmaShot);
                    timer = 40;
                }

        }


        /// <summary>
        /// Chase AI
        /// </summary>
        /// <param name="targetPosition">position of target</param>
        public void chase(Vector2 targetPosition, bool soundAbfrage = false)
        {
            if (Vector2.Distance(position, targetPosition) < 500)
            {

                //if (rotation - .1f < (float)((Math.PI / -2.0) + Math.Atan2(targetPosition.Y - position.Y, targetPosition.X - position.X)))
                //    rotation += .1f;
                rotation = (float)((Math.PI / -2.0) + Math.Atan2(targetPosition.Y - position.Y, targetPosition.X - position.X));
                //    rotation -= .1f;
                //rotation = (float)((Math.PI / -2.0) + Math.Atan2(targetPosition.Y - position.Y, targetPosition.X - position.X));

                proximityShoot = true;
                if (!soundAbfrage)
                {

                }

            }
            else
            {
                if (spawn.X < position.X)
                    velocity.X = -.5f;
                if (spawn.X > position.X)
                    velocity.X = .5f;
                if (spawn.Y < position.Y)
                    velocity.Y = -.5f;
                if (spawn.Y > position.Y)
                    velocity.Y = .5f;
                    
                proximityShoot = false;
            }
        }

     
        public void returnToSpawn()
        {
            Vector2 tempVect = new Vector2((float)(Math.Cos(rotation)), (float)(Math.Sin(rotation)));
            

            if (Vector2.Distance(position, spawn) > 100 && !maxDistance)
            {
                maxDistance = true;
                body.LinearVelocity = new Vector2(0, 0);
            }
            if (Vector2.Distance(position, spawn) < 90)
            {
                maxDistance = false;
            }

            //velocity += new Vector2((float)(Math.Cos(rotation)), (float)(Math.Sin(rotation)));
            if (velocity.X <= .05f && velocity.X >= -.05f)
            {   
                movingX = false;
            }
            if (velocity.Y <= .05f && velocity.Y >= -.05f)
            {
                movingY = false;
            }
            if (tempVect.X != 0 && tempVect.Y != 0 && !maxDistance)
            {
                if (spawn.X - 5f < position.X && !movingX)
                {
                    velocity.X = -2f * tempVect.X;
                    movingX = true;
                }
                else if (spawn.X + 5f > position.X && !movingX)
                {
                    velocity.X = 2f * tempVect.X;
                    movingX = true;
                }
                if (spawn.Y - 5f < position.Y && !movingY)
                {
                    velocity.Y = -2f * tempVect.Y;
                    movingY = true;
                }
                else if (spawn.Y + 5f > position.Y && !movingY)
                {
                    velocity.Y = 2f * tempVect.Y;
                    movingY = true;
                }
            }
            velocity = velocity / 2f;
            //if (maxDistance)
            //{
            //    if (spawn.X - 5f < position.X && !movingX)
            //    {
            //        velocity.X = -1f;
            //        movingX = true;
            //        maxDistance = false;
            //    }
            //    else if (spawn.X + 5f > position.X && !movingX)
            //    {
            //        velocity.X = 1f;
            //        movingX = true;
            //        maxDistance = false;
            //    }
            //    if (spawn.Y - 5f < position.Y && !movingY)
            //    {
            //        velocity.Y = -1f;
            //        movingY = true;
            //        maxDistance = false;
            //    }
            //    else if (spawn.Y + 5f > position.Y && !movingY)
            //    {
            //        velocity.Y = 1f;
            //        movingY = true;
            //        maxDistance = false;
            //    }
            //}
        }


        private void faceDirection()
        {
            if (direction == 'l')
            {
                //rotation = MathHelper.Pi;
                aniM.Animation = "swimHorzontalLeft";
            }
            else if (direction == 'r')
            {
                //rotation = 0;
                aniM.Animation = "swimHorzontalRight";
            }
            else if (direction == 'u')
            {
                //rotation = MathHelper.Pi / 2;
                aniM.Animation = "swimVerticalUp";
            }
            else if (direction == 'd')
            {
                //rotation = MathHelper.Pi / 2;
                aniM.Animation ="swimVerticalDown";
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (SpriteClasses.NPCSprites.PlasmaShot ps in shotList)
            {
                if(ps.IsVisible)
                    ps.Draw(spriteBatch);
            }

            if (IsVisible)
            {
                aniM.Draw(spriteBatch, texture, position, this.rotation);

            }
        }
    }
}
