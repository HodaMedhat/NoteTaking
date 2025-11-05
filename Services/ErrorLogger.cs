
using NoteTaking.ViewModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel;

namespace NoteTaking.Services
{
    public class ErrorLogger
    {
        private readonly string _filePath;

        public ErrorLogger(IWebHostEnvironment env)
        {
            // Set EPPlus license context
            ExcelPackage.License.SetNonCommercialPersonal("<Your Name>");

            // Resolve full path (e.g., wwwroot/ErrorReports/ErrorLog.xlsx)
            var folderPath = Path.Combine(env.WebRootPath, "ErrorReports");
            Directory.CreateDirectory(folderPath); // Make sure folder exists
            _filePath = Path.Combine(folderPath, "ErrorLog.xlsx");
        }

        public void AppendErrors(List<ErrorViewModel> errors)
        {
            var fileInfo = new FileInfo(_filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet;

                // If worksheet doesn't exist, create it and write headers
                if (package.Workbook.Worksheets.Count == 0)
                {
                    worksheet = package.Workbook.Worksheets.Add("Errors");

                    worksheet.Cells[1, 1].Value = "Timestamp";
                    worksheet.Cells[1, 2].Value = "Row Number";
                    worksheet.Cells[1, 3].Value = "Field Name";
                    worksheet.Cells[1, 4].Value = "ErrorMessage";
                    worksheet.Cells[1, 5].Value = "ErrorMessageInner";
                }
                else
                {
                    worksheet = package.Workbook.Worksheets[0];
                }

                // Determine the next row to write to
                int startRow = worksheet.Dimension?.End.Row + 1 ?? 2;

                // Write each error entry
                foreach (var error in errors)
                {
                    worksheet.Cells[startRow, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cells[startRow, 2].Value = startRow-1;
                    worksheet.Cells[startRow, 3].Value = error.FieldName;
                    worksheet.Cells[startRow, 4].Value = error.ErrorMessage;
                    worksheet.Cells[startRow, 5].Value = error.ErrorMessageInner;
                    startRow++;
                }

                // Save the file
                try
                {
                    package.Save();
                    Console.WriteLine("Errors appended successfully to: " + _filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving Excel file: " + ex.Message);
                }
            }
        }

    }
}


