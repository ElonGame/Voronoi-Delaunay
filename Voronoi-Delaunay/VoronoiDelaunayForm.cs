﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using CSPoint = System.Drawing.Point;

namespace Voronoi_Delaunay
{
    public partial class VoronoiDelaunayForm : Form
    {
        Graphics g;
        Random seeder;
        Bitmap backImage;
        int pointCount;

        Triangle superTriangle = new Triangle();
        List<Point> points = new List<Point>();
        List<Triangle> delaunayTriangleList = new List<Triangle>();
        List<Edge> delaunayEdgeList = new List<Edge>();
        List<Edge> voronoiEdgeList = new List<Edge>();

        public VoronoiDelaunayForm()
        {
            InitializeComponent();
            seeder = new Random();
            backImage = new Bitmap(1200, 900);
            g = Graphics.FromImage(backImage);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.Black);

            pictureBox1.Image = backImage;
        }

        public void SpreadPoints()
        {
            g.Clear(Color.Black);

            int seed = seeder.Next();
            Random rand = new Random(seed);

            for (int i = 0; i < pointCount; i++)
            {
                PointF p = new PointF((float)(rand.NextDouble() * 1190), (float)(rand.NextDouble() * 763));
                points.Add(new Point(p.X, p.Y, 0));
            }

            for (int i = 0; i < points.Count; i++)
            {
                g.FillEllipse(Brushes.White, (float)(points[i].x - 1.5f), (float)(points[i].y - 1.5f), 3, 3);
            }

            pictureBox1.Image = backImage;
        }

        public void DelaunayTriangulate()
        {
            superTriangle = Delaunay.SuperTriangle(points);
            delaunayTriangleList = Delaunay.Triangulate(superTriangle, points);
            delaunayEdgeList = Delaunay.DelaunayEdges(superTriangle, delaunayTriangleList);
            for (int i = 0; i < delaunayEdgeList.Count; i++)
            {
                CSPoint p1 = new CSPoint((int)delaunayEdgeList[i].start.x, (int)delaunayEdgeList[i].start.y);
                CSPoint p2 = new CSPoint((int)delaunayEdgeList[i].end.x, (int)delaunayEdgeList[i].end.y);
                g.DrawLine(Pens.Blue, p1.X, p1.Y, p2.X, p2.Y);
            }
            for (int i = 0; i < points.Count; i++)
            {
                g.FillEllipse(Brushes.White, (float)(points[i].x - 1.5f), (float)(points[i].y - 1.5f), 3, 3);
            }

            pictureBox1.Image = backImage;
        }

        public void VoronoiDiagram()
        {
            voronoiEdgeList = Voronoi.VoronoiEdges(delaunayTriangleList);
            for (int i = 0; i < voronoiEdgeList.Count; i++)
            {
                CSPoint p1 = new CSPoint((int)voronoiEdgeList[i].start.x, (int)voronoiEdgeList[i].start.y);
                CSPoint p2 = new CSPoint((int)voronoiEdgeList[i].end.x, (int)voronoiEdgeList[i].end.y);
                g.DrawLine(Pens.Red, p1.X, p1.Y, p2.X, p2.Y);
            }

            pictureBox1.Image = backImage;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label2.Text = e.X + ", " + e.Y;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            g.Clear(Color.Black);
            points.Clear();
            delaunayTriangleList.Clear();
            delaunayEdgeList.Clear();
            voronoiEdgeList.Clear();
            pointCount = (int)numericUpDown1.Value;

            SpreadPoints();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            g.Clear(Color.Black);
            points.Clear();
            delaunayTriangleList.Clear();
            delaunayEdgeList.Clear();
            voronoiEdgeList.Clear();

            pictureBox1.Image = backImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true && points.Count != 0)
            {
                DelaunayTriangulate();
            }
            if (checkBox2.Checked == true && delaunayTriangleList.Count != 0)
            {
                VoronoiDiagram();
            }
        }
    }
}
