namespace SoloBlackjack
{
    public class Deck
    {
        private static Random random = new Random();

        private List<Card> cards = new List<Card>();

        public Deck()
        {
            for (var i = 1; i <= 13; ++i)
            {
                cards.Add(new Card(Symbols.Spade, i));
                cards.Add(new Card(Symbols.Clover, i));
                cards.Add(new Card(Symbols.Diamond, i));
                cards.Add(new Card(Symbols.Heart, i));
            }
        }

        public Card Pop()
        {
            var index = random.Next(0, cards.Count);
            var card = cards[index];

            cards.RemoveAt(index);

            return card;
        }
    }
}
