namespace MathSolverClassLibrary
{
    public class MathSolverInitializeEventArgs : EventArgs {
        public double xmin { get; private set; }
        public double xmax { get; private set; }
        public double step { get; private set; }
        public MathSolverInitializeEventArgs(double xmin, double xmax, double step)
        {
            this.xmin = xmin;
            this.xmax = xmax;
            this.step = step;
        }
    }

    public class MathSolverProcessingFinishedEventArgs : EventArgs {
        public double result { get; private set; }
        public MathSolverProcessingFinishedEventArgs(double r) {
            result = r;
        }
    }

    public class MathSolver
    {
        public double _xmin { get; private set; }
        public double _xmax { get; private set; }
        public double _step { get; private set; }

        public event EventHandler<MathSolverInitializeEventArgs> InitializeEvent;
        public event EventHandler<EventArgs> ProcessingStartedEvent;
        public event EventHandler<MathSolverProcessingFinishedEventArgs> ProcessingFinishedEvent;

        protected void OnInitializeEvent(double xmin, double xmax, double step) {
            if (InitializeEvent != null)
                InitializeEvent(this, new MathSolverInitializeEventArgs(xmin, xmax, step));
        }

        protected void OnProcessingStartedEvent() {
            if (ProcessingStartedEvent != null)
                ProcessingStartedEvent(this, EventArgs.Empty);
        }

        protected void OnProcessingFinishedEvent(double result) {
            if (ProcessingFinishedEvent != null)
                ProcessingFinishedEvent(this, new MathSolverProcessingFinishedEventArgs(result));
        }

        public MathSolver(double xmin, double xmax, double step)
        {
            _xmin = xmin;
            _xmax = xmax;
            _step = step;
        }

        public delegate double OneArgumentFunction(double x);

        //public double Solve(OneArgumentFunction fn)
        public double Solve(Func<double, double> fn)  // останній параметр - тип даних результату
        {
            OnInitializeEvent(_xmin, _xmax, _step);
            OnProcessingStartedEvent();

            double sum = 0;
            for (double x = _xmin; x <= _xmax; x += _step) { 
                sum += fn(x);
            }

            OnProcessingFinishedEvent(sum);
            return sum;
        }

    }
}