using Spectre.Console;

namespace SoloBlackjack
{
    public class Player
    {
        private List<Card> cards = new List<Card>();

        public void GetFromDeck(Deck deck, int count = 1)
        {
            for (var i = 0; i < count; ++i)
            {
                var card = deck.Pop();

                cards.Add(card);
            }
        }

        public int GetTotalNumber()
        {
            var total = 0;
            var aces = 0;

            for (var i = 0; i < cards.Count; ++i)
            {
                if (cards[i].Number >= 11)
                {
                    total += 10;
                }
                else
                {
                    total += cards[i].Number;

                    if (cards[i].Number == 1)
                        aces++;
                }
            }

            while (aces >= 1 && total <= 11)
            {
                aces--;
                total += 10;
            }

            return total;
        }

        public string ToMarkupString()
        {
            if (cards.Count >= 1)
                return $"{string.Join(" ", cards.Select(x => x.ToMarkupString()))} [grey46]= {GetTotalNumber()}[/]";
            else
                return "";
        }
    }
}
