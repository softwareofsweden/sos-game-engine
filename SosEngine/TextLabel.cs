using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SosEngine
{

    /// <summary>
    /// Label for printing text.
    /// </summary>
    public class TextLabel : Sprite
    {

        /// <summary>
        /// Label text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Specifies if text should be centered.
        /// </summary>
        public bool Centered { get; set; }

        /// <summary>
        /// Specified if text should flash.
        /// </summary>
        public bool Flash { 
            get 
            {
                return flash;
            }
            set
            {
                flash = value;
                flashCounter = 128;
            } 
        }

        protected bool flash;
        protected int flashCounter;

        /// <summary>
        /// Creates a text label at specified position.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public TextLabel(Game game, string text, float x, float y)
            : base(game, "")
        {
            Position = new Vector2(x, y);
            Text = text;
            Color = Color.Black;
        }

        /// <summary>
        /// Update text label.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (flash)
            {
                flashCounter = flashCounter - 4;
                if (flashCounter <= 0)
                {
                    flashCounter = 128;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw text label.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
            {
                return;
            }

            Color color;
            if (flash)
            {
                // 64 --> 0 --> 64
                int v = (Math.Abs(flashCounter-64)*2)+90;
                color = new Color(v, v, v);
            }
            else
            {
                color = Color;
            }

            if (Centered)
            {
                Core.PrintCenter(SosEngine.Core.SpriteBatch, Text, Position.X, Position.Y, color);
            }
            else
            {
                Core.Print(SosEngine.Core.SpriteBatch, Text, Position.X, Position.Y, color);
            }
        }

    }
}
