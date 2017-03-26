using System;
using System.Drawing;
using System.Windows.Forms;


namespace Village
{
    public partial class Form1 : Form
    {
        private Village.Board.Board board;
        private int paddingX, paddingY;
        private int sizeRect;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ui.Width = Width;
            ui.Height = Height;

            paddingX = 5;
            paddingY = 5;
            sizeRect = 3;

            board = new Village.Board.Board(50, 50, 2);
        }

        private void ui_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int i = 0; i < board.fullBoard.GetLength(1); i++)
            {
                for (int j = 0; j < board.fullBoard.GetLength(0); j++)
                {
                    Point p = new Point(j * sizeRect + paddingX, i * sizeRect + paddingY);
                    Size s = new Size(sizeRect, sizeRect);

                    if (board.fullBoard[j, i].getCultivation())
                        g.FillRectangle(Brushes.Green, p.X, p.Y, s.Width, s.Height);
                    else
                        g.FillRectangle(Brushes.Brown, p.X, p.Y, s.Width, s.Height);
                }
            }

        }
    }
}