using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CoordinatesEditor
{
    public partial class MainForm : Form
    {
        private BindingList<Point> points = new BindingList<Point>();
        
        public MainForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            SetupEventHandlers();
            AddSampleData();
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = points;

            DataGridViewTextBoxColumn xColumn = new DataGridViewTextBoxColumn();
            xColumn.DataPropertyName = "X";
            xColumn.HeaderText = "X Coordinate";
            dataGridView1.Columns.Add(xColumn);

            DataGridViewTextBoxColumn yColumn = new DataGridViewTextBoxColumn();
            yColumn.DataPropertyName = "Y";
            yColumn.HeaderText = "Y Coordinate";
            dataGridView1.Columns.Add(yColumn);
        }

        private void SetupEventHandlers()
        {
            dataGridView1.CellValueChanged += (s, e) => drawingPanel.Invalidate();
            dataGridView1.UserDeletedRow += (s, e) => drawingPanel.Invalidate();
            points.ListChanged += (s, e) => drawingPanel.Invalidate();
            drawingPanel.Paint += DrawingPanel_Paint;
        }

        private void AddSampleData()
        {
            points.Add(new Point { X = 50, Y = 50 });
            points.Add(new Point { X = 150, Y = 100 });
            points.Add(new Point { X = 250, Y = 200 });
        }

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            if (points.Count < 2) return;

            List<PointF> convertedPoints = new List<PointF>();
            
            // Конвертация координат в размеры панели
            foreach (Point p in points)
            {
                float x = (float)p.X * drawingPanel.Width / 300;
                float y = drawingPanel.Height - (float)p.Y * drawingPanel.Height / 300;
                convertedPoints.Add(new PointF(x, y));
            }

            // Рисование линий
            using (Pen pen = new Pen(Color.Blue, 2))
            {
                for (int i = 0; i < convertedPoints.Count - 1; i++)
                {
                    g.DrawLine(pen, convertedPoints[i], convertedPoints[i + 1]);
                }
            }

            // Рисование точек
            foreach (PointF p in convertedPoints)
            {
                g.FillEllipse(Brushes.Red, p.X - 5, p.Y - 5, 10, 10);
            }
        }
    }
}