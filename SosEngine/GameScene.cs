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
    /// Base class for game scenes.
    /// </summary>
    public abstract class GameScene : DrawableGameComponent, IConsoleCommand
    {
        private RenderTarget2D renderTarget2D;

        /// <summary>
        /// Color for clearing screen.
        /// </summary>
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
        protected Color backgroundColor;

        protected KeyboardState currentKeyboardState;
        protected KeyboardState lastKeyboardState;
        protected MouseState currentMouseState;
        protected MouseState lastMouseState;

        private List<Tasks.Task> tasks;

        private bool mouseEnabled;
        private MouseCursor mouseCursor;

        private List<SosEngine.Sprite> highlighedSprites;
        private SosEngine.Sprite draggedObject;
        private bool leftMouseButtonJustPressed;
        private bool dragging;
        private Vector2 mouseDelta;

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

        private Texture2D tiledBackground;
        private bool useTiledBackground;

        private Texture2D background;
        private bool useBackground;

        private List<IGameComponent> gameComponentsToRemove;

        /// <summary>
        /// List of game components. Draw and update will
        /// be automatically called for all components in
        /// this list
        /// </summary>
        protected GameComponentCollection gameComponents;

        public GameScene(Game game)
            : base(game)
        {
            renderTarget2D = new RenderTarget2D(game.GraphicsDevice, SosEngine.Core.RenderWidth, SosEngine.Core.RenderHeight);
            backgroundColor = Color.Black;
            gameComponents = new GameComponentCollection();
            gameComponentsToRemove = new List<IGameComponent>();
            tasks = new List<Tasks.Task>();
            highlighedSprites = new List<Sprite>();
        }

        public void AddGameComponent(GameComponent gameComponent)
        {
            gameComponents.Add(gameComponent);
        }

        /// <summary>
        /// Remove game component from scene.
        /// </summary>
        /// <param name="gameComponent"></param>
        public void RemoveGameComponent(IGameComponent gameComponent)
        {
            gameComponentsToRemove.Add(gameComponent);
        }

        /// <summary>
        /// Allow the game scene to update the task
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Tasks.Task task)
        {
            tasks.Add(task);
        }

        public bool AnyActiveTasks()
        {
            return tasks.Any(x => !x.IsFinished);
        }

        /// <summary>
        /// Displays a mouse cursor using specified sprite frames
        /// </summary>
        /// <param name="spriteFrameNameNormal"></param>
        /// <param name="spriteFrameNameClick"></param>
        public void EnableMouseCursor(string spriteFrameNameNormal, string spriteFrameNameClick)
        {
            mouseCursor = new MouseCursor(Game, spriteFrameNameNormal, spriteFrameNameClick);
            mouseEnabled = true;
        }

        /// <summary>
        /// Draws the specified SpriteFrame tiled as a background
        /// </summary>
        /// <param name="spriteFrame"></param>
        public void SetTiledBackground(SpriteFrame spriteFrame)
        {
            Texture2D sourceTexture = SosEngine.Core.GetTexture(spriteFrame.AssetName);
            tiledBackground = new Texture2D(Game.GraphicsDevice, spriteFrame.Rectangle.Width, spriteFrame.Rectangle.Height, false, sourceTexture.Format);
            Color[] data = new Color[spriteFrame.Rectangle.Width * spriteFrame.Rectangle.Height];
            sourceTexture.GetData(0, spriteFrame.Rectangle, data, 0, data.Length);
            tiledBackground.SetData(data);
            useTiledBackground = true;
        }

        /// <summary>
        /// Draws the specified Texture2D as a background
        /// </summary>
        /// <param name="texture2D"></param>
        public void SetBackground(Texture2D texture2D)
        {
            background = texture2D;
            useBackground = true;
        }

        protected bool KeyPushed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        protected bool AnyKeyPushed()
        {
            return currentKeyboardState.GetPressedKeys().Length > 0 && lastKeyboardState.GetPressedKeys().Length == 0;
        }

        protected bool AnyJoystickButtonPushed()
        {
            for (int i = 0; i < SosEngine.Core.NumberOfJoysticks; i++)
            {
                if (SosEngine.Core.GetPushedJoystickButton(i) > -1)
                {
                    return true;
                }
            }
            return false;
        }

        protected bool AnyKeyOrJoystickButtonPushed()
        {
            return AnyKeyPushed() || AnyJoystickButtonPushed();
        }

        public override void Update(GameTime gameTime)
        {
            Core.UpdateInput();

            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (mouseEnabled || SosEngine.Core.Debug)
            {
                currentMouseState = Mouse.GetState();
                mouseX = (int)(Core.RenderWidth / (float)Game.GraphicsDevice.Viewport.Bounds.Width * currentMouseState.X);
                mouseY = (int)(Core.RenderHeight / (float)Game.GraphicsDevice.Viewport.Bounds.Height * currentMouseState.Y);
                if (mouseEnabled)
                {
                    mouseCursor.Position = new Vector2(mouseX, mouseY);
                    mouseCursor.SetState(currentMouseState.LeftButton == ButtonState.Pressed);
                }

                leftMouseButtonClicked = currentMouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed;
                rightMouseButtonClicked = currentMouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed;
                middleMouseButtonClicked = currentMouseState.MiddleButton == ButtonState.Released && lastMouseState.MiddleButton == ButtonState.Pressed;

                leftMouseButtonJustPressed = currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released;
                dragging = currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Pressed;

                mouseDelta = new Vector2(mouseX, mouseY) - new Vector2(lastMouseX, lastMouseY);

                lastMouseX = mouseX;
                lastMouseY = mouseY;
                lastMouseState = currentMouseState;
            }

            // Update tasks
            bool anyUpdatedTask = false;
            for (int i = 0; i < tasks.Count; i++)
            {
                if (!tasks[i].IsFinished)
                {
                    tasks[i].Update(gameTime);
                    anyUpdatedTask = true;
                }
            }

            // Remove finished tasks
            if (anyUpdatedTask)
            {
                tasks.RemoveAll(task => task.IsFinished);
            }

            // Update game components
            foreach (GameComponent component in gameComponents)
            {
                component.Update(gameTime);
            }

            // Remove game components
            foreach (GameComponent component in gameComponentsToRemove)
            {
                gameComponents.Remove(component);
            }
            gameComponentsToRemove.Clear();

            if (SosEngine.Core.Debug)
            {
                highlighedSprites.Clear();
                foreach (GameComponent component in gameComponents)
                {
                    if (component is SosEngine.Sprite)
                    {
                        if (((SosEngine.Sprite)component).BoundingBoxWithOffset.Contains(mouseX, mouseY))
                        {
                            highlighedSprites.Add((SosEngine.Sprite)component);
                        }
                    }
                }
                if (leftMouseButtonJustPressed && highlighedSprites.Count > 0)
                {
                    draggedObject = highlighedSprites[0];
                }
                else if (dragging && draggedObject != null)
                {
                    draggedObject.Position += mouseDelta;
                    if (currentKeyboardState.IsKeyDown(Keys.LeftControl))
                    {
                        int snappedX = (int)Math.Round(draggedObject.Position.X) / 8 * 8;
                        int snappedY = (int)Math.Round(draggedObject.Position.Y) / 8 * 8;
                        draggedObject.Position = new Vector2(snappedX, snappedY);
                    }
                    SosEngine.Core.Log(string.Format("Position of object is X: {0} Y: {1}", draggedObject.Position.X, draggedObject.Position.Y));
                }
                else
                {
                    draggedObject = null;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw game components
            foreach (DrawableGameComponent component in gameComponents.OfType<DrawableGameComponent>())
            {
                component.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        public void BeforeDraw()
        {
            Game.GraphicsDevice.SetRenderTarget(renderTarget2D);
            Game.GraphicsDevice.Clear(backgroundColor);

            if (useTiledBackground)
            {
                SosEngine.Core.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);
                SosEngine.Core.SpriteBatch.Draw(tiledBackground, Vector2.Zero, Game.GraphicsDevice.Viewport.Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                SosEngine.Core.SpriteBatch.End();
            }

            if (useBackground)
            {
                SosEngine.Core.SpriteBatch.Begin();
                SosEngine.Core.SpriteBatch.Draw(background, Vector2.Zero, Color.White);
                SosEngine.Core.SpriteBatch.End();
            }

            SosEngine.Core.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
        }

        public void AfterDraw(RenderTarget2D destinationRenderTarget2D)
        {
            if (mouseEnabled)
            {
                mouseCursor.RenderWithColor(Color.White);
            }

            // Draw highlighted objects when debugging
            if (SosEngine.Core.Debug)
            {
                foreach (SosEngine.Sprite sprite in highlighedSprites)
                {
                    SosEngine.Core.DrawRectangle(sprite.BoundingBoxWithOffset, Color.Aqua);
                }
            }
            SosEngine.Core.SpriteBatch.End();

            Game.GraphicsDevice.SetRenderTarget(destinationRenderTarget2D);
            //
            // Stretch the render target to the screen
            //
            if (SosEngine.Core.UseScanlines)
            {
                SosEngine.Core.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, SosEngine.Core.GetShader("Scanlines"));
            }
            else
            {
                SosEngine.Core.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null);
            }
            SosEngine.Core.SpriteBatch.Draw(renderTarget2D, Game.GraphicsDevice.Viewport.Bounds, Color.White);
            SosEngine.Core.SpriteBatch.End();

            if (currentKeyboardState.IsKeyDown(Keys.PrintScreen) && lastKeyboardState.IsKeyUp(Keys.PrintScreen))
            {
                SosEngine.Core.SaveScreenshot();
            }
        }

        public abstract void Draw(GameTime gameTime, RenderTarget2D destinationRenderTarget2D);


        public void ConsoleExecute(string command, params string[] args)
        {
            foreach (var item in gameComponents)
            {
                if (item is IConsoleCommand)
                {
                    ((IConsoleCommand)item).ConsoleExecute(command, args);
                }
            }
        }

    }

}
