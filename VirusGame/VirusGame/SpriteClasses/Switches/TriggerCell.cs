using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace VirusGame.SpriteClasses.NPCSprites
{
    public class TriggerCell : SpriteClasses.MovingSprite
    {

        #region Declarations

        public bool playedSound;
        public bool pulse;
        private int synapseResets;
        SpriteClasses.Parallax.Prop synapse;
        private bool switchedOn = false;
        private int cooldown = 0;
        private int timer;
        private String switchType;
        public String nameForLevel;
        private int unfoldTimer;

        #endregion

        /// <summary>
        /// Creates a nerve cell
        /// </summary>
        /// <param name="_texture">2D texture</param>
        /// <param name="_position">position of nerve</param>
        /// <param name="_velocity">n/a</param>
        /// <param name="_frames">frames per animation</param>
        /// <param name="_animations">rows of animations</param>
        /// <param name="_switchType">"toggle", "timer"</param>
        public TriggerCell(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations, String _switchType, float _rotation)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            rotation = _rotation;
            animation.Depth = 0.011f;
            animation.Scale = .75f;
            aniM.AddAnimation("On", 1, _frames, animation.Copy());
            aniM.AddAnimation("Switching", 2, 9, animation.Copy());
            aniM.AddAnimation("Off", 3, 6, animation.Copy());
            aniM.Animation = "Off";
            aniM.FramesPerSecond = 11;

            synapse = SpriteClasses.SpriteManager.addSynapse(position, 0f, 2000f, 1f, 0f);
            synapse.IsVisible = false;

            if (_switchType == "toggle" || _switchType == "timer")
                switchType = _switchType;
            else switchType = "toggle";

            rotates = false;
            Type = "Trigger";
            //0 no bounce 1 bounce set from 0.0 to 1.0
            body.Mass = 2000000f;
            body.Restitution = 0.0f;
            //slows the object when no force applied
            body.LinearDamping = 0f;
            body.CollidesWith = ~Category.Cat15;
            //body.IsSensor = true;
            JointFactory.CreateFixedRevoluteJoint(level, body, body.LocalCenter,
                new Vector2((float)ConvertUnits.ToSimUnits(_position.X), (float)ConvertUnits.ToSimUnits(_position.Y)));
             
            //body.ApplyForce(velocity);
        }

        /// <summary>
        /// Get/Set switch state
        /// </summary>
        public bool SwitchedOn
        {
            get { return switchedOn; }
            set { switchedOn = value; }
        }

        /// <summary>
        /// toggle switch
        /// </summary>
        public void toggleSwitch()
        {
            switchedOn = true;
        }

        /// <summary>
        /// timer switch
        /// </summary>
        /// <param name="_time"></param>
        public void timerSwitch(int _time)
        {


        }

        public void setSynapse(Vector2 _target)
        {
            if (_target != new Vector2(0, 0) && !pulse)
            {
                synapse.position = position;
                synapse.destination = _target;
                float radians = (float)Math.Atan2((double)(_target.Y - position.Y), (double)(_target.X - position.X));
                synapse.BloodStreamVelocity(radians);
                synapse.rotation = radians;
                //synapse.destination = _target;
                pulse = true;
            }
        }

        public override void Update(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


            Vector2 tempVelocity = new Vector2(0, 0);
            cooldown--;

            if (SwitchedOn && aniM.Animation != "Off" && unfoldTimer > 33)
                aniM.Animation = "Off";

            if (SwitchedOn && aniM.Animation != "Switching" && aniM.Animation == "On")
                aniM.Animation = "Switching";

            if (!SwitchedOn && aniM.Animation != "On")// && unfoldTimer < 1)
                aniM.Animation = "On";

            if (switchedOn)
                unfoldTimer++;
            tempVelocity += new Vector2(velocity.X, velocity.Y);

            body.OnCollision += OnCollision;

            synapse.Update(gameTime, new Vector2(0, 0));

            getPosition = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));

            position = new Vector2((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));

            body.Position = new Vector2((float)ConvertUnits.ToSimUnits(position.X), (float)ConvertUnits.ToSimUnits(position.Y));

            if (pulse)
            {
                synapse.IsVisible = true;
            }

            if (!pulse || synapse.synapseTurnOff > 200)
            {
                synapse.IsVisible = false;
            }


            if (position != oldPosition)
            {
                HasMoved = true;
            }
            else HasMoved = false;

            this.oldPosition = this.position;
            //this.position += this.velocity;

            //this.circle.Update(position);
            this.aniM.Update(gameTime);
            //if (rotates)
            //    this.rotation = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);

        }

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.UserData as string == "Player")
            {
                return true;
            }
            if (fixtureB.UserData as string == "Pattack")
            {
                UpdateSwitch();
                return false;
            }
            
            return false;
        }

        /// <summary>
        /// Updates the switch state and animation
        /// </summary>
        public void UpdateSwitch()
        {
            if (cooldown <= 0)
            {
                toggleSwitch();
                cooldown = 60;
            }
            //if (_reset && aniM.Animation != "Off")
            //{
            //    SwitchedOn = false;
            //}

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            synapse.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }

}
