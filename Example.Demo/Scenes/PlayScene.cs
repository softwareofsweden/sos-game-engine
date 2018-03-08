using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Example_Demo.MacOS.Objects;

namespace Example_Demo.MacOS.Scenes
{
    public class PlayScene : SosEngine.GameScene
    {

        /// <summary>
        /// The player.
        /// </summary>
        protected Player player;

        /// <summary>
        /// The level.
        /// </summary>
        private SosEngine.Level level;

        public PlayScene(Game game)
            : base(game)
        {

            // Create player with default sprite frame
            player = new Player(game, "down_0");
            // Center player on screen
            player.CenterOnScreen();

            // Create level and load tiles
            level = new SosEngine.Level(game, "test", 0, 0);

            // Add components (in correct draw order)
            AddGameComponent(level);
            AddGameComponent(player);

            /*
            var editor = new SosEngine.Editor(game, level);
            editor.Visible = true;
            AddGameComponent(editor);
            */

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void Update(GameTime gameTime)
        {
            if (KeyPushed(Keys.Escape))
            {
                Game.Exit();
            }

            var ctrlLeft = SosEngine.Core.IsPlayerInputDown(0, SosEngine.Input.PlayerInput.Left) || currentKeyboardState.IsKeyDown(Keys.Left);
            var ctrlRight = SosEngine.Core.IsPlayerInputDown(0, SosEngine.Input.PlayerInput.Right) || currentKeyboardState.IsKeyDown(Keys.Right);
            var ctrlUp = SosEngine.Core.IsPlayerInputDown(0, SosEngine.Input.PlayerInput.Up) || currentKeyboardState.IsKeyDown(Keys.Up);
            var ctrlDown = SosEngine.Core.IsPlayerInputDown(0, SosEngine.Input.PlayerInput.Down) || currentKeyboardState.IsKeyDown(Keys.Down);
            var ctrlA = SosEngine.Core.IsPlayerInputDown(0, SosEngine.Input.PlayerInput.A) || currentKeyboardState.IsKeyDown(Keys.LeftShift);
            var ctrlB = SosEngine.Core.IsPlayerInputDown(0, SosEngine.Input.PlayerInput.B) || currentKeyboardState.IsKeyDown(Keys.Space);

            player.SetControls(ctrlLeft, ctrlRight, ctrlUp, ctrlDown);

            // Change speed depending on what type of ground player is walking on
            var block = level.GetBlockAtPixel("Block", (int)Math.Round(player.Position.X), (int)Math.Round(player.Position.Y));
            if (block == 15 || block == 16 || block == 35 || block == 36) {
                player.Speed = 2;
            } else {
                player.Speed = 1;
            }


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, null);
        }

        public override void Draw(GameTime gameTime, RenderTarget2D destinationRenderTarget2D)
        {
            base.Draw(gameTime);
        }

    }
}
