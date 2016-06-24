using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses.BloodControl
{
    public class BloodAnimation
    {

        private bool reset1 = true;
        private bool reset2 = true;
        private bool reset3 = true;
        private bool reset4 = true;
        private bool reset5 = true;
        private int count;

        private static Random rand = new Random();

        private Texture2D texture;

        protected List<AnimationManager> animList;
        protected Animation[] animationList;
        //protected AnimationManager aniM = new AnimationManager();
        protected Animation a1;
        protected Animation a2;
        protected Animation a3;
        protected Animation a4;
        protected Animation a5;
        //protected Animation animation = new Animation();


        public BloodAnimation()
        {
            animList = new List<AnimationManager>();
            animationList = new Animation[5];

            texture = Globals.TextureManager.Sprites(12);
            

            for (int i = 0; i < 5; i++)
            {
                AnimationManager aniM = new AnimationManager();
                Animation animation = new Animation();
                aniM.Frames = 5;
                aniM.Rows = 1;
                aniM.Width = texture.Width / 5;
                aniM.Height = texture.Height / 1;
                aniM.Origin = new Vector2(aniM.Width, aniM.Height) / 2;
                animation.Depth = 0.074f;
                animation.Scale = .25f;
                aniM.FramesPerSecond = 10;
                animation.IsLooping = true;                
                aniM.AddAnimation("spin", 1, 5, animation.Copy());
                aniM.Animation = "spin";

                aniM.frameIndex = (i + 1);

                animList.Add(aniM);
                
            }
            texture = null;

        }

        public void Update(GameTime gameTime)
        {
            foreach (AnimationManager am in animList)
            {
                am.Update(gameTime);
            }

        }

        public AnimationManager getManager(int _number)
        {
            switch (_number)
            {
                case 1:
                    {
                        return animList[0];
                    }
                case 2:
                    {
                        return animList[1];
                    }
                case 3:
                    {
                        return animList[2];
                    }
                case 4:
                    {
                        return animList[3];
                    }
                case 5:
                    {
                        return animList[4];
                    }
                default: return animList[4];
            }

        }
    }
}
