namespace OneThreadDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ── Зразок 5а: параметр через Thread.Start(object) ───────────────────
        // Старий підхід: один параметр, обов'язковий cast
        private void btnParameterizedThread_Click(object sender, EventArgs e)
        {
            const long iterationsCount = 20_000_000;

            Thread t = new Thread((parameterObject) =>
            {
                // Параметр приходить як object — треба явно привести до типу
                long iterations = Convert.ToInt64(parameterObject);

                decimal res = 0;
                for (long i = 0; i < iterations; i++)
                    res += (decimal)Math.Sin(i);

                // Замикання захоплює this → Invoke() доступний напряму
                Invoke(() => textBox1.Text = $"[Start(param)] Результат: {res}");
            });

            t.IsBackground = true;
            t.Start(iterationsCount);  // ← передаємо параметр через Start()
            textBox1.Text = "Обчислення запущено (параметр через Thread.Start)...";
        }

        // ── Зразок 5б: параметр через замикання — сучасний підхід ───────────
        // Жодного object, жодного cast — повний type safety
        private void btnClosureParameter_Click(object sender, EventArgs e)
        {
            const long iterationsCount = 20_000_000;

            // iterationsCount захоплено замиканням — доступне прямо в лямбді
            Thread t = new Thread(() =>
            {
                decimal res = 0;
                for (long i = 0; i < iterationsCount; i++)  // ← звертаємось напряму
                    res += (decimal)Math.Sin(i);

                Invoke(() => textBox1.Text = $"[замикання] Результат: {res}");
            });

            t.IsBackground = true;
            t.Start();  // ← параметрів не потрібно, все є у замиканні
            textBox1.Text = "Обчислення запущено (параметр через замикання)...";
        }
    }
}
