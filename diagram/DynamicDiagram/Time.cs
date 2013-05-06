using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diagram.DynamicDiagram
{
    public class Time
    {
        #region Constructor
        public Time(int year, int month, int day, int hour, int minute, int second)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        public Time(String date, String time)
        {
            String[] datestr = date.Split('/');
            String[] timestr = time.Split(':');

            this.year = Convert.ToInt32(datestr[0]);
            this.month = Convert.ToInt32(datestr[1]);
            this.day = Convert.ToInt32(datestr[2]);
            this.hour = Convert.ToInt32(timestr[0]);
            this.minute = Convert.ToInt32(timestr[1]);
            this.second = Convert.ToInt32(timestr[2]);
        }
        #endregion

        #region Properties
        public const int INF = 999999999;
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public int second;

        int[] MONTH = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        #endregion

        #region Methods

        public int getdiff(Time time)
        {
            int diff = 0;

            if (time.day != day)
            {
                diff = hour * 60 * 60 - (24 + time.hour) * 60 * 60 + minute * 60 - time.minute * 60 + second - time.second;
            }
            else
            {
                diff = day * 24 * 60 * 60 - time.day * 24 * 60 * 60 + hour * 60 * 60 - time.hour * 60 * 60 + minute * 60 - time.minute * 60 + second - time.second;
            }

            return diff;
        }

        public int get_diff_by_minute(Time time)
        {
            int diff = 0;

            if (day != time.day)
            {
                diff = (24 + hour) * 60 - time.hour * 60 + minute - time.minute;
            }
            else
            {
                diff = day * 24 * 60 - time.day * 24 * 60 + hour * 60 - time.hour * 60 + minute - time.minute;
            }

            return diff;

        }

        public String toString()
        {
            return year + "/" + month + "/" + day + "/r/n   " + hour + ":" + minute + ":" + second;
        }

        public Time subtractHours(int Interval)
        {
            Time past = new Time(this.year, this.month, this.day, this.hour, this.minute, this.second);

            if (past.hour - Interval < 0)
            {
                if (past.day == 1)
                {
                    if (past.month == 1)
                    {
                        past.year -= 1;
                        past.month = 12;
                        past.day = 31;
                        past.hour = 24 + past.hour - Interval;
                    }
                    else
                    {
                        past.month -= 1;
                        past.day = MONTH[past.month];
                        past.hour = 24 + past.hour - Interval;
                    }
                }
                else
                {
                    past.day -= 1;
                    past.hour = 24 + past.hour - Interval;
                }
            }
            else
            {
                past.hour -= Interval;
            }

            return past;
        }

        public Time subtractMinutes(int Interval)
        {
            Time past = new Time(this.year, this.month, this.day, this.hour, this.minute, this.second);

            int hour = Interval / 60;
            int minu = Interval - hour * 60;

            past.hour -= hour;
            past.minute -= minu;

            if (past.minute < 0)
            {
                past.hour -= 1;
                past.minute += 60;
            }

            if (past.hour < 0)
            {
                past.day -= 1;
                past.hour += 24;
            }

            if (past.day < 1)
            {
                past.month -= 1;
                past.day += MONTH[past.month];
            }

            if (past.month < 1)
            {
                past.year -= 1;
                past.month = 12;
            }

            return past;

        }

        public String ToDateString()
        {
            String ms = month >= 10 ? (month).ToString() : ("0" + month);
            string ds = day >= 10 ? (day).ToString() : ("0" + day);
            return year + "/" + ms + "/" + ds;
        }

        public String ToTimeString()
        {
            String hs = hour >= 10 ? (hour).ToString() : ("0" + hour);
            String ms = minute >= 10 ? (minute).ToString() : ("0" + minute);
            string ss = second >= 10 ? (second).ToString() : ("0" + second);

            return hs + ":" + ms + ":" + ss;
        }
        #endregion
    }
}
