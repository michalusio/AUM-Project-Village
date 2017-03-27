using System;
using System.Drawing;
using System.Windows.Forms;
using Village.Map;

namespace Village
{
    public partial class Form1 : Form
    {
        private Board board;
        private PointF Camera;
        private int sizeRect;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ui.Width = ClientSize.Width;
            ui.Height = ClientSize.Height;

            Camera =new PointF(0,0);
            sizeRect = 16;  //size for one field

            board = new Board(50, 50);

            timer.Enabled = true;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ui.Width = ClientSize.Width;
            ui.Height = ClientSize.Height;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ui.Invalidate();
        }

        private void ui_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Size s = new Size(sizeRect, sizeRect);
            for (int i = 0; i < board.FullBoard.GetLength(1); i++)
            {
                for (int j = 0; j < board.FullBoard.GetLength(0); j++)
                {
                    g.FillRectangle(
                        board.FullBoard[j, i].getCultivation() ? Brushes.Green : Brushes.SaddleBrown,
                        j * sizeRect - Camera.X,
                        i * sizeRect - Camera.Y,
                        s.Width,
                        s.Height);
                }
            }

        }
    }
}