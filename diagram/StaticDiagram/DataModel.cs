﻿using System;
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
        }

        #region Methods
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
                String name = "diagram.Common." + node.InnerText;
                Assembly assembly = Assembly.GetAssembly(Type.GetType(name));
                Data d = (Data)assembly.CreateInstance(name);
                d.initializeData(ds);
                
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
                    String name = str[i];
                    foreach (Data d in _dataList)
                    {
                        if (d.Name == name)
                        {
                            d.DefaultColumnPos.Add(loop+1);
                            break;
                        }
                    }
                }
            }
        }

        public void saveDataConfig(List<Column> columns)
        {
            XmlDocument doc = _doc;
            XmlNode root = doc.DocumentElement;
            XmlNodeList nodeList = root.ChildNodes;

            // 保存默认列
            root = nodeList[1];
            root.RemoveAll();

            List<Data> list = _dataList;
            for (int i = 0; i < columns.Count; ++i)
            {
                ColumnHeader header = columns.ElementAt(i).Header;
                List<ColumnHeaderData> data = header.Data;

                XmlElement xe = doc.CreateElement("Column");
                xe.InnerText = data.ElementAt(0).Data.Name;
                for (int j = 1; j < data.Count; ++j)
                {
                    xe.InnerText += "," + data.ElementAt(j).Data.Name;
                }
                root.AppendChild(xe);
            }
            doc.Save(_filepath);
        }

        //private void loadData(DataSet ds)
        //{
        //    _DEPTMEAS = new DEPTMEAS();
        //    _DEPTMEAS.initializeData(ds);
        //    foreach (Data data in _dataList)
        //    {
        //        data.initializeData(ds);
        //    }
        //}
        #endregion

        //public void appendData(DataTable dt, int startIndex)
        //{
        //    for (int i = startIndex; i < dt.Rows.Count; ++i)
        //    {
        //        _DEPTMEAS._data.Add(Convert.ToDouble(dt.Rows[i]["DEPTMEAS"]));
        //        foreach (Data data in _dataList)
        //        {
        //            data._data.Add(Convert.ToDouble(dt.Rows[i][data._name]));
        //            data.setSpan();
        //        }
        //    }
        //}
    }
}
