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
using System.Diagnostics;

namespace VirusGame.UI
{
    public class ScoreBoard
    {
        SpriteClasses.Menu.BubblingSprite bubbles;
        UI.Tips tipSystem = new UI.Tips();
        SpriteFont tipFont;

        public bool nextLevel = false;
        public bool sameLevel = false;

        string filename = "savegame.infec";

        //added by chad
        StorageDevice storageDevice;
        IAsyncResult asyncResult;
        StorageContainer storageContainer;
        SavingState savingState = SavingState.NotSaving;

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

        SaveGame saveData = new SaveGame();
        SaveGame loadData = new SaveGame();

        float priorityLive = 10f;
        float priorityCollectibles = 15f;
        float priorityScore = 25f;
        float priorityKills = 10f;
        float priorityTime = 40f;

        public bool errorFreeSave = false;
        #region Declaration
        ContentManager Content;
        GraphicsDevice graphicDevice;

        Texture2D texture00;
        Texture2D texture01;
        Texture2D texture02;
        SpriteFont font;

        Vector2 liquidPosition;

        UI.ScoreBoard_Percent liveScoreboard;
        UI.ScoreBoard_Percent collectiblesScoreboard;
        UI.ScoreBoard_Percent scoreScoreboard;
        UI.ScoreBoard_Percent killScoreboard;
        UI.ScoreBoard_Percent timeScoreboard;
        UI.ScoreBoard_Percent newHighScore;

        public int timer = 0;
        public int step = 1;

        float liquidHeight = 0;


        public int levelID;

        int timeNeeded;
        int levelTime;
        int score;
        int maxScore;
        int lives;
        int maxLives;
        int collected;
        int maxCollectables;
        int restored;
        int kills;
        int maxKills;
        
        

        //added
        int minutes;
        int seconds;
        string timeFromXML;

        public bool handleTimer = false;

        string totalPercent = "0";

        float updatePercent = 0f;

        bool liveHandle = false;
        bool collectiblesHandle = false;
        bool scoreHandle = false;
        bool killsHandle = false;
        bool timeHandle = false;
        bool highScoreHandle = false;

        bool newHighScoreHandle = false;

        float livePercent;
        float collectiblesPercent;
        float scorePercent;
        float killPercent;
        float timePercent;

        float updateHealth = 0;
        float updateCollectible = 0;
        float updateScore = 0;
        float updateKill = 0;
        float updateTime = 0;

        int levelPercent;

        #endregion

        public ScoreBoard(ContentManager _content, GraphicsDevice _graphicDevice)
        {
            Content = _content;
            graphicDevice = _graphicDevice;
            Load();
        }

        void Load()
        {
            //bubbles = SpriteClasses.SpriteManager.addBubblingAnimation();
            texture00 = Content.Load<Texture2D>("UI/scoreBoard00");
            texture01 = Content.Load<Texture2D>("UI/scoreBoard01");
            texture02 = Content.Load<Texture2D>("UI/scoreBoard02");
            font = Content.Load<SpriteFont>("scoreBoard");
            tipFont = Content.Load<SpriteFont>("TipFont");
        }

        void LoadFromDevice()
        {

            //added by chad
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
        }

        //commented out by chad


        //void LoadFromDevice()
        //{
        //    XmlSerializer reader = new XmlSerializer(typeof(SaveGame));
        //    try
        //    {
        //        if (File.Exists(filename))
        //            {
        //                using (FileStream input = File.OpenRead(filename))
        //                {
        //                    loadData = reader.Deserialize(input) as SaveGame;
        //                }
        //            }
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        Console.WriteLine("Your mom is an invalid operation, with loading data from ScoreBoard");
        //    }

        //}

        void SaveToDevice()
        {
            //saveData = new SaveGame();
            saveData.saveLevel0 = loadData.saveLevel0;
            saveData.saveLevel1 = loadData.saveLevel1;
            saveData.saveLevel2 = loadData.saveLevel2;
            saveData.saveLevel3 = loadData.saveLevel3;
            saveData.saveLevel4 = loadData.saveLevel4;
            saveData.saveLevel5 = loadData.saveLevel5;
            saveData.saveLevel6 = loadData.saveLevel6;
            saveData.saveLevel7 = loadData.saveLevel7;
            saveData.level0Played = loadData.level0Played;
            saveData.level0Average = loadData.level0Average;
            saveData.level1Played = loadData.level1Played;
            saveData.level1Average = loadData.level1Average;
            saveData.level2Played = loadData.level2Played;
            saveData.level2Average = loadData.level2Average;
            saveData.level3Played = loadData.level3Played;
            saveData.level3Average = loadData.level3Average;
            saveData.level4Played = loadData.level4Played;
            saveData.level4Average = loadData.level4Average;
            saveData.level5Played = loadData.level5Played;
            saveData.level5Average = loadData.level5Average;
            saveData.level6Played = loadData.level6Played;
            saveData.level6Average = loadData.level6Average;
            saveData.level7Played = loadData.level7Played;
            saveData.level7Average = loadData.level7Average;
            saveData.level0Fastest = loadData.level0Fastest;
            saveData.level1Fastest = loadData.level1Fastest;
            saveData.level2Fastest = loadData.level2Fastest;
            saveData.level3Fastest = loadData.level3Fastest;
            saveData.level4Fastest = loadData.level4Fastest;
            saveData.level5Fastest = loadData.level5Fastest;
            saveData.level6Fastest = loadData.level6Fastest;
            saveData.level7Fastest = loadData.level7Fastest;

            if (levelID == 0)
            {
                if (Convert.ToInt32(loadData.saveLevel0) < levelPercent)
                    saveData.saveLevel0 = Convert.ToString(levelPercent);
                if (Convert.ToInt32(loadData.level0Fastest) > timeNeeded || loadData.level0Fastest == "0")
                    saveData.level0Fastest = Convert.ToString(timeNeeded);
                saveData.level0Average = Convert.ToString(((Convert.ToInt32(loadData.level0Average) * Convert.ToInt32(loadData.level0Played)) + levelPercent) / (Convert.ToInt32(loadData.level0Played) + 1));
                saveData.level0Played = Convert.ToString((Convert.ToInt32(loadData.level0Played) + 1));
            }
            if (levelID == 1)
            {
                if (Convert.ToInt32(loadData.saveLevel1) < levelPercent)
                    saveData.saveLevel1 = Convert.ToString(levelPercent);
                if (Convert.ToInt32(loadData.level1Fastest) > timeNeeded || loadData.level1Fastest == "0")
                    saveData.level1Fastest = Convert.ToString(timeNeeded);
                saveData.level1Average = Convert.ToString(((Convert.ToInt32(loadData.level1Average) * Convert.ToInt32(loadData.level1Played)) + levelPercent) / (Convert.ToInt32(loadData.level1Played) + 1));
                saveData.level1Played = Convert.ToString((Convert.ToInt32(loadData.level1Played) + 1));
            }
            if (levelID == 2)
            {
                if (Convert.ToInt32(loadData.saveLevel2) < levelPercent)
                    saveData.saveLevel2 = Convert.ToString(levelPercent);
                if (Convert.ToInt32(loadData.level2Fastest) > timeNeeded || loadData.level2Fastest == "0")
                    saveData.level2Fastest = Convert.ToString(timeNeeded);
                saveData.level2Average = Convert.ToString(((Convert.ToInt32(loadData.level2Average) * Convert.ToInt32(loadData.level2Played)) + levelPercent) / (Convert.ToInt32(loadData.level2Played) + 1));
                saveData.level2Played = Convert.ToString((Convert.ToInt32(loadData.level2Played) + 1));
            }
            if (levelID == 3)
            {
                if (Convert.ToInt32(loadData.saveLevel3) < levelPercent)
                    saveData.saveLevel3 = Convert.ToString(levelPercent);
                if (Convert.ToInt32(loadData.level3Fastest) > timeNeeded || loadData.level3Fastest == "0")
                    saveData.level3Fastest = Convert.ToString(timeNeeded);
                saveData.level3Average = Convert.ToString(((Convert.ToInt32(loadData.level3Average) * Convert.ToInt32(loadData.level3Played)) + levelPercent) / (Convert.ToInt32(loadData.level3Played) + 1));
                saveData.level3Played = Convert.ToString((Convert.ToInt32(loadData.level3Played) + 1));
            }
            if (levelID == 4)
            {
                if (Convert.ToInt32(loadData.saveLevel4) < levelPercent)
                    saveData.saveLevel4 = Convert.ToString(levelPercent);
                if (Convert.ToInt32(loadData.level4Fastest) > timeNeeded || loadData.level4Fastest == "0")
                    saveData.level4Fastest = Convert.ToString(timeNeeded);
                saveData.level4Average = Convert.ToString(((Convert.ToInt32(loadData.level4Average) * Convert.ToInt32(loadData.level4Played)) + levelPercent) / (Convert.ToInt32(loadData.level4Played) + 1));
                saveData.level4Played = Convert.ToString((Convert.ToInt32(loadData.level4Played) + 1));
            }
            if (levelID == 5)
            {
                if (Convert.ToInt32(loadData.saveLevel5) < levelPercent)
                    saveData.saveLevel5 = Convert.ToString(levelPercent);
                if (Convert.ToInt32(loadData.level5Fastest) > timeNeeded || loadData.level5Fastest == "0")
                    saveData.level5Fastest = Convert.ToString(timeNeeded);
                saveData.level5Average = Convert.ToString(((Convert.ToInt32(loadData.level5Average) * Convert.ToInt32(loadData.level5Played)) + levelPercent) / (Convert.ToInt32(loadData.level5Played) + 1));
                saveData.level5Played = Convert.ToString((Convert.ToInt32(loadData.level5Played) + 1));
            }
            if (levelID == 6)
            {
                if (Convert.ToInt32(loadData.saveLevel6) < levelPercent)
                    saveData.saveLevel6 = Convert.ToString(levelPercent);
                if (Convert.ToInt32(loadData.level6Fastest) > timeNeeded || loadData.level6Fastest == "0")
                    saveData.level6Fastest = Convert.ToString(timeNeeded);
                saveData.level6Average = Convert.ToString(((Convert.ToInt32(loadData.level6Average) * Convert.ToInt32(loadData.level6Played)) + levelPercent) / (Convert.ToInt32(loadData.level6Played) + 1));
                saveData.level6Played = Convert.ToString((Convert.ToInt32(loadData.level6Played) + 1));
            }
            if (levelID == 7)
            {
                if (Convert.ToInt32(loadData.saveLevel7) < levelPercent)
                    saveData.saveLevel7 = Convert.ToString(levelPercent);
                if (Convert.ToInt32(loadData.level7Fastest) > timeNeeded || loadData.level7Fastest == "0")
                    saveData.level7Fastest = Convert.ToString(timeNeeded);
                saveData.level7Average = Convert.ToString(((Convert.ToInt32(loadData.level7Average) * Convert.ToInt32(loadData.level7Played)) + levelPercent) / (Convert.ToInt32(loadData.level7Played) + 1));
                saveData.level7Played = Convert.ToString((Convert.ToInt32(loadData.level7Played) + 1));
            }

            //added by chad
            
            if (savingState == SavingState.NotSaving)
            {
                savingState = SavingState.ReadyToOpenStorageContainer;
            } 
            //commented out by chad
            
            //try
            //{
                
            //    XmlSerializer writer = new XmlSerializer(typeof(SaveGame));
            //    using (FileStream file = File.OpenWrite(filename))
            //    {
            //        StreamWriter write = new StreamWriter(filename);
            //        writer.Serialize(file, saveData);
            //        errorFreeSave = true;
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //    Console.WriteLine("Your mom failed to save the game properly");
            //    errorFreeSave = false;
            //}
            
        }

        private void UpdateSave()
        {
            switch (savingState)
            {
                case SavingState.ReadyToSelectStorageDevice:
                    {
                        asyncResult = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
                        savingState = SavingState.SelectingStorageDevice;
                    }
                    break;
                case SavingState.SelectingStorageDevice:
                    {
                        if (asyncResult.IsCompleted)
                        {
                            storageDevice = StorageDevice.EndShowSelector(asyncResult);
                            savingState = SavingState.ReadyToOpenStorageContainer;
                        }
                    }
                    break;
                case SavingState.ReadyToOpenStorageContainer:
                    {
                        if (storageDevice == null || !storageDevice.IsConnected)
                        {
                            savingState = SavingState.ReadyToSelectStorageDevice;
                        }
                        else
                        {
                            asyncResult = storageDevice.BeginOpenContainer(filename, null, null);
                            savingState = SavingState.OpeningStorageContainer;
                        }
                    }
                    break;

                case SavingState.OpeningStorageContainer:
                    {
                        if (asyncResult.IsCompleted)
                        {
                            storageContainer = storageDevice.EndOpenContainer(asyncResult);
                            savingState = SavingState.ReadyToSave;
                        }
                    }
                    break;
                case SavingState.ReadyToSave:
                    {
                        if (storageContainer == null)
                        {
                            savingState = SavingState.ReadyToOpenStorageContainer;
                        }
                        else
                        {
                            try
                            {
                                DeleteExisting();
                                Save();
                            }
                            catch (IOException e)
                            {
                                // Replace with in game dialog notifying user of error
                                Debug.WriteLine(e.Message);
                            }
                            finally
                            {
                                storageContainer.Dispose();
                                storageContainer = null;
                                savingState = SavingState.NotSaving;
                            }
                        }
                    }
                    break;
                default: break;


            }

        }

        private void DeleteExisting()
        {
            if (storageContainer.FileExists(filename))
            {
                storageContainer.DeleteFile(filename);
            }
        }

        private void Save()
        {
            XmlSerializer writer = new XmlSerializer(typeof(SaveGame));
            using (FileStream file = new FileStream(filename, FileMode.Create))
            {
                TextWriter write = new StreamWriter(file, new UTF8Encoding());
                writer.Serialize(write, saveData);
                write.Close();
                errorFreeSave = true;
            }
        }

        public void Data(GameTime gameTime, int levelid, int _timeNeeded, int _levelTime, int _score, int _maxScore, int _lives, int _collected, int _restored, int _kills, int _maxKills, int _maxCollectables)
        {
            #region initialize
            bubbles = SpriteClasses.SpriteManager.addBubblingAnimation();
            totalPercent = "0";
            liquidHeight = 0;
            nextLevel = false;
            sameLevel = false;
            handleTimer = false;
            step = 1;

            updateHealth = 0;
            updateCollectible = 0;
            updateScore = 0;
            updateKill = 0;
            updateTime = 0;

            levelID = levelid;

            liveHandle = false;
            collectiblesHandle = false;
            scoreHandle = false;
            killsHandle = false;
            timeHandle = false;
            highScoreHandle = false;

            updatePercent = 0f;

            newHighScoreHandle = false;

            timeNeeded = _timeNeeded;
            levelTime = _levelTime;
            score = _score;
            maxScore = _maxScore;
            lives = _lives;
            maxLives = 5;
            collected = _collected;
            maxCollectables = _maxCollectables;
            restored = _restored;
            kills = _kills;
            maxKills = _maxKills;

            #endregion

            livePercent = ((lives * 100f) / maxLives) / (100f/ priorityLive);
            collectiblesPercent = (((collected - restored)*100f) / maxCollectables) / (100f / priorityCollectibles);
            scorePercent = ((score*100f) / maxScore) / (100f / priorityScore);
            killPercent = ((kills*100f) / maxKills) / (100f / priorityKills);
            if (timeNeeded <= levelTime)
                timePercent = 100f;
            else
            {
                if ((timeNeeded - levelTime) < 100)
                {
                    timePercent = 100f - (timeNeeded - levelTime);
                }
                else { timePercent = 0f; }
            }
            timePercent = timePercent / (100f / priorityTime);

            if (livePercent > priorityLive)
                livePercent = priorityLive;
            if (collectiblesPercent > priorityCollectibles)
                collectiblesPercent = priorityCollectibles;
            if (scorePercent > priorityScore)
                scorePercent = priorityScore;
            if (killPercent > priorityKills)
                killPercent = priorityKills;
            if (timePercent > priorityTime)
                timePercent = priorityTime;

            if (lives > 0)
                levelPercent = (int)(livePercent + collectiblesPercent + scorePercent + killPercent + timePercent);
            else
            {
                levelPercent = (int)(livePercent + collectiblesPercent + scorePercent + killPercent);
                timePercent = 0f;
            }

            liveScoreboard =            new UI.ScoreBoard_Percent(new Vector2(0, 0), "Health-Bonus: " + livePercent + "%");
            collectiblesScoreboard =    new UI.ScoreBoard_Percent(new Vector2(0, (((livePercent / 100f) * 420f))), "Collectible-Bonus: " + Math.Round(collectiblesPercent) + "%");
            scoreScoreboard =           new UI.ScoreBoard_Percent(new Vector2(0, (((collectiblesPercent / 100f) * 420f) + ((livePercent / 100f) * 420f))), "Score-Bonus: " + Math.Round(scorePercent) + "%");
            killScoreboard =            new UI.ScoreBoard_Percent(new Vector2(0, (((scorePercent / 100f) * 420f) + ((collectiblesPercent / 100f) * 420f) + ((livePercent / 100f) * 420f))), "Kill-Bonus: " + Math.Round(killPercent) + "%");
            timeScoreboard =            new UI.ScoreBoard_Percent(new Vector2(0, (((scorePercent / 100f) * 420f) + ((collectiblesPercent / 100f) * 420f) + ((livePercent / 100f) * 420f) + ((killPercent / 100f) * 420f))), "Time-Bonus: " + Math.Round(timePercent) + "%");

            if (lives == 0)
            {
                highScoreHandle = true;
                newHighScore = new UI.ScoreBoard_Percent(new Vector2(0, (((scorePercent / 100f) * 420f) + ((collectiblesPercent / 100f) * 420f) + ((livePercent / 100f) * 420f) + ((killPercent / 100f) * 420f) + ((timePercent / 100f) * 420f))), "YOU DIED! TRY AGAIN");
            }
            else
                newHighScore = new UI.ScoreBoard_Percent(new Vector2(0, (((scorePercent / 100f) * 420f) + ((collectiblesPercent / 100f) * 420f) + ((livePercent / 100f) * 420f) + ((killPercent / 100f) * 420f) + ((timePercent / 100f) * 420f))), "NEW HIGHSCORE");

            LoadFromDevice();

            if (lives > 0)
            {
                SaveToDevice();

                if (levelID == 0)
                    if (Convert.ToInt32(loadData.saveLevel4) < levelPercent)
                        newHighScoreHandle = true;
                if (levelID == 1)
                    if (Convert.ToInt32(loadData.saveLevel1) < levelPercent)
                        newHighScoreHandle = true;
                if (levelID == 2)
                    if (Convert.ToInt32(loadData.saveLevel2) < levelPercent)
                        newHighScoreHandle = true;
                if (levelID == 3)
                    if (Convert.ToInt32(loadData.saveLevel3) < levelPercent)
                        newHighScoreHandle = true;
                if (levelID == 4)
                    if (Convert.ToInt32(loadData.saveLevel4) < levelPercent)
                        newHighScoreHandle = true;
                if (levelID == 5)
                    if (Convert.ToInt32(loadData.saveLevel5) < levelPercent)
                        newHighScoreHandle = true;
                if (levelID == 6)
                    if (Convert.ToInt32(loadData.saveLevel6) < levelPercent)
                        newHighScoreHandle = true;
                if (levelID == 7)
                    if (Convert.ToInt32(loadData.saveLevel7) < levelPercent)
                        newHighScoreHandle = true;
            }
        }

        public void Update(Controls playerController, GameTime gameTime)
        {
            //added by chad
            if (savingState != SavingState.NotSaving)
            {
                UpdateSave();
            }

            if (handleTimer)
            {
                if (timer > 0)
                    timer--;
                else { step += 1; handleTimer = false; }

                if (step > 5)
                    step = 5;
            }
            else
            {

                if (step == 1)
                {
                    liveHandle = true;
                    if (liquidHeight < (((livePercent / 100f) * 420f)))
                    {
                        liquidHeight += 1.0f;
                    }
                    else { handleTimer = true; collectiblesHandle = true; timer = 60; }
                }

                if (step == 2)
                {
                    if (liquidHeight < (((livePercent / 100f) * 420f) + ((collectiblesPercent / 100f) * 420f)))
                    {
                        liquidHeight += 1.0f;
                    }
                    else { handleTimer = true; scoreHandle = true; timer = 60; }
                }

                if (step == 3)
                {
                    if (liquidHeight < (((livePercent / 100f) * 420f) + ((collectiblesPercent / 100f) * 420f) + ((scorePercent / 100f) * 420f)))
                    {
                        liquidHeight += 1.0f;
                    }
                    else { handleTimer = true; killsHandle = true; timer = 60; }
                }

                if (step == 4)
                {
                    if (liquidHeight < (((livePercent / 100f) * 420f) + ((collectiblesPercent / 100f) * 420f) + ((scorePercent / 100f) * 420f) + ((killPercent / 100f) * 420f)))
                    {
                        liquidHeight += 1.0f;
                    }
                    else { handleTimer = true; timeHandle = true; timer = 60; }
                }

                if (step == 5 && lives > 0)
                {
                    if (liquidHeight < (((livePercent / 100f) * 420f) + ((collectiblesPercent / 100f) * 420f) + ((scorePercent / 100f) * 420f) + ((killPercent / 100f) * 420f) + ((timePercent / 100f) * 420f)))
                    {
                        liquidHeight += 1.0f;
                    }
                    else
                        if (newHighScoreHandle)
                            highScoreHandle = true;

                }
            }

            totalPercent = Math.Round((liquidHeight / 420f) * 100f) + "";

            if (liveHandle)
                liveScoreboard.Update(true);

            if (collectiblesHandle)
                collectiblesScoreboard.Update();

            if (scoreHandle)
                scoreScoreboard.Update();

            if (killsHandle)
                killScoreboard.Update();

            if (timeHandle)
                timeScoreboard.Update();

            if (highScoreHandle)
                if(lives == 0)
                    newHighScore.Update(true, true, true);
                else
                    newHighScore.Update(true, true);

            if (playerController.accept() && errorFreeSave)
                if (lives == 0)
                    sameLevel = true;
                else
                    nextLevel = true;

           updatePercent = ((liquidHeight*100f) / 420f);

           if (updateHealth < updatePercent && updateHealth <= livePercent)
               updateHealth += 0.42f;
           else if (updateCollectible < updatePercent - livePercent && updateCollectible < collectiblesPercent)
               updateCollectible += 0.42f;
           else if (updateScore < updatePercent - livePercent - collectiblesPercent && updateScore < scorePercent)
               updateScore += 0.42f;
           else if (updateKill < updatePercent - livePercent - collectiblesPercent - scorePercent && updateKill < killPercent)
               updateKill += 0.42f;
           else if (updateTime < updatePercent - livePercent - collectiblesPercent - scorePercent - killPercent && updateTime <= timePercent)
               updateTime += 0.42f;

           bubbles.position = new Vector2(230 + (bubbles.animationManager.Width / 2f), (liquidHeight * -1) + 500f + (bubbles.animationManager.Height / 2f));
           bubbles.Update(gameTime);
           

        }


        public Vector2 timeFromSeconds(int _seconds)
        {
            int tempMin = _seconds / 60;
            int tempSec = _seconds % 60;
            return new Vector2((float)tempMin, (float)tempSec);
        }


        public Vector2 getTimeFromTimer(TimerSprite _timer)
        {
            int tempMin = Convert.ToInt32(_timer.Minutes);
            int tempSec = Convert.ToInt32(_timer.Seconds);

            return new Vector2((float)tempMin, (float)tempSec);
        }

        public Vector2 getTimeFromString(String _time)
        {
            string tempMin = _time[0] + "" + _time[1];
            string tempSec = _time[3] + "" + _time[4];

            int tempMinute = Convert.ToInt32(tempMin);
            int tempSecond = Convert.ToInt32(tempSec);

            return new Vector2((float)tempMinute, (float)tempSecond);
        }

        public bool ALessThanB(Vector2 A, Vector2 B)
        {
            if(A.X < B.X)
            {
                return true;
            }
            if (A.X == B.X)
            {
                if (A.Y < B.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            

                spriteBatch.Draw(texture00, new Vector2(0, 0), Color.White);
                bubbles.Draw(spriteBatch);
                spriteBatch.Draw(texture02, new Vector2(0, 0), Color.White);

                if (Convert.ToInt32(totalPercent) > levelPercent)
                    totalPercent = Convert.ToString(levelPercent);

                spriteBatch.DrawString(font, "" + (Math.Round(updateHealth) + Math.Round(updateCollectible) + Math.Round(updateScore) + Math.Round(updateKill) + Math.Round(updateTime)), new Vector2(456, 500), Color.White * 0.6f, 0f, new Vector2(font.MeasureString(totalPercent).X / 2,font.MeasureString(totalPercent).Y / 2) , 1f, SpriteEffects.None, 0f);

                liveScoreboard.Draw(spriteBatch, font);
                collectiblesScoreboard.Draw(spriteBatch, font);
                scoreScoreboard.Draw(spriteBatch, font);
                killScoreboard.Draw(spriteBatch, font);
                timeScoreboard.Draw(spriteBatch, font);

                newHighScore.Draw(spriteBatch, font);

                tipSystem.drawTip(spriteBatch, tipFont, new Vector2(512, 745));

                spriteBatch.DrawString(font, "Health-Bonus: " + Math.Round(updateHealth), new Vector2(650, 105), Color.White, 0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Collectible-Bonus: " + Math.Round(updateCollectible), new Vector2(650, 145), Color.White, 0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Score-Bonus: " + Math.Round(updateScore), new Vector2(650, 185), Color.White, 0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Kill-Bonus: " + Math.Round(updateKill), new Vector2(650, 225), Color.White, 0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Time-Bonus: " + Math.Round(updateTime), new Vector2(650, 265), Color.White, 0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0f);
                
        }
    }

    //added by chad
    public enum SavingState
    {
        NotSaving,
        ReadyToSelectStorageDevice,
        SelectingStorageDevice,
        ReadyToOpenStorageContainer,
        OpeningStorageContainer,
        ReadyToSave
    }
}

