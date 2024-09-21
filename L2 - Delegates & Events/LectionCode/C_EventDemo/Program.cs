class Account
{
    // Оскільки подіі використовують делегати, то ми спочатку оголошуємо тип делегату,
    // а далі використовуємо цей тип в оголошенні подій. Тобто наші подіі будуть адресувати
    // методи, які приймають рядок як параметр та ничого не повертають
    public delegate void AccountStateHandler(string message);

    public event AccountStateHandler Withdrawn;
    public event AccountStateHandler Added;

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
        if (Added != null)
            Added($"На рахунок внесено {sum}");
    }

    public void Withdraw(int sum)
    {
        if (sum <= _sum)
        {
            _sum -= sum;
            if (Withdrawn != null)
                Withdrawn($"Знято {sum} з рахунку");
        }
        else
        {
            if (Withdrawn != null)
                Withdrawn("Недостатньо коштів на рахунку");
        }
    }

    static void Main(string[] args)
    {
        Account account = new Account(200);

        account.Added += Show_Message;
        account.Withdrawn += Show_Message;
        account.Withdraw(100);

        account.Withdrawn -= Show_Message;
        account.Withdraw(50);  // повідомлення не буде
        account.Put(150);

        Console.ReadLine();
    }

    static void Show_Message(string message) {
        Console.WriteLine(message);
    }
}
