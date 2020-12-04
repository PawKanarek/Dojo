using System.Collections.Generic;
using System.Linq;

namespace DojoBowling
{
    class Frame
    {
        public Frame(int firstThrow, int secondThrow)
        {
            FirstThrow = firstThrow;
            SecondThrow = secondThrow;
        }

        public int FirstThrow { get; set; }
        public int SecondThrow { get; set; }

        public FrameType FrameType =>
            FirstThrow == 10
                ? FrameType.Strike
                : Sum() == 10
                    ? FrameType.Spare
                    : FrameType.Open;

        public int Sum()
        {
            return FirstThrow + SecondThrow;
        }
    }

    enum FrameType
    {
        Open,
        Spare,
        Strike
    }

    public class Game
    {
        List<Frame> frames = new List<Frame>();

        public void MakeThrow(int firstThrow, int secondThrow)
        {
            frames.Add(new Frame(firstThrow, secondThrow));
        }

        public int GetScore()
        {
            var score = 0;
            for (int i = 0; i < 10; i++)
            {
                var frame = frames[i];
                if (frame.FrameType == FrameType.Strike && i < 9)
                {
                    score += frame.Sum();
                    var nextFrame = frames[i + 1];
                    if (nextFrame.FrameType == FrameType.Strike)
                    {
                        score += nextFrame.FirstThrow;
                        score += frames[i + 2].FirstThrow;
                    }
                    else
                    {
                        score += nextFrame.Sum();
                    }
                }
                else if (frame.FrameType == FrameType.Spare && i < 9)
                {
                    score += frame.Sum() + frames[i + 1].FirstThrow;
                }
                else
                {
                    score += frame.Sum();
                }
            }

            var frame9 = frames[9];
            var frame10 = frames.Count > 10 ? frames[10] : null;
            var frame11 = frames.Count > 11 ? frames[11] : null;

            if (frame10 != null && frame9.FrameType == FrameType.Strike)
            {
                score += frame10.Sum();
                if (frame11 != null && frame10.FrameType == FrameType.Strike)
                {
                    score += frame11.Sum();
                }
            }
            else if (frame10 != null && frame9.FrameType == FrameType.Spare)
            {
                score += frame10.Sum();
            }

            return score;
        }
    }
}