// Демонстрація 2.2 — Повернення результату з фонового потоку через Invoke.
//
// ЗРАЗОК 3 — Invoke через Application.OpenForms (працює, але погано)
//   Отримуємо посилання на форму через Application.OpenForms["Form1"].
//   Це «милиця»: статичний метод змушений знати ім'я класу форми,
//   залежить від колекції відкритих вікон, потребує явного cast-у.
//   Але принцип Invoke() — вірний: делегат виконається у UI-потоці.
//
// ЗРАЗОК 4 — Inline-лямбда + ті самі проблеми з OpenForms
//   Тіло потоку оформлено як лямбда-вираз (анонімний метод).
//   Зручніше читати, але досі використовується Application.OpenForms.
//
//   BRAINSTORM: чи можна замінити «form1.Invoke(...)» на просто «Invoke(...)»
//   і «form1.textBox1.Text» на «textBox1.Text» прямо в лямбді?
//   → Так! Лямбда — це замикання (closure): вона «захоплює» this форми
//     автоматично, бо оголошена всередині методу екземпляра форми.
//
// ЗРАЗОК 4 (спрощений) — Правильний підхід: замикання захоплює this
//   ✔ Не потрібен Application.OpenForms
//   ✔ Не потрібен явний cast
//   ✔ Код лаконічний і безпечний
//   Лямбда в середині методу форми неявно захоплює this →
//   this.Invoke() і this.textBox1 доступні напряму.

namespace OneThreadDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ── Зразок 3: статичний метод отримує форму через Application.OpenForms ─
        // Працює, але погано: статичний метод знає про існування "Form1"
        private void btnOpenFormsInvoke_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(BackgroundExecutorWithResult.Solve);
            t.IsBackground = true;
            t.Start();
            textBox1.Text = "Обчислення запущено у фоновому потоці...";
        }

        static class BackgroundExecutorWithResult
        {
            public static void Solve()
            {
                decimal res = 0;
                for (long i = 0; i < 20_000_000; i++)
                    res += (decimal)Math.Sin(i);

                // ❌ Так не можна — cross-thread exception:
                // ((Form1)Application.OpenForms["Form1"]!).textBox1.Text = res.ToString();

                // ✔ Через Invoke — делегат виконається у UI-потоці
                Form1 form1 = (Form1)Application.OpenForms["Form1"]!;
                form1.Invoke(new Action(() =>
                {
                    // Тепер ми у UI-потоці — можна звертатись до елементів форми
                    form1.textBox1.Text = $"[OpenForms] Результат: {res}";
                }));
            }
        }

        // ── Зразок 4: inline-лямбда, але досі через OpenForms ────────────────
        // Тіло потоку — прямо тут, зручніше читати.
        // Але лямбда все ще вручну шукає форму через OpenForms — зайве.
        private void btnInlineLambdaOpenForms_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                decimal res = 0;
                for (long i = 0; i < 20_000_000; i++)
                    res += (decimal)Math.Sin(i);

                // Лямбда захоплює this (форму) автоматично як замикання!
                // Але тут ми свідомо ігноруємо це і йдемо "в обхід" через OpenForms —
                // щоб показати студентам різницю з наступним зразком
                Form1 form1 = (Form1)Application.OpenForms["Form1"]!;
                form1.Invoke(new Action(() =>
                {
                    form1.textBox1.Text = $"[inline+OpenForms] Результат: {res}";
                }));
            });

            t.IsBackground = true;
            t.Start();
            textBox1.Text = "Обчислення запущено у фоновому потоці...";
        }

        // ── Зразок 4 (спрощений): замикання захоплює this — правильно! ───────
        // Лямбда оголошена в методі екземпляра форми, тому автоматично
        // «захоплює» this → Application.OpenForms більше не потрібен.
        // this.Invoke() і this.textBox1 доступні напряму — коротко та чисто.
        private void btnSimplifiedClosure_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                decimal res = 0;
                for (long i = 0; i < 20_000_000; i++)
                    res += (decimal)Math.Sin(i);

                // this захоплено замиканням — не потрібен Application.OpenForms
                Invoke(new Action(() =>
                {
                    textBox1.Text = $"[замикання] Результат: {res}";
                }));
            });

            t.IsBackground = true;
            t.Start();
            textBox1.Text = "Обчислення запущено у фоновому потоці...";
        }
    }
}