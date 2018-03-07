using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Mario.Scenes
{
    public class MenuScene : PlayScene
    {

        public MenuScene(Game game)
            : base(game, "Level01")
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (System.IO.Stream stream = assembly.GetManifestResourceStream("Mario.Resources.mario.rec"))
            {
                replay = new SosEngine.Replay(stream);
            }

            BackgroundColor = new Color(0, 0, 80);

            SosEngine.Sprite logo = new SosEngine.Sprite(game, "logo");
            // Set origin of logo to its center and center horizontally
            logo.Origin = new Vector2(logo.BoundingBox.Width / 2, logo.BoundingBox.Height / 2);
            logo.CenterHorizontally(40);

            gameComponents.Add(logo);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (KeyPushed(Keys.Escape))
            {
                Game.Exit();
            }
            if (AnyKeyOrJoystickButtonPushed())
            {
                SosEngine.Core.SceneManager.SwitchScene(new Scenes.PlayScene(Game, "Level01"), SosEngine.SceneManager.TransitionEffect.Fade, 1000);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime, null);
            if (gameTime.TotalGameTime.Milliseconds >= 500)
            {
                font.PrintCenter("PRESS ANY KEY/BUTTON TO START", SosEngine.Core.HorizontalRenderCenter, SosEngine.Core.RenderHeight - 32);
            }
        }

    }
}
