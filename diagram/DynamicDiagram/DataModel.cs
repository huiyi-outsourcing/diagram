using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using System.Reflection;
using diagram.Common;

namespace diagram.DynamicDiagram
{
    public class DataModel
    {
        #region Constructor
        public DataModel(String filepath, DataSet ds)
        {
            initializeData(filepath, ds);
        }
        #endregion

        #region Properties
        private DataSet _ds;
        private XmlDocument _doc;
        private String _filepath;
        private List<Data> _dataList;
        private Data _TTIME;
        private Data _TDATE;
        private int _interval;
        private int _DisplayInterval;
        private int _columnNumber;

        public int ColumnNumber
        {
            get { return _columnNumber; }
            set { _columnNumber = value; }
        }

        public int DisplayInterval
        {
            get { return _DisplayInterval; }
            set { _DisplayInterval = value; }
        }

        public Data TTIME
        {
            get { return _TTIME; }
            set { _TTIME = value; }
        }

        public Data TDATE
        {
            get { return _TDATE; }
            set { _TDATE = value; }
        }

        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        public List<Data> DataList
        {
            get { return _dataList; }
            set { _dataList = value; }
        }

        public String Filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }

        public XmlDocument Doc
        {
            get { return _doc; }
            set { _doc = value; }
        }
        #endregion

        #region Initialization
        private void initializeData(String filepath, DataSet ds)
        {
            _ds = ds;
            _dataList = new List<Data>();
            _TTIME = new TTIME();
            _TDATE = new TDATE();

            _doc = new XmlDocument();

            // Load Interval
            _doc.Load("..\\..\\DynamicDiagram\\DiagramConfig.xml");
            XmlNode xNode = _doc.SelectSingleNode("Diagram/Interval");
            _interval = Int32.Parse(xNode.InnerText);
            xNode = _doc.SelectSingleNode("Diagram/DisplayInterval");
            _DisplayInterval = Int32.Parse(xNode.InnerText);

            // Load Data Config
            _doc.Load(filepath);
            _filepath = filepath;
            XmlNode root = _doc.DocumentElement;
            XmlNodeList list = root.ChildNodes;

            XmlNode dataNode = list[0];
            XmlNodeList dataList = dataNode.ChildNodes;
            foreach (XmlNode node in dataList)
            {
                String name = "diagram.Common." + node.InnerText;
                Assembly assembly = Assembly.GetAssembly(Type.GetType(name));
                Data d = (Data)assembly.CreateInstance(name);
                _dataList.Add(d);
            }

            XmlNode columnNode = list[1];
            XmlNodeList columnList = columnNode.ChildNodes;
            _columnNumber = columnList.Count;
            for (int loop = 0; loop < columnList.Count; ++loop)
            {
                XmlNode node = columnList[loop];
                String[] str = node.InnerText.Split(',');
                for (int i = 0; i < str.Length; ++i)
                {
                    String name = str[i];
                    foreach (Data d in _dataList)
                    {
                        if (d._name == name)
                        {
                            d._defaultColumnPos.Add(loop + 1);
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Methods
        public void getData(DataSet ds)
        {
            foreach (Data data in _dataList)
            {
                data.getData(ds);
            }

            _TDATE.getDataString(ds);
            _TTIME.getDataString(ds);
        }
        #endregion
    }
}
