using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace VirusGame
{
    class SoundEngine
    {
        ContentManager content;
        List<SoundEffectInstance> engine;
        List<SoundEffect> sounds;
        List<String> _sounds;

        public SoundEngine(ContentManager _content)
        {
            content = _content;
            sounds = new List<SoundEffect>();
            engine = new List<SoundEffectInstance>();
            _sounds = new List<String>();

            

            Load();
        }

        void Load()
        {
            string[] Files = System.IO.Directory.GetFiles("Content/Audio");

            for (int b = 0; b < 3; b++)
            {
                for (int i = 0; i < Files.Length; i++)
                {
                    sounds.Add(content.Load<SoundEffect>("Audio/" + System.IO.Path.GetFileNameWithoutExtension(Files[i]).ToString()));
                    _sounds.Add(System.IO.Path.GetFileNameWithoutExtension(Files[i]).ToString());
                    engine.Add(sounds[i].CreateInstance());
                 
                }
            }

        }

        public void Play(string soundname, float volume = 0.1f, bool repeat = false)
        {
            //bool playing = false;
            for (int i = 0; i < _sounds.Count; i++)
            {
                int a = i;
                if (_sounds[a].ToString() == soundname)
                {
                    if (engine[a].State == SoundState.Stopped && engine[a].State != SoundState.Playing)
                    {
                        //playing = true;
                        if (repeat)
                            engine[a].IsLooped = true;
                        engine[a].Volume = volume;
                        //engine[a].Pitch = -1f;
                        engine[a].Play();
                    }
                }
            }
        }



        public void StopAll()
        {
            for (int i = 0; i < _sounds.Count; i++)
            {
                int a = i;
                    engine[a].Stop();
            }
        }

        public void Stop(string soundname)
        {
            for (int i = 0; i < _sounds.Count; i++)
            {
                int a = i;
                if (_sounds[a].ToString() == soundname)
                {
                    engine[a].Stop();
                }
            }
        }

    }
}
