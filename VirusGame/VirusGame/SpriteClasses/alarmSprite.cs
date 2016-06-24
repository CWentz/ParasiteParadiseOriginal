using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses
{
    class alarmSprite : MovingSprite
    {
        private Color overlay;
        private byte alphaByte = 0;
        public bool active = false;
        public bool permanentlyDisable = false;
        private int activeCount;

        public alarmSprite(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
        : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            animation.Scale = 4f;
            animation.Depth = .009f;
            aniM.FramesPerSecond = 10;
            animation.IsLooping = true;
            aniM.AddAnimation("active", 1, _frames, animation.Copy());
            aniM.Animation = "active";
            Type = "Alarm";
            rotates = false;
            body.CollidesWith = ~Category.All;
            active = true;
            IsVisible = false;
            body.Dispose();
            
        }

        public int ActiveCount
        {
            get { return activeCount; }
        }

        public override void Update(GameTime gameTime)
        {
            overlay = new Color(255, 255, 255, alphaByte);
            if (active && !permanentlyDisable)
                alphaByte++;
            if (!active)
                alphaByte = 0;

            
            if (activeCount <= 0 && IsVisible)
                activeCount = 15;
            if (activeCount > 1)
                IsVisible = true;
            if (activeCount == 1)
                IsVisible = false;

            position = Globals.CameraPosition;
            activeCount--;
            aniM.Update(gameTime);

        }
    }
}
