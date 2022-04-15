﻿using Aspose.Cells;
using System.Drawing;
using System.IO;
using System.Linq;

namespace WebApplicationDbFacadeBS5Template.Services
{
    public class ExcelService
    {
        public byte[] ExportToXlxs(Workbook workbook)
            => Export(workbook, new OoxmlSaveOptions());
        public byte[] ExportToXls(Workbook workbook)
            => Export(workbook, new XlsSaveOptions());
        private byte[] Export(Workbook workbook, SaveOptions saveOptions)
        {
            byte[] fileData;
            using (var ms = new MemoryStream())
            {
                workbook.Save(ms, saveOptions);
                fileData = ms.ToArray();
            }
            return fileData;
        }
        public Workbook ImportToWorkbookSingleSheet(string name, params System.Data.DataTable[] dataTables)
        => ImportToWorkbookSingleSheet(name, 0, 0, dataTables);
        public Workbook ImportToWorkbookSingleSheet(string name, int firstRow, int firstCol, params System.Data.DataTable[] dataTables)
        {
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = name;
            ImportToWorksheet(worksheet, firstRow, firstCol, dataTables);
            //worksheet.Protect(ProtectionType.All);
            return workbook;
        }
        public Workbook ImportToWorkbook(params System.Data.DataTable[] dataTables)
        => ImportToWorkbook(0, 0, dataTables);
        public Workbook ImportToWorkbook(int firstRow, int firstCol, params System.Data.DataTable[] dataTables)
        {
            Workbook workbook = new Workbook();
            foreach (var dt in dataTables)
            {
                Worksheet worksheet = dt == dataTables.First() ? workbook.Worksheets[0] : workbook.Worksheets[workbook.Worksheets.Add()];
                worksheet.Name = dt.TableName;
                ImportToWorksheet(worksheet, firstRow, firstCol, dt, true);
                //worksheet.Protect(ProtectionType.All);
            }
            return workbook;
        }
        private void ImportToWorksheet(Worksheet worksheet, int firstRow, int firstCol, params System.Data.DataTable[] dataTables)
        {
            int nextRow = firstRow;
            foreach(var dt in dataTables)
            {
                nextRow = ImportToWorksheet(worksheet, nextRow, firstCol, dt, dt == dataTables.Last()) + 1;
            }
        }
        private static void ApplyHeaderStyle(Range range)
        {
            Style stl = range.Worksheet.Workbook.CreateStyle();
            stl.Font.IsBold = true;
            //Set the font text color
            stl.Font.Color = Color.White;
            stl.ForegroundColor = Color.CadetBlue; //bg color
            stl.Pattern = BackgroundType.Solid;
            //Create a StyleFlag object.
            StyleFlag flg = new StyleFlag();
            //Make the corresponding attributes ON.
            flg.Font = true;
            flg.CellShading = true;
            range.ApplyStyle(stl, flg);
        }
        private int ImportToWorksheet(Worksheet worksheet, int firstRow, int firstCol, System.Data.DataTable dataTable, bool autoFilter = false)
        {
            worksheet.Cells.ImportData(dataTable, firstRow, firstCol, new ImportTableOptions() { IsFieldNameShown = true });
            worksheet.AutoFitColumns();
            var tableRange = worksheet.Cells.CreateRange(firstRow, firstCol, dataTable.Rows.Count + 1, dataTable.Columns.Count);
            var headerRange = worksheet.Cells.CreateRange(firstRow, firstCol, 1, dataTable.Columns.Count);

            ApplyHeaderStyle(headerRange);
            headerRange.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Medium, Color.Black);

            tableRange.SetOutlineBorder(BorderType.TopBorder, CellBorderType.Thick, Color.Black);
            tableRange.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thick, Color.Black);
            tableRange.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Thick, Color.Black);
            tableRange.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thick, Color.Black);

            
            
            for (int col = firstCol; col < firstCol + dataTable.Columns.Count - 1; col++)
            {
                var colRange = worksheet.Cells.CreateRange(firstRow, col, 1, 1);
                colRange.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Gray);
            }

            for (int row = firstRow + 1; row < firstRow + dataTable.Rows.Count; row++)
            {
                var rowRange = worksheet.Cells.CreateRange(row, firstCol, 1, dataTable.Columns.Count);
                rowRange.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Gray);
            }
            for (int col = firstCol; col < firstCol + dataTable.Columns.Count - 1; col++)
            {
                var colRange = worksheet.Cells.CreateRange(firstRow + 1, col, dataTable.Rows.Count, 1);
                colRange.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Gray);
            }
            if (autoFilter)
            {
                // Creating AutoFilter by giving the cells range of the heading row
                worksheet.AutoFilter.Range = $"{worksheet.Cells[firstRow, firstCol].Name}:{worksheet.Cells[firstRow, firstCol + dataTable.Columns.Count - 1].Name}";
            }

            // Add FormatConditions to the instance of Worksheet
            int idx = worksheet.ConditionalFormattings.Add();

            // Access the newly added FormatConditions via its index
            var conditionCollection = worksheet.ConditionalFormattings[idx];

            // Define a CellsArea on which conditional formatting will be applicable
            // The code creates a CellArea ranging from A1 to I20
            var area = CellArea.CreateCellArea(worksheet.Cells[firstRow+1, firstCol].Name, worksheet.Cells[firstRow + dataTable.Rows.Count, firstCol + dataTable.Columns.Count - 1].Name);

            //Add area to the instance of FormatConditions
            conditionCollection.AddArea(area);

            // Add a condition to the instance of FormatConditions
            // For this case, the condition type is expression, which is based on some formula
            idx = conditionCollection.AddCondition(FormatConditionType.Expression);

            // Access the newly added FormatCondition via its index
            FormatCondition formatCondirion = conditionCollection[idx];

            // Set the formula for the FormatCondition
            // Formula uses the Excel's built-in functions as discussed earlier in this article
            formatCondirion.Formula1 = @"=MOD(ROW(),2)=0";

            // Set the background color and patter for the FormatCondition's Style
            formatCondirion.Style.BackgroundColor = Color.LightGray;
            formatCondirion.Style.Pattern = BackgroundType.Solid;

           
            return firstRow + dataTable.Rows.Count;
        }
    }
}