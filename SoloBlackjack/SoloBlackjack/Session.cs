namespace SoloBlackjack
{
    public class Session
    {
        public enum States
        {
            Ready,
            PlayerPhase,
            DealerPhase,
            Result,
        }

        public enum Results
        {
            Draw,
            Win,
            Lose,
        }

        public Deck Deck { get; private set; } = new Deck();
        public Player Dealer { get; private set; } = new Player();
        public Player Player { get; private set; } = new Player();
        public States State { get; private set; } = States.Ready;

        public Session()
        {
            Dealer.GetFromDeck(Deck);
        }

        public void Start()
        {
            if (State != States.Ready)
                throw new Exception($"{nameof(State)}가 {States.Ready}일 때에만 호출 가능합니다.");

            Player.GetFromDeck(Deck, 2);

            var totalNumber = Player.GetTotalNumber();

            if (totalNumber == 21)
                State = States.DealerPhase;
            else
                State = States.PlayerPhase;
        }

        public void Hit()
        {
            if (State != States.PlayerPhase)
                throw new Exception($"{nameof(State)}가 {States.PlayerPhase}일 때에만 호출 가능합니다.");

            Player.GetFromDeck(Deck);

            var totalNumber = Player.GetTotalNumber();

            if (totalNumber == 21)
                State = States.DealerPhase;
            else if (totalNumber > 21)
                State = States.Result;
        }

        public void Stay()
        {
            if (State != States.PlayerPhase)
                throw new Exception($"{nameof(State)}가 {States.PlayerPhase}일 때에만 호출 가능합니다.");

            State = States.DealerPhase;
        }

        public void DoDealerStep()
        {
            if (State != States.DealerPhase)
                throw new Exception($"{nameof(State)}가 {States.DealerPhase}일 때에만 호출 가능합니다.");

            if (Dealer.GetTotalNumber() < 17)
                Dealer.GetFromDeck(Deck);
            else
                State = States.Result;
        }

        public Results GetResult()
        {
            if (State != States.Result)
                throw new Exception($"{nameof(State)}가 {States.Result}일 때에만 호출 가능합니다.");

            var playerNumber = Player.GetTotalNumber();
            var dealerNumber = Dealer.GetTotalNumber();

            if (playerNumber > 21)
                return Results.Lose;
            else if (dealerNumber > 21)
                return Results.Win;
            else if (playerNumber < dealerNumber)
                return Results.Lose;
            else if (playerNumber > dealerNumber)
                return Results.Win;

            return Results.Draw;
        }
    }
}
