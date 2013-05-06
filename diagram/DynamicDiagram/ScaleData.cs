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
        public int[] pos;
        public Time[] datetime;
        public String[] depth;
        private Time _firstTime;

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
            datetime[1] = now.subtractMinutes(DisplayInterval / 3);
            datetime[0] = now.subtractMinutes(DisplayInterval / 3 * 2);
        }
        #endregion
    }
}
