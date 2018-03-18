using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SosEngine
{

    /// <summary>
    /// Core
    /// </summary>
    public static class Core
    {
        /// <summary>
        /// Allow moving objects around using mouse when enabled.
        /// </summary>
        public static bool Debug = false;

        /// <summary>
        /// Draw bounding box around sprites when enabled.
        /// </summary>
        public static bool DebugSpriteBorders = false;

        private static List<SoundEffect> soundEffectCache;
        private static List<Texture2D> texture2DCache;
        private static List<Effect> shaderCache;
        private static List<BitmapFont> bitmapFontCache;
        private static Game game;
        private static Random random;
        private static Input input;
        private static SpriteFont spriteFont;
        private static Drawing2D drawing2D;
        private static List<string> logHistory;

        public static SpriteBatch SpriteBatch;
        public static SceneManager SceneManager;
        public static SaveDataManager SaveDataManager;
        public static ModPlayer ModPlayer;
        public static SpriteFrameCache SpriteFrameCache;

        public static int RenderWidth;
        public static int RenderHeight;
        public static bool UseScanlines;

        /// <summary>
        /// Number of connected game controllers.
        /// </summary>
        public static int NumberOfJoysticks
        {
            get { return input.NumberOfJoysticks; }
        }

        public static int HorizontalRenderCenter
        {
            get { return RenderWidth / 2; }
        }

        /// <summary>
        /// Init the game engine. This should be the first thing to call in 
        /// the LoadContent method.
        /// </summary>
        /// <param name="_game"></param>
        public static void Init(Game _game, int _renderWith, int _renderHeight, bool _useScanlines)
        {
            game = _game;
            RenderWidth = _renderWith;
            RenderHeight = _renderHeight;

            UseScanlines = _useScanlines;
            random = new Random(DateTime.Now.TimeOfDay.Milliseconds);
            input = new Input();
            soundEffectCache = new List<SoundEffect>();
            texture2DCache = new List<Texture2D>();
            shaderCache = new List<Effect>();
            bitmapFontCache = new List<BitmapFont>();
            SpriteBatch = new SpriteBatch(game.GraphicsDevice); 
            SceneManager = new SceneManager(game);
            SaveDataManager = new SaveDataManager(); 
            SosEngine.Core.ModPlayer = new SosEngine.ModPlayer(); 
            SpriteFrameCache = new SpriteFrameCache();
            drawing2D = new Drawing2D(game.GraphicsDevice);
            Mouse.WindowHandle = game.Window.Handle;
            game.IsMouseVisible = Debug;
            logHistory = new List<string>();
        }

        /// <summary>
        /// Call this in the UnloadContent method.
        /// </summary>
        public static void Shutdown()
        {
            SosEngine.Core.ModPlayer.Dispose();
            SosEngine.Core.ModPlayer = null;
            input.Dispose();
        }

        /// <summary>
        /// Write log message.
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            logHistory.Add(message);
            System.Diagnostics.Debug.WriteLine("SosEngine: " + message);
        }

        public static string[] FetchLog(int maxRows)
        {
            var count = logHistory.Count;
            if (maxRows > count)
            {
                return logHistory.ToArray();
            }
            return logHistory.GetRange(count - maxRows, maxRows).ToArray();
        }

        /// <summary>
        /// Get random int between min and max.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Random(int min, int max)
        {
            return random.Next(min, max + 1);
        }

        public static void SaveScreenshot()
        {
            /*
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar;
            int w = game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int h = game.GraphicsDevice.PresentationParameters.BackBufferHeight;
            int[] backBuffer = new int[w * h];
            game.GraphicsDevice.GetBackBufferData(backBuffer);
            Texture2D texture = new Texture2D(game.GraphicsDevice, w, h, false, game.GraphicsDevice.PresentationParameters.BackBufferFormat);
            texture.SetData(backBuffer);
            string fileNamePattern = desktopPath + "Screenshot_" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "_{0}.png";
            int nbr = 1;
            string fileName = string.Format(fileNamePattern, StringHelper.GetScoreString(nbr, 3));
            while (File.Exists(fileName))
            {
                nbr++;
                fileName = string.Format(fileNamePattern, StringHelper.GetScoreString(nbr, 3));
            }
            Core.Log("Saving screenshot to " + fileName);
            Stream stream = File.OpenWrite(fileName);
            texture.SaveAsPng(stream, w, h);
            stream.Close();
            */
        }

        public static void FillRectangle(Rectangle rect, Color color)
        {
            drawing2D.FillRectangle(SpriteBatch, rect, color);
        }

        public static void DrawRectangle(Rectangle rect, Color color)
        {
            drawing2D.DrawRectangle(SpriteBatch, rect, color, 1.0f);
        }

        public static void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            drawing2D.DrawLine(SpriteBatch, new Vector2(x1, y1), new Vector2(x2, y2), color, 1.0f);
        }

        /// <summary>
        /// Check joystick state.
        /// </summary>
        public static void UpdateInput()
        {
            input.Update();
        }

        public static bool IsPlayerInputDown(int controllerIndex, SosEngine.Input.PlayerInput playerInput)
        {
            return input.IsInput(0, playerInput);
        }

        public static bool IsPlayerInputPushed(int controllerIndex, SosEngine.Input.PlayerInput playerInput)
        {
            return input.IsInputPushed(0, playerInput);
        }

        /// <summary>
        /// Check which button that is pressed for specified joystick.
        /// </summary>
        /// <param name="controllerIndex"></param>
        /// <returns></returns>
        public static int GetPushedJoystickButton(int controllerIndex)
        {
            return input.GetPushedButton(controllerIndex);
        }

        /// <summary>
        /// Check if button is being pressed for specified joystick.
        /// </summary>
        /// <param name="controllerIndex"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool JoystickButtonDown(int controllerIndex, int button)
        {
            return input.JoystickButtonDown(controllerIndex, button);
        }

        /// <summary>
        /// Check if button was pressed and then released for specified joystick.
        /// </summary>
        /// <param name="controllerIndex"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool JoystickButtonPushed(int controllerIndex, int button)
        {
            return input.JoystickButtonPushed(controllerIndex, button);
        }

        /// <summary>
        /// Load Texture2D from content.
        /// </summary>
        /// <param name="assetName"></param>
        public static void LoadTexture(string assetName)
        {
            Texture2D texture2D = game.Content.Load<Texture2D>("Gfx\\" + assetName);
            texture2D.Name = assetName;
            texture2DCache.Add(texture2D);
        }

        /// <summary>
        /// Load Texture2D from content without caching it.
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static Texture2D LoadTextureWithoutCache(string assetName)
        {
            Texture2D texture2D = game.Content.Load<Texture2D>("Gfx\\" + assetName);
            texture2D.Name = assetName;
            return texture2D;
        }

        /// <summary>
        /// Get cached Texture2D by name.
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static Texture2D GetTexture(string assetName)
        {
            return texture2DCache.Find(x => x.Name == assetName);
        }

        /// <summary>
        /// Get cached SpriteFrame by name.
        /// </summary>
        /// <param name="frameName"></param>
        /// <returns></returns>
        public static SpriteFrame GetSpriteFrame(string frameName)
        {
            return SpriteFrameCache.GetSpriteFrame(frameName);
        }

        public static void LoadSpritesheet(string assetName, int w, int h, int count)
        {
            Core.LoadTexture(assetName);
            for (int i = 0; i < count; i++)
            {
                Core.SpriteFrameCache.AddSpriteFrame(new SpriteFrame(assetName, assetName + "_" + i.ToString(), new Rectangle(w * i, 0, w, h)));
            }
        }

        /// <summary>
        /// Load Effect from content.
        /// </summary>
        /// <param name="assetName"></param>
        public static void LoadShader(string assetName)
        {
            Effect effect = game.Content.Load<Effect>("Shader\\" + assetName);
            effect.Name = assetName;
            shaderCache.Add(effect);
        }

        /// <summary>
        /// Get cached Effect by name.
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static Effect GetShader(string assetName)
        {
            return shaderCache.Find(x => x.Name == assetName);
        }

        /// <summary>
        /// Load SoundEffect from content.
        /// </summary>
        /// <param name="assetName"></param>
        public static void LoadSoundEffect(string assetName)
        {
            SoundEffect soundEffect = game.Content.Load<SoundEffect>("Sound\\" + assetName);
            soundEffect.Name = assetName;
            soundEffectCache.Add(soundEffect);
        }

        /// <summary>
        /// Play sound effect.
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="volume">0.0f to 1.0f</param>
        /// <param name="pitch">-1.0f to 1.0f</param>
        /// <param name="pan">-1.0f to 1.0f</param>
        public static void PlaySound(string assetName, float volume = 1.0f, float pitch = 0f, float pan = 0f)
        {
            if (pitch > 1.0f)
            {
                pitch = 1.0f;
            }
            else if (pitch < -1.0f)
            {
                pitch = -1.0f;
            }

            SoundEffect soundEffect = soundEffectCache.Find(x => x.Name == assetName);
            soundEffect.Play(volume, pitch, pan);
        }

        /// <summary>
        /// Load BitmapFont from content.
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="charWidth"></param>
        /// <param name="charHeight"></param>
        /// <param name="chars"></param>
        public static void LoadBitmapFont(string assetName, int charWidth, int charHeight, string chars)
        {
            bitmapFontCache.Add(new BitmapFont(assetName, charWidth, charHeight, chars));
        }

        /// <summary>
        /// Get cached BitmapFont by name.
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static BitmapFont GetBitmapFont(string assetName)
        {
            return bitmapFontCache.Find(x => x.AssetName == assetName);
        }

        /// <summary>
        /// Load SpriteFont from content.
        /// </summary>
        /// <param name="assetName"></param>
        public static void LoadSpriteFont(string assetName)
        {
            spriteFont = game.Content.Load<SpriteFont>(assetName);
        }

        /// <summary>
        /// Print text at specified position.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Print(SpriteBatch spriteBatch, string text, float x, float y, Color color = default(Color))
        {
            spriteBatch.DrawString(spriteFont, text, new Vector2(x, y), color);
        }

        /// <summary>
        /// Print text centered at specified position.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void PrintCenter(SpriteBatch spriteBatch, string text, float x, float y, Color color = default(Color))
        {
            string[] rows = text.Split('\n');
            float yPos = y;
            foreach (string row in rows)
            {
                Vector2 size = spriteFont.MeasureString(row);
                Vector2 rowPos = new Vector2((float)Math.Round(x - (size.X / 2f)), yPos);
                yPos += 12;
                spriteBatch.DrawString(spriteFont, row, rowPos, color);
            }
        }

    }
}
