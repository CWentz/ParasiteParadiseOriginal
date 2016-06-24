using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Gleed2D;
using Gleed2D.InGame;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.DebugViews;
using FarseerPhysics;
using FarseerPhysics.Common.PhysicsLogic;
using System.Collections;



namespace VirusGame
{
    /// <summary>
    /// The LevelMain class is a abstract class for level creation.
    /// It includes all basic functions. Loads all data from gleed files.
    /// For each level created from LevelMain only
    /// basic logic needs to be added to the update function for proper 
    /// nerve/trigger and valve/geflecht to funtion properly.
    /// </summary>
    public abstract class LevelMain
    {

        #region Declarations

        #region score declarations get/set

        private List<ArrayList> finalScore = new List<ArrayList>();

        private int maxCollectables;
        private int maxKills;
        private int maxScore;
        private float seconds;
        private int minutes;
        private int score;
        private int collected;
        private int restored;
        private int kills;
        //private int playerLives;

        public int timerSeconds
        {
            get { return (int)(seconds + .5f); }
        }


        public int Seconds
        {
            get { return (int)(timeDelta + .5f); }
        }
        public int Minutes
        {
            get { return minutes; }
        }

        public int PlayerLives
        {
            get { return player.Lives; }
        }

        public int Score
        {
            get { return score; }
        }
        public int Collected
        {
            get { return collected; }
        }

        public int MaxCollectables
        {
            get { return maxCollectables; }
        }

        public int Restored
        {
            get { return restored; }
        }
        public int Kills
        {
            get { return kills; }
        }

        public int MaxKills
        {
            get { return maxKills; }
        }

        public int MaxScore
        {
            get { return maxScore; }
        }
        /////////////////////

        #endregion

        #region heartBeat

        private int tenSeconds = 10;
        private float interval = 1;
        private float faster = .2f;
        private float nextinterval;

        #endregion
        protected bool brainKilled = false;
        protected bool lastLevel = false;
        public bool toggleDB = true;
        private SpriteClasses.BloodControl.BloodAnimation bloodAnim;
        private bool bloodFlowing = true;
        private Vector2 nearestFlowingBlood;

        public bool init = false;
        private bool update = false;
        protected bool panning = false;
        protected bool cameraTeleport = true;
        protected bool cameraTrigger;
        protected Vector2 cameraDestination;
        protected int cameraPanTimer;
        protected float panSpeed;
        protected bool gameOverBiatch;
        protected float panSpeedX;
        protected float panSpeedY;
        protected bool autoPanningBack = true;

        int gameOverTimer = 60;

        public Vector2 playerPosition;
        public float playerRotation;
        public Vector2 oldPosition;
        private bool playerVisible;
        public bool levelDone = false;
        SoundEngine soundEngine;
        //GraphicsDevice graphicDevice;
        Controls playerController = new Controls();

        KeyboardState key;
        KeyboardState oldKey;
        GamePadState controller;
        GamePadState oldController;

        SpriteClasses.alarmSprite alarm;
        protected SpriteClasses.Menu.FinishSprite finish;

        protected List<Object> allCells = new List<Object>();
        List<VelocityRect> velocityRectangles = new List<VelocityRect>();


        Texture2D tex;

        public bool GameOverBiatch
        {
            get { return gameOverBiatch; }
        }

        public void StopSound()
        {
            soundEngine.StopAll();
        }


        public Vector2 followCoord;

        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        public World FromWorld
        {
            get { return world; }
        }
        public World world;

        /// <summary>
        /// Gleed2d level class
        /// </summary>
        Level level;

        Tile tile;
        PathTile pathTile;
        MyTexture myTexture;

        /// <summary>
        /// Lists of two different kind of tile
        /// </summary>
        List<Tile> tiles;
        List<PathTile> pathTiles;
        List<MyTexture> textures;

        private Vector2 playerChanged;
        public SpriteClasses.fadingSprite fadingTest;
        public SpriteClasses.Parallax.Prop bloodBG;
        public SpriteClasses.Parallax.Prop bloodBG2;
        public SpriteClasses.Parallax.Prop bloodBG3;
        public SpriteClasses.Parallax.Prop veinBG;
        public SpriteClasses.Parallax.Background paralaxBG;

        protected SpriteClasses.PlayerSprite player;

        #region nerve/trigger bloodspawn/gefect bools  synapse
        


        //destination of the synapses(only sent when their position isnt zero
        protected Vector2 synTrig1 = new Vector2(0,0);
        protected Vector2 synTrig2 = new Vector2(0, 0);
        protected Vector2 synTrig3 = new Vector2(0, 0);
        protected Vector2 synTrig4 = new Vector2(0, 0);
        protected Vector2 synTrig5 = new Vector2(0, 0);
        protected Vector2 synTrig6 = new Vector2(0, 0);
        protected Vector2 synTrig7 = new Vector2(0, 0);
        protected Vector2 synTrig8 = new Vector2(0, 0);
        protected Vector2 synTrig9 = new Vector2(0, 0);
        protected Vector2 synTrig10 = new Vector2(0, 0);

        protected Vector2 synNerve1 = new Vector2(0, 0);
        protected Vector2 synNerve2 = new Vector2(0, 0);
        protected Vector2 synNerve3 = new Vector2(0, 0);
        protected Vector2 synNerve4 = new Vector2(0, 0);
        protected Vector2 synNerve5 = new Vector2(0, 0);
        protected Vector2 synNerve6 = new Vector2(0, 0);
        protected Vector2 synNerve7 = new Vector2(0, 0);
        protected Vector2 synNerve8 = new Vector2(0, 0);
        protected Vector2 synNerve9 = new Vector2(0, 0);
        protected Vector2 synNerve10 = new Vector2(0, 0);


        protected bool nerve1On;
        protected bool nerve2On;
        protected bool nerve3On;
        protected bool nerve4On;
        protected bool nerve5On;
        protected bool nerve6On;
        protected bool nerve7On;
        protected bool nerve8On;
        protected bool nerve9On;
        protected bool nerve10On;

        protected bool trigger1On;
        protected bool trigger2On;
        protected bool trigger3On;
        protected bool trigger4On;
        protected bool trigger5On;
        protected bool trigger6On;
        protected bool trigger7On;
        protected bool trigger8On;
        protected bool trigger9On;
        protected bool trigger10On;

        protected bool bloodSpawn1Open;
        protected bool bloodSpawn2Open;
        protected bool bloodSpawn3Open;
        protected bool bloodSpawn4Open;
        protected bool bloodSpawn5Open;
        protected bool bloodSpawn6Open;
        protected bool bloodSpawn7Open;
        protected bool bloodSpawn8Open;
        protected bool bloodSpawn9Open;
        protected bool bloodSpawn10Open;

        protected bool gefecht1Open;
        protected bool gefecht2Open;
        protected bool gefecht3Open;
        protected bool gefecht4Open;
        protected bool gefecht5Open;
        protected bool gefecht6Open;
        protected bool gefecht7Open;
        protected bool gefecht8Open;
        protected bool gefecht9Open;
        protected bool gefecht10Open;


        //positions to send the synapses to
        protected Vector2 bloodSpawn1Pos;
        protected Vector2 bloodSpawn2Pos;
        protected Vector2 bloodSpawn3Pos;
        protected Vector2 bloodSpawn4Pos;
        protected Vector2 bloodSpawn5Pos;
        protected Vector2 bloodSpawn6Pos;
        protected Vector2 bloodSpawn7Pos;
        protected Vector2 bloodSpawn8Pos;
        protected Vector2 bloodSpawn9Pos;
        protected Vector2 bloodSpawn10Pos;

        protected Vector2 gefecht1Pos;
        protected Vector2 gefecht2Pos;
        protected Vector2 gefecht3Pos;
        protected Vector2 gefecht4Pos;
        protected Vector2 gefecht5Pos;
        protected Vector2 gefecht6Pos;
        protected Vector2 gefecht7Pos;
        protected Vector2 gefecht8Pos;
        protected Vector2 gefecht9Pos;
        protected Vector2 gefecht10Pos;

        #endregion

        public DebugViewXNA _debugView;
        float timeDelta;

        #endregion


        /// <summary>
        /// Creates a new level, sets up Farseer world object and loads tiles
        /// </summary>
        /// <param name="serviceProvider"></param>
        public LevelMain(GraphicsDevice graphicDevice, String _levelGleedFile)
        {
            FarseerPhysics.Settings.EnableDiagnostics = false;
            FarseerPhysics.Settings.VelocityIterations = 6;
            FarseerPhysics.Settings.PositionIterations = 2;
            FarseerPhysics.Settings.ContinuousPhysics = false;

            content = SpriteClasses.SpriteManager.Content;
            if (world == null)
            {
                world = new World(new Vector2(0f, 0f));
            }
            else
            {
                world.Clear();
            }

            LoadTiles(_levelGleedFile);
            soundEngine = new SoundEngine(content);

            

            
            // create and configure the debug view
            _debugView = new DebugViewXNA(world);
            _debugView.Enabled = true;
            _debugView.AppendFlags(DebugViewFlags.DebugPanel);
            _debugView.DefaultShapeColor = Color.White;
            _debugView.SleepingShapeColor = Color.Lavender;
            _debugView.LoadContent(graphicDevice, content);
            _debugView.AdaptiveLimits = true;
            _debugView.AppendFlags(DebugViewFlags.AABB);

            Load();
        }

        /// <summary>
        /// Tile loading method, reads gleed2d xml-file and sorts out paths and textures
        /// </summary>
        /// 
        public virtual void Load()
        {

        }

        protected void LoadTiles(String _levelGleed)
        {
            SpriteClasses.SpriteManager.Content = content;
            SpriteClasses.SpriteManager.Level = world;
            bloodAnim = new SpriteClasses.BloodControl.BloodAnimation();
            
            using (Stream stream = TitleContainer.OpenStream("Content/Levels/" + _levelGleed)) //#1 change gere you level file path
            {
                XElement xml = XElement.Load(stream);
                level = LevelLoader.Load(xml);
            }

            tiles = new List<Tile>();
            pathTiles = new List<PathTile>();
            textures = new List<MyTexture>();


            #region load sprites switch/foreach

            foreach (Layer layer in level.Layers)
            {
                String layerName = layer.Properties.Name.ToString();
                switch (layerName)
                {
                    case "Collision":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is PathItemProperties)
                                {
                                    PathItemProperties pathProperties = item.Properties as PathItemProperties;
                                    pathTile = new PathTile(pathProperties.LocalPoints, pathProperties.Position, world);
                                    pathTiles.Add(pathTile);
                                }
                                if (item.Properties is RectangleItemProperties)
                                {
                                    RectangleItemProperties pathProperties = item.Properties as RectangleItemProperties;
                                    tile = new Tile(TileCollision.Impassable, pathProperties.Width, pathProperties.Height, pathProperties.Position, pathProperties.Rotation, world);
                                    tiles.Add(tile);
                                }
                            }
                        }
                        break;
                    case "MakroCollision":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is PathItemProperties)
                                {
                                    PathItemProperties pathProperties = item.Properties as PathItemProperties;
                                    pathTile = new PathTile(pathProperties.LocalPoints, pathProperties.Position, world, true);
                                    pathTiles.Add(pathTile);
                                }
                                if (item.Properties is RectangleItemProperties)
                                {
                                    RectangleItemProperties pathProperties = item.Properties as RectangleItemProperties;
                                    tile = new Tile(TileCollision.Impassable, pathProperties.Width, pathProperties.Height, pathProperties.Position, pathProperties.Rotation, world);
                                    tiles.Add(tile);
                                }
                            }
                        }
                        break;
                    case "LevelTexture":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    string filename = "Levels/" + Path.GetFileNameWithoutExtension(textureProperties.TexturePathRelativeToContentRoot); //3# change here your tile textures' file path
                                    Texture2D texture = content.Load<Texture2D>(filename);
                                    myTexture = new MyTexture(texture, textureProperties.Position, textureProperties.Rotation, textureProperties.Scale, .071f);
                                    textures.Add(myTexture);
                                }
                            }
                        }
                        break;
                    case "Finish":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is CircleItemProperties)
                                {
                                    CircleItemProperties textureProperties = item.Properties as CircleItemProperties;
                                    Body Ziel = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(textureProperties.Radius), 1.0f);
                                    Ziel.Position = new Vector2(ConvertUnits.ToSimUnits(textureProperties.Position.X), ConvertUnits.ToSimUnits(textureProperties.Position.Y));
                                    Ziel.IgnoreGravity = true;
                                    Ziel.CollidesWith = Category.Cat5;
                                    Ziel.OnCollision += collisionZiel;
                                    finish = SpriteClasses.SpriteManager.addFinishAnimation(textureProperties.Position);
                                }
                            }
                        }
                        break;
                    case "Flow":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    velocityRectangles.Add(new VelocityRect(world, textureProperties.Position, textureProperties.Scale, textureProperties.Rotation, player));
                                    allCells.Add(new SpriteClasses.BloodControl.ParticleSpawner(textureProperties.Position, textureProperties.Rotation, 0, 1f));

                                }
                            }
                        }
                        break;
                    case "Plasma":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    maxKills++;
                                    maxScore += 100;
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    allCells.Add(SpriteClasses.SpriteManager.addPlasmaCell(tempSpawn, new Vector2(0, 0), 'd'));
                                }
                            }
                        }
                        break;
                    case "Player":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is CircleItemProperties)
                                {
                                    CircleItemProperties textureProperties = item.Properties as CircleItemProperties;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    player = SpriteClasses.SpriteManager.addPlayerCell(tempSpawn);
                                    allCells.Add(player);

                                }
                            }
                        }
                        break;
                    case "Makrophage":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    allCells.Add(SpriteClasses.SpriteManager.addMakrophageCell(tempSpawn, new Vector2(0, 0)));
                                }
                            }
                        }
                        break;
                    case "Helper":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    maxKills++;
                                    maxScore += 50;
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    allCells.Add(SpriteClasses.SpriteManager.addHilferCell(tempSpawn, new Vector2(0, 0)));
                                }
                            }
                        }
                        break;
                    case "Monozyt":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    maxKills++;
                                    maxScore += 100;
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    allCells.Add(SpriteClasses.SpriteManager.addMonozytCell(tempSpawn, new Vector2(0, 0)));
                                }
                            }
                        }
                        break;
                    case "BloodSpawner":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    float tempScale = textureProperties.Scale.X;
                                    tempScale += textureProperties.Scale.Y;
                                    tempScale /= 2f;
                                    SpriteClasses.BloodControl.BloodSpawner tempBS = new SpriteClasses.BloodControl.BloodSpawner(tempSpawn, textureProperties.Rotation, 70, 17.5f, tempScale);
                                    tempBS.nameForLevel = textureProperties.Name;
                                    allCells.Add(tempBS);
                                }
                            }
                        }
                        break;
                    case "BloodCellDespawner":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    float tempScale = textureProperties.Scale.X;
                                    tempScale += textureProperties.Scale.Y;
                                    tempScale /= 2f;
                                    SpriteClasses.BloodControl.BloodDespawner tempBS = SpriteClasses.SpriteManager.addBloodDespawner(tempSpawn, textureProperties.Rotation, tempScale);
                                    tempBS.nameForLevel = textureProperties.Name;
                                    allCells.Add(tempBS);
                                }
                            }
                        }
                        break;
                    case "Gefecht":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    float tempScale = textureProperties.Scale.X;
                                    tempScale += textureProperties.Scale.Y;
                                    tempScale /= 2f;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    SpriteClasses.Valves.Gefecht tempGef = SpriteClasses.SpriteManager.addGefecht(tempSpawn, textureProperties.Rotation, tempScale);
                                    tempGef.nameForLevel = textureProperties.Name;

                                    allCells.Add(tempGef);
                                }
                            }
                        }
                        break;
                    case "Collection":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    maxScore += 100;
                                    maxCollectables++;
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    allCells.Add(SpriteClasses.SpriteManager.addCollectCell(tempSpawn, new Vector2(0, 0)));
                                }
                            }
                        }
                        break;
                    case "Nerve":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    SpriteClasses.NPCSprites.NerveCell tempNerve = SpriteClasses.SpriteManager.addNerveCell(tempSpawn, textureProperties.Rotation);
                                    tempNerve.nameForLevel = textureProperties.Name;
                                    allCells.Add(tempNerve);
                                }
                            }
                        }
                        break;
                    case "Trigger":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    SpriteClasses.NPCSprites.TriggerCell tempTrigg = SpriteClasses.SpriteManager.addTriggerCell(tempSpawn, textureProperties.Rotation);
                                    tempTrigg.nameForLevel = textureProperties.Name;
                                    allCells.Add(tempTrigg);
                                }
                            }
                        }
                        break;
                    case "Prop":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    String textName = textureProperties.Name;
                                    String type = (String)item.Properties.CustomProperties["type"].Value;
                                    char moveType = type[0];
                                    
                                    Vector2 tempSpawn = textureProperties.Position;
                                    float tempScale = textureProperties.Scale.X;
                                    tempScale += textureProperties.Scale.Y;
                                    tempScale /= 2f;
                                    SpriteClasses.Parallax.Prop tempProp = SpriteClasses.SpriteManager.addProp(textName, tempSpawn, textureProperties.Rotation, 2000f, moveType, tempScale, .8f);
                                    tempProp.nameForLevel = textName;//textureProperties.Name;
                                    allCells.Add(tempProp);
                                }
                            }
                        }
                        break;
                    case "Synapse":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {

                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    String textName = textureProperties.Name;
                                    Vector2 tempSpawn = textureProperties.Position;
                                    float tempScale = textureProperties.Scale.X;
                                    tempScale += textureProperties.Scale.Y;
                                    tempScale /= 2f;
                                    SpriteClasses.Parallax.Prop tempProp = SpriteClasses.SpriteManager.addSynapse(tempSpawn, textureProperties.Rotation, 2000f, tempScale, 0f);
                                    tempProp.nameForLevel = textName;//textureProperties.Name;
                                    tempProp.IsVisible = false;
                                    tempProp.Type = "Synapse";
                                    allCells.Add(tempProp);
                                }
                            }
                        }
                        break;
                    default:
                        break;


                }
            }

            #endregion

            paralaxBG = new SpriteClasses.Parallax.Background(Content.Load<Texture2D>("Levels/paralax_bg"), 20f);
            bloodBG = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/bloodcellparalax"), new Vector2(-1000, 1000), new Vector2(1, -3), 150f, 'R', 1f);
            bloodBG2 = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/bloodcellparalax"), new Vector2(-2000, -2000), new Vector2(3, 2), 80f, 'B', .7f);
            bloodBG3 = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/bloodcellparalax"), new Vector2(1000, 1000), new Vector2(-4, -1), 700f, 'W', 3f);
            veinBG = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/venesParalax"), new Vector2(-900, -500), new Vector2(0, 0), 30f, 'F', 2f);
            veinBG.Rotation = (float)(2.0 / Math.PI);

            bloodBG3.Depth = 0.0092f;

            alarm = new SpriteClasses.alarmSprite(world, Content.Load<Texture2D>("Images/alarm"), new Vector2(0, 0), new Vector2(0, 0), 4, 1);
            fadingTest = SpriteClasses.SpriteManager.addFadingSprite();

            // = new UI.TimerSprite(new Vector2(200, 200));
            
            allCells.Add(paralaxBG);
            allCells.Add(bloodBG);
            allCells.Add(veinBG);
            allCells.Add(bloodBG2);
            allCells.Add(alarm);
            allCells.Add(bloodBG3);
            allCells.Add(fadingTest);
        }

        public virtual void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            timeDelta += (float)gameTime.ElapsedGameTime.TotalSeconds;
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
            }

            if (timeDelta > tenSeconds && tenSeconds <= 50)
            {
                interval -= faster;
                tenSeconds += tenSeconds;
            }

            if (timeDelta >= nextinterval)
            {
                nextinterval += interval;
                soundEngine.Play("Herz", .15f, false);
            }

            finish.Update(gameTime);

            key = Keyboard.GetState();
            controller = GamePad.GetState(PlayerIndex.One);


            int index = -1;

            bloodAnim.Update(gameTime);
        

            #region update sprites switch/foreach

            foreach (SpriteClasses.MovingSprite ms in allCells)
            {
                
                switch (ms.Type)
                {
                    case "Plasma":
                        {
                            #region plasma
                            if (!panning)
                            {
                                ms.Update(gameTime);
                                if (((SpriteClasses.NPCSprites.PlasmaCell)ms).BodyRemoved && ((SpriteClasses.NPCSprites.PlasmaCell)ms).credit)
                                {
                                    finalScore.Add(((SpriteClasses.NPCSprites.PlasmaCell)ms).getPoints);
                                    ((SpriteClasses.NPCSprites.PlasmaCell)ms).credit = false;

                                }


                                if (((SpriteClasses.NPCSprites.PlasmaCell)ms).proximity && ((SpriteClasses.NPCSprites.PlasmaCell)ms).IsVisible)
                                    soundEngine.Play("Plasmacell_Death");

                                if (((SpriteClasses.NPCSprites.PlasmaCell)ms).IsVisible)
                                {
                                    ((SpriteClasses.NPCSprites.PlasmaCell)ms).PlayerPosition = playerPosition;
                                    if (((SpriteClasses.NPCSprites.PlasmaCell)ms).proximityShoot)
                                        soundEngine.Play("Plasmacell_Attack");
                                }
                            }
                            if (panning && !((SpriteClasses.NPCSprites.PlasmaCell)ms).BodyRemoved)
                            {
                                ms.body.Position = Globals.getWorldPosition(ms.position);
                            }
                            #endregion
                        }
                        break;
                    case "Player":
                        {
                            #region player
                            if (!panning)
                            {
                                if (((SpriteClasses.PlayerSprite)ms).cheatSound)
                                {
                                    soundEngine.Play("meow", 0.75f);
                                }
                                ((SpriteClasses.PlayerSprite)ms).Update(gameTime);
                                if(((SpriteClasses.PlayerSprite)ms).Attacking)
                                    playerController.setVibrate(0.0f, 0.2f, 10);
                                if(((SpriteClasses.PlayerSprite)ms).FlippedControls)
                                    playerController.setVibrate(0.5f, 0.5f, 10);
                                if (((SpriteClasses.PlayerSprite)ms).Attacking)
                                {
                                    soundEngine.Play("Player_Attack", 0.05f);
                                }

                                //if (playerController.Attack)
                                //    soundEngine.Play("Player_Attack", 0.05f);
                                if (player.Lives <= 0 && !player.gameover)
                                {
                                    gameOverBiatch = true;
                                    soundEngine.Play("Player_Death", 0.5f);
                                    gameOverTimer--;
                                    if (gameOverTimer <= 0)
                                    {
                                        fadingTest.IsVisible = true;
                                        gameOverTimer = 60;
                                    }

                                    alarm.permanentlyDisable = true;
                                }
                                if (player.hitWallSoundActive)
                                {
                                    player.hitWallSoundActive = false;
                                    soundEngine.Play("Wall Hit", 0.1f);
                                }

                                playerVisible = ms.IsVisible;
                                if (((SpriteClasses.PlayerSprite)ms).IsVisible)
                                {
                                    
                                    playerPosition = ((SpriteClasses.PlayerSprite)ms).position;
                                    playerRotation = ms.rotation;
                                    playerChanged = (playerPosition - oldPosition);
                                    cameraDestination = playerPosition;
                                    oldPosition = playerPosition;
                                    followCoord = ((SpriteClasses.PlayerSprite)ms).position;
                                    alarm.IsVisible = ((SpriteClasses.PlayerSprite)ms).WallWasHit;
                                }
                            }
                            if (panning)
                            {
                                ((SpriteClasses.PlayerSprite)ms).Update(gameTime);
                                ms.body.Position = Globals.getWorldPosition(oldPosition);
                            }
                            #endregion

                        }
                        break;
                    case "Makrophage":
                        {
                            #region makrophage
                            if (!panning)
                            {
                                ms.Update(gameTime);
                                if (((SpriteClasses.NPCSprites.MakrophageCell)ms).IsVisible)
                                {

                                    if (((SpriteClasses.NPCSprites.MakrophageCell)ms).playScream && !((SpriteClasses.NPCSprites.MakrophageCell)ms).screamPlayed)
                                    {
                                        soundEngine.Play("MonsterSonScream", .2f, false);
                                        ((SpriteClasses.NPCSprites.MakrophageCell)ms).screamPlayed = true;
                                    }


                                    if (((SpriteClasses.NPCSprites.MakrophageCell)ms).chasing && player.IsVisible)
                                    {
                                        
                                        alarm.IsVisible = true;
                                    }
                                    if (playerVisible)
                                    {
                                        if (((SpriteClasses.NPCSprites.MakrophageCell)ms).chase(player.Position, true) > 0)
                                        {
                                            soundEngine.Play("Makro_Eating");
                                            soundEngine.Play("Player_Death");
                                        }
                                    }

                                }
                                
                            }
                            if (panning && ms.IsVisible)
                            {
                                ms.body.Position = Globals.getWorldPosition(ms.oldPosition);
                            }
                            #endregion
                        }
                        break;
                    case "Helper":
                        {
                            #region helper
                            if (!panning)
                            {
                                ms.Update(gameTime);
                                if (((SpriteClasses.NPCSprites.HilferCell)ms).stopSound)
                                {
                                    soundEngine.Stop("Helferzelle_Alarm");
                                }

                                if (((SpriteClasses.NPCSprites.HilferCell)ms).credit)
                                {

                                    ((SpriteClasses.NPCSprites.HilferCell)ms).credit = false;
                                    score += ((SpriteClasses.NPCSprites.HilferCell)ms).Points;
                                    finalScore.Add(((SpriteClasses.NPCSprites.HilferCell)ms).getPoints);
                                }
                                if (((SpriteClasses.NPCSprites.HilferCell)ms).IsVisible)
                                {

                                    if (((SpriteClasses.NPCSprites.HilferCell)ms).playingSound)
                                    {
                                        soundEngine.Play("Helferzelle_Alarm", 0.07f);
                                    }

                                    
                                    if (((SpriteClasses.NPCSprites.HilferCell)ms).ExplodeTimer > 0 && ((SpriteClasses.NPCSprites.HilferCell)ms).ExplodeTimer <= 60)
                                    {
                                        playerController.HelperExploding = ((SpriteClasses.NPCSprites.HilferCell)ms).ExplodeTimer;
                                    }
                                    ((SpriteClasses.NPCSprites.HilferCell)ms).chase(playerPosition, false);
                                }
                                if (((SpriteClasses.NPCSprites.HilferCell)ms).proximity && ((SpriteClasses.NPCSprites.HilferCell)ms).IsVisible)
                                {
                                    soundEngine.Stop("Helferzelle_Alarm");
                                    soundEngine.Play("Helferzelle_Death", 0.4f);
                                }
                            }
                            if (panning && !ms.BodyRemoved)
                            {
                                ((SpriteClasses.NPCSprites.HilferCell)ms).body.Position = Globals.getWorldPosition(((SpriteClasses.NPCSprites.HilferCell)ms).position);
                            }
                            #endregion
                        }
                        break;
                    case "Monozyt":
                        {
                            #region monozyt
                            if (!panning)
                            {
                                ms.Update(gameTime);
                                if (((SpriteClasses.NPCSprites.MonozytCell)ms).IsVisible)
                                {
                                    if (((SpriteClasses.NPCSprites.MonozytCell)ms).weldedbool)
                                    {
                                        playerController.setVibrate(10f, 30f, 10);
                                    }
                                    if (!((SpriteClasses.NPCSprites.MonozytCell)ms).weldedbool)
                                    {
                                        ((SpriteClasses.NPCSprites.MonozytCell)ms).chase(playerPosition);
                                        
                                    }
                                    if (((SpriteClasses.NPCSprites.MonozytCell)ms).chasing)
                                        soundEngine.Play("Monozyt", .1f, false);
                                    if (((SpriteClasses.NPCSprites.MonozytCell)ms).playingSound)
                                    {
                                        soundEngine.Play("Monozyt_Death");
                                    }
                                    else
                                    {
                                        soundEngine.Stop("Monozyt_Death");
                                    }
                                }
                                else
                                {
                                    if (((SpriteClasses.NPCSprites.MonozytCell)ms).playingSound)
                                    {
                                        soundEngine.Stop("Monozyt");
                                        soundEngine.Stop("Monozyt_Death");
                                        ((SpriteClasses.NPCSprites.MonozytCell)ms).playingSound = false;
                                    }
                                }

                                


                                if (((SpriteClasses.NPCSprites.MonozytCell)ms).attacked)
                                {
                                    if (((SpriteClasses.NPCSprites.MonozytCell)ms).credit)
                                    {
                                        finalScore.Add(((SpriteClasses.NPCSprites.MonozytCell)ms).getPoints);
                                        ((SpriteClasses.NPCSprites.MonozytCell)ms).credit = false;
                                    }
                                }

                            }
                            if (panning && ms.IsVisible)
                            {
                                ((SpriteClasses.NPCSprites.MonozytCell)ms).body.Position = Globals.getWorldPosition(((SpriteClasses.NPCSprites.MonozytCell)ms).position);
                            }
                            #endregion
                        }
                        break;
                    case "BloodSpawner":
                        {
                            #region bloodspawner
                            ((SpriteClasses.BloodControl.BloodSpawner)ms).BloodAnim = bloodAnim;
                            ((SpriteClasses.BloodControl.BloodSpawner)ms).Update(gameTime);
                            

                            if (!(((SpriteClasses.BloodControl.BloodSpawner)ms).Open) && !((SpriteClasses.BloodControl.BloodSpawner)ms).playSound && ((SpriteClasses.BloodControl.BloodSpawner)ms).ListCount > 1)
                            {
                                playerController.setVibrate(.25f, .25f, 10);
                                soundEngine.Play("valveclaps closing");
                                ((SpriteClasses.BloodControl.BloodSpawner)ms).playSound = true;
                            }

                            if (((SpriteClasses.BloodControl.BloodSpawner)ms).Open && nearestFlowingBlood != new Vector2(0,0))
                            {
                                float distance = Vector2.Distance(nearestFlowingBlood, playerPosition);
                                float volume = (-distance / (800f*((float)Math.Pow((double)distance, 1.0/(double)distance))));
                                volume += 1f;
                                if (volume > .3f)
                                    volume = .3f;
                                if (volume < 0f)
                                    volume = 0f;


                                soundEngine.Play("Blutrauschen_Hintergrund", volume , false);
                            }


                            if (ms.position == nearestFlowingBlood)
                            {
                                if (!((SpriteClasses.BloodControl.BloodSpawner)ms).Open)
                                {
                                    nearestFlowingBlood = new Vector2(0, 0);
                                }
                            }


                            if (Vector2.Distance(nearestFlowingBlood, Globals.CameraPosition) > 800 || nearestFlowingBlood == new Vector2(0, 0))
                            {
                                soundEngine.Stop("Blutrauschen_Hintergrund");
                            }

                            switch (((SpriteClasses.BloodControl.BloodSpawner)ms).nameForLevel)
                            {
                                case "BloodSpawner1":
                                    {
                                        bloodFlowing = ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = bloodSpawn1Open;
                                        bloodSpawn1Pos = ((SpriteClasses.BloodControl.BloodSpawner)ms).position;
                                        if (((SpriteClasses.BloodControl.BloodSpawner)ms).Open && Vector2.Distance(((SpriteClasses.BloodControl.BloodSpawner)ms).position, playerPosition) < 700)
                                        {
                                            nearestFlowingBlood = ms.position;
                                            
                                        }

                                    }
                                    break;
                                case "BloodSpawner2":
                                    {
                                    
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = bloodSpawn2Open;
                                        bloodSpawn2Pos = ((SpriteClasses.BloodControl.BloodSpawner)ms).position;
                                        if (((SpriteClasses.BloodControl.BloodSpawner)ms).Open && Vector2.Distance(((SpriteClasses.BloodControl.BloodSpawner)ms).position, playerPosition) < 700)
                                        {
                                            nearestFlowingBlood = ms.position;
                                        }
                                    }
                                    break;
                                case "BloodSpawner3":
                                    {
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = bloodSpawn3Open;
                                        bloodSpawn3Pos = ((SpriteClasses.BloodControl.BloodSpawner)ms).position;
                                        if (((SpriteClasses.BloodControl.BloodSpawner)ms).Open && Vector2.Distance(((SpriteClasses.BloodControl.BloodSpawner)ms).position, playerPosition) < 700)
                                        {
                                            nearestFlowingBlood = ms.position;
                                        }

                                    }
                                    break;
                                case "BloodSpawner4":
                                    {
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = bloodSpawn4Open;
                                        bloodSpawn4Pos = ((SpriteClasses.BloodControl.BloodSpawner)ms).position;
                                        if (((SpriteClasses.BloodControl.BloodSpawner)ms).Open && Vector2.Distance(((SpriteClasses.BloodControl.BloodSpawner)ms).position, playerPosition) < 700)
                                        {
                                            nearestFlowingBlood = ms.position;
                                        }

                                    }
                                    break;
                                case "BloodSpawner5":
                                    {
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = bloodSpawn5Open;
                                        bloodSpawn5Pos = ((SpriteClasses.BloodControl.BloodSpawner)ms).position;
                                        if (((SpriteClasses.BloodControl.BloodSpawner)ms).Open && Vector2.Distance(((SpriteClasses.BloodControl.BloodSpawner)ms).position, playerPosition) < 700)
                                        {
                                            nearestFlowingBlood = ms.position;
                                        }

                                    }
                                    break;
                                case "BloodSpawner6":
                                    {
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = bloodSpawn6Open;
                                        bloodSpawn6Pos = ((SpriteClasses.BloodControl.BloodSpawner)ms).position;
                                        if (((SpriteClasses.BloodControl.BloodSpawner)ms).Open && Vector2.Distance(((SpriteClasses.BloodControl.BloodSpawner)ms).position, playerPosition) < 700)
                                        {
                                            nearestFlowingBlood = ms.position;
                                        }

                                    }
                                    break;
                                case "BloodSpawner7":
                                    {
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = bloodSpawn7Open;
                                        bloodSpawn7Pos = ((SpriteClasses.BloodControl.BloodSpawner)ms).position;
                                        if (((SpriteClasses.BloodControl.BloodSpawner)ms).Open && Vector2.Distance(((SpriteClasses.BloodControl.BloodSpawner)ms).position, playerPosition) < 700)
                                        {
                                            nearestFlowingBlood = ms.position;
                                        }

                                    }
                                    break;
                                case "BloodSpawner8":
                                    {
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = bloodSpawn8Open;
                                        bloodSpawn8Pos = ((SpriteClasses.BloodControl.BloodSpawner)ms).position;
                                        if (((SpriteClasses.BloodControl.BloodSpawner)ms).Open && Vector2.Distance(((SpriteClasses.BloodControl.BloodSpawner)ms).position, playerPosition) < 700)
                                        {
                                            nearestFlowingBlood = ms.position;
                                        }

                                    }
                                    break;
                                case "BloodSpawner9":
                                    {
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = bloodSpawn9Open;
                                        bloodSpawn9Pos = ((SpriteClasses.BloodControl.BloodSpawner)ms).position;
                                        if (((SpriteClasses.BloodControl.BloodSpawner)ms).Open && Vector2.Distance(((SpriteClasses.BloodControl.BloodSpawner)ms).position, playerPosition) < 700)
                                        {
                                            nearestFlowingBlood = ms.position;
                                        }

                                    }
                                    break;
                                case "BloodSpawner10":
                                    {
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = bloodSpawn10Open;
                                        bloodSpawn10Pos = ((SpriteClasses.BloodControl.BloodSpawner)ms).position;
                                        if (((SpriteClasses.BloodControl.BloodSpawner)ms).Open && Vector2.Distance(((SpriteClasses.BloodControl.BloodSpawner)ms).position, playerPosition) < 700)
                                        {
                                            nearestFlowingBlood = ms.position;
                                        }

                                    }
                                    break;
                                default:
                                    break;

                            }

                            
                            #endregion
                        }
                        break;
                    case "BloodCellDespawner":
                        {
                            ((SpriteClasses.BloodControl.BloodDespawner)ms).Update(gameTime);
                        }
                        break;
                    case "Alarm":
                        {
                            #region alarm
                            if (!panning)
                            {
                                ms.Update(gameTime);
                                ms.position = Globals.CameraPosition;
                                if (ms.IsVisible)
                                {


                                }
                            }

                            if (panning)
                                ms.IsVisible = false;
                            #endregion
                        }
                        break;
                    case "Gefecht":
                        {
                            #region gefecht
                            ((SpriteClasses.Valves.Gefecht)ms).Update(gameTime);
                            if (((SpriteClasses.Valves.Gefecht)ms).open && ((SpriteClasses.Valves.Gefecht)ms).IsVisible)
                            {
                                playerController.setVibrate(.25f, .25f, 10);
                                soundEngine.Play("Trigger_Nervengeflecht");
                            }
                            
                            switch (((SpriteClasses.Valves.Gefecht)ms).nameForLevel)
                            {
                                case "Gefecht1":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = gefecht1Open;

                                        gefecht1Pos = ms.position;
                                    }
                                    break;
                                case "Gefecht2":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = gefecht2Open;
                                        gefecht2Pos = ms.position;
                                    }
                                    break;
                                case "Gefecht3":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = gefecht3Open;
                                        gefecht3Pos = ms.position;
                                    }
                                    break;
                                case "Gefecht4":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = gefecht4Open;
                                        gefecht4Pos = ms.position;
                                    }
                                    break;
                                case "Gefecht5":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = gefecht5Open;
                                        gefecht5Pos = ms.position;
                                    }
                                    break;
                                case "Gefecht6":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = gefecht6Open;
                                        gefecht6Pos = ms.position;
                                    }
                                    break;
                                case "Gefecht7":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = gefecht7Open;
                                        gefecht7Pos = ms.position;
                                    }
                                    break;
                                case "Gefecht8":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = gefecht8Open;
                                        gefecht8Pos = ms.position;
                                    }
                                    break;
                                case "Gefecht9":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = gefecht9Open;
                                        gefecht9Pos = ms.position;
                                    }
                                    break;
                                case "Gefecht10":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = gefecht10Open;
                                        gefecht10Pos = ms.position;
                                    }
                                    break;
                                default:
                                    break;

                            }
                            #endregion
                        }
                        break;
                    case "collectable":
                        {
                            #region
                            ms.Update(gameTime);
                            if (((SpriteClasses.CollectCell)ms).wasFull)
                            {
                                
                                if (((SpriteClasses.CollectCell)ms).credit)
                                {
                                    finalScore.Add(((SpriteClasses.CollectCell)ms).getPoints); 
                                    soundEngine.Play("Collectible");
                                    playerController.setVibrate(0.1f, 0.1f, 10);
                                    ((SpriteClasses.CollectCell)ms).credit = false;
                                }
                            }
                            #endregion
                        }
                        break;
                    case "Nerve":
                        {
                            #region nerves
                            ((SpriteClasses.NPCSprites.NerveCell)ms).Update(gameTime);

                            if (!((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn && !((SpriteClasses.NPCSprites.NerveCell)ms).playedSound)
                            {
                                soundEngine.Play("Trigger_Nervengeflecht");
                                ((SpriteClasses.NPCSprites.NerveCell)ms).playedSound = true;
                            }

                            switch (((SpriteClasses.NPCSprites.NerveCell)ms).nameForLevel)
                            {
                                case "Nerve1":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).setSynapse(synNerve1);
                                        nerve1On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Nerve2":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).setSynapse(synNerve2);
                                        nerve2On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Nerve3":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).setSynapse(synNerve3);
                                        nerve3On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Nerve4":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).setSynapse(synNerve4);
                                        nerve4On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Nerve5":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).setSynapse(synNerve5);
                                        nerve5On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Nerve6":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).setSynapse(synNerve6);
                                        nerve6On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Nerve7":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).setSynapse(synNerve7);
                                        nerve7On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Nerve8":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).setSynapse(synNerve8);
                                        nerve8On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Nerve9":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).setSynapse(synNerve9);
                                        nerve9On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Nerve10":
                                    {
                                        ((SpriteClasses.NPCSprites.NerveCell)ms).setSynapse(synNerve10);
                                        nerve10On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                default:
                                    break;

                            }
                            #endregion
                        }
                        break;
                    case "Trigger":
                        {
                            #region triggers
                            ((SpriteClasses.NPCSprites.TriggerCell)ms).Update(gameTime);

                            if (((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn && !((SpriteClasses.NPCSprites.TriggerCell)ms).playedSound)
                            {
                                soundEngine.Play("Trigger_Triggeraktivierung");
                                ((SpriteClasses.NPCSprites.TriggerCell)ms).playedSound = true;
                            }

                            switch (((SpriteClasses.NPCSprites.TriggerCell)ms).nameForLevel)
                            {
                                case "Trigger1":
                                    {
                                        ((SpriteClasses.NPCSprites.TriggerCell)ms).setSynapse(synTrig1);
                                        trigger1On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Trigger2":
                                    {
                                        ((SpriteClasses.NPCSprites.TriggerCell)ms).setSynapse(synTrig2);
                                        trigger2On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Trigger3":
                                    {
                                        ((SpriteClasses.NPCSprites.TriggerCell)ms).setSynapse(synTrig3);
                                        trigger3On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Trigger4":
                                    {
                                        ((SpriteClasses.NPCSprites.TriggerCell)ms).setSynapse(synTrig4);
                                        trigger4On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Trigger5":
                                    {
                                        ((SpriteClasses.NPCSprites.TriggerCell)ms).setSynapse(synTrig5);
                                        trigger5On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Trigger6":
                                    {
                                        ((SpriteClasses.NPCSprites.TriggerCell)ms).setSynapse(synTrig6);
                                        trigger6On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Trigger7":
                                    {
                                        ((SpriteClasses.NPCSprites.TriggerCell)ms).setSynapse(synTrig7);
                                        trigger7On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Trigger8":
                                    {
                                        ((SpriteClasses.NPCSprites.TriggerCell)ms).setSynapse(synTrig8);
                                        trigger8On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Trigger9":
                                    {
                                        ((SpriteClasses.NPCSprites.TriggerCell)ms).setSynapse(synTrig9);
                                        trigger9On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Trigger10":
                                    {
                                        ((SpriteClasses.NPCSprites.TriggerCell)ms).setSynapse(synTrig10);
                                        trigger10On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                default:
                                    break;

                            }
                            #endregion
                        }
                        break;
                    case "Fading":
                        {
                            ((SpriteClasses.fadingSprite)ms).Update(gameTime);
                            ms.position = followCoord;


                            if (((SpriteClasses.fadingSprite)ms).alphaByte == 240)
                            {
                                levelDone = true;
                            }
                        }
                        break;
                    case "Background":
                        {
                            ms.Update(gameTime, playerChanged);
                        }
                        break;
                    case "Prop":
                        {
                            if (panning && ((SpriteClasses.Parallax.Prop)ms).MoveType != 'F')
                            {
                                ms.Update(gameTime, playerChanged);
                            }

                            if (!panning)
                            {
                                ms.Update(gameTime, playerChanged);
                            }
                            //if (((SpriteClasses.Parallax.Prop)ms).nameForLevel == "test")
                                //ms.IsVisible = synapse1On;
                            if (panning && ((SpriteClasses.Parallax.Prop)ms).MoveType == 'F')
                            {
                                ms.position = ms.oldPosition;
                            }


                        }
                        break;
                    case "Synapse":
                        {

                            #region synapse

                            //((SpriteClasses.Parallax.Prop)ms).Update(gameTime, new Vector2(0, 0));
                            //switch (((SpriteClasses.Parallax.Prop)ms).nameForLevel)
                            //{
                            //    case "Synapse1":
                            //        {
                            //            if (((SpriteClasses.Parallax.Prop)ms).synapseTurnOff < 200)
                            //                ms.IsVisible = true;
                            //            else ms.IsVisible = false;
                            //        }
                            //        break;
                            //    case "Synapse2":
                            //        {
                            //            if (((SpriteClasses.Parallax.Prop)ms).synapseTurnOff < 200)
                            //                ms.IsVisible = true;
                            //            else ms.IsVisible = false;
                            //        }
                            //        break;
                            //    case "Synapse3":
                            //        {
                            //            if (((SpriteClasses.Parallax.Prop)ms).synapseTurnOff < 200)
                            //                ms.IsVisible = true;
                            //            else ms.IsVisible = false;
                            //        }
                            //        break;
                            //    case "Synapse4":
                            //        {
                            //            if (((SpriteClasses.Parallax.Prop)ms).synapseTurnOff < 200)
                            //                ms.IsVisible = true;
                            //            else ms.IsVisible = false;
                            //        }
                            //        break;
                            //    case "Synapse5":
                            //        {
                            //            if (((SpriteClasses.Parallax.Prop)ms).synapseTurnOff < 200)
                            //                ms.IsVisible = true;
                            //            else ms.IsVisible = false;
                            //        }
                            //        break;
                            //    case "Synapse6":
                            //        {
                            //            if (((SpriteClasses.Parallax.Prop)ms).synapseTurnOff < 200)
                            //                ms.IsVisible = true;
                            //            else ms.IsVisible = false;
                            //        }
                            //        break;
                            //    case "Synapse7":
                            //        {
                            //            if (((SpriteClasses.Parallax.Prop)ms).synapseTurnOff < 200)
                            //                ms.IsVisible = true;
                            //            else ms.IsVisible = false;
                            //        }
                            //        break;
                            //    case "Synapse8":
                            //        {
                            //            if (((SpriteClasses.Parallax.Prop)ms).synapseTurnOff < 200)
                            //                ms.IsVisible = true;
                            //            else ms.IsVisible = false;
                            //        }
                            //        break;
                            //    default: break;
                            //}
                            #endregion

                        }
                        break;
                    case "ParticleSpawner":
                        {
                            ms.Update(gameTime, playerChanged);
                        }
                        break;
                    default:
                        break;


                }
            }
            update = !update;
            #endregion

            foreach (ArrayList pts in finalScore)
            {
                if (!((bool)pts[7]))
                {
                    //score collected kills
                    score += (int)pts[1];
                    if ((bool)pts[2])
                    {
                        kills++;
                    }
                    if ((bool)pts[3])
                    {
                        collected++;
                    }
                    if ((bool)pts[4]) //heals
                    {
                        restored++;
                    }
                    pts[7] = true;
                }

            }

            //timer.Update(minutes, Seconds);

            if (!panning)
            {
                cameraPanTimer = 0;
                panSpeed = 0f;
                panSpeedX = 0f;
                panSpeedY = 0f;
            }
            if (panning)
            {
                PanCameraToPosition(cameraDestination, 50f);
                if (cameraPanTimer > 180 && autoPanningBack)
                {
                    cameraPanTimer = 0;
                    //panSpeed = 0f;
                    panning = false;
                    cameraTeleport = true;
                }
                if (cameraPanTimer > 120 && autoPanningBack)
                    cameraDestination = playerPosition;
                
                cameraPanTimer++;
            }

            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

            world.AutoClearForces = true;


            oldPosition = playerPosition;
            if(alarm.IsVisible)
                playerController.setVibrate(1f, 1f, 10);



            playerController.UpdateControls(key, oldKey, controller, oldController);


            if (index != -1)
                allCells.RemoveAt(index);



            oldController = GamePad.GetState(PlayerIndex.One);
            oldKey = Keyboard.GetState();
            
        }

        protected void PanCameraToPosition(Vector2 _position, float _ticks)
        {

            //i know this is a dumb way to do it, but earlier it wasn't working. Don't judge,
            Vector2 tempX1 = new Vector2(followCoord.X, 0);
            Vector2 tempX2 = new Vector2(_position.X, 0);
            Vector2 tempY1 = new Vector2(0, followCoord.Y);
            Vector2 tempY2 = new Vector2(0, _position.Y);
            //Vector2 direction = new Vector2(1, 1);
            //if (followCoord.X > _position.X)
            //    direction.X = Math.Abs(direction.X) * -1;
            //if (followCoord.X < _position.X)
            //    direction.X = Math.Abs(direction.X);
            //if (followCoord.Y > _position.Y)
            //    direction.Y = Math.Abs(direction.Y) * -1;
            //if (followCoord.Y < _position.Y)
            //    direction.Y = Math.Abs(direction.Y);

            if (panSpeed == 0f)
                panSpeed = Vector2.Distance(followCoord, _position) / _ticks;

            if (panSpeedX == 0f)
            {
                panSpeedX = Vector2.Distance(tempX1, tempX2) / _ticks;
            }
            if (panSpeedY == 0f)
            {
                panSpeedY = Vector2.Distance(tempY1, tempY2) / _ticks;
            }
            //Vector2 pan = new Vector2(followCoord.X - _position.X, followCoord.Y - _position.Y);

            //followCoord += (pan / 60f );

            if (followCoord.X > _position.X + panSpeed)
                followCoord.X -= panSpeedX;
            if (followCoord.X < _position.X - panSpeed)
                followCoord.X += panSpeedX;
            if (followCoord.Y > _position.Y + panSpeed)
                followCoord.Y -= panSpeedY;
            if (followCoord.Y < _position.Y - panSpeed)
                followCoord.Y += panSpeedY;

            //if (followCoord.X > _position.X - panSpeed)// && followCoord.X > -535)
            //    followCoord.X -= panSpeed;
            //if (followCoord.X < _position.X + panSpeed)// && followCoord.X < 535)
            //    followCoord.X += panSpeed;
            //if (followCoord.Y > _position.Y - panSpeed)// && followCoord.Y > -415)
            //    followCoord.Y -= panSpeed;
            //if (followCoord.Y < _position.Y + panSpeed)// && followCoord.Y < 415)
            //    followCoord.Y += panSpeed;

        }




        #region collision


        bool collisionZiel(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.UserData as String == "Player" && !lastLevel)
            {
                fadingTest.IsVisible = true;
                player.IsVisible = false;
                alarm.permanentlyDisable = true;
            }
            else if (fixtureB.UserData as String == "Player" && lastLevel && brainKilled)
            {
                fadingTest.IsVisible = true;
                player.IsVisible = false;
                alarm.permanentlyDisable = true;
            }
            return false;
        }

        #endregion



        #region Draw

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)//, LineBatch lineBatch)
        {
            
            foreach (Tile temp in tiles)
            {
                temp.Draw(spriteBatch, tex);
            }

            foreach (SpriteClasses.MovingSprite sprite in allCells)
            {
                sprite.Draw(spriteBatch);
            }

            foreach (MyTexture t in textures)
            {
                t.Draw(spriteBatch);
            }

            finish.Draw(spriteBatch);
        }
        #endregion
    }
}
