using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mario
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TestGame : Game
    {
        GraphicsDeviceManager graphics;

        public TestGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280; // 320 * 4
            graphics.PreferredBackBufferHeight = 960; // 240 * 4
            graphics.IsFullScreen = false;

            //graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;

            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // SosEngine.Core.Debug = true;
            // SosEngine.Core.DebugSpriteBorders = true;

            SosEngine.Core.Init(this, 320, 240, false); // Init Game Engine
            Helpers.ContentHelper.LoadAllContent();
            //SosEngine.Core.GetShader("Scanlines").Parameters["ImageHeight"].SetValue(graphics.GraphicsDevice.Viewport.Height);
            // SosEngine.Core.SceneManager.SwitchScene(new Scenes.MenuScene(this));

            SosEngine.Core.SceneManager.SwitchScene(new Scenes.PlayScene(this, "Level01"));
            SosEngine.Core.ModPlayer.PlayMusic("Mario.Music.wk_maria.xm");
        }

        protected override void UnloadContent()
        {
            SosEngine.Core.Shutdown();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            //System.Threading.Thread.Sleep(20);

            SosEngine.Core.SceneManager.Update(gameTime);
            base.Update(gameTime);


            SosEngine.Core.SceneManager.Draw(gameTime);
            base.Draw(gameTime);
        }

    }
}
