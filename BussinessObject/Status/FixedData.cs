using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Status
{
    public static class FixedData
    {
        public static int DATE_TO_PAY = 15;
        public static int DAY_IN_MONTH_TO_PAY = 5;
        public static DateTime GetNextPayDay()
        {
            DateTime current = DateTime.Now;
            if (current.Day < DAY_IN_MONTH_TO_PAY)
            {
                current.AddDays(DAY_IN_MONTH_TO_PAY - current.Day);
                return current;
            }
            else
            {
                return new DateTime(current.Year, current.Month, DAY_IN_MONTH_TO_PAY, 7, 0, 0).AddMonths(1);
            }
        }

        public static DateTime GetNextPayDayFromStartDate(DateTime startDate)
        {
            DateTime current = startDate;
            if (current.Day < DAY_IN_MONTH_TO_PAY)
            {
                current.AddDays(DAY_IN_MONTH_TO_PAY - current.Day);
                return current;
            }
            else
            {
                return new DateTime(current.Year, current.Month, DAY_IN_MONTH_TO_PAY, 7, 0, 0).AddMonths(1);
            }
        }
    }
}
