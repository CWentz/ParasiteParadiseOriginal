using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VirusGame.SpriteClasses;

namespace VirusGame.UI
{
    public class CreditScreen
    {
        private float creditScale = 1f;
        private float professionScale = .61f;
        private float nameScale = .3721f;
        private float titleScale = .2269f;
        private float largeGap = 300f;
        private float bigGap = 183f;
        private float mediumGap = 111.63f;
        private float smalGap = 68f;


        private Vector2 position = new Vector2( 100, 100);

        public CreditScreen()
        {

        }

        public void Update(GameTime gametime)
        {
            position.Y -= 1.614f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 tempPos = position;
            Texture2D tempText = SpriteManager.getImage("LoadingMedia/creditScreen");
            SpriteFont font = SpriteManager.Content.Load<SpriteFont>("scoreBoard");
            spriteBatch.Draw(tempText, new Rectangle(0, 0, 1024, 768), Color.White);

            

            spriteBatch.DrawString(font, "Credits", tempPos, Color.White, 0f, new Vector2(0, 0), creditScale, SpriteEffects.None, 0f);
            tempPos.Y += largeGap;
            spriteBatch.DrawString(font, "Designers", tempPos, Color.White, 0f, new Vector2(0, 0), professionScale, SpriteEffects.None, 0f);
            tempPos.Y += bigGap;
            spriteBatch.DrawString(font, "Martin Quarz", tempPos, Color.White, 0f, new Vector2(0, 0), nameScale, SpriteEffects.None, 0f);
            tempPos.Y += smalGap;
            spriteBatch.DrawString(font, "Team Leader / Game Designer / Sound Designer", tempPos, Color.White, 0f, new Vector2(0, 0), titleScale, SpriteEffects.None, 0f);
            tempPos.Y += mediumGap;
            spriteBatch.DrawString(font, "Emily Schuhmann", tempPos, Color.White, 0f, new Vector2(0, 0), nameScale, SpriteEffects.None, 0f);
            tempPos.Y += smalGap;
            spriteBatch.DrawString(font, "Game Designer", tempPos, Color.White, 0f, new Vector2(0, 0), titleScale, SpriteEffects.None, 0f);
            tempPos.Y += largeGap;
            spriteBatch.DrawString(font, "Artists ", tempPos, Color.White, 0f, new Vector2(0, 0), professionScale, SpriteEffects.None, 0f);
            tempPos.Y += bigGap;
            spriteBatch.DrawString(font, "Jan Tverdik", tempPos, Color.White, 0f, new Vector2(0, 0), nameScale, SpriteEffects.None, 0f);
            tempPos.Y += smalGap;
            spriteBatch.DrawString(font, "Game Artist / Animator", tempPos, Color.White, 0f, new Vector2(0, 0), titleScale, SpriteEffects.None, 0f);
            tempPos.Y += mediumGap; 
            spriteBatch.DrawString(font, "Eric Vetter", tempPos, Color.White, 0f, new Vector2(0, 0), nameScale, SpriteEffects.None, 0f);
            tempPos.Y += smalGap;
            spriteBatch.DrawString(font, "Game Artist / Animator", tempPos, Color.White, 0f, new Vector2(0, 0), titleScale, SpriteEffects.None, 0f);
            tempPos.Y += largeGap; 
            spriteBatch.DrawString(font, "Programming", tempPos, Color.White, 0f, new Vector2(0, 0), professionScale, SpriteEffects.None, 0f);
            tempPos.Y += bigGap;
            spriteBatch.DrawString(font, "Chad Wentz", tempPos, Color.White, 0f, new Vector2(0, 0), nameScale, SpriteEffects.None, 0f);
            tempPos.Y += smalGap;
            spriteBatch.DrawString(font, "Programmer", tempPos, Color.White, 0f, new Vector2(0, 0), titleScale, SpriteEffects.None, 0f);
            tempPos.Y += mediumGap;
            spriteBatch.DrawString(font, "Kevin Collmer", tempPos, Color.White, 0f, new Vector2(0, 0), nameScale, SpriteEffects.None, 0f);
            tempPos.Y += smalGap;
            spriteBatch.DrawString(font, "Programmer", tempPos, Color.White, 0f, new Vector2(0, 0), titleScale, SpriteEffects.None, 0f);
            tempPos.Y += bigGap;
            //spriteBatch.DrawString(font, "test", tempPos, Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
            //tempPos.Y += 200;
            //spriteBatch.DrawString(font, "test", tempPos, Color.White, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 0f);
            //tempPos.Y += 200;
            if (tempPos.Y < 0)
                position.Y = 800;
        }

    }
}
