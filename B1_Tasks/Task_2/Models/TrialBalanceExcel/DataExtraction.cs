using Task_2.TrialBalanceExcel.Entities;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Text.RegularExpressions;

namespace Task_2.TrialBalanceExcel;

public class DataExtraction
{
    private const string _periodDatesPattern = @"(\d{2}.\d{2}.\d{4}).*?(\d{2}.\d{2}.\d{4})";
    private const string _currencyPattern = @"^\s*?в?\s*?(\S+)\s*?$";
    private const string _only4NumbersPattern = @"^\d{4}$";
    private const string _classPattern = @"^\s*КЛАСС\s*(\d+).*?$";
    private const string _classOverallPattern = @"^\s*?ПО КЛАССУ\s*?$";

    private const int _dataStartRow = 8;

    public static TrialBalanceFileData GetDataFromFile(string filepath)
    {
        HSSFWorkbook hssfwb;
        using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
        {
            hssfwb = new HSSFWorkbook(file);
        }

        var sheetName = hssfwb.GetSheetName(hssfwb.ActiveSheetIndex);
        ISheet sheet = hssfwb.GetSheet(sheetName);

        TrialBalanceFileData data = new()
        {
            Classes = new()
        };

        // File name
        var filename = Path.GetFileName(filepath);
        data.FileName = filename;

        // Creation/uploding time
        var created = File.GetCreationTime(filepath);
        data.Created = created;

        // Read meta data
        ReadMeta(sheet, data);

        // Read data
        ReadData(sheet, data);

        return data;
    }

    private static void ReadMeta(ISheet sheet, TrialBalanceFileData data)
    {
        // Organisation name
        var orgNameRow = sheet.GetRow(0);
        var orgNameCell = orgNameRow.GetCell(0);
        data.OrganisationName = orgNameCell.StringCellValue;

        // Period
        var periodRow = sheet.GetRow(2);
        var periodCell = periodRow.GetCell(0);

        var periodMatch = Regex.Match(periodCell.StringCellValue, _periodDatesPattern);
        if (!periodMatch.Success)
        {
            throw new Exception("Invalid trial balance period or wrong format");
        }

        DateOnly periodStart = DateOnly.Parse(periodMatch.Groups[1].Value);
        DateOnly periodEnd = DateOnly.Parse(periodMatch.Groups[2].Value);
        data.PeriodStart = periodStart;
        data.PeriodEnd = periodEnd;

        // Data time and currency
        var dataTimeRow = sheet.GetRow(5);
        var dataTimeCell = dataTimeRow.GetCell(0);
        var currencyCell = dataTimeRow.GetCell(6);

        if (dataTimeCell.DateCellValue is null)
        {
            throw new Exception("Invalid data time");
        }

        DateTime dataTime = (DateTime)dataTimeCell.DateCellValue;
        data.DataTime = dataTime;

        var currencyMatch = Regex.Match(currencyCell.StringCellValue, _currencyPattern);
        if (!currencyMatch.Success)
        {
            throw new Exception("Invalid currency field format");
        }
        data.Currency = currencyMatch.Groups[1].Value;
    }

    private static void ReadData(ISheet sheet, TrialBalanceFileData data)
    {
        TrialBalanceClass? currentClass = null;

        for (int i = _dataStartRow; i < sheet.LastRowNum; i++)
        {
            var currentRow = sheet.GetRow(i);
            var firstCell = currentRow.GetCell(0);

            if (firstCell.CellType == CellType.Numeric)
            {
                var firstCellValue = firstCell.NumericCellValue.ToString();
                if (Regex.IsMatch(firstCellValue, _only4NumbersPattern))
                {
                    ProcessBalanceRecord(currentClass, currentRow, firstCellValue);
                    continue;
                }
            }

            if (firstCell.CellType == CellType.String)
            {
                var firstCellValue = firstCell.StringCellValue;

                if (Regex.IsMatch(firstCellValue, _only4NumbersPattern))
                {
                    ProcessBalanceRecord(currentClass, currentRow, firstCellValue);
                    continue;
                }

                if (Regex.IsMatch(firstCellValue, _classPattern))
                {
                    currentClass = ProcessClassRecord(data, firstCellValue);
                    continue;
                }

                if (Regex.IsMatch(firstCellValue, _classOverallPattern))
                {
                    if (currentClass is null)
                    {
                        throw new Exception("Found end of class group until its start");
                    }
                    data.Classes.Add(currentClass!);
                    currentClass = null;
                    continue;
                }

                continue;
            }
        }
    }

    private static TrialBalanceClass ProcessClassRecord(TrialBalanceFileData data, string firstCellValue)
    {
        var match = Regex.Match(firstCellValue, _classPattern);
        var currentClass = new TrialBalanceClass()
        {
            Name = match.Groups[0].Value,
            Number = match.Groups[1].Value,
            Balances = new()
        };
        return currentClass;
    }

    private static void ProcessBalanceRecord(TrialBalanceClass? currentClass, IRow currentRow, string firstCellValue)
    {
        if (currentClass is null)
        {
            throw new Exception("Found balance record before first record of class");
        }

        var record = new TrialBalanceRecord()
        {
            Number = firstCellValue,
        };

        record.OpeningBalanceActive = (decimal)currentRow.GetCell(1).NumericCellValue;
        record.OpeningBalancePassive = (decimal)currentRow.GetCell(2).NumericCellValue;
        record.TurnoverDebit = (decimal)currentRow.GetCell(3).NumericCellValue;
        record.TurnoverCredit = (decimal)currentRow.GetCell(4).NumericCellValue;
        record.ClosingBalanceActive = (decimal)currentRow.GetCell(5).NumericCellValue;
        record.ClosingBalancePassive = (decimal)currentRow.GetCell(6).NumericCellValue;

        currentClass.Balances.Add(record);
    }
}
