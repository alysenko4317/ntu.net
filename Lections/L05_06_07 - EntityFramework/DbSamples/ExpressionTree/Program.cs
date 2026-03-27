using System;
using System.Linq.Expressions;

class Program
{
    static void Main(string[] args)
    {
        // Простий варіант використання лямбда-виразу
        Func<int, int, int> lambda = (x, y) => (x + y) * 2;
        Console.WriteLine(lambda(5, 4));

        // Визначення параметрів для лямбда-виразу
        ParameterExpression xParam = Expression.Parameter(typeof(int), "x");
        ParameterExpression yParam = Expression.Parameter(typeof(int), "y");

        // Створення постійної величини 2
        ConstantExpression constant = Expression.Constant(2);

        // Створення виразу для додавання x і y
        Expression sum = Expression.Add(xParam, yParam);

        // Створення виразу для множення результату суми на 2
        Expression mult = Expression.Multiply(sum, constant);

        // Створення лямбда-виразу
        LambdaExpression lambdaExpression = Expression.Lambda(mult, xParam, yParam);

        // Компіляція лямбда-виразу в делегат
        var newLambda = (Func<int, int, int>)lambdaExpression.Compile();

        // Виклик скомпільованого делегата
        Console.WriteLine(newLambda(5, 4));

        Console.ReadLine();
    }
}
