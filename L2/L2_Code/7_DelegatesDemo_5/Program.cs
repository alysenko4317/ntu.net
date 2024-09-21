class Account
{
    public delegate void AccountStateHandler(string message);

    private int _sum;
    private AccountStateHandler _listeners;

    public Account(int sum) {
        _sum = sum;
    }

    // використання мультікаст-делегатів

    public void RegisterHandler(AccountStateHandler del) {
        //_listeners = System.Delegate.Combine(_listeners, del) as AccountStateHandler;
        _listeners = _listeners + del;
    }

    public void UnregisterHandler(AccountStateHandler del) {
        //_listeners = System.Delegate.Remove(_listeners, del) as AccountStateHandler;
        _listeners -= del;
    }

    public int CurrentSum {
        get { return _sum; }
    }

    public void Put(int sum)
    {
        if (sum > 0)
        {
            _sum += sum;
            Console.WriteLine($"Successfully deposited {sum} units.");
        }
        else
        {
            Console.WriteLine("Invalid deposit amount. Please enter a positive number.");
        }
    }

    public void Withdraw(int sum)
    {
        if (sum <= _sum)
        {
            _sum -= sum;
            //if (_listeners != null)
            //    _listeners($"Знято {sum} одиниць з рахунку.");
            _listeners?.Invoke($"Successfully withdrew {sum} units.");
        }
        else
        {
            //if (_listeners != null)
            //    _listeners("Не вдалося зняти кошти з рахунку.");
            _listeners?.Invoke("Failed to withdraw funds.");
        }
    }

    static void Main(string[] args)
    {
        Account account = new Account(200);

        Account.AccountStateHandler colorDelegate = new Account.AccountStateHandler(ColorMessage);
        account.RegisterHandler(new Account.AccountStateHandler(ShowMessage));
        account.RegisterHandler(colorDelegate);

        account.Withdraw(100);
        Console.WriteLine(Environment.NewLine);

        account.Withdraw(150);
        Console.WriteLine(Environment.NewLine);

        account.UnregisterHandler(colorDelegate);
        account.Withdraw(50);

        Console.ReadLine();
    }

    private static void ShowMessage(string message) {
        Console.WriteLine(message);
    }

    private static void ColorMessage(string message) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
