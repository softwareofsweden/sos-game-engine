using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using SharpMik;

namespace SosEngine
{

    /// <summary>
    /// Mod music player. Wrapper class for SharpMod
    /// </summary>
    public class ModPlayer : IDisposable
    {

        /// <summary>
        /// Is song currently playing?
        /// </summary>
        private bool isPlaying = false;

        /// <summary>
        /// The currently loaded song
        /// </summary>
        // private Module song = null;
        private string currentResourceName;

        /// <summary>
        /// Module player
        /// </summary>
        // private SharpMik.Player.MikMod modulePlayer = null;

        public ModPlayer()
        {
            /*
            modulePlayer = new SharpMik.Player.MikMod();
            modulePlayer.Init<SharpMik.Drivers.NaudioDriver>();
            
            modulePlayer.PlayerStateChangeEvent += new SharpMik.Player.ModPlayer.PlayerStateChangedEvent(PlayerStateChangeEvent);
            */
        }

        /*
        public void PlayerStateChangeEvent(SharpMik.Player.ModPlayer.PlayerState state)
        {
            if (state == SharpMik.Player.ModPlayer.PlayerState.kStopped)
            {
                modulePlayer.Play(song);
                PlayMusic(currentResourceName);
            }
        }
        */

        /// <summary>
        /// Starts playing specified resource
        /// </summary>
        /// <param name="resourceName">The case-sensitive name of the manifest resource</param>
        public void PlayMusic(string resourceName)
        {
            if (isPlaying)
            {
                StopMusic();
            }

            currentResourceName = resourceName;

            // Load song from resource
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                // song = modulePlayer.LoadModule(stream);
            }

            // song.volume = 40;

            // Start playing
            // modulePlayer.Play(song);
            isPlaying = true;
        }

        public void PauseMusic()
        {
            
        }

        public void ResumeMusic()
        {
            
        }

        /// <summary>
        /// Stop the music
        /// </summary>
        public void StopMusic()
        {
            if (isPlaying)
            {
                //modulePlayer.Stop();
                //modulePlayer.UnLoadModule(song);
                //song = null;
                isPlaying = false;
            }
        }

        public void Dispose()
        {
            StopMusic();
            //modulePlayer.Exit();
        }
    }
}
