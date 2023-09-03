class Account
{
    public delegate void AccountStateHandler(string message);

    private int _sum;
    private AccountStateHandler _delegate;

    public Account(int sum) {
        _sum = sum;
    }

    public void RegisterHandler(AccountStateHandler d) {
        _delegate = d;
    }

    public int CurrentSum {
        get { return _sum; }
    }

    public void Put(int sum)
    {
        if (sum > 0) {
            _sum += sum;
            Console.WriteLine($"Successfully deposited {sum} units.");
        }
        else {
            Console.WriteLine("Invalid deposit amount. Please enter a positive number.");
        }
    }

    public void Withdraw(int sum)
    {
        if (sum <= _sum)
        {
            _sum -= sum;
            //if (_del != null)
            //    _del($"Знято {sum} одиниць з рахунку.");
            _delegate?.Invoke($"Successfully withdrew {sum} units.");
        }
        else
        {
            //if (_del != null)
            //    _del("Не вдалося зняти кошти з рахунку.");
            _delegate?.Invoke("Failed to withdraw funds.");
        }
    }

    static void Main(string[] args)
    {
        Account account = new Account(200); // Початковий баланс рахунку 200

        account.RegisterHandler(new Account.AccountStateHandler(ShowMessage));
        //account.RegisterHandler(ShowMessage);

        account.Withdraw(100); // Спроба зняти 100 одиниць
        account.Withdraw(150); // Спроба зняти 150 одиниць

        Console.ReadLine();
    }

    private static void ShowMessage(string message) {
        Console.WriteLine(message);
    }
}