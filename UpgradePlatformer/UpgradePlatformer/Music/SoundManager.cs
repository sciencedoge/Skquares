using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace UpgradePlatformer.Music
{
    //HEADER============================================
    //Name: Sami Chamberlain, Preston Precourt
    //Date: 5/27/2021
    //Purpose: Manages all of the sound in this project
    //==================================================
    class SoundManager
    {
        private static readonly Lazy<SoundManager>
            lazy =
            new Lazy<SoundManager>
                (() => new SoundManager());
        public static SoundManager Instance { get { return lazy.Value; } }
        public ContentManager content;

        public bool Muted {get {return MediaPlayer.IsMuted; } set {MediaPlayer.IsMuted = value;}}
        private SoundEffect buttonClick;
        private SoundEffect jump;
        private SoundEffect land;
        private SoundEffect coinGrab;

        private Song fart;
        private Song star;
        private Song amogus;

        /// <summary>
        /// Loads SFX and music
        /// </summary>
        public void LoadContent()
        {
            buttonClick = content.Load<SoundEffect>("Music/button");
            jump = content.Load<SoundEffect>("Music/jump");
            land = content.Load<SoundEffect>("Music/jumpland");
            coinGrab = content.Load<SoundEffect>("Music/coin");
            fart = content.Load<Song>("Music/fart");
            star = content.Load<Song>("Music/boss");
            amogus = content.Load<Song>("Music/AMOGUS");
        }

        /// <summary>
        /// plays a sound effect with a given action
        /// </summary>
        /// <param name="action">action of the player</param>
        public void PlaySFX(string action)
        {
            if (Muted) return;
            switch (action.ToLower().Trim())
            {
                case "button":
                    buttonClick.Play(1f, 0, 0);
                    break;
                case "jump":
                    jump.Play(0.02f, 0, 0);
                    break;
                case "land":
                    land.Play(0.75f, 0, 0);
                    break;
                case "coin":
                    coinGrab.Play(0.75f, 0, 0);
                    break;
            }
        }

        /// <summary>
        /// plays music in the game
        /// </summary>
        /// <param name="section"></param>
        public void PlayMusic(string section)
        {
            switch(section.ToLower().Trim())
            {
                case "continue":
                    MediaPlayer.Resume();
                    break;
                case "pause":
                    MediaPlayer.Pause();
                    break;
                case "menu":
                    MediaPlayer.Stop();
                    MediaPlayer.Play(fart);
                    MediaPlayer.IsRepeating = true;
                    break;
                case "game":
                    MediaPlayer.Stop();
                    //MediaPlayer.Volume = 0.5f;
                    MediaPlayer.Play(star);
                    MediaPlayer.IsRepeating = true;
                    break;
                case "gameover":
                    MediaPlayer.Stop();
                    MediaPlayer.Play(amogus);
                    MediaPlayer.IsRepeating = true;
                    break;

            }
        }
    }
}
