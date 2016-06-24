using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses
{
    public static class SpriteManager
    {
        
        private static World level;
        private static ContentManager content;
        private static Spawner empty;
        


        public static World Level
        {
            set
            {
                level = value;
            }
            get
            {
                return level;
            }
        }

        public static ContentManager Content
        {
            set
            {
                content = value;
            }
            get
            {
                return content;
            }
        }

        public static Texture2D getImage(String _location)
        {
            return content.Load<Texture2D>(_location);
        }



        #region get symbols textures
        /// <summary>
        /// returns a texture of the entered number/symbol
        /// </summary>
        /// <param name="_Text">0-9 or ":" "/" </param>
        /// <returns></returns>
        public static Texture2D getTextTexture(Char _Text)
        {
            switch (_Text)
            {
                case '0':
                    return content.Load<Texture2D>("Images/Numbers/0");
                case '1':
                    return content.Load<Texture2D>("Images/Numbers/1");
                case '2':
                    return content.Load<Texture2D>("Images/Numbers/2");
                case '3':
                    return content.Load<Texture2D>("Images/Numbers/3");
                case '4':
                    return content.Load<Texture2D>("Images/Numbers/4");
                case '5':
                    return content.Load<Texture2D>("Images/Numbers/5");
                case '6':
                    return content.Load<Texture2D>("Images/Numbers/6");
                case '7':
                    return content.Load<Texture2D>("Images/Numbers/7");
                case '8':
                    return content.Load<Texture2D>("Images/Numbers/8");
                case '9':
                    return content.Load<Texture2D>("Images/Numbers/9");
                case ':':
                    return content.Load<Texture2D>("Images/Numbers/colon");
                case '/' :
                    return content.Load<Texture2D>("Images/Numbers/slash");
                default:
                    return content.Load<Texture2D>("Images/Numbers/0");
            }

        }
        #endregion



        public static NPCSprites.BloodCell addBloodCell(Vector2 _position, Vector2 _velocity)
        {
            Random rand = new Random();
            return new SpriteClasses.NPCSprites.BloodCell(
                level, 
                content.Load<Texture2D>("Test/bloodcell spritesheet"), 
                new Vector2((float)(rand.Next((int)_position.X - 30, (int)_position.X + 30)), (float)rand.Next((int)_position.Y - 30, (int)_position.Y + 30)), 
                _velocity,
                5, 
                1);
        }



        public static NPCSprites.HilferCell addHilferCell(Vector2 _position, Vector2 _velocity)
        {
            return new SpriteClasses.NPCSprites.HilferCell(level, content.Load<Texture2D>("Sprites/spritesheet_helferzelle"), _position, _velocity, 10, 3);
        }


        public static NPCSprites.MakrophageCell addMakrophageCell(Vector2 _position, Vector2 _velocity)
        {
            return new SpriteClasses.NPCSprites.MakrophageCell(level, content.Load<Texture2D>("Test/enemyspritesheet"), _position, _velocity, 8, 2);
        }


        public static NPCSprites.MonozytCell addMonozytCell(Vector2 _position, Vector2 _velocity)
        {
            return new SpriteClasses.NPCSprites.MonozytCell(level, content.Load<Texture2D>("Images/monozyt spritesheet Kopie"), _position, _velocity, 10, 3);
        }

        public static BloodControl.BloodDespawner addBloodDespawner(Vector2 _position, float _rotation, float _scale)
        {
            return new SpriteClasses.BloodControl.BloodDespawner(level, content.Load<Texture2D>("Test/vene background"), _position, new Vector2(0, 0), 6, 1, _rotation, _scale);
        }

            
        public static NPCSprites.NerveCell addNerveCell(Vector2 _position, float _rotation)
        {
            return new SpriteClasses.NPCSprites.NerveCell(level, content.Load<Texture2D>("Sprites/Nervenew"), _position, new Vector2(0, 0), 9, 3, "toggle", _rotation);
        }

        public static NPCSprites.TriggerCell addTriggerCell(Vector2 _position, float _rotation)
        {
            return new SpriteClasses.NPCSprites.TriggerCell(level, content.Load<Texture2D>("Sprites/Triggernew"), _position, new Vector2(0, 0), 11, 3, "toggle", _rotation);
        }


        public static NPCSprites.PlasmaCell addPlasmaCell(Vector2 _position, Vector2 _velocity, char _direction)
        {
            return new SpriteClasses.NPCSprites.PlasmaCell(level, content.Load<Texture2D>("Images/plasmacellspritesheet"), _position, _velocity, 8, 6, _direction);
        }


        public static NPCSprites.PlasmaShot addPlasmaShot(Vector2 _position, Vector2 _velocity, char _direction)
        {
            return new NPCSprites.PlasmaShot(level, content.Load<Texture2D>("Test/tail"), new Vector2(_position.X, _position.Y), new Vector2(0, 0), 5, 1, _direction);
        }

        #region player stuff
        public static PlayerSprite addPlayerCell(Vector2 _position)
        {
            return new PlayerSprite(level, content.Load<Texture2D>("Sprites/Parasite_Animation"), _position, new Vector2(0,0), 12, 6);
        }

        public static Player.PlayerAttack addPlayerAttack(Vector2 _position)
        {
            return new SpriteClasses.Player.PlayerAttack(level, content.Load<Texture2D>("Sprites/parattack"), new Vector2(_position.X, _position.Y), new Vector2(0, 0), 18, 1);
        }

        public static Player.PlayerBleed addPlayerBleed()
        {
            return new SpriteClasses.Player.PlayerBleed(level, content.Load<Texture2D>("Sprites/PlayerTextures/blood_parasite_sheet"), new Vector2(0, 0), new Vector2(0, 0), 7, 1);
        }
        public static Player.PlayerHeal addPlayerHeal()
        {
            return new SpriteClasses.Player.PlayerHeal(level, content.Load<Texture2D>("Sprites/PlayerTextures/healsheet"), new Vector2(0, 0), new Vector2(0, 0), 6, 1);
        }
        public static Player.PlayerConfuse addPlayerConfuse()
        {
            return new SpriteClasses.Player.PlayerConfuse(level, content.Load<Texture2D>("Sprites/PlayerTextures/confusesheet"), new Vector2(0, 0), new Vector2(0, 0), 6, 1);
        }
        public static Player.PlayerCollect addPlayerCollect()
        {
            return new SpriteClasses.Player.PlayerCollect(level, content.Load<Texture2D>("Sprites/PlayerTextures/getcollectible"), new Vector2(0, 0), new Vector2(0, 0), 6, 1);
        }

        #endregion


        public static CollectCell addCollectCell(Vector2 _position, Vector2 _velocity)
        {
            return new SpriteClasses.CollectCell(level, content.Load<Texture2D>("Images/collectible2 Kopie"), _position, _velocity, 12, 1);
        }

        #region gefecht claps
        public static SpriteClasses.Valves.Gefecht addGefecht(Vector2 _position, float _rotation, float _scale)
        {
            return new SpriteClasses.Valves.Gefecht(level, content.Load<Texture2D>("Sprites/Gefechtspritesheet"), _position, new Vector2(0,0), 8, 2, _rotation, _scale);
        }


        public static SpriteClasses.Valves.Claps addClap(Vector2 _position, String _animation, float _rotation, float _scale)
        {
            if (_animation == "right" || _animation == "left")
                return new SpriteClasses.Valves.Claps(level, content.Load<Texture2D>("Images/valveL"), _position, new Vector2(0, 0), 1, 1, _animation, _rotation, _scale);
            else
                return new SpriteClasses.Valves.Claps(level, content.Load<Texture2D>("Images/valveL"), _position, new Vector2(0, 0), 1, 1, "left", _rotation, _scale);
        }
        #endregion

        public static SpriteClasses.fadingSprite addFadingSprite()
        {
            return new SpriteClasses.fadingSprite(level, content.Load<Texture2D>("Images/BGs/testEnd"), content.Load<Texture2D>("Images/BGs/testEndFader"), new Vector2(0, 0), new Vector2(0, 0), 4, 4);
        }

        public static UI.MainMenuAnimation addMainMenuAnimtion()
        {
            return new UI.MainMenuAnimation(level, content.Load<Texture2D>("UI/mainMenuAnimation"), new Vector2(0, 0), new Vector2(0, 0), 8, 1);
        }

        public static SpriteClasses.Menu.BubblingSprite addBubblingAnimation()
        {
            return new SpriteClasses.Menu.BubblingSprite(level, content.Load<Texture2D>("LoadingMedia/scoreBoard_blubber"), new Vector2(0, 0), new Vector2(0, 0), 5, 1);
        }

        public static SpriteClasses.Menu.FinishSprite addFinishAnimation(Vector2 _position)
        {
            return new SpriteClasses.Menu.FinishSprite(level, content.Load<Texture2D>("Images/Exit_light"), _position, new Vector2(0, 0), 20, 1);
        }

        public static SpriteClasses.NPCSprites.BrainSprite addBrain(Vector2 _position)
        {
            return new SpriteClasses.NPCSprites.BrainSprite(level, SpriteManager.getImage("Sprites/BrainTrigger"), _position, new Vector2(0, 0), 12, 2);
        }


        public static SpriteClasses.Parallax.Prop addProp(String _texture, Vector2 _position, float _rotation, float _speed, char _movementType, float _scale, float _depth)
        {
            _texture = "Images/BGs/" + _texture;
            return new SpriteClasses.Parallax.Prop(content.Load<Texture2D>(_texture), _position, _rotation, _speed, _movementType, _scale, _depth);
        }

        public static SpriteClasses.Parallax.Prop addSynapse(Vector2 _position, float _rotation, float _speed, float _scale, float _depth)
        {
            return new SpriteClasses.Parallax.Prop(content.Load<Texture2D>("Test/ChadSynapse"), _position, _rotation, _speed, 'S', _scale, _depth);
        }

    }
}
