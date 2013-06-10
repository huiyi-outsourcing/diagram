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

namespace diagram.DynamicDiagram
{
    /// <summary>
    /// ChangeIntervalWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeIntervalWindow : Window
    {
        #region Properties
        DataModel _model;
        #endregion

        #region Constructor
        public ChangeIntervalWindow(DataModel model)
        {
            InitializeComponent();
            InitData(model);
        }
        #endregion

        #region Methods
        private void InitData(DataModel model)
        {
            _model = model;
            tbInterval.Text = (_model.Interval / 1000).ToString();
            tbDisplayInterval.Text = _model.DisplayInterval.ToString();
        }
        #endregion

        #region Event Handler
        private void btnResetClicked(object sender, RoutedEventArgs e)
        {
            tbInterval.Text = (_model.Interval / 1000).ToString();
            tbDisplayInterval.Text = _model.DisplayInterval.ToString();
        }

        private void btnConfirmlicked(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.DisplayInterval = Int32.Parse(tbDisplayInterval.Text);
                _model.Interval = Int32.Parse(tbInterval.Text) * 1000;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "请输入整数");
            }
        }
        #endregion
    }
}
