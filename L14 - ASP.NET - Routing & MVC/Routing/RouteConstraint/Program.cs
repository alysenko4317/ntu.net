
namespace App
{
    // ----------------------------------------------------------------
    // Sample 1
    // ----------------------------------------------------------------

    public class SecretCodeConstraint : IRouteConstraint
    {
        string secretCode;    // дозволений код

        public SecretCodeConstraint(string secretCode) { 
            this.secretCode = secretCode;
        }

        public bool Match(
            HttpContext? httpContext,
            IRouter? route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection
        ) {
            return values[routeKey]?.ToString() == secretCode;
        }
    }

    // ----------------------------------------------------------------
    // Sample 2
    // ----------------------------------------------------------------

    public class InvalidNamesConstraint : IRouteConstraint
    {
        string[] names = new[] { "Tom", "Sam", "Bob" };

        public bool Match(
            HttpContext? httpContext,
            IRouter? route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection
        ) {
            return !names.Contains(values[routeKey]?.ToString());
        }
    }

    // ----------------------------------------------------------------
    // MAIN
    // ----------------------------------------------------------------

    class Application
    {
        static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            // проектуємо класс SecretCodeConstraint на inline-обмеження secretcode
            builder.Services.Configure<RouteOptions>(options =>
               options.ConstraintMap.Add("secretcode", typeof(SecretCodeConstraint)));

            // альтернативне додавання класу обмеження
            // builder.Services.AddRouting(options => options.ConstraintMap.Add("secretcode", typeof(SecretConstraint)));

            var app = builder.Build();

            app.Map(
               "/users/{name}/{token:secretcode(123466)}/",
               (string name, int token) => $"Name: {name} \nToken: {token}"
            );

            app.Map("/", () => "Index Page");

            app.Run();

        }

        static void Main_2()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddRouting(options =>
               options.ConstraintMap.Add("invalidnames", typeof(InvalidNamesConstraint)));

            var app = builder.Build();

            app.Map("/users/{name:invalidnames}", (string name) => $"Name: {name}");
            app.Map("/", () => "Index Page");

            app.Run();
        }
    }
}
