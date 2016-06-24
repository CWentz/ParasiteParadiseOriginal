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
    public class LoadingScreen
    {
        GraphicsDevice graphics;
        Texture2D texture;
        Texture2D levelbackground;
        List<Texture2D> levelTextures = new List<Texture2D>();

        public void Load(ContentManager content, GraphicsDevice newGraphics)
        {
            graphics = newGraphics;
            texture = content.Load<Texture2D>("UI/startScreen");
            levelbackground = content.Load<Texture2D>("UI/LoadSave/level_");
            levelTextures.Add(content.Load<Texture2D>("UI/LoadSave/level1"));
            levelTextures.Add(content.Load<Texture2D>("UI/LoadSave/level2"));
            levelTextures.Add(content.Load<Texture2D>("UI/LoadSave/level3"));
            levelTextures.Add(content.Load<Texture2D>("UI/LoadSave/level4"));
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(0, 0), Color.White);
        }
    }
}
