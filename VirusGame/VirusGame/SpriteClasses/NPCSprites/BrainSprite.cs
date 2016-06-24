using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace VirusGame.SpriteClasses.NPCSprites
{
    public class BrainSprite : MovingSprite
    {

        public bool active = false;
        public bool permanentlyDisable = false;
        public int timer = 0;

        public BrainSprite(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            position = _position;
            //position.Y += aniM.Height / 2f;
            animation.Scale = 2.4f;
            animation.Depth = .1f;
            aniM.FramesPerSecond = 10;
            animation.IsLooping = true;
            //rotation = (float)Math.PI / 2f;
            aniM.AddAnimation("active", 1, 12, animation.Copy());
            animation.IsLooping = false;
            aniM.AddAnimation("killed", 2, 12, animation.Copy());
            aniM.Animation = "active";
            body.FixtureList[0].UserData = "brain";
            body.IsSensor = true;
            body.CollidesWith = Category.All;
            body.CollisionCategories = Category.Cat5;
            Type = "brain";
            rotates = false;
            active = true;
            IsVisible = true;
        }

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.UserData as string == "Pattack")
            {
                if(aniM.Animation != "killed")
                    aniM.Animation = "killed";
                return true;
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            body.OnCollision += OnCollision;
            if (aniM.Animation == "killed")
            {
                timer++;
            }
            aniM.Update(gameTime);
        }

    }
}
