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
using FuncWorks.XNA.XTiled;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SosEngine
{

    public class Level : DrawableGameComponent
    {
        protected Map map;
        protected Rectangle mapView;
        protected string levelName;

        private List<TileAnimation> tileAnimations;
        private int tileAnimationCounter;

        private List<TileEffect> tileEffects;

        protected float scrollX;
        int mapScreenOffsetX;
        int mapScreenOffsetY;

        protected List<Sprite> parallaxSprites;

        public int GetScrollX()
        {
            return mapScreenOffsetX;
        }

        public int Width
        {
            get { return map.Width; }
        }

        public int Height
        {
            get { return map.Height; }
        }

        public int WidthInPixels
        {
            get { return widthInPixels; }
        }
        private int widthInPixels;

        public int HeightInPixels
        {
            get { return heightInPixels; }
        }
        private int heightInPixels;

        public List<TileAnimationDefinition> TileAnimationDefinitions { get; set; }

        private Dictionary<string, int> sourceIdOffset;

        public Level(Game game, string levelName, int screenOffsetX = 0, int screenOffsetY = 0)
            : base(game)
        {
            parallaxSprites = new List<Sprite>();

            this.levelName = levelName;

            // Create a new ContentManager to avoid loading cached map
            ContentManager cm = new ContentManager(Game.Services, Game.Content.RootDirectory);
            map = cm.Load<Map>("Levels\\" + levelName);
            cm.Unload();
            cm.Dispose();

            mapScreenOffsetX = screenOffsetX;
            mapScreenOffsetY = screenOffsetY;

            sourceIdOffset = new Dictionary<string, int>();
            int tilesCount = 0;

            for (int i = 0; i < map.Tilesets.Length; i++)
            {
                Tileset tileset = map.Tilesets[i];
                tileset.Texture = SosEngine.Core.GetTexture(
                    System.IO.Path.GetFileNameWithoutExtension(tileset.ImageFileName)
                    );
                sourceIdOffset.Add(map.TileLayers[i].Name, tilesCount);
                tilesCount += (tileset.ImageWidth / tileset.TileWidth) * (tileset.ImageHeight / tileset.TileHeight);
            }

            widthInPixels = map.Width * map.TileWidth;
            heightInPixels = map.Height * map.TileHeight;

            mapView = new Rectangle(0, 0, widthInPixels, heightInPixels);

            tileAnimations = new List<TileAnimation>();

            tileEffects = new List<TileEffect>();

            // Debug - Write layer names
            /*
            Core.Log("TileLayers:");
            foreach (TileLayer tileLayer in map.TileLayers)
            {
                Core.Log("  " + tileLayer.Name);
            }
            */
        }

        public string GetCustomProperty(string name)
        {
            if (map.Properties.ContainsKey(name))
            {
                return map.Properties[name].Value;
            }
            return "";
        }

        public void AddParallaxSprite(Sprite sprite, int slowness)
        {
            sprite.Tag = slowness;
            parallaxSprites.Add(sprite);
        }

        protected void SetLayerVisibility(string layerName, bool visible)
        {
            map.TileLayers[layerName].Visible = visible;
        }

        public void HideLayer(string layerName)
        {
            SetLayerVisibility(layerName, false);
        }

        public void ShowLayer(string layerName)
        {
            SetLayerVisibility(layerName, true);
        }

        /// <summary>
        /// Get top left location of all blocks of specified type
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public List<Point> GetTilePositionsForBlock(string layerName, int block)
        {
            List<Point> result = new List<Point>();
            TileLayer layer = map.TileLayers[layerName];
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (layer.Tiles[x][y] != null)
                    {
                        if (layer.Tiles[x][y].SourceID + 1 - sourceIdOffset[layerName] == block)
                        {
                            result.Add(layer.Tiles[x][y].Target.Location);
                        }
                    }
                }
            }
            return result;
        }

        public void SetupAnimatedTiles(string layerName)
        {
            TileLayer layer = map.TileLayers[layerName];

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var block = GetBlock(layerName, x, y);
                    if (block > 0)
                    {
                        var tileAnimationDefinition = TileAnimationDefinitions.FirstOrDefault(t => t.Block == block);
                        if (tileAnimationDefinition != null)
                        {
                            tileAnimations.Add(new TileAnimation
                            {
                                Layer = layerName,
                                Sequence = tileAnimationDefinition.Sequence,
                                Index = 0,
                                X = x,
                                Y = y
                            });
                            // SosEngine.Core.Log(string.Format("Animate tile {0} {1}", x, y));
                        }
                    }
                }
            }

        }

        public void StopTileAnimationAt(string layerName, int x, int y)
        {
            var tileAnimation = tileAnimations.FirstOrDefault(t => t.Layer == layerName && t.X == x && t.Y == y);
            if (tileAnimation != null)
            {
                tileAnimations.Remove(tileAnimation);
            }
        }

        public int GetBlockAtPixel(string layerName, int x, int y)
        {
            if (scrollX != 0)
            {
                x = x - (int)mapScreenOffsetX;
            }

            TileLayer layer = map.TileLayers[layerName];
            int bx = x / map.TileWidth;
            int by = y / map.TileHeight;
            if (x < 0 || y < 0 || bx < 0 || by < 0 || bx > map.Width - 1 || by > map.Height - 1)
            {
                return 0;
            }
            if (layer.Tiles[bx][by] != null)
            {
                return layer.Tiles[bx][by].SourceID + 1;
            }
            return 0;
        }

        public int GetBlockAtPixel(string layerName, int x, int y, out int bx, out int by)
        {
            if (scrollX != 0)
            {
                x = x - (int)mapScreenOffsetX;
            }

            TileLayer layer = map.TileLayers[layerName];
            bx = x / map.TileWidth;
            by = y / map.TileHeight;
            if (x < 0 || y < 0 || bx < 0 || by < 0 || bx > map.Width - 1 || by > map.Height - 1)
            {
                return 0;
            }
            if (layer.Tiles[bx][by] != null)
            {
                return layer.Tiles[bx][by].SourceID + 1;
            }
            return 0;
        }

        public int GetBlock(string layerName, int x, int y)
        {
            if (x < 0 || y < 0 || x > map.Width - 1 || y > map.Height - 1)
            {
                return 0;
            }
            TileLayer layer = map.TileLayers[layerName];
            if (layer.Tiles[x][y] != null)
            {
                return layer.Tiles[x][y].SourceID + 1 - sourceIdOffset[layerName];
            }
            return 0;
        }

        public void PutBlock(string layerName, int x, int y, int block)
        {
            var layer = map.TileLayers[layerName];
            if (layer.Tiles[x][y] == null)
            {
                layer.Tiles[x][y] = new TileData()
                {
                    SourceID = block,
                    Effects = SpriteEffects.None,
                    Rotation = 0,
                    Target = new Rectangle((x * map.TileWidth) + map.TileWidth / 2, (y * map.TileHeight) + map.TileHeight / 2, map.TileWidth, map.TileHeight)
                };
            }
            else
            {
                layer.Tiles[x][y].SourceID = block;
            }
        }

        public void RemoveBlock(string layerName, int x, int y)
        {
            var layer = map.TileLayers[layerName];
            layer.Tiles[x][y] = null;
        }

        public void BounceBlock(string layerName, int x, int y)
        {
            if (IsBlockBouncing(layerName, x, y))
            {
                return;
            }

            var layer = map.TileLayers[layerName];
            if (layer.Tiles[x][y] != null)
            {
                this.tileEffects.Add(new BounceTileEffect(layer.Tiles[x][y], layerName, x, y));
            }
        }

        public bool IsBlockBouncing(string layerName, int x, int y)
        {
            var tileEffect = tileEffects.FirstOrDefault(t => t.LayerName == layerName && t.TileX == x && t.TileY == y);
            return tileEffect != null && tileEffect is BounceTileEffect;
        }

        public List<LevelObject> GetLevelObjects(string layerName, string objectType = "")
        {
            List<LevelObject> result = new List<LevelObject>();
            ObjectLayer layer = map.ObjectLayers[layerName];
            for (int i = 0; i < layer.MapObjects.Length; i++)
            {
                if (string.IsNullOrEmpty(objectType) || layer.MapObjects[i].Type == objectType)
                {
                    result.Add(new LevelObject(layer.MapObjects[i].Name, layer.MapObjects[i].Bounds.X, layer.MapObjects[i].Bounds.Y, layer.MapObjects[i].Bounds));
                }
            }
            return result;
        }

        public bool ScrollX(float amount)
        {
            scrollX += amount;
            mapScreenOffsetX = (int)Math.Round(scrollX);

            if (mapScreenOffsetX > 0)
            {
                scrollX = 0;
                mapScreenOffsetX = 0;
                return false;
            }

            if (amount < 0 && mapScreenOffsetX < -widthInPixels + SosEngine.Core.RenderWidth)
            {
                scrollX = -widthInPixels + SosEngine.Core.RenderWidth;
                mapScreenOffsetX = -widthInPixels + SosEngine.Core.RenderWidth;
                return false;
            }

            return true;

        }

        public void SetOffset(int x, int y)
        {
            scrollX = x;
            mapScreenOffsetX = x;
        }

        protected void AnimateTiles()
        {
            foreach (var tileAnimation in tileAnimations)
            {
                tileAnimation.Index++;
                if (tileAnimation.Index > tileAnimation.Sequence.Count() - 1)
                {
                    tileAnimation.Index = 0;
                }
                PutBlock(tileAnimation.Layer, tileAnimation.X, tileAnimation.Y, tileAnimation.Sequence[tileAnimation.Index] - 1);
            }
        }

        public override void Update(GameTime gameTime)
        {
            tileAnimationCounter++;
            if (tileAnimationCounter > 6)
            {
                tileAnimationCounter = 0;
                AnimateTiles();
            }

            foreach (var tileEffect in tileEffects)
            {
                tileEffect.Update(gameTime);
            }
            tileEffects.RemoveAll(x => x.IsFinished);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var parallaxSprite in parallaxSprites)
            {
                parallaxSprite.DrawOffsetX = (int)Math.Round(mapScreenOffsetX * (1f / (float)parallaxSprite.Tag));
                parallaxSprite.Draw(gameTime);
            }

            var mapViewDraw = new Rectangle(-mapScreenOffsetX, mapScreenOffsetY, widthInPixels, heightInPixels);
            map.Draw(SosEngine.Core.SpriteBatch, mapViewDraw);
            base.Draw(gameTime);
        }

    }

}
