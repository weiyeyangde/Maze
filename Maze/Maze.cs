using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Maze
{
    public class Maze
    {
        private static Random rand = new Random();
        
        private const int PEN_WIDTH = 4;
        private const int PATH_WIDTH = 10;
        private const int SCALE = 10 * PEN_WIDTH;
        private const int PEN_OFFSET = PEN_WIDTH / 2;
        private const int PATH_OFFSET = SCALE / 2;
        private const int LEFT = 0;
        private const int UP = 1;
        private const int RIGHT = 2;
        private const int DOWN = 3;

        private readonly int _num_of_rows;
        private readonly int _num_of_cols;
        private Bitmap _image;
        
        public Bitmap Image { get => _image; }
        public CellInfo[,] Cell_info { get; } 

        public Maze(int num_of_rows, int num_of_cols)
        {
            _num_of_rows = num_of_rows;
            _num_of_cols = num_of_cols;

            CellInfo[,] cell_info = new CellInfo[_num_of_rows, _num_of_cols];
            for (int i = 0; i < _num_of_rows; i++)
            {
                for (int j = 0; j < _num_of_cols; j++)
                {
                    cell_info[i, j] = new CellInfo();
                }
            }
            GenerateMaze(cell_info);
            Cell_info = cell_info;
            _image = GetMazeImage();
        }

        public void GenerateMaze(CellInfo[,] cell_info)
        {
            int i = 0;
            int j = 0;
            List<int> main_path = new List<int>();
            
            main_path.Add(i);
            main_path.Add(j);
            while (main_path.Count > 0)
            {
                cell_info[i, j].Visited = true; // mark the curr cell as visited.
                List<int> remainingWalls = new List<int>();
                // check whether the adjacent cells are valid to move forward.
                if (j - 1 >= 0 && cell_info[i, j - 1].Visited == false) { remainingWalls.Add(LEFT); }
                if (i - 1 >= 0 && cell_info[i - 1, j].Visited == false) { remainingWalls.Add(UP); }
                if (j + 1 < _num_of_cols && cell_info[i, j + 1].Visited == false) { remainingWalls.Add(RIGHT); }
                if (i + 1 < _num_of_rows && cell_info[i + 1, j].Visited == false) { remainingWalls.Add(DOWN); }

                if (remainingWalls.Count > 0) // If there is at least one valid cell that exists to move forward.
                {
                    main_path.Add(i);
                    main_path.Add(j);
                    // Randomly choose a valid direction to move forward.
                    int direction = remainingWalls[rand.Next(remainingWalls.Count)];
                    if (direction == LEFT)
                    {
                        cell_info[i, j].HasLeftWall = false;
                        j -= 1;
                        cell_info[i, j].HasRightWall = false;
                    }
                    else if (direction == UP)
                    {
                        cell_info[i, j].HasTopWall = false;
                        i -= 1;
                        cell_info[i, j].HasBottomWall = false;
                    }
                    else if (direction == RIGHT)
                    {
                        cell_info[i, j].HasRightWall = false;
                        j += 1;
                        cell_info[i, j].HasLeftWall = false;
                    }
                    else
                    {
                        cell_info[i, j].HasBottomWall = false;
                        i += 1;
                        cell_info[i, j].HasTopWall = false;
                    }
                }
                else
                {
                    j = main_path[main_path.Count - 1]; main_path.RemoveAt(main_path.Count - 1);
                    i = main_path[main_path.Count - 1]; main_path.RemoveAt(main_path.Count - 1);
                }
            }

            // Open the wall at the beginning and end of the maze.
            cell_info[0, 0].HasLeftWall = false;
            cell_info[_num_of_rows - 1, _num_of_cols - 1].HasRightWall = false;
        }

        public Bitmap GetMazeImage()
        {
            int bitmapWidth = _num_of_cols * SCALE + PEN_WIDTH;
            int bitmapHeight = _num_of_rows * SCALE + PEN_WIDTH;
            Bitmap resultImage = new Bitmap(bitmapWidth, bitmapHeight);
            Pen blackPen = new Pen(Brushes.Black, PEN_WIDTH);

            using (Graphics g = Graphics.FromImage(resultImage))
            {
                g.Clear(Color.White);
                g.TranslateTransform(PEN_OFFSET, PEN_OFFSET);
                for (int i = 0; i < _num_of_rows; i++)
                {
                    for (int j = 0; j < _num_of_cols; j++)
                    {
                        if (Cell_info[i, j].HasLeftWall)
                        {
                            var line = GetLeftWall(i, j);
                            g.DrawLine(blackPen, line.top, line.bottom);
                        }
                        if (Cell_info[i, j].HasTopWall)
                        {
                            var line = GetTopWall(i, j);
                            g.DrawLine(blackPen, line.left, line.right);
                        }
                        if (Cell_info[i, j].HasRightWall)
                        {
                            var line = GetRightWall(i, j);
                            g.DrawLine(blackPen, line.top, line.bottom);
                        }
                        if (Cell_info[i, j].HasBottomWall)
                        {
                            var line = GetBottomWall(i, j);
                            g.DrawLine(blackPen, line.left, line.right);
                        }
                    }
                }   
            }
            return resultImage;
        }

        public Bitmap GetSolutionImage()
        {
            Bitmap solutionImage = new Bitmap(_image);
            List<(int, int)> stack = new List<(int, int)>();
            int[,] visited = new int[_num_of_rows, _num_of_cols];
            int[,] moves = new int[,] { { 0, -1 }, { -1, 0 }, { 0, 1 }, { 1, 0 } }; // LEFT, UP, RIGHT, DOWN
            bool helper(int i, int j)
            {
                if (0 <= i && i < _num_of_rows && 0 <= j && j < _num_of_cols && visited[i, j] == 0)
                {
                    visited[i, j] = 1;
                    stack.Add((i, j));
                    if (i == _num_of_rows - 1 && j == _num_of_cols - 1) return true;
                    for (int direction = 0; direction < 4; direction++)
                    {
                        int new_i = i + moves[direction, 0];
                        int new_j = j + moves[direction, 1];
                        if (canEnter(direction, i, j) && helper(new_i, new_j)) return true;
                    }
                    stack.RemoveAt(stack.Count - 1);
                }
                return false;
            }
            helper(0, 0);
            PathDispaly(solutionImage, stack);
            return solutionImage;
        }

        private bool canEnter(int direction, int i, int j)
        {
            switch (direction)
            {
                case LEFT:
                    return !Cell_info[i, j].HasLeftWall;
                case UP:
                    return !Cell_info[i, j].HasTopWall;
                case RIGHT:
                    return !Cell_info[i, j].HasRightWall;
                case DOWN:
                    return !Cell_info[i, j].HasBottomWall;
                default:
                    return false;
            }
        }

        public void PathDispaly(Bitmap bitmap, List<(int, int)> path)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Pen redPen = new Pen(Color.Red, 10);
                SolidBrush redBrush = new SolidBrush(Color.Red);

                g.TranslateTransform(PEN_OFFSET, PEN_OFFSET);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                for (int i = 1; i < path.Count; i++)
                {
                    g.DrawLine(redPen, PathPointAdjust(path[i]), PathPointAdjust(path[i - 1]));
                    var (x_i, y_i) = PositionAdjust(path[i]);
                    g.FillEllipse(redBrush, x_i, y_i, PATH_WIDTH, PATH_WIDTH);                   
                }

                var (x_0, y_0) = PositionAdjust(path[0]);
                g.FillEllipse(redBrush, x_0, y_0, PATH_WIDTH, PATH_WIDTH);
                g.DrawLine(redPen, PathPointAdjust(path[0]), new Point(-PEN_OFFSET, 20));
                g.DrawLine(redPen, PathPointAdjust(path[path.Count - 1]), new Point(bitmap.Width, path[path.Count - 1].Item1 * SCALE + PATH_OFFSET));
            }
        }

        private (Point top, Point bottom) GetLeftWall(int i, int j)
        {
            return (new Point(j * SCALE, i * SCALE - PEN_OFFSET), new Point(j * SCALE, (i + 1) * SCALE + PEN_OFFSET));
        }

        private (Point left, Point right) GetTopWall(int i, int j)
        {
            return (new Point(j * SCALE - PEN_OFFSET, i * SCALE), new Point((j + 1) * SCALE + PEN_OFFSET, i * SCALE));
        }

        private (Point top, Point bottom) GetRightWall(int i, int j)
        {
            return (new Point((j + 1) * SCALE, i * SCALE - PEN_OFFSET), new Point((j + 1) * SCALE, (i + 1) * SCALE + PEN_OFFSET));
        }

        private (Point left, Point right) GetBottomWall(int i, int j)
        {
            return (new Point(j * SCALE - PEN_OFFSET, (i + 1) * SCALE), new Point((j + 1) * SCALE + PEN_OFFSET, (i + 1) * SCALE));
        }

        private Point PathPointAdjust((int, int) cell)
        {
            return new Point(cell.Item2 * SCALE + PATH_OFFSET, cell.Item1 * SCALE + PATH_OFFSET);
        }
        
        private (int x, int y) PositionAdjust((int, int) cell)
        {
            return (cell.Item2 * SCALE + PATH_OFFSET - PATH_WIDTH / 2, cell.Item1 * SCALE + PATH_OFFSET - PATH_WIDTH / 2);
        }
    }
}
