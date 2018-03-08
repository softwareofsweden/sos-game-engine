using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mario.Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Mario.Scenes
{
    public class PlayScene : SosEngine.GameScene
    {
        protected SosEngine.BitmapFont font;
        private SosEngine.Level level;
        private Hud hud;
        private SosEngine.Editor editor;
        private EntityManager entityManager;
        private Player player;
        private Texture2D background;

        protected SosEngine.Replay replay;

        private string levelName;

        public Player Player
        {
            get { return player; }
        }

        public PlayScene(Game game, string levelName, PlayerStats playerStats = null, PlayScene mainScene = null)
            : base(game)
        {
            this.levelName = levelName;

            replay = new SosEngine.Replay("C:\\Temp\\mario.rec", SosEngine.Replay.Mode.Standby);

            BackgroundColor = new Color(0, 128, 240);
            font = SosEngine.Core.GetBitmapFont("font");

            //
            // Level
            //
            level = new SosEngine.Level(game, levelName, 0, 0);
            Mario.Helpers.LevelHelper.SetupTileAnimation(level);
            level.SetupAnimatedTiles("Block");
            level.HideLayer("Items");

            var backgroundImage = level.GetCustomProperty("BackgroundImage");
            if (backgroundImage != "")
            {
                background = SosEngine.Core.LoadTextureWithoutCache("bg_sky");
                SetBackground(background);
            }
            else
            {
                BackgroundColor = new Color(0, 0, 0);
            }

            var parallax2Objects = level.GetLevelObjects("Objects", "Parallax2");
            foreach (var levelObject in parallax2Objects)
            {
                level.AddParallaxSprite(new SosEngine.Sprite(game, levelObject.Name, levelObject.X, levelObject.Y), 4);
            }

            var parallax1Objects = level.GetLevelObjects("Objects", "Parallax1");
            foreach (var levelObject in parallax1Objects)
            {
                level.AddParallaxSprite(new SosEngine.Sprite(game, levelObject.Name, levelObject.X, levelObject.Y), 2);
            }

            //
            // Entities
            //
            entityManager = new EntityManager(game, level);

            //
            // Player
            //
            player = new Objects.Player(game, "mario_move_0", level);
            player.MainPlayScene = mainScene;
            player.CenterOnScreen();
            int playerX;
            int playerY;
            Helpers.LevelHelper.GetPlayerStartPosition(level, "Player", out playerX, out playerY);
            player.Position = new Vector2(playerX - 8, playerY);
            player.EntityManager = entityManager;
            if (playerStats != null)
            {
                player.Stats = playerStats;
            }

            //
            // Hud
            //
            hud = new Hud(game);
            hud.ShowFps = false;
            hud.ShowSpecialKeys = true;
            hud.SetData(1, 1, 100);

            //
            // Editor
            //
            editor = new SosEngine.Editor(game, level);
            editor.Visible = true;

            gameComponents.Add(level);
            gameComponents.Add(hud);
            gameComponents.Add(entityManager);
            gameComponents.Add(player);
            gameComponents.Add(editor);

            var pipeIns = level.GetLevelObjects("Objects", "PipeIn");
            foreach (var pipeIn in pipeIns)
            {
                var pipe = new Pipe(game, pipeIn.Bounds, level, Pipe.PipeTypes.Entrance, pipeIn.Name);
                AddGameComponent(pipe);
                player.AddPipe(pipe);
            }

            var pipeOuts = level.GetLevelObjects("Objects", "PipeOut");
            foreach (var pipeOut in pipeOuts)
            {
                var pipe = new Pipe(game, pipeOut.Bounds, level, Pipe.PipeTypes.Exit, pipeOut.Name);
                AddGameComponent(pipe);
                player.AddPipe(pipe);
            }
        }

        public override string ToString()
        {
            return base.ToString() + " [" + levelName + "]";
        }

        protected override void Dispose(bool disposing)
        {
            SosEngine.Core.Log(this.ToString() + " is being disposed!");
            replay.Save();
            base.Dispose(disposing);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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

            replay.Update(ref ctrlLeft, ref ctrlRight, ref ctrlUp, ref ctrlDown, ref ctrlA, ref ctrlB);
            player.SetControls(ctrlLeft, ctrlRight, ctrlUp, ctrlDown, ctrlA, ctrlB);
            entityManager.PlayerX = (int)Math.Round(player.Position.X);

            entityManager.SetDrawOffset(level.GetScrollX());

            hud.SetData(1, 1, player.Score);

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
