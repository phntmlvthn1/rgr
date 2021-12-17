using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace rgr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int start = 0;
        List<Edge> E = new List<Edge>();
        public List<Edge> MST = new List<Edge>();
        static int n = 25;
        static int[,] arr = new int[n, n];
        public int cnt = 0;
        Bitmap bmp = new Bitmap(1800, 800);
        MyStorage str = new MyStorage();

        public Bitmap Image { get; internal set; }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(bmp);
            Form1 pb = sender as Form1;
            void algorithmByPrim(int numberV, List<Edge> E, List<Edge> MST)
            {
                
                //неиспользованные ребра
                List<Edge> notUsedE = new List<Edge>(E);
                    //использованные вершины
                    List<int> usedV = new List<int>();
                    //неиспользованные вершины
                    List<int> notUsedV = new List<int>();
                    for (int i = 0; i < numberV; i++)
                        notUsedV.Add(i);
                    //выбираем случайную начальную вершину
                    Random rand = new Random();
                    usedV.Add(rand.Next(0, numberV));
                    notUsedV.RemoveAt(usedV[0]);
                    while (notUsedV.Count > 0)
                    {
                        int minE = -1; //номер наименьшего ребра
                                       //поиск наименьшего ребра
                        for (int i = 0; i < notUsedE.Count; i++)
                        {
                            if ((usedV.IndexOf(notUsedE[i].v1) != -1) && (notUsedV.IndexOf(notUsedE[i].v2) != -1) ||
                                (usedV.IndexOf(notUsedE[i].v2) != -1) && (notUsedV.IndexOf(notUsedE[i].v1) != -1))
                            {
                                if (minE != -1)
                                {
                                    if (notUsedE[i].weight < notUsedE[minE].weight)
                                        minE = i;
                                }
                                else
                                    minE = i;
                            }
                        }
                        //заносим новую вершину в список использованных и удаляем ее из списка неиспользованных
                        if (usedV.IndexOf(notUsedE[minE].v1) != -1)
                        {
                            usedV.Add(notUsedE[minE].v2);
                            notUsedV.Remove(notUsedE[minE].v2);
                        }
                        else
                        {
                            usedV.Add(notUsedE[minE].v1);
                            notUsedV.Remove(notUsedE[minE].v1);
                        }
                        //заносим новое ребро в дерево и удаляем его из списка неиспользованных
                        MST.Add(notUsedE[minE]);
                        notUsedE.RemoveAt(minE);
                    
                }
            }         
            algorithmByPrim(cnt, E, MST);
            str.DrawGreenLines(MST, cnt, pb, bmp, g);
            this.Refresh();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {

            Graphics g = Graphics.FromImage(bmp);
            Form1 pb = (Form1)sender;
            int ves = 1;
            if (textBox3.Text != "")
                ves = Int32.Parse(textBox3.Text);
            
            if (str.isHit(ves, E, e.X, e.Y, pb, bmp, g))
                this.Refresh();
            else
            {

                str.Add(new CCircle(e.X, e.Y), pb, bmp, g);
                cnt++;
                this.Refresh();
            }

        }
        public class CCircle
        {

            public int x, y, num,ves;
            public bool isSelected = false;
            public int rad = 10;
            public CCircle(int x_, int y_)
            {
                x = x_;
                y = y_;
            }
            ~CCircle()
            {

            }

            public void DrawCircleBlack(int size, Form1 sender, Bitmap bmp, Graphics g)
            {
                num = size + 1;
                Rectangle rect = new Rectangle(x - rad, y - rad, rad * 2, rad * 2);
                Pen pen = new Pen(Color.Black, 8);
                Font font = new Font("Arial", 15, FontStyle.Regular);

                isSelected = true;
                g.DrawEllipse(pen, rect);

                g.DrawString((num).ToString(), font, Brushes.Black, x - 10, y - 10);
                sender.BackgroundImage = bmp;
                Zalivka(sender, bmp, g);
            }

            public void Zalivka(Form1 sender, Bitmap bmp, Graphics g)
            {
                Font font = new Font("Arial", 15, FontStyle.Regular);
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, x - rad, y - rad, rad * 2, rad * 2);
                g.DrawString((num).ToString(), font, Brushes.Black, x - 10, y - 10);

            }

            public void ZalivkaGreen(Form1 sender, Bitmap bmp, Graphics g)
            {
                Font font = new Font("Arial", 15, FontStyle.Regular);
                SolidBrush brush = new SolidBrush(Color.Green);
                g.FillEllipse(brush, x - rad, y - rad, rad * 2, rad * 2);
                g.DrawString((num).ToString(), font, Brushes.White, x - 10, y - 10);

            }

            public void DrawCircleGreen(int size, Form1 sender, Bitmap bmp, Graphics g)
            {
                Rectangle rect = new Rectangle(x - rad, y - rad, rad * 2, rad * 2);
                Pen pen = new Pen(Color.Green, 3);
                Font font = new Font("Arial", 15, FontStyle.Regular);
                isSelected = true;
                g.DrawEllipse(pen, rect);
                g.DrawString((size + 1).ToString(), font, Brushes.Green, x - 10, y - 10);
                sender.BackgroundImage = bmp;
            }

            public void DrawLine(int v, int x1, int y1, int x2, int y2, Form1 sender, Bitmap bmp, Graphics g)
            {
                Font font = new Font("Arial", 15, FontStyle.Regular);
                Pen p = new Pen(Color.DimGray, 3);
                Point p1 = new Point(x1, y1);
                Point p2 = new Point(x2, y2);
                int a = (x1+x2)/2;
                int b = (y1 + y2) / 2;
                g.DrawLine(p, p1, p2);
                g.DrawString((v).ToString(), font, Brushes.Black,a, b);
            }

            public void DrawLineGreen(int x1, int y1, int x2, int y2, Form1 sender, Bitmap bmp, Graphics g)
            {
                Pen p = new Pen(Color.Green, 3);
                Point p1 = new Point(x1, y1);
                Point p2 = new Point(x2, y2);
                g.DrawLine(p, p1, p2);

            }

            public bool isHit(int x_, int y_)
            {
                if (((x - rad) < x_) && (x + rad > x_) && ((y - rad - rad) < y_) && (y + rad > y_))
                {

                    return true;
                }
                else
                    return false;
            }

            public int GetCoorX()
            {
                return (x);
            }
            public int GetCoorY()
            {
                return (y);
            }
        }

        public class MyStorage
        {
            static int[,] arr2 = new int[25, 25];

            static public int size = 0;
            static public int dlc = 0;
            static public int x1, x2, y1, y2;
            static public int dl1 = -1;
            static public int dl2 = -1;
            static public CCircle[] objects;

            public MyStorage()
            {
                objects = new CCircle[100];
                for (int i = 0; i < 25; i++)
                    for (int j = 0; j < 25; j++)
                        arr2[i, j] = 0;
            }

            ~MyStorage()
            {

            }

            public void Drawing(int index, Form1 sender, Bitmap bmp, Graphics g)
            {
                if (objects[index] != null)
                    objects[index].DrawCircleBlack(size, sender, bmp, g);

            }


            public int GetSize()
            {
                return (size);
            }
            public void ZalivkaGreen(int index, Form1 sender, Bitmap bmp, Graphics g)
            {
                objects[index].ZalivkaGreen(sender, bmp, g);
            }
            public void GetArr(int[,] arr)
            {
                for (int i = 0; i < 25; i++)
                    for (int j = 0; j < 25; j++)
                        arr[i, j] = arr2[i, j];
            }
            public void Add( CCircle obj, Form1 sender, Bitmap bmp, Graphics g)
            {

                objects[size] = obj;
                Drawing(size, sender, bmp, g);
                size++;

            }
            public void DrawL(int ves, List<Edge> a, Form1 sender, Bitmap bmp, Graphics g)
            {
                x1 = objects[dl1].GetCoorX();
                y1 = objects[dl1].GetCoorY();
                x2 = objects[dl2].GetCoorX();
                y2 = objects[dl2].GetCoorY();
                objects[0].DrawLine(ves, x1, y1, x2, y2, sender, bmp, g);
                objects[dl1].Zalivka(sender, bmp, g);
                objects[dl2].Zalivka(sender, bmp, g);
                arr2[dl1, dl2] = 1;
                arr2[dl2, dl1] = 1;
                a.Add(new Edge(dl1, dl2, ves));

            }
            public void DrawLG(Form1 sender, Bitmap bmp, Graphics g)
            {
                x1 = objects[dl1].GetCoorX();
                y1 = objects[dl1].GetCoorY();
                x2 = objects[dl2].GetCoorX();
                y2 = objects[dl2].GetCoorY();
                objects[0].DrawLineGreen(x1, y1, x2, y2, sender, bmp, g);
                objects[dl1].ZalivkaGreen(sender, bmp, g);
                objects[dl2].ZalivkaGreen(sender, bmp, g);

            }
            public void DrawLG2(int dl1, int dl2, Form1 sender, Bitmap bmp, Graphics g)
            {

                x1 = objects[dl1].GetCoorX();
                y1 = objects[dl1].GetCoorY();
                x2 = objects[dl2].GetCoorX();
                y2 = objects[dl2].GetCoorY();
                objects[0].DrawLineGreen(x1, y1, x2, y2, sender, bmp, g);
                objects[dl1].ZalivkaGreen(sender, bmp, g);
                objects[dl2].ZalivkaGreen(sender, bmp, g);

            }
            public void DrawAll(List<Edge> a, Form1 sender, Bitmap bmp, Graphics g)
            {
                for (int i = 0; i < size; i++)
                {
                    if (objects[i] != null)
                        objects[i].Zalivka(sender, bmp, g);
                    for (int j = 0; j < size; j++)
                    {
                        if (arr2[i, j] == 1)
                        {
                            dl1 = i;
                            dl2 = j;
                            DrawL(1,a, sender, bmp, g);
                        }
                    }
                }
                dl1 = -1;
                dl2 = -1;
                dlc = 0;
            }
            public void DLG(int index, Form1 sender, Bitmap bmp, Graphics g)
            {
                if (dl1 == -1)
                {
                    dl1 = index;
                    dlc++;
                }
                else
                {
                    dl2 = index;
                    dlc++;
                }
                if (dlc == 2)
                {
                    DrawLG(sender, bmp, g);
                    dl1 = dl2;
                    dl2 = -1;
                    dlc = 1;
                }

            }
            public bool isHit(int v, List<Edge> a, int x, int y, Form1 sender, Bitmap bmp, Graphics g)
            {
                for (int i = 0; i < size; i++)
                {
                    if (objects[i] != null)
                        if (objects[i].isHit(x, y))
                        {
                            if (dl1 == -1)
                            {
                                dl1 = i;
                                dlc++;
                            }
                            else
                            {
                                dl2 = i;
                                dlc++;
                            }
                            if (dlc == 2)
                                if (dl1 != dl2)
                                {
                                    DrawL(v, a, sender, bmp, g);
                                    dl1 = -1;
                                    dl2 = -1;
                                    dlc = 0;
                                }
                                else
                                {
                                    dl1 = -1;
                                    dl2 = -1;
                                    dlc = 0;
                                }
                            return true;
                        }
                }
                return false;
            }

            public void DrawGreenLines(List<Edge> M, int cnt, Form1 sender, Bitmap bmp, Graphics g)
            {
                for (int i = 0; i < M.Count; i++)
                            DrawLG2(M[i].v1, M[i].v2, sender, bmp, g);

            }
        }

        public class Edge
        {
            public int v1, v2;

            public int weight;

            public Edge(int v1, int v2, int weight)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.weight = weight;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                start = Int32.Parse(textBox4.Text);
                start--;
            }
            else
                start = 0;
        }
    }
    
}
