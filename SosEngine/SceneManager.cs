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
using System.Reflection;
using System.Globalization;

namespace SosEngine
{

    /// <summary>
    /// Manages game scenes and transitions between them.
    /// </summary>
    public class SceneManager : IConsoleCommand
    {

        /// <summary>
        /// Transition effect to use when switching scene.
        /// </summary>
        public enum TransitionEffect
        {
            None,
            Fade,
            TileExplode
        }

        public int FPS
        {
            get { return (int)Math.Round(frameRate); }
        }
        float frameRate;

        /// <summary>
        /// Current scene used in game.
        /// </summary>
        public GameScene CurrentScene;

        /// <summary>
        /// Next scene to be activated.
        /// </summary>
        protected GameScene NextScene;

        /// <summary>
        /// The game.
        /// </summary>
        protected Game game;

        /// <summary>
        /// The transition effect to be used when switching to the next scene.
        /// </summary>
        protected TransitionEffect transitionEffect;

        /// <summary>
        /// Render target for current scene used for transition.
        /// </summary>
        protected RenderTarget2D surfaceCurrentScene;

        /// <summary>
        /// Render target for next scene used for transition.
        /// </summary>
        protected RenderTarget2D surfaceNextScene;

        /// <summary>
        /// Duration in milliseconds for transition.
        /// </summary>
        protected int transitionDuration;

        /// <summary>
        /// Time in milliseconds since transition started.
        /// </summary>
        protected int transitionCount;

        protected static int TilesH = 16;
        protected static int TilesV = 10;
        protected Vector4[,] tile;

        protected Stack<GameScene> sceneStack;

        protected bool disposePreviousScene;

        protected bool oldConsoleToggleKeyDown;
        protected ConsoleWindow consoleWindow;

        /// <summary>
        /// Creates a new scene manager.
        /// </summary>
        /// <param name="game"></param>
        public SceneManager(Game game)
        {
            this.sceneStack = new Stack<GameScene>();
            this.game = game;
            surfaceCurrentScene = new RenderTarget2D(game.GraphicsDevice, game.GraphicsDevice.Viewport.Bounds.Width, game.GraphicsDevice.Viewport.Bounds.Height);
            surfaceNextScene = new RenderTarget2D(game.GraphicsDevice, game.GraphicsDevice.Viewport.Bounds.Width, game.GraphicsDevice.Viewport.Bounds.Height);
            consoleWindow = new ConsoleWindow(game, this);
        }

        protected bool IsConsoleToggleKeyDown()
        {
            return Keyboard.GetState().IsKeyDown(Keys.OemPipe);
        }

        /// <summary>
        /// Draw game scene to specified render target.
        /// </summary>
        /// <param name="gameScene"></param>
        /// <param name="surface"></param>
        public void DrawSceneToSurface(GameScene gameScene, RenderTarget2D surface)
        {
            gameScene.BeforeDraw();
            gameScene.Draw(new GameTime(), surface);
            gameScene.AfterDraw(surface);
            game.GraphicsDevice.SetRenderTarget(null);
        }

        private void DebugStack()
        {
            SosEngine.Core.Log("Scenes:");
            foreach (var scene in sceneStack)
            {
                SosEngine.Core.Log("  " + scene.ToString());
            }
        }

        public void PushScene(GameScene gameScene, TransitionEffect transitionEffect = TransitionEffect.None, int duration = 0)
        {
            if (CurrentScene != null)
            {
                sceneStack.Push(CurrentScene);
            }
            sceneStack.Push(gameScene);
            SwitchScene(gameScene, transitionEffect, duration, false);
            DebugStack();
        }

        public void PopScene(TransitionEffect transitionEffect = TransitionEffect.None, int duration = 0)
        {
            sceneStack.Pop();
            SwitchScene(sceneStack.Pop(), transitionEffect, duration, true);
            DebugStack();
        }

        /// <summary>
        /// Switch scene.
        /// </summary>
        /// <param name="gameScene">New game scene</param>
        /// <param name="transitionEffect">Transition effect to use when switching scene</param>
        /// <param name="duration">Time in milliseconds for transition</param>
        public void SwitchScene(GameScene gameScene, TransitionEffect transitionEffect = TransitionEffect.None, int duration = 0, bool disposePreviousScene = true)
        {

            this.disposePreviousScene = disposePreviousScene;

            transitionCount = 0;

            if (duration == 0 || CurrentScene == null)
            {
                transitionEffect = TransitionEffect.None;
            }

            if (transitionEffect != TransitionEffect.None)
            {
                this.transitionEffect = transitionEffect;
                transitionDuration = duration;
                DrawSceneToSurface(CurrentScene, surfaceCurrentScene);
                DrawSceneToSurface(gameScene, surfaceNextScene);

                if (transitionEffect == TransitionEffect.TileExplode)
                {
                    int tileWidth = surfaceCurrentScene.Width / TilesH;
                    int tileHeight = surfaceCurrentScene.Height / TilesV;
                    tile = new Vector4[TilesH, TilesV];
                    for (int y = 0; y < TilesV; y++)
                    {
                        for (int x = 0; x < TilesH; x++)
                        {
                            tile[x, y] = new Vector4(x * tileWidth, y * tileHeight, SosEngine.Core.Random(-8, 8), -SosEngine.Core.Random(12, 20));
                        }
                    }
                }
            }

            if (CurrentScene != null)
            {
                if (disposePreviousScene)
                {
                    CurrentScene.Dispose();
                }
                CurrentScene = gameScene;
            }
            else
            {
                // Wait until updating to switch scene
                NextScene = gameScene;
            }
        }

        /// <summary>
        /// Update current scene or transition if active.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (transitionCount < transitionDuration)
            {
                // Show transition
                transitionCount += gameTime.ElapsedGameTime.Milliseconds;
                if (transitionCount > transitionDuration)
                {
                    transitionCount = 0;
                    transitionDuration = 0;
                }
            }
            else
            {
                // Check if we should switch to a new scene
                if (NextScene != null)
                {
                    if (CurrentScene != null && disposePreviousScene)
                    {
                        CurrentScene.Dispose();
                    }
                    CurrentScene = NextScene;
                    NextScene = null;
                }
                if (CurrentScene != null)
                {
                    // Update current scene
                    CurrentScene.Update(gameTime);
                }
            }

            var consoleToggleKeyDown = IsConsoleToggleKeyDown();
            if (consoleToggleKeyDown && !oldConsoleToggleKeyDown)
            {
                consoleWindow.Visible = !consoleWindow.Visible;
            }
            oldConsoleToggleKeyDown = consoleToggleKeyDown;

            consoleWindow.Update(gameTime);
        }

        /// <summary>
        /// Special drawing when in transition between scenes.
        /// </summary>
        protected void DrawTransition()
        {
            switch (transitionEffect)
            {
                case TransitionEffect.Fade:
                    DrawTransitionFade();
                    break;
                case TransitionEffect.TileExplode:
                    DrawTransitionTileExplode();
                    break;
            }
        }

        /// <summary>
        /// Fade out current scene and fade in next scene.
        /// </summary>
        protected void DrawTransitionFade()
        {
            game.GraphicsDevice.Clear(Color.Black);
            if (transitionCount < (transitionDuration / 2))
            {
                // Fade out current scene
                float progress = MathHelper.SmoothStep(1f, 0f, (float)transitionCount / (float)(transitionDuration / 2));
                SosEngine.Core.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                SosEngine.Core.SpriteBatch.Draw(surfaceCurrentScene, game.GraphicsDevice.Viewport.Bounds, new Color(progress, progress, progress, progress));
                SosEngine.Core.SpriteBatch.End();
            }
            else
            {
                // Fade in next scene
                float progress = MathHelper.SmoothStep(0f, 1f, (float)(transitionCount - (transitionDuration / 2)) / (float)(transitionDuration / 2));
                SosEngine.Core.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                SosEngine.Core.SpriteBatch.Draw(surfaceNextScene, game.GraphicsDevice.Viewport.Bounds, new Color(progress, progress, progress, progress));
                SosEngine.Core.SpriteBatch.End();
            }
        }

        /// <summary>
        /// Explode current scene into tiles.
        /// </summary>
        protected void DrawTransitionTileExplode()
        {
            SosEngine.Core.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            // Draw next scene
            SosEngine.Core.SpriteBatch.Draw(surfaceNextScene, game.GraphicsDevice.Viewport.Bounds, Color.White);

            int tileWidth = surfaceCurrentScene.Width / TilesH;
            int tileHeight = surfaceCurrentScene.Height / TilesV;

            // p = 0 --> 1
            float p = (float)transitionCount / (float)transitionDuration;
            float offset = p * surfaceCurrentScene.Width;

            // Draw Exploding tiles
            for (int y = 0; y < TilesV; y++)
            {
                for (int x = 0; x < TilesH; x++)
                {
                    float xPos = tile[x, y].X;
                    float yPos = tile[x, y].Y;
                    tile[x, y] = new Vector4(tile[x, y].X + tile[x, y].Z, tile[x, y].Y + tile[x, y].W, tile[x, y].Z, tile[x, y].W + 1f);
                    SosEngine.Core.SpriteBatch.Draw(surfaceCurrentScene, new Vector2(xPos, yPos), new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), Color.White);
                }
            }

            SosEngine.Core.SpriteBatch.End();
        }

        /// <summary>
        /// Draw current scene or transition if active.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            //System.Threading.Thread.Sleep(120);
            //CurrentScene.Update(gameTime);

            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (transitionCount < transitionDuration)
            {
                DrawTransition();
            }
            else
            {
                if (CurrentScene != null)
                {
                    CurrentScene.BeforeDraw();
                    CurrentScene.Draw(gameTime);
                    CurrentScene.AfterDraw(null);
                }
            }

            consoleWindow.Draw(gameTime);

        }

        public void ConsoleExecute(string command, params string[] args)
        {
            if (CurrentScene != null && CurrentScene is IConsoleCommand)
            {
                ((IConsoleCommand)CurrentScene).ConsoleExecute(command, args);
            }
        }

    }
}
