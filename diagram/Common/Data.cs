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
        public String _name;
        public List<int> _defaultColumnPos;
        public double _min;
        public double _max;
        public List<double> _data;
        public List<String> _StringData;
        public string _Chinese;
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
            _name = "DEPTMEAS";
            _Chinese = "深度(m)";
        }
        public override void setSpan()
        {
            _min = _data.Min();
            _max = _data.Max();
        }
    }

    public class BKHT : Data
    {
        public BKHT()
        {
            _name = "BKHT";
            _Chinese = "钩位(m)";
        }

        public override void setSpan()
        {
            _min = _data.Min();
            _max = _data.Max();
        }
    }

    public class HOOKLOAD : Data
    {
        public HOOKLOAD()
        {
            _name = "HOOKLOAD";
            _Chinese = "悬重(KN)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }


    public class ROP : Data
    {
        public ROP()
        {
            _name = "ROP";
            _Chinese = "钻时(m/min)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }


    public class WOB : Data
    {
        public WOB()
        {
            _name = "WOB";
            _Chinese = "钻压(KN)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }


    public class RPM : Data
    {
        public RPM()
        {
            _name = "RPM";
            _Chinese = "转速(r/min)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }


    public class TORQUE : Data
    {
        public TORQUE()
        {
            _name = "TORQUE";
            _Chinese = "转盘扭矩(KN/m)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    public class SPM1 : Data
    {
        public SPM1()
        {
            _name = "SPM1";
            _Chinese = "泵冲数1(N/min)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    public class SPM2 : Data
    {
        public SPM2()
        {
            _name = "SPM2";
            _Chinese = "泵冲数2(N/min)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    public class SPM3 : Data
    {
        public SPM3()
        {
            _name = "SPM3";
            _Chinese = "泵冲数3(N/min)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    public class STANDPRES : Data
    {
        public STANDPRES()
        {
            _name = "STANDPRES";
            _Chinese = "立管压力(Mpa)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    public class TOTALPIT : Data
    {
        public TOTALPIT()
        {
            _name = "TOTALPIT";
            _Chinese = "总池体积(m3)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    class FLOWIN : Data
    {
        public FLOWIN()
        {
            _name = "FLOWIN";
            _Chinese = "入口流量(L/s)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    class FLOWOUT : Data
    {
        public FLOWOUT()
        {
            _name = "FLOWOUT";
            _Chinese = "出口流量(L/s)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    class WEIGHTIN : Data
    {
        public WEIGHTIN()
        {
            _name = "WEIGHTIN";
            _Chinese = "入口密度(g/cm3)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    class WEIGHTOUT : Data
    {
        public WEIGHTOUT()
        {
            _name = "WEIGHTOUT";
            _Chinese = "出口密度(g/cm3)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    class TEMPIN : Data
    {
        public TEMPIN()
        {
            _name = "TEMPIN";
            _Chinese = "入口温度(°C)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    class TEMPOUT : Data
    {
        public TEMPOUT()
        {
            _name = "TEMPOUT";
            _Chinese = "出口温度(°C)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    class CONDIN : Data
    {
        public CONDIN()
        {
            _name = "CONDIN";
            _Chinese = "入口电导(ps/m)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    class CONDOUT : Data
    {
        public CONDOUT()
        {
            _name = "CONDOUT";
            _Chinese = "出口电导(ps/m)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }


    class PIT1 : Data
    {
        public PIT1()
        {
            _name = "PIT1";
            _Chinese = "1#池体积(m3)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 5.0;
        }
    }

    class PIT2 : Data
    {
        public PIT2()
        {
            _name = "PIT2";
            _Chinese = "2#池体积(m3)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    class PIT3 : Data
    {
        public PIT3()
        {
            _name = "PIT3";
            _Chinese = "3#池体积(m3)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 2.0;
        }
    }

    class PIT4 : Data
    {
        public PIT4()
        {
            _name = "PIT4";
            _Chinese = "4#池体积(m3)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 5.0;
        }
    }

    class PIT5 : Data
    {
        public PIT5()
        {
            _name = "PIT5";
            _Chinese = "5#池体积(m3)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 5.0;
        }
    }

    class PIT6 : Data
    {
        public PIT6()
        {
            _name = "PIT6";
            _Chinese = "6#池体积(m3)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 5.0;
        }
    }

    class PIT7 : Data
    {
        public PIT7()
        {
            _name = "PIT7";
            _Chinese = "7#池体积(m3)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 5.0;
        }
    }


    class C1 : Data
    {
        public C1()
        {
            _name = "C1";
            _Chinese = "C1(%)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 5.0;
        }
    }

    class C2 : Data
    {
        public C2()
        {
            _name = "C2";
            _Chinese = "C2(%)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 5.0;
        }
    }

    class CO2 : Data
    {
        public CO2()
        {
            _name = "CO2";
            _Chinese = "CO2(%)";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.5;
            _max = _data.Max() * 5.0;
        }
    }

    class TVOLACT : Data 
    {
        public TVOLACT()
        {
            _name = "TVOLACT";
            _Chinese = "总池体积";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.8;
            _max = _data.Max() * 1.2;
        }
    }

    class MFIA : Data
    {
        public MFIA()
        {
            _name = "MFIA";
            _Chinese = "MFIA";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.8;
            _max = _data.Max() * 1.2;
        }
    }

    class MFOA : Data
    {
        public MFOA()
        {
            _name = "MFOA";
            _Chinese = "MFOA";
        }

        public override void setSpan()
        {
            _min = _data.Min() * 0.8;
            _max = _data.Max() * 1.2;
        }
    }

    public class TTIME : Data
    {
        public TTIME()
        {
            _name = "TTIME";
            _Chinese = "TTIME";
        }
    }

    public class TDATE : Data
    {
        public TDATE()
        {
            _name = "TDATE";
            _Chinese = "TDATE";
        }
    }

    public class MTOA : Data
    {
        public MTOA()
        {
            _name = "MTOA";
            _Chinese = "MTOA";
        }
    }

    public class MTIA : Data
    {
        public MTIA()
        {
            _name = "MTIA";
            _Chinese = "MTIA";
        }
    }

    public class DXC : Data
    {
        public DXC()
        {
            _name = "DXC";
            _Chinese = "DXC";
        }
    }

    #endregion
}
