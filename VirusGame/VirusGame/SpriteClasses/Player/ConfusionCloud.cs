using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame.SpriteClasses.Player
{
    class ConfusionCloud : MovingSprite
    {
        private Color overlay;
        private byte alphaByte = 0;
        private int timer;
        public bool active = false;
        public bool permanentlyDisable = false;
        private int activeCount;
        private byte stage;
        private bool activate;
        private bool higherActive;

        public ConfusionCloud(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            animation.Scale = 8f;
            animation.Depth = .0091f;
            aniM.FramesPerSecond = 1;
            animation.IsLooping = true;
            aniM.AddAnimation("one", 1, _frames, animation.Copy());
            aniM.AddAnimation("two", 2, _frames, animation.Copy());
            aniM.AddAnimation("three", 3, _frames, animation.Copy());
            aniM.AddAnimation("four", 4, _frames, animation.Copy());
            aniM.Animation = "one";
            body.Dispose();
            Type = "Alarm";
            rotates = false;
            active = true;
            IsVisible = false;


        }

        public bool Activate
        {
            set { activate = value; }
        }


        public int ActiveCount
        {
            get { return activeCount; }
        }

        public override void Update(GameTime gameTime)
        {
            
            if (activate && stage < 4)
            {
                stage++;
                activate = false;
                timer = 120;
            }


            if (stage == 0)
            {
                isVisible = false;
                activate = false;
            }
            else
            {
                isVisible = true;
            }

            if (stage == 1 && aniM.Animation != "one")
            {
                aniM.Animation = "one";
                timer = 120;
            }
            if (stage == 2 && aniM.Animation != "two")
            {
                aniM.Animation = "two";
                timer = 120;
            }
            if (stage == 3 && aniM.Animation != "three")
            {
                aniM.Animation = "three";
                timer = 120;
            }
            if (stage == 4 && aniM.Animation != "four")
            {
                aniM.Animation = "four";
                timer = 120;
            }

            if (timer == 0 && stage > 0)
            {
                stage--;
            }

            timer--;
            activeCount--;
            aniM.Update(gameTime);

        }
    }
}
