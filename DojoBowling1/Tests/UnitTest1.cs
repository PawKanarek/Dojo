using System;
using DojoBowling;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        private Game game;

        public UnitTest1()
        {
            this.game = new Game();
        }
        [Fact]
        public void OpenFrameTest()
        {
            game.MakeThrow(1, 3);
            for (int i = 0; i < 9; i++)
            {
                game.MakeThrow(0, 0);
            }

            Assert.Equal(4, game.GetScore());
        }

        [Fact]
        public void OpenFrameTest2()
        {
            game.MakeThrow(1, 3);
            for (int i = 0; i < 9; i++)
            {
                game.MakeThrow(1, 1);
            }

            Assert.Equal(22, game.GetScore());
        }

        [Fact]
        public void SpareTest()
        {
            game.MakeThrow(5, 5); // Spare
            game.MakeThrow(1, 3);
            for (int i = 0; i < 8; i++)
            {
                game.MakeThrow(0, 0);
            }

            Assert.Equal(15, game.GetScore());
        }

        [Fact]
        public void StrikeTest()
        {
            game.MakeThrow(10, 0); // Strike
            game.MakeThrow(1, 3);
            for (int i = 0; i < 8; i++)
            {
                game.MakeThrow(0, 0);
            }

            Assert.Equal(18, game.GetScore());
        }

        [Fact]
        public void StrikeTest2()
        {
            game.MakeThrow(10, 0); // Strike 10 + 10
            game.MakeThrow(5, 5); // Spare 10 + 1
            game.MakeThrow(1, 3); // Open 4
            for (int i = 0; i < 7; i++)
            {
                game.MakeThrow(0, 0);
            }

            Assert.Equal(35, game.GetScore());
        }

        [Fact]
        public void GutterBalls()
        {
            for (int i = 0; i < 10; i++)
            {
                game.MakeThrow(0, 0);
            }
            
            Assert.Equal(0, game.GetScore());
        }

        [Fact]
        public void Threes()
        {
            for (int i = 0; i < 10; i++)
            {
                game.MakeThrow(3, 3);   
            }
            Assert.Equal(60, game.GetScore());
        }

        [Fact]
        public void Spare()
        {
            game.MakeThrow(4, 6);
            game.MakeThrow(3, 5);
            for (int i = 0; i < 8; i++)
            {
                game.MakeThrow(0, 0);   
            }
            Assert.Equal(21, game.GetScore());
        }

        [Fact]
        public void Spare2()
        {
            game.MakeThrow(4, 6);
            game.MakeThrow(5, 3);
            for (int i = 0; i < 8; i++)
            {
                game.MakeThrow(0, 0);   
            }
            Assert.Equal(23, game.GetScore());
        }

        [Fact]
        public void Strike()
        {
            game.MakeThrow(10,0);
            game.MakeThrow(5, 3);
            for (int i = 0; i < 8; i++)
            {
                game.MakeThrow(0, 0);   
            }
            Assert.Equal(26, game.GetScore());
        }
        
        [Fact] 
        public void StrikeFinalFrame() {
            for (int i = 0; i < 9; i++)
            {
                game.MakeThrow(0, 0);   
            }
            game.MakeThrow(10, 0);
            game.MakeThrow(5, 3);
            Assert.Equal(18, game.GetScore());
        }
        
        [Fact] public void SpareFinalFrame() {
            for (int i = 0; i < 9; i++)
            {
                game.MakeThrow(0, 0);   
            }
            game.MakeThrow(4,6);
            game.MakeThrow(5,0);
            Assert.Equal(15, game.GetScore());
        }

        [Fact] public void Perfect() {
            for (int i = 0; i < 10; i++)
            {
                game.MakeThrow(10,0);
            }
            game.MakeThrow(10, 0);
            game.MakeThrow(10, 0);
            Assert.Equal( 300, game.GetScore()); 
        }

        [Fact] public void Alternating() {
            for ( int i = 0; i < 5; i++ ) {
                game.MakeThrow(10, 0);
                game.MakeThrow(4,6);
            }
            game.MakeThrow(10,0);
            Assert.Equal( 200, game.GetScore()); 
        }
    }
}