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
using System.Data.SqlClient;


namespace diagram.DynamicDiagram
{
    public class TimeBasedDynamicDiagram : ScrollViewer
    {
        #region Constructor
        public TimeBasedDynamicDiagram(double width, DataModel model)
        {
            initializeData(width, model);
            initializeGraphics();
            initializeHandler();
        }
        #endregion

        #region Properties
        private StackPanel _panel;

        private DataModel _model;       // 存储所有数据
        private List<Column> _columns;
        private ScaleColumn _scale;

        public ScaleColumn Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }
        private enum _ColumnWidth : int
        { BIG = 400, MIDDLE = 300, SMALL = 200 };
        private double _width;          // ScrollViewer的宽度
        private double _colWidth;
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
        #endregion

        #region Initialization
        private void initializeData(double width, DataModel model)
        {
            _width = width;

            _colWidth = adjustColumnWidth(width, model.DataList.Count);
            _model = model;
            _columns = new List<Column>();
            _panel = new StackPanel();

            List<List<Data>> list = new List<List<Data>>();
            for (int i = 0; i < _model.ColumnNumber; ++i)
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

            _scale = new ScaleColumn(_colWidth);
            _panel.Children.Add(_scale);
            _height = _scale.CanvasHeight;
            for (int i = 0; i < _model.ColumnNumber; ++i)
            {
                Column c = new Column(_colWidth, _height, list.ElementAt(i), _model);
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
        }

        private void initializeHandler()
        {
            this.AddHandler(Column.delColumnEvent, new RoutedEventHandler(delColumn));
            this.AddHandler(Column.saveConfigEvent, new RoutedEventHandler(saveDataConfig));
        }
        #endregion

        #region Methods
        public int adjustColumnWidth(double width, int colnum)
        {
            if (width / (int)_ColumnWidth.BIG >= colnum) { return (int)_ColumnWidth.BIG; }
            else if (width / (int)_ColumnWidth.MIDDLE >= colnum) { return (int)_ColumnWidth.MIDDLE; }
            else { return (int)_ColumnWidth.SMALL; }
        }

        public void addColumn(int pos, List<Data> list)
        {
            Column c = new Column(_colWidth, _height, list, _model);
            _columns.Insert(pos - 1, c);
            _panel.Children.Insert(pos, c);
            adjustGraphics();
        }
        #endregion

        #region RoutingMethods
        private void delColumn(object sender, RoutedEventArgs args)
        {
            delEventArgs e = (delEventArgs)args;
            _columns.RemoveAt(e.index - 1);           // stackpanel中多一列ColumnScale
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
                    //c.drawGraphics();
                }
                _panel.Children.RemoveRange(0, _panel.Children.Count);
                //_scale.adjustGraphics(width);
                _panel.Children.Add(_scale);
                foreach (Column c in _columns)
                {
                    _panel.Children.Add(c);
                }
                _colWidth = width;
            }
        }
        #endregion

        #region TimeBasedGraphics

        public void startDynamicDrawing()
        {
            
        }

        public void getData(Time last, ScaleData data, DataSet ds)
        {
            Time first = last.subtractMinutes(_model.DisplayInterval);
            data.FirstTime = first;

            int diff = last.get_diff_by_minute(first);
            data.getTime(last, diff);
            int rowCount = ds.Tables[0].Rows.Count;
            for (int i = 0; i < 2; ++i)
            {
                data.depth[i] = ds.Tables[0].Rows[rowCount / 3 * (i + 1)]["DEPTH"].ToString();
                data.pos[i] = rowCount / 3 * (i + 1);
            }

            _model.getData(ds);
        }



        public void getData(SqlDataAccess conn,
                            String TableName,
                            String WellID,
                            String WellBoreID,
                            ScaleData data)
        {
            DataSet dataSet = new DataSet();
            dataSet = conn.SelectDataSet("SELECT TOP 1 * FROM " + TableName + " WHERE WELLID = '" + WellID
                                            + "' AND WELLBOREID ='" + WellBoreID + "' ORDER BY TDATE DESC, TTIME DESC");
            String date = dataSet.Tables[0].Rows[0]["TDATE"].ToString();
            String time = dataSet.Tables[0].Rows[0]["TTIME"].ToString();
            // 数据截止时间
            Time last = new Time(date, time);
            Time first = last.subtractMinutes(_model.DisplayInterval);
            date = first.ToDateString();
            time = first.ToTimeString();

            dataSet.Clear();
            dataSet.Dispose();

            DataSet ds = new DataSet();

            string str = "SELECT * FROM " + TableName + " Where (TDATE = '" + date + "' And TTime >= '" + time + "') Or (TDate >'" + date + "') AND WELLID = '" + WellID + "' AND WELLBOREID ='" + WellBoreID + "' ORDER BY TDATE asc,TTIME asc";

            

            //ds = conn.SelectDataSet("SELECT * FROM (SELECT * FROM " + TableName
            //                        + " WHERE TDATE >= '" + date + "') AS TEMP WHERE TEMP.TTIME >= '" + time + "' ORDER BY TEMP.TDATE asc, TEMP.TTIME asc");
            
            
            ds = conn.SelectDataSet(str);

            int N = ds.Tables[0].Rows.Count;


            // 数据起始时间
            first = getDateTime(ds.Tables[0].Rows[0]["TDATE"].ToString(),
                                     ds.Tables[0].Rows[0]["TTIME"].ToString());

            data.FirstTime = first;
            // 获取两时间的差值
            int diff = last.get_diff_by_minute(first);
            data.getTime(last, diff);

            int rowCount = ds.Tables[0].Rows.Count;
            for (int i = 0; i < 2; ++i)
            {
                data.depth[i] = ds.Tables[0].Rows[rowCount / 3 * (i + 1)]["DEPTH"].ToString();
                data.pos[i] = rowCount / 3 * (i + 1);
            }

            _model.getData(ds);
            ds.Clear();
            ds.Dispose();
        }

        // 调整表头
        public void adjustHeader()
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
                    }
                }
            }
        }

        // 用于获取正确的时间
        private Time getDateTime(String date, String time)
        {
            String[] DATE = date.ToString().Split('/');
            String[] TIME = time.ToString().Split(':');

            Time datetime = new Time(Convert.ToInt32(DATE[0]),
                                             Convert.ToInt32(DATE[1]),
                                             Convert.ToInt32(DATE[2]),
                                             Convert.ToInt32(TIME[0]),
                                             Convert.ToInt32(TIME[1]),
                                             Convert.ToInt32(TIME[2]));
            return datetime;
        }

        private void TimerTicked(object sender, EventArgs args)
        {
            //ScaleData data = getData();
            //adjustHeader();
            //foreach (Column col in _columns)
            //{
            //    col.repaint(data);
            //}
            //_scale.repaintScale(data);
            //double showHeight = _scale.CanvasHeight * _scale.Scale;
            //if (_depth >= showHeight + _model.DEPTMEAS._min - 100)
            //{
            //    _scale.adjustScale(_depth+500);
            //    foreach (Column c in _columns)
            //    {
            //        c._scale = _scale.Scale;
            //        c.Body.repaint();
            //        c.drawGraphics();
            //    }
            //}
            //else 
            //{
            //    addGraphics(_rowCount - 20);
            //}
        }

        public void endDrawing()
        {
            
        }

        public void saveDataConfig(object sender, RoutedEventArgs args)
        {
            XmlDocument doc = _model.Doc;
            XmlNode root = doc.DocumentElement;
            XmlNodeList nodeList = root.ChildNodes;
            String prefix = "Diagram.DynamicDiagram.";

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
                xe.InnerText = prefix + data.ElementAt(0).Lblname.Content.ToString();
                for (int j = 1; j < data.Count; ++j)
                {
                    xe.InnerText += "," + prefix + data.ElementAt(j).Lblname.Content.ToString();
                }
                root.AppendChild(xe);
            }
            doc.Save(_model.Filepath);
        }
        #endregion

    }
}
