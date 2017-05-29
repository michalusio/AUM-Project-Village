using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Village.Agents;
using Village.Genes;
using Village.Genes.Chromosomes;
using Village.Map;

namespace Village
{
    public partial class Form1 : Form
    {
        private const float CAMERA_SPEED = 5;
        private const int GRASS_GROWTH = 20;

        private Board _board;
        private PointF _camera;
        private PointF _moveDir;
        private int _sizeRect;
        private float _zoom;
        private Agent _selectedAgent;
        private int _tick;
        private readonly Font _fontArial = new Font("Arial Black", 10);

        private Image[] _cabbages;

        public Form1()
        {
            InitializeComponent();
            _tick = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ui.Width = ClientSize.Width;
            ui.Height = ClientSize.Height-150;

            _camera = new PointF(800, 800);
            _zoom = 1;
            _sizeRect = 16; //size for one field

            _cabbages = new Image[7];
            _cabbages[0]=Image.FromFile("textures/Cabbage_0.png");
            _cabbages[1] = Image.FromFile("textures/Cabbage_1.png");
            _cabbages[2] = Image.FromFile("textures/Cabbage_2.png");
            _cabbages[3] = Image.FromFile("textures/Cabbage_3.png");
            _cabbages[4] = Image.FromFile("textures/Cabbage_4.png");
            _cabbages[5] = Image.FromFile("textures/Cabbage_5.png");
            _cabbages[6] = Image.FromFile("textures/Cabbage_6.png");
            _board = new Board(100, 100, new PointF(50,50));
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ui.Width = ClientSize.Width;
            ui.Height = ClientSize.Height-150;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _camera.X += _moveDir.X;
            _camera.Y += _moveDir.Y;
            if (simSpeed.Value > 0 && simSpeed.Value < 4)
            {
                _tick++;
                if (_tick >= simSpeed.Value)
                {
                    _tick = 0;
                    TickGame();
                }
            }
            if (simSpeed.Value > 3)
            {
                for(int i = 0; i < simSpeed.Value - 2; ++i) TickGame();
            }
            ui.Invalidate();
        }

        private void TickGame()
        {
            _board.GetVillage().TickFood();
            _board.GetVillage().TickAge(agingSpeed.Value/5f);
            _board.GetVillage().ReproduceAgents();
            foreach (var agent in _board.GetVillage().GetAgentList)
            {
                agent.DoAction();
            }
            for (int i = 0; i < GRASS_GROWTH * _board.FullBoard.Length / (60 * 60); i++)
            {
                int x = Genome.Rnd.Next(_board.FullBoard.GetLength(0));
                int y = Genome.Rnd.Next(_board.FullBoard.GetLength(1));
                var f = _board.FullBoard[x, y];
                if (f.GetGrass() < 0.99f && IsNearGrass(f))
                {
                    f.AddCultivation(grassGrowth.Value/20f);
                }
                else
                {
                    if (f.GetGrass()>0.25f) f.AddCultivation(grassGrowth.Value/60f);
                }
            }
        }

        private bool IsNearGrass(Field f)
        {
            return f.GetCultivation() || f.GetRelative(-1, 0).GetCultivation() || f.GetRelative(1, 0).GetCultivation() ||
                   f.GetRelative(0, -1).GetCultivation() || f.GetRelative(0, 1).GetCultivation();
        }

        private void ui_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode=InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode=PixelOffsetMode.Half;
            g.SmoothingMode=SmoothingMode.None;
            Rectangle area = e.ClipRectangle;
            SizeF s = new SizeF(_sizeRect * _zoom, _sizeRect * _zoom);
            for (var i = 0; i < _board.FullBoard.GetLength(0); i++)
            for (var j = 0; j < _board.FullBoard.GetLength(1); j++)
            {
                Field f = _board.FullBoard[i, j];
                    PointF p= new PointF(
                        (i * _sizeRect - _camera.X) * _zoom + area.Width * 0.5f,
                        (j * _sizeRect - _camera.Y) * _zoom + area.Height * 0.5f);
                g.FillRectangle(
                    f.GetBase()?Brushes.Black : new SolidBrush(InterpolateColor(Color.SaddleBrown, Color.Green, f.GetGrass())),
                    new RectangleF(p.X, p.Y, s.Width, s.Height));
                if (f.GetFood().Value > 0)
                {
                    g.DrawImage(_cabbages[(i*_board.FullBoard.GetLength(0)+j)%_cabbages.Length], new RectangleF(p.X, p.Y, s.Width,s.Height));
                }
            }

            foreach (var agent in _board.GetVillage().GetAgentList)
            {
                PointF p = new PointF(
                    (agent.GetCurrentX * _sizeRect - _camera.X) * _zoom + area.Width * 0.5f,
                    (agent.GetCurrentY * _sizeRect - _camera.Y) * _zoom + area.Height * 0.5f);
                g.FillEllipse(new SolidBrush(InterpolateColor(Color.Aqua,Color.DarkBlue,agent.GetAge/agent.GetGenome().GetDurability())),
                    p.X, p.Y,
                    s.Width,
                    s.Height);
                if (agent.GetHoldedFood > 0)
                {
                    g.DrawImage(_cabbages[Math.Abs(agent.GetHashCode())%_cabbages.Length],new RectangleF(p.X, p.Y, s.Width*0.75f,s.Height*0.75f));
                }
            }
            if (_selectedAgent != null)
            {
                g.DrawEllipse(new Pen(Color.Red,4*_zoom), (_selectedAgent.GetCurrentX * _sizeRect - _camera.X) * _zoom + area.Width * 0.5f,
                    (_selectedAgent.GetCurrentY * _sizeRect - _camera.Y) * _zoom +
                    area.Height * 0.5f,
                    s.Width,
                    s.Height);
            }
            g.FillRectangle(Brushes.Bisque, 0,0,199,748);
            g.DrawString("Food in Village: "+_board.GetVillage().GetTotalFood.ToString("F1"),_fontArial, Brushes.BlueViolet,0,0);
            if (_selectedAgent != null) DrawAgentDescription(g,area);
            _board.GetVillage().FoodGraph.Plot(g,new Rectangle(0,20,200,200));
            g.DrawString("Agents in Village: " + _board.GetVillage().GetAgentList.Count, _fontArial, Brushes.BlueViolet, 0, 264);
            _board.GetVillage().PopGraph.Plot(g, new Rectangle(0, 284, 200, 200));
            g.DrawString("Scavenge genes in Village:", _fontArial, Brushes.BlueViolet, 0, 508);
            _board.GetVillage().Genes.Plot(g, new Rectangle(0, 528, 200, 100));
        }

        private Color InterpolateColor(Color a, Color b, float t)
        {
            return Color.FromArgb((int) (a.R + t * (b.R - a.R)), (int) (a.G + t * (b.G - a.G)), (int) (a.B + t * (b.B - a.B)));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) _moveDir.Y = -CAMERA_SPEED / _zoom;
            if (e.KeyCode == Keys.S) _moveDir.Y = CAMERA_SPEED / _zoom;
            if (e.KeyCode == Keys.A) _moveDir.X = -CAMERA_SPEED / _zoom;
            if (e.KeyCode == Keys.D) _moveDir.X = CAMERA_SPEED / _zoom;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) _moveDir.Y = 0;
            if (e.KeyCode == Keys.S) _moveDir.Y = 0;
            if (e.KeyCode == Keys.A) _moveDir.X = 0;
            if (e.KeyCode == Keys.D) _moveDir.X = 0;
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
                _zoom *= 0.9f;
            if (e.Delta > 0)
                if (_zoom * 1.1f <= 5f) _zoom *= 1.1f;
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
            g.DrawString("Food/Age gathered: "+_selectedAgent.GetFood/_selectedAgent.GetAge,_fontArial,Brushes.Black, area.Width * 0.8f, 0);
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

        private void button1_Click(object sender, EventArgs e)
        {
            _selectedAgent = null;
            _board = new Board(100, 100, new PointF(50, 50));
        }
    }
}