using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace diagram.StaticDiagram
{
    public class ColumnScale : Grid
    {
        #region Properties
        private Canvas _header;
        private Canvas _body;
        private double _minDepth;
        private double _maxDepth;
        private double _scale;              // 刻度
        private int _canvasHeight;          // canvas的高度
        private double _colWidth;              // 数据显示列的宽度
        private int _width = 60;            // 自身宽度
        private int _headerHeight;          // 表头的高度
        private int _showHeight;            // 显示井深高度

        //private Canvas _canvas;
        private ContextMenu _menu;

        public int CanvasHeight
        {
            get { return _canvasHeight; }
            set { _canvasHeight = value; }
        }

        public double Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public Canvas Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public Canvas Body
        {
            get { return _body; }
            set { _body = value; }
        }
        #endregion

        public ColumnScale(double minDepth, double maxDepth, int colWidth, int showHeight, int headerHeight, int bodyHeight)
        {
            initializeData(minDepth, maxDepth, colWidth, showHeight, headerHeight, bodyHeight);
            adjustScale();
            initializeGraphics();
            initializeContextMenu();
        }

        public void adjustScale(double maxDepth)
        {
            _maxDepth = maxDepth;
            adjustScale();
            this.Children.RemoveRange(0, this.Children.Count);
            drawHeader();
            drawCanvas();
            initializeContextMenu();
        }

        public void adjustScaleWindow(double scale)
        {
            _scale = scale;
            _header.Children.RemoveRange(0, _header.Children.Count);
            _body.Children.RemoveRange(0, _body.Children.Count);
            drawHeader();
            drawCanvas();
            StackPanel panel = _header.Parent as StackPanel;
            StaticDiagram diagram = panel.Parent as StaticDiagram;
            diagram.adjustScale(scale);
        }

        #region 初始化
        private void initializeData(double minDepth, double maxDepth, int colWidth, int showHeight, int headerHeight, int bodyHeight)
        {
            _canvasHeight = bodyHeight;
            _headerHeight = headerHeight;
            _showHeight = showHeight;

            _minDepth = minDepth;
            _maxDepth = maxDepth;
            _colWidth = colWidth;
        }

        private void initializeContextMenu()
        {
            _menu = new ContextMenu();
            MenuItem add = new MenuItem() { Header = "在右添加一列" };
            add.Click += new RoutedEventHandler(addColumnToTheRight);
            MenuItem alter = new MenuItem() { Header = "更改刻度.." };
            alter.Click += new RoutedEventHandler(alterColumn);
            MenuItem save = new MenuItem() { Header = "保存配置" };
            save.Click += new RoutedEventHandler(saveConfig);

            _menu.Items.Add(add);
            _menu.Items.Add(alter);
            _menu.Items.Add(save);
            _header.ContextMenu = _menu;
        }
        #endregion

        #region 路由事件
        private void addColumnToTheRight(object sender, RoutedEventArgs args)
        {
            StackPanel panel = _header.Parent as StackPanel;
            StaticDiagram diagram = panel.Parent as StaticDiagram;
            int index = panel.Children.IndexOf(_header);
            ChooseColumnWindow window = new ChooseColumnWindow(diagram, index, this);
            window.Show();
        }

        public void alterColumn(object sender, RoutedEventArgs args)
        {
            ChangeScaleWindow window = new ChangeScaleWindow(this, _colWidth);
            window.Show();
        }

        private void saveConfig(object sender, RoutedEventArgs args)
        {
            saveEventArgs save = new saveEventArgs(Column.saveConfigEvent, this);
            _header.RaiseEvent(save);
        }
        #endregion

        #region 绘制边框及刻度
        private void initializeGraphics()
        {
            //ColumnDefinition cdef = new ColumnDefinition() { Width = new GridLength(_width, GridUnitType.Pixel) };
            //RowDefinition header = new RowDefinition() { Height = new GridLength(_headerHeight, GridUnitType.Pixel) };
            //RowDefinition canvas = new RowDefinition();

            //this.ColumnDefinitions.Add(cdef);
            //this.RowDefinitions.Add(header);
            //this.RowDefinitions.Add(canvas);

            drawHeader();
            drawCanvas();
        }

        private void drawHeader()
        {
            _header = new Canvas();
            _header.Width = _width;
            _header.Height = _headerHeight;

            TextBlock block = new TextBlock();
            Canvas.SetTop(block, 40);
            Canvas.SetLeft(block, 5);
            block.Text = "井深(m)";
            block.FontSize = 13;
            block.Foreground = Brushes.Black;
            Border border = new Border();
            border.BorderBrush = Brushes.LightBlue;
            border.BorderThickness = new Thickness(1.5);
            border.Width = _width;
            border.Height = _headerHeight;
            _header.Children.Add(block);
            _header.Children.Add(border);
        }

        private void drawCanvas()
        {
            _body = new Canvas() { Width = _width, Height = _canvasHeight };
            drawBorder();
            drawScale();
        }

        private void drawBorder()
        {
            Line top = new Line() { X1 = 0, Y1 = 0, X2 = _width, Y2 = 0, Stroke = Brushes.LightBlue, StrokeThickness = 1.5 };
            Line bottom = new Line() { X1 = 0, Y1 = _canvasHeight, X2 = _width, Y2 = _canvasHeight, Stroke = Brushes.LightBlue, StrokeThickness = 1.5 };
            Line left = new Line() { X1 = 0, Y1 = 0, X2 = 0, Y2 = _canvasHeight, Stroke = Brushes.LightBlue, StrokeThickness = 1.5 };
            Line right = new Line() { X1 = _width, Y1 = 0, X2 = _width, Y2 = _canvasHeight, Stroke = Brushes.LightBlue, StrokeThickness = 1.5 };
            _body.Children.Add(top);
            _body.Children.Add(bottom);
            _body.Children.Add(left);
            _body.Children.Add(right);

            for (int i = 1; i * _scale * _colWidth < _canvasHeight; ++i)
            {
                Line line = new Line()
                {
                    X1 = 0,
                    Y1 = i * _colWidth,
                    X2 = _width,
                    Y2 = i * _colWidth,
                    Stroke = Brushes.LightBlue,
                    StrokeThickness = 1,
                    StrokeDashArray = new DoubleCollection() { 0.5, 0.5 }
                };
                _body.Children.Add(line);
            }
        }

        public void adjustGraphics(double width)
        {
            _colWidth = width;
            adjustScale();
            //_body.Children.RemoveRange(0, _body.Children.Count);
            //drawHeader();
            //drawCanvas();
            initializeGraphics();
            initializeContextMenu();
        }

        private void drawScale()
        {
            for (int i = 0; i * _colWidth < _canvasHeight; ++i)
            {
                TextBlock block = new TextBlock();
                block.Text = (i * _colWidth * _scale + _minDepth).ToString();
                block.Foreground = Brushes.Black;
                block.FontSize = 12;
                Canvas.SetLeft(block, 20);
                Canvas.SetTop(block, i * _colWidth);
                _body.Children.Add(block);
            }
        }
        #endregion

        #region 调整Canvas高度、自动生成Scale
        private void adjustScale()
        {
            //double range = _maxDepth - _minDepth;
            _scale = _showHeight * 1.0 / _canvasHeight;
        }
        #endregion
    }
}
