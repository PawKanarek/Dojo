using System.Linq;
using DojoBowling2;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        public UnitTest1()
        {
            game = new Game();
        }

        private readonly Game game;

        [Fact]
        public void CanMakeFrame()
        {
            game.MakeFrame(0, 0);
            Assert.True(true);
        }

        [Fact]
        public void EmptyFrameMakesNoScore()
        {
            game.MakeFrame(0, 0);
            Assert.Equal(0, game.Score());
        }

        [Fact]
        public void SpareMakesNextPinDouble()
        {
            game.MakeFrame(5, 5);
            game.MakeFrame(1, 1);
            Assert.Equal(13, game.Score());
        }

        [Fact]
        public void StrikeMakesAddsScoreToCurrent()
        {
            game.MakeFrame(10, 0);
            game.MakeFrame(1, 1);
            Assert.Equal(14, game.Score());
        }

        [Fact]
        public void TestsWorking()
        {
            Assert.True(true);
        }

        [Fact]
        public void FrameWithPoints()
        {
            game.MakeFrame(1, 1);
            Assert.Equal(2, game.Score());
        }

        [Fact]
        public void After10ThrowThereIsNoBonusForOpenFrame()
        {
            for (int i = 0; i < 9; i++)
            {
                game.MakeFrame(0,0);
            }
            game.MakeFrame(0,0);
            
            Assert.IsNotType<Game.Bouns>(game.Frames.Last());
            Assert.Equal(0, game.Score());
        }
        
        [Fact]
        public void After10ThrowThereIsSingleBonusForSpare()
        {
            for (int i = 0; i < 9; i++)
            {
                game.MakeFrame(0,0);
            }
            game.MakeFrame(5,5);
            game.MakeFrame(0,0);
            
            Assert.IsType<Game.Bouns>(game.Frames[^1]);
            Assert.IsType<Game.Bouns>(game.Frames[^2]);
            Assert.Equal(0, game.Score());
        }
        
        [Fact]
        public void After10ThrowThereIs2BonusAfterSpareSpare()
        {
            for (int i = 0; i < 9; i++)
            {
                game.MakeFrame(0,0);
            }
            game.MakeFrame(0,0);
            game.MakeFrame(0,0);
            
            Assert.IsType<Game.Bouns>(game.Frames[^1]);
            Assert.IsType<Game.Bouns>(game.Frames[^2]);
            Assert.Equal(0, game.Score());
        }
        
    }
}