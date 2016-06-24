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
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace VirusGame.UI
{
    public class SaveLoadScreen
    {
        GraphicsDevice graphics;
        ContentManager content;

        public bool dataLoaded = false;

        int timer = 30;

        string filename = "savegame.infec";

        [Serializable()]
        public class SaveGame
        {
            public string saveLevel0 = "0";
            public string saveLevel1 = "0";
            public string saveLevel2 = "0";
            public string saveLevel3 = "0";
            public string saveLevel4 = "0";
            public string saveLevel5 = "0";
            public string saveLevel6 = "0";
            public string saveLevel7 = "0";
            public string level0Played = "0";
            public string level0Average = "0";
            public string level1Played = "0";
            public string level1Average = "0";
            public string level2Played = "0";
            public string level2Average = "0";
            public string level3Played = "0";
            public string level3Average = "0";
            public string level4Played = "0";
            public string level4Average = "0";
            public string level5Played = "0";
            public string level5Average = "0";
            public string level6Played = "0";
            public string level6Average = "0";
            public string level7Played = "0";
            public string level7Average = "0";
            public string level0Fastest = "0";
            public string level1Fastest = "0";
            public string level2Fastest = "0";
            public string level3Fastest = "0";
            public string level4Fastest = "0";
            public string level5Fastest = "0";
            public string level6Fastest = "0";
            public string level7Fastest = "0";
        }

        SaveGame loadData = new SaveGame();

        public bool menuChoosen = false;
        public int selected = 0;

        Texture2D background;
        Texture2D overlay;
        Texture2D levelBG;
        Texture2D level0;
        Texture2D level1;
        Texture2D level2;
        Texture2D level3;
        Texture2D level4;
        Texture2D level5;
        Texture2D level6;
        Texture2D level7;
        Texture2D lvl0;
        Texture2D lvl1;
        Texture2D lvl2;
        Texture2D lvl3;
        Texture2D lvl4;
        Texture2D lvl5;
        Texture2D lvl6;
        Texture2D lvl7;
        Texture2D grey;
        Texture2D rahmen;

        SpriteFont font;
        SpriteFont averageFont;

        int level0Percent = 0;
        int level1Percent = 0;
        int level2Percent = 0;
        int level3Percent = 0;
        int level4Percent = 0;
        int level5Percent = 0;
        int level6Percent = 0;
        int level7Percent = 0;

        int playedLevel0 = 0;
        float averageLevel0 = 0f;
        int playedLevel1 = 0;
        float averageLevel1 = 0f;
        int playedLevel2 = 0;
        float averageLevel2 = 0f;
        int playedLevel3 = 0;
        float averageLevel3 = 0f;
        int playedLevel4 = 0;
        float averageLevel4 = 0f;
        int playedLevel5 = 0;
        float averageLevel5 = 0f;
        int playedLevel6 = 0;
        float averageLevel6 = 0f;
        int playedLevel7 = 0;
        float averageLevel7 = 0f;

        int level0Fastest = 0;
        int level1Fastest = 0;
        int level2Fastest = 0;
        int level3Fastest = 0;
        int level4Fastest = 0;
        int level5Fastest = 0;
        int level6Fastest = 0;
        int level7Fastest = 0;

        Vector2 level0Pos;
        Vector2 level1Pos;
        Vector2 level2Pos;
        Vector2 level3Pos;
        Vector2 level4Pos;
        Vector2 level5Pos;
        Vector2 level6Pos;
        Vector2 level7Pos;

        Vector2 minScale = new Vector2(200, 153);
        Vector2 maxScale = new Vector2(400, 305);

        Vector2 level0Size;
        Vector2 level1Size;
        Vector2 level2Size;
        Vector2 level3Size;
        Vector2 level4Size;
        Vector2 level5Size;
        Vector2 level6Size;
        Vector2 level7Size;

        public void Load(ContentManager _content, GraphicsDevice newGraphics)
        {
            content = _content;
            graphics = newGraphics;

            background = content.Load<Texture2D>("UI/LoadSave/background");
            levelBG = content.Load<Texture2D>("UI/LoadSave/level_bg");
            overlay = content.Load<Texture2D>("UI/LoadSave/overlay");
            grey = content.Load<Texture2D>("UI/LoadSave/grey");
            rahmen = content.Load<Texture2D>("UI/LoadSave/rahmen");
            level0 = content.Load<Texture2D>("Levels/level0");
            level1 = content.Load<Texture2D>("Levels/level1");
            level2 = content.Load<Texture2D>("Levels/level2");
            level3 = content.Load<Texture2D>("Levels/level3");
            level4 = content.Load<Texture2D>("Levels/level4");
            level5 = content.Load<Texture2D>("Levels/level5");
            level6 = content.Load<Texture2D>("Levels/level6");
            level7 = content.Load<Texture2D>("Levels/level7");

            lvl0 = content.Load<Texture2D>("UI/LoadSave/lvl0");
            lvl1 = content.Load<Texture2D>("UI/LoadSave/lvl1");
            lvl2 = content.Load<Texture2D>("UI/LoadSave/lvl2");
            lvl3 = content.Load<Texture2D>("UI/LoadSave/lvl3");
            lvl4 = content.Load<Texture2D>("UI/LoadSave/lvl4");
            lvl5 = content.Load<Texture2D>("UI/LoadSave/lvl5");
            lvl6 = content.Load<Texture2D>("UI/LoadSave/lvl6");
            lvl7 = content.Load<Texture2D>("UI/LoadSave/lvl7");

            font = content.Load<SpriteFont>("scoreBoard");
            averageFont = content.Load<SpriteFont>("fpsFont");

        }

        void LoadFromDevice()
        {
            loadData = new SaveGame();
            try
            {
                XmlSerializer reader = new XmlSerializer(typeof(SaveGame));
                if (File.Exists(filename))
                {
                    using (FileStream input = File.OpenRead(filename))
                    {
                        StreamReader read = new StreamReader(filename);
                        loadData = (SaveGame)reader.Deserialize(read);
                        read.Close();
                    }
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Your mom is an invalid operation, when loading data from SaveLoadScreen");
                loadData = new SaveGame();
            }

            dataLoaded = true;
            

        }


        public void Update(Controls key)
        {
            if(!dataLoaded)
                LoadFromDevice();

            level0Percent = Convert.ToInt32(loadData.saveLevel0);
            level1Percent = Convert.ToInt32(loadData.saveLevel1);
            level2Percent = Convert.ToInt32(loadData.saveLevel2);
            level3Percent = Convert.ToInt32(loadData.saveLevel3);
            level4Percent = Convert.ToInt32(loadData.saveLevel4);
            level5Percent = Convert.ToInt32(loadData.saveLevel5);
            level6Percent = Convert.ToInt32(loadData.saveLevel6);
            level7Percent = Convert.ToInt32(loadData.saveLevel7);
            playedLevel0 = Convert.ToInt32(loadData.level0Played);
            averageLevel0 = Convert.ToUInt32(loadData.level0Average);
            playedLevel1 = Convert.ToInt32(loadData.level1Played);
            averageLevel1 = Convert.ToUInt32(loadData.level1Average);
            playedLevel2 = Convert.ToInt32(loadData.level2Played);
            averageLevel2 = Convert.ToUInt32(loadData.level2Average);
            playedLevel3 = Convert.ToInt32(loadData.level3Played);
            averageLevel3 = Convert.ToUInt32(loadData.level3Average);
            playedLevel4 = Convert.ToInt32(loadData.level4Played);
            averageLevel4 = Convert.ToUInt32(loadData.level4Average);
            playedLevel5 = Convert.ToInt32(loadData.level5Played);
            averageLevel5 = Convert.ToUInt32(loadData.level5Average);
            playedLevel6 = Convert.ToInt32(loadData.level6Played);
            averageLevel6 = Convert.ToUInt32(loadData.level6Average);
            playedLevel7 = Convert.ToInt32(loadData.level7Played);
            averageLevel7 = Convert.ToUInt32(loadData.level7Average);

            level0Fastest = Convert.ToInt32(loadData.level0Fastest);
            level1Fastest = Convert.ToInt32(loadData.level1Fastest);
            level2Fastest = Convert.ToInt32(loadData.level2Fastest);
            level3Fastest = Convert.ToInt32(loadData.level3Fastest);
            level4Fastest = Convert.ToInt32(loadData.level4Fastest);
            level5Fastest = Convert.ToInt32(loadData.level5Fastest);
            level6Fastest = Convert.ToInt32(loadData.level6Fastest);
            level7Fastest = Convert.ToInt32(loadData.level7Fastest);

            if (key.moveLeft() && timer == 0)
            {
                if (selected > 0)
                    selected--;

                timer = 30;
            }

            if (key.moveRight() && timer == 0)
            {
                if (selected < 7)
                    selected++;

                timer = 30;
            }

            level0Size = minScale;
            level1Size = minScale;
            level2Size = minScale;
            level3Size = minScale;
            level4Size = minScale;
            level5Size = minScale;
            level6Size = minScale;
            level7Size = minScale;

            level0Pos = new Vector2(2000, 0);
            level1Pos = new Vector2(2000, 0);
            level2Pos = new Vector2(2000, 0);
            level3Pos = new Vector2(2000, 0);
            level4Pos = new Vector2(2000, 0);
            level5Pos = new Vector2(2000, 0);
            level6Pos = new Vector2(2000, 0);
            level7Pos = new Vector2(2000, 0);

            switch (selected)
            {
                case 0:
                    {
                        level0Size = maxScale;
                        level0Pos = new Vector2(0, 0);
                        level1Pos = new Vector2(410, 76);
                    }
                    break;
                case 1:
                    {
                        level1Size = maxScale;
                        level0Pos = new Vector2(-210, 76);
                        level1Pos = new Vector2(0, 0);
                        level2Pos = new Vector2(410, 76);
                    }
                    break;
                case 2:
                    {
                        level2Size = maxScale;
                        level1Pos = new Vector2(-210, 76);
                        level2Pos = new Vector2(0, 0);
                        level3Pos = new Vector2(410, 76);
                    }
                    break;
                case 3:
                    {
                        level3Size = maxScale;
                        level2Pos = new Vector2(-210, 76);
                        level3Pos = new Vector2(0, 0);
                        level4Pos = new Vector2(410, 76);
                    }
                    break;
                case 4:
                    {
                        level4Size = maxScale;
                        level3Pos = new Vector2(-210, 76);
                        level4Pos = new Vector2(0, 0);
                        level5Pos = new Vector2(410, 76);
                    }
                    break;
                case 5:
                    {
                        level5Size = maxScale;
                        level4Pos = new Vector2(-210, 76);
                        level5Pos = new Vector2(0, 0);
                        level6Pos = new Vector2(410, 76);
                    }
                    break;
                case 6:
                    {
                        level6Size = maxScale;
                        level5Pos = new Vector2(-210, 76);
                        level6Pos = new Vector2(0, 0);
                        level7Pos = new Vector2(410, 76);
                    }
                    break;
                case 7:
                    {
                        level7Size = maxScale;
                        level6Pos = new Vector2(-210, 76);
                        level7Pos = new Vector2(0, 0);

                    }
                    break;

                default: break;
            }

            if (key.accept() && timer == 0)
            {
                if (selected == 0)
                    menuChoosen = true;
                if (selected == 1 && level0Percent > 0)
                    menuChoosen = true;
                if (selected == 2 && level1Percent > 0)
                    menuChoosen = true;
                if (selected == 3 && level2Percent > 0)
                    menuChoosen = true;
                if (selected == 4 && level3Percent > 0)
                    menuChoosen = true;
                if (selected == 5 && level4Percent > 0)
                    menuChoosen = true;
                if (selected == 6 && level5Percent > 0)
                    menuChoosen = true;
                if (selected == 7 && level6Percent > 0)
                    menuChoosen = true;

                timer = 30;
            }

            if (timer > 0)
                timer--;
        }

        public string timeFromSeconds(int _seconds)
        {
            int tempMin = _seconds / 60;
            int tempSec = _seconds % 60;
            string seconds = "";
            string minutes = tempMin.ToString();
            if(tempSec < 10)
                seconds = "0" + tempSec;
            else
                seconds = tempSec.ToString();
            return minutes + ":" + seconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {


            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(rahmen, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(levelBG, new Rectangle((int)(150 + level0Pos.X), (int)(210 + level0Pos.Y), (int)level0Size.X, (int)level0Size.Y), Color.White);
            spriteBatch.Draw(levelBG, new Rectangle((int)(150 + level1Pos.X), (int)(210 + level1Pos.Y), (int)level1Size.X, (int)level1Size.Y), Color.White);
            spriteBatch.Draw(levelBG, new Rectangle((int)(150 + level2Pos.X), (int)(210 + level2Pos.Y), (int)level2Size.X, (int)level2Size.Y), Color.White);
            spriteBatch.Draw(levelBG, new Rectangle((int)(150 + level3Pos.X), (int)(210 + level3Pos.Y), (int)level3Size.X, (int)level3Size.Y), Color.White);
            spriteBatch.Draw(levelBG, new Rectangle((int)(150 + level4Pos.X), (int)(210 + level4Pos.Y), (int)level4Size.X, (int)level4Size.Y), Color.White);
            spriteBatch.Draw(levelBG, new Rectangle((int)(150 + level5Pos.X), (int)(210 + level5Pos.Y), (int)level5Size.X, (int)level5Size.Y), Color.White);
            spriteBatch.Draw(levelBG, new Rectangle((int)(150 + level6Pos.X), (int)(210 + level6Pos.Y), (int)level6Size.X, (int)level6Size.Y), Color.White);
            spriteBatch.Draw(levelBG, new Rectangle((int)(150 + level7Pos.X), (int)(210 + level7Pos.Y), (int)level7Size.X, (int)level7Size.Y), Color.White);

            spriteBatch.Draw(level0, new Rectangle((int)(150 + level0Pos.X), (int)(210 + level0Pos.Y), (int)level0Size.X, (int)level0Size.Y), Color.White);
            spriteBatch.Draw(level1, new Rectangle((int)(150 + level1Pos.X), (int)(210 + level1Pos.Y), (int)level1Size.X, (int)level1Size.Y), Color.White);
            spriteBatch.Draw(level2, new Rectangle((int)(150 + level2Pos.X), (int)(210 + level2Pos.Y), (int)level2Size.X, (int)level2Size.Y), Color.White);
            spriteBatch.Draw(level3, new Rectangle((int)(150 + level3Pos.X), (int)(210 + level3Pos.Y), (int)level3Size.X, (int)level3Size.Y), Color.White);
            spriteBatch.Draw(level4, new Rectangle((int)(150 + level4Pos.X), (int)(210 + level4Pos.Y), (int)level4Size.X, (int)level4Size.Y), Color.White);
            spriteBatch.Draw(level5, new Rectangle((int)(150 + level5Pos.X), (int)(210 + level5Pos.Y), (int)level5Size.X, (int)level5Size.Y), Color.White);
            spriteBatch.Draw(level6, new Rectangle((int)(150 + level6Pos.X), (int)(210 + level6Pos.Y), (int)level6Size.X, (int)level6Size.Y), Color.White);
            spriteBatch.Draw(level7, new Rectangle((int)(150 + level7Pos.X), (int)(210 + level7Pos.Y), (int)level7Size.X, (int)level7Size.Y), Color.White);


            if (level0Percent == 0)
                spriteBatch.Draw(grey, new Rectangle((int)(150 + level1Pos.X), (int)(210 + level1Pos.Y), (int)level1Size.X, (int)level1Size.Y), Color.White);
            if (level1Percent == 0)
                spriteBatch.Draw(grey, new Rectangle((int)(150 + level2Pos.X), (int)(210 + level2Pos.Y), (int)level2Size.X, (int)level2Size.Y), Color.White);
            if (level2Percent == 0)
                spriteBatch.Draw(grey, new Rectangle((int)(150 + level3Pos.X), (int)(210 + level3Pos.Y), (int)level3Size.X, (int)level3Size.Y), Color.White);
            if (level3Percent == 0)
                spriteBatch.Draw(grey, new Rectangle((int)(150 + level4Pos.X), (int)(210 + level4Pos.Y), (int)level4Size.X, (int)level4Size.Y), Color.White);
            if (level4Percent == 0)
                spriteBatch.Draw(grey, new Rectangle((int)(150 + level5Pos.X), (int)(210 + level5Pos.Y), (int)level5Size.X, (int)level5Size.Y), Color.White);
            if (level5Percent == 0)
                spriteBatch.Draw(grey, new Rectangle((int)(150 + level6Pos.X), (int)(210 + level6Pos.Y), (int)level6Size.X, (int)level6Size.Y), Color.White);
            if (level6Percent == 0)
                spriteBatch.Draw(grey, new Rectangle((int)(150 + level7Pos.X), (int)(210 + level7Pos.Y), (int)level7Size.X, (int)level7Size.Y), Color.White);

            spriteBatch.Draw(overlay, new Vector2(0, 0), Color.White);

            switch (selected)
            {
                case 0:
                    {
                        spriteBatch.Draw(lvl0, new Vector2(0, 0), Color.White);

                        spriteBatch.DrawString(font, "Tutorial-Level", new Vector2(350, 150), Color.White, 0f, new Vector2(font.MeasureString(Convert.ToString("Tutorial-Level")).X / 2, 0), .3f, SpriteEffects.None, 0f);
                        if (level0Percent > 0)
                            spriteBatch.DrawString(font, Convert.ToString(level0Percent), new Vector2(350, 580), Color.White * 0.8f, 0f, new Vector2(font.MeasureString(Convert.ToString(level0Percent)).X / 2, font.MeasureString(Convert.ToString(level0Percent)).Y / 2), .6f, SpriteEffects.None, 0f);
                        if (playedLevel0 > 0)
                        {
                            spriteBatch.DrawString(averageFont, "Played: " + playedLevel0 + " Average: " + Math.Round(averageLevel0) + " Fastest Run: " + timeFromSeconds(level0Fastest), new Vector2(350, 630), Color.White, 0f, new Vector2(averageFont.MeasureString(Convert.ToString("Played: " + playedLevel0 + " Average: " + Math.Round(averageLevel0) + " Fastest Run: " + timeFromSeconds(level0Fastest))).X / 2, averageFont.MeasureString(Convert.ToString("Played: " + playedLevel0 + " Average: " + Math.Round(averageLevel0))).Y / 2), .8f, SpriteEffects.None, 0f);
                        }
                    }
                    break;
                case 1:
                    {
                        spriteBatch.Draw(lvl1, new Vector2(0, 0), Color.White);

                        spriteBatch.DrawString(font, "Level: 1", new Vector2(350, 150), Color.White, 0f, new Vector2(font.MeasureString(Convert.ToString("Level: 1")).X / 2, 0), .3f, SpriteEffects.None, 0f);
                        if (level1Percent > 0)
                            spriteBatch.DrawString(font, Convert.ToString(level1Percent), new Vector2(350, 580), Color.White * 0.8f, 0f, new Vector2(font.MeasureString(Convert.ToString(level1Percent)).X / 2, font.MeasureString(Convert.ToString(level1Percent)).Y / 2), .6f, SpriteEffects.None, 0f);
                        if (playedLevel1 > 0)
                        {
                            spriteBatch.DrawString(averageFont, "Played: " + playedLevel1 + " Average: " + Math.Round(averageLevel1) + " Fastest Run: " + timeFromSeconds(level1Fastest), new Vector2(350, 630), Color.White, 0f, new Vector2(averageFont.MeasureString(Convert.ToString("Played: " + playedLevel1 + " Average: " + Math.Round(averageLevel1) + " Fastest Run: " + timeFromSeconds(level1Fastest))).X / 2, averageFont.MeasureString(Convert.ToString("Played: " + playedLevel1 + " Average: " + Math.Round(averageLevel1))).Y / 2), .8f, SpriteEffects.None, 0f);
                        }
                    }
                    break;
                case 2:
                    {
                        spriteBatch.Draw(lvl2, new Vector2(0, 0), Color.White);

                        spriteBatch.DrawString(font, "Level: 2", new Vector2(350, 150), Color.White, 0f, new Vector2(font.MeasureString(Convert.ToString("Level: 2")).X / 2, 0), .3f, SpriteEffects.None, 0f);
                        if (level2Percent > 0)
                            spriteBatch.DrawString(font, Convert.ToString(level2Percent), new Vector2(350, 580), Color.White * 0.8f, 0f, new Vector2(font.MeasureString(Convert.ToString(level2Percent)).X / 2, font.MeasureString(Convert.ToString(level2Percent)).Y / 2), .6f, SpriteEffects.None, 0f);
                        if (playedLevel2 > 0)
                        {
                            spriteBatch.DrawString(averageFont, "Played: " + playedLevel2 + " Average: " + Math.Round(averageLevel2) + " Fastest Run: " + timeFromSeconds(level2Fastest), new Vector2(350, 630), Color.White, 0f, new Vector2(averageFont.MeasureString(Convert.ToString("Played: " + playedLevel2 + " Average: " + Math.Round(averageLevel2) + " Fastest Run: " + timeFromSeconds(level2Fastest))).X / 2, averageFont.MeasureString(Convert.ToString("Played: " + playedLevel2 + " Average: " + Math.Round(averageLevel2))).Y / 2), .8f, SpriteEffects.None, 0f);
                        }
                    }
                    break;
                case 3:
                    {
                        spriteBatch.Draw(lvl3, new Vector2(0, 0), Color.White);

                        spriteBatch.DrawString(font, "Level: 3", new Vector2(350, 150), Color.White, 0f, new Vector2(font.MeasureString(Convert.ToString("Level: 3")).X / 2, 0), .3f, SpriteEffects.None, 0f);
                        if (level3Percent > 0)
                            spriteBatch.DrawString(font, Convert.ToString(level3Percent), new Vector2(350, 580), Color.White * 0.8f, 0f, new Vector2(font.MeasureString(Convert.ToString(level0Percent)).X / 2, font.MeasureString(Convert.ToString(level0Percent)).Y / 2), .6f, SpriteEffects.None, 0f);
                        if (playedLevel3 > 0)
                        {
                            spriteBatch.DrawString(averageFont, "Played: " + playedLevel3 + " Average: " + Math.Round(averageLevel3) + " Fastest Run: " + timeFromSeconds(level3Fastest), new Vector2(350, 630), Color.White, 0f, new Vector2(averageFont.MeasureString(Convert.ToString("Played: " + playedLevel3 + " Average: " + Math.Round(averageLevel3) + " Fastest Run: " + timeFromSeconds(level3Fastest))).X / 2, averageFont.MeasureString(Convert.ToString("Played: " + playedLevel3 + " Average: " + Math.Round(averageLevel3))).Y / 2), .8f, SpriteEffects.None, 0f);
                        }
                    }
                    break;
                case 4:
                    {
                        spriteBatch.Draw(lvl4, new Vector2(0, 0), Color.White);

                        spriteBatch.DrawString(font, "Level: 4", new Vector2(350, 150), Color.White, 0f, new Vector2(font.MeasureString(Convert.ToString("Level: 4")).X / 2, 0), .3f, SpriteEffects.None, 0f);
                        if (level4Percent > 0)
                            spriteBatch.DrawString(font, Convert.ToString(level4Percent), new Vector2(350, 580), Color.White * 0.8f, 0f, new Vector2(font.MeasureString(Convert.ToString(level4Percent)).X / 2, font.MeasureString(Convert.ToString(level4Percent)).Y / 2), .6f, SpriteEffects.None, 0f);
                        if (playedLevel4 > 0)
                        {
                            spriteBatch.DrawString(averageFont, "Played: " + playedLevel4 + " Average: " + Math.Round(averageLevel4) + " Fastest Run: " + timeFromSeconds(level4Fastest), new Vector2(350, 630), Color.White, 0f, new Vector2(averageFont.MeasureString(Convert.ToString("Played: " + playedLevel4 + " Average: " + Math.Round(averageLevel4) + " Fastest Run: " + timeFromSeconds(level4Fastest))).X / 2, averageFont.MeasureString(Convert.ToString("Played: " + playedLevel4 + " Average: " + Math.Round(averageLevel4))).Y / 2), .8f, SpriteEffects.None, 0f);
                        }
                    }
                    break;
                case 5:
                    {
                        spriteBatch.Draw(lvl5, new Vector2(0, 0), Color.White);

                        spriteBatch.DrawString(font, "Level: 5", new Vector2(350, 150), Color.White, 0f, new Vector2(font.MeasureString(Convert.ToString("Level: 5")).X / 2, 0), .3f, SpriteEffects.None, 0f);
                        if (level5Percent > 0)
                            spriteBatch.DrawString(font, Convert.ToString(level5Percent), new Vector2(350, 580), Color.White * 0.8f, 0f, new Vector2(font.MeasureString(Convert.ToString(level5Percent)).X / 2, font.MeasureString(Convert.ToString(level5Percent)).Y / 2), .6f, SpriteEffects.None, 0f);
                        if (playedLevel5 > 0)
                        {
                            spriteBatch.DrawString(averageFont, "Played: " + playedLevel5 + " Average: " + Math.Round(averageLevel5) + " Fastest Run: " + timeFromSeconds(level5Fastest), new Vector2(350, 630), Color.White, 0f, new Vector2(averageFont.MeasureString(Convert.ToString("Played: " + playedLevel5 + " Average: " + Math.Round(averageLevel5) + " Fastest Run: " + timeFromSeconds(level5Fastest))).X / 2, averageFont.MeasureString(Convert.ToString("Played: " + playedLevel5 + " Average: " + Math.Round(averageLevel5))).Y / 2), .8f, SpriteEffects.None, 0f);
                        }
                    }
                    break;
                case 6:
                    {
                        spriteBatch.Draw(lvl6, new Vector2(0, 0), Color.White);

                        spriteBatch.DrawString(font, "Level: 6", new Vector2(350, 150), Color.White, 0f, new Vector2(font.MeasureString(Convert.ToString("Level: 6")).X / 2, 0), .3f, SpriteEffects.None, 0f);
                        if (level6Percent > 0)
                            spriteBatch.DrawString(font, Convert.ToString(level6Percent), new Vector2(350, 580), Color.White * 0.8f, 0f, new Vector2(font.MeasureString(Convert.ToString(level6Percent)).X / 2, font.MeasureString(Convert.ToString(level6Percent)).Y / 2), .6f, SpriteEffects.None, 0f);
                        if (playedLevel6 > 0)
                        {
                            spriteBatch.DrawString(averageFont, "Played: " + playedLevel6 + " Average: " + Math.Round(averageLevel6) + " Fastest Run: " + timeFromSeconds(level6Fastest), new Vector2(350, 630), Color.White, 0f, new Vector2(averageFont.MeasureString(Convert.ToString("Played: " + playedLevel6 + " Average: " + Math.Round(averageLevel6) + " Fastest Run: " + timeFromSeconds(level6Fastest))).X / 2, averageFont.MeasureString(Convert.ToString("Played: " + playedLevel6 + " Average: " + Math.Round(averageLevel6))).Y / 2), .8f, SpriteEffects.None, 0f);
                        }
                    }
                    break;
                case 7:
                    {
                        spriteBatch.Draw(lvl7, new Vector2(0, 0), Color.White);

                        spriteBatch.DrawString(font, "Level: 7", new Vector2(350, 150), Color.White, 0f, new Vector2(font.MeasureString(Convert.ToString("Level: 7")).X / 2, 0), .3f, SpriteEffects.None, 0f);
                        if (level7Percent > 0)
                            spriteBatch.DrawString(font, Convert.ToString(level7Percent), new Vector2(350, 580), Color.White * 0.8f, 0f, new Vector2(font.MeasureString(Convert.ToString(level7Percent)).X / 2, font.MeasureString(Convert.ToString(level7Percent)).Y / 2), .6f, SpriteEffects.None, 0f);
                        if (playedLevel7 > 0)
                        {
                            spriteBatch.DrawString(averageFont, "Played: " + playedLevel7 + " Average: " + Math.Round(averageLevel7) + " Fastest Run: " + timeFromSeconds(level7Fastest), new Vector2(350, 630), Color.White, 0f, new Vector2(averageFont.MeasureString(Convert.ToString("Played: " + playedLevel7 + " Average: " + Math.Round(averageLevel7) + " Fastest Run: " + timeFromSeconds(level7Fastest))).X / 2, averageFont.MeasureString(Convert.ToString("Played: " + playedLevel7 + " Average: " + Math.Round(averageLevel7))).Y / 2), .8f, SpriteEffects.None, 0f);
                        }
                    }
                    break;


                default: break;

            }

           
        }
    }
}
