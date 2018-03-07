using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mario.Objects
{
    /// <summary>
    /// Heads up display for displaying room, lives, and score
    /// </summary>
    public class Hud : SosEngine.Sprite
    {
        public int Room { get; set; }
        public int Lives { get; set; }
        public int Score { get; set; }

        /// <summary>
        /// Display FPS
        /// </summary>
        public bool ShowFps { get; set; }

        /// <summary>
        /// Show info about keys for debugging
        /// </summary>
        public bool ShowSpecialKeys { get; set; }

        private SosEngine.BitmapFont font;

        /// <summary>
        /// Creates a new Hud component
        /// </summary>
        /// <param name="game"></param>
        public Hud(Game game) : base (game, null)
        {
            font = SosEngine.Core.GetBitmapFont("font");
        }

        /// <summary>
        /// Updates the info to be displayed in the hud
        /// </summary>
        /// <param name="room">Current room</param>
        /// <param name="lives">Player Lives</param>
        /// <param name="score">Player Score</param>
        public void SetData(int room, int lives, int score)
        {
            this.Room = room;
            this.Lives = lives;
            this.Score = score;
        }

        public override void Draw(GameTime gameTime)
        {
            if (ShowFps)
            {
                font.Print("FPS:" + SosEngine.Core.SceneManager.FPS.ToString(), 0, 0);
            }
            if (ShowSpecialKeys)
            {
                //font.PrintCenter("F1=BBOX F2=NEXTLEV", 160, 0);
                //font.PrintCenter("F2=NEXTLEV", 160, 0);
            }
            string hud = string.Format("{0}  LIVES {1}  LEVEL {2}",
                SosEngine.StringHelper.GetScoreString(Score, 6),
                SosEngine.StringHelper.GetScoreString(Lives, 2),
                SosEngine.StringHelper.GetScoreString(Room, 2));
            font.PrintCenter(hud, SosEngine.Core.RenderWidth / 2, 2);
        }
    }
}
