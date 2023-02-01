using BombPalne;
using BombPlane.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BombPalne.AI;

namespace WinFormsApp2
{
    public partial class Form3 : Form
    {
        bool isus = true;
        int[] nowclick;
        int[][] myflag;
        List<int[]> head;
        Button[][] group_btn1;
        Button[][] group_btn2;
        bool PushFlag;
        private int count = 0;
        private int [][] flag;
        private int state;//1:单人 2:多人
        private int existNum = 3;
        Player player = new();
        AI aiplayer = new ();
        List<node> s= new();
        mapType[,] nowMap = new mapType[11, 11];
        public void open()
        {
            Server.Accept();
        }
        public Form3()
        {
            InitializeComponent();
        }
        public Form3(int m, int[][] r, List<int[]> Head) : this()
        {
            myflag = new int[10][];
            PushFlag = false;
            for(int i = 0; i < 10; i++)
            {
                myflag[i] = new int[10];
            }
            state = m;
            label1.Text = "思考时间还剩15秒";
            group_btn1 = new Button[10][];
            for (int i = 0; i < group_btn1.Length; i++)
            {
                group_btn1[i] = new Button[10];
            }
            for (int i = 0; i <= 9; i++)
                for (int k = 0; k <= 9; k++)
                {
                    group_btn1[i][k] = new Button();
                    group_btn1[i][k].Margin = new Padding(0);
                    group_btn1[i][k].Size = new Size(flowLayoutPanel1.Width / 10, flowLayoutPanel1.Height / 10);
                    if (r[i][k] == 1)
                        group_btn1[i][k].BackColor = Color.Red;
                    flowLayoutPanel1.Controls.Add(group_btn1[i][k]);
                }
            group_btn2 = new Button[10][];
            for (int i = 0; i <= 9; i++)
                group_btn2[i] = new Button[10];
            for (int i = 0; i <= 9; i++)
                for (int k = 0; k <= 9; k++)
                {
                    group_btn2[i][k] = new Button();
                    group_btn2[i][k].Name = (i * 10 + k).ToString();
                    group_btn2[i][k].Margin = new Padding(0);
                    group_btn2[i][k].Size = new Size(flowLayoutPanel2.Width / 10, flowLayoutPanel2.Height / 10);
                    flowLayoutPanel2.Controls.Add(group_btn2[i][k]);
                    group_btn2[i][k].MouseDown += new MouseEventHandler(address_MouseDown);
                }
            head = new List<int[]>();
            foreach(var i in Head)
            {
                this.head.Add(new int[2] { i[0], i[1] });
            }

        }
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!isus)
                return;
            count++;
            if (count == 300)
            {
                MessageBox.Show("思考时间到，认输");
                timer1.Enabled= false;
                comeback();
                return;
            }
            //if (count == 15)
            //{
            //    MessageBox.Show("现在还剩5秒");
            //}
            label1.Text = "现在思考时间还剩" + (300 - count).ToString();
        }
        private void comeback()
        {
            ((Form1)(this.Owner)).Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comeback();
        }
       void address_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isus)
                return;
            int i = (int.Parse(((Button)sender).Name)) / 10;
            int j = (int.Parse(((Button)sender).Name)) % 10;
            int temp = 10 * i + j;

            if (e.Button == MouseButtons.Left)
            {
                if (nowclick != null)
                {
                    MessageBox.Show("你已经选择过了轰炸点，本回合不能再炸");
                    return;
                }
                if (myflag[i][j] != 1)
                {
                    myflag[i][j] = 1;
                    nowclick = new int[2];
                    nowclick[0] = i;
                    nowclick[1] = j;
                    group_btn2[i][j].Text = "X";
                }
            }
            else
            {
                if (nowclick == null)
                {
                    return;
                }
                else
                {
                    myflag[nowclick[0]][nowclick[1]] = 0;
                    group_btn2[nowclick[0]][nowclick[1]].Text = "";
                    nowclick = null;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(state == 1)
            {
                aibattle();
            }
            else
            {
                if (nowclick == null)
                {
                    MessageBox.Show("请选择轰炸点");
                    return;
                }
                isus = false;

                PushFlag = true;

            }
        }
        public void aibattle()
        {
            string att = aiplayer.getNextStep(ref s, nowMap);
         //   Console.WriteLine(att);
            string[] pos = att.Split(' ');
            int x = int.Parse(pos[0]), y = int.Parse(pos[1]);
            int ans = player.isAttacked(x, y);
            if (group_btn1[0][0].Text == "X") { group_btn1[x][y].Text = ""; }
            switch (ans)
            {
                //击中空值
                case 0:
                    aiplayer.nowMap[x, y] = AI.mapType.empty;
                    aiplayer.elimination(ref s,x, y, AI.mapType.empty);
                    group_btn1[x][y].Text = "X";
                    break;
                //击中机头
                case 1:
                    aiplayer.nowMap[x, y] = AI.mapType.planeHead;
                    aiplayer.elimination(ref s, x, y, AI.mapType.planeHead);
                    group_btn1[x][y].Text = "X";
                    group_btn1[x][y].BackColor = Color.Blue;
                    break;
                //击中机身
                case 2:
                    aiplayer.nowMap[x, y] = AI.mapType.plane;
                    aiplayer.elimination(ref s, x, y, AI.mapType.plane);
                    group_btn1[x][y].Text = "X";
                    group_btn1[x][y].BackColor = Color.Yellow;
                    break;
            }
            int res = aiplayer.isAttacked(nowclick[0], nowclick[1]);
            switch (res)
            {
                //击中空值
                case 0:
                    group_btn2[nowclick[0]][nowclick[1]].BackColor = Color.Gray;
                    break;
                //击中机头
                case 1:
                    this.existNum--;
                    group_btn2[nowclick[0]][nowclick[1]].BackColor = Color.Red;
                    break;
                //击中机身
                case 2:
                    group_btn2[nowclick[0]][nowclick[1]].BackColor = Color.Yellow;
                    break;
            }
            if (player.isLose() && aiplayer.isLose())
            {
                MessageBox.Show("平局");
                comeback();
            }
            else if (player.isLose())
            {
                MessageBox.Show("You Lose!");
                comeback();
            }
            else if (aiplayer.isLose())
            {
                MessageBox.Show("You Win!");
                comeback();
            }
        }
        public void interaction()
        {
            while (true)
            {
                while (PushFlag == false) ;
                PushFlag = false;
                string message1 = nowclick[0].ToString() + ' ' + nowclick[1].ToString();
                //发送攻击信息
                Server.sendMessage(message1);

                //或得敌方反馈回来的信息
                string info = Server.getMessage();

                //string[] att_info = info.Split(" ");
                //int att_res = int.Parse(att_info[0]);
                //int isWin = 1 - int.Parse(att_info[1]);

                int att_res = int.Parse(info);
                switch (att_res)
                {
                    //击中空值
                    case 0:
                        group_btn2[nowclick[0]][nowclick[1]].BackColor = Color.Gray;
                        break;
                    //击中机头
                    case 1:
                        this.existNum--;
                        group_btn2[nowclick[0]][nowclick[1]].BackColor = Color.Red;
                        break;
                    //击中机身
                    case 2:
                        group_btn2[nowclick[0]][nowclick[1]].BackColor = Color.Yellow;
                        break;
                }

                // MessageBox.Show("You Win!");

                //敌方攻击我方
                nowclick = null;


                string point = Server.getMessage();

                string[] new_att = point.Split(" ");
                int x = int.Parse(new_att[0]);
                int y = int.Parse(new_att[1]);
                int atted_res = player.isAttacked(x, y);
                switch (atted_res)
                {
                    //击中空值
                    case 0:
                        group_btn1[x][y].Text = "X";
                        break;
                    //击中机头
                    case 1:
                        group_btn1[x][y].Text = "X";
                        group_btn1[x][y].BackColor = Color.Blue;
                        break;
                    //击中机身
                    case 2:
                        group_btn1[x][y].Text = "X";
                        group_btn1[x][y].BackColor = Color.Yellow;
                        break;
                }
                //我方输
                string message = atted_res.ToString();
                //将敌方的攻击信息发送给对方
                Server.sendMessage(message);
                //判断对局是否结束
                if(player.isLose()&& this.existNum == 0)
                {
                    MessageBox.Show("平局");
                    comeback();
                }
                else if (player.isLose())
                {
                    MessageBox.Show("You Lose!");
                    comeback();
                }
                else if (this.existNum == 0)
                {
                    MessageBox.Show("You Win!");
                    comeback();
                }
                isus = true;
                count = 0;

            }
        }
        public void initPlayer(List<int[]> Head, List<int[]> tail)
        {
           s = aiplayer.initNodes();
            for (int i = 0; i <= 10; ++i)
                for (int j = 0; j <= 10; j++)
                    nowMap[i, j] = mapType.unknown;
            for (int i = 0; i < 3; i++)
            {
                int row = Head[i][0];
                int col = Head[i][1];
                int tr = row-tail[i][0];
                int tc = col-tail[i][1];
                if(tc == 0)
                {
                    if(tr > 0)
                    {
                        player.addPlane(row, col, 0);
                        aiplayer.addPlane(row, col, 0);
                    }
                    else
                    {
                        player.addPlane(row, col, 2);
                        aiplayer.addPlane(row, col, 2);
                    }
                }
                else
                {
                    if(tc > 0)
                    {
                        player.addPlane(row, col, 1);
                        aiplayer.addPlane(row, col, 1);
                    }
                    else
                    {
                        player.addPlane(row, col, 3);
                        aiplayer.addPlane(row, col, 3);
                    }
                }
            }
        }
    }
}
