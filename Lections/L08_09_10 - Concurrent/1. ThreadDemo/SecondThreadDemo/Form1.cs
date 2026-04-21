// Демонстрація 3.1 — Інкапсуляція керування потоком у класі BackgroundExecutor
//
// Порівняно з ThreadDemo.1/2/3 де потік створювався прямо у Form,
// тут логіка виконання перенесена у окремий клас BackgroundExecutor.
//
// Переваги такого підходу:
//   ✔ Form не знає деталей реалізації (не знає про Thread, Sleep, цикл)
//   ✔ BackgroundExecutor можна повторно використовувати в інших формах
//   ✔ Можна замінити реалізацію (Thread → Task) не змінюючи Form
//
// Проблеми цього прикладу (навмисно залишені для обговорення):
//   ✗ Немає способу зупинити або поставити на паузу виконання
//   ✗ Немає зворотного зв'язку (результат нікуди не повертається)
//   ✗ IsBackground не встановлено → якщо закрити форму, процес не завершиться
//     поки фоновий потік не закінчить роботу
//   → Рішення у наступних проектах (StartStopDemo_2, StopPauseDemo)

namespace SecondThreadDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            textBox1.Text = "Запущено...";

            BackgroundExecutor executor = new BackgroundExecutor();
            executor.Start();

            // Зверніть увагу: Start() повертається ОДРАЗУ.
            // Форма залишається вільною, але ми не знаємо коли закінчиться виконання.
            textBox1.Text = "Виконується у фоновому потоці (результат нікуди не повернеться)";
        }

        // ── Виконавець — інкапсулює потік та логіку роботи ───────────────────
        class BackgroundExecutor
        {
            private Thread? _thread;

            public void Start()
            {
                _thread = new Thread(Solve);
                // ⚠ IsBackground не встановлено (за замовчуванням false).
                // Це означає що процес не завершиться поки цей потік живий —
                // навіть якщо користувач закрив форму!
                // Виправлення: _thread.IsBackground = true;
                _thread.Start();
            }

            private void Solve()
            {
                decimal res = 0;
                for (long i = 0; i < 100_000_000; i++)
                    res += (decimal)Math.Sin(i);

                // Результат є, але немає механізму повернути його у форму.
                // Немає посилання на форму, немає Invoke, немає події.
                // → Дивись StartStopDemo_2 / StopPauseDemo для правильного підходу.
            }
        }
    }
}