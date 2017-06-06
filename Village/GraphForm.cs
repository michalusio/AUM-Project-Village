using System.Drawing;
using System.Windows.Forms;
using Village.Map;

namespace Village
{
    public partial class GraphForm : Form
    {
        public MainForm F;

        public GraphForm()
        {
            InitializeComponent();
            Hide();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var third = e.ClipRectangle.Height / 3;
            var w = 100000;
            var graph = F.Board.GetVillages()[0].WholeFoodGraph;
            graph.Plot(g, new Rectangle(((e.ClipRectangle.Width - w) * hScrollBar1.Value) / 100, 0, w, third));
            graph = F.Board.GetVillages()[0].WholePopGraph;
            graph.Plot(g, new Rectangle(((e.ClipRectangle.Width - w) * hScrollBar1.Value) / 100, third, w, third));
            var graph2 = F.Board.GetVillages()[0].WholeGenes;
            graph2.Plot(g, new Rectangle(((e.ClipRectangle.Width - w) * hScrollBar1.Value) / 100, third*2, w, third));
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void GraphForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            F.Board = new Board(100, 100);
            F.Graphs = new GraphForm {F = F};
        }
    }
}
