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

namespace diagram.DynamicDiagram
{
    public class ScaleColumn : Grid
    {
        #region Constructor
        public ScaleColumn(double colWidth, int headerHeight, int bodyHeight)
        {
            initializeData(colWidth, headerHeight, bodyHeight);
            initializeGraphics();
            initializeContextMenu();
        }
        #endregion

        #region Properties
        private int _canvasHeight;          // canvas的高度
        private double _colWidth;              // 数据显示列的宽度
        private int _width = 100;            // 自身宽度
        private int _headerHeight;          // 表头的高度

        private Canvas _canvas;
        private ContextMenu _menu;

        public int CanvasHeight
        {
            get { return _canvasHeight; }
            set { _canvasHeight = value; }
        }
        #endregion

        #region Initialization
        private void initializeData(double colWidth, int headerHeight, int bodyHeight)
        {

            _canvasHeight = bodyHeight;
            _headerHeight = headerHeight;
            _colWidth = colWidth;
        }

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

        private void initializeContextMenu()
        {
            _menu = new ContextMenu();
            MenuItem add = new MenuItem() { Header = "在右添加一列" };
            add.Click += new RoutedEventHandler(addColumnToTheRight);
            MenuItem alter = new MenuItem() { Header = "更改时间间隔.." };
            alter.Click += new RoutedEventHandler(alterColumn);
            MenuItem save = new MenuItem() { Header = "保存配置" };
            save.Click += new RoutedEventHandler(saveConfig);
            MenuItem draw = new MenuItem() { Header = "动态绘图" };
            draw.Click += new RoutedEventHandler(drawGraphics);
            MenuItem stop = new MenuItem() { Header = "停止绘图" };
            stop.Click += new RoutedEventHandler(stopDrawing);

            _menu.Items.Add(add);
            _menu.Items.Add(alter);
            _menu.Items.Add(save);
            _menu.Items.Add(draw);
            _menu.Items.Add(stop);
            this.ContextMenu = _menu;
        }
        #endregion

        #region Methods
        #endregion

        #region RoutingMethods
        private void addColumnToTheRight(object sender, RoutedEventArgs args)
        {
            StackPanel panel = this.Parent as StackPanel;
            TimeBasedDynamicDiagram diagram = panel.Parent as TimeBasedDynamicDiagram;
            int index = panel.Children.IndexOf(this);
            ChooseColumnWindow window = new ChooseColumnWindow(diagram, index, this);
            window.Show();
        }

        public void alterColumn(object sender, RoutedEventArgs args)
        {
            StackPanel panel = this.Parent as StackPanel;
            TimeBasedDynamicDiagram diagram = panel.Parent as TimeBasedDynamicDiagram;
            ChangeIntervalWindow window = new ChangeIntervalWindow(diagram.Model);
            window.Show();
            //ChangeScaleWindow window = new ChangeScaleWindow(this, _colWidth);
            //window.Show();
        }

        private void saveConfig(object sender, RoutedEventArgs args)
        {
            saveEventArgs save = new saveEventArgs(Column.saveConfigEvent, this);
            this.RaiseEvent(save);
        }

        private void drawGraphics(object sender, RoutedEventArgs args)
        {
            StackPanel panel = this.Parent as StackPanel;
            TimeBasedDynamicDiagram diagram = panel.Parent as TimeBasedDynamicDiagram;
            diagram.startDynamicDrawing();
        }

        private void stopDrawing(object sender, RoutedEventArgs args)
        {
            StackPanel panel = this.Parent as StackPanel;
            TimeBasedDynamicDiagram diagram = panel.Parent as TimeBasedDynamicDiagram;
            diagram.endDrawing();
        }
        #endregion

        #region Graphics
        private void drawHeader()
        {
            TextBlock block = new TextBlock();
            block.Text = "时间\r\n井深";
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
            //drawScale();
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
        }

        public void repaintScale(ScaleData data)
        {
            if (this._canvas.Children.Count > 4)
            {
                this._canvas.Children.RemoveRange(4, this.Children.Count);
            }
            double scale = _canvasHeight * 1.0 / 3;
            for (int i = 1; i < 3; ++i)
            {
                TextBlock block = new TextBlock();
                block.Text = data.Datetime[i - 1].ToDateString() + "\n"
                    + data.Datetime[i-1].ToTimeString() + "\n"
                    +Math.Round(Convert.ToDouble(data.Depth[i - 1].ToString()),2)+"(m)";

                block.Foreground = Brushes.Red;

                Canvas.SetLeft(block, 10);
                Canvas.SetTop(block, i * scale - 30);
                _canvas.Children.Add(block);
            }
        }
        #endregion
    }
}
