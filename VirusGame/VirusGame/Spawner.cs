using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;

namespace VirusGame
{

    //test
    public class Spawner
    {
        public int spawnAmount;
        public float spawnTimer;
        public float time;
        public int listCount;
        public String spawn;
        private SpriteClasses.NPCSprites.BloodCell temp;
        private Vector2 position;
        private Vector2 velocity;
        char direction;
        Random rand = new Random();
       

        /// <summary>
        /// Creates a mob spawner.
        /// </summary>
        /// <param name="_content">content manager</param>
        /// <param name="_spawnAmount">amount of mobs to spawn</param>
        /// <param name="_spawnTimer">time between mob spawns</param>
        /// <param name="_position"> position of spawner</param>
        /// <param name="_velo">initial velocity of spawned mobs</param>
        /// <param name="_spawn">String value of what mob type "bloodcell", "makrophage", "plasma", "monozyt" </param>
        public Spawner(int _spawnAmount, float _spawnTimer, Vector2 _position, Vector2 _velo, String _spawn)
        {
            spawnAmount = _spawnAmount;
            spawnTimer = _spawnTimer;
            spawn = _spawn;
            position = _position;
            velocity = _velo;
        }

        public Spawner(int _spawnAmount, float _spawnTimer, Vector2 _position, Vector2 _velo, String _spawn, char _direction)
        {
            spawnAmount = _spawnAmount;
            spawnTimer = _spawnTimer;
            spawn = _spawn;
            position = _position;
            velocity = _velo;
            direction = _direction;
        }

        public SpriteClasses.NPCSprites.BloodCell AddSpawn()
        {
            if (time >= spawnTimer && listCount < spawnAmount)
            {
                time = 0;
                return SpriteClasses.SpriteManager.addBloodCell(position, velocity);
            }
            return temp;
        }

        public void Update(GameTime gameTime, int _listCount)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            listCount = _listCount;
        }
    }
}
