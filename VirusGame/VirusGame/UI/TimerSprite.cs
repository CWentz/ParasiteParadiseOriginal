using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VirusGame.SpriteClasses;

namespace VirusGame.UI
{
    public class TimerSprite
    {

        #region declarations
        private Vector2 position;
        private Vector2 collectPosition;
        private String minutes;
        private String seconds;
        private Char onesMinutes = '0';
        private Char tensMinutes = '0';
        private Char onesSeconds = '0';
        private Char tensSeconds = '0';
        public bool isVisible = true;
        private float scale = .15f;

        private String maxCollect;
        private String currentCollect;
        private Texture2D collect = SpriteManager.Content.Load<Texture2D>("Images/Numbers/collectUI");
        private Texture2D slash = SpriteManager.getTextTexture('/');

        private Char onesMaxCollect = '0';
        private Char tensMaxCollect = '0';
        private Char onesCollected = '0';
        private Char tensCollected = '0';

        private Texture2D oneCollected;
        private Texture2D tenCollected;
        private Texture2D oneMaxCollect;
        private Texture2D tenMaxCollect;


        private Texture2D oneMin;
        private Texture2D tenMin;
        private Texture2D oneSec;
        private Texture2D tenSec;
        private Texture2D colon = SpriteManager.getTextTexture(':');

        #endregion 

        public TimerSprite(Vector2 _position)
        {
            position = _position;
            collectPosition = position + new Vector2(-50, 0);
            Update(0,0,0,0);
        }


        public String Minutes
        {
            get { return minutes; }
        }

        public String Seconds
        {
            get { return seconds; }
        }

        public void Update(int _minutes, int _seconds, int _collected, int _maxCollect)
        {   
            #region timer control
            minutes = _minutes + "";
            seconds = ((int)_seconds) + "";
            //if (seconds[0] == '6')
            //    seconds[0] = '0';
            if (minutes.Length > 1)
            {
                tensMinutes = minutes[0];
                onesMinutes = minutes[1];
            }
            else
            {
                tensMinutes = '0';
                onesMinutes = minutes[0];
            }


            if (seconds.Length > 1)
            {
                
                tensSeconds = seconds[0];
                onesSeconds = seconds[1];
            }
            else
            {
                tensSeconds = '0';
                onesSeconds = seconds[0];
            }

            tenMin = SpriteManager.getTextTexture(tensMinutes);
            oneMin = SpriteManager.getTextTexture(onesMinutes);
            tenSec = SpriteManager.getTextTexture(tensSeconds);
            oneSec = SpriteManager.getTextTexture(onesSeconds);


            #endregion

            #region collect control
            currentCollect = _collected + "";
            maxCollect = _maxCollect + "";


            if (currentCollect.Length > 1)
            {
                tensCollected = currentCollect[0];
                onesCollected = currentCollect[1];
            }
            else
            {
                tensCollected = '0';
                onesCollected = currentCollect[0];
            }
            if (maxCollect.Length > 1)
            {
                tensMaxCollect = maxCollect[0];
                onesMaxCollect = maxCollect[1];
            }
            else
            {
                tensMaxCollect = '0';
                onesMaxCollect = maxCollect[0];
            }

            tenCollected = SpriteManager.getTextTexture(tensCollected);
            oneCollected = SpriteManager.getTextTexture(onesCollected);
            tenMaxCollect = SpriteManager.getTextTexture(tensMaxCollect);
            oneMaxCollect = SpriteManager.getTextTexture(onesMaxCollect);

            #endregion
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                //900,700
                #region timer
                Vector2 tempVectTime = position;
                tempVectTime.Y -= 50;
                spriteBatch.Draw(SpriteManager.Content.Load<Texture2D>("Images/Numbers/TimeUI"), new Vector2(900 - 150, 700 - 30f)
                    , null, Color.White, 0f, new Vector2(tenMin.Width / 2f, tenMin.Height / 2f), 1f, SpriteEffects.None, 0f);

                spriteBatch.Draw(tenMin, tempVectTime
                    , null, Color.White, 0f, new Vector2(tenMin.Width / 2f, tenMin.Height / 2f) * scale, scale, SpriteEffects.None, 0f);
                tempVectTime.X += (tenMin.Width * scale);
                spriteBatch.Draw(oneMin, tempVectTime
                    , null, Color.White, 0f, new Vector2(oneMin.Width / 2f, oneMin.Height / 2f) * scale, scale, SpriteEffects.None, 0f);
                tempVectTime.X += (oneMin.Width * scale);
                spriteBatch.Draw(colon, tempVectTime//new Vector2(position.X + ((tenMin.Width + oneMin.Width) * scale), position.Y + ((tenMin.Height / 4f) * scale))
                    , null, Color.White, 0f, new Vector2(colon.Width / 2f, colon.Height / 2f) * scale, scale, SpriteEffects.None, 0f);
                tempVectTime.X += (colon.Width * scale);
                spriteBatch.Draw(tenSec, tempVectTime//new Vector2(position.X + ((tenMin.Width + colon.Width + oneMin.Width) * scale), position.Y)
                    , null, Color.White, 0f, new Vector2(tenSec.Width / 2f, tenSec.Height / 2f) * scale, scale, SpriteEffects.None, 0f);
                tempVectTime.X += (tenSec.Width * scale);
                spriteBatch.Draw(oneSec, tempVectTime//new Vector2(position.X + ((tenMin.Width + colon.Width + oneMin.Width + tenSec.Width) * scale), position.Y)
                    , null, Color.White, 0f, new Vector2(oneSec.Width / 2f, oneSec.Height / 2f) * scale, scale, SpriteEffects.None, 0f);
                #endregion
                #region collectables
                Vector2 tempVect = collectPosition;

                spriteBatch.Draw(collect, tempVect
                    , null, Color.White, 0f, new Vector2(collect.Width / 2f, collect.Height / 2f) * scale, 1f, SpriteEffects.None, 0f);
                
                tempVect.X += (collect.Width);
                spriteBatch.Draw(tenCollected, tempVect
                    , null, Color.White, 0f, new Vector2(tenCollected.Width / 2f, tenCollected.Height / 2f) * scale, scale, SpriteEffects.None, 0f);

                tempVect.X += (tenCollected.Width * scale);
                spriteBatch.Draw(oneCollected, tempVect
                    , null, Color.White, 0f, new Vector2(oneCollected.Width / 2f, oneCollected.Height / 2f) * scale, scale, SpriteEffects.None, 0f);

                tempVect.X += (oneCollected.Width * scale);
                spriteBatch.Draw(slash, tempVect
                    , null, Color.White, 0f, new Vector2(slash.Width / 2f, slash.Height / 2f) * scale, scale, SpriteEffects.None, 0f);

                tempVect.X += (slash.Width * scale);
                spriteBatch.Draw(tenMaxCollect, tempVect
                    , null, Color.White, 0f, new Vector2(tenMaxCollect.Width / 2f, tenMaxCollect.Height / 2f) * scale, scale, SpriteEffects.None, 0f);

                tempVect.X += (tenMaxCollect.Width * scale);
                spriteBatch.Draw(oneMaxCollect, tempVect
                    , null, Color.White, 0f, new Vector2(oneMaxCollect.Width / 2f, oneMaxCollect.Height / 2f) * scale, scale, SpriteEffects.None, 0f);
                

                #endregion
            }
        }
    }
}
