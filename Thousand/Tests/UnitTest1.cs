using System.Linq;
using FluentAssertions;
using Thousand;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Cards_Are_Divided_Correctly()
        {
            var game = new Game();
            
            var totalPoints = game.Players
                .Select(x => x.Cards.Sum(x => x.Points)).Sum() + game.Stock!.Sum(x => x.Points);
            totalPoints.Should().Be(120);

            game.Players[0].Cards.Should().HaveCount(7);
            game.Players[1].Cards.Should().HaveCount(7);
            game.Players[2].Cards.Should().HaveCount(7);
            game.Stock.Should().HaveCount(3);

            this.CheckPlayersHaveDiffrentCards(game);
        }

        [Fact]
        public void When_No_More_Cards_Result_Saved()
        {
            var game = new Game(); 
            game.FightForStock();
            game.StockWinnerGives2Cards();
            game.Play();
            game.Players[0].Cards.Should().HaveCount(0);
            game.Players[1].Cards.Should().HaveCount(0);
            game.Players[2].Cards.Should().HaveCount(0);
        }

        [Fact]
        public void Deck_Contains_6_Cards_Of_Each_Suit()
        {
            var deck = new Deck();

            var grouped = deck.Cards.ToLookup(x => x.Suit, x => x);

            grouped[Suit.Hearts].Should().HaveCount(6);
            grouped[Suit.Diamonds].Should().HaveCount(6);
            grouped[Suit.Clubs].Should().HaveCount(6);
            grouped[Suit.Spades].Should().HaveCount(6);
        }

        [Fact]
        public void Highest_Bidder_Wins_Stock()
        {
            var game = new Game();
            var winner = game.FightForStock();

            winner.Should().NotBeNull();
            game.Stock.Should().BeNull();
            winner.Cards.Should().HaveCount(10);
        }

        [Fact]
        public void Highest_Bidder_Gives_2_Cards()
        {
            var game = new Game();
            var winner = game.FightForStock();
            game.StockWinnerGives2Cards();

            game.Players[0].Cards.Should().HaveCount(8);
            game.Players[1].Cards.Should().HaveCount(8);
            game.Players[2].Cards.Should().HaveCount(8);
            
            this.CheckPlayersHaveDiffrentCards(game);
        }

        [Fact]
        public void Total_Deck_Count_Equals_24()
        {
            var deck = new Deck();
            var count = deck.Cards.Count;

            count.Should().Be(24);
        }

        [Fact]
        public void Total_Deck_Points_Equals_120()
        {
            var deck = new Deck();
            var sum = deck.Cards.Sum(x => x.Points);

            sum.Should().Be(120);
        }

        [Fact]
        public void Total_Suit_Points_Equals_30()
        {
            var suit = CardsDefinitions.Suit.Copy();
            var sum = suit.Sum(x => x.Points);

            sum.Should().Be(30);
        }

        private void CheckPlayersHaveDiffrentCards(Game game)
        {
            game.Players[0].Cards.ContainsAnyElementFrom(game.Players[1].Cards).Should().BeFalse();
            game.Players[0].Cards.ContainsAnyElementFrom(game.Players[2].Cards).Should().BeFalse();
            game.Players[1].Cards.ContainsAnyElementFrom(game.Players[0].Cards).Should().BeFalse();
            game.Players[1].Cards.ContainsAnyElementFrom(game.Players[2].Cards).Should().BeFalse();
            game.Players[2].Cards.ContainsAnyElementFrom(game.Players[0].Cards).Should().BeFalse();
            game.Players[2].Cards.ContainsAnyElementFrom(game.Players[1].Cards).Should().BeFalse();
        }
    }
}