using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mario.Objects
{
    public class Editor : SosEngine.Sprite, SosEngine.IConsoleCommand
    {
        private static Color gridColor = Color.White * 0.5f;
        private static Color activeTileColor = Color.Navy * 0.5f;

        private SosEngine.BitmapFont font;
        private SosEngine.Level level;
        private SosEngine.MouseCursor mouseCursor;

        protected MouseState currentMouseState;
        protected MouseState lastMouseState;
        //private List<SosEngine.Sprite> highlighedSprites;
        //private SosEngine.Sprite draggedObject;
        private bool leftMouseButtonJustPressed;
        private bool dragging;
        private Vector2 mouseDelta;

        private int mouseBx;
        private int mouseBy;
          

        /// <summary>
        /// Check if left mouse button was pressed
        /// </summary>
        public bool LeftMouseButtonClicked
        {
            get { return leftMouseButtonClicked; }
        }
        private bool leftMouseButtonClicked;

        /// <summary>
        /// Check if right mouse button was pressed
        /// </summary>
        public bool RightMouseButtonClicked
        {
            get { return rightMouseButtonClicked; }
        }
        private bool rightMouseButtonClicked;

        /// <summary>
        /// Check if middle mouse button was pressed
        /// </summary>
        public bool MiddleMouseButtonClicked
        {
            get { return middleMouseButtonClicked; }
        }
        private bool middleMouseButtonClicked;

        /// <summary>
        /// X position of mouse
        /// </summary>
        public int MouseX
        {
            get { return mouseX; }
        }
        private int mouseX;
        private int lastMouseX;

        /// <summary>
        /// Y position of mouse
        /// </summary>
        public int MouseY
        {
            get { return mouseY; }
        }
        private int mouseY;
        private int lastMouseY;

        public Editor(Game game, SosEngine.Level level) : base (game, null)
        {
            this.font = SosEngine.Core.GetBitmapFont("font");
            this.level = level;
            this.mouseCursor = new SosEngine.MouseCursor(game, "mouse_pointer", "mouse_grab");
            this.Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Visible)
            {
                return;
            }

            currentMouseState = Mouse.GetState();
            mouseX = (int)(SosEngine.Core.RenderWidth / (float)Game.GraphicsDevice.Viewport.Bounds.Width * currentMouseState.X);
            mouseY = (int)(SosEngine.Core.RenderHeight / (float)Game.GraphicsDevice.Viewport.Bounds.Height * currentMouseState.Y);
            mouseCursor.Position = new Vector2(mouseX, mouseY);
            mouseCursor.SetState(currentMouseState.LeftButton == ButtonState.Pressed);

            leftMouseButtonClicked = currentMouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed;
            rightMouseButtonClicked = currentMouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed;
            middleMouseButtonClicked = currentMouseState.MiddleButton == ButtonState.Released && lastMouseState.MiddleButton == ButtonState.Pressed;

            leftMouseButtonJustPressed = currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released;
            dragging = currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Pressed;

            mouseDelta = new Vector2(mouseX, mouseY) - new Vector2(lastMouseX, lastMouseY);

            lastMouseX = mouseX;
            lastMouseY = mouseY;
            lastMouseState = currentMouseState;

            level.GetBlockAtPixel("Block", MouseX + 4, mouseY + 4, out mouseBx, out mouseBy);

            if (leftMouseButtonJustPressed)
            {
                level.PutBlock("Block", mouseBx, mouseBy, 64);
            }

            base.Update(gameTime);
        }

        protected void DrawGrid(int xSpacing, int ySpacing)
        {
            var xOffset = 16 + (level.GetScrollX() % 16);
            var numCols = SosEngine.Core.RenderWidth / xSpacing;
            var numRows = SosEngine.Core.RenderHeight / ySpacing;

            SosEngine.Core.FillRectangle(
                new Rectangle((mouseBx * xSpacing) + level.GetScrollX(), (mouseBy * ySpacing) + 1, xSpacing - 1, ySpacing - 1),
                activeTileColor
                );
            
            for (int y = 0; y < numRows; y++)
            {
                SosEngine.Core.DrawLine(0, y * ySpacing, SosEngine.Core.RenderWidth, y * ySpacing, gridColor);
            }
            for (int x = 0; x < numCols; x++)
            {
                SosEngine.Core.DrawLine((x * xSpacing) + xOffset, 0, (x * xSpacing) + xOffset, SosEngine.Core.RenderHeight, gridColor);
            }

            /*
            for (int y = 0; y < numRows; y++)
            {
                for (int x = 0; x < numCols; x++)
                {
                    var b = level.GetBlockAtPixel("Block", (x * xSpacing) + xOffset, y * ySpacing);
                    font.Print(b.ToString(), (x * xSpacing) + xOffset, y * ySpacing);
                }
            }
            */
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                DrawGrid(16, 16);
                mouseCursor.Draw(gameTime);
            }
        }

        public void ConsoleExecute(string command, params string[] args)
        {
            if (command == "editor")
            {
                Visible = !Visible;
            }
        }
    }
}
