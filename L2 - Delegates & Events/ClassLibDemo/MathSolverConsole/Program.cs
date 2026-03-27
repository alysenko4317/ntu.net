// See https://aka.ms/new-console-template for more information

using MathSolverClassLibrary;

double demoFunc(double x) {
    return x * x;
}

Console.WriteLine("Hello, World!");

MathSolver solver = new MathSolver(-1, 1, 0.2);

solver.InitializeEvent += (sender, args) => {
    Console.WriteLine($"xmin={args.xmin} xmax={args.xmax} step={args.step}");
};

solver.ProcessingStartedEvent += (sender, args) => {
    Console.WriteLine("ProcessingStartedEvent received");
};

//double result = solver.Solve(demoFunc);
double result = solver.Solve(x => x * x);

Console.WriteLine(result);