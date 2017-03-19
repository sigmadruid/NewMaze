using System;

namespace GamePlot
{
    public interface SegmentHandler
    {
        void Start();
        void Update();
        void End();
    }

    public class AnimationSegmentHandler : SegmentHandler
    {
        public void Start()
        {
        }
        public void Update()
        {
        }
        public void End()
        {
        }
    }

    public class MoveSegmentHandler : SegmentHandler
    {
        public void Start()
        {
        }
        public void Update()
        {
        }
        public void End()
        {
        }
    }
}

