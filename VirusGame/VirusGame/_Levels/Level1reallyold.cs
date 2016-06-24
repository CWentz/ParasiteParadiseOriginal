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
    public class Level1old
    {

        #region Declarations

        //score section//////
        private int seconds;
        public int score;
        private int collected;
        private int restored;
        private int kills;
        
        /////////////////////

        public bool init = false;

        private Vector2 playerPosition;
        public Vector2 oldPosition;
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

        public SpriteClasses.BloodControl.BloodSpawner bloodSpawner1;
        public SpriteClasses.BloodControl.BloodSpawner bloodSpawner2;


        SpriteClasses.alarmSprite alarm;

        List<Object> allCells = new List<Object>();
        List<VelocityRect> velocityRectangles = new List<VelocityRect>();


        Texture2D tex;

        public SpriteClasses.Parallax.Background paralaxBG;


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

        public SpriteClasses.PlayerSprite player;
        public SpriteClasses.NPCSprites.HilferCell helper;
        public SpriteClasses.NPCSprites.MonozytCell monozyt;
        public SpriteClasses.NPCSprites.MakrophageCell makrophage;
        public SpriteClasses.NPCSprites.PlasmaCell plasma;
        public SpriteClasses.NPCSprites.NerveCell bloodValveNerveLowerLeft;

        public SpriteClasses.NPCSprites.NerveCell valveBULeft;
        public SpriteClasses.NPCSprites.NerveCell valveBULower;
        private Vector2 playerChanged;

        public SpriteClasses.NPCSprites.TriggerCell triggerCellRight;
        public SpriteClasses.NPCSprites.TriggerCell triggerCellLower;
        public SpriteClasses.NPCSprites.TriggerCell triggerCellLeft;
        
        public SpriteClasses.NPCSprites.PlasmaCell plasma2;
        public SpriteClasses.NPCSprites.MonozytCell monozyt2;
        public SpriteClasses.NPCSprites.MonozytCell monozyt3;
        public SpriteClasses.NPCSprites.HilferCell helper2;

        public SpriteClasses.fadingSprite fadingTest;

        public SpriteClasses.Parallax.Prop bloodBG;
        public SpriteClasses.Parallax.Prop bloodBG2;
        public SpriteClasses.Parallax.Prop bloodBG3;
        public SpriteClasses.Parallax.Prop veinBG;

        public SpriteClasses.Valves.Gefecht valveMiddle;
                
        #endregion

        public DebugViewXNA _debugView;


        /// <summary>
        /// Creates a new level, sets up Farseer world object and loads tiles
        /// </summary>
        /// <param name="serviceProvider"></param>
        public Level1old(ContentManager serviceProvider, GraphicsDevice graphicDevice)
        {
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
            _debugView.SleepingShapeColor = Color.LightGray;
            _debugView.LoadContent(graphicDevice, content);
            
        }

        /// <summary>
        /// Tile loading method, reads gleed2d xml-file and sorts out paths and textures
        /// </summary>
        protected void LoadTiles()
        {

            SpriteClasses.SpriteManager.Content = content;
            SpriteClasses.SpriteManager.Level = world;

            Vector2 playerSpawn = new Vector2(0, 0);

            using (Stream stream = TitleContainer.OpenStream("Content/Levels/level1.gleed")) //#1 change gere you level file path
            {
                XElement xml = XElement.Load(stream);
                level = LevelLoader.Load(xml);
            }

            tiles = new List<Tile>();
            pathTiles = new List<PathTile>();
            textures = new List<MyTexture>();

            paralaxBG = new SpriteClasses.Parallax.Background(Content.Load<Texture2D>("Levels/level1_bg"), 20f);
            bloodBG = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/bloodcellparalax"), new Vector2(-1000, 1000), new Vector2(1, -3), 150f, 'R', 1f);
            bloodBG2 = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/bloodcellparalax"), new Vector2(-2000, -2000), new Vector2(3, 2), 80f, 'B', .7f);
            bloodBG3 = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/bloodcellparalax"), new Vector2(1000, 1000), new Vector2(-4, -1), 700f, 'W', 3f);
            veinBG = new SpriteClasses.Parallax.Prop(Content.Load<Texture2D>("Images/BGs/venesParalax"), new Vector2(-1100, -500), new Vector2(0, 0), 50f, 'F', 2f);
            veinBG.Rotation = (float)(2.0 / Math.PI);

            allCells.Add(paralaxBG);
            allCells.Add(bloodBG);
            allCells.Add(veinBG);
            allCells.Add(bloodBG2);

            #region layers
            foreach (Layer layer in level.Layers)
            {
                if (layer.Properties.Name.Equals("Collision")) //2# change here your layer's name
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

                if (layer.Properties.Name.Equals("Textures"))
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

                if (layer.Properties.Name.Equals("Player"))
                {
                    foreach (LayerItem item in layer.Items)
                    {
                        if (item.Properties is CircleItemProperties)
                        {
                            CircleItemProperties textureProperties = item.Properties as CircleItemProperties;
                            playerSpawn = textureProperties.Position;
                            //allCells.Add(SpriteClasses.SpriteManager.addPlayerCell(playerSpawn));
                        }
                    }
                }
                if (layer.Properties.Name.Equals("End"))
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
                if (layer.Properties.Name.Equals("Collectibles"))
                {
                    foreach (LayerItem item in layer.Items)
                    {
                        if (item.Properties is CircleItemProperties)
                        {
                            CircleItemProperties textureProperties = item.Properties as CircleItemProperties;

                            allCells.Add(SpriteClasses.SpriteManager.addCollectCell(new Vector2(textureProperties.Position.X, textureProperties.Position.Y), new Vector2(0, 0)));
                        }
                    }
                }

                if (layer.Properties.Name.Equals("Path"))
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
            }

            #endregion

            player = SpriteClasses.SpriteManager.addPlayerCell(playerSpawn);
            makrophage = SpriteClasses.SpriteManager.addMakrophageCell(new Vector2(-900, -550), new Vector2(0, 0));
            helper = SpriteClasses.SpriteManager.addHilferCell(new Vector2(200, 200), new Vector2(0, 0));
            monozyt = SpriteClasses.SpriteManager.addMonozytCell(new Vector2(580, -100), new Vector2(0, 0));
            plasma = SpriteClasses.SpriteManager.addPlasmaCell(new Vector2(-250, 100), new Vector2(0, 0), 'd');

            plasma2 = SpriteClasses.SpriteManager.addPlasmaCell(new Vector2(-260, -130), new Vector2(0, 0), 'd');
            monozyt2 = SpriteClasses.SpriteManager.addMonozytCell(new Vector2(-600, -350), new Vector2(0, 0));
            monozyt3 = SpriteClasses.SpriteManager.addMonozytCell(new Vector2(-680, -650), new Vector2(0, 0));
            helper2 = SpriteClasses.SpriteManager.addHilferCell(new Vector2(660, 300), new Vector2(0, 0));

            //valveMiddle = SpriteClasses.SpriteManager.addGefecht(new Vector2(290, -160), 0f);

            //lower nerve
            bloodValveNerveLowerLeft = SpriteClasses.SpriteManager.addNerveCell(new Vector2(-940, 630), 1f);
            
            //upper nerve
            triggerCellRight = SpriteClasses.SpriteManager.addTriggerCell(new Vector2(540, -230), (float)Math.PI);
            triggerCellLeft = SpriteClasses.SpriteManager.addTriggerCell(new Vector2(-540, -20), -.9f);
            triggerCellLower = SpriteClasses.SpriteManager.addTriggerCell(new Vector2(323, 20), (float)((5.0 * Math.PI) / 4.0));
            
            triggerCellLower.SwitchedOn = false;
            triggerCellLeft.SwitchedOn = false;
            triggerCellRight.SwitchedOn = false;
            

            //center nerve
            valveBULeft = SpriteClasses.SpriteManager.addNerveCell(new Vector2(-170, -210), (float)Math.PI);
            valveBULower = SpriteClasses.SpriteManager.addNerveCell(new Vector2(780, 290), (float)Math.PI);

            //bloodValveNerveLowerLeft.rotation = 1f;

            //triggerCellLeft.rotation = -.9f;
            //triggerCellLower.rotation = (float)((5.0*Math.PI)/4.0);
            //triggerCellRight.rotation = (float)Math.PI;

            //valveBULeft.rotation = (float)Math.PI;
            //valveBULower.rotation = (float)Math.PI;
            
            alarm = new SpriteClasses.alarmSprite(world, Content.Load<Texture2D>("Images/alarm"), new Vector2(0, 0), new Vector2(0, 0), 4, 1);

            bloodSpawner1 = new SpriteClasses.BloodControl.BloodSpawner(new Vector2(940, -570), -MathHelper.Pi / 4f, 70, 17.5f, 1f);
            bloodSpawner2 = new SpriteClasses.BloodControl.BloodSpawner(new Vector2(-920, 75), -2.51f, 75, 17.5f, 1f);
            

            fadingTest = SpriteClasses.SpriteManager.addFadingSprite();

            //player.setupAttack();
            
            allCells.Add(player);
            allCells.Add(helper);
            allCells.Add(helper2);
            allCells.Add(monozyt);
            allCells.Add(makrophage);
            allCells.Add(plasma);
            allCells.Add(bloodValveNerveLowerLeft);
            allCells.Add(valveBULeft);
            allCells.Add(valveBULower);
            allCells.Add(triggerCellRight);
            allCells.Add(triggerCellLower);
            allCells.Add(triggerCellLeft);
            allCells.Add(plasma2);
            allCells.Add(alarm);
            allCells.Add(bloodSpawner1);
            allCells.Add(bloodSpawner2);
            allCells.Add(valveMiddle);
            allCells.Add(bloodBG3);
            allCells.Add(fadingTest);
            

        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            seconds += (int)gameTime.ElapsedGameTime.TotalSeconds;

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


                            collected = ((SpriteClasses.PlayerSprite)ms).Collected;
                            restored = ((SpriteClasses.PlayerSprite)ms).Restored;

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
                                if (player.IsVisible)
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
                            ms.Update(gameTime);
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
                            ms.Update(gameTime);
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
                        }
                        break;
                    case "Trigger":
                        {
                            ((SpriteClasses.NPCSprites.TriggerCell)ms).Update(gameTime);
                        }
                        break;
                    case "Fading":
                        {
                            ((SpriteClasses.fadingSprite)ms).Update(gameTime);
                            if (((SpriteClasses.fadingSprite)ms).active)
                            {
                                ((SpriteClasses.fadingSprite)ms).position = followCoord;
                            }

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
 
            if (bloodValveNerveLowerLeft.SwitchedOn)
            {
                bloodSpawner2.Open = true;
            }
            else bloodSpawner2.Open = false;


            if (triggerCellLeft.SwitchedOn && triggerCellLower.SwitchedOn || triggerCellRight.SwitchedOn)
            {
                valveMiddle.open = true;
            }
            else valveMiddle.open = false;


            if (valveBULeft.SwitchedOn || valveBULower.SwitchedOn)
            {
                bloodSpawner1.Open = true;
            }
            else bloodSpawner1.Open = false;

     
            //collected = player.Collected;
            //restored = player.Restored;

            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

            oldPosition = playerPosition;
            playerController.Vibrate = alarm.IsVisible;
            playerController.Attack = player.Attacking;
            playerController.FlippedControls = player.FlippedControls;

            playerController.UpdateControls(key, oldKey, controller, oldController);

            

            //collected = player.Collected;
            //restored = player.Restored;

            #region SoundEngine
            if (playerController.attack())
                soundEngine.Play("Player_Attack", 0.05f);
            if (player.Lives <= 0 && !player.gameover)
                soundEngine.Play("Player_Death", 0.5f);

            if(plasma.IsVisible)
                if(plasma.proximityShoot)
                    soundEngine.Play("Plasmacell_Attack");
            if (plasma.proximity && plasma.IsVisible)
                soundEngine.Play("Plasmacell_Death");

            if (monozyt.proximity && monozyt.IsVisible || monozyt.attacked && monozyt.IsVisible)
                soundEngine.Play("Monozyt_Death");

            if (helper.IsVisible)
                soundEngine.Play("Helferzelle_Alarm", 0.01f);
            if (!helper.IsVisible)
                soundEngine.Stop("Helferzelle_Alarm");

            
            #endregion

            if (index != -1)
                allCells.RemoveAt(index);
            
            

            oldController = GamePad.GetState(PlayerIndex.One);
            oldKey = Keyboard.GetState();
        }



        #region collision 


        bool collisionZiel(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            fadingTest.active = true;
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
            
            //fadingTest.DrawOverlay(spriteBatch);

        }
        #endregion
    }
}
