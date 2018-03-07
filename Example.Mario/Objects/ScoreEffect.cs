using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class ScoreEffect : BaseEntity
    {

        int displayCount;

        public ScoreEffect(Game game, int x, int y, int score) :
            base(game, null, x, y, new Vector2(0, -1), null, 0)
        {
            if (score == -2)
            {
                SetSpriteFrame("score_1up");
            } 
            else
            {
                SetSpriteFrame("score_" + score.ToString());
            }
            Position -= new Vector2(GetBoundingBox().Width / 2, 0);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            displayCount++;
            if (displayCount > 50)
            {
                IsFinished = true;
            }
            Position = Position + speed;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawOffsetX = 0;
            base.Draw(gameTime);
        }

        

    }
}
