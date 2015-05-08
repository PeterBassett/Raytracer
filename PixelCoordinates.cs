using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Raytracer
{
    public partial class PixelCoordinates : Form
    {
        public delegate void RenderRequested(object sender, int x, int y);
        public event RenderRequested OnRenderRequested;
        private int _x, _y;

        public PixelCoordinates()
        {
            InitializeComponent();
        }

        public void Display(int x, int y)
        {
            _x = x;
            _y = y;

            var boxes = GetTextboxes();

            DisplayCoordinates(x, y, boxes);

            this.ShowDialog();
        }

        private void DisplayCoordinates(int x, int y, IEnumerable<TextBox> boxes)
        {
            foreach (var box in boxes)
            {
                box.Text = string.Format(box.Tag.ToString(), x, y);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        IEnumerable<TextBox> GetTextboxes()
        {
            foreach (var control in this.Controls)
            {
                var box = control as TextBox;
                if (box != null)
                    yield return box;
            }
        }

        private void btnRenderAtPixel_Click(object sender, EventArgs e)
        {
            int x, y;
            GetCoordinates(textBox1.Text, out x, out y);

            var onRenderRequestedEvent = OnRenderRequested;

            if (onRenderRequestedEvent != null)
                onRenderRequestedEvent(this, x, y);
        }

        private void GetCoordinates(string text, out int x, out int y)
        {
            var split = text.Split(':');

            x = int.Parse(split[0]);
            y = int.Parse(split[1]);
        }
    }
}
