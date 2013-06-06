using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace diagram.Common
{
    public abstract class Data
    {
        #region Properties
        private String _name;
        private List<int> _defaultColumnPos;
        private double _min;
        private double _max;
        private List<double> _data;
        private List<String> _StringData;
        private string _Chinese;
        #endregion

        #region Getter / Setter
        public double Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public List<int> DefaultColumnPos
        {
            get { return _defaultColumnPos; }
            set { _defaultColumnPos = value; }
        }

        public double Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public List<double> DData
        {
            get { return _data; }
            set { _data = value; }
        }

        public List<String> StringData
        {
            get { return _StringData; }
            set { _StringData = value; }
        }

        public string Chinese
        {
            get { return _Chinese; }
            set { _Chinese = value; }
        }
        #endregion

        public Data()
        {
            _defaultColumnPos = new List<int>();
            _min = 0;
            _max = 0;
        }

        public void initializeData(DataSet ds)
        {
            _defaultColumnPos = new List<int>();
            _data = new List<double>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
            {
                _data.Add(Convert.ToDouble(ds.Tables[0].Rows[i][_name]));
            }
            setSpan();
        }

        public void getData(DataSet ds)
        {
            if (_data == null) _data = new List<double>();
            else _data.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
            {
                if (ds.Tables[0].Rows[i][_name] is DBNull)
                {
                    _data.Add(Convert.ToDouble("0.00"));
                }
                else
                {
                    _data.Add(Convert.ToDouble(ds.Tables[0].Rows[i][_name]));
                }
            }
            if (ds.Tables[0].Rows.Count != 0) setSpan();
        }

        public void getDataString(DataSet ds)
        {
            if (_data == null) _StringData = new List<String>();
            else _StringData.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
            {
                _StringData.Add(ds.Tables[0].Rows[i][_name].ToString());
            }
        }

        public virtual void setSpan()
        {
            if (_data.Min() == _data.Max())
            {
                _min = _data.Min();
                _max = _min + 1;
            }
            else
            {
                _min = _data.Min() * 0.8;
                _max = _data.Max() * 1.2;
            }
        }
    }

    #region ConcreteDataRegion

    public class DEPTMEAS : Data
    {
        public DEPTMEAS()
        {
            Name = "DEPTMEAS";
            Chinese = "深度(m)";
        }

        public override void setSpan()
        {
            Min = DData.Min();
            Max = DData.Max();
        }
    }

    public class BKHT : Data
    {
        public BKHT()
        {
            Name = "BKHT";
            Chinese = "钩位(m)";
        }

        public override void setSpan()
        {
            Min = DData.Min();
            Max = DData.Max();
        }
    }

    public class HOOKLOAD : Data
    {
        public HOOKLOAD()
        {
            Name = "HOOKLOAD";
            Chinese = "悬重(KN)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }


    public class ROP : Data
    {
        public ROP()
        {
            Name = "ROP";
            Chinese = "钻时(m/min)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }


    public class WOB : Data
    {
        public WOB()
        {
            Name = "WOB";
            Chinese = "钻压(KN)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }


    public class RPM : Data
    {
        public RPM()
        {
            Name = "RPM";
            Chinese = "转速(r/min)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }


    public class TORQUE : Data
    {
        public TORQUE()
        {
            Name = "TORQUE";
            Chinese = "转盘扭矩(KN/m)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    public class SPM1 : Data
    {
        public SPM1()
        {
            Name = "SPM1";
            Chinese = "泵冲数1(N/min)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    public class SPM2 : Data
    {
        public SPM2()
        {
            Name = "SPM2";
            Chinese = "泵冲数2(N/min)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    public class SPM3 : Data
    {
        public SPM3()
        {
            Name = "SPM3";
            Chinese = "泵冲数3(N/min)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    public class STANDPRES : Data
    {
        public STANDPRES()
        {
            Name = "STANDPRES";
            Chinese = "立管压力(Mpa)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    public class TOTALPIT : Data
    {
        public TOTALPIT()
        {
            Name = "TOTALPIT";
            Chinese = "总池体积(m3)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    class FLOWIN : Data
    {
        public FLOWIN()
        {
            Name = "FLOWIN";
            Chinese = "入口流量(L/s)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    class FLOWOUT : Data
    {
        public FLOWOUT()
        {
            Name = "FLOWOUT";
            Chinese = "出口流量(L/s)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    class WEIGHTIN : Data
    {
        public WEIGHTIN()
        {
            Name = "WEIGHTIN";
            Chinese = "入口密度(g/cm3)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    class WEIGHTOUT : Data
    {
        public WEIGHTOUT()
        {
            Name = "WEIGHTOUT";
            Chinese = "出口密度(g/cm3)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    class TEMPIN : Data
    {
        public TEMPIN()
        {
            Name = "TEMPIN";
            Chinese = "入口温度(°C)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    class TEMPOUT : Data
    {
        public TEMPOUT()
        {
            Name = "TEMPOUT";
            Chinese = "出口温度(°C)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    class CONDIN : Data
    {
        public CONDIN()
        {
            Name = "CONDIN";
            Chinese = "入口电导(ps/m)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    class CONDOUT : Data
    {
        public CONDOUT()
        {
            Name = "CONDOUT";
            Chinese = "出口电导(ps/m)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }


    class PIT1 : Data
    {
        public PIT1()
        {
            Name = "PIT1";
            Chinese = "1#池体积(m3)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 5.0;
        }
    }

    class PIT2 : Data
    {
        public PIT2()
        {
            Name = "PIT2";
            Chinese = "2#池体积(m3)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    class PIT3 : Data
    {
        public PIT3()
        {
            Name = "PIT3";
            Chinese = "3#池体积(m3)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 2.0;
        }
    }

    class PIT4 : Data
    {
        public PIT4()
        {
            Name = "PIT4";
            Chinese = "4#池体积(m3)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 5.0;
        }
    }

    class PIT5 : Data
    {
        public PIT5()
        {
            Name = "PIT5";
            Chinese = "5#池体积(m3)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 5.0;
        }
    }

    class PIT6 : Data
    {
        public PIT6()
        {
            Name = "PIT6";
            Chinese = "6#池体积(m3)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 5.0;
        }
    }

    class PIT7 : Data
    {
        public PIT7()
        {
            Name = "PIT7";
            Chinese = "7#池体积(m3)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 5.0;
        }
    }


    class C1 : Data
    {
        public C1()
        {
            Name = "C1";
            Chinese = "C1(%)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 5.0;
        }
    }

    class C2 : Data
    {
        public C2()
        {
            Name = "C2";
            Chinese = "C2(%)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 5.0;
        }
    }

    class CO2 : Data
    {
        public CO2()
        {
            Name = "CO2";
            Chinese = "CO2(%)";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.5;
            Max = DData.Max() * 5.0;
        }
    }

    class TVOLACT : Data 
    {
        public TVOLACT()
        {
            Name = "TVOLACT";
            Chinese = "总池体积";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.8;
            Max = DData.Max() * 1.2;
        }
    }

    class MFIA : Data
    {
        public MFIA()
        {
            Name = "MFIA";
            Chinese = "MFIA";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.8;
            Max = DData.Max() * 1.2;
        }
    }

    class MFOA : Data
    {
        public MFOA()
        {
            Name = "MFOA";
            Chinese = "MFOA";
        }

        public override void setSpan()
        {
            Min = DData.Min() * 0.8;
            Max = DData.Max() * 1.2;
        }
    }

    public class TTIME : Data
    {
        public TTIME()
        {
            Name = "TTIME";
            Chinese = "TTIME";
        }
    }

    public class TDATE : Data
    {
        public TDATE()
        {
            Name = "TDATE";
            Chinese = "TDATE";
        }
    }

    public class MTOA : Data
    {
        public MTOA()
        {
            Name = "MTOA";
            Chinese = "MTOA";
        }
    }

    public class MTIA : Data
    {
        public MTIA()
        {
            Name = "MTIA";
            Chinese = "MTIA";
        }
    }

    public class DXC : Data
    {
        public DXC()
        {
            Name = "DXC";
            Chinese = "DXC";
        }
    }

    #endregion
}
