using System;

class AccountEventArgs : EventArgs
{
    public string Message { get; }
    public int Sum { get; }

    public AccountEventArgs(string mes, int sum)
    {
        Message = mes;
        Sum = sum;
    }
}

class Account
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);

    public event AccountStateHandler Withdrawn;
    public event AccountStateHandler Added;

    //public event EventHandler<AccountEventArgs> Withdrawn;
    //public event EventHandler<AccountEventArgs> Added;

    private int _sum;

    public Account(int sum) {
        _sum = sum;
    }

    public int CurrentSum {
        get { return _sum; }
    }

    public void Put(int sum)
    {
        _sum += sum;
        Added?.Invoke(this, new AccountEventArgs($"Ha paxyнок зараховано {sum}", sum));
    }

    public void Withdraw(int sum)
    {
        if (_sum >= sum)
        {
            _sum -= sum;
            Withdrawn?.Invoke(this, new AccountEventArgs($"Cymy {sum} знято з рахунку", sum));
        }
        else
        {
            Withdrawn?.Invoke(this, new AccountEventArgs("Недостатньо коштів на рахунку", sum));
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Account account = new Account(200);
        account.Added += Show_Message;
        account.Withdrawn += Show_Message;

        account.Withdraw(100);
        account.Withdrawn -= Show_Message;

        account.Withdraw(50);
        account.Put(150);
    }

    private static void Show_Message(object sender, AccountEventArgs e)
    {
        Console.WriteLine($"Cyma транзакціі: {e.Sum}");
        Console.WriteLine(e.Message);
    }
}