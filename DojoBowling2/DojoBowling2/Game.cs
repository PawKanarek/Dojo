using System.Collections.Generic;
using System.Linq;

namespace DojoBowling2
{
    public class Game
    {
        public readonly IList<Frame> Frames = new List<Frame>();

        public void MakeFrame(int first, int second)
        {
            Frame frame = null;
            if (this.Frames.Count > 9 && this.Frames[^1].GetFrameScore() != 0)
                frame = new Bouns(first, second, Frames.Count, Frames);
            else if (first == 10)
                frame = new Strike(first, second, Frames.Count, Frames);
            else if (first + second == 10)
                frame = new Spare(first, second, Frames.Count, Frames);
            else
                frame = new Frame(first, second, Frames.Count, Frames);


            Frames.Add(frame);
        }

        public int Score()
        {
            return Frames.Select(f => f.GetFrameScore()).Sum();
        }

        public class Frame
        {
            public Frame(int first, int second, int frameNumber, IList<Frame> frames)
            {
                First = first;
                Second = second;
                FrameNumber = frameNumber;
                Frames = frames;
            }

            public int First { get; set; }
            public int Second { get; set; }
            public int FrameNumber { get; }
            public IList<Frame> Frames { get; }

            public Frame NextFrame
            {
                get
                {
                    if (FrameNumber + 1 >= 0 && Frames.Count > FrameNumber + 1)
                    {
                        return Frames[FrameNumber + 1];
                    }
                    return null;
                }
            }

            public virtual int GetFrameScore()
            {
                return First + Second;
            }
        }

        public class Spare : Frame
        {
            public Spare(int first, int second, int frameNumber, IList<Frame> frames)
                : base(first, second, frameNumber, frames)
            {
            }

            public override int GetFrameScore()
            {
                return base.GetFrameScore() + (NextFrame?.First ?? 0);
            }
        }

        public class Strike : Frame
        {
            public Strike(int first, int second, int frameNumber, IList<Frame> frames)
                : base(first, second, frameNumber, frames)
            {
            }

            public override int GetFrameScore()
            {
                return base.GetFrameScore() + (NextFrame?.GetFrameScore() ?? 0);
            }
        }

        public class Bouns : Frame
        {
            public Bouns(int first, int second, int frameNumber, IList<Frame> frames)
                : base(first, second, frameNumber, frames)
            {
            }
        }
    }
}