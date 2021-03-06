﻿using System;
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

using diagram.Common;

namespace diagram.DynamicDiagram
{
    public class ColumnHeaderData
    {
        #region Constructor
        public ColumnHeaderData(Data data)
        {
            _data = data;

            _lblname = new Label();
            _lblname.Content = _data.Chinese;
            _lblname.HorizontalAlignment = HorizontalAlignment.Center;
            _lblname.VerticalAlignment = VerticalAlignment.Center;
            _lblname.FontSize = 13;
            _lblname.Foreground = Brushes.LightBlue;

            _lblmin = new Label();
            _lblmin.Content = _data.Min.ToString();
            _lblmin.HorizontalAlignment = HorizontalAlignment.Center;
            _lblmin.VerticalAlignment = VerticalAlignment.Center;
            _lblmin.FontSize = 10;

            _lblmax = new Label();
            _lblmax.Content = _data.Max.ToString();
            _lblmax.HorizontalAlignment = HorizontalAlignment.Center;
            _lblmax.VerticalAlignment = VerticalAlignment.Center;
            _lblmax.FontSize = 10;
        }
        #endregion

        #region Properties
        // 数据元素
        private Data _data;

        public Data Data
        {
            get { return _data; }
            set { _data = value; }
        }

        // 图形元素
        private Label _lblname;      // 用于表示名称
        private Label _lblmin;       // 用于表示最小值
        private Label _lblmax;       // 用于表示最大值

        public Label Lblmax
        {
            get { return _lblmax; }
            set { _lblmax = value; }
        }

        public Label Lblname
        {
            get { return _lblname; }
            set { _lblname = value; }
        }

        public Label Lblmin
        {
            get { return _lblmin; }
            set { _lblmin = value; }
        }
        #endregion

        #region Methods
        public void adjustLabel()
        {
            _lblmin.Content =Math.Round(Convert.ToDouble(_data.Min.ToString()),2);
            _lblmax.Content =Math.Round(Convert.ToDouble(_data.Max.ToString()),2);
        }
        #endregion
    }
}
