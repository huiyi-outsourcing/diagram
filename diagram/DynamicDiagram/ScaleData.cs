using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diagram.DynamicDiagram
{
    public class ScaleData
    {
        #region Constructor
        public ScaleData()
        {
            pos = new int[3];
            datetime = new Time[3];
            depth = new String[3];
        }
        #endregion

        #region Properties
        private int[] pos;
        private Time[] datetime;
        private String[] depth;
        private Time _firstTime;

        public int[] Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public Time[] Datetime
        {
            get { return datetime; }
            set { datetime = value; }
        }

        public String[] Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        public Time FirstTime
        {
            get { return _firstTime; }
            set { _firstTime = value; }
        }
        #endregion

        #region Methods
        public void getTime(Time now, int DisplayInterval)
        {
            datetime[2] = now;
            datetime[1] = now.SubtractMinutes(DisplayInterval / 3);
            datetime[0] = now.SubtractMinutes(DisplayInterval / 3 * 2);
        }
        #endregion
    }
}
