using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame.SpriteClasses.BloodControl
{
    public class ParticleSpawner : MovingSprite
    {
        private int spawnAmount = 3;
        private float spawnTimer = 0f;
        private float time;
        private int listCount;
        private Vector2 particleSpawnPosition;
        //private float rotation;
        private bool open = true;
        private int distance;
        private float speed;
        private int timer = 150;
        private int defaultTimer;
        public String nameForLevel;
        private float scale = 1f;
        private bool firstAdded;
        private float spin;
        


        private List<SpriteClasses.Parallax.Prop> propList = new List<SpriteClasses.Parallax.Prop>();

        public ParticleSpawner(Vector2 _position, float _rotation, int _distance, float _speed)
            : base(_position, _rotation, _distance, _speed)
        {
            scale = .5f;
            speed = _speed;
            position = _position;
            rotation = _rotation;
            distance = (int)(_distance * Globals.GlobalScale * scale);

            defaultTimer = timer = (int)(timer * scale);
            nameForLevel = Type = "ParticleSpawner";

            texture = null;
        }

        public override void Update(GameTime gameTime, Vector2 _change)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            listCount = propList.Count;

            if (!firstAdded)
            {
                AddParticles();
                firstAdded = true;
                spin += .5f;
            }

            if (spin > 6f)
                spin = 0f;

            //removes cells that are not visible or have null values
            for (int i = 0; i < propList.Count; i++)
                if (propList[i] == null || propList[i].BodyRemoved)
                {
                    propList.RemoveAt(i);
                }

            foreach (SpriteClasses.Parallax.Prop prop in propList)
            {
                prop.Update(gameTime, _change);

                if (prop.Alpha == 75)
                    firstAdded = false;

            }

        }


        private void AddParticles()
        {
            if (time >= spawnTimer && listCount < spawnAmount)
            {
                time = 0;
                var tempProp = new SpriteClasses.Parallax.Prop(Globals.particles, position, speed);
                tempProp.rotation = spin;
                propList.Add(tempProp);
            }

        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (SpriteClasses.Parallax.Prop prop in propList)
            {
                if(prop.IsVisible)
                    prop.Draw(spriteBatch);
            }
        }
    }
}
