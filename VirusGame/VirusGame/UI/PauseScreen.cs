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
    public class PauseScreen
    {
        SpriteClasses.Menu.PauseMenuSprite pauseSprite;
        private int menuItem = 1;
        public bool menuChoosen = false;
        bool menuReady = true;

        public int MenuItem
        {
            get { return menuItem; }
        }

        public void Load(ContentManager content)
        {
            pauseSprite = new SpriteClasses.Menu.PauseMenuSprite(SpriteClasses.SpriteManager.Level, content.Load<Texture2D>("UI/pauseScreen"), new Vector2(0, 0), new Vector2(0, 0), 3, 5);
        }

        public void Update(GameTime gameTime, Controls _controls)
        {

            menuChoosen = false;

            if(_controls.menuUp() || _controls.menuRight())
            {
                menuItem++;
            }
            if (_controls.menuDown() || _controls.menuLeft())
            {
                menuItem--;
            }

            if (_controls.pause())
            {
                menuChoosen = true;
                menuItem = 1;
            }

            if (_controls.accept())
            {
                menuChoosen = true;
            }

            if (menuItem > 2)
                menuItem = 1;
            if (menuItem < 1)
                menuItem = 2;

            pauseSprite.Update(gameTime, menuItem);

            if (menuChoosen)
            {
                pauseSprite.IsVisible = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pauseSprite.Draw(spriteBatch);
        }
    }
}
