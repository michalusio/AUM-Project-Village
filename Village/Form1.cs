using System;
using System.Drawing;
using System.Windows.Forms;
using Village.Map;

namespace Village
{
    public partial class Form1 : Form
    {
        private const float CAMERA_SPEED = 5;

        private Board _board;
        private PointF _camera;
        private PointF _moveDir;
        private int _sizeRect;
        private float _zoom;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ui.Width = ClientSize.Width;
            ui.Height = ClientSize.Height;

            _camera = PointF.Empty;
            _zoom = 1;
            _sizeRect = 16; //size for one field

            _board = new Board(30, 30);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ui.Width = ClientSize.Width;
            ui.Height = ClientSize.Height;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _camera.X += _moveDir.X;
            _camera.Y += _moveDir.Y;
            foreach (var agent in _board.GetVillage().GetAgentList)
            {
                agent.DoAction();
            }
            ui.Invalidate();
        }

        private void ui_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SizeF s = new SizeF(_sizeRect * _zoom, _sizeRect * _zoom);
            for (var i = 0; i < _board.FullBoard.GetLength(1); i++)
            for (var j = 0; j < _board.FullBoard.GetLength(0); j++)
            {
                Field f = _board.FullBoard[j, i];
                g.FillRectangle(
                    f.GetBase()?Brushes.Black:f.GetCultivation()
                        ? f.GetFood().Value > 0 ? Brushes.DarkGreen : Brushes.Green
                        : Brushes.SaddleBrown,
                    ((j - _board.FullBoard.GetLength(0)) * _sizeRect - _camera.X) * _zoom + e.ClipRectangle.Width * 0.5f,
                    ((i - _board.FullBoard.GetLength(1)) * _sizeRect - _camera.Y) * _zoom +
                    e.ClipRectangle.Height * 0.5f,
                    s.Width,
                    s.Height);
            }

            foreach (var agent in _board.GetVillage().GetAgentList)
            {
                g.FillRectangle(Brushes.Blue,
                    ((agent.GetCurrentX - _board.FullBoard.GetLength(0)) * _sizeRect - _camera.X) * _zoom + e.ClipRectangle.Width * 0.5f,
                    ((agent.GetCurrentY - _board.FullBoard.GetLength(1)) * _sizeRect - _camera.Y) * _zoom +
                    e.ClipRectangle.Height * 0.5f,
                    s.Width,
                    s.Height);
            }
            g.DrawString(_board.GetVillage().GetTotalFood.ToString("F1"),new Font("Arial Black",10), Brushes.BlueViolet,10,10);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) _moveDir.Y = -CAMERA_SPEED / _zoom;
            if (e.KeyCode == Keys.Down) _moveDir.Y = CAMERA_SPEED / _zoom;
            if (e.KeyCode == Keys.Left) _moveDir.X = -CAMERA_SPEED / _zoom;
            if (e.KeyCode == Keys.Right) _moveDir.X = CAMERA_SPEED / _zoom;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) _moveDir.Y = 0;
            if (e.KeyCode == Keys.Down) _moveDir.Y = 0;
            if (e.KeyCode == Keys.Left) _moveDir.X = 0;
            if (e.KeyCode == Keys.Right) _moveDir.X = 0;
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
                _zoom *= 0.9f;
            if (e.Delta > 0)
                if (_zoom * 1.1f <= 1) _zoom *= 1.1f;
        }
    }
}