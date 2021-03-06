﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace diagram.DynamicDiagram
{
    public class ColumnBody : Canvas
    {
        #region Constructor
        public ColumnBody(double width, int height)
        {
            initializeData(width, height);
            initializeGraphics();
        }
        #endregion

        #region Properties
        private int _height;                // canvas的高度
        private double _width;                 // canvas的宽度
        private DoubleCollection _dc;       // 调整线条间隔等
        private Brush _brush = Brushes.LightBlue;
        #endregion

        #region Initialization
        private void initializeData(double width, int height)
        {
            _height = height;
            _width = width;

            _dc = new DoubleCollection();
            _dc.Add(5.0);
            _dc.Add(5.0);
        }

        private void initializeGraphics()
        {
            this.Height = _height;
            this.Width = _width;

            addBorder();
            addHorizontalLine();
            addVerticalLine();
        }
        #endregion

        #region Methods
        public void repaint(int width)
        {
            _width = width;
            this.Width = width;
            this.Children.RemoveRange(0, this.Children.Count);
            addBorder();
            addHorizontalLine();
            addVerticalLine();
        }

        public void repaint()
        {
            this.Children.RemoveRange(0, this.Children.Count);
            addBorder();
            addHorizontalLine();
            addVerticalLine();
        }
        #endregion

        #region Graphics
        private void addHorizontalLine()
        {
            // 网格线
            for (int i = 1; i < this.Height / 50; ++i)
            {
                Line line = new Line();
                line.X1 = 0;
                line.X2 = this.Width;
                line.Y1 = 50 * i;
                line.Y2 = 50 * i;
                line.Stroke = _brush;
                if (i % (this.Width / 50) != 0)
                    line.StrokeDashArray = _dc;
                this.Children.Add(line);
            }

            // 刻度线
            double scale = this.Height / 3;
            for (int i = 1; i * scale < this.Height; ++i)
            {
                Line line = new Line();
                line.X1 = 0;
                line.X2 = this.Width;
                line.Y1 = i * scale;
                line.Y2 = i * scale;
                line.Stroke = Brushes.Black;
                line.StrokeDashArray = _dc;
                this.Children.Add(line);
            }
        }

        private void addVerticalLine()
        {
            for (int i = 1; i < this.Width / 50; ++i)
            {
                Line line = new Line();
                line.X1 = 50 * i;
                line.Y1 = 0;
                line.X2 = 50 * i;
                line.Y2 = this.Height;
                line.Stroke = _brush;
                line.StrokeDashArray = _dc;
                this.Children.Add(line);
            }
        }

        private void addBorder()
        {
            // 画边框
            Line top = new Line() { X1 = 0, Y1 = 0, X2 = 0, Y2 = this.Width, Stroke = _brush };
            Line bottom = new Line() { X1 = this.Height, Y1 = 0, X2 = this.Height, Y2 = this.Width, Stroke = _brush };
            Line left = new Line() { X1 = 0, Y1 = 0, X2 = 0, Y2 = this.Height, Stroke = _brush };
            Line right = new Line() { X1 = this.Width, Y1 = 0, X2 = this.Width, Y2 = this.Height, Stroke = _brush };

            this.Children.Add(top);
            this.Children.Add(bottom);
            this.Children.Add(left);
            this.Children.Add(right);
        }
        #endregion
    }
}
