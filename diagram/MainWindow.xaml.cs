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
using System.Xml;

using System.Threading;
using System.Reflection;

namespace diagram
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TabControl _control = new TabControl();

            #region 静态图表
            TabItem _Sitem = new TabItem();
            _Sitem.Header = "静态绘图";

            // 连接数据库
            SqlDataAccess Sconn = new SqlDataAccess("Data Source=(local);Initial Catalog=数据发送数据库;Integrated Security=True");

            DataSet Sds = Sconn.SelectDataSet("Select * from WS_Drilling_Depth_Based where WELLID = '龙109井' and WELLBOREID ='主井眼' order by DEPTMEAS asc");

            if (Sds.Tables[0].Rows.Count == 0)
            {
                System.Windows.MessageBox.Show("'龙109井'的录井数据为空，请重新选择！");
                return;
            }
            else
            {
                StaticDiagram.DataModel Smodel = new StaticDiagram.DataModel("..\\..\\StaticDiagram\\DataConfig.xml", Sds);
                StaticDiagram.StaticDiagram diagram = new StaticDiagram.StaticDiagram(900, Smodel, "..\\..\\StaticDiagram\\DiagramConfig.xml");
                diagram.drawGraphics();
                _Sitem.Content = diagram;
                _control.Items.Add(_Sitem);
            }
            #endregion

            #region 动态图表
            _Ditem = new TabItem();
            _Ditem.Header = "动态绘图";
            initializeDataBase();
            initializeGraphics();

            System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

            _timer.Enabled = true;
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(DataCollect);

            _control.Items.Add(_Ditem);
            #endregion

            this._grid.Children.Add(_control);
        }

        #region 模拟动态数据传输
        // Properties
        TabItem _Ditem;
        SqlDataAccess DSconn;
        SqlDataAccess DRconn;
        DataSet ReceiveDs;
        DataSet SendDs;

        DynamicDiagram.DataModel Dmodel;
        DynamicDiagram.TimeBasedDynamicDiagram Ddiagram;
        DynamicDiagram.ScaleData data;

        static int lineNumber = 0;
        // 动态图标初始化
        private void initializeGraphics()
        {
            Dmodel = new DynamicDiagram.DataModel("..//..//DynamicDiagram//TimeBasedDataConfig.xml", ReceiveDs);
            Ddiagram = new DynamicDiagram.TimeBasedDynamicDiagram(900, Dmodel, "..//..//Dynamicdiagram//DiagramConfig.xml");
            data = new DynamicDiagram.ScaleData();

            Ddiagram.startDynamicDrawing(DRconn, "WS_Drilling_Depth_Based", "龙109井", "主井眼", data);

            //Ddiagram.getData(DRconn, "WS_Drilling_Depth_Based", "龙109井", "主井眼", data);

            _Ditem.Content = Ddiagram;
        }

        private void initializeDataBase()
        {
            DSconn = new SqlDataAccess("Data Source=(local);Initial Catalog=数据发送数据库;Integrated Security=True");
            DRconn = new SqlDataAccess("Data Source=(local);Initial Catalog=数据接收数据库;Integrated Security=True");
            SendDs = DSconn.SelectDataSet("Select * from WS_Drilling_Depth_Based where WELLID = '龙109井' and WELLBOREID ='主井眼' order by TDATE ASC, TTIME ASC");

            string sql = "DELETE FROM WS_Drilling_Depth_Based where WELLID = '龙109井' and WELLBOREID ='主井眼'";
            DRconn.ExeSQL(sql);
        }

        private void DataCollect(object sender, EventArgs args)
        {
            // 模拟传输数据
            if (lineNumber > SendDs.Tables[0].Rows.Count)
                return;

            DataRow row = SendDs.Tables[0].Rows[lineNumber++];

            string insert = "Insert into WS_Drilling_Depth_Based (WELLID,WELLBOREID,STKNUM,RECID,SEQID,TDATE,TTIME,ACTCOD,DEPTMEAS,DEPTVERT,ROPA,WOBA,HKLA,SPPA,TORQA,RPMA,BTREVC,MDIA,ECDTD,MFIA,MFOA,MFOP,TVOLACT,CPDI,CPDC,BTDTIME,BTDDIST,DXC,PipeNo,PipeLength,KellyDown,Dcn,pf,Frac,DRTM,BPOS,SPM1,SPM2,SPM3,HookTorq,CHKP,MDOA,MTOA,MTIA,MCOA,MCIA,SFLAG) VALUES('" + "龙109井" + "','"
                                       + "主井眼" + "','" + row["STKNUM"].ToString() + "','" + row["RECID"].ToString() + "','" + row["SEQID"].ToString() + "','" + row["TDATE"].ToString() + "','" + row["TTIME"].ToString() + "','" + row["ACTCOD"].ToString() + "','" + row["DEPTMEAS"].ToString() + "','"
               + row["DEPTVERT"].ToString() + "','" + row["ROPA"].ToString() + "','" + row["WOBA"].ToString() + "','" + row["HKLA"].ToString() + "','" + row["SPPA"].ToString() + "','" + row["TORQA"].ToString() + "','" + row["RPMA"].ToString() + "','" + row["BTREVC"].ToString() + "','" + row["MDIA"].ToString() + "','" + row["ECDTD"].ToString() + "','" + row["MFIA"].ToString() + "','" + row["MFOA"].ToString() + "','" + row["MFOP"].ToString() + "','"
               + row["TVOLACT"].ToString() + "','" + row["CPDI"].ToString() + "','" + row["CPDC"].ToString() + "','" + row["BTDTIME"].ToString() + "','" + row["BTDDIST"].ToString() + "','" + row["DXC"].ToString() + "','" + row["PipeNo"].ToString() + "','" + row["PipeLength"].ToString() + "','" + row["KellyDown"].ToString() + "','" + row["Dcn"].ToString() + "','" + row["pf"].ToString() + "','" + row["Frac"].ToString() + "','" + row["DRTM"].ToString() + "','"
               + row["BPOS"].ToString() + "','" + row["SPM1"].ToString() + "','" + row["SPM2"].ToString() + "','" + row["SPM3"].ToString() + "','" + row["HookTorq"].ToString() + "','" + row["CHKP"].ToString() + "','" + row["MDOA"].ToString() + "','" + row["MTOA"].ToString() + "','" + row["MTIA"].ToString() + "','" + row["MCOA"].ToString() + "','" + row["MCIA"].ToString() + "','" + row["SFLAG"].ToString() + "')";

            DRconn.ExeSQL(insert);
            
            // DynamicDiagram获取数据
            //Ddiagram.getData(DRconn, "WS_Drilling_Depth_Based", "龙109井", "主井眼", data);

            //Ddiagram.adjustHeader();
            //foreach (DynamicDiagram.Column col in Ddiagram.Columns)
            //{
            //    col.repaint(data);
            //}
            //Ddiagram.Scale.repaintScale(data);
        }
        #endregion
    }
}
