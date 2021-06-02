﻿using System;
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
        private SoundEffect shoot;
        private SoundEffect hit;

        private Song menu;
        private Song clouds;
        private Song cave;
        private Song desert;
        private Song gameOver;


        /// <summary>
        /// Loads SFX and music
        /// </summary>
        public void LoadContent()
        {
            buttonClick = content.Load<SoundEffect>("Music/button");
            jump = content.Load<SoundEffect>("Music/jump");
            land = content.Load<SoundEffect>("Music/jumpland");
            coinGrab = content.Load<SoundEffect>("Music/coin");
            menu = content.Load<Song>("Music/menu");

            clouds = content.Load <Song>("Music/clouds");
            cave = content.Load<Song>("Music/cave");
            gameOver = content.Load<Song>("Music/game over");
            desert = content.Load<Song>("Music/desert");
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
                    MediaPlayer.Play(menu);
                    MediaPlayer.Volume = 0.25f;
                    MediaPlayer.IsRepeating = true;
                    break;
                case "clouds":
                    MediaPlayer.Stop();
                    MediaPlayer.Play(clouds);
                    MediaPlayer.Volume = 0.25f;
                    MediaPlayer.IsRepeating = true;
                    break;
                case "desert":
                    MediaPlayer.Stop();
                    MediaPlayer.Play(desert);
                    MediaPlayer.Volume = 0.25f;
                    MediaPlayer.IsRepeating = true;
                    break;
                case "cave":
                    MediaPlayer.Stop();
                    MediaPlayer.Play(cave);
                    MediaPlayer.Volume = 0.25f;
                    MediaPlayer.IsRepeating = true;
                    break;
                case "gameover":
                    MediaPlayer.Stop();
                    MediaPlayer.Play(gameOver);
                    MediaPlayer.IsRepeating = true;
                    break;

            }
        }
    }
}
