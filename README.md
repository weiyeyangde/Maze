## Project2 - Maze
Generates a rectangular maze with given number of rows and columns.

### Usage
```
Description:
  Generates a maze with the given number of rows and columns.

Usage:
  Maze [options]

Options:
  --num_of_rows <num_of_rows>                      The number of rows in the maze (from 1 to 200).
  --num_of_columns <num_of_columns>                The number of columns in the maze (from 1 to 200).
  --output_directory_path <output_directory_path>  The directory path for the output image file.
  --with_solution                                  Return the solution of the maze or not.
  --version                                        Show version information
  -?, -h, --help                                   Show help and usage information
```
```
.\Maze.exe --num_of_rows 50 --num_of_columns 50 --output_directory_path "E:\" --with_solution
```
### Examples
- 20 x 20 maze and its solution
  <p align="left">
    <img src="https://github.com/weiyeyangde/Maze/blob/main/maze_20_20.png" title="Maze" style="width: 20%; height:auto;">
    <br><br>
    <img src="https://github.com/weiyeyangde/Maze/blob/main/solved_maze_20_20.png" title="Solution" style="width: 20%; height:auto;">
  </p>
  
- 50 x 50 maze and its solution
  <p align="left">
    <img src="https://github.com/weiyeyangde/Maze/blob/main/maze_50_50.png" title="Maze" style="width: 30%; height:auto;">
    <br><br>
    <img src="https://github.com/weiyeyangde/Maze/blob/main/solved_maze_50_50.png" title="Solution" style="width: 30%; height:auto;">
  </p>
 
- 100 x 100 maze and its solution
  <p align="left">
    <img src="https://github.com/weiyeyangde/Maze/blob/main/maze_100_100.png" title="Maze" style="width: 50%; height:auto;">
    <br><br>
    <img src="https://github.com/weiyeyangde/Maze/blob/main/solved_maze_100_100.png" title="Solution" style="width: 50%; height:auto;">
  </p>
  
### High-level ideas
  - Using DFS algorithm to generate a maze.
  - Using DFS algorithm to solve the maze.
