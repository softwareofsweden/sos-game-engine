using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mario.Objects
{
    /// <summary>
    /// Handles items that can be picked up by the player
    /// </summary>
    public class PickupManager : SosEngine.SpriteGroup
    {
        /// <summary>
        /// Animation delay in milliseconds for
        /// animated items
        /// </summary>
        private static int animationDelay = 100;
        /// <summary>
        /// Items that are hidden when level is started. They will
        /// be relealed once a revealing item is picked up
        /// </summary>
        private static int[] hiddenItems = { };
        /// <summary>
        /// Items that will reveal hidden items
        /// </summary>
        private static int[] revealingItems = { };
        /// <summary>
        /// Items that are animated
        /// </summary>
        private static int[] animatedItems = { 170 };
        /// <summary>
        /// Items that spawn ladder blocks on level 6
        /// </summary>
        private static int[] ladderItems = { };
        /// <summary>
        /// Plug for draining bathtub
        /// </summary>
        private static int bathtubPlug = -1;
        /// <summary>
        /// Key for spawning toothbrush
        /// </summary>
        private static int toothBrushKey = -1;
        /// <summary>
        /// Key for bird cage
        /// </summary>
        private static int birdCageKey = -1;
        /// <summary>
        /// Parachute
        /// </summary>
        private static int parachute = -1;
        /// <summary>
        /// Sprite frame cache for pickup items
        /// </summary>
        private SosEngine.SpriteFrameCache spriteFrameCache;
        
        /// <summary>
        /// Check if item is a hidden item
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private bool IsHiddenItem(int block)
        {
            return hiddenItems.Contains(block);
        }

        /// <summary>
        /// Check if item is a revealing item
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private bool IsRevealingItem(int block)
        {
            return revealingItems.Contains(block);
        }
        
        /// <summary>
        /// Check if item is an animated item
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private bool IsAnimatedItem(int block)
        {
            return animatedItems.Contains(block);
        }

        /// <summary>
        /// Ensure the specific block has a sprite frame
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private string GetSpriteFrameNameForBlock(int block)
        {
            string spriteFrameName = string.Format("Pickups_{0}", block);
            if (!spriteFrameCache.SpriteFrameExists(spriteFrameName))
            {
                int rx = (block - 1) % 32;
                int ry = (block - 1) / 32;
                Rectangle rectangle = new Rectangle(rx * 16, ry * 16, 16, 16);
                spriteFrameCache.AddSpriteFrame(new SosEngine.SpriteFrame("Pickups", spriteFrameName, rectangle));
            }
            return spriteFrameName;
        }

        public bool IsBathtubPlug(Pickup pickup)
        {
            return pickup.Block == bathtubPlug;
        }

        public bool IsToothBrushKey(Pickup pickup)
        {
            return pickup.Block == toothBrushKey;
        }

        public bool IsBirdCageKey(Pickup pickup)
        {
            return pickup.Block == birdCageKey;
        }

        public bool IsParachute(Pickup pickup)
        {
            return pickup.Block == parachute;
        }

        public bool IsLadderSpawnItem(Pickup pickup)
        {
            return ladderItems.Contains(pickup.Block);
        }

        public PickupManager(Game game, SosEngine.Level level, GameComponentCollection gameComponents) : base (game)
        {
            spriteFrameCache = new SosEngine.SpriteFrameCache();
            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {
                    int block = level.GetBlock("Pickups", x, y);
                    if (block > 0)
                    {
                        string spriteFrameName = GetSpriteFrameNameForBlock(block);
                        Pickup pickup = new Pickup(game, spriteFrameName, block, x * 16, y * 16, animationDelay, 100, spriteFrameCache);
                        if (IsHiddenItem(block))
                        {
                            pickup.Visible = false;
                        }
                        if (IsAnimatedItem(block))
                        {
                            string spriteFrameName2 = GetSpriteFrameNameForBlock(block + 1);
                            pickup.AddFrame(spriteFrameName2, animationDelay);
                            pickup.AddFrame(GetSpriteFrameNameForBlock(block + 2), animationDelay);
                            pickup.AddFrame(GetSpriteFrameNameForBlock(block + 3), animationDelay);

                        }
                        pickup.DrawOffsetX = 0;
                        gameComponents.Add(pickup);
                        AddSprite(pickup);
                    }
                }
            }
        }

        public void RevealHiddenItems()
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                if (!sprites[i].Visible)
                {
                    sprites[i].Visible = true;
                }
            }
        }

        public override void RemoveSprite(SosEngine.Sprite sprite)
        {
            Pickup pickup = (Pickup)sprite;
            if (IsRevealingItem(pickup.Block))
            {
                RevealHiddenItems();
            }
            base.RemoveSprite(sprite);
        }

        public void PlaySoundForPickup(Pickup pickup)
        {
            /*
            if (IsBathtubPlug(pickup))
            {
                SosEngine.Core.PlaySound("drain");
            }
            else if (IsRevealingItem(pickup.Block) || IsToothBrushKey(pickup) || IsBathtubPlug(pickup) || IsBirdCageKey(pickup) || IsParachute(pickup))
            {
                SosEngine.Core.PlaySound("pickup_special");
            }
            else
            {
                SosEngine.Core.PlaySound("pickup");
            }
            */
            SosEngine.Core.PlaySound("pickup");
        }

        public void SetDrawOffset(int offset)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].DrawOffsetX = offset;
            }
        }

    }
}
