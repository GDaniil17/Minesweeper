using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Сапёр
{
    public partial class Form1 : Form
    {

        int count_seconds = 0;
        int[,] directions = { { -1, 0 }, { -1, -1, }, { -1, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { 0, 1 } };
        int size = 12;
        public int count_bombs;
        int count = 0;
        Random rand = new Random();
        Bomb_or_Cell[,] field;
        int s = 0;
        bool game_just_started = false;


        void ShowBombs()
        {
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    field[i, j].btn.Text = field[i, j].status.Text;
                    field[i, j].btn.Font = new Font("Times New Roman", 44, field[i, j].btn.Font.Style | FontStyle.Bold);
                    if (field[i, j].isBomb == 1)
                    {
                        field[i, j].btn.BackColor = Color.Gold;
                    }
                    else
                    {
                        field[i, j].btn.BackColor = Color.Red;
                    }
                }
            }
        }

        bool Exist(int x, int y)
        {
            return (x > -1 && x < size && y > -1 && y < size);
        }

        void Reveal(Bomb_or_Cell cell)
        {

            if (game_just_started)
            {
                s = 0;
                game_just_started = false;
                Console.WriteLine("IMMMMMMMMM Working!!!!!!!!!");
                timer1.Start();
            }
            Console.WriteLine("Clicked {0} {1}", cell.X, cell.Y);
            cell.isOpened = true;
            cell.status.Font = new Font("Times New Roman", 44, cell.status.Font.Style | FontStyle.Bold);
            cell.status.Show();
            cell.btn.Hide();
            count += 1;
            cell.btn.Hide();
            int new_x;
            int new_y;
            Bomb_or_Cell t;

            for (int i = 0; i < 8; i++)
            {
                new_x = cell.X + directions[i, 0];
                new_y = cell.Y + directions[i, 1];
                if (Exist(new_x, new_y) && field[new_x, new_y].isBomb == 0 && field[new_x, new_y].isOpened == false && field[new_x, new_y].status.Text == "0")
                {
                    t = field[new_x, new_y];
                    OnCellClicked(t);
                }
            }

        }
        void OnCellClicked(Bomb_or_Cell cell)
        {
            if (cell.isBomb == 1)
            {
                textBox1.Text = "0";
                timer1.Stop();
                    textBox1.Hide();
                ShowBombs();
                MessageBox.Show("Игра окончена!");
                toolStripSplitButton1.HideDropDown();
                panel1.Hide();
                toolStrip1.Hide();

                InitializeComponent();
            }
            else
            {
                //Console.WriteLine("count {0}", count);
                Reveal(cell);
                //Console.WriteLine("count {0}", count);
                if (size * size - count == count_bombs)
                {
                    textBox1.Text = "0";
                    timer1.Stop();
                    textBox1.Hide();
                    MessageBox.Show(String.Format("You won! \n In {0} second(-s) and haven't launched {1} bomb(-s)", s.ToString(), count_bombs));
                    toolStripSplitButton1.HideDropDown();
                    panel1.Hide();
                    toolStrip1.Hide();

                    InitializeComponent();
                }
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public Form1()
        {
            InitializeComponent();
            //InitField(size);
        }



        void InitField(int _bombs)
        {
            s = 0;
            game_just_started = true;
            panel1.Size = new System.Drawing.Size(90*size, 90*(size+1));
            panel1.MinimumSize = new System.Drawing.Size(90 * size, 90 * (size+1));
            panel1.MaximumSize = new System.Drawing.Size(90 * size, 90 * (size+1));
            count_bombs = Math.Min(_bombs, size * size);
            //Console.WriteLine("///{0} {1}", _bombs, size);
            count = 0;
            field = new Bomb_or_Cell[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    field[i, j] = new Bomb_or_Cell();
                    field[i, j].X = i;
                    field[i, j].Y = j;
                    field[i, j].isOpened = false;
                    field[i, j].isBomb = 0;
                    field[i, j].status = new Label();
                    field[i, j].status.Font = new Font("Times New Roman", 44, field[i, j].status.Font.Style | FontStyle.Bold);
                    field[i, j].btn = new Button();


                }
            }

            for (int i = 0; i < Math.Min(count_bombs, size*size); i++)
            {
                int x = rand.Next(0, size);
                int y = rand.Next(0, size);
                while (field[x, y].isBomb == 1)
                {
                    x = rand.Next(0, size);
                    y = rand.Next(0, size);

                }
                field[x, y].isBomb = 1;

            }
            for (int x = 0; x < size; ++x)
            {
                for (int y = 0; y < size; ++y)
                {

                    Label l = new Label();
                    if (field[x, y].isBomb == 1)
                    {
                        l.Text = "☠";
                    }
                    else
                    {
                        l.Text = field[x, y].Risk.ToString();
                    }
                    l.SetBounds(90 * x, 90 * (y+1), 90, 90);
                    l.Font = new Font("Times New Roman", 44, l.Font.Style | FontStyle.Bold);
                    l.Parent = panel1;
                    l.Hide();
                    field[x, y].status = l;

                    Bomb_or_Cell t = new Bomb_or_Cell();
                    field[x, y].X = x;
                    field[x, y].Y = y;
                    field[x, y].isOpened = false;
                    //field[x, y] = t;
                    field[x, y].btn = new Button();
                    field[x, y].btn.SetBounds(90 * x, 90 * (y+1), 90, 90);

                    field[x, y].btn.Parent = panel1;
                    t = field[x, y];

                    field[x, y].btn.Click += (sender, e) => OnCellClicked(t);
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i > 0)
                    {
                        field[i, j].Risk += field[i - 1, j].isBomb;
                        if (j < size - 1)
                        {
                            field[i, j].Risk += field[i - 1, j + 1].isBomb;
                        }
                        if (j > 0)
                        {
                            field[i, j].Risk += field[i - 1, j - 1].isBomb;
                        }
                    }
                    if (i < size - 1)
                    {
                        field[i, j].Risk += field[i + 1, j].isBomb;
                        if (j < size - 1)
                        {
                            field[i, j].Risk += field[i + 1, j + 1].isBomb;
                        }
                        if (j > 0)
                        {
                            field[i, j].Risk += field[i + 1, j - 1].isBomb;
                        }
                    }
                    if (j < size - 1)
                    {
                        field[i, j].Risk += field[i, j + 1].isBomb;
                    }
                    if (j > 0)
                    {
                        field[i, j].Risk += field[i, j - 1].isBomb;
                    }

                    Label l = new Label();
                    if (field[i, j].isBomb == 1)
                    {
                        l.Text = "☠";
                    }
                    else
                    {
                        l.Text = field[i, j].Risk.ToString();
                    }
                    l.SetBounds(90 * i, 90 * (j+1), 90, 90);
                    l.Parent = panel1;
                    l.Hide();
                    field[i, j].status = l;



                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripSplitButton1.HideDropDown();
            panel1.Hide();
            toolStrip1.Hide();
            textBox1.Hide();

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = richTextBox1.Text;
            bool tr = true;
            string s2 = richTextBox4.Text;
            bool tr2 = true;
            if (s.Length != 0)
            {
                foreach (char i in s)
                {
                     if (Char.IsDigit(i) == false)
                    {
                        tr = false;
                    }
                }
            }
            if (s2.Length != 0)
            {
                foreach (char i in s2)
                {
                    if (Char.IsDigit(i) == false)
                    {
                        tr2 = false;
                    }
                }
            }

            if (tr2 && s.Length != 0)
            {
                size = Math.Max(2, Math.Min(Int32.Parse(s2), (Screen.PrimaryScreen.WorkingArea.Height-50)/100));
            }
            else
            {
                size = 2;
            }

            if (tr && s.Length != 0)
            {
                //InitializeComponent();
                richTextBox1.Hide();
                richTextBox2.Hide();
                richTextBox3.Hide();
                richTextBox4.Hide();
                button1.Hide();
                count_bombs = Math.Max(1, Math.Min(Int32.Parse(s), size*size));
                InitField(count_bombs);
                Btn();
            }
            else
            {
                //InitializeComponent();
                richTextBox1.Hide();
                richTextBox2.Hide();
                richTextBox3.Hide();
                richTextBox4.Hide();
                button1.Hide();
                count_bombs = Math.Max(1, Math.Min(5, size*size));
                InitField(5);
                Btn();
            }
        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            s += 1;
            textBox1.Text = s.ToString();

        }
    }

    public class Bomb_or_Cell
    {
        public sbyte isBomb;
        public int Risk;
        public bool isOpened;
        public int X;
        public int Y;
        public Button btn;
        public Label status;
    }
}
