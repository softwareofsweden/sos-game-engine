using System.Collections.Generic;

namespace SosEngine
{

    public class Animation
    {

        protected List<SosEngine.AnimationFrame> frames;

        public int FrameCount
        {
            get
            {
                return frames.Count;
            }
        }

        public Animation(string assetName, int frameCount, int delay)
        {
            this.frames = new List<AnimationFrame>();
            for (int i = 0; i < frameCount; i++)
            {
                string spriteFrameName = assetName + "_" + i.ToString();
                frames.Add(new AnimationFrame() { SpriteFrameName = spriteFrameName, Delay = delay });
            }
        }

    }
}
