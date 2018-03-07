using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{

    // PipeIn, Goto:[Sublevel]
    // PipeIn, Return:[NameOfPipeOut]
    // PipeOut, [A]

    public class Pipe : SosEngine.Sprite
    {

        public enum PipeTypes
        {
            Entrance,
            Exit
        }

        public enum PipeOrientation
        {
            Horizontal,
            Vertical
        }

        public enum PipeAction
        {
            None,
            Goto,
            Return,
        }

        public string Destination
        {
            get
            {
                if (destination.Contains(':'))
                {
                    return destination.Split(':')[1];
                }
                return destination;
            }
        }

        public PipeAction Action
        {
            get
            {
                if (destination.Contains(':'))
                {
                    var action = destination.Split(':')[0];
                    if (action.ToUpper() == "GOTO")
                    {
                        return PipeAction.Goto;
                    }
                    if (action.ToUpper() == "RETURN")
                    {
                        return PipeAction.Return;
                    }
                }
                return PipeAction.None;
            }
        }

        public PipeTypes PipeType { get; internal set; }
        public PipeOrientation Orientation { get; internal set; }

        protected SosEngine.Level level;
        protected Rectangle rect;
        protected string destination;

        public Pipe(Game game, Rectangle rect, SosEngine.Level level, PipeTypes pipeType, string destination) :
            base(game, "", 0, 0)
        {
            this.rect = rect;
            if (rect.Width > rect.Height)
            {
                this.Orientation = PipeOrientation.Horizontal;
                if (pipeType == PipeTypes.Entrance)
                {
                    this.rect.Inflate(-4, -4);
                    this.rect.Offset(0, -8);
                }
            } 
            else
            {
                this.Orientation = PipeOrientation.Vertical;
                if (pipeType == PipeTypes.Entrance)
                {
                    this.rect.Inflate(-4, -4);
                    this.rect.Offset(-4, 0);
                }
            }
            this.level = level;
            this.PipeType = pipeType;
            this.destination = destination;
        }

        protected override Rectangle GetBoundingBox()
        {
            DrawOffsetX = level.GetScrollX();
            return new Rectangle(rect.X + DrawOffsetX, rect.Y + DrawOffsetY, rect.Width, rect.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (SosEngine.Core.DebugSpriteBorders)
            {
                SosEngine.Core.DrawRectangle(GetBoundingBox(), Color.Red);
            }
        }

    }
}
