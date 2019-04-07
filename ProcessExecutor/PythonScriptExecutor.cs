using System.Collections.Generic;
using System.Diagnostics;

namespace Tomis.Utils
{
    /// <summary>
    /// Provide a python script to execute. <para/>
    /// Assumes that python.exe is in PATH. If a specific python executable is needed set <see cref="PythonExecutablePath"/>
    /// accordingly. Args can now be set for the python script
    /// <para/>
    /// USAGE EXAMPLE
    /// <para/>
    /// var pythonExecutor = new PythonScriptExecutor("path\to\pythonscript.py") <para/>
    /// {                                                                        <para/>
    ///    Mode = ProcessExecutor.RedirectionMode.RedirectStreams,               <para/>
    ///    WaitForExit = true,
    ///    Args = new [] {"Some", "args", "for", "python script"}                <para/>
    /// };                                                                       <para/>      
    /// Console.WriteLine(pythonExecutor.<see cref="ProcessExecutor.StdoutReader()"/>.ReadToEnd());
    /// </summary>
    public class PythonScriptExecutor : ProcessExecutor
    {
        public string PythonExecutablePath { get; set; }
        
        public string PythonScriptPath { get; set; }
        
        public PythonScriptExecutor(string pythonScriptPath) : base("python")
        {
            PythonScriptPath = pythonScriptPath;
        }

        protected override Process Execute(RedirectionMode mode, params string[] args)
        {
            var argsNew = new List<string>(args);
            argsNew.Insert(0, PythonScriptPath);
            return base.Execute(mode, argsNew.ToArray());
        }
    }
}