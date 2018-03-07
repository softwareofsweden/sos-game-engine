using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SosEngine
{

    /// <summary>
    /// Animated Sprite
    /// </summary>
    public class AnimatedSprite : Sprite
    {

        /// <summary>
        /// Time in milliseconds current frame has been displayed.
        /// </summary>
        protected int milliseconds;

        /// <summary>
        /// List of frames to cycle through.
        /// </summary>
        protected List<SosEngine.AnimationFrame> frames;

        /// <summary>
        /// Should the animation repeat?
        /// </summary>
        public bool IsRepeatable
        {
            get { return isRepeatable; }
            set { isRepeatable = value;  }
        }
        protected bool isRepeatable;

        public bool CompletedAFullCycle
        {
            get { return completedAFullCycle; }
        }
        private bool completedAFullCycle;

        /// <summary>
        /// Index of current frame.
        /// </summary>
        protected int frameIndex;

        public bool Paused
        {
            get
            {
                return paused;
            }
            set
            {
                paused = value;
                if (paused)
                {
                    milliseconds = 0;
                }
            }
        }
        bool paused;

        /// <summary>
        /// Creates a new AnimatedSprite.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteFrameName"></param>
        /// <param name="delay">Delay in milliseconds</param>
        public AnimatedSprite(Game game, string spriteFrameName, int delay, SpriteFrameCache spriteFrameCache = null)
            : base(game, spriteFrameName, 0, 0, spriteFrameCache)
        {
            this.isRepeatable = true;
            this.completedAFullCycle = false;
            this.milliseconds = 0;
            frames = new List<AnimationFrame>();
            if (!string.IsNullOrEmpty(spriteFrameName))
            {
                AddFrame(spriteFrameName, delay);
            }
            frameIndex = 0;
        }

        private int RestartAnimation()
        {
            completedAFullCycle = true;
            if (isRepeatable)
            {
                return 0;
            }
            return frameIndex;
        }

        public override void Update(GameTime gameTime)
        {
            if (!paused && frames.Count > 1)
            {
                milliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (milliseconds >= frames[frameIndex].Delay)
                {
                    milliseconds = 0;
                    frameIndex = (frames.Count > frameIndex + 1) ? frameIndex + 1 : RestartAnimation();
                    SetSpriteFrame(frames[frameIndex].SpriteFrameName);
                }
            }
            base.Update(gameTime);
        }

        public void Animate()
        {
            if (frames.Count > 1)
            {
                frameIndex = (frames.Count > frameIndex + 1) ? frameIndex + 1 : RestartAnimation();
                SetSpriteFrame(frames[frameIndex].SpriteFrameName);
            }
        }

        public void AnimateTo(int index)
        {
            this.frameIndex = index;
            if (frames.Count > 1)
            {
                frameIndex = (frames.Count > frameIndex) ? frameIndex : RestartAnimation();
                SetSpriteFrame(frames[frameIndex].SpriteFrameName);
            }
        }
 
        /// <summary>
        /// Add frame for animation
        /// </summary>
        /// <param name="spriteFrameName"></param>
        /// <param name="delay">Delay in milliseconds</param>
        public void AddFrame(string spriteFrameName, int delay)
        {
            frames.Add(new AnimationFrame() { SpriteFrameName = spriteFrameName, Delay = delay });
        }
        
    }
}
