using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mario.Helpers
{

    /// <summary>
    /// Helper class for loading content
    /// </summary>
    public static class ContentHelper
    {

        /// <summary>
        /// Load all content
        /// </summary>
        public static void LoadAllContent()
        {
            Helpers.ContentHelper.LoadTextures(); // Load Texture
            Helpers.ContentHelper.LoadSpriteFrames(); // Setup Sprite Frames
            Helpers.ContentHelper.LoadSoundEffects(); // Load Sound Effects
            Helpers.ContentHelper.LoadFonts(); // Load Fonts
            Helpers.ContentHelper.LoadShaders(); // Load Shaders
            Helpers.ContentHelper.LoadBitmapFonts(); // Load Bitmap Fonts
        }

        /// <summary>
        /// Load shaders
        /// </summary>
        public static void LoadShaders()
        {
            //SosEngine.Core.LoadShader("Scanlines");
        }

        /// <summary>
        /// Load fonts
        /// </summary>
        public static void LoadFonts()
        {
            //SosEngine.Core.LoadSpriteFont("Font");
        }

        public static void LoadBitmapFonts()
        {
            SosEngine.Core.LoadBitmapFont("font", 10, 10, "| !\"#$%&'()*+,-./0123456789:;<=>? ||||||||||||||||*_^|||||||½|||||ABCDEFGHIJKLMNOPQRSTUVWXYZ[£]||");
            SosEngine.Core.GetBitmapFont("font").CharSpacing = 8;
        }

        /// <summary>
        /// Load all sound effects
        /// </summary>
        public static void LoadSoundEffects()
        {
            SosEngine.Core.LoadSoundEffect("Bump");
            SosEngine.Core.LoadSoundEffect("Coin");
            SosEngine.Core.LoadSoundEffect("jump");
            SosEngine.Core.LoadSoundEffect("Powerup");
            SosEngine.Core.LoadSoundEffect("ItemSprout");
            SosEngine.Core.LoadSoundEffect("1up");
            SosEngine.Core.LoadSoundEffect("PowerDown");
            SosEngine.Core.LoadSoundEffect("BrickShatter");
            SosEngine.Core.LoadSoundEffect("Fireball");
        }

        /// <summary>
        /// Load all textures
        /// </summary>
        public static void LoadTextures()
        {
            SosEngine.Core.LoadTexture("font");
            SosEngine.Core.LoadTexture("Items");
            SosEngine.Core.LoadTexture("Tileset16x16");
            SosEngine.Core.LoadTexture("Mario32x32");
            SosEngine.Core.LoadTexture("Hills");
            SosEngine.Core.LoadTexture("Clouds2");
            SosEngine.Core.LoadTexture("Misc");
            SosEngine.Core.LoadTexture("Editor");
        }

        /// <summary>
        /// Define all sprite frames
        /// </summary>
        public static void LoadSpriteFrames()
        {
            // Logo x=0 y=0 w=176 h=88
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Misc", "logo", new Rectangle(0, 0, 176, 88)));

            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Editor", "mouse_pointer", new Rectangle(0, 0, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Editor", "mouse_grab", new Rectangle(32, 0, 32, 32)));

            // Mario 32x32
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "mario_move_0", new Rectangle(32 * 0, 0, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "mario_move_1", new Rectangle(32 * 1, 0, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "mario_move_2", new Rectangle(32 * 2, 0, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "mario_jump", new Rectangle(32 * 3, 0, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "mario_skidding", new Rectangle(32 * 4, 0, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "mario_sitting", new Rectangle(32 * 5, 0, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "mario_shooting", new Rectangle(32 * 6, 0, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "tiny_mario_move_0", new Rectangle(32 * 0, 32, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "tiny_mario_move_1", new Rectangle(32 * 1, 32, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "tiny_mario_move_2", new Rectangle(32 * 2, 32, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "tiny_mario_jump", new Rectangle(32 * 3, 32, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "tiny_mario_skidding", new Rectangle(32 * 4, 32, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "super_mario_move_0", new Rectangle(32 * 0, 64, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "super_mario_move_1", new Rectangle(32 * 1, 64, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "super_mario_move_2", new Rectangle(32 * 2, 64, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "super_mario_jump", new Rectangle(32 * 3, 64, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "super_mario_skidding", new Rectangle(32 * 4, 64, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "super_mario_sitting", new Rectangle(32 * 5, 64, 32, 32)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Mario32x32", "super_mario_shooting", new Rectangle(32 * 6, 64, 32, 32)));


            // Mushroom
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "mushroom", new Rectangle(0, 0, 16, 16)));
            // 1-up Mushroom
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "1up_mushroom", new Rectangle(16, 0, 16, 16)));
            // Deadly Mushroom
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "deadly_mushroom", new Rectangle(32, 0, 16, 16)));

            // Flower
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "flower_0", new Rectangle(16 * 4, 0, 16, 16)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "flower_1", new Rectangle(16 * 5, 0, 16, 16)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "flower_2", new Rectangle(16 * 6, 0, 16, 16)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "flower_3", new Rectangle(16 * 7, 0, 16, 16)));

            // Fireball
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "fireball", new Rectangle(32, 148, 8, 8)));

            // Coin effect 20x16
            for (int i = 0; i < 9; i++)
            {
                SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", string.Format("coin_{0}", i), new Rectangle((i * 21) + 1, 128 + 1, 20, 16)));
            }

            // Smoke effect 16x16
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "smoke_1", new Rectangle(0, 160, 16, 16)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "smoke_2", new Rectangle(16, 160, 16, 16)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "smoke_3", new Rectangle(32, 160, 16, 16)));

            // Fireball explode
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "fireball_explode_1", new Rectangle(48, 160, 16, 16)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "fireball_explode_2", new Rectangle(64, 160, 16, 16)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "fireball_explode_3", new Rectangle(80, 160, 16, 16)));

            // 1-Up
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "score_1up", new Rectangle(0, 176, 16, 8)));

            // Score
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "score_100", new Rectangle(16, 176, 13, 8)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "score_200", new Rectangle(29, 176, 14, 8)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "score_400", new Rectangle(43, 176, 14, 8)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "score_500", new Rectangle(57, 176, 14, 8)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "score_800", new Rectangle(71, 176, 14, 8)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "score_1000", new Rectangle(85, 176, 17, 8)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "score_2000", new Rectangle(102, 176, 18, 8)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "score_4000", new Rectangle(120, 176, 18, 8)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "score_8000", new Rectangle(138, 176, 18, 8)));

            // Breaking block
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "break_block_1", new Rectangle(0, 148, 8, 8)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "break_block_2", new Rectangle(8, 148, 8, 8)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "break_block_3", new Rectangle(16, 148, 8, 8)));

            // Enemies

            // Goomba 16x16
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "goomba", new Rectangle(0, 192, 16, 16)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "goomba_stomped", new Rectangle(16, 192, 16, 16)));

            // Bullet 16x16
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "bullet_1", new Rectangle(0, 232, 16, 16)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "bullet_2", new Rectangle(16, 232, 16, 16)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Items", "bullet_3", new Rectangle(32, 232, 16, 16)));

            // Hills
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Hills", "hill_1_159x68", new Rectangle(0, 0, 159, 68)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Hills", "hill_2_192x84", new Rectangle(160, 0, 192, 84)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Hills", "hill_3_336x116", new Rectangle(0, 84, 336, 116)));
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Hills", "hill_4_384x180", new Rectangle(0, 200, 384, 180)));

            // Clouds
            SosEngine.Core.SpriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Clouds2", "clouds_1_384x144", new Rectangle(0, 0, 384, 144)));
        }

    }
}
