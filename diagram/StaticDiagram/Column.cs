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

using diagram.Common;

namespace diagram.StaticDiagram
{
    class Column : Grid
    {
        public Column(int width, int height, List<Data> datalist, double minDEPTH, double maxDEPTH, double scale, DataModel model)
        {
            initializeData(width, height, datalist, minDEPTH, maxDEPTH, scale, model);
            initializeGraphics();
            initializeContextMenu();
        }

        #region properties
        private ColumnHeader _header;
        private ColumnBody _body;
        private ContextMenu _menu;
        private ColumnDefinition _coldef;
        public double _scale;
        public static Brush[] colors = new Brush[5] 
        { 
            Brushes.Red,
            Brushes.DarkBlue,
            Brushes.DarkGreen,
            Brushes.Brown,
            Brushes.Black
        };

        private int _minwidth;
        private int _width;
        private double _minDEPTH;
        private double _maxDEPTH;
        private DataModel _model;

        public ColumnHeader Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public ColumnBody Body
        {
            get { return _body; }
            set { _body = value; }
        }
        #endregion

        #region 初始化
        private void initializeData(int width, int height, List<Data> datalist, double minDEPTH, double maxDEPTH, double scale, DataModel model)
        {
            List<ColumnHeaderData> list = new List<ColumnHeaderData>();
            foreach (Data data in datalist)
            {
                list.Add(new ColumnHeaderData(data));
            }

            _header = new ColumnHeader(list);
            _body = new ColumnBody(width, height);
            _minDEPTH = minDEPTH;
            _maxDEPTH = maxDEPTH;
            _scale = scale;

            XmlDocument xml = new XmlDocument();
            xml.Load("..\\..\\StaticDiagram\\DiagramConfig.xml");
            XmlNode node = xml.SelectSingleNode("Diagram/Column/minwidth");
            _minwidth = Int32.Parse(node.InnerText);
            _width = width;

            _model = model;
        }

        public void initializeGraphics()
        {
            _coldef = new ColumnDefinition();
            _coldef.MinWidth = _minwidth;
            _coldef.Width = new GridLength(_width, GridUnitType.Pixel);
            this.ColumnDefinitions.Add(_coldef);
            RowDefinition _rowdef1 = new RowDefinition();
            RowDefinition _rowdef2 = new RowDefinition();
            this.RowDefinitions.Add(_rowdef1);
            this.RowDefinitions.Add(_rowdef2);

            initializeHeader();
            initializeBody();
        }

        public void initializeHeader()
        {
            Grid.SetColumn(_header, 0);
            Grid.SetRow(_header, 0);
            this.Children.Add(_header);
        }

        public void initializeBody()
        {
            Grid.SetColumn(_body, 0);
            Grid.SetRow(_body, 1);
            this.Children.Add(_body);
        }

        private void initializeContextMenu()
        {
            _menu = new ContextMenu();
            MenuItem add = new MenuItem() { Header = "在右添加一列" };
            add.Click += new RoutedEventHandler(addColumnToTheRight);
            MenuItem alter = new MenuItem() { Header = "变更.." };
            alter.Click += new RoutedEventHandler(alterColumn);
            alter.Tag = this;
            MenuItem delete = new MenuItem() { Header = "删除该列" };
            delete.Click += new RoutedEventHandler(deleteColumn);
            MenuItem draw = new MenuItem() { Header = "绘图" };
            draw.Click += new RoutedEventHandler(repaint);
            MenuItem save = new MenuItem() { Header = "保存配置" };
            save.Click += new RoutedEventHandler(saveConfig);
            
            _menu.Items.Add(add);
            _menu.Items.Add(delete);
            _menu.Items.Add(alter);
            _menu.Items.Add(draw);
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

        private void alterColumn(object sender, RoutedEventArgs args)
        {
            Column c = (sender as MenuItem).Tag as Column;
            AlterColumnWindow window = new AlterColumnWindow(c);
            window.Show();
        }

        private void deleteColumn(object sender, RoutedEventArgs args)
        {
            StackPanel panel = this.Parent as StackPanel;
            int index = panel.Children.IndexOf(this);

            delEventArgs del = new delEventArgs(delColumnEvent, this);
            del.index = index;
            this.RaiseEvent(del);
        }

        private void saveConfig(object sender, RoutedEventArgs args)
        {
            saveEventArgs save = new saveEventArgs(saveConfigEvent, this);
            this.RaiseEvent(save);
        }

        public void repaint(object sender, RoutedEventArgs args)
        {
            drawGraphics();
        }

        // 注册路由事件
        public static readonly RoutedEvent saveConfigEvent = EventManager.RegisterRoutedEvent
            ("saveConfigEvent", RoutingStrategy.Bubble, typeof(EventHandler<saveEventArgs>), typeof(Column));

        public static readonly RoutedEvent delColumnEvent = EventManager.RegisterRoutedEvent
            ("delColumnEvent", RoutingStrategy.Bubble, typeof(EventHandler<delEventArgs>), typeof(Column));
        #endregion

        #region 图形处理
        public void adjustGraphics(int width)
        {
            _width = width;
            _coldef.Width = new GridLength(width, GridUnitType.Pixel);
            _header.adujustGraphics(width);
            _body.repaint(width);
        }

        public void drawGraphics()
        {
            List<ColumnHeaderData> list = _header.Data;

            for (int i = 0; i < list.Count; ++i)
            {
                ColumnHeaderData c = list.ElementAt(i);
                List<double> data = c.Data.DData;

                double min = c.Data.Min;
                double max = c.Data.Max;

                if (max == min)
                {
                    max = max + 1;
                } 
                
                double span = max - min;

                double Dmin = _model.DEPTMEAS.DData.Min();

                for (int n = 0; n < data.Count-1; ++n)
                {
                    Line line = new Line() { X1 = (data.ElementAt(n) - min) * this._body.Width / span, Y1 = (getDEPTMEAS(n) - Dmin) / _scale, X2 = (data.ElementAt(n + 1) - min) * this._body.Width / span, Y2 = (getDEPTMEAS(n+1) - Dmin) / _scale };
                    line.Stroke = colors[i];
                    line.StrokeThickness = 0.8;
                    
                    _body.Children.Add(line);
                }
            }
        }

        private double getDEPTMEAS(int i)
        {
            return _model.DEPTMEAS.DData.ElementAt(i);
        }

        //public void addGraphics(int startIndex)
        //{
        //    List<ColumnHeaderData> list = _header.Data;

        //    for (int i = 0; i < list.Count; ++i)
        //    {
        //        ColumnHeaderData c = list.ElementAt(i);
        //        List<double> data = c.Data._data;

        //        double min = c.Data._min;
        //        double max = c.Data._max;
        //        double span = max - min;

        //        for (int n = startIndex; n < data.Count-1; ++n)
        //        {
        //            Line line = new Line() { X1 = (data.ElementAt(n) - min) * this._body.Width / span, Y1 = (n) / _scale, X2 = (data.ElementAt(n + 1) - min) * this._body.Width / span, Y2 = (n + 1) / _scale };
        //            line.Stroke = colors[i];
        //            line.StrokeThickness = 0.8;

        //            _body.Children.Add(line);
        //        }
        //    }
        //}
        #endregion
    }

    class saveEventArgs : RoutedEventArgs
    { 
        public saveEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source) { }
    }

    class delEventArgs : RoutedEventArgs
    {
        public delEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source) { }

        public int index { get; set; }
    }
}
