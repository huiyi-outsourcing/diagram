using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Reflection;
using System.Data;
using diagram.Common;

namespace diagram.StaticDiagram
{
    class DataModel
    {
        #region properties
        private XmlDocument _doc;
        private int _defaultColumnNumber;
        private Data _DEPTMEAS;
        private List<Data> _dataList;
        private String _filepath;
        public DataTable _dt;

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

        public int DefaultColumnNumber
        {
            get { return _defaultColumnNumber; }
            set { _defaultColumnNumber = value; }
        }

        public Data DEPTMEAS
        {
            get { return _DEPTMEAS; }
            set { _DEPTMEAS = value; }
        }

        public List<Data> DataList
        {
            get { return _dataList; }
            set { _dataList = value; }
        }
        #endregion

        public DataModel(String path, DataSet ds)
        {
            initializeData(path, ds);
            _dt = ds.Tables[0];
            //loadData(ds);
        }

        private void initializeData(String path, DataSet ds)
        {
            _defaultColumnNumber = 0;
            _dataList = new List<Data>();

            _filepath = path;
            _doc = new XmlDocument();
            _doc.Load(_filepath);
            XmlNode root = _doc.DocumentElement;
            XmlNodeList list = root.ChildNodes;

            _DEPTMEAS = new DEPTMEAS();
            _DEPTMEAS.initializeData(ds);

            XmlNode dataNode = list[0];
            XmlNodeList dataList = dataNode.ChildNodes;
            foreach (XmlNode node in dataList)
            {
                Assembly assembly = Assembly.GetAssembly(Type.GetType(node.InnerText));
                Data d = (Data)assembly.CreateInstance(node.InnerText);
                d.initializeData(ds);
                /*
                if (node.Attributes.Count > 0)
                {
                    double min = Double.Parse(node.Attributes["min"].Value);
                    double max = Double.Parse(node.Attributes["max"].Value);
                    d.setSpan(min, max);
                }
                */
                _dataList.Add(d);
            }

            XmlNode columnNode = list[1];
            XmlNodeList columnList = columnNode.ChildNodes;
            _defaultColumnNumber = columnList.Count;
            for (int loop = 0; loop < columnList.Count; ++loop)
            {
                XmlNode node = columnList[loop];
                String[] str = node.InnerText.Split(',');
                for (int i = 0; i < str.Length; ++i)
                {
                    String[] nameArray = str[i].Split('.');
                    String name = nameArray[2].Trim();
                    foreach (Data d in _dataList)
                    {
                        if (d._name == name)
                        {
                            d._defaultColumnPos.Add(loop+1);
                            break;
                        }
                    }
                }
            }
        }

        private void loadData(DataSet ds)
        {
            _DEPTMEAS = new DEPTMEAS();
            _DEPTMEAS.initializeData(ds);
            foreach (Data data in _dataList)
            {
                data.initializeData(ds);
            }
        }

        public void appendData(DataTable dt, int startIndex)
        {
            for (int i = startIndex; i < dt.Rows.Count; ++i)
            {
                _DEPTMEAS._data.Add(Convert.ToDouble(dt.Rows[i]["DEPTMEAS"]));
                foreach (Data data in _dataList)
                {
                    data._data.Add(Convert.ToDouble(dt.Rows[i][data._name]));
                    data.setSpan();
                }
            }
        }

    }
}
