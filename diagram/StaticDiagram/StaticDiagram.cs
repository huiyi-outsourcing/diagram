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

        #region Constructor
        private void initializeData(double width, DataModel model)
        {
            _width = width;
            _colWidth = adjustColumnWidth(width, model.DefaultColumnNumber); 
            _model = model;
            _columns = new List<Column>();
            _panel = new StackPanel();

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

            _scale = new ColumnScale(_model.DEPTMEAS.Min, _model.DEPTMEAS.Max, _colWidth);
            _panel.Children.Add(_scale);
            _height = _scale.CanvasHeight;
            for (int i = 0; i < _model.DefaultColumnNumber; ++i)
            {
                Column c = new Column(_colWidth, _height, list.ElementAt(i), _model.DEPTMEAS.DData.Min(), _model.DEPTMEAS.DData.Max(), _scale.Scale, _model);
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

        #region Event Handling Methods
        public void addColumn(int pos, List<Data> list)
        {
            Column c = new Column(_colWidth, _height, list, _model.DEPTMEAS.DData.Min(), _model.DEPTMEAS.DData.Max(), _scale.Scale, _model);
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

        public void saveDataConfig(object sender, RoutedEventArgs args)
        {
            _model.saveDataConfig(_columns);
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