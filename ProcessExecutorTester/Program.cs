using System;
using System.Diagnostics;
using System.IO;
using Tomis.Utils;

public static class Program
{
    static int Main(string[] args)
    {
        Console.WriteLine("Executing process");
        var processPath = @"..\..\..\testprocess\bin\Debug\testprocess.exe";
        var pythonScriptPath = @"..\..\simple.py";
        
        var arguments = new[] {"-f", "filename"};
        StreamReader stdout;
        StreamReader stderr;
        StreamReader outReader;
        
        #region ProcessExecutor static methods testing

        Console.WriteLine("========= Simple execution ============= (No output in this console) ");
        ProcessExecutor.ExecuteProcess(processPath, true, arguments);

        
        Console.WriteLine("========= Capturing bot stdout and stderr =============");
        var p = ProcessExecutor.ExecuteProcess(processPath, out stdout, out stderr, true, arguments);
        Console.WriteLine("Captured stdout: \n" + stdout.ReadToEnd());
        Console.WriteLine("Captured stderr: \n" + stderr.ReadToEnd());

        
        Console.WriteLine("========= Capturing only stdout =============");

        p = ProcessExecutor.ExecuteProcess(processPath, true, out outReader, true, arguments);
        Console.WriteLine("Captured only stdout: \n" + outReader.ReadToEnd());
        
        
        Console.WriteLine("========= Capturing only stderr =============");
        ProcessExecutor.ExecuteProcess(processPath, false, out outReader, true,arguments);
        Console.WriteLine("Captured only stderr: \n" + outReader.ReadToEnd());
        
      
        Console.WriteLine("========= Adding stderr and stderr handlers =============");
        // Two ways of invoking
        ProcessExecutor.ExecuteProcess(
            processPath,
            (object sender, DataReceivedEventArgs e) => StdOutHandler(sender, e),
            StdErrHandler,
            true,
            arguments);
        
        Console.WriteLine("========= Adding stderr and stderr handlers =============");
        // Two ways of invoking
        ProcessExecutor.ExecuteProcess(
            processPath,
            (object sender, DataReceivedEventArgs e) => StdOutHandler(sender, e),
            StdErrHandler,
            true,
            arguments);
        
        
        Console.WriteLine("========= Adding only one handler (stdout) =============");
        ProcessExecutor.ExecuteProcess(
            processPath,
            true,
            OutHandler,
            true,
            arguments);

        
        Console.WriteLine("========= Adding only one handler (stderr) =============");
        ProcessExecutor.ExecuteProcess(
            processPath,
            false,
            OutHandler,
            true,
            arguments);
        
        Console.WriteLine("========= Capturing stdout Stream and Adding stderr handlers =============");
        // Two ways of invoking
        ProcessExecutor.ExecuteProcess(
            processPath,
            true,
            out outReader,
            StdErrHandler,
            true,
            arguments);
        Console.WriteLine("Captured only stdout as a stream: \n" + outReader.ReadToEnd());
        
        
        Console.WriteLine("========= Capturing stderr Stream and Adding stdout handlers =============");
        ProcessExecutor.ExecuteProcess(
            processPath,
            false,
            out outReader,
            StdOutHandler,
            true,
            arguments);
        Console.WriteLine("Captured only stderr as a stream: \n" + outReader.ReadToEnd());

        
        #endregion

        #region ProcessExecutor as an instance testing

        Console.WriteLine("========= ********* Testing ProcessExecutor as an object ************ =============");
        var executor = new ProcessExecutor(processPath)
        {
            WaitForExit = true,
            // ReSharper disable once ConvertClosureToMethodGroup
            // Demonstrating how one can use lambda expression to assign handler.
            StdoutHandler = (sender, e) => { StdOutHandler(sender,e);},
            StderrHandler = StdErrHandler
        };


        Console.WriteLine("========= Simple execution ============= (No output in this console) ");
        ProcessExecutor.ExecuteProcess(processPath, true, arguments);
        executor.Execute();
        
        Console.WriteLine("========= Capturing both stdout and stderr =============");
        executor.Mode = ProcessExecutor.RedirectionMode.RedirectStreams;
        executor.Execute();
        Console.WriteLine("Captured stdout: \n" + executor.StdoutReader.ReadToEnd());
        Console.WriteLine("Captured stderr: \n" + executor.StderrReader.ReadToEnd());

        
        Console.WriteLine("========= Capturing only stdout =============");
        executor.Mode = ProcessExecutor.RedirectionMode.RedirectStdout;
        executor.Execute();
        Console.WriteLine("Captured only stdout: \n" + executor.StdoutReader.ReadToEnd());
        
        
        Console.WriteLine("========= Capturing only stderr =============");
        executor.Mode = ProcessExecutor.RedirectionMode.RedirectStderr;
        executor.Execute();
        Console.WriteLine("Captured only stderr: \n" + executor.StderrReader.ReadToEnd());
        
      
        Console.WriteLine("========= Adding stderr and stderr handlers =============");
        executor.Mode = ProcessExecutor.RedirectionMode.UseHandlers;
        executor.Execute();
        
        
        Console.WriteLine("========= Using only one handler (stdout) =============");
        executor.Mode = ProcessExecutor.RedirectionMode.StdoutHandler;
        executor.Execute();
        
        Console.WriteLine("========= Using only one handler (stderr) =============");
        executor.Mode = ProcessExecutor.RedirectionMode.StderrHandler;
        executor.Execute();
        
        Console.WriteLine("========= Capturing stdout Stream and using stderr handler =============");
        executor.Mode = ProcessExecutor.RedirectionMode.RedirectStdoutWithStderrHandler;
        executor.Execute();
        Console.WriteLine("Captured only stdout as a stream: \n" + executor.StdoutReader.ReadToEnd());
        
        
        Console.WriteLine("========= Capturing stderr Stream and using stdout handler =============");
        executor.Mode = ProcessExecutor.RedirectionMode.RedirectStderrWithStdoutHandler;
        p = executor.Execute();
        Console.WriteLine("Captured only stderr as a stream: \n" + executor.StderrReader.ReadToEnd());

        Console.WriteLine("Process Exit Code: " + p.ExitCode); // Can gain access to process handle in order to get ExitCode and other things.
        #endregion

        Console.WriteLine("========= Executing a python script with python exe as executable =============");
        executor.ExecutablePath = @"python.exe"; // we need the python executable
        executor.Mode = ProcessExecutor.RedirectionMode.RedirectStreams;
        executor.Args = new[] {pythonScriptPath, "-f", "test"};
        executor.Execute();

        Console.WriteLine("Python stdout: "  + executor.StdoutReader.ReadToEnd());
        Console.WriteLine("Python stderr: "  + executor.StderrReader.ReadToEnd());
        

        Console.WriteLine("========= Executing a python script using PythonScriptExecutor =============");
        var pythonExecutor = new PythonScriptExecutor(pythonScriptPath)
        {
            Mode = ProcessExecutor.RedirectionMode.RedirectStreams,
            WaitForExit = true,
            Args = arguments
        };
        
        pythonExecutor.Execute();
        Console.WriteLine("Python stdout: \n"  + pythonExecutor.StdoutReader.ReadToEnd());
        
        pythonExecutor.Execute();
        Console.WriteLine("Python stderr: \n"  + pythonExecutor.StderrReader.ReadToEnd());
        return 0;
    }

    private static void StdOutHandler(object sender, DataReceivedEventArgs e)
    {
        Console.WriteLine($"StdOutHandler. Data received: \n<{e.Data}>");
    }
    private static void StdErrHandler(object sender, DataReceivedEventArgs e)
    {
        Console.WriteLine($"StderrHandler. Data received: \n<{e.Data}>");
    }
    
    private static void OutHandler(object sender, DataReceivedEventArgs e)
    {
        Console.WriteLine($"OutHandler. Data received: \n<{e.Data}>");
    }

}
