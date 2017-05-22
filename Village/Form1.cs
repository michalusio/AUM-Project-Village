using System;
using System.Drawing;
using System.Windows.Forms;
using Village.Agents;
using Village.Genes.Chromosomes;
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
        private Agent _selectedAgent;
        private int _speed;
        private int _tick;
        private readonly Font _fontArial = new Font("Arial Black", 10);

        public Form1()
        {
            InitializeComponent();
            _speed = 4;
            _tick = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ui.Width = ClientSize.Width;
            ui.Height = ClientSize.Height;

            _camera = new PointF(240,240);
            _zoom = 1;
            _sizeRect = 16; //size for one field

            _board = new Board(60, 60);
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
            if (_speed != 4)
            {
                _tick++;
                if (_tick >= _speed)
                {
                    _tick = 0;
                    _board.GetVillage().TickFood();
                    foreach (var agent in _board.GetVillage().GetAgentList)
                    {
                        agent.DoAction();
                    }
                }
            }
            ui.Invalidate();
        }

        private void ui_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle area = e.ClipRectangle;
            SizeF s = new SizeF(_sizeRect * _zoom, _sizeRect * _zoom);
            for (var i = 0; i < _board.FullBoard.GetLength(1); i++)
            for (var j = 0; j < _board.FullBoard.GetLength(0); j++)
            {
                Field f = _board.FullBoard[j, i];
                g.FillRectangle(
                    f.GetBase()?Brushes.Black:f.GetCultivation()
                        ? f.GetFood().Value > 0 ? Brushes.DarkGreen : Brushes.Green
                        : Brushes.SaddleBrown,
                    (j * _sizeRect - _camera.X) * _zoom + area.Width * 0.5f,
                    (i * _sizeRect - _camera.Y) * _zoom +
                    area.Height * 0.5f,
                    s.Width,
                    s.Height);
            }

            foreach (var agent in _board.GetVillage().GetAgentList)
            {
                g.FillRectangle(_selectedAgent==agent?Brushes.Red:Brushes.Blue,
                    (agent.GetCurrentX * _sizeRect - _camera.X) * _zoom + area.Width * 0.5f,
                    (agent.GetCurrentY * _sizeRect - _camera.Y) * _zoom +
                    area.Height * 0.5f,
                    s.Width,
                    s.Height);
            }
            g.DrawString("Food in Village: "+_board.GetVillage().GetTotalFood.ToString("F1"),_fontArial, Brushes.BlueViolet,10,10);
            if (_selectedAgent != null) DrawAgentDescription(g,area);
            g.DrawString("SPEED: "+(4-_speed),_fontArial,Brushes.Black, 10, 32);
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
            if (e.KeyCode == Keys.D1) _speed = 3;
            if (e.KeyCode == Keys.D2) _speed = 2;
            if (e.KeyCode == Keys.D3) _speed = 1;
            if (e.KeyCode == Keys.D0) _speed = 4;
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
                _zoom *= 0.9f;
            if (e.Delta > 0)
                if (_zoom * 1.1f <= 1) _zoom *= 1.1f;
        }

        private void ui_MouseClick(object sender, MouseEventArgs e)
        {
            float realX = ((e.X - ui.Width * 0.5f) / _zoom + _camera.X) / _sizeRect;
            float realY = ((e.Y - ui.Height * 0.5f) / _zoom + _camera.Y) / _sizeRect;
            Agent a = null;
            float minD = float.MaxValue;
            foreach (var agent in _board.GetVillage().GetAgentList)
            {
                float d = Extensions.Sqr(agent.GetCurrentX - realX) + Extensions.Sqr(agent.GetCurrentY - realY);
                if (d < 5 && d < minD)
                {
                    minD = d;
                    a = agent;
                }
            }
            _selectedAgent = a;
        }

        private void DrawAgentDescription(Graphics g, Rectangle area)
        {
            float height = 96 + Math.Max(FoodChromosome.GeneCount, MoveChromosome.GeneCount) * 24;
            g.FillRectangle(Brushes.Bisque,area.Width*0.8f,0,area.Width*0.2f, height);
            g.DrawString("Total food gathered: "+_selectedAgent.GetFood,_fontArial,Brushes.Black, area.Width * 0.8f, 0);
            g.DrawString("Age: "+_selectedAgent.GetAge,_fontArial,Brushes.Black,area.Width*0.8f,24);
            g.DrawString("Action: "+_selectedAgent.GetAction().Type,_fontArial,Brushes.Black,area.Width*0.8f,48);
            var sizeF=g.MeasureString("Genome:", _fontArial);
            g.DrawString("Genome:", _fontArial, Brushes.Black, area.Width * 0.9f -sizeF.Width*0.5f, 72);
            var f1 = _selectedAgent.GetGenome().GetChromosomes().Item1.Functions;
            for (var index = 0; index < FoodChromosome.GeneCount; index++)
            {
                var f = f1[index];
                g.DrawString(f.GetName(), _fontArial, Brushes.Black, area.Width * 0.8f, 96+index*24);
            }
            var f2 = _selectedAgent.GetGenome().GetChromosomes().Item2.Functions;
            for (var index = 0; index < MoveChromosome.GeneCount; index++)
            {
                var f = f2[index];
                sizeF = g.MeasureString(f.GetName(), _fontArial);
                g.DrawString(f.GetName(), _fontArial, Brushes.Black, area.Width - sizeF.Width, 96 + index * 24);
            }
            g.DrawRectangle(Pens.Black,area.Width*0.8f,72,area.Width*0.2f,24+Math.Max(FoodChromosome.GeneCount, MoveChromosome.GeneCount)*24);
            g.DrawLine(Pens.Black,area.Width*0.8f,96,area.Width,96);
            g.DrawLine(Pens.Black,area.Width*0.9f,96,area.Width*0.9f,height);
        }
    }
}