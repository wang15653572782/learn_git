using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombPalne
{
    internal class Board
    {
        protected int[,] board = new int[10,10];

        public Board()
        {
            initBoard();
        }
        public void initBoard()
        {
            for(int i = 0; i < 10; i++)
            {
                for(int j=0;j<10; j++)
                {
                    board[i,j] = 0;
                }
            }
        }
        public void setBoard(int[,] newBoard)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    board[i, j] = newBoard[i,j];
                }
            }
        }
        public int[,] getBoard()
        {
            return board;
        }
        public void setBoard(int[,] pos,int value,int length,int start=0)
        {
            //根据pos坐标更新棋盘，更新的值为value，从start开始更新length个坐标
            for(int i=start;i-start<length; i++)
            {
                board[pos[i, 0], pos[i,1]] = value;
            }
        }
        public int getValue(int row,int col)
        {
            return board[row,col];
        }
    }
}
