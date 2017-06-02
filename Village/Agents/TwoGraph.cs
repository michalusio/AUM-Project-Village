using System;
using System.Collections.Generic;
using System.Drawing;

namespace Village.Agents
{
    public class TwoGraph
    {
        public Color Color1 = Color.Red;
        public Color Color2 = Color.Blue;

        private readonly List<PointF> _points1 = new List<PointF>();
        private readonly List<PointF> _points2 = new List<PointF>();
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
            for (var index = 0; index < _points1.Count; index++)
            {
                var p = _points1[index];
                if (maxY < p.Y) maxY = p.Y;
                p = _points2[index];
                if (maxY < p.Y) maxY = p.Y;
            }
            _maxYEver = Math.Max(_maxYEver, maxY*1.25f);
            var pen1 = new Pen(Color1, 2);
            var pen2 = new Pen(Color2, 2);
            for (var index = 1; index < _points1.Count; index++)
            {
                var p = _points1[index-1];
                var p2 = _points1[index];
                g.DrawLine(pen1, area.Left+p.X*area.Width,area.Bottom-p.Y*area.Height/ _maxYEver, area.Left + p2.X * area.Width, area.Bottom - p2.Y * area.Height / _maxYEver);
                p = _points2[index - 1];
                p2 = _points2[index];
                g.DrawLine(pen2, area.Left + p.X * area.Width, area.Bottom - p.Y * area.Height / _maxYEver, area.Left + p2.X * area.Width, area.Bottom - p2.Y * area.Height / _maxYEver);
            }
            g.DrawRectangle(Pens.Black, area);
        }

        public void AddPoint(float y, float y2)
        {
            if (_points1.Count == 0)
            {
                _points1.Add(new PointF(0, y));
                _points2.Add(new PointF(0, y2));
            }
            else
            {
                if (_points1[_points1.Count - 1].X >= 1-_pointMod)
                {
                    for (var index = _points1.Count - 1; index >= 0; index--)
                    {
                        var p = _points1[index];
                        _points1[index]=new PointF(p.X - _pointMod, p.Y);
                        if (_points1[index].X<0) _points1.RemoveAt(index);
                        p = _points2[index];
                        _points2[index] = new PointF(p.X - _pointMod, p.Y);
                        if (_points2[index].X < 0) _points2.RemoveAt(index);
                    }
                }
                _points1.Add(new PointF(_points1[_points1.Count - 1].X + _pointMod, y));
                _points2.Add(new PointF(_points2[_points2.Count - 1].X + _pointMod, y2));
            }
        }
    }
}