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

namespace diagram.StaticDiagram
{
    /// <summary>
    /// ChangeScaleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeScaleWindow : Window
    {
        private ColumnScale _columnscale;
        private double _colWidth;

        public ChangeScaleWindow(ColumnScale scale, double colWidth)
        {
            InitializeComponent();

            initializeTextBox(scale, colWidth);
        }

        private void initializeTextBox(ColumnScale scale, double colWidth)
        {
            _columnscale = scale;
            _colWidth = colWidth;
            this.scaleBox.Text = ((_columnscale.Scale) * _columnscale.CanvasHeight).ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String scale = this.scaleBox.Text;
            _columnscale.adjustScaleWindow(Double.Parse(scale) / _columnscale.CanvasHeight);
            this.Close();
        }
    }
}
