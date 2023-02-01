using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{

    public partial class Form2 : Form
    {
        public List<int[]> Head ;
        Thread thread;
        public List<int[]> dir=new List<int[]>();
        public List<int[]> tail=null;
        public List<List<int[]>> placed_plane;
        public List<List<int[]>>possible_plane;
        public int[][] flag ;
        int nowstate = 0;
        Button[][] group_btn;
        public int state;
        public Form2()
        {
            InitializeComponent();
             Head=new List<int[]>();
            placed_plane=new List<List<int[]>>();
            lbl1.Text = "现在摆放了0架飞机";
            lbl2.Text = "现在该确定机头";
             flag = new int[10][];
            for (int i = 0; i <= 9; i++)
                flag[i] = new int[10];
            group_btn = new Button[10][];
            for (int i = 0; i < 10; i++)
                group_btn[i] = new Button[10];
            for(int i=0;i<10;i++)
                for(int j = 0; j < 10; j++)
                {

                    group_btn[i][j] = new Button();
                    group_btn[i][j].Margin = new Padding(0);
                    group_btn[i][j].Name = new String(((i) *10+ (j)).ToString());
                    group_btn[i][j].Size = new Size(flowLayoutPanel1.Width / 10, flowLayoutPanel1.Height / 10);
                    flowLayoutPanel1.Controls.Add(group_btn[i][j]);
                    group_btn[i][j].MouseDown +=  new MouseEventHandler(address_MouseDown);
                }
            lbl1.TabIndex = 0;
        }
        public Form2(int m):this()
        {
            state = m;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void address_MouseDown(object sender, MouseEventArgs e)
        {
            int i = (int.Parse(((Button)sender).Name)) / 10;
            int j = (int.Parse(((Button)sender).Name)) % 10;
            if (e.Button == MouseButtons.Left) {
            int myflag = 0;
                if (nowstate == 6)
                {
                    MessageBox.Show("无法再次放置飞机");
                    return;
                }

                if (nowstate % 2 == 0)
                {
                    possible_plane = new List<List<int[]>>();
                    tail = new List<int[]>();
                    if (flag[i][j] == 0)
                    {
                        if (i + 3 <= 9)
                        {
                            if (j + 2 <= 9 && j - 2 >= 0)
                            {

                                List<int[]> q = new List<int[]>();

                                myflag = 0;
                                for (int k = 0; k <= 3; k++)
                                {
                                    if (k != 1 && k != 3)
                                    {
                                        if (flag[i + k][j] == 0)
                                            q.Add(new int[2] { i + k, j });
                                        else
                                        {
                                            myflag = 1;
                                        }
                                    }
                                    else if (k == 1)
                                    {
                                        for (int l = -2; l <= 2; l++)
                                        {
                                            if (flag[i + k][j + l] == 0)
                                                q.Add(new int[2] { i + k, j + l });
                                            else
                                                myflag = 1;
                                        }

                                    }
                                    else if (k == 3)
                                    {
                                        for (int l = -1; l <= 1; l++)
                                        {
                                            if (flag[i + k][j + l] == 0)
                                                q.Add(new int[2] { i + k, j + l });
                                            else
                                                myflag = 1;
                                        }
                                    }
                                }
                                if (myflag == 0)
                                {
                                    group_btn[i + 3][j].BackColor = Color.Yellow;
                                    possible_plane.Add(q);
                                    tail.Add(new int[2] { i + 3, j });
                                }
                            }
                        }
                        if (i - 3 >= 0)
                        {
                            if (j + 2 <= 9 && j - 2 >= 0)
                            {

                                List<int[]> q = new List<int[]>();

                                myflag = 0;
                                for (int k = 0; k >= -3; k--)
                                {
                                    if (k != -1 && k != -3)
                                    {
                                        if (flag[i + k][j] == 0)
                                            q.Add(new int[2] { i + k, j });
                                        else
                                        {
                                            myflag = 1;
                                        }
                                    }
                                    else if (k == -1)
                                    {
                                        for (int l = -2; l <= 2; l++)
                                        {
                                            if (flag[i + k][j + l] == 0)
                                                q.Add(new int[2] { i + k, j + l });
                                            else
                                                myflag = 1;
                                        }

                                    }
                                    else if (k == -3)
                                    {
                                        for (int l = -1; l <= 1; l++)
                                        {
                                            if (flag[i + k][j + l] == 0)
                                                q.Add(new int[2] { i + k, j + l });
                                            else
                                                myflag = 1;

                                        }
                                    }
                                }
                                if (myflag == 0)
                                {
                                    group_btn[i - 3][j].BackColor = Color.Yellow;
                                    possible_plane.Add(q);
                                    tail.Add(new int[2] { i - 3, j });
                                }
                            }
                        }
                        if (j + 3 <= 9)
                        {
                            if (i + 2 <= 9 && i - 2 >= 0)
                            {
                                List<int[]> q = new List<int[]>();

                                myflag = 0;
                                for (int k = 0; k <= 3; k++)
                                {
                                    if (k != 1 && k != 3)
                                    {
                                        if (flag[i][j + k] == 0)
                                            q.Add(new int[2] { i, j + k });
                                        else
                                        {
                                            myflag = 1;
                                        }
                                    }
                                    else if (k == 1)
                                    {
                                        for (int l = -2; l <= 2; l++)
                                        {
                                            if (flag[i + l][j + k] == 0)
                                                q.Add(new int[2] { i + l, j + k });
                                            else
                                                myflag = 1;
                                        }

                                    }
                                    else if (k == 3)
                                    {
                                        for (int l = -1; l <= 1; l++)
                                        {
                                            if (flag[i + l][j + k] == 0)
                                                q.Add(new int[2] { i + l, j + k });
                                            else
                                                myflag = 1;

                                        }
                                    }
                                }
                                if (myflag == 0)
                                {
                                    group_btn[i][j + 3].BackColor = Color.Yellow;
                                    possible_plane.Add(q);
                                    tail.Add(new int[2] { i, j + 3 });
                                }
                            }
                        }
                        if (j - 3 >= 0)
                        {
                            if (i + 2 <= 9 && i - 2 >= 0)
                            {

                                List<int[]> q = new List<int[]>();

                                myflag = 0;
                                for (int k = 0; k >= -3; k--)
                                {
                                    if (k != -1 && k != -3)
                                    {
                                        if (flag[i][j + k] == 0)
                                            q.Add(new int[2] { i, j + k });
                                        else
                                        {
                                            myflag = 1;
                                        }
                                    }
                                    else if (k == -1)
                                    {
                                        for (int l = -2; l <= 2; l++)
                                        {
                                            if (flag[i + l][j + k] == 0)
                                                q.Add(new int[2] { i + l, j + k });
                                            else
                                                myflag = 1;
                                        }

                                    }
                                    else if (k == -3)
                                    {
                                        for (int l = -1; l <= 1; l++)
                                        {
                                            if (flag[i + l][j + k] == 0)
                                                q.Add(new int[2] { i + l, j + k });
                                            else
                                                myflag = 1;

                                        }
                                    }
                                }
                                if (myflag == 0)
                                {
                                    group_btn[i][j - 3].BackColor = Color.Yellow;
                                    possible_plane.Add(q);
                                    tail.Add(new int[2] { i, j - 3 });
                                }
                            }
                        }
                        if (tail.Count == 0)
                            MessageBox.Show("该位置无法放置机头");
                        else
                        {
                            Head.Add(new int[2] { i, j });
                            group_btn[i][j].BackColor = Color.Red;
                            nowstate++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("无法放置机头");
                    }
                }
                else
                {
                    int t = 0;
                    for (t = 0; t < tail.Count; t++)
                    {
                        if (tail[t][0] == i && tail[t][1] == j)
                        {
                            myflag = 1;
                            break;
                        }
                    }
                    if (myflag == 0)
                        MessageBox.Show("请在黄色网格中选择放置");
                    else
                    {
                        foreach (var o1 in possible_plane[t])
                        {

                            group_btn[o1[0]][o1[1]].BackColor = Color.Red;
                            flag[o1[0]][o1[1]] = 1;
                        }
                        for (int o1 = 0; o1 < tail.Count; o1++)
                        {
                            if (o1 != t)
                                group_btn[tail[o1][0]][tail[o1][1]].BackColor = Color.White;
                            else
                                dir.Add(tail[o1]);
                        }
                        placed_plane.Add(possible_plane[t]);
                        nowstate++;
                    }
                }
                }
           else if (e.Button == MouseButtons.Right) {
                if (nowstate == 0)
                {
                    MessageBox.Show("当前无飞机可以删除");
                    return;
                }
                if (nowstate % 2 == 1)
                {

                        group_btn[Head[Head.Count - 1][0]][Head[Head.Count - 1][1]].BackColor
                            = Color.White;
                        foreach(var t in tail)
                        {
                            group_btn[t[0]][t[1]].BackColor= Color.White;
                        }
                        Head.Remove(Head[Head.Count - 1]);
                        nowstate--;


                }
              else  if (nowstate % 2 == 0) {
                    if (group_btn[i][j].BackColor != Color.Red)
                        MessageBox.Show("当前位置没有飞机可以删除");
                    else
                    {
                        int myflag = 0;
                        int t = 0;
                        for(t = 0; t < placed_plane.Count; t++)
                        {

                           foreach(var t1 in placed_plane[t])
                            {
                                if (t1[0] == i && t1[1] == j)
                                {
                                    myflag = 1;
                                    break;
                                }
                            }
                            if (myflag == 1)
                                break;
                        }
                       foreach(var p in placed_plane[t])
                        {
                            group_btn[p[0]][p[1]].BackColor=Color.White;
                            flag[p[0]][p[1]] = 0;
                        }
                        nowstate -= 2;
                        placed_plane.RemoveAt(t);
                        Head.RemoveAt(t);
                    }
                }
            }
            lbl1.Text ="现在已经摆放了"+ (nowstate / 2).ToString()+"台飞机";
            if (nowstate == 6)
                lbl2.Text = "放置完成";
            else if (nowstate == 1)
                lbl2.Text = "请在黄色网格内选择机尾";
            else
                lbl2.Text = "现在该确定机头位置了";
            }


        private void button2_Click(object sender, EventArgs e)
        {

            ((Form1)(this.Owner)).Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (nowstate != 6)
            {
                MessageBox.Show("请放置三架飞机后再点击此按钮");
                return;
            }
            if (state == 2)
            {
                
                Form3 frm3 = new Form3(state, flag, Head);

                frm3.initPlayer(Head, dir);
                
                frm3.Owner = this.Owner;
                frm3.Show();
                this.Hide();
                frm3.open();
                thread = new Thread(new ThreadStart(frm3.interaction));

                thread.Start();
            }
            else
            {
                Form3 frm3 = new Form3(state, flag, Head);

                frm3.initPlayer(Head,dir);

                frm3.Owner = this.Owner;
                frm3.Show();
                this.Hide();
            }
        }
    }
}
