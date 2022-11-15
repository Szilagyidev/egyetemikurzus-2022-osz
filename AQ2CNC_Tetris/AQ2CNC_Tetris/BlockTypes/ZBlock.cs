﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ2CNC_Tetris.BlockTypes
{
    public class ZBlock : Block
    {
        public override int Id => 7;
        protected override Position StartOffSet => new(0, 3);

        protected override Position[][] Tiles => new Position[][]
        {
            new Position[] {new(0,0), new(0,1), new(1,1), new(1,2)},
            new Position[] {new(0,2), new(1,1), new(1,2), new(2,1)},
            new Position[] {new(1,0), new(1,1), new(2,1), new(2,2)},
            new Position[] {new(0,1), new(1,0), new(1,1), new(2,0)}
        };
    }
}
