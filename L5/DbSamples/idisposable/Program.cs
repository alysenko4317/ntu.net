
// The IDisposable interface is a central concept in .NET for managing and releasing UNMANAGED resources.
//
// Let's break down the key points:
//
// Purpose of IDisposable:
//     The primary purpose of IDisposable is to provide a standard mechanism to release unmanaged resources.
//     Unmanaged resources can be things like file handles, database connections, network sockets, or native memory.
//     Managed resources, on the other hand, are managed by the .NET runtime and garbage collector.
//
// Dispose Method:
//     The IDisposable interface defines a single method:
//         Dispose().
//     When you implement this interface, you are required to provide an implementation for this method.
//     The Dispose method is called to release or reset unmanaged resources.
//
// Using Statement:
//     One of the most common ways to utilize the IDisposable interface is with the --using-- statement in C#.
//     Any object instantiated within a --using-- block will have its Dispose method automatically called when
//     the block is exited, ensuring that resources are cleaned up promptly.
//
// Common Use Cases:
//     Many classes in the .NET Framework implement IDisposable. Common examples include streams (Stream, FileStream,
//     MemoryStream), graphics objects(Graphics, Bitmap), and various other resource-heavy objects like SqlConnection.
//
// =========== *** ===========
//
// The using statement in C# is often referred to as a "using statement" or "using directive,"
// but in the context of resource management and IDisposable, it's more specifically called
// a "using declaration" or "using block." When it's utilized to manage resources, it's sometimes
// referred to as a "disposable context" or "disposable pattern."
//
// However, the term "context manager" is more commonly associated with Python's --with-- statement,
// which serves a similar purpose to C#'s using statement in terms of resource management.
//
// In essence, the --using-- block in C# provides a syntactical convenience to ensure that Dispose
// is called on an object implementing IDisposable, which is especially useful for resource management.


namespace app
{
    class Program
    {
        public class Person : IDisposable
        {
            public string Name { get; }

            // constructor
            public Person(string name) => Name = name;

            // destructor (Finalizer)
            ~Person() {
                Console.WriteLine($"{Name} is being finalized");
            }

            public void Dispose() {
                Console.WriteLine($"{Name} has been disposed");

                // It's a best practice to suppress finalization when Dispose is called to prevent the finalizer
                // from running since the object has already been cleaned up.
                GC.SuppressFinalize(this);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("main entered");

            using (Person tom = new Person("Tom"))
            {
                // tom object is accesible only under using-block
                Console.WriteLine($"Name: {tom.Name}");
            }

            Console.WriteLine("other code execution...");
        }
    }
}
