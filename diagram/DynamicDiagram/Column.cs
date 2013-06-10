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
using System.Windows.Shapes;
using System.Xml;

using diagram.Common;

namespace diagram.DynamicDiagram
{
    public class Column : Grid
    {
        #region Constructor
        public Column(double width, int headerHeight, int bodyHeight, List<Data> datalist, DataModel model)
        {
            _model = model;
            initializeData(width, headerHeight, bodyHeight, datalist);
            initializeGraphics();
            initializeContextMenu();
        }
        #endregion

        #region Properties
        private ColumnHeader _header;
        private ColumnBody _body;
        private ContextMenu _menu;
        private ColumnDefinition _coldef;
        private List<Data> _dataList;
        private DataModel _model;

        public DataModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public List<Data> DataList
        {
            get { return _dataList; }
            set { _dataList = value; }
        }

        public static Brush[] colors = new Brush[5] 
        { 
            Brushes.Red,
            Brushes.DarkBlue,
            Brushes.DarkGreen,
            Brushes.Brown,
            Brushes.Black
        };

        private double _minwidth;
        private double _width;

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

        #region Initialization
        private void initializeData(double width, int headerHeight, int bodyHeight, List<Data> datalist)
        {
            List<ColumnHeaderData> list = new List<ColumnHeaderData>();
            foreach (Data data in datalist)
            {
                list.Add(new ColumnHeaderData(data));
            }

            _header = new ColumnHeader(list, headerHeight);
            _body = new ColumnBody(width, bodyHeight);

            XmlDocument xml = new XmlDocument();
            xml.Load("..\\..\\DynamicDiagram\\DiagramConfig.xml");
            XmlNode node = xml.SelectSingleNode("Diagram/Column/minwidth");
            _minwidth = Int32.Parse(node.InnerText);
            _width = width;
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
            add.Click += new RoutedEventHandler(addColumnToTheLeft);
            MenuItem alter = new MenuItem() { Header = "变更.." };
            alter.Click += new RoutedEventHandler(alterColumn);
            alter.Tag = this;
            MenuItem delete = new MenuItem() { Header = "删除该列" };
            delete.Click += new RoutedEventHandler(deleteColumn);
            //MenuItem draw = new MenuItem() { Header = "绘图" };
            //draw.Click += new RoutedEventHandler(repaint);
            MenuItem save = new MenuItem() { Header = "保存配置" };
            save.Click += new RoutedEventHandler(saveConfig);

            _menu.Items.Add(add);
            _menu.Items.Add(delete);
            _menu.Items.Add(alter);
            //_menu.Items.Add(draw);
            _menu.Items.Add(save);
            this.ContextMenu = _menu;
        }
        #endregion

        #region Methods
        #endregion

        #region RoutingMethods
        private void addColumnToTheLeft(object sender, RoutedEventArgs args)
        {
            StackPanel panel = this.Parent as StackPanel;
            TimeBasedDynamicDiagram diagram = panel.Parent as TimeBasedDynamicDiagram;
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

        // 注册路由事件
        public static readonly RoutedEvent saveConfigEvent = EventManager.RegisterRoutedEvent
            ("saveConfigEvent", RoutingStrategy.Bubble, typeof(EventHandler<saveEventArgs>), typeof(Column));

        public static readonly RoutedEvent delColumnEvent = EventManager.RegisterRoutedEvent
            ("delColumnEvent", RoutingStrategy.Bubble, typeof(EventHandler<delEventArgs>), typeof(Column));
        #endregion

        #region Graphics
        public void adjustGraphics(int width)
        {
            _width = width;
            _coldef.Width = new GridLength(width, GridUnitType.Pixel);
            _header.adujustGraphics(width);
            _body.repaint(width);
        }

        public void drawGraphics(ScaleData sData)
        {
            // 获取本列中的所有数据
            List<ColumnHeaderData> list = _header.Data;

            for (int ii = 0; ii < list.Count; ++ii)
            {
                // 获取单项数据
                ColumnHeaderData c = list.ElementAt(ii);
                List<double> data = c.Data.DData;

                double min = c.Data.Min;
                double max = c.Data.Max;

                if(max==min)
                {
                    max = max + 1;
                }

                double span = max - min;

                double scale = _body.Height * 1.0 / 3.0;

                //Time ROOT = new Time(_model.TDATE._StringData.ElementAt(sData.pos[0]).ToString(),
                //                     _model.TTIME._StringData.ElementAt(sData.pos[0]).ToString());

                Time ROOT = sData.FirstTime;

                int rowInterval = sData.Datetime[0].Getdiff(sData.Datetime[2]) / 3;

                for (int i = 0; i < 3; ++ i)
                {
                    for (int j = i==0 ? 0 : sData.Pos[i-1]; j < sData.Pos[i]; ++j)
                    {
                        Time first = new Time(_model.TDATE.StringData.ElementAt(j).ToString(),
                                              _model.TTIME.StringData.ElementAt(j).ToString());

                        Time second = new Time(_model.TDATE.StringData.ElementAt(j + 1).ToString(),
                                              _model.TTIME.StringData.ElementAt(j + 1).ToString());
                        int interval = first.Getdiff(second);

                        Line line = new Line()
                        {
                            X1 = (data.ElementAt(j) - min) * this._body.Width / span,
                            Y1 = (ROOT.Getdiff(first)) * 1.0 / rowInterval * scale,
                            X2 = (data.ElementAt(j+1) - min) * this._body.Width / span,
                            Y2 = (ROOT.Getdiff(second)) * 1.0 / rowInterval * scale,
                            Stroke = colors[ii],
                            StrokeThickness = 0.8
                        };

                        _body.Children.Add(line);
                    }
                }
            }
        }

        public void repaint(ScaleData data)
        {
            _body.repaint();
            drawGraphics(data);
        }
        #endregion
    }

    #region RoutedEventArgs
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
    #endregion

}
