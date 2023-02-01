using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombPalne
{
    internal class Player : Board
    {
        public Plane[] plane = new Plane[3];
        private int[] status = new int[3];// 0：不存在/击中  1：未击中机头
        private int existNums;
        public int[,] attack_board = new int[10, 10];//地方被攻击的位置  -1:未攻击 0:攻击为空 1:击中机头 2:击中机身
        public Player() : base()
        {
            for(int i = 0; i < 3; i++) { status[i] = 0; }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    attack_board[i, j] = 0;
                }
            }
            existNums = 0;
        }
        //放置飞机
        public int addPlane(int row,int col,int direction)
        {
            //已经存在3架飞机,无法创建
            if (existNums == 3) return 0;
            Plane tmpPlane = new Plane(row, col, direction);
            //判断飞机是否超出棋盘
            if (tmpPlane.checkPosition())
            {
                //飞机没有越界，在棋盘内
                int[,] tmpBoard = getBoard();
                int[,] plane_pos = tmpPlane.getPlanePos();
                for(int i = 0; i < 10; i++)
                {
                    //不同飞机是否重叠，重叠返回错误3
                    if (tmpBoard[plane_pos[i, 0], plane_pos[i, 1]] == 1) { return 3; }
                    else tmpBoard[plane_pos[i, 0], plane_pos[i, 1]] = 1;
                }
                //没有错误则更新棋盘并保存新建的飞机
                setBoard(tmpBoard);
                plane[existNums] = tmpPlane;
                status[existNums]++;
                existNums++;
                //返回成功创建
                return 1;
            }
            else
                //飞机越界
                return 2;
        }
        public int isAttacked(int row,int col)
        {
            int[,] tmpboard = getBoard();
            //目标点为空，返回0
            if (tmpboard[row, col] == 0) { return 0; }
            //目标点有飞机
            for(int i = 0; i < 3; i++)
            {
                if (plane[i].isExist)
                {
                    int idx = plane[i].isInPlanePos(row, col);
                    if (idx == -1)
                    {
                        continue;
                    }
                    //击中机头
                    if (idx == 0)
                    {
                        /*
                        plane[i].isExist = false;
                        //清空棋盘上当前飞机所在位置的值
                        setBoard(plane[i].plane_pos, 0, 10, 0);
                        //更新飞机状态
                        for(int j = 0; j < 10; j++)
                        {
                            plane[i].updateStatus(j, 0);
                        }
                        */
                        plane[i].updateStatus(0, 0);
                        setBoard(plane[i].plane_pos, 0, 1, 0);
                        existNums--;
                        status[i] = 0;
                        //击中机头返回1
                        return 1;
                    }
                    else
                    {
                        //更新飞机状态和棋盘坐标
                        plane[i].updateStatus(idx,0);
                        setBoard(plane[i].plane_pos, 0, 1, idx);
                        //击中机身返回2
                        return 2;
                    }
                }
            }
            return 0;
        }

        public bool isLose() { return this.existNums.Equals(0); }
    }
    class RealPlayer : Player
    {

    }
    class AI : Player
    {
        public List<node> s =new List<node>();
        public mapType[,] nowMap = new mapType[N + 1, N + 1];
        public AI():base()
        {

        }
        public struct pair
        {
            public pair(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }

        }
        public enum mapType
        {
            unknown,
            empty,
            plane,
            planeHead,
        };
        public mapType[,] a = new mapType[11, 11];
        int[,] upPlane = new int[10, 2];
        int[,] downPlane = new int[10, 2];
        int[,] leftPlane = new int[10, 2];
        int[,] rightPlane = new int[10, 2];
        const int plan_num = 3;
        const int N = 10;
        public void init()
        {
            int[] u = { 1, -2, +1, -1, +1, 0, +1, +1, +1, +2, +2, 0, +3, -1, +3, 0, +3, +1 };
            int[] d = { -1, -2, -1, -1, -1, 0, -1, +1, -1, +2, -2, 0, -3, -1, -3, 0, -3, +1 };
            int[] l = { -2, +1, -1, +1, 0, +1, +1, +1, +2, +1, 0, +2, -1, +3, 0, +3, +1, +3 };
            int[] r = { -2, -1, -1, -1, 0, -1, +1, -1, +2, -1, 0, -2, -1, -3, 0, -3, +1, -3 };
            for (int i = 0; i < 9; ++i)
                for (int j = 0; j < 2; ++j) upPlane[i, j] = u[2 * i + j];
            for (int i = 0; i < 9; ++i)
                for (int j = 0; j < 2; ++j) downPlane[i, j] = d[2 * i + j];
            for (int i = 0; i < 9; ++i)
                for (int j = 0; j < 2; ++j) leftPlane[i, j] = l[2 * i + j];
            for (int i = 0; i < 9; ++i)
                for (int j = 0; j < 2; ++j) rightPlane[i, j] = r[2 * i + j];
            for (int i = 0; i <= 10; ++i)
                for (int j = 0; j <= 10; ++j)
                {
                    a[i, j] = mapType.empty;
                }
        }

        public struct node
        {

            public mapType[,] map = new mapType[11, 11];
            public node()
            {
                for (int i = 0; i <= 10; i++)
                    for (int j = 0; j <= 10; j++)
                    {
                        map[i, j] = mapType.empty;
                    }
            }


            public long hash()
            {
                long M = 10000000000037, p = 107;
                long ret = 1;
                for (int i = 1; i <= 10; i++)
                    for (int j = 1; j <= 10; j++)
                        if (map[i, j] == mapType.planeHead || map[i, j] == mapType.plane)
                            ret = ret * (i * 10 + j) % M * p % M;
                return ret;
            }
        }
        public void dfs(int nowPNum, ref List<node> VN)
        {
            if (nowPNum > plan_num)
            {
                node x = new node();
                // copy(begin(a), end(a), begin(x.map));
                for (int i = 1; i < 10; ++i)
                    for (int j = 1; j <= 10; ++j)
                        x.map[i, j] = a[i, j];
                //  memcpy(x.map, a, sizeof(a));
                VN.Add(x);
                return;
            }
            /**
             * 枚举当前这架飞机的所有可能位置
             * 总共4种情况，即对应飞机的4种朝向，每种情况枚举机头位置
             */
            mapType[,] b = new mapType[N + 1, N + 1];
            for (int i = 1; i <= 10; ++i)
                for (int j = 1; j <= 10; ++j) b[i, j] = a[i, j];
            //  memcpy(b, a, sizeof(a));   // 备份数组a

            for (int dir = 0; dir < 4; dir++)   // 枚举飞机朝向
                for (int i = 1; i <= N; i++)
                    for (int j = 1; j <= N; j++)
                    {  // 枚举机头位置
                        for (int m = 1; m <= 10; ++m)
                            for (int n = 1; n <= 10; ++n) a[m, n] = b[m, n];
                        //memcpy(a, b, sizeof(b));   // 首先初始化数组a
                        bool flag = true;
                        if (a[i, j] != mapType.empty) continue;
                        a[i, j] = mapType.planeHead;
                        for (int k = 0; k < 9; k++)
                        {   // 枚举机身位置
                            int ii, jj;
                            if (dir == 0) { ii = i + upPlane[k, 0]; jj = j + upPlane[k, 1]; }

                            else if (dir == 1) { ii = i + downPlane[k, 0]; jj = j + downPlane[k, 1]; }

                            else if (dir == 2) { ii = i + leftPlane[k, 0]; jj = j + leftPlane[k, 1]; }

                            else { ii = i + rightPlane[k, 0]; jj = j + rightPlane[k, 1]; }
                            if (ii < 1 || ii > N || jj < 1 || jj > N)
                            {
                                flag = false;
                                break;
                            };
                            if (a[ii, jj] != mapType.empty)
                            {
                                flag = false;
                                break;
                            };
                            a[ii, jj] = mapType.plane;
                        }
                        if (flag) dfs(nowPNum + 1, ref VN);
                    }
        }
        public List<node> initNodes()
        {
            /**
             * 给出所有可能的摆放方式（已去重）
             */
            for (int i = 1; i <= N; i++)
                for (int j = 1; j <= N; j++)
                    a[i, j] = mapType.empty;

            List<node> temp = new();
            List<node> ret = new();
            dfs(1, ref temp);
            //Console.WriteLine("{0}", temp.Count());
            HashSet<long> ss = new();   // 用于去重
            foreach (node x in temp)
            {
                long h = x.hash();
                if (!ss.Contains(h))
                {
                    ret.Add(x);
                    ss.Add(h);
                }
            }
            return ret;
        }
        public string getNextStep(ref List<node> s, mapType[,] nowMap)
        {
            int ii = 0, jj = 0, maxEarn = 0;
            for (int i = 1; i <= N; i++)
                for (int j = 1; j <= N; j++)
                    if (nowMap[i, j] == mapType.unknown)
                    {  // 枚举可以选的位置
                       // 首先计算当前位置各种情况的频率
                        int p1 = 0, p2 = 0, p3 = 0;
                        foreach (node x in s)
                        {
                            if (x.map[i, j] == mapType.planeHead) p2++;
                            if (x.map[i, j] == mapType.plane) p1++;
                            if (x.map[i, j] == mapType.empty) p3++;
                        }
                        int earn = p3 * (p1 + p2) + p2 * (p1 + p3) + p1 * (p2 + p3);
                        if (earn > maxEarn)
                        {
                            ii = i; jj = j;
                            maxEarn = earn;
                        }
                    }
            //pair t = new pair(ii, jj);
            string ans = ii.ToString() + ' ' + jj.ToString();
            return ans;
        }
        public void elimination(ref List<node> s, int x, int y, mapType m)
        {
            List<node> temp = new List<node>(s.Count);
            //temp.Re(s.Count());
            foreach (node t in s) temp.Add(t);
            s.Clear();
            foreach (node t in temp)
            {
                if (t.map[x, y] == m) s.Add(t);
            }
        }
    }
}
