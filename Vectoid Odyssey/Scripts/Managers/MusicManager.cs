using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace VectoidOdyssey
{
    static class MusicManager
    {
        private static Dictionary<string, SoundEffect> songs;

        public static void Init()
        {
            songs = new Dictionary<string, SoundEffect>();

            SoundEffect[] tempSongs = Load.GetCollection<SoundEffect>("Sound/Music");

            foreach (SoundEffect song in tempSongs)
            {
                songs.Add(song.Name.Split('/').LastOrDefault(), song);
            }
        }

        public static void Play(string aName)
        {
            // TODO: Fix music manager

            if (!songs.ContainsKey(aName))
            {
                Console.WriteLine("Tried to play nonexistent or unloaded song.");
                return;
            }

            Sound.PlaySong(songs[aName]);
        }
    }
}
