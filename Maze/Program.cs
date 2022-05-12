using System;
using System.IO;
using System.CommandLine;

namespace Maze
{
    class Program
    {
        static void Main(string[] args)
        {
            var numOfRows = new Option<int>
                ("--num_of_rows", "The number of rows in the maze (from 1 to 200).");
            var numOfColumns = new Option<int>
                ("--num_of_columns", "The number of columns in the maze (from 1 to 200).");
            var outputDirectoryPath = new Option<string>
                ("--output_directory_path", "The directory path for the output image file.");
            var withSolution = new Option<bool>
                ("--with_solution", "Return the solution of the maze or not.");

            var rootCommand = new RootCommand("Generates a maze with the given number of rows and columns.");
            rootCommand.Add(numOfRows);
            rootCommand.Add(numOfColumns);
            rootCommand.Add(outputDirectoryPath);
            rootCommand.Add(withSolution);

            rootCommand.SetHandler((int numOfRows, int numOfColumns, string outputDirectoryPath, bool withSolution) =>
            {
                if (!(numOfRows >= 1 && numOfRows <= 200 && numOfColumns >= 1 && numOfColumns <= 200))
                {
                    Console.WriteLine("Please enter the correct num of rows and/or num of columns (from 1 to 200).");
                }
                else if (!IsValidOutputPath(outputDirectoryPath))
                {
                    Console.WriteLine("Please enter the correct path for the output image file.");
                }
                else
                {
                    run(numOfRows, numOfColumns, outputDirectoryPath, withSolution);
                }
            },
            numOfRows, numOfColumns, outputDirectoryPath, withSolution);

            rootCommand.Invoke(args);
        }

        static void run(int num_of_rows, int num_of_cols, string outputDirectoryPath, bool withSolution)
        {
            Maze maze = new Maze(num_of_rows, num_of_cols);
            string path1 = Path.Combine(outputDirectoryPath, "maze.png");
            maze.Image.Save(path1);
            Console.WriteLine($"The maze has been saved in {path1}.");

            if (withSolution)
            {
                string path2 = Path.Combine(outputDirectoryPath, "solved_maze.png");
                maze.GetSolutionImage().Save(path2);
                Console.WriteLine($"The solution has been saved in {path2}.");
            }          
        }

        static bool IsValidOutputPath(string path)
        {
            return Directory.Exists(path);
        }
    }
}
