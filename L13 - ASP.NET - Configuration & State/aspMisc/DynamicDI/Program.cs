
// In C#, to achieve a similar level of flexibility as Java's XML-based configuration in Spring,
//    where you can add a new class and specify it in a configuration file without recompiling
//    the entire application, you can use reflection along with a configuration file.
//    Reflection allows you to create instances of types at runtime, based on their names
//    as strings in the configuration file.
//
// Here's a more detailed example of how you could set this up in an ASP.NET Core application:

using System.Reflection;

namespace MyApp
{
    public class Services
    {
        public interface IService {
            String Execute();
        }

        // Service implementations could be in separate libraries that you load.
        public class ServiceA : IService {
            public String Execute() {
               return "Service A executed.";
            }
        }

        public class ServiceB : IService {
            public String Execute() {
                return "Service B executed.";
            }
        }
    }

    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("dicfg.json");
            
            var assemblyName = Assembly.GetAssembly(typeof(Services.ServiceA)).GetName().Name;
            var fullTypeName = typeof(MyApp.Services.ServiceA).FullName;

            var implTypeName= builder.Configuration.GetSection("ServiceImplementation").Value;

            var serviceType = Type.GetType(implTypeName);  // Load the type specified in the configuration
            if (serviceType != null && typeof(Services.IService).IsAssignableFrom(serviceType)) {
                builder.Services.AddSingleton(typeof(Services.IService), serviceType);
            }
            else
                throw new InvalidOperationException("The specified service type is invalid or not found.");

            var app = builder.Build();

            // option 3 - DI usage
            app.Map("/", (Services.IService svc) => {
                return Results.Content(svc.Execute(), "text/html");
            });

            app.Run();
        }
    }
}

// Adding a new implementation, like ServiceC, to your application without rebuilding the entire
//    project involves a few steps, especially since your application is designed to load services
//    dynamically using configuration files and reflection. Here's a general outline of the process you can follow:
//
//    Implement the New Service:
//        Create a new class, ServiceC, implementing the IService interface in an appropriate project.
//        You can place it in a separate project (library), and you will need to compile this into a DLL.
//        Make sure this DLL is accessible to your main application, either by placing it in the same directory
//        as your main application's executable or in a known location.
//
//    Update the Configuration File:
//        Modify the configuration file (dicfg.json) to refer to your new service. This will involve changing
//        the ServiceImplementation value to something like "MyApp.Services.ServiceC, AssemblyName" where
//        AssemblyName is the name of the assembly containing ServiceC.
//
//    Ensure Dynamic Loading:
//        Your application should be able to load the new implementation dynamically at runtime, based on the
//        configuration. This means when you change the configuration file, your application should use the
//        new ServiceC implementation.