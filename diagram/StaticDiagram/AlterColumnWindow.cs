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
using System.Windows.Shapes;

using diagram.Common;

namespace diagram.StaticDiagram
{
    /*
     * 用于点击"变更"按钮后的弹出对话框
     */ 
    class AlterColumnWindow : Window
    {
        #region properties
        // 图形试图元素
        private TabControl _tab;                    
        private Grid _grid;
        private StackPanel _panel;
        public TabControl Tab
        {
            get { return _tab; }
            set { _tab = value; }
        }

        private Column _column;                         // 用于保存需要更改的列的引用
        private List<ColumnHeaderData> _list;           // 用于获取需要更改的列的表头数据
        #endregion

        /*
         * 构造函数: 
         * 参数一：用于传入需要更改的列
         */ 
        public AlterColumnWindow(Column column)
        {
            // 初始化图形元素
            this.Width = 300;
            this.Height = 200;
            _column = column;
            _list = _column.Header.Data;
            _grid = new Grid();
            _grid.ColumnDefinitions.Add(new ColumnDefinition());
            _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(130) });
            _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
            _tab = new TabControl();
            Grid.SetColumn(_tab, 0);
            Grid.SetRow(_tab, 0);

            initializeTabControl();
            initializeButton();

            _grid.Children.Add(_panel);
            _grid.Children.Add(_tab);
            this.Content = _grid;
        }

        #region 初始化图形元素
        // 初始化TabControl控件
        private void initializeTabControl()
        {
            ColumnHeader header = _column.Header;
            foreach (ColumnHeaderData head in header.Data)
            {
                Data data = head.Data;
                createTabItem(data);
            }
        }

        // 创建TabControl中的TabItem
        public void createTabItem(Data data)
        {
            TabItem item = new TabItem();
            item.Header = data._name;
            item.Tag = data;

            Grid grid = new Grid();
            ColumnDefinition left = new ColumnDefinition();
            left.Width = new GridLength(0.3, GridUnitType.Star);
            ColumnDefinition right = new ColumnDefinition();
            right.Width = new GridLength(0.7, GridUnitType.Star);

            grid.ColumnDefinitions.Add(left);
            grid.ColumnDefinitions.Add(right);

            for (int i = 0; i < 4; ++i)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            Label leftname = new Label() { Content = "名称" };
            Grid.SetColumn(leftname, 0);
            Grid.SetRow(leftname, 0);
            Label leftmin = new Label() { Content = "最小值" };
            Grid.SetColumn(leftmin, 0);
            Grid.SetRow(leftmin, 1);
            Label leftmax = new Label() { Content = "最大值" };
            Grid.SetColumn(leftmax, 0);
            Grid.SetRow(leftmax, 2);
            grid.Children.Add(leftname);
            grid.Children.Add(leftmin);
            grid.Children.Add(leftmax);

            TextBlock name = new TextBlock() { Text = data._name };
            Grid.SetColumn(name, 1);
            Grid.SetRow(name, 0);
            TextBox min = new TextBox() { Text = data._min.ToString() };
            Grid.SetColumn(min, 1);
            Grid.SetRow(min, 1);
            TextBox max = new TextBox() { Text = data._max.ToString() };
            Grid.SetColumn(max, 1);
            Grid.SetRow(max, 2);
            grid.Children.Add(name);
            grid.Children.Add(min);
            grid.Children.Add(max);

            item.Content = grid;
            _tab.Items.Add(item);
        }

        // Window中的Button
        private void initializeButton()
        {
            _panel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            Button add = new Button() { Content = "添加", Width = 40, Height = 30 };
            add.Click += new RoutedEventHandler(addButtonClicked);
            Button confirm = new Button() { Content = "确定", Width = 40, Height = 30 };
            confirm.Click += new RoutedEventHandler(confirmButtonClicked);
            Button delete = new Button() { Content = "删除", Width = 40, Height = 30 };
            delete.Click += new RoutedEventHandler(deleteButtonClicked);
            Button cancel = new Button() { Content = "取消", Width = 40, Height = 30 };
            cancel.Click += new RoutedEventHandler(cancelButtonClicked);
            _panel.Children.Add(add);
            _panel.Children.Add(confirm);
            _panel.Children.Add(delete);
            _panel.Children.Add(cancel);
            Grid.SetColumn(_panel, 0);
            Grid.SetRow(_panel, 1);
        }
        #endregion

        #region 路由事件
        // 点击 添加 按钮后的响应事件
        private void addButtonClicked(object sender, RoutedEventArgs args)
        {
            StackPanel panel = _column.Parent as StackPanel;
            StaticDiagram diagram = panel.Parent as StaticDiagram;
            int index = panel.Children.IndexOf(_column);
            ChooseColumnWindow window = new ChooseColumnWindow(diagram, index, this);
            window.Show();
        }

        // 点击 确定 按钮后的响应事件
        private void confirmButtonClicked(object sender, RoutedEventArgs args)
        {
            List<ColumnHeaderData> list = new List<ColumnHeaderData>();

            for (int i = 0; i < _tab.Items.Count; ++i)
            {
                TabItem item = _tab.Items[i] as TabItem;
                Data data = item.Tag as Data;

                Grid grid = item.Content as Grid;
                TextBox min = grid.Children[4] as TextBox;
                TextBox max = grid.Children[5] as TextBox;
                data._min = Double.Parse(min.Text);
                data._max = Double.Parse(max.Text);

                ColumnHeaderData headerData = new ColumnHeaderData(data);
                list.Add(headerData);
            }

            _column.Children.Remove(_column.Header);
            _column.Children.Remove(_column.Body);
            _column.Header = new ColumnHeader(list);
            _column.initializeHeader();
            _column.Body.Children.RemoveRange(0, _column.Body.Children.Count);
            _column.Body = new ColumnBody(Convert.ToInt32(_column.Body.Width), Convert.ToInt32(_column.Body.Height));
            _column.initializeBody();

            _column.drawGraphics();

            this.Close();
        }

        // 点击 删除 按钮后的响应事件
        private void deleteButtonClicked(object sender, RoutedEventArgs args)
        {
            _tab.Items.RemoveAt(_tab.Items.IndexOf(_tab.SelectedItem));
        }

        // 点击 取消 按钮后的响应事件
        private void cancelButtonClicked(object sender, RoutedEventArgs args)
        {
            this.Close();
        }
        #endregion
    }
}
