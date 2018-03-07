using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SosEngine
{
    public class ConsoleWindow : DrawableGameComponent
    {
        protected static bool debugKeys = false;

        protected Drawing2D drawing2D;
        protected Rectangle bounds;
        protected Color backgroundColor;
        protected int lineSpacing;
        protected int maxLinesToRender;
        protected int blinkCounter;
        protected bool cursorVisible;
        protected string inputBuffer;
        protected KeyboardState oldKeyboardState;
        protected SceneManager sceneManager;

        private static string validKeys = "abcdefghijklmnopqrstuvwxyz0123456789-+";

        public ConsoleWindow(Game game, SceneManager sceneManager)
            : base(game)
        {
            this.sceneManager = sceneManager;
            this.drawing2D = new Drawing2D(game.GraphicsDevice);
            this.bounds = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Bounds.Width, Game.GraphicsDevice.Viewport.Bounds.Height / 4);
            this.backgroundColor = Color.Black * 0.8f;
            this.lineSpacing = 18;
            this.maxLinesToRender = bounds.Height / lineSpacing;
            this.inputBuffer = "";
            this.Visible = false;
        }

        protected void ExecuteCommand()
        {
            if (inputBuffer.Trim() != "")
            {
                SosEngine.Core.Log(inputBuffer);
                if (inputBuffer.Trim() == "debug")
                {
                    SosEngine.Core.DebugSpriteBorders = !SosEngine.Core.DebugSpriteBorders;
                    SosEngine.Core.Log("Sprite borders are turned " + (SosEngine.Core.DebugSpriteBorders ? "on." : "off."));
                }
                var args = inputBuffer.Trim().Split(' ');
                sceneManager.ConsoleExecute(args[0], args.Skip(1).ToArray());
            }
            inputBuffer = "";
        }

        public override void Update(GameTime gameTime)
        {
            if (!Visible)
            {
                return;
            }

            var keyboardState = Keyboard.GetState();
            var keys = keyboardState.GetPressedKeys();
            var oldKeys = oldKeyboardState.GetPressedKeys();
            foreach (var key in keys)
            {
                if (!oldKeys.Contains(key))
                {
                    var strKey = key.ToString().ToLower();
                    if (debugKeys)
                    {
                        SosEngine.Core.Log("Keypress: " + strKey);
                    }
                    if (strKey.Length == 1)
                    {
                        if (validKeys.Contains(strKey[0]))
                        {
                            inputBuffer = inputBuffer + strKey;
                        }
                    }
                    else
                    {
                        if (strKey == "space")
                        {
                            inputBuffer = inputBuffer + " ";
                        }
                        if (strKey == "add")
                        {
                            inputBuffer = inputBuffer + "+";
                        }
                        if (strKey == "subtract")
                        {
                            inputBuffer = inputBuffer + "-";
                        }
                        if (strKey == "back")
                        {
                            if (inputBuffer.Length > 0)
                            {
                                inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1);
                            }
                        }
                        if (strKey == "enter")
                        {
                            ExecuteCommand();
                        }
                    }
                }
            }
            oldKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
            {
                return;
            }

            SosEngine.Core.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null);
            drawing2D.FillRectangle(SosEngine.Core.SpriteBatch, bounds, backgroundColor);

            var rows = SosEngine.Core.FetchLog(maxLinesToRender - 1);
            for (int i = 0; i < maxLinesToRender; i++)
            {
                if (rows.Length > i)
                {
                    SosEngine.Core.Print(SosEngine.Core.SpriteBatch, ">" + rows[i], 0, i * lineSpacing, Color.Lime);
                }
            }

            blinkCounter++;
            if (blinkCounter > 39)
            {
                blinkCounter = 0;
            }
            cursorVisible = blinkCounter >= 20;

            var inputY = (maxLinesToRender - 1) * lineSpacing;
            SosEngine.Core.Print(SosEngine.Core.SpriteBatch, ">" + inputBuffer + (cursorVisible ? "_" : ""), 0, inputY, Color.White);

            SosEngine.Core.SpriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
