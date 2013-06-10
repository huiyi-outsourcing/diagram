using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace diagram.DynamicDiagram
{
    public class Time
    {
        #region Constructor
        public Time()
        { 
        
        }

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
        public int Getdiff(Time time)
        {
            int diff = 0;

            if (time.day != day)
            {
                diff = (hour - time.hour + 24) * 3600 + (minute - time.minute) * 60 + second - time.second;
                //diff = hour * 60 * 60 - (24 + time.hour) * 60 * 60 + minute * 60 - time.minute * 60 + second - time.second;
            }
            else
            {
                diff = (day - time.day) * 24 * 60 * 60  + (hour - time.hour) * 60 * 60 + (minute - time.minute) * 60 + second - time.second;
            }

            return diff;
        }

        public int GetDiffByMinute(Time time)
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

        public Time SubtractHours(int Interval)
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

        public Time SubtractMinutes(int Interval)
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

        #region Tests
        [Test]
        public void TestGetDiff()
        {
            // 基础测试
            Time t1 = new Time(2000, 1, 1, 0, 0, 0);
            Time t2 = new Time(2000, 1, 1, 0, 3, 1);
            Assert.AreEqual(181, t2.Getdiff(t1));

            // 当日期变更时的测试
            t1 = new Time(2000, 1, 1, 23, 59, 50);
            t2 = new Time(2000, 1, 2, 0, 1, 20);
            Assert.AreEqual(90, t2.Getdiff(t1));

            // 当月变更时的测试
            t1 = new Time(2000, 1, 31, 23, 58, 40);
            t2 = new Time(2000, 2, 1, 0, 4, 30);
            Assert.AreEqual(350, t2.Getdiff(t1));

            // 当年变更时的测试
            t1 = new Time(2000, 12, 31, 23, 59, 50);
            t2 = new Time(2001, 1, 1, 0, 3, 5);
            Assert.AreEqual(195, t2.Getdiff(t1));
        }

        [Test]
        public void TestGetDiffByMinute()
        {
            Time t1 = new Time(2000, 1, 1, 0, 3, 0);
            Time t2 = new Time(2000, 1, 1, 0, 5, 3);
            Assert.AreEqual(2, t2.GetDiffByMinute(t1));

            t1 = new Time(2000, 1, 1, 23, 59, 3);
            t2 = new Time(2000, 1, 2, 1, 6, 5);
            Assert.AreEqual(67, t2.GetDiffByMinute(t1));
        }

        [Test]
        public void TestSubtractMinutes()
        {
            Time t1 = new Time(2000, 1, 2, 1, 1, 3);
            Time t2 = t1.SubtractMinutes(50);
            Assert.AreEqual(50, t1.GetDiffByMinute(t2));

            t1 = new Time(2000, 1, 2, 0, 1, 3);
            t2 = t1.SubtractMinutes(50);
            Assert.AreEqual(50, t1.GetDiffByMinute(t2));
        }

        [Test]
        public void TestSubtractHours()
        {
            Time t1 = new Time(2000, 1, 2, 1, 1, 4);
            Time t2 = t1.SubtractHours(2);
            Assert.AreEqual(23, t2.hour);

            t1 = new Time(2000, 1, 2, 23, 3, 3);
            t2 = t1.SubtractHours(24);
            Assert.AreEqual(23, t2.hour);
            Assert.AreEqual(1, t2.day);
        }
        #endregion
    }
}
