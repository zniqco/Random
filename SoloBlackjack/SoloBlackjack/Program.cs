using Spectre.Console;
using Spectre.Console.Rendering;

namespace SoloBlackjack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var myMoney = 3000;

            while (true)
            {
                var session = new Session();
                var bet = default(int);

                while (true)
                {
                    AnsiConsole.Clear();

                    AnsiConsole.Write(
                        new Grid()
                            .AddColumn()
                            .AddRow(
                                new Grid()
                                .AddColumns(2)
                                .AddRow(new IRenderable[]
                                {
                                    new Text("남은 돈", new Style(Color.Aquamarine1)),
                                    new Text(myMoney.ToString())
                                })
                                .AddRow(new IRenderable[]
                                {
                                    new Text("딜러", new Style(Color.Yellow)),
                                    new Markup(session.Dealer.ToMarkupString())
                                })
                                .AddRow(new IRenderable[]
                                {
                                    new Text("나", new Style(Color.Yellow)),
                                    new Markup(session.Player.ToMarkupString())
                                })));

                    AnsiConsole.WriteLine();

                    if (session.State == Session.States.Ready)
                    {
                        bet = AnsiConsole.Prompt(
                            new TextPrompt<int>("얼마를 베팅할까요?")
                                .PromptStyle("blue")
                                .ValidationErrorMessage("[red]올바른 값이 아닙니다.[/]")
                                .Validate(age =>
                                {
                                    return age switch
                                    {
                                        < 1 => ValidationResult.Error("[red]베팅 금액이 너무 적습니다.[/]"),
                                        _ when age > myMoney => ValidationResult.Error("[red]베팅 금액이 내가 가진 돈보다 많습니다.[/]"),
                                        _ => ValidationResult.Success(),
                                    };
                                }));

                        myMoney -= bet;

                        session.Start();
                    }
                    else if (session.State == Session.States.PlayerPhase)
                    {
                        var nextMove = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .AddChoices(new[] { "Hit", "Stay" }));

                        if (nextMove == "Hit")
                            session.Hit();
                        else if (nextMove == "Stay")
                            session.Stay();
                    }
                    else if (session.State == Session.States.DealerPhase)
                    {
                        Thread.Sleep(1000);

                        session.DoDealerStep();
                    }
                    else if (session.State == Session.States.Result)
                    {
                        var result = session.GetResult();

                        if (result == Session.Results.Win)
                        {
                            AnsiConsole.Write(new Text("플레이어가 이겼습니다!", new Style(Color.Green1)));

                            myMoney += bet * 2;
                        }
                        else if (result == Session.Results.Lose)
                        {
                            AnsiConsole.Write(new Text("플레이어가 졌습니다...", new Style(Color.Red)));
                        }
                        else
                        {
                            AnsiConsole.Write(new Text("무승부입니다.", new Style(Color.Yellow2)));

                            myMoney += bet;
                        }

                        Thread.Sleep(1000);

                        Console.ReadKey();

                        break;
                    }
                }

                if (myMoney <= 0)
                {
                    AnsiConsole.Write(new Text("돈이 다 떨어져서 게임을 종료합니다.", new Style(Color.Red)));

                    break;
                }
            }
        }
    }
}
