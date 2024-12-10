using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1;

public class Subtask_1
{
    // Directory for generated files
    const string _dir = "./generated";
    // Count of files to generate
    const int _filesToGenereate = 100;
    // Count of lines to generate per file
    const int _linesToGenerate = 100_000;

    // Date ticks for start of program execution
    static long _dateNowTicks = DateTime.Now.Ticks;
    // Date ticks for 5 years ago from start of program execution
    static long _dateFor5YearsAgoTicks = DateTime.Now.AddYears(-5).Ticks;

    // Global generator of numbers
    static Random _globalRnd = new Random((int)DateTime.Now.Ticks);
    // Mutex for global generator of numbers
    static Mutex _globalRndMutex = new Mutex();

    // GenerateString - function which generates lines of task format   
    static string GenerateString(Random rnd)
    {
        // Date between now and 5 years ago, string casting
        var date = new DateTime(rnd.NextInt64(_dateFor5YearsAgoTicks, _dateNowTicks)).ToString("dd.MM.yy");

        // Generating latin and russian string sequances
        StringBuilder latinString = new StringBuilder(0, 10);
        StringBuilder rusString = new StringBuilder(0, 10);
        for (int i = 0; i < 10; i++)
        {
            int latinOffset = rnd.Next(0, 52);
            int rusOffset = rnd.Next(0, 66);

            latinString.Append((char)(latinOffset < 26 ? 'A' + latinOffset : 'a' + (latinOffset - 26)), 1);
            rusString.Append((char)(rusOffset < 64 ? 'А' + rusOffset : (rusOffset == 64 ? 'Ё' : 'ё')), 1);
        }

        // Genarating integer (1 - 100_000_000) and fractional number (1 - 20)
        var integer = rnd.Next(1, 100_000_001).ToString();
        var number = (rnd.NextDouble() * 19 + 1).ToString("0.########");

        // Returning formatted line
        return string.Format("{0}||{1}||{2}||{3}||{4}||", date, latinString, rusString, integer, number);
    }

    // CreateAndWriteToFile - function for creating file and filling with generated lines
    static void CreateAndWriteToFile(int fileNumber)
    {
        // Combine directory path and file name using file routing rules
        string current = Path.Combine(_dir, string.Format("file{0}.txt", fileNumber));

        // Global random is shared resource and not thread-safe, so access only by mutex
        _globalRndMutex.WaitOne();
        var ownRnd = new Random(_globalRnd.Next());
        _globalRndMutex.ReleaseMutex();

        using (StreamWriter sw = new StreamWriter(current))
        {
            for (int i = 0; i < _linesToGenerate; i++)
            {
                sw.WriteLine(GenerateString(ownRnd));
            }
        }
        Console.WriteLine("Writing {0} completed", fileNumber);
    }


    // PlayScenario - function for subtask scenario
    public static void PlayScenario()
    {
        Console.WriteLine("Scenario of Subtask 1 started");

        // Check if directory doesn't exist
        if (!Directory.Exists(_dir))
        {
            Directory.CreateDirectory(_dir);
        }

        Parallel.For(0, _filesToGenereate, CreateAndWriteToFile);

        Console.WriteLine("Scenario of Subtask 1 completed");
        Console.WriteLine("------------------------------------------------------------------------\n");
    }
}
