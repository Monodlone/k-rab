using k_rab.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace k_rab
{
    public partial class FormMain : Form
    {
        private readonly List<IDrawable> _shapes = new List<IDrawable>();
        private readonly SolidBrush _brush = new SolidBrush(Color.Black);
        private readonly Pen _pen = new Pen(Color.Pink, 5);
        private Shape _selectedShape;
        private Point _offset;

        public FormMain()
        {
            InitializeComponent();
            //typeof(Panel).InvokeMember(
            //    "DoubleBuffered",
            //    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
            //    null,
            //    panel1,
            //    new object[] { true });
            //var shapes = typeof(Shape).Assembly
            //    .GetTypes()
            //    .Where(f => f.IsSubclassOf(typeof(Shape)))
            //    .ToArray();
            //var constructorNames = shapes.Select(f => f.Name).ToArray();

            //var buttons = new List<Button>();
            //foreach (var name in constructorNames)
            //    buttons.Add(new Button());


            //var btn = new Button();
            //btn.Parent = this;
            //btn.Location = new Point(158, 12);
            //btn.Size = new Size(193, 67);
            //btn.Text = constructorNames[0];
            //btn.Show();
            //btn.BringToFront();
            //label.Font.Size = new Size(158, 12);
            //var constructors = shapes.Select(t => t.GetConstructors()[1]).ToArray();
            //var l = new Label();
            //l.Parent = this;
            //l.Location = new Point(100, 100);
            //l.Size = new Size(200, 200);
            //l.Text = constructors[0].GetParameters()[0].Name;
            //l.Show();
            //l.BringToFront();
        }

        private void doubleBufferedPanel1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var shape in _shapes)
                shape.Draw(e.Graphics, _brush, _pen);
        }

        private void doubleBufferedPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                for (int i = _shapes.Count - 1; i >= 0; --i)
                {
                    if (!_shapes[i].IsPointInside(e.Location))
                    {
                        _selectedShape = null;
                        continue;
                    }

                    _selectedShape = (Shape)_shapes[i];
                    _offset = _selectedShape.GetOffset(e.Location);
                    _selectedShape.IsSelected = true;
                    _shapes[i] = _shapes[_shapes.Count - 1];
                    _shapes[_shapes.Count - 1] = _selectedShape;
                    break;
                }
            }
        }

        private void doubleBufferedPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);

            MoveSelected(e.Location);
        }

        private void doubleBufferedPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_selectedShape == null) return;
            MoveSelected(e.Location);
            _selectedShape.IsSelected = false;
            _selectedShape = null;
        }

        private void ElipseBtn_Click(object sender, EventArgs e)
        { 
            _shapes.Add(new Elipse(Shape_Info_Input.FromOneSide()));
            doubleBufferedPanel1.Refresh();
        }

        private void TriangleBtn_Click(object sender, EventArgs e)
        {
            _shapes.Add(new Triangle(Shape_Info_Input.FromTwoSides()));
            doubleBufferedPanel1.Refresh();
        }

        private void SquareBtn_Click(object sender, EventArgs e)
        {
            _shapes.Add(new Square(Shape_Info_Input.FromTwoSides()));
            doubleBufferedPanel1.Refresh();
        }

        private void RectangleBtn_Click(object sender, EventArgs e)
        {
            _shapes.Add(new Rectangle(Shape_Info_Input.FromOneSide()));
            doubleBufferedPanel1.Refresh();
        }

        private void MoveSelected(Point point)
        {
            if (_selectedShape == null) return;
            _selectedShape.X = point.X - _offset.X;
            _selectedShape.Y = point.Y - _offset.Y;
            doubleBufferedPanel1.Refresh();
        }

    }
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
        }
    }
}