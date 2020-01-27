using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Modules;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ProductManager.WebHelpers
{
    public class DataExport : AbpModule, IDataExport, ISingletonDependency
    {
        protected List<string> Headers = new List<string>();
        protected List<string> Type = new List<string>();
        protected IWorkbook Workbook;
        protected ISheet Sheet;

        public async Task<byte[]> ExportExcel<T>
            (IReadOnlyList<T> exportData, string sheetName)
        {
            Workbook = new XSSFWorkbook();
            Sheet = Workbook.CreateSheet(sheetName);

            var headerStyle = Workbook.CreateCellStyle();
            var headerFont = Workbook.CreateFont();
            headerFont.IsBold = true;
            headerStyle.SetFont(headerFont);

            WriteData(exportData);

            var header = Sheet.CreateRow(0);
            for (var i = 0; i < Headers.Count; i++)
            {
                var cell = header.CreateCell(i);
                cell.SetCellValue(Headers[i]);
                cell.CellStyle = headerStyle;
            }

            //for (var i = 0; i < Headers.Count; i++)
            //{
            //    Sheet.AutoSizeColumn(i);
            //}

            await using var memoryStream = new MemoryStream();
            Workbook.Write(memoryStream);

            return memoryStream.ToArray();
        }

        private void WriteData<T>(IEnumerable<T> exportData)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));

            var table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                Type.Add(type.Name);
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                                             prop.PropertyType);
                var name = Regex.Replace(prop.Name, "([A-Z])", " $1").Trim(); //space separated 
                //name by caps for header
                Headers.Add(name);
            }

            foreach (var item in exportData)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            for (var i = 0; i < table.Rows.Count; i++)
            {
                var sheetRow = Sheet.CreateRow(i + 1);
                for (var j = 0; j < table.Columns.Count; j++)
                {
                    var row1 = sheetRow.CreateCell(j);

                    var type = Type[j];
                    var currentCellValue = table.Rows[i][j];

                    if (currentCellValue != null &&
                        !string.IsNullOrEmpty(Convert.ToString(currentCellValue)))
                    {
                        switch (type)
                        {
                            case "Int32":
                                row1.SetCellValue(Convert.ToInt32(currentCellValue));
                                break;
                            case "Double":
                            case "Decimal":
                                row1.SetCellValue(Convert.ToDouble(currentCellValue));
                                break;
                            default:
                                row1.SetCellValue(Convert.ToString(currentCellValue));
                                break;
                        }
                    }
                    else
                    {
                        row1.SetCellValue(string.Empty);
                    }
                }
            }
        }
    }
}