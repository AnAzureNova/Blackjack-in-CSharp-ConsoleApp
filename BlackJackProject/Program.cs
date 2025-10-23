using System.Diagnostics;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        File.WriteAllText("session.txt", string.Empty);
        File.WriteAllText("sessionLog.txt", string.Empty);
        int dealerAiLimit = 16;
        int bet = 0;
        int bal = 100;
        int maxVal = 21;
        Console.WriteLine("Welcome to BlackJack!");
        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Starting balance: {bal}$");
        Console.ResetColor();
        Console.WriteLine("\nPress any key to start the game...");
        Console.ReadKey();
        Console.Clear();
        for (int i = 0; i < 3; i++)
        {
            Console.Write(".");
            Thread.Sleep(500);
        }
        BetSystem(bal, maxVal, bet, dealerAiLimit);
    }
    static void BetSystem(int bal, int maxVal, int bet, int dealerAiLimit)
    {
        Console.Clear();
        Console.CursorVisible = true;
        if (bal == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        Console.WriteLine($"Balance: {bal}$");
        Console.ResetColor();
        if (bal == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nYou don't have any money left!");
            Console.ResetColor();
            Console.ReadKey();
            File.AppendAllText("session.txt", "--Session ended--\n\nScore: Went broke");
            File.AppendAllText("sessionLog.txt", "Went broke\n");
            Process.Start(new ProcessStartInfo("notepad.exe", "session.txt"));
            Process.Start(new ProcessStartInfo("notepad.exe", "sessionLog.txt"));
            Environment.Exit(0);
        }
        Console.SetCursorPosition(0, 5);
        Console.WriteLine("(Press ESC to exit the game)");
        Console.SetCursorPosition(0, 2);
        Console.Write("Enter your bet: ");

        var input = ReadLineWithEscape();
        if (input == null)
        {
            File.AppendAllText("session.txt", $"--Session ended--\n\nScore: {bal}$ ");
            File.AppendAllText("sessionLog.txt", $"{bal}$\n");
            Process.Start(new ProcessStartInfo("notepad.exe", "session.txt"));
            Process.Start(new ProcessStartInfo("notepad.exe", "sessionLog.txt"));
            Environment.Exit(0);
        }

        if (!int.TryParse(input, out bet))
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input! Please enter a valid number.");
            Console.ResetColor();
            Thread.Sleep(1000);
            BetSystem(bal, maxVal, bet, dealerAiLimit);
            return;
        }

        if (bet > bal)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You don't have enough money!");
            Console.ResetColor();
            Thread.Sleep(1000);
            BetSystem(bal, maxVal, bet, dealerAiLimit);
        }
        else if (bet == 0)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Oooooo broke boy~");
            Console.ResetColor();
            Thread.Sleep(1000);
            BetSystem(bal, maxVal, bet, dealerAiLimit);
        }
        else if (bet < 0)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't bet negative money!");
            Console.ResetColor();
            Thread.Sleep(1000);
            BetSystem(bal, maxVal, bet, dealerAiLimit);
        }
        else
        {
            Console.CursorVisible = false;
            bal = bal - bet;
            Thread.Sleep(300);
            Console.Clear();
            MainGame(bal, maxVal, bet, dealerAiLimit);
        }
    }
    static string ReadLineWithEscape()
    {
        StringBuilder input = new StringBuilder();
        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                return null;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                input.Append(key.KeyChar);
                Console.Write(key.KeyChar);
            }
        }
        return input.ToString();
    }
    static void MainGame(int bal, int maxVal, int bet, int dealerAiLimit)
    {
        int playerCardCount = 2;
        int dealerCardCount = 2;
        Console.ResetColor();
        int sumDealer = 0, sumPlayer = 0;
        int[] dealer = new int[50];
        int[] player = new int[50];
        Random random = new();
        for (int i = 0; i < 50; i++)
        {
            dealer[i] = random.Next(1, 12);
            player[i] = random.Next(1, 12);
        }
        sumDealer = dealer[0] + dealer[1];
        sumPlayer = player[0] + player[1];
        Console.Clear();
        Console.CursorVisible = true;
        while (true)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Dealer's cards: {dealer[0]} + ???");
            Console.Write($" ?  {dealer[1],2}");
            for (int i = 2; i < dealerCardCount; i++)
            {
                Console.Write($"  {dealer[i],2}");
            }
            Console.WriteLine();
            Console.WriteLine($"Player's cards: {sumPlayer}");
            for (int i = 0; i < playerCardCount; i++)
            {
                Console.Write($"{player[i],2}  ");
            }
            Console.WriteLine();
            if (sumPlayer == 21)
            {
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nBLACKJACK");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                WinCore(bal, maxVal, bet, dealerAiLimit);
            }
            string choice;
            do
            {
                Console.SetCursorPosition(0, 6);
                Console.Write("                             ");
                Console.SetCursorPosition(0, 5);
                Console.Write("                             ");
                Console.SetCursorPosition(0, 5);
                Console.WriteLine("Hit or Stand?");
                choice = Console.ReadLine()?.Trim().ToLower();
            } while (choice != "hit" && choice != "stand");

            if (choice == "hit")
            {
                player[playerCardCount] = random.Next(1, 12);
                sumPlayer += player[playerCardCount];
                playerCardCount++;
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Dealer's cards: {dealer[0]} + ???");
                Console.Write($" ?  {dealer[1],2}");
                for (int i = 2; i < dealerCardCount; i++)
                {
                    Console.Write($"  {dealer[i],2}");
                }
                Console.WriteLine();
                Console.WriteLine($"Player's cards: {sumPlayer}");
                for (int i = 0; i < playerCardCount; i++)
                {
                    Console.Write($"{player[i],2}  ");
                }
                Console.WriteLine();
                if (sumPlayer > maxVal)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nBUST");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    LossCore(bal, maxVal, bet, dealerAiLimit);
                }
                else if (sumPlayer == maxVal)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nBLACKJACK");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    WinCore(bal, maxVal, bet, dealerAiLimit);
                }
            }
            else
            {
                while (sumDealer < dealerAiLimit)
                {
                    dealer[dealerCardCount] = random.Next(1, 12);
                    sumDealer += dealer[dealerCardCount];
                    dealerCardCount++;
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"Dealer's cards: {dealer[0]} + ???");
                    Console.Write($" ?  {dealer[1],2}");
                    for (int i = 2; i < dealerCardCount; i++)
                    {
                        Console.Write($"  {dealer[i],2}");
                    }
                    Console.WriteLine();
                    Console.WriteLine($"Player's cards: {sumPlayer}");
                    for (int i = 0; i < playerCardCount; i++)
                    {
                        Console.Write($"{player[i],2}  ");
                    }
                    Console.WriteLine();
                    Thread.Sleep(500);
                }
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Dealer's cards: {sumDealer}");
                for (int i = 0; i < dealerCardCount; i++)
                {
                    Console.Write($"{dealer[i],2}  ");
                }
                Console.WriteLine();
                Console.WriteLine($"Player's cards: {sumPlayer}");
                for (int i = 0; i < playerCardCount; i++)
                {
                    Console.Write($"{player[i],2}  ");
                }
                Console.WriteLine();
                if (sumDealer > 21)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nYOU WIN");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    WinCore(bal, maxVal, bet, dealerAiLimit);
                }
                else if (sumPlayer < sumDealer)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nDEALER WINS");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    LossCore(bal, maxVal, bet, dealerAiLimit);
                }
                else if (sumPlayer > sumDealer)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nYOU WIN");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    WinCore(bal, maxVal, bet, dealerAiLimit);
                }
                else if (sumPlayer == sumDealer)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nDRAW");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    DrawCore(bal, maxVal, bet, dealerAiLimit);
                }
            }
        }
    }
    static void WinCore(int bal, int maxVal, int bet, int dealerAiLimit)
    {
        Console.CursorVisible = false;
        Console.Clear();
        bal = bal + bet * 2;
        File.AppendAllText("sessionLog.txt", $"Win: {bal}$\n");
        BetSystem(bal, maxVal, bet, dealerAiLimit);
    }
    static void DrawCore(int bal, int maxVal, int bet, int dealerAiLimit)
    {
        Console.CursorVisible = false;
        Console.Clear();
        bal = bal + bet;
        File.AppendAllText("sessionLog.txt", $"Draw: {bal}$\n");
        BetSystem(bal, maxVal, bet, dealerAiLimit);
    }
    static void LossCore(int bal, int maxVal, int bet, int dealerAiLimit)
    {
        Console.CursorVisible = false;
        Console.Clear();
        File.AppendAllText("sessionLog.txt", $"Loss: {bal}$\n");
        BetSystem(bal, maxVal, bet, dealerAiLimit);
    }
}
