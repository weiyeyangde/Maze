using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class CellInfo
    {
        public bool HasLeftWall { get; set; }
        public bool HasTopWall { get; set; }
        public bool HasRightWall { get; set; }
        public bool HasBottomWall { get; set; }
        public bool Visited { get; set; }

        public CellInfo()
        {
            HasLeftWall = true;
            HasTopWall = true;
            HasRightWall = true;
            HasBottomWall = true;
            Visited = false;
        }
    }
}
