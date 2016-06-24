using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;

namespace VirusGame.SpriteClasses.BloodControl
{
    
    public class BloodSpawner : MovingSprite
    {
        private SpriteClasses.BloodControl.BloodAnimation bloodAnim;
        public bool playBloodSound;
        public bool playSound;
        private int spawnAmount = 100;
        private float spawnTimer = .05f;
        private float time;
        private int listCount;
        private Vector2 bloodSpawnPosition;
        //private float rotation;
        private bool open = true;
        private int distance;
        private float speed;
        private int timer = 140;
        private int defaultTimer;
        public String nameForLevel;
        private float scale = 1f;
        public bool wallCollide;
        private float cellMass = 10f;

        private List<SpriteClasses.NPCSprites.BloodCell> bloodList = new List<NPCSprites.BloodCell>();
        private Valves.Valve valve;

         public BloodSpawner(Vector2 _position, float _rotation, int _distance, float _speed, float _scale)
            : base(_position, _rotation, _distance, _speed)
        {
            scale = _scale;
            speed = _speed;
            position = _position;
            rotation = _rotation;
            distance = (int)(_distance * Globals.GlobalScale * _scale);

            valve = new Valves.Valve(position, rotation, distance, _scale);
            bloodSpawnPosition = BloodCellSpawner(rotation);
            BloodVelocity(rotation);
            defaultTimer = timer = (int)(timer * _scale);
            //spawnAmount = timer / 2;
            Type = "BloodSpawner";
            

        }

         public SpriteClasses.BloodControl.BloodAnimation BloodAnim
         {
             set { bloodAnim = value; }
         }

        public int Timer
        {
            set { timer = value; }
        }
        public int ListCount
        {
            get { return listCount; }
        }
        public float SpawnTimer
        {
            set { spawnTimer = value; }
        }

        public float CellMass
        {
            set {
                float ratio = (value / cellMass);
                cellMass = value;
                speed = speed * ratio;
                BloodVelocity(rotation);
                }

        }



        public bool Open
        {
            set { open = value; }
            get { return open; }
        }


        public override void Update(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            listCount = bloodList.Count;
            valve.open = Open;

            if (Open)
            {
                AddCell();
            }

            //removes cells that are not visible or have null values
            for (int i = 0; i < bloodList.Count; i++)
                if (bloodList[i] == null || bloodList[i].BodyRemoved)
                {
                    bloodList.RemoveAt(i);
                }

            foreach (SpriteClasses.NPCSprites.BloodCell bloodCell in bloodList)
            {
                bloodCell.Update(gameTime);

                if (bloodCell.Timer <= timer / 2f && !bloodCell.despawnerCollide)
                {
                    bloodCell.despawnerCollide = true;
                }

                if (!bloodCell.firstRun && wallCollide)
                {
                    Random a = new Random();
                    bloodCell.body.CollisionCategories = Category.Cat2;
                    bloodCell.body.CollidesWith = ~Category.Cat2;
                    //bloodCell.body.CollidesWith = Category.All & ~Category.Cat2;
                    //bloodCell.animationManager.CurrentFrame = a.Next(1, 4);
                    bloodCell.Timer = timer;
                    bloodCell.Mass = cellMass;
                    bloodCell.firstRun = true;
                }
                if (!bloodCell.firstRun)
                {
                    Random a = new Random();
                    bloodCell.body.CollisionCategories = Category.Cat2;
                    //bloodCell.body.CollidesWith = Category.All & ~Category.Cat2;
                    //bloodCell.animationManager.CurrentFrame = a.Next(1, 4);
                    bloodCell.Timer = timer;
                    bloodCell.firstRun = true;
                }
                
                bloodCell.AnimationMan = bloodAnim.getManager(bloodCell.AnimationNumber);
                
                
                bloodCell.velocity = bloodCell.InitialVelocity;
            }

            valve.Update(gameTime);
        }

        private void AddCell()
        {
            if (time >= spawnTimer && listCount < spawnAmount)
            {
                time = 0;
                bloodList.Add(SpriteClasses.SpriteManager.addBloodCell(bloodSpawnPosition, velocity));
            }

        }

        private void BloodVelocity(float _rotation)
        {

            float X = (float)(Math.Cos(_rotation));
            float Y = (float)(Math.Sin(_rotation));
            Vector2 newPos = new Vector2(X, Y);
            velocity = newPos * new Vector2(-speed, -speed);
        }

        private Vector2 BloodCellSpawner(float _rotation)
        {
            return position + (new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation )) * ((distance * 2.5f * Globals.GlobalScale) * Globals.GlobalScale));

            //float X = (float)(Math.Cos(_rotation));
            //float Y = (float)(Math.Sin(_rotation));
            //Vector2 newPos = new Vector2(X, Y);
            //return position + newPos * distance;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteClasses.SpriteManager.Content.Load<Texture2D>("Test/lower"), position, null, Color.White, rotation - .1f, new Vector2(150 / scale, 150), scale * .35f * Globals.GlobalScale, SpriteEffects.None, 0.074f + .001f);
            spriteBatch.Draw(SpriteClasses.SpriteManager.Content.Load<Texture2D>("Test/upper"), position, null, Color.White, rotation - .1f, new Vector2(150 / scale, 150), scale * .35f * Globals.GlobalScale, SpriteEffects.None, 0.074f - .001f);
            foreach(SpriteClasses.NPCSprites.BloodCell bloodCell in bloodList)
            {
                bloodCell.Draw(spriteBatch);
            }

            valve.Draw(spriteBatch);

        }
    }
}
