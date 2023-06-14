using Spectre.Console;

namespace SoloBlackjack
{
    public class Card
    {
        public Symbols Symbol { get; private set; }
        public int Number { get; private set; }

        public Card(Symbols symbol, int number)
        {
            Symbol = symbol;
            Number = number;
        }

        public string ToMarkupString()
        {
            var symbol = Symbol switch
            {
                Symbols.Spade => "♠",
                Symbols.Clover => "♣",
                Symbols.Diamond => "◆",
                Symbols.Heart => "♥",
                _ => throw new Exception(),
            };

            var number = Number switch
            {
                1 => "Ａ",
                2 => "２",
                3 => "３",
                4 => "４",
                5 => "５",
                6 => "６",
                7 => "７",
                8 => "８",
                9 => "９",
                11 => "Ｊ",
                12 => "Ｑ",
                13 => "Ｋ",
                _ => Number.ToString(),
            };

            var color = Symbol switch
            {
                Symbols.Spade or Symbols.Clover => "white",
                Symbols.Diamond or Symbols.Heart => "red",
                _ => throw new Exception(),
            };

            return $"[{color}]{symbol}{number}[/]";
        }
    }
}
