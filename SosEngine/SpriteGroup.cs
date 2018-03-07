using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SosEngine
{
    public class SpriteGroup : DrawableGameComponent
    {
        protected List<Sprite> sprites;

        public IList<Sprite> Sprites
        {
            get { return sprites; }
        }

        public SpriteGroup(Game game) : base (game)
        {
            sprites = new List<Sprite>();
        }

        public void AddSprite(Sprite sprite)
        {
            sprites.Add(sprite);
        }

        public virtual void RemoveSprite(Sprite sprite)
        {
            sprites.Remove(sprite);
        }

        public Sprite GetSpriteAtLocation(int x, int y)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].Contains(x, y))
                {
                    return sprites[i];
                }
            }
            return null;
        }

        public bool Collide(Sprite sprite)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].Collide(sprite))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Collide(Rectangle rectangle)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].Collide(rectangle))
                {
                    return true;
                }
            }
            return false;
        }

        public List<Sprite> GetCollidingSprites(Sprite sprite)
        {
            return GetCollidingSprites(sprite.BoundingBox);
        }

        public List<Sprite> GetCollidingSprites(Rectangle rectangle)
        {
            List<Sprite> result = new List<Sprite>();
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].Collide(rectangle))
                {
                    result.Add(sprites[i]);
                }
            }
            return result;
        }

    }
}
