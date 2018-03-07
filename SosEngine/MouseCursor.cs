using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SosEngine
{
    public class MouseCursor : Sprite
    {
        private SpriteFrame spriteFrameNormal;
        private SpriteFrame spriteFrameClick;
        private bool currentButtonState;

        public MouseCursor(Game game, string spriteFrameNameNormal, string spriteFrameNameClick)
            : base(game, spriteFrameNameNormal)
        {
            spriteFrameNormal = Core.GetSpriteFrame(spriteFrameNameNormal);
            spriteFrameClick = Core.GetSpriteFrame(spriteFrameNameClick);
        }

        public void SetState(bool buttonPressed)
        {
            if (currentButtonState != buttonPressed)
            {
                currentButtonState = buttonPressed;
                spriteFrame = buttonPressed ? spriteFrameClick : spriteFrameNormal;
            }
        }

    }
}
