using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace VirusGame.UI
{
    public class StartScreen
    {
        UI.Tips tipSystem = new UI.Tips();

        GraphicsDevice graphics;

        SpriteFont font;

        public string[] tips;

        UI.MainMenuAnimation animation;

        Texture2D texture;
        Texture2D start;
        //tutorial/credits from chad
        Texture2D tutorial;
        Texture2D tutorialH;
        Texture2D credits;
        Texture2D creditsH;

        Texture2D startH;
        Texture2D load;
        Texture2D loadH;
        Texture2D exit;
        Texture2D exitH;

        public int menuItem = 1;
        public bool menuChoosen = false;
        bool menuReady = true;

        public void Load(ContentManager content, GraphicsDevice newGraphics)
        {

            animation = SpriteClasses.SpriteManager.addMainMenuAnimtion();
            animation.IsVisible = true;

            font = content.Load<SpriteFont>("TipFont");

            texture = content.Load<Texture2D>("UI/startScreen");
            start = content.Load<Texture2D>("UI/Start_on");
            startH = content.Load<Texture2D>("UI/Start_off");
            //load = content.Load<Texture2D>("UI/Load_on");
            //loadH = content.Load<Texture2D>("UI/Load_off");
            exit = content.Load<Texture2D>("UI/Exit_on");
            exitH = content.Load<Texture2D>("UI/Exit_off");
            tutorial = content.Load<Texture2D>("UI/Tutorial_on");
            tutorialH = content.Load<Texture2D>("UI/Tutorial_off");
            credits = content.Load<Texture2D>("UI/Credit_on");
            creditsH = content.Load<Texture2D>("UI/Credit_off");
            
            graphics = newGraphics;
        }

        public void Update(Controls _controls, GameTime gameTime)
        {
            animation.Update(gameTime);

            menuChoosen = false;
            if (_controls.menuUp())
            {
                menuItem--;
            }

            if (_controls.menuDown())
            {
                menuItem++;
            }
            

            if (menuItem > 4)
                menuItem = 1;
            if (menuItem < 1)
                menuItem = 4;


            if (_controls.accept())
                menuChoosen = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(0, 0), Color.White);
            Color color = Color.White;

            Vector2 tempVect = new Vector2(100, 300);

            if (menuItem != 4)
                spriteBatch.Draw(tutorialH, tempVect, Color.White);
            else
                spriteBatch.Draw(tutorial, tempVect, Color.White);

            tempVect.Y += 100f;

            if (menuItem != 1)
                spriteBatch.Draw(startH, tempVect, Color.White);
            else
                spriteBatch.Draw(start, tempVect, Color.White);

            tempVect.Y += 100f;

            if (menuItem != 2)
                spriteBatch.Draw(creditsH, tempVect, Color.White);
            else
                spriteBatch.Draw(credits, tempVect, Color.White);


            tempVect.Y += 100f;

            if (menuItem != 3)
                spriteBatch.Draw(exitH, tempVect, Color.White);
            else
                spriteBatch.Draw(exit, tempVect, Color.White);

            
            


            animation.Draw(spriteBatch);

            tipSystem.drawTip(spriteBatch, font, new Vector2(512, 749));

        }


    }
}
