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

namespace SosEngine
{

    /// <summary>
    /// Sprite class.
    /// </summary>
    public class Sprite : DrawableGameComponent
    {

        /// <summary>
        /// The current sprite frame.
        /// </summary>
        protected SpriteFrame spriteFrame;

        /// <summary>
        /// Sprite cache associated with sprite.
        /// </summary>
        protected SpriteFrameCache spriteFrameCache;

        /// <summary>
        /// The current position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Flip horizontally.
        /// </summary>
        public bool Flipped { get; set; }

        /// <summary>
        /// Flip vertically.
        /// </summary>
        public bool FlippedVertically { get; set; }

        /// <summary>
        /// Screen drawing offset
        /// </summary>
        public int DrawOffsetX { get; set; }

        /// <summary>
        /// Screen drawing offset
        /// </summary>
        public int DrawOffsetY { get; set; }

        /// <summary>
        /// Clip sprite from bottom using specified amount.
        /// </summary>
        public int ClipBottomAmount { get; set; }

        /// <summary>
        /// Clip sprite from right using specified amount.
        /// </summary>
        public int ClipRightAmount { get; set; }

        /// <summary>
        /// Transparancy, 1.0 = Opaque 0.5 = 50% Visible.
        /// </summary>
        protected float alpha = 1.0f;

        /// <summary>
        /// If sprite is fading out.
        /// </summary>
        private bool isFadingOut = false;

        /// <summary>
        /// How long in milliseconds to wait before fading out after FadeOut is called.
        /// </summary>
        private int fadeOutDelay;

        /// <summary>
        /// How long in milliseconds to fade out from 1 to 0.
        /// </summary>
        private int fadeOutDuration;

        /// <summary>
        /// Time in milliseconds left to fade out.
        /// </summary>
        private int fadeOutDurationLeft;

        /// <summary>
        /// Put whatever you like here
        /// </summary>
        public int Tag;

        /// <summary>
        /// Scale of sprite.
        /// </summary>
        public float ScaleX
        {
            get { return scaleX; }
            set { scaleX = value; }
        }
        float scaleX;

        /// <summary>
        /// Scale of sprite.
        /// </summary>
        public float ScaleY
        {
            get { return scaleY; }
            set { scaleY = value; }
        }
        float scaleY;

        /// <summary>
        /// Origin of sprite.
        /// </summary>
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }
        protected Vector2 origin;

        /// <summary>
        /// BoundingBox used for collision detection
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                return GetBoundingBox();
            }
        }

        public Rectangle BoundingBoxWithOffset
        {
            get
            {
                Rectangle r = BoundingBox;
                r.Offset(DrawOffsetX, DrawOffsetY);
                return r;
            }
        }

        /// <summary>
        /// Returns the bounding box for the sprite. Used for collision detection.
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle GetBoundingBox()
        {
            if (spriteFrame != null)
            {
                if (spriteFrame.Rectangle.IsEmpty)
                {
                    return Rectangle.Empty;
                }
                return new Rectangle((int)Position.X - (int)origin.X, (int)Position.Y - (int)origin.Y, spriteFrame.Rectangle.Width, spriteFrame.Rectangle.Height);
            }
            else
            {
                return Rectangle.Empty;
            }
        }

        /// <summary>
        /// Creates a new sprite.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteFrameName"></param>
        public Sprite(Game game, string spriteFrameName, int x = 0, int y = 0, SpriteFrameCache spriteFrameCache = null)
            : base(game)
        {
            this.spriteFrameCache = spriteFrameCache == null ? SosEngine.Core.SpriteFrameCache : spriteFrameCache;

            scaleX = 1.0f;
            scaleY = 1.0f;
            origin = Vector2.Zero;
            Position = Vector2.Zero;
            if (!string.IsNullOrWhiteSpace(spriteFrameName))
            {
                spriteFrame = this.spriteFrameCache.GetSpriteFrame(spriteFrameName);
                Position = new Vector2(x, y);
            }
        }

        /// <summary>
        /// Check if Sprite collide with other Sprite
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Collide(Sprite other)
        {
            return this.BoundingBox.Intersects(other.BoundingBox);
        }

        /// <summary>
        /// Check if Sprite collide with any sprite in the group
        /// </summary>
        /// <param name="spriteGroup"></param>
        /// <returns></returns>
        public bool Collide(SpriteGroup spriteGroup)
        {
            return spriteGroup.Collide(this);
        }

        /// <summary>
        /// Check if Sprite collide with the specified rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public bool Collide(Rectangle rectangle)
        {
            return this.BoundingBox.Intersects(rectangle);
        }

        /// <summary>
        /// Check if Sprite contains the specified point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Contains(int x, int y)
        {
            return this.BoundingBox.Contains(x, y);
        }

        /// <summary>
        /// Update sprite.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (isFadingOut)
            {
                if (fadeOutDelay > 0)
                {
                    fadeOutDelay -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    fadeOutDurationLeft -= gameTime.ElapsedGameTime.Milliseconds;
                    alpha = (float)fadeOutDurationLeft / (float)fadeOutDuration;
                    if (fadeOutDurationLeft <= 0)
                    {
                        isFadingOut = false;
                        alpha = 0f;
                    }
                }
            }
        }

        /// <summary>
        /// Draw sprite.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
            {
                return;
            }
            if (spriteFrame != null)
            {
                SpriteEffects spriteEffects = Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                if (FlippedVertically)
                {
                    spriteEffects = spriteEffects | SpriteEffects.FlipVertically;
                }

                // texture, position, sourceRectangle, color
                // SosEngine.Core.SpriteBatch.Draw(Core.GetTexture(spriteFrame.AssetName), this.Position + new Vector2(DrawOffsetX, DrawOffsetY), spriteFrame.Rectangle, Color.White);

                // texture, position, sourceRectangle, color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth
                Color color = alpha < 1f ? Color.White * alpha : Color.White;
                Rectangle sourceRect = spriteFrame.Rectangle;
                if (ClipBottomAmount > 0 || ClipRightAmount > 0)
                {
                    sourceRect.Height = sourceRect.Height - ClipBottomAmount;
                    sourceRect.Width = sourceRect.Width - ClipRightAmount;
                    if (sourceRect.Width < 0)
                    {
                        sourceRect.Width = 0;
                    }
                }
                SosEngine.Core.SpriteBatch.Draw(Core.GetTexture(spriteFrame.AssetName), this.Position + new Vector2(DrawOffsetX, DrawOffsetY), sourceRect, color,
                    0, this.origin, new Vector2(this.scaleX, this.scaleY), spriteEffects, 0);

                if (Core.DebugSpriteBorders && !BoundingBox.IsEmpty)
                {
                    Rectangle rectangle = new Rectangle(BoundingBox.X + DrawOffsetX, BoundingBox.Y + DrawOffsetY, BoundingBox.Width, BoundingBox.Height);
                    Core.DrawRectangle(rectangle, Color.Red);
                }
            }
        }

        public void RenderAtPosition(int x, int y)
        {
            if (spriteFrame != null)
            {
                Vector2 pos = new Vector2(x, y);
                SpriteEffects spriteEffects = Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                SosEngine.Core.SpriteBatch.Draw(Core.GetTexture(spriteFrame.AssetName), pos + new Vector2(DrawOffsetX, DrawOffsetY), spriteFrame.Rectangle, Color.White * alpha, 0f, origin, new Vector2(scaleX, scaleY), spriteEffects, 0f);
            }
        }

        /// <summary>
        /// Draw sprite using specified color.
        /// </summary>
        /// <param name="color"></param>
        public void RenderWithColor(Color color)
        {
            SosEngine.Core.SpriteBatch.Draw(Core.GetTexture(spriteFrame.AssetName), this.Position, spriteFrame.Rectangle, color);
        }

        /// <summary>
        /// Draw sprite with transparency.
        /// </summary>
        /// <param name="alpha"></param>
        public void RenderWithAlpa(float alpha)
        {
            Color c = Color.White * alpha;
            SosEngine.Core.SpriteBatch.Draw(Core.GetTexture(spriteFrame.AssetName), this.Position, spriteFrame.Rectangle, c);
        }

        /// <summary>
        /// Begins to fade out a sprite.
        /// </summary>
        /// <param name="delay">Time in milliseconds before starting to fade out</param>
        /// <param name="duration">Time in milliseconds for the fade out</param>
        public void FadeOut(int delay, int duration)
        {
            Visible = true;
            fadeOutDuration = duration;
            fadeOutDurationLeft = duration;
            fadeOutDelay = delay;
            isFadingOut = true;
            alpha = 1.0f;
        }

        /// <summary>
        /// Set current sprite frame.
        /// </summary>
        /// <param name="spriteFrameName"></param>
        public virtual void SetSpriteFrame(string spriteFrameName)
        {
            if (spriteFrame == null || spriteFrame.FrameName != spriteFrameName)
            {
                spriteFrame = this.spriteFrameCache.GetSpriteFrame(spriteFrameName);
            }
        }

        /// <summary>
        /// Positions the sprite centered on screen.
        /// </summary>
        public void CenterOnScreen()
        {
            Position = new Vector2(
                SosEngine.Core.RenderWidth / 2 - BoundingBox.Width / 2,
                SosEngine.Core.RenderHeight / 2 - BoundingBox.Height / 2
                );
        }

        /// <summary>
        /// Positions the sprite horizontally centered and specified distance from top.
        /// </summary>
        /// <param name="y"></param>
        public void CenterHorizontally(int y)
        {
            Position = new Vector2(SosEngine.Core.RenderWidth / 2 - BoundingBox.Width / 2 + this.origin.X, y + this.origin.Y);
        }

    }
}
