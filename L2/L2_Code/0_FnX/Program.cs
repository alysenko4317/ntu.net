Console.WriteLine("Hello, World!");

double xMin = -5;
double xMax = 5;
double h = 0.05;

for (double x = xMin; x <= xMax; x += h)
{
    double y = x * x * x;
    Console.WriteLine($"f({x:F2}) = {y:F2}");
}
