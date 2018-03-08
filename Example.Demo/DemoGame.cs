using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Example_Demo.MacOS
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DemoGame : Game
    {
        GraphicsDeviceManager graphics;

        public DemoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 960; // 320 * 3
            graphics.PreferredBackBufferHeight = 720; // 240 * 3
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Init the engine

            //SosEngine.Core.Debug = true;
            //SosEngine.Core.DebugSpriteBorders = true;
            SosEngine.Core.Init(this, 320, 240, false); // Init Game Engine

            // Load content

            SosEngine.Core.LoadTexture("terrainTiles_default");
            SosEngine.Core.LoadTexture("george"); // 4 x 4 (48x48 pixels)
            SosEngine.Core.LoadTexture("Editor");

            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "down_0", new Rectangle(48 * 0, 48 * 0, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "down_1", new Rectangle(48 * 0, 48 * 1, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "down_2", new Rectangle(48 * 0, 48 * 2, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "down_3", new Rectangle(48 * 0, 48 * 3, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "left_0", new Rectangle(48 * 1, 48 * 0, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "left_1", new Rectangle(48 * 1, 48 * 1, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "left_2", new Rectangle(48 * 1, 48 * 2, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "left_3", new Rectangle(48 * 1, 48 * 3, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "up_0", new Rectangle(48 * 2, 48 * 0, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "up_1", new Rectangle(48 * 2, 48 * 1, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "up_2", new Rectangle(48 * 2, 48 * 2, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "up_3", new Rectangle(48 * 2, 48 * 3, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "right_0", new Rectangle(48 * 3, 48 * 0, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "right_1", new Rectangle(48 * 3, 48 * 1, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "right_2", new Rectangle(48 * 3, 48 * 2, 48, 48)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("george", "right_3", new Rectangle(48 * 3, 48 * 3, 48, 48)));

            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Editor", "mouse_pointer", new Rectangle(0, 0, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Editor", "mouse_grab", new Rectangle(32, 0, 32, 32)));

            // Create start scene

            SosEngine.Core.SceneManager.SwitchScene(new Scenes.PlayScene(this));

        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            SosEngine.Core.SceneManager.Update(gameTime);
            base.Update(gameTime);
            SosEngine.Core.SceneManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
