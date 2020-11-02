using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyPaySlipMaker
{
    class PeriodData
    {

        private DateTime _periodEnd;
        private decimal _hoursWorked;
        private decimal _payForPeriod;
        private decimal _holidayHoursUsed;
        private decimal _holidayHoursGained;
        private decimal _netPay;


        public PeriodData(DateTime periodEnd, decimal hoursWorked, decimal payForPeriod, decimal holidayHoursUsed, decimal holidayHoursGained, decimal netPay)
        {
            _periodEnd = periodEnd;
            _hoursWorked = hoursWorked;
            _payForPeriod = payForPeriod;
            _holidayHoursUsed = holidayHoursUsed;
            _holidayHoursGained = holidayHoursGained;
            _netPay = netPay;

        }

        public string csvData()
        {
            return _periodEnd.ToString("d/M/yyyy") + "," + _hoursWorked.ToString() + "," + _payForPeriod.ToString() + "," + _holidayHoursUsed +"," + _holidayHoursGained + "," + _netPay;
        }
        public string PeriodEnd
        {
            get { return _periodEnd.Day + " " + _periodEnd.ToString("MMMM") +" " + _periodEnd.Year; }
        }
        public decimal PayForPeriod
        {
            get { return _payForPeriod; }
        }
        public decimal HoursForPeriod
        {
            get { return _hoursWorked; }
        }
        public decimal HolidayHoursUsed
        {
            get { return _holidayHoursUsed; }
        }
        public decimal HolidayHoursGained
        {
            get { return _holidayHoursGained; }
        }
        public decimal NetPay
        {
            get { return _netPay; }
            set { _netPay = value; }
        }

        public void SetNetPay(decimal pay)
        {
            _netPay = pay;
        }

        public void SetHolidayHoursGained(decimal holidayHours)
        {
            _holidayHoursGained = holidayHours;
        }

        public DateTime PeriodEndDateTime
        {
            get { return _periodEnd; }
        }
    }
}
