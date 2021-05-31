using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Shape currentShape;
        int size,
             interval,
             x,
             y;
        int[,] map;
        int lineRemoved;
        int score;
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            size = 39;
            y = 13;
            x = 6;
            map = new int[y + 1, x];

            score = 0;
            lineRemoved = 0;
            label1.Text = "Score: " + score;
            label2.Text = "Lines : " + lineRemoved;

            currentShape = new Shape(1, 0);
            this.KeyDown += new KeyEventHandler(KeyFunc);

            interval = 400;
            timer1.Interval = interval;
            timer1.Tick += new EventHandler(Update);

            Invalidate();
        }

        private void KeyFunc(object sender, KeyEventArgs e)
        {
            if (timer1.Enabled)
            {
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        Update(this, new EventArgs());
                        break;
                    case Keys.Up:
                        if (!IsOverlapping())
                        {
                            ResetArea();
                            currentShape.RotateShape();
                            Merge();
                            Invalidate();
                        }
                        break;
                    case Keys.Left:
                        if (!CollideHor(-1))
                        {
                            ResetArea();
                            currentShape.MoveLeft();
                            Merge();
                            Invalidate();
                        }
                        break;
                    case Keys.Right:
                        if (!CollideHor(+1))
                        {
                            ResetArea();
                            currentShape.MoveRight();
                            Merge();
                            Invalidate();
                        }
                        break;
                }
            }
        }

        private void Update(object sender, EventArgs e)
        {
            timer1.Interval = interval;
            ResetArea();
            if (!Collide())
            {
                currentShape.MoveDown();
            }
            else
            {
                Merge();
                SliceMap();
                timer1.Interval = 1000;
                currentShape.ResetShape(1, 0);

                if (Collide())
                {
                    PauseOrPlay(this, new EventArgs());
                    MessageBox.Show($"       GAMEOVER ( ✖ _ ✖ )\n\tScore: {score}\n\tLines :{lineRemoved}");
                    score = 0;
                    lineRemoved = 0;
                    label1.Text = "Score: " + score;
                    label2.Text = "Lines : " + lineRemoved;
                    timer1.Interval = 400;
                    for (int i = 0; i < y; i++)
                    {
                        for (int j = 0; j < x; j++)
                        {
                            map[i, j] = 0;
                        }
                    }
                }
            }
            Merge();
            Invalidate();
        }
        public void SliceMap()
        {
            int count = 0;
            bool[,] toRemove = new bool[y, x];
            bool flag = false;

            //horizontal
            for (int i = 0; i < y; i++)
            {
                count = 0;
                int prev = 0;
                for (int j = 0; j < x; j++)
                {
                    if (map[i, j] != 0 && map[i, j] == prev)
                    {
                        count++;
                    }
                    else
                    {
                        if (count >= 3)
                        {
                            for (int k = j - count; k < j; k++)
                            {
                                toRemove[i, k] = true;
                            }
                        }
                        count = map[i, j] == 0 ? 0 : 1;
                    }
                    prev = map[i, j];
                }
                if (count >= 3)
                {
                    for (int k = x - count; k < x; k++)
                    {
                        toRemove[i, k] = true;
                    }
                }
                
            }

            //vertical
            for (int j = 0; j < x; j++)
            {
                count = 0;
                int prev = 0;
                for (int i = 0; i < y; i++)
                {
                    if (map[i, j] != 0 && map[i, j] == prev)
                    {
                        count++;
                    }
                    else
                    {
                        if (count >= 3)
                        {
                            for (int k = i - count; k < i; k++)
                            {
                                toRemove[k, j] = true;
                            }
                        }
                        count = map[i, j] == 0 ? 0 : 1;
                    }
                    prev = map[i, j];
                }
                if (count >= 3)
                {
                    for (int k = y - count; k < y; k++)
                    {
                        toRemove[k, j] = true;
                    }
                }

            }

            //diagonally
            for (int i = 0; i <= y; i++)
            {
                int d = i;
                count = 0;
                int prev = 0;
                for (int j = 0; j < x && d <= y; j++, d++)
                {
                    if (map[d, j] != 0 && map[d, j] == prev)
                    {
                        count++;
                    }
                    else
                    {
                        if (count >= 3)
                        {
                            for (int k = 1; k <= count; k++)
                            {
                                toRemove[d - k, j - k] = true;
                            }
                        }
                        count = map[d, j] == 0 ? 0 : 1;
                    }
                    prev = map[d, j];
                }
                if (count >= 3)
                {
                    for (int k = 1; k <= count; k++)
                    {
                        toRemove[d - k,x - k] = true;
                    }
                }
            }
            for (int i = 0; i <= y; i++)
            {
                int d = i;
                count = 0;
                int prev = 0;
                for (int j = x - 1; j >= 0 && d <= y; j--, d++)
                {
                    if (map[d, j] != 0 && map[d, j] == prev)
                    {
                        count++;
                    }
                    else
                    {
                        if (count >= 3)
                        {
                            for (int k = 1; k <= count; k++)
                            {
                                toRemove[d - k, j + k] = true;
                            }
                        }
                        count = map[d, j] == 0 ? 0 : 1;
                    }
                    prev = map[d, j];
                }
                if (count >= 3)
                {
                    for (int k = 1; k <= count; k++)
                    {
                        toRemove[d - k, k - 1] = true;
                    }
                }
            }

            for (int j = 0; j < x; j++)
            {
                for (int i = 0; i < y; i++)
                {
                    if (toRemove[i, j])
                    {
                        for (int k = i; k >= 1; k--)
                        {
                            map[k, j] = map[k - 1, j];
                        }
                        flag = true;
                    }
                }
            }

            if (flag)
            {
                SliceMap();
            }
        }
        public bool IsOverlapping()
        {
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (j >= 0 && j <= x - 1)
                    {
                        if (i >= 0 && i <= y - 1)
                        {
                            if (map[i, j] != 0 && currentShape.matrix[i - currentShape.y, j - currentShape.x] == 0)
                            {
                                return true;
                            }
                        }
                        if (i == y)
                        {
                            return true;
                        }
                    }
                    if (j == x)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void Merge()
        {
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)
                        map[i, j] = currentShape.matrix[i - currentShape.y, j - currentShape.x];
                }
            }
        }

        public bool Collide()
        {
            for (int i = currentShape.y + currentShape.sizeMatrix - 1; i >= currentShape.y; i--)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)
                    {
                        if (i + 1 == y)
                            return true;
                        if (map[i + 1, j] != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool CollideHor(int dir)
        {
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)
                    {
                        if (j + 1 * dir > x - 1 || j + 1 * dir < 0)
                            return true;
                        if (map[i, j + 1 * dir] != 0)
                        {
                            if (j - currentShape.x + 1 * dir >= currentShape.sizeMatrix || j - currentShape.x + 1 * dir < 0)
                            {
                                return true;
                            }
                            if (currentShape.matrix[i - currentShape.y, j - currentShape.x + 1 * dir] == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public void ResetArea()
        {
            for (int i = currentShape.y; i < currentShape.y + currentShape.sizeMatrix; i++)
            {
                for (int j = currentShape.x; j < currentShape.x + currentShape.sizeMatrix; j++)
                {
                    if (i >= 0 && j >= 0 && i < y && j < x)
                    {
                        if (currentShape.matrix[i - currentShape.y, j - currentShape.x] != 0)
                        {
                            map[i, j] = 0;
                        }
                    }
                }
            }
        }
        public void DrawNextShape(Graphics e)
        {
            for (int i = 0; i < currentShape.sizeNextMatrix; i++)
            {
                for (int j = 0; j < currentShape.sizeNextMatrix; j++)
                {
                    if (currentShape.nextMatrix[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.DeepSkyBlue, new Rectangle(350 + j * size + 3, 150 + i * size + 3, size - 3, size - 3));
                    }
                    if (currentShape.nextMatrix[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.LightBlue, new Rectangle(375 + j * size + 3, 150 + i * size + 3, size - 3, size - 3));
                    }
                    if (currentShape.nextMatrix[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.DarkOrange, new Rectangle(375 + j * size + 3, 150 + i * size + 3, size - 3, size - 3));
                    }
                    if (currentShape.nextMatrix[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.BlanchedAlmond, new Rectangle(375 + j * size + 3, 150 + i * size + 3, size - 3, size - 3));
                    }
                    if (currentShape.nextMatrix[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.LightGreen, new Rectangle(375 + j * size + 3, 150 + i * size + 3, size - 3, size - 3));
                    }
                    if (currentShape.nextMatrix[i, j] == 6)
                    {
                        e.FillRectangle(Brushes.Violet, new Rectangle(375 + j * size + 3, 175 + i * size + 3, size - 3, size - 3));
                    }
                    if (currentShape.nextMatrix[i, j] == 7)
                    {
                        e.FillRectangle(Brushes.FloralWhite, new Rectangle(375 + j * size + 3, 150 + i * size + 3, size - 3, size - 3));
                    }
                }
            }
        }
        public void DrawMap(Graphics e)
        {
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Brush brush = Brushes.Gray;
                    switch (map[i, j])
                    {
                        case 1:
                            brush = Brushes.DeepSkyBlue;
                            break;
                        case 2:
                            brush = Brushes.DarkRed;
                            break;
                        case 3:
                            brush = Brushes.DarkOrange;
                            break;
                        case 4:
                            brush = Brushes.DarkGreen;
                            break;
                        case 5:
                            brush = Brushes.Yellow;
                            break;
                        default:
                            break;
                    }
                    e.FillRectangle(brush, new Rectangle(50 + j * size + 3, 50 + i * size + 3, size - 3, size - 3));
                }
            }
        }
        public void DrawGridForNextShape(Graphics g)
        {
            for (int i = 0; i <= 4; i++)
            {
                g.DrawLine(Pens.Black, new Point(350, 150 + i * size), new Point(350 + 4 * size, 150 + i * size));
            }
            for (int i = 0; i <= 4; i++)
            {
                g.DrawLine(Pens.Black, new Point(350 + i * size, 150), new Point(350 + i * size, 150 + 4 * size));
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    g.FillRectangle(Brushes.Gray, new Rectangle(350 + j * size + 1, 150 + i * size + 1, size - 1, size - 1));
                }
            }
        }
        public void DrawGrid(Graphics g)
        {

            for (int i = 0; i <= y; i++)
            {
                g.DrawLine(Pens.Black, new Point(50, 50 + i * size), new Point(50 + 8 * size, 50 + i * size));
            }
            for (int i = 0; i <= x; i++)
            {
                g.DrawLine(Pens.Black, new Point(50 + i * size, 50), new Point(50 + i * size, 50 + 16 * size));
            }
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    g.FillRectangle(Brushes.Gray, new Rectangle(50 + j * size + 1, 50 + i * size + 1, size - 1, size - 1));
                }
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            DrawGrid(e.Graphics);
            DrawMap(e.Graphics);
            DrawGridForNextShape(e.Graphics);
            DrawNextShape(e.Graphics);
        }

        public void PauseOrPlay(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                pauseLabel.Visible = false;
                playButton.Text = "▶";
            }
            else
            {
                pauseLabel.Visible = false;
                timer1.Enabled = true;
                playButton.Text = "▉";
            }
        }
    }
}
