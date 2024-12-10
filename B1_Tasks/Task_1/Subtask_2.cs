using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Task_1;

public class Subtask_2
{
    // Directory of our geneated files
    const string _dir = "./generated";
    // File for data merging
    const string _mergeFilePath = "./merge_file.txt";

    // PlayScenario - function for subtask scenario
    public static void PlayScenario()
    {
        Console.WriteLine("Scenario of Subtask 2 started");

        // Get files from directory
        DirectoryInfo dirInfo = new DirectoryInfo(_dir);
        var files = dirInfo.GetFiles();

        Console.WriteLine("Found {0} files in \"{1}\". We are ready to merge", files.Length, _dir);

        // Ask for line deleting by entered pattern        
        Console.WriteLine("Do you wanna delete lines with specific pattern? Y/N");
        bool gotAnswer = false, deleteByPattern = false;
        do
        {
            var answer = Console.ReadLine();
            switch (answer)
            {
                case "Y":
                case "y":
                    gotAnswer = true;
                    deleteByPattern = true;
                    break;
                case "N":
                case "n":
                    gotAnswer = true;
                    deleteByPattern = false;
                    break;
            }
        } while (!gotAnswer);

        // Positive answer? So lets read our pattern!
        string pattern = string.Empty;
        if (deleteByPattern)
        {
            Console.WriteLine("Write your specific pattern:");
            pattern = Console.ReadLine() ?? string.Empty;
        }

        int patternMatchCounter = 0;


        // Open file stream for merging file
        using (StreamWriter sw = new StreamWriter(_mergeFilePath))
        {
            object locker = new object();

            Parallel.ForEach(files, file =>
            {
                // Open file stream for separate file
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    // Read by line
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Agreed to delete by pattern and it matched? Lets go!
                        if (deleteByPattern && line.Contains(pattern))
                        {
                            // Interlocked incrementation for thread-safety
                            Interlocked.Increment(ref patternMatchCounter);
                            continue;
                        }
                        // Alternative for mutex. We write into shared source!  
                        lock (locker)
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                Console.WriteLine("Reading \"{0}\" completed", file.Name);
            });
        }

        // Log statistics only for those who agreed to delete by pattern
        if (deleteByPattern)
        {
            Console.WriteLine("Deleted lines with pattern \"{0}\": {1}", pattern, patternMatchCounter);
        }
        Console.WriteLine("Rest lines are merged to file \"{0}\"", _mergeFilePath);

        Console.WriteLine("Scenario of Subtask 2 completed");
        Console.WriteLine("------------------------------------------------------------------------\n");
    }
}
