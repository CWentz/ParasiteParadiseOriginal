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
using LuaInterface;
using C3.XNA;
using FarseerPhysics.DebugViews;

using FarseerPhysics.Dynamics;

namespace VirusGame
{
    //modify the mouse to make it visible

    public class VirusGameMain : Microsoft.Xna.Framework.Game
    {

        #region debug Declarations
        bool debugMode = false;
        FPSMonitor fpsMonitor = new FPSMonitor();
        SpriteFont fpsFont;


        #endregion

        #region UI-System
        //UI-System
        enum GameState
        {
            StartMenu,
            ScoreBoard,
            GameOver,
            Loading,
            Playing,
            Paused,
            SaveLoad
        }
        GameState gameState;
        UI.GameUI gameUI;
        UI.LoadingScreen loadingScreen;
        UI.PauseScreen pauseScreen;
        UI.SaveLoadScreen saveLoadScreen;
        UI.StartScreen startScreen;

        #endregion

        #region Declarations



        public LineBatch lineBatch;
        LevelManager levelManager;
        SoundEngine soundEngine;

        UI.TimerSprite timer;
        UI.ScoreBoard scoreBoard;

        //main camera follows character
        Camera.Camera2D cam;
        bool cameraActive = true;

        

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
        Lua luainstance = new Lua();

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
            graphics.ApplyChanges();

            //the following saves the screen measurements.
            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;

            gameState = GameState.StartMenu;

            //LUA
            luainstance.RegisterFunction("changenumber", this, this.GetType().GetMethod("changenumber"));

            base.Initialize();
        }
        #endregion

        #region Load-Function
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SpriteClasses.SpriteManager.Content = Content;
            //SpriteClasses.SpriteManager.Level = levelManager.level1.world;

            lineBatch = new LineBatch(GraphicsDevice);
            scoreBoard = new UI.ScoreBoard(Content, GraphicsDevice);
            levelManager = new LevelManager(1, Content, GraphicsDevice, scoreBoard);
            soundEngine = new SoundEngine(Content);
            soundEngine.Play("Menu_Background", 0.1f, true);
            
            //debug
            fpsFont = Content.Load<SpriteFont>("fpsFont");
            Globals.Pixel = Content.Load<Texture2D>("Test/test");

            timer = new UI.TimerSprite(new Vector2(900, 700));

            pauseScreen = new UI.PauseScreen();
            pauseScreen.Load(Content);

            startScreen = new UI.StartScreen();
            startScreen.Load(Content, GraphicsDevice);

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



            checkPause();

            #region LUA
            //no functionality yet
            /*
             * if (key.IsKeyDown(Keys.F1))
            { 
                luainstance.DoFile("test.lua"); 
            }
            */
            #endregion

            if (key.IsKeyDown(Keys.F1))
                levelManager.ChangeLevel(1);

            if (key.IsKeyDown(Keys.F2))
                levelManager.ChangeLevel(2);

            if (key.IsKeyDown(Keys.F3))
                levelManager.ChangeLevel(3);

            if (key.IsKeyDown(Keys.F4))
                levelManager.ChangeLevel(4);

            if (playerController.escape())
                gameState = GameState.StartMenu;

            if (playerController.debugToogle())
                if (debugMode)
                    debugMode = false;
                else debugMode = true;


            #region DEVMODE (F11)
            if(debugMode)
            {
                fpsMonitor.Update();
            }
            #endregion

            #region gameState: StartMenu
            if (gameState == GameState.StartMenu)
            {
                startScreen.Update(playerController);
                if (startScreen.menuChoosen)
                {
                    if (startScreen.menuItem == 1)
                    {
                        gameState = GameState.Playing;
                        soundEngine.Stop("Menu_Background");
                        soundEngine.Play("Ambient_01", 0.1f, true);
                        levelManager.ChangeLevel(1);
                    }
                    if (startScreen.menuItem == 3)
                        this.Exit();
                }

            }
            #endregion

            #region gameState: Loading
            if (gameState == GameState.Loading)
            {

            }
            #endregion

            #region gameState: Pause
            if (gameState == GameState.Paused)
            {
                
                pauseScreen.Update(gameTime, playerController);
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
                }

            }
            #endregion

            #region gameState: SaveLoad
            if (gameState == GameState.SaveLoad)
            {

            }
            #endregion

            #region gameState: Playing
            if (gameState == GameState.Playing)
            {
                cam.Update(levelManager.followCord);
                
                if (key.IsKeyDown(Keys.W))
                    cam.Zoom += .01f;
                if (key.IsKeyDown(Keys.S))
                    cam.Zoom -= .01f;
                if (key.IsKeyDown(Keys.A))
                    cam.Rotation += .01f;
                if (key.IsKeyDown(Keys.D))
                    cam.Rotation -= .01f;

                levelManager.Update(gameTime, Keyboard.GetState());

                timer.Update(levelManager.CurrentMinutes, levelManager.CurrentSeconds);

                if (levelManager.showScoreBoard)
                    gameState = GameState.ScoreBoard;
            }
            #endregion

            #region gameState: ScoreBoard
            if (gameState == GameState.ScoreBoard)
            {
                scoreBoard.Update(playerController);
                if (scoreBoard.nextLevel)
                {
                    levelManager.NextLevel();
                    gameState = GameState.Playing;
                }
            }
            #endregion

            playerController.UpdateControls(key, oldKey, controller, oldController);

            oldController = GamePad.GetState(PlayerIndex.One);
            oldKey = Keyboard.GetState();

            base.Update(gameTime);

        }

        #endregion

        #region Pause-Function
        void checkPause()
        {
            if(playerController.pause())  //modified because i added pause test to the controller class.
            {
                //gameState = GameState.Paused;
                pauseKey = true;
            }
            else if (pauseKey)
            {
                
                if (gameState == GameState.Paused)
                    gameState = GameState.Playing;
                else if (gameState == GameState.Playing)
                    gameState = GameState.Paused;
                pauseKey = false;
            }
        }
        #endregion

        #region Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Vector2 gdv = new Vector2((float)ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width), (float)ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Height));
            Vector2 camSU = new Vector2((float)ConvertUnits.ToSimUnits(cam.X), (float)ConvertUnits.ToSimUnits(cam.Y));
            Vector2 sc = new Vector2((float)ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 2f), (float)ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Height / 2f)); 
            // calculate the projection and view adjustments for the debug view
            Matrix projection = Matrix.CreateOrthographicOffCenter(0f, gdv.X, gdv.Y, 0f, 0f, 1f);
            //Matrix view = Matrix.CreateTranslation(new Vector3(-sc, 0f)) * Matrix.CreateTranslation(new Vector3(sc, 0f));
            Matrix view = cam.camSimUnit;

            #region GameState = Playing
            if (gameState == GameState.Playing || gameState == GameState.Paused)
            {
                if (cameraActive)
                    spriteBatch.Begin(SpriteSortMode.BackToFront, //back = 1.0f front = 0f
                        BlendState.AlphaBlend,
                        null, null, null, null, cam.Transform);
                else spriteBatch.Begin();
                levelManager.Draw(gameTime, spriteBatch, lineBatch);

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
                    if (levelManager.levelid == 1)
                        levelManager.level1._debugView.RenderDebugData(ref projection, ref view);
                    if (levelManager.levelid == 2)
                        levelManager.level2._debugView.RenderDebugData(ref projection, ref view);
                    if (levelManager.levelid == 3)
                        levelManager.level3._debugView.RenderDebugData(ref projection, ref view);
                    if (levelManager.levelid == 4)
                        levelManager.level4._debugView.RenderDebugData(ref projection, ref view);

                }

                spriteBatch.End();

                spriteBatch.Begin();
                {
                    timer.Draw(spriteBatch);
                }
                spriteBatch.End();

                #region GameState = Pause
                if (gameState == GameState.Paused)
                {
                    spriteBatch.Begin();
                    pauseScreen.Draw(spriteBatch);
                    spriteBatch.End();
                }
                #endregion
            }
            #endregion

            #region ScoreBoard
            if (gameState == GameState.ScoreBoard)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                scoreBoard.Draw(spriteBatch);
                spriteBatch.End();
            }
            #endregion

            //#region GameState = Pause
            //if (gameState == GameState.Paused)
            //{
            //    spriteBatch.Begin();
            //    pauseScreen.Draw(spriteBatch);
            //    spriteBatch.End();
            //}
            //#endregion

            #region GameState = StartMenu
            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Begin();
                startScreen.Draw(spriteBatch);
                spriteBatch.End();
            }
            #endregion

            base.Draw(gameTime);
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
