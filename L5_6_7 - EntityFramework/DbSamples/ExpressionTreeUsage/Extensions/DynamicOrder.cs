using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public static class DynamicOrder
{
    public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName)
    {
        // Створюємо параметр виразу (x)
        ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");

        // Створюємо вираз для отримання значення властивості (x.PropertyName)
        Expression propertyExpression = Expression.Property(parameterExpression, propertyName);

        // Створюємо лямбда-вираз для сортування (x => x.PropertyName)
        var resultExpression = Expression.Lambda(propertyExpression, parameterExpression);

        // Компілюємо лямбду в делегат Func<T, object>
        var lambda = resultExpression.Compile();

        // Використовуємо LINQ для сортування, використовуючи скомпільований делегат
        //return source.OrderBy(lambda); // але так як делегат lambda нетипізований, а orderby очікує типізований делегат Func<T,TKey>
        //
        //

        // ----------------------------------------------------------------------------
        // тому використаэмо механізм Reflection щоб викликти метод OrderBy дінамічно

        // Отримуємо тип IEnumerable (тому що OrderBy реалізован як extension method для класу Enumerable)
        Type enumerableType = typeof(Enumerable);

        // Отримуємо всі публічні статичні методи типу Enumerable
        var methods = enumerableType.GetMethods(BindingFlags.Public | BindingFlags.Static);

        // Фільтруємо методи для пошуку методу OrderBy з двома параметрами (спрощено, так як не контролюэмо типи цых двох параметрів)
        var selectedMethods = methods.Where(m => m.Name == "OrderBy" && m.GetParameters().Count() == 2);

        // Отримуємо перший метод, який підходить
        var method = selectedMethods.First();

        // але ми отримали метаопис узагальненого методу:  OrderBy<T>(IEnumerable<T> source, Func<T, TKey> keySelector)
        // і нам його потрібно параметризувати конкретними типами
        // тож
        // Конкретизуємо узагальнений метод OrderBy для типу T (елементи колекції) та типу властивості (TKey)
        method = method.MakeGenericMethod(typeof(T), propertyExpression.Type);

        // Викликаємо метод OrderBy з переданими параметрами (джерело даних і скомпільована лямбда)
        var result = (IEnumerable<T>) method.Invoke(
            null /* тому що метод статичний, об'єкту немає */,
            new object[] { source, lambda } /* перелік параметрів який ми передаємо - джерело даних та наша скомпільована лямбда */);

        return result;

    }
}
