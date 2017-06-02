using System;
using System.Collections.Generic;
using System.Drawing;

namespace Village.Agents
{
    public class Graph
    {
        public Color Color = Color.Red;

        private readonly List<PointF> _points = new List<PointF>();
        private float _pointMod = 0.01f;
        private int _pointCount = 100;
        private float _maxYEver = 5;

        public int PointCount
        {
            get { return _pointCount; }
            set { _pointCount = value;
                _pointMod = 1f / _pointCount;
            }
        }

        public void Plot(Graphics g, Rectangle area)
        {
            float maxY = -1000000;
            foreach (var p in _points)
            {
                if (maxY < p.Y) maxY = p.Y;
            }
            _maxYEver = Math.Max(_maxYEver, maxY*1.25f);
            var pen = new Pen(Color, 2);
            for (var index = 1; index < _points.Count; index++)
            {
                var p = _points[index-1];
                var p2 = _points[index];
                g.DrawLine(pen, area.Left+p.X*area.Width,area.Bottom-p.Y*area.Height/ _maxYEver, area.Left + p2.X * area.Width, area.Bottom - p2.Y * area.Height / _maxYEver);
            }
            g.DrawRectangle(Pens.Black, area);
        }

        public void AddPoint(float y)
        {
            if (_points.Count == 0)
            {
                _points.Add(new PointF(0, y));
            }
            else
            {
                if (_points[_points.Count - 1].X >= 1-_pointMod)
                {
                    for (var index = _points.Count - 1; index >= 0; index--)
                    {
                        var p = _points[index];
                        _points[index]=new PointF(p.X - _pointMod, p.Y);
                        if (_points[index].X<0) _points.RemoveAt(index);
                    }
                }
                _points.Add(new PointF(_points[_points.Count-1].X + _pointMod, y));
            }
        }
    }
}