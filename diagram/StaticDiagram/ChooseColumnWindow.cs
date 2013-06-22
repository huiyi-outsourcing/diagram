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

using diagram.Common;

namespace diagram.StaticDiagram
{
    /*
     * 类:ChooseColumnWindow
     * 作用：用于添加列时弹出选择列的对话框
     */ 
    class ChooseColumnWindow : Window
    {
        #region Properties
        // 图形元素
        private ListBox _listBox;
        private UIElement _invoker;

        private StaticDiagram _diagram;        // 用于获得动态图表的引用
        private int _index;                     // 用于存储列号
        #endregion

        /*
         * 构造函数
         */ 
        public ChooseColumnWindow(StaticDiagram diagram, int index, UIElement invoker)
        {
            initializeData(diagram, index, invoker);
            initializeGraphics();
        }

        #region 初始化
        private void initializeGraphics()
        {
            this.Title = "选择添加的列"; 
            this.Width = 250;
            this.Height = 250;

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(160) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(70) });

            Grid.SetColumn(_listBox, 0);
            Grid.SetRow(_listBox, 0);

            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            Button confirm = new Button() { Content = "确定", Height = 30, Width = 50 };
            confirm.Click += new RoutedEventHandler(confirmButtonClicked);
            Button cancel = new Button() { Content = "取消", Height = 30, Width = 50 };
            cancel.Click += new RoutedEventHandler(cancelButtonClicked);
            panel.Children.Add(confirm);
            panel.Children.Add(cancel);
            Grid.SetColumn(panel, 0);
            Grid.SetRow(panel, 1);

            grid.Children.Add(_listBox);
            grid.Children.Add(panel);

            this.Content = grid;
        }

        private void initializeData(StaticDiagram diagram, int index, UIElement invoker)
        {
            _diagram = diagram;
            _invoker = invoker;
            _index = index;
            _listBox = new ListBox();
            _listBox.SelectionMode = SelectionMode.Multiple;
            foreach (Data data in _diagram.Model.DataList)
            {
                ListBoxItem item = new ListBoxItem() { Content = data.Name, Tag = data };
                _listBox.Items.Add(item);
            }
        }
        #endregion

        #region 路由事件
        private void confirmButtonClicked(object sender, RoutedEventArgs args)
        {
            if (_listBox.SelectedItems.Count == 0)
            {
                cancelButtonClicked(sender, args);
                MessageBox.Show("未选中任何列");
            }
            else
            {
                List<Data> list = new List<Data>();
                foreach (ListBoxItem item in _listBox.Items)
                {
                    if (item.IsSelected)
                        list.Add((Data)item.Tag);
                }

                if (_invoker.GetType() == typeof(Column) || _invoker.GetType() == typeof(ColumnScale))
                {
                    _diagram.addColumn(_index, list);
                }
                else
                {
                    AlterColumnWindow window = _invoker as AlterColumnWindow;
                    foreach (Data data in list)
                    {
                        window.createTabItem(data);
                    }
                }
                this.Close();
            }
        }

        private void cancelButtonClicked(object sender, RoutedEventArgs args)
        {
            this.Close();
        }
        #endregion
    }
}
