﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ2CNC_Tetris
{
    public class GameState
    {
        private Block currentBlock;
        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                for (int i = 0; i < 2; i++) //a látható sorba spawnlojon a block
                {
                    currentBlock.Move(1, 0);
                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }

        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10); // +2 sor a blockok spawnolása miatt
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }

        private bool BlockFits() //checkolom hogy a block ne menjen ki a gamegrid-ből vagy overlapeljen egy tile-al.
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }

            return true;
        }

        public void HoldBlock()
        {
            try
            {
                if (!CanHold)
                {
                    return;
                }
                if (HeldBlock == null)
                {
                    HeldBlock = currentBlock;
                    currentBlock = BlockQueue.GetAndUpdate();
                }
                else
                {
                    Block tmp = CurrentBlock;
                    CurrentBlock = HeldBlock;
                    HeldBlock = tmp;
                }

                CanHold = false;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCCW();
            }
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCW();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            try
            {
                foreach (Position p in CurrentBlock.TilePositions())
                {
                    GameGrid[p.Row, p.Column] = CurrentBlock.Id;
                }

                Score += GameGrid.ClearFullRows();

                if (IsGameOver())
                {
                    GameOver = true;
                }
                else
                {
                    CurrentBlock = BlockQueue.GetAndUpdate();
                    CanHold = true;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        public void MoveBlockDown()
        {
            try
            {
                CurrentBlock.Move(1, 0);

                if (!BlockFits())
                {
                    CurrentBlock.Move(-1, 0);
                    PlaceBlock();
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }

        private int TileDropDistance(Position position)
        {
            int drop = 0;

            while (GameGrid.IsEmpty(position.Row + drop + 1, position.Column))
            {
                drop++;
            }

            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (Position position in CurrentBlock.TilePositions())
            {
                int[] dropArray = { drop, TileDropDistance(position)};
                drop = dropArray.Min();
            }

            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
