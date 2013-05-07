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
using diagram.Common;
using System.Data;
using System.Data.Sql;
using diagram.StaticDiagram;

namespace diagram
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlDataAccess Dconn = new SqlDataAccess("Data Source=(local);Initial Catalog=数据接收数据库;Integrated Security=True");

        public MainWindow()
        {
            InitializeComponent();


            TabControl _control = new TabControl();
            
            //#region 静态图表
            //SqlDataAccess conn = new SqlDataAccess("Data Source=(local);Initial Catalog=数据发送数据库;Integrated Security=True");

            //DataSet ds = conn.SelectDataSet("Select * from WS_Drilling_Depth_Based where WELLID = '龙109井' and WELLBOREID ='主井眼' order by DEPTMEAS asc");

            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    System.Windows.MessageBox.Show("'龙109井'的录井数据为空，请重新选择！");
            //    return;
            //}
            //else
            //{
            //    DataModel model = new DataModel("..\\..\\StaticDiagram\\DataConfig.xml", ds);
            //    StaticDiagram.StaticDiagram diagram = new StaticDiagram.StaticDiagram(800, model);
            //    this.Content = diagram;
            //}
            //#endregion
     
        //    System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
        //    _timer.Enabled = true;
        //    _timer.Interval = 5000;
        //    _timer.Tick += new EventHandler(time_Tick);
        }

        //private void time_Tick(object sender, EventArgs args)
        //{
        //    DataSet ds = Dconn.SelectDataSet("Select * from WS_Drilling_Depth_Based where WELLID = '龙109井' and WELLBOREID ='主井眼' order by DEPTMEAS asc");

        //    DynamicDiagram.DataModel model = new DynamicDiagram.DataModel("..\\..\\DynamicDiagram\\DataConfig.xml", ds);
        //    DynamicDiagram.TimeBasedDynamicDiagram diagram = new DynamicDiagram.TimeBasedDynamicDiagram(800, model);
        //    this.Content = diagram;
        //}
    }
}
