using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace VirusGame.SpriteClasses.Parallax
{
    public class Prop : SpriteClasses.Parallax.BackgroundSprite
    {
        private Vector2 direction;
        //private Vector2 position;
        private Vector2 originalPosition;
        private int synapseResets;
        private char movementType;
        private int testCollisionTimer;
        private bool hasVelocity;
        
        private Vector2 change;
        //private Vector2 oldPosition;
        
        private float scale = 1f;
        //private float rotation;
        private bool isParticleProp;
        private const int positionResetTimer = 200;
        private int positionTimer = 0;
        protected byte alphaColor = 0;
        public Color fade = Color.White;
        private bool countUp = true;
        private bool updatePause;
        private int updateInterval;
        public String nameForLevel = "Prop";
        private float speedInterval = 1f;
        private int speedTimer;
        private float defaultSpeed;
        public bool pulse;
        public short synapseTurnOff;
        public Vector2 destination;

        /// <summary>
        /// particle prop
        /// </summary>
        /// <param name="_texture"></param>
        /// <param name="_position"></param>
        /// <param name="_rotation"></param>
        /// <param name="_speed"></param>
        public Prop(Texture2D _texture, Vector2 _position, float _speed)
            : base(_texture, _speed)
        {
            isVisible = true;
            isParticleProp = true;
            depth = .5f;
            //rotation = 0f;
            originalPosition = position = _position;

            scale = 1f;

            speed = _speed;
            texture = _texture;
            Type = "Prop";
            center = new Vector2(position.X + (texture.Width / 2f), position.Y + (texture.Height / 2f));
            

            //outer limit for textures
            Vector2 levelSize = new Vector2(1050, 800);
            levelSize.X += (texture.Width * scale);
            levelSize.Y += (texture.Height * scale);
            limit = levelSize;

            movementType = 'R';
            
            //body = new Body(SpriteManager.Level);

            body = BodyFactory.CreateCircle(SpriteManager.Level, .5f, 3f, Globals.getWorldPosition(position));
            //body.Position = Globals.getWorldPosition(position);
            body.Mass = 6f;
            body.CollisionCategories = Category.Cat5;
            body.CollidesWith = ~Category.All | Category.Cat10;// | Category.Cat15 | Category.Cat2 | Category.Cat3;
            body.FixtureList[0].UserData = "Prop";
            body.IsStatic = false;
            body.IgnoreGravity = true;
            body.IsSensor = true;
            //body.IsBullet = true;
            body.SleepingAllowed = true;

        }

        /// <summary>
        /// prop
        /// </summary>
        /// <param name="_texture"></param>
        /// <param name="_position"></param>
        /// <param name="_direction"></param>
        /// <param name="_speed"></param>
        /// <param name="_movementType"></param>
        /// <param name="_scale"></param>
        public Prop(Texture2D _texture, Vector2 _position, Vector2 _direction, float _speed, char _movementType, float _scale)
            : base(_texture, _speed)
        {
            depth = .8f;
            rotation = 0f;
            scale = _scale;
            originalPosition = position = _position;
            direction = _direction;
            speed = _speed;
            texture = _texture;
            Type = "Prop";
            center = new Vector2(position.X + (texture.Width / 2f), position.Y + (texture.Height / 2f));


            //outer limit for textures
            Vector2 levelSize = new Vector2(1050, 800);
            levelSize.X += (texture.Width * scale);
            levelSize.Y += (texture.Height * scale);
            limit = levelSize;
            

            //ensures that the char is uppercase.
            if (char.IsLower(_movementType))
                movementType = char.ToUpper(_movementType);
            else movementType = _movementType;


            maxPosition = new Vector2(1050f + ((texture.Width*scale) / 2f), 800f + ((texture.Height*scale) / 2f));
            minPosition = maxPosition * -1f;
            //minPosition = new Vector2(Math.Abs(limit.X), Math.Abs(limit.Y)) * -1f;


            //sets a inner limit within the level
            //note doesn't work for smaller textures.
            if (movementType == 'F')
            {
                limit = new Vector2((((texture.Width * scale) - 2100f) / 2f), (((texture.Height * scale) - 1600f) / 2f));
                limit = new Vector2(Math.Abs(limit.X), Math.Abs(limit.Y));
                maxPosition = limit / 2f;
                minPosition = limit * -1f;
            }

            
        }

        public Prop(Texture2D _texture, Vector2 _position, float _rotation, float _speed, char _movementType, float _scale, float _depth)
            : base(_texture, _speed)
        {
            depth = _depth;
            rotation = _rotation;
            scale = _scale;
            originalPosition = position = _position;
            BloodStreamVelocity(rotation);
            direction = velocity;
            defaultSpeed = speed = _speed;
            texture = _texture;
            Type = "Prop";
            center = new Vector2(position.X + (texture.Width / 2f), position.Y + (texture.Height / 2f));

            

            //outer limit for textures
            Vector2 levelSize = new Vector2(1050, 800);
            levelSize.X += (texture.Width * scale);
            levelSize.Y += (texture.Height * scale);
            limit = levelSize;


            //ensures that the char is uppercase.
            if (char.IsLower(_movementType))
                movementType = char.ToUpper(_movementType);
            else movementType = _movementType;


            maxPosition = new Vector2(1050f + ((texture.Width * scale) / 2f), 800f + ((texture.Height * scale) / 2f));
            minPosition = maxPosition * -1f;
            //minPosition = new Vector2(Math.Abs(limit.X), Math.Abs(limit.Y)) * -1f;


            //sets a inner limit within the level
            //note doesn't work for smaller textures.
            if (movementType == 'F')
            {
                limit = new Vector2((((texture.Width * scale) - 2100f) / 2f), (((texture.Height * scale) - 1600f) / 2f));
                limit = new Vector2(Math.Abs(limit.X), Math.Abs(limit.Y));
                maxPosition = limit / 2f;
                minPosition = limit * -1f;
            }
            if (movementType == 'S')
                IsVisible = false;

        }



        public override void Update(GameTime gameTime, Vector2 _positionChange)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            

            #region movementType switch
            if (!isParticleProp)
            {

                switch (movementType)
                {
                    case 'B': //bouncing
                        {
                            if (position.X > maxPosition.X)
                                direction.X = Math.Abs(direction.X) * -1;
                            if (position.X < minPosition.X)
                                direction.X = Math.Abs(direction.X);
                            if (position.Y > maxPosition.Y)
                                direction.Y = Math.Abs(direction.Y) * -1;
                            if (position.Y < minPosition.Y)
                                direction.Y = Math.Abs(direction.Y);
                            position += direction * speed * timeDelta;
                        }
                        break;
                    case 'R': //repeating
                        {
                            if (position.X > maxPosition.X || position.Y > maxPosition.Y)
                            {
                                position = originalPosition;
                            }
                            if (position.X < minPosition.X || position.Y < minPosition.Y)
                            {
                                position = originalPosition;
                            }
                            position += direction * speed * timeDelta;
                        }
                        break;
                    case 'S': //synapse
                        {
                            if (Vector2.Distance(destination, originalPosition) < Vector2.Distance(position, originalPosition))
                            {
                                speedTimer = -100;
                                speed = defaultSpeed;
                                position = originalPosition;
                                if(IsVisible)
                                    synapseResets++;
                            }

                            if (synapseResets == 2)
                            {
                                synapseTurnOff = 201;
                                isVisible = false;
                            }

                            isParticleProp = false;
                            if (position.X > maxPosition.X || position.Y > maxPosition.Y)
                            {
                                speedTimer = -100;
                                speed = defaultSpeed;
                                position = originalPosition;
                            }
                            if (position.X < minPosition.X || position.Y < minPosition.Y)
                            {
                                speedTimer = -100;
                                speed = defaultSpeed;
                                position = originalPosition;
                            }

                            if (IsVisible)
                            {
                                //(1/((x*.1))^2)*2000
                                //speedInterval = 1.0f / (float)((double)Math.Pow(speedTimer * .1f, 2.0));
                                //if(speedTimer < -90)
                                speedInterval = (float)Math.Abs(Math.Cos(speedTimer / 5f));
                                //if (speedTimer > -75)
                                //    speedInterval /= .4f;
                                speed = defaultSpeed * (speedInterval);
                                speedTimer += 1;
                                synapseTurnOff++;
                            }

                            if (synapseTurnOff > 200)
                                IsVisible = false;

                            if (speedTimer > 100)
                                speedTimer = -100;
                           
                            position += direction * speed * timeDelta;
                        }
                        break;
                    case 'W': //warping
                        {
                            if (position.X > maxPosition.X)
                                position.X = minPosition.X + 10;
                            if (position.X < minPosition.X)
                                position.X = maxPosition.X - 10f;
                            if (position.Y > maxPosition.Y)
                                position.Y = minPosition.Y + 10f;
                            if (position.Y < minPosition.Y)
                                position.Y = maxPosition.Y - 10f;
                            position += direction * speed * timeDelta;
                        }
                        break;
                    case 'F': //fixed
                        {
                            float xMove = 0;
                            float yMove = 0;

                            change = new Vector2(0, 0);




                            if (_positionChange.X != 0)
                            {
                                if (_positionChange.X > 0)
                                    xMove = 1;
                                else xMove = -1;
                            }
                            if (_positionChange.Y != 0)
                            {
                                if (_positionChange.Y > 0)
                                    yMove = 1;
                                else yMove = -1;
                            }


                            //while the center is within the bounds it will scroll
                            if (center.Y < maxPosition.Y && center.Y > minPosition.Y)
                                changedY = yMove * yDirection * speed * timeDelta;
                            if (center.X < maxPosition.X && center.X > minPosition.X)
                                changedX = xMove * xDirection * speed * timeDelta;

                            if (center.X < maxPosition.X && center.X > minPosition.X && center.Y < maxPosition.Y && center.Y > minPosition.Y)
                                change += (changedX + changedY);

                            // if the center point goes beyond the bounds it will reset to the old position.
                            if (center.X + 20 > maxPosition.X || center.X - 20 < minPosition.X)
                                position.X = oldPosition.X - 1f;
                            if (center.Y + 20 > maxPosition.Y || center.Y - 20 < minPosition.Y)
                                position.Y = oldPosition.Y - 1f;






                            position += change;

                            center = new Vector2(position.X + (texture.Width / 2f), position.Y + (texture.Height / 2f));




                            oldPosition = position;
                        }
                        break;
                    default:
                        {
                            movementType = 'B';
                        }
                        break;
                }
            }

            #endregion
            #region old code
            //if (!isFixed)
            //{
            //    //tests for multiple states are true
            //    if (isBouncing && isRepeating || isRepeating && isWarping || isBouncing && isWarping)
            //    {
            //        isBouncing = false;
            //        isRepeating = false;
            //        isWarping = true;
            //    }
            //    if (!isBouncing && !isRepeating && !isWarping)
            //    {
            //        isWarping = true;
            //    }

            //    //bounce
            //    if (isBouncing && !isRepeating && !isWarping)
            //    {
                    
            //        if (position.X > 1000)
            //            direction.X = Math.Abs(direction.X) * -1;
            //        if (position.X < -1000 - texture.Width)
            //            direction.X = Math.Abs(direction.X);
            //        if (position.Y > 1000)
            //            direction.Y = Math.Abs(direction.Y) * -1;
            //        if (position.Y < -1000 - texture.Height)
            //            direction.Y = Math.Abs(direction.Y);
            //        position += direction * speed * timeDelta;
            //    }


            //    //repeats
            //    if (isRepeating && !isBouncing && !isWarping)
            //    {

                    
            //        if (position.X > 1000 || position.Y > 1000)
            //            position = originalPosition;
            //        if (position.X < -1000 - texture.Width || position.Y < -1000 - texture.Height)
            //            position = originalPosition;
            //        position += direction * speed * timeDelta;
            //    }


            //    //warps
            //    if (isWarping && !isRepeating && !isBouncing)
            //    {
                    
            //        if (position.X > 1000)
            //            position.X = -990 - texture.Width;
            //        if (position.X + texture.Width < -1100)
            //            position.X = 990;
            //        if (position.Y > 1000)
            //            position.Y = -990 - texture.Height;
            //        if (position.Y + texture.Height < -1000)
            //            position.Y = 990;
            //        position += direction * speed * timeDelta;
            //    }
            //}
            //#endregion
            //#region isFixed
            //else
            //{
            //    float xMove = 0;
            //    float yMove = 0;

            //    change = new Vector2(0,0);




            //    if (_positionChange.X != 0)
            //    {
            //        if (_positionChange.X > 0)
            //            xMove = 1;
            //        else xMove = -1;
            //    }
            //    if (_positionChange.Y != 0)
            //    {
            //        if (_positionChange.Y > 0)
            //            yMove = 1;
            //        else yMove = -1;
            //    }
                

            //    //while the center is within the bounds it will scroll
            //    if (center.Y < maxPosition.Y && center.Y > minPosition.Y)
            //        changedY = yMove * yDirection * speed * timeDelta;
            //    if (center.X < maxPosition.X && center.X > minPosition.X)
            //        changedX = xMove * xDirection * speed * timeDelta;

            //    if (center.X < maxPosition.X && center.X > minPosition.X && center.Y < maxPosition.Y && center.Y > minPosition.Y)
            //        change += (changedX + changedY);

            //    // if the center point goes beyond the bounds it will reset to the old position.
            //    if (center.X + 10 >= maxPosition.X || center.X - 10 <= minPosition.X)
            //        position.X = oldPosition.X;
            //    if (center.Y + 10 >= maxPosition.Y || center.Y - 10 <= minPosition.Y)
            //        position.Y = oldPosition.Y;


                



            //    position += change;

            //    center = new Vector2(position.X + (texture.Width / 2f), position.Y + (texture.Height / 2f));
                
                
                
                
            //    oldPosition = position;
            
            //}
            #endregion

            #region particle prop
            if (isParticleProp)
            {
                //if (positionTimer >= positionResetTimer)
                //{
                //    position = originalPosition;
                //    body.Position = Globals.getWorldPosition(position);
                //    body.LinearVelocity = new Vector2(0, 0);
                //    positionTimer = 0;
                //}
                //if (updatePause)
                //{
                //if (testCollision)
                //{
                if(!hasVelocity)
                    body.OnCollision += OnCollision;
                //}
                //testCollision = !testCollision;
                
                    //}
                //updatePause = !updatePause;

                body.ApplyForce(velocity);
                
                positionTimer++;
                
                position = Globals.getDisplayPosition(body.Position);

                if (countUp)
                {
                    alphaColor++;
                }
                else
                {
                    alphaColor--;
                }
                if (alphaColor == 75)
                {
                    countUp = false;
                }

                fade = new Color(0, 0, 0, alphaColor);
                if (alphaColor == 0 && !countUp)
                {
                    IsVisible = false;
                }

                if (!IsVisible)
                {
                    body.Dispose();
                    bodyRemoved = true;

                }

            }
            #endregion

        }

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            switch (fixtureB.UserData as string)
            {
                case "flow":
                    {
                        BloodStreamVelocity(fixtureB.Body.Rotation);
                        hasVelocity = true;

                        return false;
                    }
                //case "Wall":
                //    {
                //        return false;
                //    }
                //case "BloodCellDespawner":
                //    {
                //        return false;
                //    }
                case "valve":
                    {
                        return false;
                    }
                case "Prop":
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        public char MoveType
        {
            get { return movementType; }
        }

        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
            }
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 OriginalPosition
        {
            get { return originalPosition; }
            set { originalPosition = value; }
        }

        public byte Alpha
        {
            get { return alphaColor; }
        }

        public void BloodStreamVelocity(float _rotation)
        {
            float X = (float)(Math.Cos(_rotation));
            float Y = (float)(Math.Sin(_rotation));
            Vector2 newPos = new Vector2(X, Y);
            velocity = direction = (newPos);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isParticleProp)
            {
                if (IsVisible)
                    spriteBatch.Draw(Globals.TextureManager.Sprites(35), position, null, fade, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, depth);
            }
            else if(IsVisible)
                spriteBatch.Draw(texture, position, null, fade, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, depth);

            //if (movementType == 'F')
            //    spriteBatch.Draw(Globals.Pixel, new Rectangle((int)center.X, (int)center.Y, 20, 20), Color.White);
        }
    }
}
