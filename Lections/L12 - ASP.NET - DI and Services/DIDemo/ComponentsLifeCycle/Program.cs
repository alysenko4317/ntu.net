
namespace MyNamespace // Change this to your actual namespace
{
    // ------------------------------------------------------------------
    // ICounter
    // ------------------------------------------------------------------

    public interface ICounter {
        int Value { get; }
    }

    public class RandomCounter : ICounter
    {
        static Random rnd = new Random();

        private int _value;

        public RandomCounter() {
            _value = rnd.Next(0, 1000000);
        }

        public int Value {
            get => _value;
        }
    }

    // ------------------------------------------------------------------
    // CounterService
    // ------------------------------------------------------------------

    public class CounterService
    {
        public ICounter Counter { get; }

        public CounterService(ICounter counter) {
            Counter = counter;
        }
    }

    // ------------------------------------------------------------------
    // CounterMiddleware
    // ------------------------------------------------------------------

    public class CounterMiddleware // singletone
    {
        RequestDelegate next;
        ICounter counter;
        CounterService counterService;

        int i = 0; // лічильник запитів

        public CounterMiddleware(RequestDelegate next, ICounter counter, CounterService counterService) {
            this.next = next;
            this.counter = counter;
            this.counterService = counterService;
        }

        public async Task InvokeAsync(HttpContext httpContext/*, ICounter counter, CounterService counterService*/)
        {
            i++;
            httpContext.Response.ContentType = "text/html;charset=utf-8";
            await httpContext.Response.WriteAsync(
                $"Запит {i}; Counter: {counter.Value}; Service: {counterService.Counter.Value}");
        }
    }

    // ------------------------------------------------------------------
    // Main
    // ------------------------------------------------------------------

    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddTransient<ICounter, RandomCounter>();  // AddScoped
            builder.Services.AddTransient<CounterService>();

            var app = builder.Build();

            app.UseMiddleware<CounterMiddleware>();

            app.Run();

        }
    }
}
