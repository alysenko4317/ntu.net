Console.WriteLine("Hello, World!");

double xMin = -5;
double xMax = 5;
double h = 0.05;

// Якщо ти хочеш зробити код більш універсальним та слабше зв'язаним, можна винести обчислення функції 
// f(x) у делегат, що дозволить змінювати логіку обчислення без змін основної структури коду.

for (double x = xMin; x <= xMax; x += h)
{
    double y = x * x * x;
    Console.WriteLine($"f({x:F2}) = {y:F2}");  // інтерполяція рядків (string interpolation)
}
