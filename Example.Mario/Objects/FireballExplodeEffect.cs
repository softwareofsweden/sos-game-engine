using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class FireballExplodeEffect : BaseEntity
    {

        public FireballExplodeEffect(Game game, int x, int y) :
            base(game, "fireball_explode_1", x, y, Vector2.Zero, null, 80)
        {
            // SosEngine.Core.PlaySound("Coin");
            this.IsRepeatable = false;
            AddFrame("fireball_explode_2", 80);
            AddFrame("fireball_explode_3", 80);
        }

        protected override Rectangle GetBoundingBox()
        {
            return Rectangle.Empty;
        }

        public override void Update(GameTime gameTime)
        {
            if (CompletedAFullCycle)
            {
                IsFinished = true;
            }
            base.Update(gameTime);
        }

    }
}
