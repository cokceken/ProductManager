using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManager.WebHelpers
{
    public interface IDataExport
    {
        Task<byte[]> ExportExcel<T>(IReadOnlyList<T> exportData, string sheetName);
    }
}