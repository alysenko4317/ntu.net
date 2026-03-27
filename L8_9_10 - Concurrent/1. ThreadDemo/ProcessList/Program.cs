using System.Diagnostics;

var processes = Process.GetProcesses();
foreach (var process in processes)
{
    Console.WriteLine($"{process.Id}:{process.ProcessName}");
}

//var p = Process.GetProcessById(1212);
//p?.Kill();
