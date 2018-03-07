using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SosEngine
{

    /// <summary>
    /// Font based on a bitmap.
    /// </summary>
    public class BitmapFont
    {

        public string AssetName { get; set; }

        private SpriteFrameCache sfcChars;

        public int CharWidth
        {
            get { return charWidth; }
        }
        private int charWidth;

        public int CharHeight
        {
            get { return charHeight; }
        }
        private int charHeight;
        private string chars;

        public int CharSpacing { get; set; }

        /// <summary>
        /// Creates a new bitmap font.
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="charWidth">Width in pixels of characters</param>
        /// <param name="charHeight">Height in pixels of characters</param>
        /// <param name="chars">Characters in the order they appear in the bitmap</param>
        public BitmapFont(string assetName, int charWidth, int charHeight, string chars)
        {
            this.AssetName = assetName;
            this.charWidth = charWidth;
            this.charHeight = charHeight;
            this.chars = chars;
            this.sfcChars = new SpriteFrameCache();
            this.CharSpacing = charWidth;

            Texture2D texture = SosEngine.Core.GetTexture(assetName);

            // Create sprite frame for each character
            int charsH = texture.Width / charWidth;
            int charsV = texture.Height / charHeight;
            int charIndex = 0;
            for (int y = 0; y < charsV; y++)
            {
                for (int x = 0; x < charsH; x++)
                {
                    sfcChars.AddSpriteFrame(new SpriteFrame(assetName, "char_" + charIndex.ToString(), new Rectangle(x * charWidth, y * charHeight, charWidth, charHeight)));
                    charIndex++;
                }
            }

        }

        /// <summary>
        /// Prints specified char at specified location.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void PutChar(char c, int x, int y)
        {
            int charIndex = chars.IndexOf(c);
            SpriteFrame spriteFrame = sfcChars.GetSpriteFrame("char_" + charIndex.ToString());
            SosEngine.Core.SpriteBatch.Draw(Core.GetTexture(spriteFrame.AssetName), new Vector2(x, y), spriteFrame.Rectangle, Color.White);
        }

        /// <summary>
        /// Prints specified string at specified location.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Print(string s, int x, int y)
        {
            for (int i = 0; i < s.Length; i++)
            {
                PutChar(s[i], x + (i * CharSpacing), y);
            }
        }

        /// <summary>
        /// Prints specified string centered at specified location.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="centerX"></param>
        /// <param name="y"></param>
        public void PrintCenter(string s, int centerX, int y)
        {
            Print(s, centerX - (s.Length * CharSpacing) / 2, y);
        }

        /// <summary>
        /// Prints string to fit withint specified box
        /// </summary>
        /// <param name="s"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="maxChars"></param>
        public void PrintBox(string s, int x, int y, int w, int h, int maxChars = 500)
        {
            if (maxChars > s.Length)
            {
                maxChars = s.Length;
            }

            int charsPerRow = w / CharSpacing;
            
            string stringLeft = s.Substring(0, maxChars);
            string stringRight = "";
            for (int i = maxChars; i < s.Length; i++)
            {
                if (s[i] != ' ')
                {
                    stringRight += "½";
                }
                else
                {
                    stringRight += " ";
                }
            }

            string[] words = (stringLeft + stringRight).Split(' ');
            string row = "";
            for (int i = 0; i < words.Length; i++)
            {
                if ((row + " " + words[i]).Trim().Length <= charsPerRow)
                {
                    row = (row + " " + words[i]).Trim();
                }
                else
                {
                    Print(row.Replace('½', ' '), x, y);
                    y = y + charHeight;
                    row = words[i];
                }
            }
            if (row != "")
            {
                Print(row.Replace('½', ' '), x, y);
            }
        }
                
    }
}
