using diagram.Common;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Threading;

namespace diagram.StaticDiagram
{
    class StaticDiagram : StackPanel
    {
        #region properties
        private StackPanel _headerPanel;
        private ScrollViewer _bodyViewer;
        private StackPanel _bodyPanel;

        private DataModel _model;       // 存储所有数据
        private List<Column> _columns;
        private ColumnScale _scale;
        private enum _ColumnWidth : int
        { BIG = 400, MIDDLE = 300, SMALL = 200 };
        private double _width;          // ScrollViewer的宽度
        private int _colWidth;

        private int _bodyHeight;
        private int _headerHeight;
        private int _showHeight;

        public DataModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public List<Column> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public StackPanel HeaderPanel
        {
            get { return _headerPanel; }
            set { _headerPanel = value; }
        }

        public StackPanel BodyPanel
        {
            get { return _bodyPanel; }
            set { _bodyPanel = value; }
        }

        public StaticDiagram(double width, DataModel model, String path)
        {
            initializeData(width, model, path);
            initializeGraphics();
            initializeHandler();
        }
        #endregion

        #region Constructor
        private void initializeData(double width, DataModel model, String path)
        {
            // 初始化图形属性
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode("Diagram/ColumnHeader/Height");
            _headerHeight = Int32.Parse(node.InnerText);
            node = doc.SelectSingleNode("Diagram/ColumnBody/CanvasHeight");
            _bodyHeight = Int32.Parse(node.InnerText);
            node = doc.SelectSingleNode("Diagram/ScaleColumn/ShowHeight");
            _showHeight = Int32.Parse(node.InnerText);

            // 初始化数据元素
            _width = width;
            _colWidth = adjustColumnWidth(width, model.DefaultColumnNumber); 
            _model = model;
            _columns = new List<Column>();
            _headerPanel = new StackPanel();
            _headerPanel.Orientation = Orientation.Horizontal;
            _bodyViewer = new ScrollViewer();
            _bodyViewer.Height = Int32.Parse(doc.SelectSingleNode("Diagram/BodyViewer/Height").InnerText);
            _bodyPanel = new StackPanel();
            _bodyPanel.Orientation = Orientation.Horizontal;
            _bodyViewer.Content = _bodyPanel;

            List<List<Data>> list = new List<List<Data>>();
            for (int i = 0; i < _model.DefaultColumnNumber; ++i)
            {
                List<Data> datalist = new List<Data>();
                list.Add(datalist);
            }
            for (int i = 0; i < _model.DataList.Count; ++i)
            {
                Data d = _model.DataList.ElementAt(i);
                if (d.DefaultColumnPos.Count == 0)
                    continue;
                for (int j = 0; j < d.DefaultColumnPos.Count; ++j)
                {
                    list.ElementAt(d.DefaultColumnPos.ElementAt(j) - 1).Add(d);
                }
            }

            _scale = new ColumnScale(_model.DEPTMEAS.Min, _model.DEPTMEAS.Max, _colWidth, _showHeight, _headerHeight, _bodyHeight);
            _headerPanel.Children.Add(_scale.Header);
            _bodyPanel.Children.Add(_scale.Body);
            for (int i = 0; i < _model.DefaultColumnNumber; ++i)
            {
                Column c = new Column(_colWidth, _headerHeight, _bodyHeight, list.ElementAt(i), _model.DEPTMEAS.DData.Min(), _model.DEPTMEAS.DData.Max(), _scale.Scale, _model);
                _columns.Add(c);
                _headerPanel.Children.Add(c.Header);
                _bodyPanel.Children.Add(c.Body);
            }
        }

        private void initializeGraphics()
        {
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Top;
            Orientation = Orientation.Vertical;
            this.Children.Add(_headerPanel);
            this.Children.Add(_bodyViewer);
            //drawGraphics();
        }

        private void initializeHandler()
        {
            this.AddHandler(Column.delColumnEvent, new RoutedEventHandler(delColumn));
            this.AddHandler(Column.saveConfigEvent, new RoutedEventHandler(saveDataConfig));
            this.AddHandler(Column.saveGraphicsEvent, new RoutedEventHandler(saveGraphicsConfig));
        }
        #endregion 

        #region Event Handling Methods
        public void addColumn(int pos, List<Data> list)
        {
            Column c = new Column(_colWidth, _headerHeight, _bodyHeight, list, _model.DEPTMEAS.DData.Min(), _model.DEPTMEAS.DData.Max(), _scale.Scale, _model);
            _columns.Insert(pos, c);
            _headerPanel.Children.Insert(pos+1, c.Header);
            _bodyPanel.Children.Insert(pos+1, c.Body);
            adjustGraphics();
            drawGraphics();
        }

        private void delColumn(object sender, RoutedEventArgs args)
        {
            delEventArgs e = (delEventArgs)args;
            _columns.RemoveAt(e.index-1);           // stackpanel中多一列ColumnScale
            _headerPanel.Children.RemoveAt(e.index);
            _bodyPanel.Children.RemoveAt(e.index);
            adjustGraphics();
        }

        public void saveDataConfig(object sender, RoutedEventArgs args)
        {
            _model.saveDataConfig(_columns);
        }

        private void saveGraphicsConfig(object sender, RoutedEventArgs args)
        { 
            
        }

        private void adjustGraphics()
        {
            int width = adjustColumnWidth(_width, _columns.Count);
            if (width != _colWidth)
            {
                foreach (Column c in _columns)
                {
                    c.adjustGraphics(width);
                    c.drawGraphics();
                }
                _headerPanel.Children.RemoveRange(0, _headerPanel.Children.Count);
                _bodyPanel.Children.RemoveRange(0, _bodyPanel.Children.Count);
                _scale.adjustGraphics(width);

                _headerPanel.Children.Add(_scale.Header);
                _bodyPanel.Children.Add(_scale.Body);
                foreach (Column c in _columns)
                {
                    _headerPanel.Children.Add(c.Header);
                    _bodyPanel.Children.Add(c.Body);
                }
                _colWidth = width;
            }
        }
        #endregion

        #region Graphics Methods
        public int adjustColumnWidth(double width, int colnum)
        {
            if (width / (int)_ColumnWidth.BIG >= colnum) { return (int)_ColumnWidth.BIG; }
            else if (width / (int)_ColumnWidth.MIDDLE >= colnum) { return (int)_ColumnWidth.MIDDLE; }
            else { return (int)_ColumnWidth.SMALL; }
        }

        public void adjustScale(double scale)
        {
            foreach (Column c in _columns)
            {
                c._scale = scale;
                c.Body.repaint();
                c.drawGraphics();
            }
        }

        public void drawGraphics()
        {
            foreach (Column column in _columns)
            {
                column.Body.repaint();
                column.drawGraphics();
            }
        }
        #endregion
    }
}