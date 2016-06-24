using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;

namespace VirusGame.SpriteClasses.Player
{
    public class PlayerAttack : SpriteClasses.MovingSprite
    {

        private bool killed;

        public PlayerAttack(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {

            animation.Scale = .5f;
            animation.Depth = 0.0712f;
            aniM.FramesPerSecond = 20;
            aniM.AddAnimation("attack", 1, _frames, animation.Copy());
            aniM.Animation = "attack";
            
            Type = "attack";
            rotates = false;
            body.Dispose();


            body = BodyFactory.CreateRectangle(level, ConvertUnits.ToSimUnits(30.0 * Globals.GlobalScale), ConvertUnits.ToSimUnits((140.0 * Globals.GlobalScale) * Globals.GlobalScale), 100f);

            body.Position = new Vector2((float)ConvertUnits.ToSimUnits(position.X), (float)ConvertUnits.ToSimUnits(position.Y));
            body.BodyType = BodyType.Dynamic;
            //fixture = FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(10), 1, body.Position, body);

            body.Enabled = true;
            body.Mass = 10f;
            body.FixedRotation = true;
            body.IgnoreGravity = true;
            body.FixtureList[0].UserData = "Pattack";
            body.IsSensor = true;
            body.Mass = 0f;
            body.Restitution = 0f;
            body.Friction = 0f;
        }

        public bool Killed
        {
            get { return killed; }
        }


        public override void Update(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (isVisible)
            {
                body.CollidesWith = Category.All;
            }
            else 
            { 
                body.CollidesWith = ~Category.All; 
            }


            Vector2 tempVelocity = new Vector2(0, 0);


            tempVelocity += new Vector2(velocity.X, velocity.Y);

            if (body.Friction != 0f)
            {
                killed = true;
            }
            else
            {
                killed = false;
            }

            //0 no bounce 1 bounce set from 0.0 to 1.0

            //slows the object when no force applied;
            
            //body.ApplyForce(velocity);
            rotates = true;
            

            //body.Position = body.Position + new Vector2((float)ConvertUnits.ToSimUnits((float)Math.Cos(rotation)), (float)ConvertUnits.ToSimUnits((float)Math.Sin(rotation)));
            body.Position = new Vector2((float)ConvertUnits.ToSimUnits(position.X), (float)ConvertUnits.ToSimUnits(position.Y));
            //position = new Vector2((float)ConvertUnits.ToDisplayUnits(body.Position.X), (float)ConvertUnits.ToDisplayUnits(body.Position.Y));

            if (position != oldPosition)
                HasMoved = true;
            else HasMoved = false;

            this.oldPosition = this.position;

            this.aniM.Update(gameTime);
            

        }

   
    }
}
