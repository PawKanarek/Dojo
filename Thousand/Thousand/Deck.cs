using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace Thousand
{
    public class Score
    {
        public Score(string playerName, int playerPoints)
        {
            PlayerName = playerName;
            PlayerPoints = playerPoints;
        }

        public string PlayerName { get; set; }
        public int PlayerPoints { get; set; }
    }

    public class ScoreBoard
    {
        public List<(Score,Score,Score)> Score = new List<(Score,Score,Score)>();

        public void UpdateScore(List<Player> players)
        {
            var score = players.Select(x => new Score(x.Name, x.ScoreCards.SumPoints())).ToArray();
            Score.Add((score[0], score[1], score[2]));
        }
    }

    public class Game
    {
        public List<Player> Players = new()
        {
            new Player("A"), new Player("B"), new Player("C")
        };

        public CardsCollection? Stock;

        public CardsCollection RoundCards = new CardsCollection();
        public ScoreBoard ScoreBoard = new ScoreBoard();

        public Game() : this(new Deck())
        {
        }

        public Game(Deck deck)
        {
            deck.Cards.Shuffle();

            Stock = new CardsCollection();

            var playerId = 0;
            foreach (var card in deck.Cards)
            {
                if (Stock.Count < 3)
                {
                    Stock.Add(card);
                    continue;
                }

                var player = Players[playerId];
                if (player.Cards.Count < 7)
                {
                    player.Cards.Add(card);
                    if (player.Cards.Count == 7) playerId++;
                }
            }
        }

        public override string ToString()
        {
            return $"Stock: {Stock}, Players: {string.Join(", ", Players)}";
        }

        public Player? FightForStock()
        {
            if (Stock == null)
            {
                return null;
            }

            Players.Sort((x, y) => x.Cards.SumPoints().CompareTo(y.Cards.SumPoints()));
            Players[0].Cards.Add(Stock);
            Stock = null;

            return Players[0];
        }

        public void StockWinnerGives2Cards()
        {
            var winner = Players[0];

            var lowest1 = winner.Cards.GetLowestCard(true);
            Players[1].Cards.Add(lowest1);

            var lowest2 = winner.Cards.GetLowestCard(true);
            Players[2].Cards.Add(lowest2);
        }

        public void Play()
        {
            while (Players[0].Cards.Count > 0 && Players[1].Cards.Count > 0 && Players[2].Cards.Count > 0)
            {
                var highestCard0 = Players[0].Cards.GetHigestCard(true);
                var highestCard1 = Players[1].Cards.GetHigestCard(true);
                var highestCard2 = Players[2].Cards.GetHigestCard(true);
            }
        }
    }

    public class Player
    {
        public readonly CardsCollection Cards = new();
        public readonly CardsCollection ScoreCards = new(); 

        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"Player {Name} Cards {Cards}";
        }
    }

    public class Deck
    {
        public readonly CardsCollection Cards = new();

        public Deck()
        {
            foreach (var suit in (Suit[]) Enum.GetValues(typeof(Suit))) Cards.Add(CardsDefinitions.Suit.Copy(suit));
        }

        public override string ToString()
        {
            return $"Deck: {Cards}";
        }
    }

    public class CardsCollection : Collection<Card>
    {
        public CardsCollection() : this(null)
        {
        }

        public CardsCollection(CardsCollection? cardsCollection)
        {
            if (cardsCollection == null) return;

            Add(cardsCollection);
        }

        public void Add(CardsCollection cardsCollection)
        {
            foreach (var cards in cardsCollection) Items.Add(cards);
        }

        public CardsCollection Copy(Suit suit)
        {
            var collection = new CardsCollection();
            foreach (var item in Items) collection.Add(new Card(item.Points, suit));

            return collection;
        }

        public int SumPoints()
        {
            return this.Sum(x => x.Points);
        }

        public CardsCollection Copy()
        {
            return new(this);
        }

        public override string ToString()
        {
            return string.Join(", ", Items);
        }

        public Card GetLowestCard(bool remove)
        {
            var lowest = this.Items.Min(x => x.Points);
            var card = this.Items.FirstOrDefault(x => x.Points == lowest);
            if (remove)
            {
                this.Items.Remove(card);
            }

            return card;
        }

        public Card GetHigestCard(bool remove)
        {
            var highest = this.Items.Max(x => x.Points);
            var card  = this.Items.FirstOrDefault(x => x.Points == highest);
            if (remove)
            {
                this.Items.Remove(card);
            }
            return card;
        }
    }

    public struct Card
    {
        public Card(int points) : this(points, Suit.Hearts)
        {
        }

        public Card(int points, Suit suit)
        {
            Points = points;
            Suit = suit;
        }

        public int Points { get; }
        public Suit Suit { get; }

        public override string ToString()
        {
            return $"{Suit} {Points}";
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class CardsDefinitions
    {
        public static readonly Card Card_A = new(11);
        public static readonly Card Card_10 = new(10);
        public static readonly Card Card_K = new(4);
        public static readonly Card Card_Q = new(3);
        public static readonly Card Card_J = new(2);
        public static readonly Card Card_9 = new(0);

        public static readonly CardsCollection Suit = new()
        {
            Card_A, Card_10, Card_K, Card_Q, Card_J, Card_9
        };
    }

    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random? Local;

        public static Random ThisThreadsRandom
        {
            get
            {
                return Local ??=
                    new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId));
            }
        }
    }

    public static class MyExtensions
    {
        public static bool ContainsAnyElementFrom(this CardsCollection list1, CardsCollection list2)
        {
            return list1.Any(list2.Contains);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}