using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;



namespace MonthlyPaySlipMaker
{

    

    public partial class Form1 : Form
    {
        private const string FILTER = "csv File|*.csv|All Files|*.*";
        public string openFilePerson = "";
        Person employee;


        public Form1()
        {
            InitializeComponent();
            MinimumSize = MaximumSize = Size;
            textBoxPeriodEnd.Text = DateTime.Today.ToString("dd/MM/yyy");
        }

        private void buttonLoadPerson_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = FILTER;
            //string line = "";
            //string[] csvArray;
            //string name = "";
            //decimal payRate = 0;
            //string taxCode = "";
            //string type = "";
            //decimal yearToDate = 0;
            //decimal holidayHrs = 0;
            //string bankNumber = "";
            //int row = 0;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFilePerson = openFileDialog1.FileName;
                employee = new Person();

                employee.readPeriodData(openFilePerson);

                textBoxPerson.Text = employee.Name;
                buttonCreatePaySlip.Enabled = true;


            }

        }

        private void buttonCreatePaySlip_Click(object sender, EventArgs e)
        {
            decimal hoursWorked;
            decimal holidayHoursUsed;
            DateTime periodEnded;

            if (decimal.TryParse(textBoxHours.Text, out hoursWorked) && hoursWorked >= 0)
            {
                if ((decimal.TryParse(textBoxHolidayHoursUsed.Text, out holidayHoursUsed) && holidayHoursUsed >= 0) || textBoxHolidayHoursUsed.Text == "")
                {
                    if(textBoxHolidayHoursUsed.Text == "")
                    {
                        holidayHoursUsed = 0;
                    }
                   

                    if (DateTime.TryParse(textBoxPeriodEnd.Text, out periodEnded))
                    {
                        if (periodEnded.DayOfWeek == DayOfWeek.Friday)
                        {
                            employee.AddPeriodData(periodEnded, hoursWorked, hoursWorked * Convert.ToDecimal(employee.PayRate), holidayHoursUsed, 0, 0);
                            //MessageBox.Show("File Wrote");
                            employee.UpdateNetPay();
                            employee.UpdateHolidayHrsGained();
                            buttonCreatePaySlip.Enabled = false;


                        }
                        else
                        {
                            MessageBox.Show("Period has to end on a Friday(includes hours worked on that Friday)");
                            return;
                        }

                    }

                    else
                    {
                        MessageBox.Show("Invalid Date entered");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid holiday Hours");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid hours entered");
                return;
            }

            saveFileDialog1.Filter = "Excel File |*.xlsx";
            saveFileDialog1.FileName = employee.MakeFileName;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Updates the form
                employee.updateTotalPay(hoursWorked, holidayHoursUsed);
                employee.updateHolidayHoursBalance(holidayHoursUsed);
                employee.writeFileAllData(openFilePerson);
                textBoxPerson.Text = "";
                textBoxHours.Text = "";
                textBoxPeriodEnd.Text = DateTime.Today.ToString("dd/MM/yyy");
                textBoxHolidayHoursUsed.Text = "";
                //save file using motel save method
                CreateSpreadsheetWorkbook(saveFileDialog1.FileName);
            }


        }
        public void CreateSpreadsheetWorkbook(string fileName)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                workbookPart.Workbook.Save();

                // Close the document.
                document.Close();

                CreatePaySlip(fileName);
                //InsertText(fileName, "Inserted Text");
            }
        }




        // Given a document name and text, 
        // inserts a new work sheet and writes the text to cell "A1" of the new worksheet.

        public void CreatePaySlip(string docName)
        {
            // Open the document for editing.
            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(docName, true))
            {
                // Get the SharedStringTablePart. If it does not exist, create a new one.
                SharedStringTablePart shareStringPart;
                if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                {
                    shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                }
                else
                {
                    shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
                }


                // Insert a new worksheet.

                WorksheetPart worksheetPart = InsertWorksheet(spreadSheet.WorkbookPart);

                insertDataIntoCell(shareStringPart, worksheetPart, employee.CompanyName, "A", 1);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.EmployerAddress, "A", 2);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.EmployerSuburb, "A", 3);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.EmployerCity, "A", 4);
                insertDataIntoCell(shareStringPart, worksheetPart, Convert.ToString(employee.EmployerPostCode), "B", 4);

                insertDataIntoCell(shareStringPart, worksheetPart, "Pay Slip", "A", 6);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.Name, "A", 7);

                insertDataIntoCell(shareStringPart, worksheetPart, "Period End:", "A", 9);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.PeriodEnd, "B", 9);
                insertDataIntoCell(shareStringPart, worksheetPart, "Tax Code", "D", 9);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.TaxCode, "E", 9);

                insertDataIntoCell(shareStringPart, worksheetPart, "Payments", "A", 10);
                insertDataIntoCell(shareStringPart, worksheetPart, "Type", "B", 10);
                insertDataIntoCell(shareStringPart, worksheetPart, "Rate", "C", 10);
                insertDataIntoCell(shareStringPart, worksheetPart, "Hours", "D", 10);
                insertDataIntoCell(shareStringPart, worksheetPart, "Amount", "E", 10);

                insertDataIntoCell(shareStringPart, worksheetPart, "Ordinary Pay", "A", 11);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.Type, "B", 11);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.PayRate.ToString("c"), "C", 11);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.HoursForPeriod, "D", 11);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.PayForHoursWorked, "E", 11);

                uint row = 14;
                if(employee.HolidayHoursUsed > 0)
                {
                    insertDataIntoCell(shareStringPart, worksheetPart, "Holiday Pay:", "A", 12);
                    insertDataIntoCell(shareStringPart, worksheetPart, employee.PayRate.ToString("c"), "C", 12);
                    insertDataIntoCell(shareStringPart, worksheetPart, employee.HolidayHoursUsed.ToString(), "D", 12);
                    insertDataIntoCell(shareStringPart, worksheetPart, employee.HolidayPay, "E", 12);
                    insertDataIntoCell(shareStringPart, worksheetPart, employee.TotalGrossPay.ToString("c"), "E", 13);
                    row += 2;
                }
                

                insertDataIntoCell(shareStringPart, worksheetPart, "Summary", "A", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "Normal Pay & Leave:", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.TotalGrossPay.ToString("c"), "B", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "KiwiSaver Deductions:", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, (employee.KiwiSaverDeductions * -1).ToString("c"), "B", row);
                insertDataIntoCell(shareStringPart, worksheetPart, " @" + employee.KiwiSaverContrabutionPercent, "C", row);

              
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "P.A.Y.E:", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, (employee.PAYE * -1).ToString("c"), "B", row);

                insertDataIntoCell(shareStringPart, worksheetPart, "ACC Levy:", "C", row);
                insertDataIntoCell(shareStringPart, worksheetPart, (employee.ACCLevy * -1).ToString("c"), "D", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "PAYE (including ACC earner’s levy):", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, (employee.PAYEandACC * -1).ToString("c"), "B", row);

                //row += 1;
                //insertDataIntoCell(shareStringPart, worksheetPart, "PAYE (including ACC earner’s levy):", "A", row);
                //insertDataIntoCell(shareStringPart, worksheetPart, (employee.PAYE * -1).ToString("c"), "B", row);



                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "Student loan deductions:", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, (employee.StudentLoadRepayment  * -1).ToString("c"), "B", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "Total deductions:", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, ((employee.KiwiSaverDeductions + employee.PAYEandACC + employee.StudentLoadRepayment) * -1).ToString("c"), "B", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "Net Pay:", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, (employee.TotalGrossPay - employee.PAYEandACC - employee.KiwiSaverDeductions - employee.StudentLoadRepayment).ToString("c"), "B", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "Year To Date Pay:", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.YearToDatePay, "B", row);

                row += 2;
                insertDataIntoCell(shareStringPart, worksheetPart, "Kiwisaver employer contributions", "A", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "Gross employer contributions: ", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.EmployerKiwiSaverDeductions.ToString("c"), "B", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "Employer super annuation contribution tax (ESCT)", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, (employee.EmployerESCT * -1).ToString("c"), "B", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "Net employer contributions (payable to IR)", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, (employee.EmployerKiwiSaverDeductions - employee.EmployerESCT).ToString("c"), "B", row);
         



                row += 2;
                insertDataIntoCell(shareStringPart, worksheetPart, "Holiday Hours Balance:", "A", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "Holiday Hours used:", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, employee.HolidayHoursUsed.ToString(), "B", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "Holiday Hours gained (based on 4/52 for hours/week in this period):", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, Math.Round(employee.HolidayHoursGained, 2).ToString(), "B", row);
                row += 1;
                insertDataIntoCell(shareStringPart, worksheetPart, "Total Holiday hours:", "A", row);
                insertDataIntoCell(shareStringPart, worksheetPart, Math.Round(employee.HolidayHours, 2).ToString(), "B", row);




              //// Save the new worksheet.
              worksheetPart.Worksheet.Save();
            }
        }
        private static void insertDataIntoCell(SharedStringTablePart shareStringPart, WorksheetPart worksheetPart, string data, string col, uint row)
        {
            // Insert the text into the SharedStringTablePart.
            int index = InsertSharedStringItem(data, shareStringPart);
            // Insert cell A1 into the new worksheet.
            Cell cell = InsertCellInWorksheet(col, row, worksheetPart);
            cell.CellValue = new CellValue(index.ToString());
            //int dataInt;
            //if (Int32.TryParse(data, out dataInt))
            //{
            //    cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            //}
            //else
            //{
                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            //}


            worksheetPart.Worksheet.Save();

        }



        // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
        // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        // Given a WorkbookPart, inserts a new worksheet.
        private static WorksheetPart InsertWorksheet(WorkbookPart workbookPart)
        {
            // Add a new worksheet part to the workbook.
            WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());
            newWorksheetPart.Worksheet.Save();


            Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
            string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

            // Get a unique ID for the new sheet.
            uint sheetId = 1;


            string sheetName = "Payslip";

            // Append the new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);
            workbookPart.Workbook.Save();

            return newWorksheetPart;
        }

        // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
        // If the cell already exists, returns it. 
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }
    }
}


















