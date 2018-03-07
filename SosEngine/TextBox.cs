using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SosEngine
{
        
    /// <summary>
    /// Displays a textbox with multiple pages using specified font.
    /// </summary>
    public class TextBox : Sprite
    {

        private BitmapFont font; 
        private int pageIndex;
        private List<string> pages;
        private int currentLength;
        private int x;
        private int y;
        private int w;
        private int h;
        private int charDelay;
        private int waitTime;
        private int borderSize;
        private int padding;
        private Color backgroundColor;
        private Color borderColor;
        private bool showNextPageIndicator;
        private bool isClosed;

        /// <summary>
        /// Creates a TextBox.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="font"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="charDelay"></param>
        /// <param name="borderSize"></param>
        /// <param name="padding"></param>
        public TextBox(Game game, BitmapFont font, int x, int y, int w, int h, Color backgroundColor, Color borderColor, int charDelay = 30, int borderSize = 2, int padding = 2, float alpha = 1.0f)
            : base(game, null)
        {
            this.font = font;
            this.pageIndex = 0;
            this.currentLength = 1;
            this.pages = new List<string>();
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.alpha = alpha;
            this.backgroundColor = backgroundColor * alpha;
            this.borderColor = borderColor * alpha;
            this.borderSize = borderSize;
            this.padding = padding;
            this.charDelay = charDelay;
            this.waitTime = 0;
            this.showNextPageIndicator = false;
            this.isClosed = false;
        }

        /// <summary>
        /// Adds page.
        /// </summary>
        /// <param name="content"></param>
        public void AddPage(string content)
        {
            pages.Add(content);
        }

        /// <summary>
        /// Goto next page. Closes the textbox after last page.
        /// </summary>
        public void NextPage()
        {
            if (pageIndex < pages.Count - 1)
            {
                pageIndex++;
                currentLength = 1;
                waitTime = 0;
                showNextPageIndicator = false;
            }
            else
            {
                isClosed = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            waitTime += gameTime.ElapsedGameTime.Milliseconds;
            if (waitTime > charDelay)
            {
                waitTime = 0;
                currentLength++;
                if (currentLength > pages[pageIndex].Length)
                {
                    currentLength = pages[pageIndex].Length;
                    if (pageIndex < pages.Count - 1)
                    {
                        showNextPageIndicator = true;
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!isClosed)
            {
                SosEngine.Core.FillRectangle(new Rectangle(x - borderSize - padding, y - borderSize - padding, w + borderSize * 2 + padding * 2, h + borderSize * 2 + padding * 2), borderColor);
                SosEngine.Core.FillRectangle(new Rectangle(x - padding, y - padding, w + padding * 2, h + padding * 2), backgroundColor);
                font.PrintBox(pages[pageIndex], x, y, w, h, currentLength);
                if (showNextPageIndicator)
                {
                    font.PrintCenter("...", x + w / 2, y + h - font.CharHeight);
                }
                base.Draw(gameTime);
            }
        }

    }


}
