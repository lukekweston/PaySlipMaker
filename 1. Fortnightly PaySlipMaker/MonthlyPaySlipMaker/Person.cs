using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace MonthlyPaySlipMaker
{
    class Person
    {
        private string _name;
        private decimal _payRate;
        private string _taxCode;
        private string _type;
        private decimal _yearToDatePay;
        private decimal _holidayHours;
        private string _bankNumber;
        private List<PeriodData> _periodData;
        private int _kiwisaver;
        private int _employerKiwisaver;
        private string _companyName;
        private string _employerAddress;
        private string _employerSuburb;
        private string _employerCity;
        private int _employerPostcode;
        private decimal _accLevy;

        public Person()
        {
            _name = "";
            _payRate = 0;
            _taxCode = "";
            _type = "";
            _accLevy = 0;
            _yearToDatePay = 0;
            _kiwisaver = 0;
            _bankNumber = "";
            _periodData = new List<PeriodData>();
            _companyName = "";
            _employerAddress = "";
            _employerSuburb = "";
            _employerCity = "";
            _employerPostcode = 0;
        }


        public Person(string name, decimal payRate, string taxCode, string type, decimal yearToDatePay, decimal holidayHours, string bankNumber)
        {
            _name = name;
            _payRate = payRate;
            _taxCode = taxCode;
            _type = type;
            _yearToDatePay = yearToDatePay;
            _holidayHours = holidayHours;
            _bankNumber = bankNumber;
            _periodData = new List<PeriodData>();
        }

        public void updateTotalPay(decimal hoursThisPeriod, decimal holidayHoursUsed)
        {
            _yearToDatePay += _payRate * (hoursThisPeriod + holidayHoursUsed);
        }

        public void updateHolidayHoursBalance(decimal holidayHoursUsed)
        {
            _holidayHours += HolidayHoursGained - holidayHoursUsed;



        }


        /// <summary>
        /// Get all person unique data
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }
        public decimal PayRate { get { return _payRate; } set { _payRate = Convert.ToDecimal(value); } }
        public string TaxCode { get { return _taxCode; } set { _taxCode = value; } }
        public string Type { get { return _type; } set { _type = value; } }
        public string YearToDatePay { get { return _yearToDatePay.ToString("c"); } set { _yearToDatePay = Convert.ToDecimal(value); } }
        public decimal HolidayHours { get { return _holidayHours; } set { _holidayHours = Convert.ToDecimal(value); } }
        public string KiwiSaverContrabutionPercent { get { return Convert.ToString(_kiwisaver) + " %"; } set { _kiwisaver = Convert.ToInt32(value); } }
        public string BankNumber { get { return _bankNumber; } set { _bankNumber = value; } }
        public string CompanyName { get { return _companyName; } set { _companyName = value; } }
        public string EmployerAddress { get { return _employerAddress; } set { _employerAddress = value; } }
        public string EmployerSuburb { get { return _employerSuburb; } set { _employerSuburb = value; } }
        public string EmployerCity { get { return _employerCity; } set { _employerCity = value; } }
        public int EmployerPostCode { get { return _employerPostcode; } set { _employerPostcode = value; } }
        public string PeriodEnd { get { return _periodData[(_periodData.Count) - 1].PeriodEnd; } }
        public string HoursForPeriod { get { return _periodData[(_periodData.Count) - 1].HoursForPeriod.ToString(); } }
        public string PayForHoursWorked { get { return _periodData[(_periodData.Count) - 1].PayForPeriod.ToString("c"); } }
        public decimal HolidayHoursUsed { get { return _periodData[(_periodData.Count) - 1].HolidayHoursUsed; } }
        public string HolidayPay { get { return (HolidayHoursUsed * _payRate).ToString("c"); } }
        public decimal TotalGrossPay { get { return ((HolidayHoursUsed + Convert.ToDecimal(HoursForPeriod)) * _payRate); } }
        public decimal KiwiSaverDeductions { get { return (TotalGrossPay * (Convert.ToDecimal(_kiwisaver) / 100)); } }
        public decimal EmployerKiwiSaverDeductions { get { return (TotalGrossPay * (Convert.ToDecimal(_employerKiwisaver) / 100)); } }
        public decimal EmployerESCT { get { return EmployerKiwiSaverDeductions * (0.175m); } }
        public decimal PAYE
        {
            get
            {
                if (_taxCode == "S")
                {
                    return (TotalGrossPay * 0.1889m) - 0.005m;
                }

                //Old way -> works out tax when year threshold gets reached

                //else if (_taxCode == "M" || _taxCode == "MSL")
                //{
                //    Console.WriteLine(YearToDatePay);
                //    if (_yearToDatePay < 14000)
                //    {

                //        Console.WriteLine(TotalGrossPay * 0.105m);
                //        return (TotalGrossPay * 0.105m);

                //    }
                //    else if (_yearToDatePay < 48000)
                //    {
                //        Console.WriteLine(TotalGrossPay * 0.175m);
                //        return (((TotalGrossPay - 14000) * 0.175m) + (14000 * 0.105m));
                //    }
                //    else if (_yearToDatePay < 70000)
                //    {
                //        return (((TotalGrossPay - 48000) * 0.3m) + (34000 * 0.175m) + (14000 * 0.105m));
                //    }
                //    else
                //    {
                //        return (((TotalGrossPay - 70000) * 0.33m) + (22000 * 0.3m) + (34000 * 0.175m) + (14000 * 0.105m));
                //    }

                //}

                //New way - assumes each pay will be conisitant for the year

                else if (_taxCode == "M" || _taxCode == "MSL")
                {
                    decimal payPerYear = (PayRate * decimal.Parse(HoursForPeriod)) * 26;

                    if (payPerYear < 14000)
                    {
                        return (payPerYear * 0.105m) / 26;
                    }
                    else if (payPerYear < 48000)
                    {
                        return (((payPerYear - 14000) * 0.175m) + (14000 * 0.105m)) / 26;
                    }

                    else if (payPerYear < 70000)
                    {
                        return (((payPerYear - 48000) * 0.3m) + (34000 * 0.175m) + (14000 * 0.105m)) / 26;
                    }
                    else
                    {
                        return (((payPerYear - 70000) * 0.33m) + (22000 * 0.3m) + (34000 * 0.175m) + (14000 * 0.105m)) / 26;
                    }
                }

                //Hard coded PAYE % from https://www.ird.govt.nz/tasks/work-out-paye-deductions-from-salary-or-wages calculator
                else if (_taxCode =="ME SL")
                {
                    return (PayRate * (decimal.Parse(HoursForPeriod) + HolidayHoursUsed) * 0.1173m);
                   
                }

                return 10000000000000000000;
            }
        }
        public decimal HolidayHoursGained
        {
            get
            {
                //https://support.flexitime.co.nz/hc/en-us/articles/201818424-How-do-I-calculate-initial-Annual-Leave-Accrued-in-Advance
                decimal weeksWorked = (GetBusinessDays(_periodData[(_periodData.Count) - 2].PeriodEndDateTime, _periodData[(_periodData.Count) - 1].PeriodEndDateTime)) / 5;
                decimal totalDaysWorked = Convert.ToDecimal(((_periodData[(_periodData.Count) - 1].PeriodEndDateTime - _periodData[(_periodData.Count) - 2].PeriodEndDateTime).TotalDays));
                decimal hourspWeek = Convert.ToDecimal(HoursForPeriod) / weeksWorked;
                Console.WriteLine(GetBusinessDays(_periodData[(_periodData.Count) - 2].PeriodEndDateTime, _periodData[(_periodData.Count) - 1].PeriodEndDateTime));
                Console.WriteLine("Weeks worked: " + weeksWorked);
                Console.WriteLine("Days worked: " + totalDaysWorked);
                Console.WriteLine("Hours p week: " + hourspWeek);
                decimal holidayGained = (totalDaysWorked * hourspWeek) / 7 * 4 / 52;
                return holidayGained;
                //decimal holidayHoursPHourOfWork = weeksWorked / (hourspWeek * 52);
                //return Convert.ToDecimal(HoursForPeriod) * holidayHoursPHourOfWork;
            }
        }

        public decimal ACCLevy
        {
            get
            {
                return (PayRate * decimal.Parse(HoursForPeriod)) * _accLevy;
            }
        }

        public decimal StudentLoadRepayment
        {
            get
            {
                if (!_taxCode.Contains("SL"))
                {
                    return 0.0m;
                }
           // https://www.govt.nz/browse/education/tertiary-education/paying-back-your-student-loan/

                else if ((PayRate * decimal.Parse(HoursForPeriod)) > ((20020 / 52) * 2)){

                    return (((PayRate * decimal.Parse(HoursForPeriod))) - 770) * 0.12m;

                }
                else
                {
                    return 0.0m;
                }
            }
        }


        public decimal PAYEandACC
        {
            get
            {
                return this.ACCLevy + this.PAYE;
            }
        }

        public static decimal GetBusinessDays(DateTime startD, DateTime endD)
        {
            startD = startD.AddDays(1);
            double calcBusinessDays =
                1 + ((endD - startD).TotalDays * 5 -
                (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            return Convert.ToDecimal(calcBusinessDays);
        }




        /// <summary>
        /// Add new period data to the period list
        /// </summary>
        /// <param name="periodDate"></param>
        /// <param name="hoursWorked"></param>
        /// <param name="payForPeriod"></param>
        /// <param name="holidayHoursUsed"></param>
        public void AddPeriodData(DateTime periodDate, decimal hoursWorked, decimal payForPeriod, decimal holidayHoursUsed, decimal holidayHoursGained, decimal netPay)
        {
            _periodData.Add(new PeriodData(periodDate, hoursWorked, payForPeriod, holidayHoursUsed, holidayHoursGained, netPay));
        }

        public void readPeriodData(string fileName)
        {
            string line = "";
            string[] csvArray;
            bool atPeriodData = false;
            int row = 0;

            StreamReader reader = new StreamReader(fileName);
            while (!reader.EndOfStream)
            {
                row++;
                line = reader.ReadLine();
                csvArray = line.Split(',');

                if (atPeriodData)
                {
                    try
                    {
                        AddPeriodData(Convert.ToDateTime(csvArray[0]), Convert.ToDecimal(csvArray[1]), Convert.ToDecimal(csvArray[2]), Convert.ToDecimal(csvArray[3]), Convert.ToDecimal(csvArray[4]), Convert.ToDecimal(csvArray[5]));
                    }
                    catch
                    {
                        MessageBox.Show("Invalid data on row " + row);
                        reader.Close();
                        return;
                    }
                }
                else
                {
                    if (row == 1 && csvArray[0] == "Name:")
                    {
                        _name = csvArray[1];
                    }
                    else if (row == 2 && csvArray[0] == "Pay Rate:")
                    {
                        _payRate = Convert.ToDecimal(csvArray[1]);
                    }
                    else if (row == 3 && csvArray[0] == "Tax Code:")
                    {
                        _taxCode = csvArray[1];
                    }
                    else if (row == 4 && csvArray[0] == "Type:")
                    {
                        _type = csvArray[1];
                    }
                    else if (row == 5 && csvArray[0] == "ACC Levy:")
                    {
                        _accLevy = Convert.ToDecimal(csvArray[1]);
                    }
                    else if (row == 6 && csvArray[0] == "Year to date pay:")
                    {
                        _yearToDatePay = Convert.ToDecimal(csvArray[1]);
                    }
                    else if (row == 7 && csvArray[0] == "Holiday Hours:")
                    {
                        _holidayHours = Convert.ToDecimal(csvArray[1]);
                    }
                    else if (row == 8 && csvArray[0] == "Kiwisaver deduction %(3% 5% or 8%):")
                    {
                        _kiwisaver = Convert.ToInt32(csvArray[1]);
                    }
                    else if ( row == 9 && csvArray[0] == "Employer KiwiSaver:")
                    {
                        _employerKiwisaver = Convert.ToInt32(csvArray[1]);
                    }

                    else if (row == 10 && csvArray[0] == "Bank Number:")
                    {
                        _bankNumber = csvArray[1];
                    }
                    else if (row == 11 && csvArray[0] == "Company Name:")
                    {
                        _companyName = csvArray[1];
                    }
                    else if (row == 12 && csvArray[0] == "Employer Address:")
                    {
                        _employerAddress = csvArray[1];
                    }
                    else if (row == 13 && csvArray[0] == "Employer Suburb:")
                    {
                        _employerSuburb = csvArray[1];
                    }
                    else if (row == 14 && csvArray[0] == "Employer City:")
                    {
                        _employerCity = csvArray[1];
                    }
                    else if (row == 15 && csvArray[0] == "Employer Postcode:")
                    {
                        _employerPostcode = Convert.ToInt32(csvArray[1]);
                    }


                    else if (csvArray[0] == "Period Ended")
                    {
                        atPeriodData = true;
                    }
                    else if (row != 16 && row != 17)
                    {
                        MessageBox.Show("Invalid data on row " + row +  " " + csvArray[0] + csvArray [1] + csvArray[2]);
                        reader.Close();
                        return;
                    }

                }
            }
            reader.Close();
        }

        public string MakeFileName
        {
            get { return _name + " " + _periodData[_periodData.Count - 1].PeriodEnd + " Payslip"; }
        }

        public void writeFileAllData(string fileName)
        {
            StreamWriter writer = new StreamWriter(fileName);
            writer.WriteLine("Name:," + _name);
            writer.WriteLine("Pay Rate:," + _payRate);
            writer.WriteLine("Tax Code:," + _taxCode);
            writer.WriteLine("Type:," + _type);
            writer.WriteLine("ACC Levy:," + _accLevy);           
            writer.WriteLine("Year to date pay:," + _yearToDatePay);
            writer.WriteLine("Holiday Hours:," + _holidayHours);
            writer.WriteLine("Kiwisaver deduction %(3% 5% or 8%):," + _kiwisaver);
            writer.WriteLine("Employer KiwiSaver:," + _employerKiwisaver);
            writer.WriteLine("Bank Number:," + _bankNumber);
            writer.WriteLine("Company Name:," + _companyName);
            writer.WriteLine("Employer Address:," + _employerAddress);
            writer.WriteLine("Employer Suburb:," + _employerSuburb);
            writer.WriteLine("Employer City:," + _employerCity);
            writer.WriteLine("Employer Postcode:," + _employerPostcode);


            writer.WriteLine("");
            writer.WriteLine("Past Payslips");
            writer.WriteLine("Period Ended,Hours Worked,Pay For Period, Holiday Hours used, Holiday Hours Gained, Net Pay");
            foreach (PeriodData pD in _periodData)
            {
                writer.WriteLine(pD.csvData());
            }

            writer.Close();
        }

        public void UpdateNetPay()
        {
            _periodData[_periodData.Count() - 1].SetNetPay((TotalGrossPay - PAYE - KiwiSaverDeductions));
        }

        public void UpdateHolidayHrsGained()
        {
            _periodData[_periodData.Count() - 1].SetHolidayHoursGained(HolidayHoursGained);
        }


    }

}
