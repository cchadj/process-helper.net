# process-helper.net
A convenient and expandable library for launching other applications and processes from C# easily

# Usage examples
## ProcessExecutor ( Execute any application easily )
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

## PythonExecutor ( Execute any python script easily )

Just like ProcessExecutor but use python scripts instead.

```csharp
Using Tomis.Utilities;

string scriptPath = @"path\to\script.py";
string[] arguments = new[] {"-r", "argument"};

void SimpleExample()
{ 
        //========= Executing a python script using PythonScriptExecutor =============
        var pythonExecutor = new PythonScriptExecutor(pythonScriptPath)
        {
            Mode = ProcessExecutor.RedirectionMode.RedirectStreams,
            WaitForExit = true,
            Args = arguments
        };
        pythonExecutor.Execute();
}
More examples in ProcessExecutorTester
```

<aside class="notice">
WARNING 
</aside>
PythonExecutor assumes that python is in your PATH. To use a specific python executable or if python not in PATH use
`` pythonExecutor.PythonExecutablePath = "path/to/python/python.exe``

# To build using VS2017.
1) Open sln
2) Build ProcessExecutor poject
3) Get dll from bin/Debug (or release)
4) Add dll as reference in your projects
