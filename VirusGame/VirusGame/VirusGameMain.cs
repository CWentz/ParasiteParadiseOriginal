using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics;
using LuaInterface;
using System.Runtime.InteropServices;


namespace VirusGame
{
    //modify the mouse to make it visible

    public class VirusGameMain : Microsoft.Xna.Framework.Game
    {
        
        bool resetGD = false;
        
        #region debug Declarations
        bool allowDebug = true;
        bool debugMode = false;
        FPSMonitor fpsMonitor = new FPSMonitor();
        SpriteFont fpsFont;
        #endregion

        #region UI-System
        //UI-System
        enum GameState
        {
            Intro,
            StartMenu,
            ScoreBoard,
            Playing,
            Paused,
            SaveLoad,
            Credits
        }
        static GameState gameState;
        UI.StartingVideo intro;
        UI.PauseScreen pauseScreen;
        UI.SaveLoadScreen saveLoadScreen;
        UI.StartScreen startScreen;
        UI.CreditScreen creditScreen;

        #endregion

        #region Declarations

        int creditsCD = 10;
        public LineBatch lineBatch;
        LevelManager levelManager;
        SoundEngine soundEngine;

        UI.TimerSprite timer;
        UI.ScoreBoard scoreBoard;

        //main camera follows character
        static Camera.Camera2D cam;
        bool cameraActive = true;

        Texture2D overlay;

        private bool pauseKey = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Controls playerController = new Controls();

        //Display variables
        protected float screenWidth;
        protected float screenHeight;

        //parallax backgrounds
        public SpriteClasses.BackgroundSprite bg;
        public SpriteClasses.BackgroundSprite bg2;

        Random rand = new Random();

        //controller/keyboard states
        KeyboardState key;
        KeyboardState oldKey;
        GamePadState controller;
        GamePadState oldController;

        //quadtree
        //private QuadTree<SpriteClasses.MovingSprite> quadTree;

        

        //debug
        //Lua luainstance = new Lua();

        #endregion

        #region Main
        public VirusGameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #endregion

        #region Initialize
        protected override void Initialize()
        {
            //sets resolution to 1024x768
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferMultiSampling = true;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
           
            //the following saves the screen measurements.
            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;
            gameState = GameState.Intro;

            //LUA
            //luainstance.RegisterFunction("changenumber", this, this.GetType().GetMethod("changenumber"));

            base.Initialize();
        }
        #endregion

        #region Load-Function
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SpriteClasses.SpriteManager.Content = Content;
            Globals.TextureManager = new SpriteClasses.TextureManager(this.Content);
            creditScreen = new UI.CreditScreen();
            lineBatch = new LineBatch(GraphicsDevice);
            scoreBoard = new UI.ScoreBoard(Content, GraphicsDevice);
            levelManager = new LevelManager(0, Content, GraphicsDevice, scoreBoard);
            soundEngine = new SoundEngine(Content);
            soundEngine.Play("Menu_Background", 0.1f, true);

            overlay = Content.Load<Texture2D>("overlay");
            
            //debug
            fpsFont = Content.Load<SpriteFont>("fpsFont");
            Globals.Pixel = Content.Load<Texture2D>("Globals/pixel");

            timer = new UI.TimerSprite(new Vector2(900, 720));
            intro = new UI.StartingVideo();
            pauseScreen = new UI.PauseScreen();
            pauseScreen.Load(Content);

            startScreen = new UI.StartScreen();
            startScreen.Load(Content, GraphicsDevice);

            saveLoadScreen = new UI.SaveLoadScreen();
            saveLoadScreen.Load(Content, GraphicsDevice);
            Globals.particles = Content.Load<Texture2D>("Test/tempPart");
            cam = new Camera.Camera2D(GraphicsDevice.Viewport);
        }
        #endregion

        #region Unload-Function
        protected override void UnloadContent()
        {
            
        }
        #endregion

        #region Update-Function
        protected override void Update(GameTime gameTime)
        {
            key = Keyboard.GetState();
            controller = GamePad.GetState(PlayerIndex.One);


            if (playerController.pause())
            {
                checkPause();
            }


            #region DEVMODE (F12)
            if(debugMode)
            {
                fpsMonitor.Update();
            }

            #region LUA
            //no functionality yet

            //if (key.IsKeyDown(Keys.F1))
            //{ 
            //    luainstance.DoFile("test.lua"); 
            //}

            #endregion

            if (allowDebug)
            {
                if (playerController.debugToogle())
                    debugMode = !debugMode;
                else if (key.IsKeyDown(Keys.F1))
                {
                    levelManager.ChangeLevel(0);
                    resetGD = true;
                }
                else if (key.IsKeyDown(Keys.F2))
                {
                    levelManager.ChangeLevel(1);
                    resetGD = true;
                }
                else if (key.IsKeyDown(Keys.F3))
                {
                    levelManager.ChangeLevel(2);
                    resetGD = true;
                }
                else if (key.IsKeyDown(Keys.F4))
                {
                    levelManager.ChangeLevel(3);
                    resetGD = true;
                }
                else if (key.IsKeyDown(Keys.F5))
                {
                    levelManager.ChangeLevel(4);
                    resetGD = true;
                }
                else if (key.IsKeyDown(Keys.F6))
                {
                    levelManager.ChangeLevel(5);
                    resetGD = true;
                }
                else if (key.IsKeyDown(Keys.F7))
                {
                    levelManager.ChangeLevel(6);
                    resetGD = true;
                }
                else if (key.IsKeyDown(Keys.F8))
                {
                    levelManager.ChangeLevel(7);
                    resetGD = true;
                }

                else if (key.IsKeyDown(Keys.F9))
                    levelManager.levelDone = true;
            }
            #endregion


            if (playerController.escape() && gameState == GameState.Playing)
                gameState = GameState.Paused;

            switch (gameState)
            {
                #region gameState: Intro

                case GameState.Intro:
                    {
                        if (levelManager.showOutro)
                        {
                            intro.State = 'O';
                            levelManager.ChangeLevel(0);
                            levelManager.showOutro = false;
                        }

                        

                        if (playerController.escape() || playerController.attack())
                        {
                            intro.Skip = 'X';
                        }

                        intro.Update(gameTime);

                        if (intro.Skip == 'X')
                        {
                            gameState = GameState.StartMenu;
                        }

                    

                    }
                    break;
                #endregion

                #region gameState: Credits
                case GameState.Credits:
                    {
                        if (gameState == GameState.Credits)
                        {
                            creditScreen.Update(gameTime);
                            creditsCD--;
                            if (playerController.accept() && creditsCD < 0 || playerController.escape())
                            {
                                gameState = GameState.StartMenu;
                            }
                        }  
                    }
                    break;
                #endregion

                #region gameState: Pause
                case GameState.Paused:
                    {

                        if (levelManager.CurrentLevel != null)
                            levelManager.CurrentLevel.StopSound();
                            
                        if (pauseScreen.menuChoosen)
                        {
                            if (pauseScreen.MenuItem == 1)
                            {
                                gameState = GameState.Playing;
                            }
                            if (pauseScreen.MenuItem == 2)
                            {
                                gameState = GameState.StartMenu;
                            }
                            pauseScreen = new UI.PauseScreen();
                            pauseScreen.Load(SpriteClasses.SpriteManager.Content);
                            resetGD = true;
                        }

                        pauseScreen.Update(gameTime, playerController);
                    }
                    break;
                #endregion

                #region gameState: Playing

                case GameState.Playing:
                    {   
                        if (debugMode)
                        {
                            if (key.IsKeyDown(Keys.PageUp))
                                cam.Zoom += .01f;
                            if (key.IsKeyDown(Keys.PageDown))
                                cam.Zoom -= .01f;
                            if (key.IsKeyDown(Keys.Home))
                                cam.Rotation += .01f;
                            if (key.IsKeyDown(Keys.End))
                                cam.Rotation -= .01f;
                        }
                            

                        

                        cam.Update(levelManager.followCord);
                        levelManager.Update(gameTime, Keyboard.GetState());
                        timer.Update(levelManager.CurrentMinutes,
                                         levelManager.TimerSeconds,
                                         levelManager.CurrentCollects,
                                         levelManager.CurrentMaxCollects);

                        if (levelManager.showScoreBoard)
                            gameState = GameState.ScoreBoard;
                    }
                    break;
                #endregion

                #region gameState: SaveLoad
                case GameState.SaveLoad:
                    {
                        saveLoadScreen.Update(playerController);
                        if (saveLoadScreen.menuChoosen)
                        {
                            saveLoadScreen.menuChoosen = false;
                            gameState = GameState.Playing;
                            soundEngine.Stop("Menu_Background");
                            soundEngine.Play("Ambient_01", 0.1f, true);
                            levelManager.ChangeLevel(saveLoadScreen.selected);
                            resetGD = true;
                        }
                    }
                    break;
                #endregion

                #region gameState: ScoreBoard
                case GameState.ScoreBoard:
                    {
                        
                        if (levelManager.CurrentLevel.GameOverBiatch)
                        {
                            levelManager.levelDone = false;
                            levelManager.ChangeLevel(scoreBoard.levelID);
                            levelManager.showScoreBoard = false;
                            gameState = GameState.Playing;
                        }

                        if (scoreBoard.nextLevel && !levelManager.CurrentLevel.GameOverBiatch)
                        {

                            levelManager.levelDone = false;
                            if (levelManager.levelid == 7)
                            {
                                levelManager.showOutro = true;
                                gameState = GameState.Intro;
                            }
                            else
                            {
                                levelManager.ChangeLevel(scoreBoard.levelID + 1);
                                gameState = GameState.Playing;
                            }

                            levelManager.showScoreBoard = false;
                        }
                        resetGD = true;
                        scoreBoard.Update(playerController, gameTime);
                    }
                    break;
                #endregion

                #region gameState: StartMenu
                case GameState.StartMenu:
                    {
                        if (levelManager.CurrentLevel != null)
                            levelManager.CurrentLevel.StopSound();

                        startScreen.Update(playerController, gameTime);
                        
                        if (startScreen.menuChoosen)
                        {
                            switch (startScreen.menuItem)
                            {
                                case 1: // play
                                    {
                                        saveLoadScreen.dataLoaded = false;
                                        levelManager.doTutorial = false;
                                        gameState = GameState.SaveLoad;
                                    }
                                    break;
                                case 2: // credits
                                    {
                                        gameState = GameState.Credits;
                                        creditsCD = 10;
                                    }
                                    break;
                                case 3: // exit option
                                    {
                                        this.Exit();
                                    }
                                    break;
                                case 4:  //tutorial option
                                    {
                                        gameState = GameState.Playing;
                                        soundEngine.Stop("Menu_Background");
                                        soundEngine.Play("Ambient_01", 0.1f, true);
                                        levelManager.doTutorial = true;
                                        levelManager.ChangeLevel(0);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                #endregion

                default :
                    break;

            }

            

            playerController.UpdateControls(key, oldKey, controller, oldController);

            oldController = GamePad.GetState(PlayerIndex.One);
            oldKey = Keyboard.GetState();

            base.Update(gameTime);

        }

        #endregion

        #region Pause-Function
        void checkPause()
        {
            if (gameState == GameState.Paused)
                gameState = GameState.Playing;
            else if (gameState == GameState.Playing)
                gameState = GameState.Paused;
        }
        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            #region matrix calcs

            Vector2 gdv = new Vector2((float)ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width), (float)ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Height));
            Vector2 camSU = new Vector2((float)ConvertUnits.ToSimUnits(cam.X), (float)ConvertUnits.ToSimUnits(cam.Y));
            Vector2 sc = new Vector2((float)ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 2f), (float)ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Height / 2f)); 
            // calculate the projection and view adjustments for the debug view
            Matrix projection = Matrix.CreateOrthographicOffCenter(0f, gdv.X, gdv.Y, 0f, 0f, 1f);
            //Matrix view = Matrix.CreateTranslation(new Vector3(-sc, 0f)) * Matrix.CreateTranslation(new Vector3(sc, 0f));
            Matrix view = cam.camSimUnit;
            #endregion

            
            switch (gameState)
            {
                #region GameState: Intro
                case GameState.Intro:
                    {
                        
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                        intro.Draw(spriteBatch);
                        spriteBatch.End();

                        if (resetGD)
                            ResetGD();
                    }
                    break;
                #endregion

                #region GameState = StartMenu
                case GameState.StartMenu:
                    {
                        spriteBatch.Begin();
                        startScreen.Draw(spriteBatch);
                        spriteBatch.End();
                    }
                    break;
                        #endregion

                #region GameState = SaveLoad
                case GameState.SaveLoad:
                    {
                        spriteBatch.Begin();
                        saveLoadScreen.Draw(spriteBatch);
                        spriteBatch.End();
                    }
                    break;
                        #endregion

                #region GameState = Playing
                case GameState.Playing:
                    {
                        
                        if (cameraActive)
                            spriteBatch.Begin(SpriteSortMode.BackToFront, //back = 1.0f front = 0f
                                BlendState.AlphaBlend,
                                null, null, null, null, cam.Transform);
                        else spriteBatch.Begin();


                        levelManager.Draw(gameTime, spriteBatch);//, lineBatch);

                        spriteBatch.End();


                        spriteBatch.Begin();
                

                        if (debugMode)
                        {
                            fpsMonitor.Draw(spriteBatch, fpsFont, new Vector2(10, 10), Color.Red);
                            spriteBatch.DrawString(fpsFont, "Level: " + levelManager.levelid +
                                                            " \nPlayerpos:" +
                                                            " \nX:" + levelManager.playerPos.X +
                                                            " \nY: " + levelManager.playerPos.Y +
                                                            " \nRotation: " + levelManager.playerRotation +
                                                            " \nGravity: " + levelManager.gravity +
                                                            " \nZoom: " + cam.Zoom, new Vector2(10, 30), Color.Red);
                            switch(levelManager.levelid)
                            {
                                case 0:
                                    levelManager.level0._debugView.RenderDebugData(ref projection, ref view);
                                    break;
                                case 1:
                                    levelManager.level1._debugView.RenderDebugData(ref projection, ref view);
                                    break;
                                case 2:
                                    levelManager.level2._debugView.RenderDebugData(ref projection, ref view);
                                    break;
                                case 3:
                                    levelManager.level3._debugView.RenderDebugData(ref projection, ref view);
                                    break;
                                case 4:
                                    levelManager.level4._debugView.RenderDebugData(ref projection, ref view);
                                    break;
                                case 5:
                                    levelManager.level5._debugView.RenderDebugData(ref projection, ref view);
                                    break;
                                case 6:
                                    levelManager.level6._debugView.RenderDebugData(ref projection, ref view);
                                    break;
                                case 7:
                                    levelManager.level7._debugView.RenderDebugData(ref projection, ref view);
                                    break;
                                default:
                                    break;
                            }
                        }

                        spriteBatch.Draw(overlay, Vector2.Zero, Color.White);
                        timer.Draw(spriteBatch);


                        spriteBatch.End();

                        if (resetGD)
                            ResetGD();
                    }
                    break;
                    #endregion

                #region GameState = Paused
                case GameState.Paused:
                    {
                        if (cameraActive)
                            spriteBatch.Begin(SpriteSortMode.BackToFront, //back = 1.0f front = 0f
                                BlendState.AlphaBlend,
                                null, null, null, null, cam.Transform);
                        else spriteBatch.Begin();


                        levelManager.Draw(gameTime, spriteBatch);//, lineBatch);
                        spriteBatch.End();


                        spriteBatch.Begin();
                        pauseScreen.Draw(spriteBatch);
                        spriteBatch.End();
                
                    }
                    break;
                #endregion

                #region GameState = ScoreBoard
                case GameState.ScoreBoard:
                    {
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                        scoreBoard.Draw(spriteBatch);
                        spriteBatch.End();
                    }
                    break;
                        #endregion

                #region GameState = Credits
                case GameState.Credits:
                    {
                        spriteBatch.Begin();
                        creditScreen.Draw(spriteBatch);
                        spriteBatch.End();
                    }
                    break;
                #endregion
            }

            base.Draw(gameTime);
        }

        private void ResetGD()
        {
            try
            {
                GraphicsDevice.Reset();
                GraphicsDevice.Clear(Color.Black);
                resetGD = false;
            }
            catch (Exception e)
            {
                resetGD = true;
            }

        }
        #endregion
        
        #region LUA DEBUGGING
        /// <summary>
        /// change number in the LUA file
        /// </summary>
        /// <param name="n"></param>
        public void changenumber(float n)
        {
            //testPlayer.animation.Scale = n;
        }
        #endregion
    }
}
