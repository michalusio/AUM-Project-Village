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
    public partial class MainForm : Form
    {
        private const float CAMERA_SPEED = 5;
        private const int GRASS_GROWTH = 20;

        public Board Board;
        private PointF _camera;
        private PointF _moveDir;
        private int _sizeRect;
        private float _zoom;
        private Agent _selectedAgent;
        private Agents.Village _selectedVillage;
        private int _tick;
        private readonly Font _fontArial = new Font("Arial Black", 10);

        private Image[] _cabbages, _carrots;
        private Image _holdCarrot;

        public GraphForm Graphs;

        public MainForm()
        {
            InitializeComponent();
            Graphs = new GraphForm {F = this};
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

            _carrots = new Image[5];
            _carrots[0] = Image.FromFile("textures/Carrot_0.png");
            _carrots[1] = Image.FromFile("textures/Carrot_1.png");
            _carrots[2] = Image.FromFile("textures/Carrot_2.png");
            _carrots[3] = Image.FromFile("textures/Carrot_3.png");
            _carrots[4] = Image.FromFile("textures/Carrot_4.png");

            _holdCarrot = Image.FromFile("textures/Carrot_5.png");
            Board = new Board(200, 200);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ui.Width = ClientSize.Width;
            ui.Height = ClientSize.Height-150;
            ui.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _camera.X += _moveDir.X;
            _camera.Y += _moveDir.Y;
            if (simSpeed.Value > 0 && simSpeed.Value < 4)
            {
                _tick++;
                if (_tick >= 4-simSpeed.Value)
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
            foreach (var v in Board.GetVillages())
            {
                v.TickFood();
                v.TickAge(agingSpeed.Value / 5f);
                v.ReproduceAgents();
                foreach (var agent in v.GetAgentList)
                {
                    agent.DoAction();
                }
            }
            for (int i = 0; i < GRASS_GROWTH * Board.FullBoard.Length / (60 * 60); i++)
            {
                int x = Genome.Rnd.Next(Board.FullBoard.GetLength(0));
                int y = Genome.Rnd.Next(Board.FullBoard.GetLength(1));
                var f = Board.FullBoard[x, y];
                if (f.GetGrass() < 0.99f && IsNearGrass(f))
                {
                    f.AddCultivation(grassGrowth.Value / 20f);
                }
                else
                {
                    if (f.GetGrass() > 0.25f) f.AddCultivation(grassGrowth.Value / 60f);
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
            for (var i = 0; i < Board.FullBoard.GetLength(0); i++)
            for (var j = 0; j < Board.FullBoard.GetLength(1); j++)
            {
                Field f = Board.FullBoard[i, j];
                    PointF p= new PointF(
                        (i * _sizeRect - _camera.X) * _zoom + area.Width * 0.5f,
                        (j * _sizeRect - _camera.Y) * _zoom + area.Height * 0.5f);
                g.FillRectangle(
                    f.GetBase()>-1?Board.GetVillages()[f.GetBase()].GetBrush() : new SolidBrush(InterpolateColor(Color.SaddleBrown, Color.Green, f.GetGrass())),
                    new RectangleF(p.X, p.Y, s.Width, s.Height));
                if (f.GetFood().Value > 0)
                {
                    var tab = f.GetFood().Level == 1 ? _cabbages : _carrots;
                    g.DrawImage(tab[(i*Board.FullBoard.GetLength(0)+j)% tab.Length], new RectangleF(p.X, p.Y, s.Width,s.Height));
                }
            }
            foreach (var v in Board.GetVillages())
            {
                foreach (var agent in v.GetAgentList)
                {
                    PointF p = new PointF(
                        (agent.GetCurrentX * _sizeRect - _camera.X) * _zoom + area.Width * 0.5f,
                        (agent.GetCurrentY * _sizeRect - _camera.Y) * _zoom + area.Height * 0.5f);
                    g.FillEllipse(
                        new SolidBrush(InterpolateColor(Color.White, Color.Black,
                            agent.GetAge / agent.GetGenome().GetDurability())),
                        p.X, p.Y,
                        s.Width,
                        s.Height);
                    g.FillEllipse(v.Brush, p.X +_sizeRect*_zoom*0.1f, p.Y + _sizeRect * _zoom * 0.1f, s.Width - _sizeRect * _zoom * 0.2f, s.Height - _sizeRect * _zoom * 0.2f);
                    if (agent.GetHoldedFood > 0)
                    {
                        g.DrawImage(
                            agent.GetHoldedType == 1
                                ? _cabbages[Math.Abs(agent.GetHashCode()) % _cabbages.Length]
                                : _holdCarrot,
                            new RectangleF(p.X, p.Y, s.Width * 0.75f, s.Height * 0.75f));
                    }
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
            if (_selectedVillage != null)
            {
                DrawVillage(g, _selectedVillage, area);
            }
        }

        private void DrawVillage(Graphics g, Agents.Village v, Rectangle area)
        {
            g.FillRectangle(Brushes.Bisque, 0, 0, 199, 628);
            g.DrawString("Food in Village: " + v.GetTotalFood.ToString("F1"), _fontArial, Brushes.BlueViolet, 0, 0);
            if (_selectedAgent != null) DrawAgentDescription(g, area);
            v.FoodGraph.Plot(g, new Rectangle(0, 20, 200, 200));
            g.DrawString("Agents in Village: " + v.GetAgentList.Count, _fontArial, Brushes.BlueViolet, 0, 264);
            v.PopGraph.Plot(g, new Rectangle(0, 284, 200, 200));
            g.DrawString("Genes in Village:", _fontArial, Brushes.BlueViolet, 0, 508);
            v.Genes.Plot(g, new Rectangle(0, 528, 200, 100));
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
            if (e.Delta < 0) _zoom *= 0.9f;
            else if (_zoom * 1.1f <= 5f) _zoom *= 1.1f;
        }

        private void ui_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float realX = ((e.X - ui.Width * 0.5f) / _zoom + _camera.X) / _sizeRect;
                float realY = ((e.Y - ui.Height * 0.5f) / _zoom + _camera.Y) / _sizeRect;
                object a = null;
                float minD = float.MaxValue;
                foreach (var v in Board.GetVillages())
                {
                    float d = Extensions.Sqr(v.VillageMain.X - realX) + Extensions.Sqr(v.VillageMain.Y - realY);
                    if (d < 5 && d < minD)
                    {
                        minD = d;
                        a = v;
                    }
                    foreach (var agent in v.GetAgentList)
                    {
                        d = Extensions.Sqr(agent.GetCurrentX - realX) + Extensions.Sqr(agent.GetCurrentY - realY);
                        if (d < 5 && d < minD)
                        {
                            minD = d;
                            a = agent;
                        }
                    }
                }
                if (_selectedAgent != null && a is Agents.Village)
                {
                    _selectedVillage = (Agents.Village) a;
                }
                else
                {
                    if (_selectedVillage != null && a is Agent)
                    {
                        _selectedAgent = (Agent) a;
                    }
                    else
                    {
                        if (_selectedAgent == null && _selectedVillage == null)
                        {
                            _selectedAgent = a as Agent;
                            _selectedVillage = a as Agents.Village;
                        } 
                    }
                }
            }
            if (e.Button == MouseButtons.Middle)
            {
                float realX = ((e.X - ui.Width * 0.5f) / _zoom + _camera.X) / _sizeRect;
                float realY = ((e.Y - ui.Height * 0.5f) / _zoom + _camera.Y) / _sizeRect;
                int x = (int) realX;
                int y = (int) realY;
                if (VillageValid(x, y)&&Board.FullBoard[x,y].GetBase()==-1)
                {
                    Agents.Village v = new Agents.Village(Board,new PointF(x,y));
                    int cv = Board.GetVillages().Count;
                    Board.FullBoard[x, y].SetBase(cv);
                    Board.FullBoard[x-1, y].SetBase(cv);
                    Board.FullBoard[x+1, y].SetBase(cv);
                    Board.FullBoard[x, y-1].SetBase(cv);
                    Board.FullBoard[x, y+1].SetBase(cv);
                    Board.GetVillages().Add(v);
                    for (int i = 0; i < Board.GetVillages().Count; i++)
                    {
                        v = Board.GetVillages()[i];
                        v.Brush=new SolidBrush(Extensions.HsvToRgb((360*i)/(float)Board.GetVillages().Count,1,1));
                    }
                }
            }
        }

        private bool VillageValid(int x, int y)
        {
            return Board.IsValid(x, y) && Board.IsValid(x - 1, y) && Board.IsValid(x + 1, y) && Board.IsValid(x, y - 1) &&
                   Board.IsValid(x, y + 1);
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
            Graphs.Show();
        }
    }
}