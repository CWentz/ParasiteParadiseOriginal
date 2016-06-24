using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using VirusGame.SpriteClasses;

namespace VirusGame.UI
{
    public class StartingVideo
    {
        public Texture2D gamesAcademyTexture;
        public Texture2D infechtTechTexture;
        public Texture2D farseerTexture;
        public Texture2D currentTexture;
        
        public Color fadingColor;

        public char state;

        private Byte alpha;
        private bool gaPlayed;
        private bool inftPlayed;
        private bool farsPlayed;
        private bool introPlayed;
        private bool outroPlayed;
        private bool countingUp;
        //private bool playNext;
        private bool wasDown;
        private bool firstRun = true;

        //public const int interval = 7;
        //public int currentFrame = 1;
        //public int ticker;
        //public int extra;
        //public int imageTimer;
        private float scale = 1f;

        private Video video;
        private VideoPlayer videoPlayer = new VideoPlayer();

        public StartingVideo()
        {
            alpha = 254;
            gamesAcademyTexture = SpriteManager.getImage("LoadingMedia/GA");
            farseerTexture = SpriteManager.getImage("LoadingMedia/Farseer");
            infechtTechTexture = SpriteManager.getImage("LoadingMedia/startScreen");
            fadingColor = new Color(0, 0, 0, 255);
            currentTexture = Globals.Pixel;
            state = 'I';
        }

        public char Skip
        {
            set { state = 'X'; }
            get { return state; }
        }

        public char State
        {
            set
            {
                currentTexture = Globals.Pixel;
                state = value; 
            }
            get { return state; }
        }


        public void Update(GameTime gameTime)
        {
            fadingColor = new Color(255, 255, 255, alpha);

            if (state == 'V')// || state == 'O')
            {
                fadingColor = Color.White;
                //if (videoPlayer.State != MediaState.Playing && !outroPlayed && state == 'O')
                //{
                //    currentTexture = Globals.Pixel;
                //}


            }
            if (firstRun)
            {
                int i=0;
                while (i < 60)
                {
                    i++;
                    if (i == 59)
                        firstRun = false;
                }
            }

            if (alpha == 0)
            {
                countingUp = true;
                wasDown = true;
            }
            if (alpha == 254)
            {
                countingUp = false;
            }

            if (countingUp)
            {
                alpha += 2;
            }
            else
            {
                alpha -= 2;
            }



            switch (state)
            {
                case 'I':
                    {
                        if (farsPlayed && gaPlayed && inftPlayed && introPlayed)
                        {
                            currentTexture = Globals.Pixel;
                            state = 'O';
                            break;
                        }
                        if (!gaPlayed)
                        {
                            if (scale != .5f)
                            {
                                currentTexture = gamesAcademyTexture;
                                scale = .5f;
                            }
                            if (wasDown && !countingUp)
                            {
                                
                                wasDown = false;
                                gaPlayed = true;
                            }
                        }
                        if (!inftPlayed && gaPlayed)
                        {
                            if (scale != 1f)
                            {
                                scale = 1f;
                                currentTexture = infechtTechTexture;
                            }
                            if (wasDown && !countingUp)
                            {
                                //String temp = "Video/video_ (" + 1 + ")";
                                //currentTexture = SpriteManager.getImage(temp);
                                wasDown = false;
                                inftPlayed = true;
                                //farsPlayed = true;
                            }
                        }
                        if (!farsPlayed && inftPlayed)
                        {
                            if (currentTexture != farseerTexture)
                            {
                                currentTexture = farseerTexture;
                            }

                            if (wasDown && !countingUp)
                            {
                                currentTexture = SpriteManager.getImage("Video/pixel");
                                wasDown = false;
                                farsPlayed = true;
                            }
                        }

                        if (farsPlayed && gaPlayed && inftPlayed && !introPlayed)
                        {
                            state = 'V';
                        }
                        
                    }
                    break;
                case 'V':
                    {
                        if (videoPlayer.State == MediaState.Stopped && introPlayed)
                        {
                            currentTexture = Globals.Pixel;
                            
                            state = 'X';
                            break;
                        }
                        if (videoPlayer.State != MediaState.Playing)
                        {
                            video = SpriteManager.Content.Load<Video>("Video/Intro");
                            videoPlayer.Play(video);
                            introPlayed = true;
                        }
                        currentTexture = videoPlayer.GetTexture();
 
                        //if (currentFrame == 2)
                        //{
                        //    extra = 120;
                        //}
                        //else
                        //{
                        //    extra = 0;
                        //}
                        //ticker++;
                        //if (ticker >= interval + extra)
                        //{
                        //    currentFrame++;
                        //    ticker = 0;
                        //}
                        //String temp = "Video/video_ (" + currentFrame + ")";
                        //currentTexture = SpriteManager.getImage(temp);
                        //if (currentFrame == 129)
                        //{
                        //    state = 'X';
                        //}
                        
                    }
                    break;
                case 'O':
                    {
                        if (videoPlayer.State != MediaState.Playing && !outroPlayed)
                        {
                            video = SpriteManager.Content.Load<Video>("Video/Outro");
                            videoPlayer.Play(video);
                            outroPlayed = true;
                        }

                        if (videoPlayer.State == MediaState.Stopped && outroPlayed)
                        {
                            currentTexture = Globals.Pixel;
                            state = 'X';
                        }
                        
                        currentTexture = videoPlayer.GetTexture();
 
                    }
                    break;
                case 'X':
                    {
                        currentTexture = Globals.Pixel;
                        gamesAcademyTexture = Globals.Pixel;
                        farseerTexture = Globals.Pixel;
                        infechtTechTexture = Globals.Pixel;
                    }
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            Rectangle tempRect = new Rectangle(0, 0, 1024, 768);
            
            switch (state)
            {
                case 'I':
                    {
                        spriteBatch.Draw(currentTexture, new Vector2(1024 / 2f , 768 / 2f), null, fadingColor, 0f, new Vector2(currentTexture.Width/2f, currentTexture.Height / 2f), scale, SpriteEffects.None, 0f);
                        
                        //fading effect
                        spriteBatch.Draw(Globals.Pixel, tempRect, fadingColor);
                    }
                    break;
                case 'V':
                    {
                        
                        spriteBatch.Draw(currentTexture, tempRect, Color.White);
                    }
                    break;
                case 'O':
                    {
                        spriteBatch.Draw(currentTexture, tempRect, Color.White);
                    }
                    break;
                case 'X':
                    {
                        tempRect = new Rectangle(0, 0, 0, 0);
                        spriteBatch.Draw(currentTexture, tempRect, Color.White);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
