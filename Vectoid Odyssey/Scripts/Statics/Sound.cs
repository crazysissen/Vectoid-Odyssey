using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace VectoidOdyssey
{
    static class Sound
    {
        public static float SFXVolume { get; private set; }
        public static float MusicVolume { get; private set; }

        private static Dictionary<string, SoundEffect> effects;

        private static List<SoundEffectInstance> playingEffects;
        private static SoundEffectInstance playingSong;

        public static void Init()
        {
            playingEffects = new List<SoundEffectInstance>();
            effects = new Dictionary<string, SoundEffect>();

            SoundEffect[] tempEffects = Load.GetCollection<SoundEffect>("Sound/SFX");

            foreach (SoundEffect effect in tempEffects)
            {
                effects.Add(effect.Name, effect);
            }
        }

        public static void DeInit()
        {
            effects = null;

            StopSong();
            StopAllEffects();
        }

        public static void Update()
        {
            for (int i = playingEffects.Count; i >= 0; --i)
            {
                if (playingEffects[i].State == SoundState.Stopped)
                {
                    playingEffects.RemoveAt(i);
                }
            }
        }

        public static void PlayEffect(string aName)
        {
            SoundEffectInstance tempInstance = effects[aName].CreateInstance();
            tempInstance.Play();
            tempInstance.Volume = SFXVolume;
            
            playingEffects.Add(tempInstance);
        }

        public static SoundEffect Effect(string aName)
            => effects[aName];

        public static void SetSFXVolume(float volume)
        {
            SFXVolume = volume;

            foreach (SoundEffectInstance effect in playingEffects)
            {
                effect.Volume = volume;
            }
        }

        public static void SetMusicVolume(float volume)
        {
            MusicVolume = volume;

            playingSong.Volume = volume;
        }

        public static void PlaySong(SoundEffect effect)
        {
            StopSong();

            playingSong = effect.CreateInstance();
            playingSong.IsLooped = true;
            playingSong.Play();
        }

        public static void StopSong()
        {
            playingSong?.Stop();
            playingSong = null;
        }

        public static void PauseAllEffects()
        {
            foreach (SoundEffectInstance effect in playingEffects)
            {
                effect.Pause();
            }
        }

        public static void ResumeAllEffects()
        {
            foreach (SoundEffectInstance effect in playingEffects)
            {
                effect.Resume();
            }
        }

        public static void StopAllEffects()
        {
            foreach (SoundEffectInstance effect in playingEffects)
            {
                effect.Stop();
                effect.Dispose();
            }

            playingEffects.Clear();
        }
    }
}
