﻿using System;
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
        public PixelCoordinates()
        {
            InitializeComponent();
        }

        public void Display(int x, int y)
        {
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

        private void PixelCoordinates_Load(object sender, EventArgs e)
        {

        }
    }
}