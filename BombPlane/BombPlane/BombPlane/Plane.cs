using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombPalne
{
    internal class Plane
    {
        public int head_row;
        public int head_col;
        public int direction;//机头的方向与x轴的夹角  0:0/360   1:90   2:180   3:270
        public int[,] plane_pos = new int[10, 2];//0:机头 1~9：机身
        public bool isExist;
        public int[] status = new int[10];//坐标的状态 0：击中 1：正常
        public Plane(int row,int col,int direction)
        {
            this.head_row = row;
            this.head_col = col;
            this.direction = direction;
            isExist = true;
            for(int i=0; i < 10; i++) { status[i] = 1; }
            setPlanePos();
        }
        public int[,] getPlanePos() { return plane_pos; }
        private void setPlanePos()
        {
            plane_pos[0,0] = head_row;
            plane_pos[0,1] = head_col;
            int[][,] plane_sample = new int[4][,];
            //0
            plane_sample[0] = new int[10, 2] { { 0, 0 }, { -1, -2 }, { -1, -1 }, { -1, 0 }, { -1, 1 }, { -1, 2 }, { -2, 0 }, { -3, -1 }, { -3, 0 }, { -3, 1 } };
            //90
            plane_sample[1] = new int[10, 2] { { 0, 0 }, { -2, -1 }, { -1, -1 }, { 0, -1 }, { 1, -1 }, { 2, -1 }, { 0, -2 }, { -1, -3 }, { 0, -3 }, { 1, -3 } };
            //180
            plane_sample[2] = new int[10, 2] { { 0, 0 }, { 1, -2 }, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 1, 2 }, { 2, 0 }, { 3, -1 }, { 3, 0 }, { 3, 1 } };
            //270
            plane_sample[3] = new int[10, 2] { { 0, 0 }, { -2, 1 }, { -1, 1 }, { 0, 1 }, { 1, 1 }, { 2, 1 }, { 0, 2 }, { -1, 3 }, { 0, 3 }, { 1, 3 } };

            for(int i=0; i < 10; i++)
            {
                plane_pos[i, 0] = head_row + plane_sample[direction][i, 0];
                plane_pos[i, 1] = head_col + plane_sample[direction][i, 1];
            }
        }
        //更新坐标plan_pos[i]的状态，表示该节点被击中
        public void updateStatus(int i,int value)
        {
            status[i] = value;
        }
        public int isInPlanePos(int row,int col)
        {
            for(int i = 0; i < 10; i++)
            {
                if (plane_pos[i, 0] == row && plane_pos[i, 1] == col) return i;
            }
            //不在坐标集合内返回-1
            return -1;
        }
        public bool checkPosition()
        {
            int max_x = 0, min_x = 11;
            int max_y = 0, min_y = 11;
            for(int i = 0; i < 10; i++)
            {
                max_x = Math.Max(max_x, plane_pos[i,0]);
                min_x = Math.Min(min_x, plane_pos[i,0]);
                max_y = Math.Max(max_y, plane_pos[i, 1]);
                min_y = Math.Min(min_y, plane_pos[i,1]);
            }
            //不在棋盘内，越界
            if(max_x>9||min_x<0|| max_y > 9 || min_y < 0)
            {
                return false;
            }
            //在棋盘内
            return true;
        }
    }
}
