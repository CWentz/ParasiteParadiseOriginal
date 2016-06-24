using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace VirusGame.SpriteClasses.NPCSprites
{
    public class Valve : SpriteClasses.MovingSprite
    {

        #region Declarations

        static Random rand = new Random();
        public int timer = 140;
        #endregion

        public Valve(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            
            Type = "valve";
            aniM.Depth = 0.01f;
            animation.Scale = .25f;
            changeCOR(0f);
            changeMass(5f);
            aniM.AddAnimation("spin", 1, 5, animation.Copy());
            aniM.Animation = "spin";
            circle.Radius = (aniM.Height / 2) * animation.Scale;
            body.SleepingAllowed = true;
            body.Restitution = 0f;
            body.Mass = 1f;
        }


        public void UpdateTest(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


            Vector2 tempVelocity = new Vector2(0, 0);


            tempVelocity += new Vector2(velocity.X, velocity.Y);


            //0 no bounce 1 bounce set from 0.0 to 1.0
            body.Mass = 2f;
            body.Restitution = 0f;
            //slows the object when no force applied
            body.LinearDamping = .75f;

            body.ApplyForce(velocity);


            getPosition = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));

            position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));
            this.rectangle.X = (int)this.position.X - (this.rectangle.Width / 2);
            this.rectangle.Y = (int)this.position.Y - (this.rectangle.Height / 2);

            if (position != oldPosition)
                HasMoved = true;
            else HasMoved = false;

            this.oldPosition = this.position;
            //this.position += this.velocity;

            this.circle.Update(position);
            this.aniM.Update(gameTime);
            if (rotates)
                this.rotation = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);

        }

    }
}
