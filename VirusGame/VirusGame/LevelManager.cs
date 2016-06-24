using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VirusGame._Levels;

namespace VirusGame
{
    public class LevelManager
    {
        ContentManager Content;
        GraphicsDevice graphicDevice;
        private LevelMain currentLevel;

        UI.ScoreBoard scoreBoard;

        int timer = 220;

        public int level0Time = 20;
        public int level1Time = 40;
        public int level2Time = 50;
        public int level3Time = 50;
        public int level4Time = 45;
        public int level5Time = 55;
        public int level6Time = 60;
        public int level7Time = 60;
        public int level8Time = 60;
        public int level9Time = 60;

        private bool gdReset = false;
        public bool levelDone = false;
        public bool soundStopped;
        public bool toggleDB;
        public int levelid;

        public bool showOutro = false;
        public bool showScoreBoard = false;
        
        public Vector2 followCord;
        public Vector2 gravity;
        public Vector2 playerPos;
        public float playerRotation;

        public Level0 level0;
        public Level1 level1;
        public Level2 level2;
        public Level3 level3;
        public Level4 level4;
        public Level5 level5;
        public Level6 level6;
        public Level7 level7;

        private int timerSeconds;
        private int currentLevelMinutes;
        private int currentLevelSeconds;
        private int currentCollects;
        private int currentMaxCollects;
        public bool doTutorial;

        public int TimerSeconds
        {
            get { return timerSeconds; }
        }

        public int CurrentMinutes
        {
            get { return currentLevelMinutes; }
        }
        public int CurrentSeconds
        {
            get { return currentLevelSeconds; }
        }
        public int CurrentCollects
        {
            get { return currentCollects; }
        }
        public int CurrentMaxCollects
        {
            get { return currentMaxCollects; }
        }

        public LevelMain CurrentLevel
        {
            set { currentLevel = value; }
            get { return currentLevel; }
        }


        public LevelManager(int _levelid, ContentManager _content, GraphicsDevice _graphicDevice, UI.ScoreBoard _scoreBoard)
        {
            levelid = _levelid;
            Content = _content;
            graphicDevice = _graphicDevice;
            scoreBoard = _scoreBoard;

            level0 = new Level0(graphicDevice, "level0.gleed");
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            switch (levelid)
            {
                case 0:
                    {
                        
                        if (currentLevel != level0)
                            currentLevel = level0;

                        
                        if (level0.levelDone)
                        {
                            levelDone = level0.levelDone;
                        }
                        else if(levelDone)
                        {
                            level0.levelDone = true;
                        }
                        else
                        {
                            levelDone = false;
                        }

                        if (doTutorial)
                        {
                            level0.doTutorial = true;
                        }
                        else
                        {
                            level0.doTutorial = false;
                        }
                        if (level0.GameOverBiatch)
                        {
                            timer--;
                            if (timer <= 0)
                            {
                                levelDone = true;
                                timer = 220;
                            }
                        }
                        followCord = level0.followCoord;
                        gravity = level0.world.Gravity;
                        playerPos = level0.playerPosition;
                        playerRotation = level0.playerRotation;
                        level0.Update(gameTime, keyboardState);
                        currentLevelMinutes = level0.Minutes;
                        currentLevelSeconds = level0.Seconds;
                        currentCollects = level0.Collected;
                        currentMaxCollects = level0.MaxCollectables;
                        timerSeconds = level0.timerSeconds;

                        if (levelDone)
                        {
                            scoreBoard.Data(gameTime, levelid, level0.Seconds, level0Time, level0.Score, level0.MaxScore,
                                level0.PlayerLives, level0.Collected, level0.Restored, level0.Kills, level0.MaxKills, level0.MaxCollectables);
                            if (!soundStopped)
                            {
                                level0.StopSound();
                                soundStopped = true;
                            }
                            showScoreBoard = true;
                            levelDone = false;
                        }
                        
                        toggleDB = level0.toggleDB;
                    }
                    break;
                case 1:
                    {
                        if (currentLevel != level1)
                            currentLevel = level1;



                        if (level1.levelDone )
                            levelDone = true;
                        else if (levelDone)
                            level1.levelDone = true;
                        else
                            levelDone = false;

                        if(level1.GameOverBiatch)
                        {
                            timer--;
                            if(timer <= 0)
                            {
                                levelDone = true;
                                timer = 220;
                            }
                        }
                        followCord = level1.followCoord;
                        gravity = level1.world.Gravity;
                        playerPos = level1.playerPosition;
                        playerRotation = level1.playerRotation;
                        level1.Update(gameTime, keyboardState);
                        currentLevelMinutes = level1.Minutes;
                        currentLevelSeconds = level1.Seconds;
                        currentCollects = level1.Collected;
                        currentMaxCollects = level1.MaxCollectables;
                        timerSeconds = level1.timerSeconds;
                        
                        if (levelDone)
                        {
                            scoreBoard.Data(gameTime, levelid, level1.Seconds, level1Time, level1.Score, level1.MaxScore, 
                                level1.PlayerLives, level1.Collected, level1.Restored, level1.Kills, level1.MaxKills, level1.MaxCollectables);
                            if (!soundStopped)
                            {
                                level1.StopSound();
                                soundStopped = true;
                            }
                            showScoreBoard = true;
                            levelDone = false;
                        }
                        toggleDB = level1.toggleDB;
                    }
                    break;
                case 2:
                    {
                        if (currentLevel != level2)
                            currentLevel = level2;



                        if (level2.levelDone )
                            levelDone = true;
                        else if (levelDone)
                            level2.levelDone = true;
                        else
                            levelDone = false;


                        if (level2.GameOverBiatch)
                        {
                            timer--;
                            if (timer <= 0)
                            {
                                levelDone = true;
                                timer = 220;
                            }
                        }
                        followCord = level2.followCoord;
                        gravity = level2.world.Gravity;
                        playerPos = level2.playerPosition;
                        playerRotation = level2.playerRotation;
                        level2.Update(gameTime, keyboardState);
                        currentLevelMinutes = level2.Minutes;
                        currentLevelSeconds = level2.Seconds;
                        currentCollects = level2.Collected;
                        currentMaxCollects = level2.MaxCollectables;
                        timerSeconds = level2.timerSeconds;
                        if (levelDone)
                        {
                            scoreBoard.Data(gameTime, levelid, level2.Seconds, level2Time, level2.Score, level2.MaxScore,
                                level2.PlayerLives, level2.Collected, level2.Restored, level2.Kills, level2.MaxKills, level2.MaxCollectables);
                            if (!soundStopped)
                            {
                                level2.StopSound();
                                soundStopped = true;
                            }
                            showScoreBoard = true;
                            levelDone = false;
                        }
                        toggleDB = level2.toggleDB;
                    }
                    break;
                case 3:
                    {
                        if (currentLevel != level3)
                            currentLevel = level3;



                        if (level3.levelDone )
                            levelDone = true;
                        else if (levelDone)
                            level3.levelDone = true;
                        else
                            levelDone = false;
                        
                        if (level3.GameOverBiatch)
                        {
                            timer--;
                            if (timer <= 0)
                            {
                                levelDone = true;
                                timer = 220;
                            }
                        }
                        followCord = level3.followCoord;
                        gravity = level3.world.Gravity;
                        playerPos = level3.playerPosition;
                        playerRotation = level3.playerRotation;
                        level3.Update(gameTime, keyboardState);
                        currentLevelMinutes = level3.Minutes;
                        currentLevelSeconds = level3.Seconds;
                        currentCollects = level3.Collected;
                        currentMaxCollects = level3.MaxCollectables;
                        timerSeconds = level3.timerSeconds;
                        if (levelDone)
                        {
                            scoreBoard.Data(gameTime, levelid, level3.Seconds, level3Time, level3.Score, level3.MaxScore,
                                level3.PlayerLives, level3.Collected, level3.Restored, level3.Kills, level3.MaxKills, level3.MaxCollectables);
                            if (!soundStopped)
                            {
                                level3.StopSound();
                                soundStopped = true;
                            }
                            showScoreBoard = true;
                            levelDone = false;
                        }
                        toggleDB = level3.toggleDB;
                    }
                    break;
                case 4:
                    {
                        if (currentLevel != level4)
                            currentLevel = level4;
                        if (level4.levelDone )
                            levelDone = true;
                        else if (levelDone)
                            level4.levelDone = true;
                        else
                            levelDone = false;
                        if (level4.GameOverBiatch)
                        {
                            timer--;
                            if (timer <= 0)
                            {
                                levelDone = true;
                                timer = 220;
                            }
                        }
                        followCord = level4.followCoord;
                        gravity = level4.world.Gravity;
                        playerPos = level4.playerPosition;
                        playerRotation = level4.playerRotation;
                        level4.Update(gameTime, keyboardState);
                        currentLevelMinutes = level4.Minutes;
                        currentLevelSeconds = level4.Seconds;
                        currentCollects = level4.Collected;
                        currentMaxCollects = level4.MaxCollectables;
                        timerSeconds = level4.timerSeconds;
                        if (levelDone)
                        {

                            scoreBoard.Data(gameTime, levelid, level4.Seconds, level4Time, level4.Score, level4.MaxScore,
                                level4.PlayerLives, level4.Collected, level4.Restored, level4.Kills, level4.MaxKills, level4.MaxCollectables);
                            if (!soundStopped)
                            {
                                level4.StopSound();
                                soundStopped = true;
                            }
                            showScoreBoard = true;
                            levelDone = false;
                        }
                        toggleDB = level4.toggleDB;
                    }
                    break;
                case 5:
                    {
                        if (currentLevel != level5)
                            currentLevel = level5;
                        if (level5.levelDone )
                            levelDone = true;
                        else if (levelDone)
                            level5.levelDone = true;
                        else
                            levelDone = false;
                        if (level5.GameOverBiatch)
                        {
                            timer--;
                            if (timer <= 0)
                            {
                                levelDone = true;
                                timer = 220;
                            }
                        }
                        followCord = level5.followCoord;
                        gravity = level5.world.Gravity;
                        playerPos = level5.playerPosition;
                        playerRotation = level5.playerRotation;
                        level5.Update(gameTime, keyboardState);
                        currentLevelMinutes = level5.Minutes;
                        currentLevelSeconds = level5.Seconds;
                        currentCollects = level5.Collected;
                        currentMaxCollects = level5.MaxCollectables;
                        timerSeconds = level5.timerSeconds;
                        if (levelDone)
                        {

                            scoreBoard.Data(gameTime, levelid, level5.Seconds, level5Time, level5.Score, level5.MaxScore,
                                level5.PlayerLives, level5.Collected, level5.Restored, level5.Kills, level5.MaxKills, level5.MaxCollectables);
                            if (!soundStopped)
                            {
                                level5.StopSound();
                                soundStopped = true;
                            }
                            showScoreBoard = true;
                            levelDone = false;
                        }
                        toggleDB = level5.toggleDB;
                    }
                    break;
                case 6:
                    {
                        if (currentLevel != level6)
                            currentLevel = level6;
                        if (level6.levelDone )
                            levelDone = true;
                        else if (levelDone)
                            level6.levelDone = true;
                        else
                            levelDone = false;
                        if (level6.GameOverBiatch)
                        {
                            timer--;
                            if (timer <= 0)
                            {
                                levelDone = true;
                                timer = 220;
                            }
                        }
                        followCord = level6.followCoord;
                        gravity = level6.world.Gravity;
                        playerPos = level6.playerPosition;
                        playerRotation = level6.playerRotation;
                        level6.Update(gameTime, keyboardState);
                        currentLevelMinutes = level6.Minutes;
                        currentLevelSeconds = level6.Seconds;
                        currentCollects = level6.Collected;
                        currentMaxCollects = level6.MaxCollectables;
                        timerSeconds = level6.timerSeconds;
                        if (levelDone)
                        {

                            scoreBoard.Data(gameTime, levelid, level6.Seconds, level6Time, level6.Score, level6.MaxScore,
                                level6.PlayerLives, level6.Collected, level6.Restored, level6.Kills, level6.MaxKills, level6.MaxCollectables);
                            if (!soundStopped)
                            {
                                level6.StopSound();
                                soundStopped = true;
                            }
                            showScoreBoard = true;
                            levelDone = false;
                        }
                        toggleDB = level6.toggleDB;
                    }
                    break;
                case 7:
                    {
                        if (currentLevel != level7)
                            currentLevel = level7;
                        if (level7.levelDone )
                            levelDone = true;
                        else if (levelDone)
                            level7.levelDone = true;
                        else
                            levelDone = false;
                        if (level7.GameOverBiatch)
                        {
                            timer--;
                            if (timer <= 0)
                            {
                                levelDone = true;
                                timer = 220;
                            }
                        }
                        followCord = level7.followCoord;
                        gravity = level7.world.Gravity;
                        playerPos = level7.playerPosition;
                        playerRotation = level7.playerRotation;
                        level7.Update(gameTime, keyboardState);
                        currentLevelMinutes = level7.Minutes;
                        currentLevelSeconds = level7.Seconds;
                        currentCollects = level7.Collected;
                        currentMaxCollects = level7.MaxCollectables;
                        timerSeconds = level7.timerSeconds;
                        if (levelDone)
                        {

                            scoreBoard.Data(gameTime, levelid, level7.Seconds, level7Time, level7.Score, level7.MaxScore,
                                level7.PlayerLives, level7.Collected, level7.Restored, level7.Kills, level7.MaxKills, level7.MaxCollectables);
                            if (!soundStopped)
                            {
                                level7.StopSound();
                                soundStopped = true;
                            }
                            showScoreBoard = true;
                            levelDone = false;
                        }
                        toggleDB = level7.toggleDB;
                    }
                    break;
                
                default: break;


            }
        }

        public void ChangeLevel(int id)
        {
            levelid = id;
            soundStopped = false;
            gdReset = true;
            switch (levelid)
            {
                case 0:
                    {
                        level0 = new Level0(graphicDevice, "level0.gleed");
                        level1 = null;
                        level2 = null;
                        level3 = null;
                        level4 = null;
                        level5 = null;
                        level6 = null;
                        level7 = null;
                    }
                    break;
                case 1:
                    {
                        level0 = null;
                        level1 = new Level1(graphicDevice, "level1.gleed");
                        level2 = null;
                        level3 = null;
                        level4 = null;
                        level5 = null;
                        level6 = null;
                        level7 = null;
                    }
                    break;
                case 2:
                    {
                        level0 = null;
                        level1 = null;
                        level2 = new Level2(graphicDevice, "level2.gleed");
                        level3 = null;
                        level4 = null;
                        level5 = null;
                        level6 = null;
                        level7 = null;

                    }
                    break;
                case 3:
                    {
                        level0 = null;
                        level1 = null;
                        level2 = null;
                        level3 = new Level3(graphicDevice, "level3.gleed");
                        level4 = null;
                        level5 = null;
                        level6 = null;
                        level7 = null;

                    }
                    break;
                case 4:
                    {
                        level0 = null;
                        level1 = null;
                        level2 = null;
                        level3 = null;
                        level4 = new Level4(graphicDevice, "level4.gleed");
                        level5 = null;
                        level6 = null;
                        level7 = null;

                    }
                    break;
                case 5:
                    {
                        level0 = null;
                        level1 = null;
                        level2 = null;
                        level3 = null;
                        level4 = null;
                        level5 = new Level5(graphicDevice, "level5.gleed");
                        level6 = null;
                        level7 = null;

                    }
                    break;
                case 6:
                    {
                        level0 = null;
                        level1 = null;
                        level2 = null;
                        level3 = null;
                        level4 = null;
                        level5 = null;
                        level6 = new Level6(graphicDevice, "level6.gleed");
                        level7 = null;

                    }
                    break;
                case 7:
                    {
                        level0 = null;
                        level1 = null;
                        level2 = null;
                        level3 = null;
                        level4 = null;
                        level5 = null;
                        level6 = null;
                        level7 = new Level7(graphicDevice, "Level7.gleed");

                    }
                    break;
                
                default: break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)//, LineBatch lineBatch)
        {
            switch (levelid)
            {
                case 0:
                    level0.Draw(gameTime, spriteBatch); //, lineBatch);
                    break;
                case 1:
                    level1.Draw(gameTime, spriteBatch); //, lineBatch);
                    break;
                case 2:
                    level2.Draw(gameTime, spriteBatch); //, lineBatch);
                    break;
                case 3:
                    level3.Draw(gameTime, spriteBatch); //, lineBatch);
                    break;
                case 4:
                    level4.Draw(gameTime, spriteBatch); //, lineBatch);
                    break;
                case 5:
                    level5.Draw(gameTime, spriteBatch); //, lineBatch);
                    break;
                case 6:
                    level6.Draw(gameTime, spriteBatch); //, lineBatch);
                    break;
                case 7:
                    level7.Draw(gameTime, spriteBatch); //, lineBatch);
                    break;
            }
        }
    }
}
