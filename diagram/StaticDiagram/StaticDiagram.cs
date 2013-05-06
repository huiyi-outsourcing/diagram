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
    class StaticDiagram : ScrollViewer
    {
        #region properties
        private StackPanel _panel;

        private DataModel _model;       // 存储所有数据
        private List<Column> _columns;
        private ColumnScale _scale;
        private enum _ColumnWidth : int
        { BIG = 400, MIDDLE = 300, SMALL = 200 };
        private double _width;          // ScrollViewer的宽度
        private int _colWidth;
        private int _height;

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

        public StaticDiagram(double width, DataModel model)
        {
            initializeData(width, model);
            initializeGraphics();
            initializeHandler();
        }
        #endregion

        #region 初始化
        private void initializeData(double width, DataModel model)
        {
            _width = width;
            _colWidth = adjustColumnWidth(width, model.DefaultColumnNumber); 
            _model = model;
            _columns = new List<Column>();
            _panel = new StackPanel();

            
            //Button start = new Button() { Content = "开始动态画图", Height = 50, Width = 100 };
            //Button end = new Button() { Content = "停止", Height = 50, Width = 100 };
            //start.Click += new RoutedEventHandler(startButtonClicked);
            //end.Click += new RoutedEventHandler(endButtonClicked);
            //_panel.Children.Add(start);
            //_panel.Children.Add(end);
            

            List<List<Data>> list = new List<List<Data>>();
            for (int i = 0; i < _model.DefaultColumnNumber; ++i)
            {
                List<Data> datalist = new List<Data>();
                list.Add(datalist);
            }
            for (int i = 0; i < _model.DataList.Count; ++i)
            {
                Data d = _model.DataList.ElementAt(i);
                if (d._defaultColumnPos.Count == 0)
                    continue;
                for (int j = 0; j < d._defaultColumnPos.Count; ++j)
                    list.ElementAt(d._defaultColumnPos.ElementAt(j) - 1).Add(d);
            }

            _scale = new ColumnScale(_model.DEPTMEAS._min, _model.DEPTMEAS._max, _colWidth);
            _panel.Children.Add(_scale);
            _height = _scale.CanvasHeight;
            for (int i = 0; i < _model.DefaultColumnNumber; ++i)
            {
                Column c = new Column(_colWidth, _height, list.ElementAt(i), _model.DEPTMEAS._data.Min(), _model.DEPTMEAS._data.Max(), _scale.Scale, _model);
                _columns.Add(c);
                _panel.Children.Add(c);
            }
        }

        private void initializeGraphics()
        {
            this.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            this.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            
            _panel.HorizontalAlignment = HorizontalAlignment.Center;
            _panel.VerticalAlignment = VerticalAlignment.Top;
            _panel.Orientation = Orientation.Horizontal;
            this.Content = _panel;
            //drawGraphics();
        }

        private void initializeHandler()
        {
            this.AddHandler(Column.delColumnEvent, new RoutedEventHandler(delColumn));
            this.AddHandler(Column.saveConfigEvent, new RoutedEventHandler(saveDataConfig));
        }
        #endregion 

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

        #region 事件处理函数
        public void addColumn(int pos, List<Data> list)
        {
            Column c = new Column(_colWidth, _height, list, _model.DEPTMEAS._data.Min(), _model.DEPTMEAS._data.Max(), _scale.Scale, _model);
            _columns.Insert(pos-1, c);
            _panel.Children.Insert(pos, c);
            adjustGraphics();
            drawGraphics();
        }

        private void delColumn(object sender, RoutedEventArgs args)
        {
            delEventArgs e = (delEventArgs)args;
            _columns.RemoveAt(e.index-1);           // stackpanel中多一列ColumnScale
            _panel.Children.RemoveAt(e.index);
            adjustGraphics();
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
                _panel.Children.RemoveRange(0, _panel.Children.Count);
                _scale.adjustGraphics(width);
                _panel.Children.Add(_scale);
                foreach (Column c in _columns)
                {
                    _panel.Children.Add(c);
                }
                _colWidth = width;
            }
        }
        #endregion

        #region 动态图表
        private int _rowCount = 0;
        private int _depth ;


        public void addData(int rowCount, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count - rowCount; ++i)
            {
                _model.appendData(dt, rowCount + i);
                _depth++;
                _rowCount++;
            }
        }

        public void startDynamicDrawing(DataTable dt)
        {
            _model._dt = dt;
            _rowCount = dt.Rows.Count;
            _depth = (int)_model.DEPTMEAS._data.Max();
            drawGraphics();
        }


        public void adjustHeader(int index)
        {
            foreach (Column c in _columns)
            {
                ColumnHeader header = c.Header;
                foreach (ColumnHeaderData data in header.Data)
                {
                    double max = Double.Parse(data.Lblmax.Content.ToString());
                    double min = Double.Parse(data.Lblmin.Content.ToString());
                    if (max != data.Data._max || max != data.Data._min)
                    {
                        data.adjustLabel();
                        c.Body.repaint();
                        c.drawGraphics();
                    }
                }
            }
            double showHeight = _scale.CanvasHeight * _scale.Scale;
            if (_depth >= showHeight + _model.DEPTMEAS._min - 100)
            {
                _scale.adjustScale(_depth + 20);
                foreach (Column c in _columns)
                {
                    c._scale = _scale.Scale;
                    c.Body.repaint();
                    c.drawGraphics();
                }
            }
            else
            {
                addGraphics(_rowCount - index);
            }

        }

        private void TimerTicked(object sender, EventArgs args)
        {
            //addData();
            //adjustHeader();
            double showHeight = _scale.CanvasHeight * _scale.Scale;
            if (_depth >= showHeight + _model.DEPTMEAS._min - 100)
            {
                _scale.adjustScale(_depth+500);
                foreach (Column c in _columns)
                {
                    c._scale = _scale.Scale;
                    c.Body.repaint();
                    c.drawGraphics();
                }
            }
            else 
            {
                addGraphics(_rowCount - 20);
            }
        }

        private void addGraphics(int startIndex)
        {
            foreach (Column c in _columns)
            {
                c.addGraphics(startIndex);
            }
        }
        #endregion
        
        public void drawGraphics()
        {
            foreach (Column column in _columns)
            {
                column.Body.repaint();
                column.drawGraphics();
            }
        }

        public void saveDataConfig(object sender, RoutedEventArgs args)
        {
            XmlDocument doc = _model.Doc;
            XmlNode root = doc.DocumentElement;
            XmlNodeList nodeList = root.ChildNodes;
            String prefix = "Diagram.StaticDiagram.";

            // 保存默认值
            /*
            root = nodeList[0];
            root.RemoveAll();
            List<Data> defaultList = _model.DataList;
            for (int i = 0; i < defaultList.Count; ++i)
            {
                XmlElement xe = doc.CreateElement("DataItem");
                Data data = defaultList.ElementAt(i);
                if (data._min != data._data.Min() && data._max != data._data.Max())
                {
                    xe.SetAttribute("min", data._min.ToString());
                    xe.SetAttribute("max", data._max.ToString());
                }
                xe.InnerText = prefix + data._name;
                root.AppendChild(xe);
            
            }
             */

            // 保存默认列
            root = nodeList[1];
            root.RemoveAll();

            List<Data> list = _model.DataList;
            for (int i = 0; i < _columns.Count; ++i)
            {
                ColumnHeader header = _columns.ElementAt(i).Header;
                List<ColumnHeaderData> data = header.Data;

                XmlElement xe = doc.CreateElement("Column");
                xe.InnerText = prefix + data.ElementAt(0).Data._name;
                for (int j = 1; j < data.Count; ++j)
                {
                    xe.InnerText += "," + prefix + data.ElementAt(j).Data._name;
                }
                root.AppendChild(xe);
            }
            doc.Save(_model.Filepath);
        }
    }
}
