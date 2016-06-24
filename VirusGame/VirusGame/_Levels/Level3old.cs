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


namespace VirusGame
{
    /// <summary>
    /// Class representing a game level. It loads tiles from .gleed file and draws them onto screen.
    /// Tiles set their bodies by themself
    /// </summary>
    public class Level3
    {

        #region Declarations

        //score section//////
        public int seconds;
        public int score;
        public int score_;
        public int lives;
        public int collected;
        public int restored;
        public int kills;
        public int collected_;
        public int kills_;

        /////////////////////
        public bool init = false;
        //private bool update = false;


        private Vector2 playerPosition;
        public Vector2 oldPosition;
        private bool playerVisible;
        public bool levelDone = false;
        private bool wallHit = false;
        private int wallHitTimer = 10;
        SoundEngine soundEngine;
        GraphicsDevice graphicDevice;
        Controls playerController = new Controls();

        KeyboardState key;
        KeyboardState oldKey;
        GamePadState controller;
        GamePadState oldController;

        SpriteClasses.alarmSprite alarm;

        List<Object> allCells = new List<Object>();
        List<VelocityRect> velocityRectangles = new List<VelocityRect>();


        Texture2D tex;



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

        public SpriteClasses.PlayerSprite player;

        private bool nerve1On;
        private bool nerve2On;
        private bool trigger1On;
        private bool trigger2On;
        private bool trigger3On;


        public DebugViewXNA _debugView;

        #endregion

        //public SpriteClasses.BloodControl.BloodDespawner despawn;

        /// <summary>
        /// Creates a new level, sets up Farseer world object and loads tiles
        /// </summary>
        /// <param name="serviceProvider"></param>
        public Level3(ContentManager serviceProvider, GraphicsDevice graphicDevice)
        {

            FarseerPhysics.Settings.EnableDiagnostics = false;
            FarseerPhysics.Settings.VelocityIterations = 6;
            FarseerPhysics.Settings.PositionIterations = 2;
            FarseerPhysics.Settings.ContinuousPhysics = false;

            content = serviceProvider;
            if (world == null)
            {
                world = new World(new Vector2(0f, 0f));
            }
            else
            {
                world.Clear();
            }

            LoadTiles();
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
        }

        /// <summary>
        /// Tile loading method, reads gleed2d xml-file and sorts out paths and textures
        /// </summary>
        protected void LoadTiles()
        {

            SpriteClasses.SpriteManager.Content = content;
            SpriteClasses.SpriteManager.Level = world;


            using (Stream stream = TitleContainer.OpenStream("Content/Levels/level3.gleed")) //#1 change gere you level file path
            {
                XElement xml = XElement.Load(stream);
                level = LevelLoader.Load(xml);
            }

            tiles = new List<Tile>();
            pathTiles = new List<PathTile>();
            textures = new List<MyTexture>();

            //despawn = new SpriteClasses.BloodControl.BloodDespawner(world, content.Load<Texture2D>("Test/test"), new Vector2(0, 0), new Vector2(0, 0), 1, 1, 2f, 2f);


            #region update sprites switch/foreach

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
                    case "LevelTexture":
                        {
                            foreach (LayerItem item in layer.Items)
                            {
                                if (item.Properties is TextureItemProperties)
                                {
                                    TextureItemProperties textureProperties = item.Properties as TextureItemProperties;
                                    string filename = "Levels/" + Path.GetFileNameWithoutExtension(textureProperties.TexturePathRelativeToContentRoot); //3# change here your tile textures' file path
                                    Texture2D texture = content.Load<Texture2D>(filename);
                                    myTexture = new MyTexture(texture, textureProperties.Position, textureProperties.Rotation, textureProperties.Scale * Globals.GlobalScale, .071f);
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
                                    kills_++;
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
                                    kills_++;
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
                                    collected_++;
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
                    default:
                        break;


                }
            }

            #endregion

            paralaxBG = new SpriteClasses.Parallax.Background(Content.Load<Texture2D>("Levels/level1_bg"), 20f);
            bloodBG = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/bloodcellparalax"), new Vector2(-1000, 1000), new Vector2(1, -3), 150f, 'R', 1f);
            bloodBG2 = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/bloodcellparalax"), new Vector2(-2000, -2000), new Vector2(3, 2), 80f, 'B', .7f);
            bloodBG3 = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/bloodcellparalax"), new Vector2(1000, 1000), new Vector2(-4, -1), 700f, 'W', 3f);
            veinBG = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/venesParalax"), new Vector2(-1100, -500), new Vector2(0, 0), 50f, 'F', 2f);
            veinBG.Rotation = (float)(2.0 / Math.PI);

            alarm = new SpriteClasses.alarmSprite(world, Content.Load<Texture2D>("Images/alarm"), new Vector2(0, 0), new Vector2(0, 0), 4, 1);
            fadingTest = SpriteClasses.SpriteManager.addFadingSprite();

            allCells.Add(paralaxBG);
            allCells.Add(bloodBG);
            allCells.Add(veinBG);
            allCells.Add(bloodBG2);
            allCells.Add(alarm);
            allCells.Add(bloodBG3);
            allCells.Add(fadingTest);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            seconds += (int)gameTime.ElapsedGameTime.TotalSeconds;

            //despawn.Update(gameTime);

            key = Keyboard.GetState();
            controller = GamePad.GetState(PlayerIndex.One);


            int index = -1;



            #region update sprites switch/foreach

            foreach (SpriteClasses.MovingSprite ms in allCells)
            {

                switch (ms.Type)
                {
                    case "Plasma":
                        {

                            ms.Update(gameTime);
                            if (((SpriteClasses.NPCSprites.PlasmaCell)ms).BodyRemoved && ((SpriteClasses.NPCSprites.PlasmaCell)ms).credit)
                            {
                                score += ((SpriteClasses.NPCSprites.PlasmaCell)ms).GivePoints();
                                ((SpriteClasses.NPCSprites.PlasmaCell)ms).credit = false;
                                kills++;
                            }
                            if (((SpriteClasses.NPCSprites.PlasmaCell)ms).IsVisible)
                            {
                                ((SpriteClasses.NPCSprites.PlasmaCell)ms).PlayerPosition = playerPosition;
                            }

                        }
                        break;
                    case "Player":
                        {

                            ((SpriteClasses.PlayerSprite)ms).Update(gameTime);
                            playerController.Attack = ((SpriteClasses.PlayerSprite)ms).Attacking;
                            playerController.FlippedControls = ((SpriteClasses.PlayerSprite)ms).FlippedControls;


                            collected = ((SpriteClasses.PlayerSprite)ms).Collected;
                            restored = ((SpriteClasses.PlayerSprite)ms).Restored;
                            lives = ((SpriteClasses.PlayerSprite)ms).Lives;
                            score = ((SpriteClasses.PlayerSprite)ms).Score;

                            playerVisible = ms.IsVisible;
                            if (((SpriteClasses.PlayerSprite)ms).IsVisible)
                            {
                                playerPosition = ((SpriteClasses.PlayerSprite)ms).position;
                                playerChanged = (playerPosition - oldPosition);
                                oldPosition = playerPosition;
                                followCoord = ((SpriteClasses.PlayerSprite)ms).position;
                                alarm.IsVisible = ((SpriteClasses.PlayerSprite)ms).WallWasHit;
                            }
                            if (followCoord.Y < -415)
                                followCoord.Y = -415;
                            if (followCoord.Y > 415)
                                followCoord.Y = 415;

                            if (followCoord.X < -535)
                                followCoord.X = -535;
                            if (followCoord.X > 535)
                                followCoord.X = 535;

                        }
                        break;
                    case "Makrophage":
                        {

                            ms.Update(gameTime);
                            if (((SpriteClasses.NPCSprites.MakrophageCell)ms).IsVisible)
                            {
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
                        break;
                    case "Helper":
                        {

                            ms.Update(gameTime);
                            if (((SpriteClasses.NPCSprites.HilferCell)ms).credit)
                            {
                                ((SpriteClasses.NPCSprites.HilferCell)ms).credit = false;
                                score += ((SpriteClasses.NPCSprites.HilferCell)ms).Points;
                            }
                            if (((SpriteClasses.NPCSprites.HilferCell)ms).IsVisible)
                            {
                                if (((SpriteClasses.NPCSprites.HilferCell)ms).ExplodeTimer > 0 && ((SpriteClasses.NPCSprites.HilferCell)ms).ExplodeTimer <= 60)
                                    playerController.HelperExploding = ((SpriteClasses.NPCSprites.HilferCell)ms).ExplodeTimer;
                                ((SpriteClasses.NPCSprites.HilferCell)ms).chase(playerPosition, false);
                            }

                        }
                        break;
                    case "Monozyt":
                        {

                            ms.Update(gameTime);
                            if (((SpriteClasses.NPCSprites.MonozytCell)ms).IsVisible)
                            {
                                if (!((SpriteClasses.NPCSprites.MonozytCell)ms).weldedbool)
                                    ((SpriteClasses.NPCSprites.MonozytCell)ms).chase(playerPosition);
                            }
                            else
                            {
                                score += ((SpriteClasses.NPCSprites.MonozytCell)ms).GivePoints();
                            }

                            if (((SpriteClasses.NPCSprites.MonozytCell)ms).attacked)
                            {
                                if (((SpriteClasses.NPCSprites.MonozytCell)ms).credit)
                                {
                                    ((SpriteClasses.NPCSprites.MonozytCell)ms).credit = false;
                                    kills++;
                                }
                            }


                        }
                        break;
                    case "BloodSpawner":
                        {

                            ((SpriteClasses.BloodControl.BloodSpawner)ms).Update(gameTime);
                            switch (((SpriteClasses.BloodControl.BloodSpawner)ms).nameForLevel)
                            {
                                case "BloodSpawner1":
                                    {
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = nerve1On;
                                    }
                                    break;
                                case "BloodSpawner2":
                                    {
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = nerve2On;
                                    }
                                    break;
                                case "BloodSpawner3":
                                    {
                                        ((SpriteClasses.BloodControl.BloodSpawner)ms).Open = false;
                                    }
                                    break;
                                default:
                                    break;

                            }

                        }
                        break;
                    case "BloodCellDespawner":
                        {

                            ((SpriteClasses.BloodControl.BloodDespawner)ms).Update(gameTime);
                        }
                        break;
                    case "Alarm":
                        {
                            ms.Update(gameTime);
                            ms.position = followCoord;
                            if (ms.IsVisible)
                            {


                            }
                        }
                        break;
                    case "Gefecht":
                        {

                            ((SpriteClasses.Valves.Gefecht)ms).Update(gameTime);
                            switch (((SpriteClasses.Valves.Gefecht)ms).nameForLevel)
                            {
                                case "Gefecht1":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = trigger1On;
                                    }
                                    break;
                                case "Gefecht2":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = trigger3On;
                                    }
                                    break;
                                case "Gefecht3":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = trigger2On;
                                    }
                                    break;
                                case "Gefecht4":
                                    {
                                        ((SpriteClasses.Valves.Gefecht)ms).open = trigger2On;
                                    }
                                    break;
                                default:
                                    break;

                            }

                        }
                        break;
                    case "collectable":
                        {

                            ms.Update(gameTime);
                            if (((SpriteClasses.CollectCell)ms).wasFull)
                            {
                                score += ((SpriteClasses.CollectCell)ms).GivePoints();
                                if (((SpriteClasses.CollectCell)ms).credit)
                                {
                                    soundEngine.Play("Collectible");
                                    ((SpriteClasses.CollectCell)ms).credit = false;
                                }
                            }

                        }
                        break;
                    case "Nerve":
                        {

                            ((SpriteClasses.NPCSprites.NerveCell)ms).Update(gameTime);
                            switch (((SpriteClasses.NPCSprites.NerveCell)ms).nameForLevel)
                            {
                                case "Nerve1":
                                    {
                                        nerve1On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Nerve2":
                                    {
                                        nerve2On = ((SpriteClasses.NPCSprites.NerveCell)ms).SwitchedOn;
                                    }
                                    break;
                                default:
                                    break;

                            }

                        }
                        break;
                    case "Trigger":
                        {


                            ((SpriteClasses.NPCSprites.TriggerCell)ms).Update(gameTime);
                            switch (((SpriteClasses.NPCSprites.TriggerCell)ms).nameForLevel)
                            {
                                case "Trigger1":
                                    {
                                        trigger1On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Trigger2":
                                    {
                                        trigger2On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                case "Trigger3":
                                    {
                                        trigger3On = ((SpriteClasses.NPCSprites.TriggerCell)ms).SwitchedOn;
                                    }
                                    break;
                                default:
                                    break;

                            }

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

                            ms.Update(gameTime, playerChanged);

                        }
                        break;
                    default:
                        break;


                }
            }

            #endregion




            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            world.AutoClearForces = true;


            oldPosition = playerPosition;
            playerController.Vibrate = alarm.IsVisible;


            playerController.UpdateControls(key, oldKey, controller, oldController);


            if (index != -1)
                allCells.RemoveAt(index);



            oldController = GamePad.GetState(PlayerIndex.One);
            oldKey = Keyboard.GetState();
        }



        #region collision


        bool collisionZiel(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            fadingTest.IsVisible = true;
            player.IsVisible = false;
            alarm.permanentlyDisable = true;

            return false;
        }

        #endregion



        #region Draw

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, LineBatch lineBatch)
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

            //despawn.Draw(spriteBatch);
            //fadingTest.DrawOverlay(spriteBatch);

        }
        #endregion
    }
}
