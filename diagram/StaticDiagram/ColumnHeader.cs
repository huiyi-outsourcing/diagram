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

namespace diagram.StaticDiagram
{
    class ColumnHeader : Grid
    {
        // 数据元素
        private List<ColumnHeaderData> _data;          // 存储表头数据

        // 图形元素
        private Border _border;
        private readonly Brush _brush = Brushes.LightBlue;
        private readonly int _ColumnNum = 3;                             // 列数
        private int _minHeight;                                         // 表头最小高度
        private int _defaultHeight;                                     // 默认高度
        
        public List<ColumnHeaderData> Data
        {
          get { return _data; }
          set { _data = value; }
        }

        public ColumnHeader(List<ColumnHeaderData> datalist)
        {
            initializeData(datalist);
            initializeGraphics();
        }

        #region 初始化
        private void initializeGraphics()
        {
            if (_data != null)
            {
                // 绘制Grid
                ColumnDefinition cdleft = new ColumnDefinition();
                cdleft.Width = new GridLength(0.2, GridUnitType.Star);
                ColumnDefinition cdcenter = new ColumnDefinition();
                cdcenter.Width = new GridLength(0.6, GridUnitType.Star);
                //cdcenter.MinWidth = width * 0.6;
                ColumnDefinition cdright = new ColumnDefinition();
                cdright.Width = new GridLength(0.2, GridUnitType.Star);
                this.ColumnDefinitions.Add(cdleft);
                this.ColumnDefinitions.Add(cdcenter);
                this.ColumnDefinitions.Add(cdright);
                this.RowDefinitions.Add(new RowDefinition() { MinHeight = _minHeight, MaxHeight = _defaultHeight });

                addLabel();
            }

            // 绘制边框
            _border = new Border();
            _border.BorderBrush = _brush;
            _border.BorderThickness = new Thickness(1);
            if (_data != null)
            {
                Grid.SetColumnSpan(_border, _ColumnNum);
                Grid.SetRowSpan(_border, _data.Count);
            }
            this.Children.Add(_border);
        }

        public void addLabel()
        {
            for (int i = 0; i < _ColumnNum; ++i)
            {
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Vertical;
                panel.HorizontalAlignment = HorizontalAlignment.Center;
                panel.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(panel, i);
                Grid.SetRow(panel, 0);
                for (int j = 0; j < _data.Count; ++j)
                {
                    if (i == 0) 
                        panel.Children.Add(_data.ElementAt(j).Lblmin);
                    else if (i == 1)
                    {
                        _data.ElementAt(j).Lblname.Foreground = Column.colors[j];
                        panel.Children.Add(_data.ElementAt(j).Lblname);
                    }
                    else panel.Children.Add(_data.ElementAt(j).Lblmax);
                }
                this.Children.Add(panel);
            }
        }

        private void initializeData(List<ColumnHeaderData> datalist)
        {
            _data = datalist;

            XmlDocument xml = new XmlDocument();
            xml.Load("..\\..\\StaticDiagram\\DiagramConfig.xml");
            XmlNode node = xml.SelectSingleNode("Diagram/ColumnHeader/height");
            _defaultHeight = Int32.Parse(node.InnerText);
            node = xml.SelectSingleNode("Diagram/ColumnHeader/minheight");
            _minHeight = Int32.Parse(node.InnerText);
        }
        #endregion

        public void adujustGraphics(int width)
        {
            this.Width = width;
        }
    }
}
