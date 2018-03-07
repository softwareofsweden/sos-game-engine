using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mario.Helpers
{
    public static class LevelHelper
    {
        /// <summary>
        /// Number of levels.
        /// </summary>
        private static int NumberOfLevels = 7;

        /// <summary>
        /// Check if block is a wall.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static bool IsWall(int block)
        {
            if (block < 1)
            {
                return false;
            }
            switch (block)
            {
                case 1:
                    // Grass
                    return true;
                case 2:
                    // Grass
                    return true;
                case 3:
                    // Grass
                    return true;
                case 4:
                    // Underground
                    return true;
                case 5:
                    // Underground
                    return true;
                case 6:
                    // Underground
                    return true;
                case 65:
                    // Bricks
                    return true;
                case 66:
                    // Bricks
                    return true;
                case 67:
                    // Bricks
                    return true;
                case 68:
                    // Used block
                    return true;
                case 69:
                    // Used block
                    return true;
                case 70:
                    // Used block
                    return true;
                case 97:
                    // Question block
                    return true;
                case 98:
                    // Question block
                    return true;
                case 99:
                    // Question block
                    return true;
                case 129:
                    // Hard block
                    return true;
                case 130:
                    // Hard block
                    return true;
                case 131:
                    // Hard block
                    return true;
                case 163:
                    // Pipe up
                    return true;
                case 164:
                    // Pipe up
                    return true;
                case 195:
                    // Pipe up
                    return true;
                case 196:
                    // Pipe up
                    return true;
                case 225:
                    // Pipe side
                    return true;
                case 226:
                    // Pipe side
                    return true;
                case 227:
                    // Pipe side
                    return true;
                case 257:
                    // Pipe side
                    return true;
                case 258:
                    // Pipe side
                    return true;
                case 259:
                    // Pipe side
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsQuestionBlock(int block)
        {
            return block == 97 || block == 98 || block == 99;
        }

        public static bool IsBreakableBlock(int block)
        {
            return block == 65 || block == 66 || block == 67;
        }

        /// <summary>
        /// Check if block is a ladder.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static bool IsLadder(int block)
        {
            return false;
            // return block >= 1537 && block <= 2048;
        }

        /// <summary>
        /// Check if block is harmfull.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static bool IsHarmful(int block)
        {
            return false;
            // return block >= 513 && block <= 768;
        }

        /// <summary>
        /// Check if block is an exit.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static bool IsExit(int block)
        {
            return false;
            // return block >= 769 && block <= 1024;
        }

        /// <summary>
        /// Get player start position in pixels.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="playerName">Name of object</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void GetPlayerStartPosition(SosEngine.Level level, string playerName, out int x, out int y)
        {
            x = 0;
            y = 0;
            List<SosEngine.LevelObject> players = level.GetLevelObjects("Objects", "Player");
            if (players.Exists(p => p.Name == playerName))
            {
                SosEngine.LevelObject player = players.Find(p => p.Name == playerName);
                x = player.X;
                y = player.Y;
            }
        }

        /// <summary>
        /// Get next room number
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static int NextRoom(int room)
        {
            int nextRoom = room + 1;
            if (nextRoom > NumberOfLevels)
            {
                nextRoom = 1;
            }
            return nextRoom;
        }

        public static void SpawnLadderPart(SosEngine.Level level)
        {
            int y = 8;
            while (level.GetBlock("Block", 32, y) != 0)
            {
                y--;
            }
            level.PutBlock("Block", 32, y, 284);
            level.PutBlock("Block", 33, y, 285);
        }

        public static void SetupTileAnimation(SosEngine.Level level)
        {
            var tileAnimationDefinitions = new List<SosEngine.TileAnimationDefinition>();

            // [?]
            tileAnimationDefinitions.Add(new SosEngine.TileAnimationDefinition
            {
                Block = 97,
                Sequence = new int[] { 97, 98, 99 },
            });
            // Lava
            tileAnimationDefinitions.Add(new SosEngine.TileAnimationDefinition
            {
                Block = 100,
                Sequence = new int[] { 100, 101, 102, 103, 104, 105, 106, 107 },
            });
            // Grass
            tileAnimationDefinitions.Add(new SosEngine.TileAnimationDefinition
            {
                Block = 108,
                Sequence = new int[] { 108, 140, 172, 140 },
            });
            // Grass
            tileAnimationDefinitions.Add(new SosEngine.TileAnimationDefinition
            {
                Block = 109,
                Sequence = new int[] { 109, 141, 173, 141 },
            });
            // Grass
            tileAnimationDefinitions.Add(new SosEngine.TileAnimationDefinition
            {
                Block = 110,
                Sequence = new int[] { 110, 142, 174, 142 },
            });

            level.TileAnimationDefinitions = tileAnimationDefinitions;
        }

    }
}
