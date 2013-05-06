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
        #region properties
        private double _minDepth;
        private double _maxDepth;
        private double _scale;              // 刻度
        private int _canvasHeight;          // canvas的高度
        private double _colWidth;              // 数据显示列的宽度
        private int _width = 60;            // 自身宽度
        private int _headerHeight;          // 表头的高度

        private Canvas _canvas;
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
        #endregion

        public ColumnScale(double minDepth, double maxDepth, int colWidth)
        {
            initializeData(minDepth, maxDepth, colWidth);
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
        }

        public void adjustScaleWindow(double scale)
        {
            _scale = scale;
            this.Children.RemoveRange(0, this.Children.Count);
            drawHeader();
            drawCanvas();
            StackPanel panel = this.Parent as StackPanel;
            StaticDiagram diagram = panel.Parent as StaticDiagram;
            diagram.adjustScale(scale);
        }

        #region 初始化
        private void initializeData(double minDepth, double maxDepth, int colWidth)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("..\\..\\StaticDiagram\\DiagramConfig.xml");
            XmlNode node = xml.SelectSingleNode("Diagram/ColumnBody/canvasheight");
            _canvasHeight = Int32.Parse(node.InnerText);
            node = xml.SelectSingleNode("Diagram/ColumnHeader/height");
            _headerHeight = Int32.Parse(node.InnerText);

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
            MenuItem draw = new MenuItem() { Header = "动态绘图" };

            _menu.Items.Add(add);
            _menu.Items.Add(alter);
            _menu.Items.Add(save);
            this.ContextMenu = _menu;
        }
        #endregion

        #region 路由事件
        private void addColumnToTheRight(object sender, RoutedEventArgs args)
        {
            StackPanel panel = this.Parent as StackPanel;
            StaticDiagram diagram = panel.Parent as StaticDiagram;
            int index = panel.Children.IndexOf(this);
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
            this.RaiseEvent(save);
        }
        #endregion

        #region 绘制边框及刻度
        private void initializeGraphics()
        {
            ColumnDefinition cdef = new ColumnDefinition() { Width = new GridLength(_width, GridUnitType.Pixel) };
            RowDefinition header = new RowDefinition() { Height = new GridLength(_headerHeight, GridUnitType.Pixel) };
            RowDefinition canvas = new RowDefinition();

            this.ColumnDefinitions.Add(cdef);
            this.RowDefinitions.Add(header);
            this.RowDefinitions.Add(canvas);

            drawHeader();
            drawCanvas();
        }

        private void drawHeader()
        {
            TextBlock block = new TextBlock();
            block.Text = "井深(m)";
            block.FontSize = 13;
            block.Foreground = Brushes.Black;
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            Grid.SetColumn(panel, 0);
            Grid.SetRow(panel, 0);
            panel.Children.Add(block);
            Border border = new Border();
            border.BorderBrush = Brushes.LightBlue;
            border.BorderThickness = new Thickness(1.5);
            this.Children.Add(panel);
            this.Children.Add(border);
        }

        private void drawCanvas()
        {
            _canvas = new Canvas() { Width = _width, Height = _canvasHeight };
            drawBorder();
            drawScale();
            Grid.SetColumn(_canvas, 0);
            Grid.SetRow(_canvas, 1);
            this.Children.Add(_canvas);
        }

        private void drawBorder()
        {
            Line top = new Line() { X1 = 0, Y1 = 0, X2 = _width, Y2 = 0, Stroke = Brushes.LightBlue, StrokeThickness = 1.5 };
            Line bottom = new Line() { X1 = 0, Y1 = _canvasHeight, X2 = _width, Y2 = _canvasHeight, Stroke = Brushes.LightBlue, StrokeThickness = 1.5 };
            Line left = new Line() { X1 = 0, Y1 = 0, X2 = 0, Y2 = _canvasHeight, Stroke = Brushes.LightBlue, StrokeThickness = 1.5 };
            Line right = new Line() { X1 = _width, Y1 = 0, X2 = _width, Y2 = _canvasHeight, Stroke = Brushes.LightBlue, StrokeThickness = 1.5 };
            _canvas.Children.Add(top);
            _canvas.Children.Add(bottom);
            _canvas.Children.Add(left);
            _canvas.Children.Add(right);

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
                _canvas.Children.Add(line);
            }
        }

        public void adjustGraphics(double width)
        {
            _colWidth = width;
            adjustScale();
            _canvas.Children.RemoveRange(0, _canvas.Children.Count);
            drawHeader();
            drawCanvas();
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
                _canvas.Children.Add(block);
            }
        }
        #endregion

        #region 调整Canvas高度、自动生成Scale
        private void adjustScale()
        {
            double range = _maxDepth - _minDepth;
            _scale = range * 1.0 / _canvasHeight;
        }
        #endregion
    }
}
