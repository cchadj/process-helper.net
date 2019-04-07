# process-helper.net
A convenient and expandable library for launching other applications and processes inside of C# easily

# Table of Contents

1. [Usage examples](#usage-examples)
2. [Using the test project](#Using the test project)
3. [Build using Visual Studio(VS) or Rider.](#Using the test project)
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

```

<aside class="notice">
WARNING 
</aside>
PythonExecutor assumes that python is in your PATH. To use a specific python executable or if python not in PATH use
` pythonExecutor.PythonExecutablePath = "path/to/python/python.exe`

# Using the test project
ProcessExecutorTester project provides all the library capabilities. 

*To build any project right click on project in sollution explorer -> build (in Visual Sutdio) or Build Selected Projects (in Rider)*  
To run the test:
1) Build testprocess project. (This project creates a small executable to run from ProcessExecutorTester)
2) Build ProcessExecutor. (This creates the dll in order to use ProcessExecutor from the tester )
3) Build and run ProcessExecutorTester.

You can change 
```csharp
        var processPath = @"..\..\..\testprocess\bin\Debug\testprocess.exe";
        var pythonScriptPath = @"..\..\simple.py";
        var arguments = new[] {"-f", "filename"};
```
to any values you like in order to test other application or other arguments.

# To build using Visual Studio(VS) or Rider.
1) Clone or download this repo
2) Open ProcessExecutor.sln (in VS or Rider)
3) Right click ProcessExecutor (Solution Explorer) -> build (VS) or Build Selected Projects (in Rider)
4) dll now resides in <clone-directory>\process-helper.net\ProcessExecutor\bin\Debug
6) Add dll as reference in your projects  
  
*You can also build as release*
