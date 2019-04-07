# process-helper.net
A convenient and expandable library for launching other applications and processes from C# easily

# Usage example
```csharp
Using Tomis.Utilities;


string processPath = @"path\to\process";
string[] arguments = new[] {"-r", "argument"};

void SimpleExample()
{ 
        var executor = new ProcessExecutor(processPath)
        {
            WaitForExit = true,
        };
        executor.Execute();
}

void ExampleWithHandlers()
{
        var executor = new ProcessExecutor(processPath)
        {
            WaitForExit = true,
            StdoutHandler = (sender, e) => { Console.WriteLine(e.Data);},
            StderrHandler = StdErrHandler
        };
        executor.Mode = ProcessExecutor.RedirectionMode.UseHandlers;
        executor.Execute(); // Now stdout and stderr of process is handled by provided handlers
        
}

void StdErrHandler(object sender, DataReceivedEventArgs e)
{
  Console.WriteLine(e.Data);
}

void ExampleWithStreamRedirection()
{
        var executor = new ProcessExecutor(processPath)
        {
            WaitForExit = true
        };
        executor.Mode = ProcessExecutor.RedirectionMode.RedirectStreams;
        executor.Execute();
        Console.WriteLine("Captured stdout: \n" + executor.StdoutReader.ReadToEnd());
        Console.WriteLine("Captured stderr: \n" + executor.StderrReader.ReadToEnd());      
}
```

More examples in ProcessExecutorTester
