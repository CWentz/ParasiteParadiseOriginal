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


namespace VirusGame
{
    /// <summary>
    /// Class representing a game level. It loads tiles from .gleed file and draws them onto screen.
    /// Tiles set their bodies by themself
    /// </summary>
    public class Level1
    {
        #region Declarations
        GraphicsDevice graphicDevice;
        Controls playerController = new Controls();

        KeyboardState key;
        KeyboardState oldKey;
        GamePadState controller;
        GamePadState oldController;

        Spawner bloodSpawn1;
        Spawner bloodSpawn2;

        //list of blood cells for testing
        List<SpriteClasses.MovingSprite> cells = new List<SpriteClasses.MovingSprite>();
        List<SpriteClasses.MovingSprite> cells2 = new List<SpriteClasses.MovingSprite>();
        List<SpriteClasses.CollectCell> collectibles = new List<SpriteClasses.CollectCell>();

        List<VelocityRect> velocityRectangles = new List<VelocityRect>();

        Texture2D tex;
        public SpriteClasses.BackgroundSprite paralaxBG;

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
        public SpriteClasses.NPCSprites.MakrophageCell blood;
        public SpriteClasses.NPCSprites.PlasmaCell plasma;
        public SpriteClasses.NPCSprites.NerveCell nerve1;
        public SpriteClasses.NPCSprites.NerveCell nerve2;

        //GravityController test = new GravityController(1f, 5,1);
                        
        #endregion
        

        /// <summary>
        /// Creates a new level, sets up Farseer world object and loads tiles
        /// </summary>
        /// <param name="serviceProvider"></param>
        public Level1(ContentManager serviceProvider, GraphicsDevice graphicDevice)
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

            paralaxBG = new SpriteClasses.BackgroundSprite(Content.Load<Texture2D>("Levels/level1_bg"), new Vector2(0, 0), 7, 10, graphicDevice.Viewport.Width, graphicDevice.Viewport.Height);
        }

        /// <summary>
        /// Tile loading method, reads gleed2d xml-file and sorts out paths and textures
        /// </summary>
        protected void LoadTiles()
        {

            Vector2 playerSpawn = new Vector2(0, 0);

            using (Stream stream = TitleContainer.OpenStream("Content/Levels/level1.gleed")) //#1 change gere you level file path
            {
                XElement xml = XElement.Load(stream);
                level = LevelLoader.Load(xml);
            }

            tiles = new List<Tile>();
            pathTiles = new List<PathTile>();
            textures = new List<MyTexture>();

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
                            myTexture = new MyTexture(texture, textureProperties.Position, textureProperties.Rotation, textureProperties.Scale);
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
                            collectibles.Add(new SpriteClasses.CollectCell(world, Content.Load<Texture2D>("Images/collectible2 Kopie"), new Vector2(textureProperties.Position.X, textureProperties.Position.Y), new Vector2(0, 0), 12, 1));
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

            player = new SpriteClasses.PlayerSprite(world, Content.Load<Texture2D>("Test/anim_sp"), playerSpawn, new Vector2(0, 0), 4, 1);
            blood = new SpriteClasses.NPCSprites.MakrophageCell(world, Content.Load<Texture2D>("Test/enemyspritesheet"), new Vector2(-900, -600), new Vector2(0, 0), 5, 2);


            helper = new SpriteClasses.NPCSprites.HilferCell(world, Content.Load<Texture2D>("Images/helferzelle_alarm"), new Vector2(200, 200), new Vector2(0, 0), 4, 1);
            monozyt = new SpriteClasses.NPCSprites.MonozytCell(world, Content.Load<Texture2D>("Images/monozyt spritesheet"), new Vector2(400, 200), new Vector2(0, 0), 14, 1);
            plasma = new SpriteClasses.NPCSprites.PlasmaCell(world, Content.Load<Texture2D>("Images/plasmacellmoving spritesheet"), new Vector2(400, 300), new Vector2(0, 0), 8, 1);
            
            
            //upper
            nerve1 = new SpriteClasses.NPCSprites.NerveCell(world, Content.Load<Texture2D>("Test/nerve"), new Vector2(-935, 625), new Vector2(0, 0), 4, 2, "toggle");
            nerve1.rotation = 1f;
            
            //lower
            nerve2 = new SpriteClasses.NPCSprites.NerveCell(world, Content.Load<Texture2D>("Test/nerve"), new Vector2(550, -250), new Vector2(0, 0), 4, 2, "toggle");
            nerve2.rotation = (float)Math.PI;
            
            for (int i = 0; i < collectibles.Count; i++)
                collectibles[i].body.OnCollision += collectCoin;

            //lower spawner
            bloodSpawn1 = new Spawner(Content, 50, .04f, new Vector2(-1100, -33), new Vector2(4.5f, 3f), "bloodcell");

            //upper spawner
            bloodSpawn2 = new Spawner(Content, 50, .04f, new Vector2(1030, -630), new Vector2(-4.5f, 3f), "bloodcell");

            player.setupAttack(Content);
          

        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {


            for (int i = 0; i < velocityRectangles.Count; i++)
                velocityRectangles[i].body.FixtureList[0].OnCollision += Body_OnCollision;

            key = Keyboard.GetState();
            controller = GamePad.GetState(PlayerIndex.One);

            player.UpdateTest(gameTime);
            blood.UpdateTest(gameTime);
            plasma.UpdateTest(gameTime);
            monozyt.UpdateTest(gameTime);
            helper.UpdateTest(gameTime);
            nerve1.UpdateTest(gameTime);
            nerve2.UpdateTest(gameTime);

            

            plasma.body.OnCollision += PlasmaCellCollision;

            foreach (SpriteClasses.CollectCell cc in collectibles)
                cc.UpdateTest(gameTime);

            #region blood cell load, updates, & remove

            if (nerve1.SwitchedOn)
            {
                bloodSpawn1.Update(gameTime, cells.Count);
                cells.Add(bloodSpawn1.AddSpawn(world));
            }
            if (nerve2.SwitchedOn)
            {
                bloodSpawn2.Update(gameTime, cells2.Count);
                cells2.Add(bloodSpawn2.AddSpawn(world));
            }
            //removes cells that are not visible or have null values
            for (int i = 0; i < cells.Count; i++)
                if (cells[i] == null || !cells[i].isVisible)
                {
                    //if (cells[i] != null)
                        //world.RemoveBody(cells[i].body);
                    cells.RemoveAt(i);
                    
                }

            for (int i = 0; i < cells2.Count; i++)
                if (cells2[i] == null || !cells2[i].isVisible)
                {
                    //if (cells2[i] != null)
                        //world.RemoveBody(cells2[i].body);
                    cells2.RemoveAt(i);
                    
                }


            foreach (SpriteClasses.NPCSprites.BloodCell ms2 in cells2)
            {

                ms2.UpdateTest(gameTime);
                if (!ms2.firstRun)
                {
                    ms2.body.CollisionCategories = Category.Cat2;
                    ms2.body.CollidesWith = Category.All & ~Category.Cat2 & ~Category.Cat15;
                    //quadTree.Add(ms);
                    ms2.firstRun = true;
                }
                ms2.timer--;

                if (ms2.timer == 0)
                    ms2.isVisible = false;
                ms2.speedGovernor();

            }



            foreach (SpriteClasses.NPCSprites.BloodCell ms in cells)
            {

                ms.UpdateTest(gameTime);
                if (!ms.firstRun)
                {
                    ms.body.CollisionCategories = Category.Cat2;
                    ms.body.CollidesWith = Category.All & ~Category.Cat2 & ~Category.Cat15;
                    //quadTree.Add(ms);
                    ms.firstRun = true;
                }
                ms.timer--;
                if (ms.timer == 0)
                    ms.isVisible = false;
                ms.speedGovernor();
            }


            

            #endregion

            if (player.isVisible)
                player.attacked(blood.chase(player.position));
            else blood.chase(true);

            nerve1.body.OnCollision += nerve1_OnCollision;
            nerve2.body.OnCollision += nerve2_OnCollision;
            plasma.body.OnCollision += plasma_OnCollision;


            //world.RayCast(player.fixture, player.Position, player.attackRay())
            //{
            
            //}

            

            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));


            followCoord = player.getPosition;
            if (followCoord.Y < -415)
                followCoord.Y = -415;
            if (followCoord.Y > 415)
                followCoord.Y = 415;

            if (followCoord.X < -535)
                followCoord.X = -535;
            if (followCoord.X > 535)
                followCoord.X = 535;

            //if (!blood.blackhole)
            //{
            //    player.body.IsStatic = true;
            //    test.Enabled = true;
            //    test.AddBody(player.body);
            //    test.GravityType = GravityType.DistanceSquared;
            //    test.IsActiveOn(player.body);
            //    test.World = world;
            //    test.AddPoint(player.origin);
            //    player.body.CollidesWith = ~Category.All;
            //    //player.body.CollidesWith = ~Category.Cat10;
            //    blood.body.CollidesWith = ~Category.Cat5;
            //    test.Update(1f);
            //}

            paralaxBG.Update(gameTime, playerController, player.getPosition);

            playerController.sendControllers(key, oldKey, controller, oldController);

            oldController = GamePad.GetState(PlayerIndex.One);
            oldKey = Keyboard.GetState();
        }

        bool plasma_OnCollision(Fixture _bodyA, Fixture _bodyB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (_bodyB.UserData as string == "Player")
            {
                player.flipControls();
                return true;
            }
            return true;
        }

        bool collectCoin(Fixture _bodyA, Fixture _bodyB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (_bodyB.UserData as string == "Player")
                _bodyA.Body.FixtureList[0].UserData = "!remove!";
            return false;
        }


        bool nerve1_OnCollision(Fixture _bodyA, Fixture _bodyB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (_bodyB.UserData as string == "Player")
            {
                return true;
            }
            if (_bodyB.UserData as string == "Pattack" && player.attacktimer > 0)
                nerve1.UpdateSwitch();

            return false;
        }

        bool nerve2_OnCollision(Fixture _bodyA, Fixture _bodyB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (_bodyB.UserData as string == "Player")
            {
                return true;
            }
            if (_bodyB.UserData as string == "Pattack" && player.attacktimer > 0)
                nerve2.UpdateSwitch();

            return false;
        }
        bool PlasmaCellCollision(Fixture _bodyA, Fixture _bodyB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (_bodyB.UserData as string == "Player")
            {
                player.flipControls();
                return true;
            }
            return true;
        }
        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            player.BloodStreamVelocity(fixtureA.Body.Rotation);
            return false;
        }

        #region Draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, LineBatch lineBatch)
        {

            paralaxBG.Draw(spriteBatch);

            foreach (SpriteClasses.MovingSprite ms in cells)
            {
                ms.aniM.FramesPerSecond = 5;
                ms.DrawAnim(spriteBatch);
            }

            foreach (SpriteClasses.MovingSprite ms2 in cells2)
            {
                ms2.aniM.FramesPerSecond = 5;
                ms2.DrawAnim(spriteBatch);
            }

            foreach (Tile temp in tiles)
            {
                temp.Draw(spriteBatch, tex);
            }
            player.attackSprite.DrawAnim(spriteBatch);
            player.DrawAnim(spriteBatch);
            foreach (MyTexture t in textures)
            {
                t.Draw(spriteBatch);
            }
            
            
            blood.DrawAnim(spriteBatch);
            plasma.DrawAnim(spriteBatch);
            monozyt.DrawAnim(spriteBatch);
            helper.DrawAnim(spriteBatch);
            nerve1.DrawAnim(spriteBatch);
            nerve2.DrawAnim(spriteBatch);

            for (int i = 0; i < collectibles.Count; i++)
                if (!collectibles[i].isCollected)
                    collectibles[i].DrawAnim(spriteBatch);
        }
        #endregion
    }
}
