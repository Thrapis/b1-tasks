using Task_2.TrialBalanceExcel.Entities;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Task_2.TrialBalanceExcel;

public class DataPacking
{
    private const string _titleLine1 = "Оборотная ведомость по балансовым счетам";
    private const string _titleLine2Template = "за период с {0} по {1}";
    private const string _titleLine3 = "по банку";
    private const string _currencyLineTemplate = "в {0}";
    private const string _accountColumnTitle = "Б/сч";
    private const string _openingBalanceColumnTitle = "ВХОДЯЩЕЕ САЛЬДО";
    private const string _turnoverBalanceColumnTitle = "ОБОРОТЫ";
    private const string _closingBalanceColumnTitle = "ИСХОДЯЩЕЕ САЛЬДО";
    private const string _activeColumnTitle = "Актив";
    private const string _passiveColumnTitle = "Пассив";
    private const string _debetColumnTitle = "Дебет";
    private const string _creditColumnTitle = "Кредит";
    private const string _classOverallRowTitle = "ПО КЛАССУ";

    private const int _dataStartRow = 8;
    private const int _classRowHeight = 22;

    public static void CreateFileFromData(string filepath, TrialBalanceFileData data)
    {
        IWorkbook workbook = new XSSFWorkbook();
        ISheet sheet = workbook.CreateSheet("Sheet1");

        // Set width for columns
        sheet.SetColumnWidth(0, 12 * 256);
        sheet.SetColumnWidth(1, 22 * 256);
        sheet.SetColumnWidth(2, 22 * 256);
        sheet.SetColumnWidth(3, 22 * 256);
        sheet.SetColumnWidth(4, 22 * 256);
        sheet.SetColumnWidth(5, 22 * 256);
        sheet.SetColumnWidth(6, 22 * 256);

        // Write meta data into sheet
        WriteMeta(workbook, sheet, data);

        // Write data into sheet
        WriteData(workbook, sheet, data);

        // Save file
        using (FileStream fs = File.Create(filepath))
        {
            workbook.Write(fs);
            fs.Close();
        }
    }

    public static void WriteMeta(IWorkbook workbook, ISheet sheet, TrialBalanceFileData data)
    {
        var defaultFont = workbook.CreateFont();
        defaultFont.FontName = "Arial";
        defaultFont.FontHeightInPoints = 10;
        var defaultStyle = workbook.CreateCellStyle();
        defaultStyle.Alignment = HorizontalAlignment.Center;
        defaultStyle.SetFont(defaultFont);

        // Set organisation cell value
        sheet.CreateRow(0).CreateCell(0).SetCellValue(data.OrganisationName);
        sheet.GetRow(0).GetCell(0).CellStyle.SetFont(defaultFont);

        // Set data for title lines
        sheet.CreateRow(1).CreateCell(0).SetCellValue(_titleLine1);
        sheet.CreateRow(2).CreateCell(0).SetCellValue(string.Format(_titleLine2Template, data.PeriodStart, data.PeriodEnd));
        sheet.CreateRow(3).CreateCell(0).SetCellValue(string.Format(_titleLine3, data.PeriodStart, data.PeriodEnd));
        sheet.CreateRow(4).CreateCell(0);

        // Set styles for title lines
        var titleMainStyle = workbook.CreateCellStyle();
        titleMainStyle.CloneStyleFrom(defaultStyle);
        var titleMainFont = workbook.CreateFont();
        titleMainFont.CloneStyleFrom(defaultFont);
        titleMainFont.FontHeightInPoints = 14;
        titleMainStyle.SetFont(titleMainFont);

        sheet.GetRow(1).GetCell(0).CellStyle = titleMainStyle;

        var titleBoldStyle = workbook.CreateCellStyle();
        titleBoldStyle.CloneStyleFrom(defaultStyle);
        var titleBoldFont = workbook.CreateFont();
        titleBoldFont.CloneStyleFrom(defaultFont);
        titleBoldFont.FontHeightInPoints = 10;
        titleBoldFont.IsBold = true;
        titleBoldStyle.SetFont(titleBoldFont);

        sheet.GetRow(2).GetCell(0).CellStyle = titleBoldStyle;
        sheet.GetRow(3).GetCell(0).CellStyle = titleBoldStyle;
        sheet.GetRow(4).GetCell(0).CellStyle = defaultStyle;

        // Settings for 5th row (Date time and currency)
        var row5 = sheet.CreateRow(5);

        var dataTimeCell = row5.CreateCell(0);
        var dataTimeFormat = workbook.CreateDataFormat().GetFormat("d.m.yyyy h:mm:ss");
        var dataTimeStyle = workbook.CreateCellStyle();
        dataTimeStyle.CloneStyleFrom(defaultStyle);
        dataTimeStyle.DataFormat = dataTimeFormat;
        dataTimeStyle.Alignment = HorizontalAlignment.Left;
        dataTimeCell.CellStyle = dataTimeStyle;
        dataTimeCell.SetCellValue(data.DataTime);

        var currencyCell = row5.CreateCell(6);
        var currencyStyle = workbook.CreateCellStyle();
        currencyStyle.CloneStyleFrom(defaultStyle);
        currencyStyle.Alignment = HorizontalAlignment.Right;
        currencyCell.CellStyle = currencyStyle;
        currencyCell.SetCellValue(string.Format(_currencyLineTemplate, data.Currency));

        // Setting table columns and their styles
        var columnStyle = workbook.CreateCellStyle();
        columnStyle.CloneStyleFrom(defaultStyle);
        columnStyle.VerticalAlignment = VerticalAlignment.Center;
        columnStyle.BorderTop = BorderStyle.Medium;
        columnStyle.BorderRight = BorderStyle.Medium;
        columnStyle.BorderBottom = BorderStyle.Medium;
        columnStyle.BorderLeft = BorderStyle.Medium;
        var columnFont = workbook.CreateFont();
        columnFont.CloneStyleFrom(defaultFont);
        columnFont.IsBold = true;
        columnStyle.SetFont(columnFont);

        for (int i = 6; i <= 7; i++)
        {
            var row = sheet.CreateRow(i);
            for (int j = 0; j <= 6; j++)
            {
                row.CreateCell(j).CellStyle = columnStyle;
            }
        }

        sheet.GetRow(6).GetCell(0).SetCellValue(_accountColumnTitle);
        sheet.GetRow(6).GetCell(1).SetCellValue(_openingBalanceColumnTitle);
        sheet.GetRow(6).GetCell(3).SetCellValue(_turnoverBalanceColumnTitle);
        sheet.GetRow(6).GetCell(5).SetCellValue(_closingBalanceColumnTitle);
        sheet.GetRow(7).GetCell(1).SetCellValue(_activeColumnTitle);
        sheet.GetRow(7).GetCell(2).SetCellValue(_passiveColumnTitle);
        sheet.GetRow(7).GetCell(3).SetCellValue(_debetColumnTitle);
        sheet.GetRow(7).GetCell(4).SetCellValue(_creditColumnTitle);
        sheet.GetRow(7).GetCell(5).SetCellValue(_activeColumnTitle);
        sheet.GetRow(7).GetCell(6).SetCellValue(_passiveColumnTitle);

        // Merge specific cells
        sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 6));
        sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 6));
        sheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 6));
        sheet.AddMergedRegion(new CellRangeAddress(4, 4, 0, 6));
        sheet.AddMergedRegion(new CellRangeAddress(5, 5, 0, 1));
        sheet.AddMergedRegion(new CellRangeAddress(6, 7, 0, 0));
        sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 2));
        sheet.AddMergedRegion(new CellRangeAddress(6, 6, 3, 4));
        sheet.AddMergedRegion(new CellRangeAddress(6, 6, 5, 6));
    }

    public static void WriteData(IWorkbook workbook, ISheet sheet, TrialBalanceFileData data)
    {
        // Set default style and font
        var defaultFont = workbook.CreateFont();
        defaultFont.FontName = "Arial";
        defaultFont.FontHeightInPoints = 10;

        var defaultStyle = workbook.CreateCellStyle();
        defaultStyle.Alignment = HorizontalAlignment.Right;
        defaultStyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.00");

        defaultStyle.SetFont(defaultFont);

        // Set class style and font
        var classFont = workbook.CreateFont();
        classFont.CloneStyleFrom(defaultFont);
        classFont.FontHeightInPoints = 9;
        classFont.IsBold = true;

        var classStyle = workbook.CreateCellStyle();
        classStyle.CloneStyleFrom(defaultStyle);
        classStyle.Alignment = HorizontalAlignment.Center;
        classStyle.VerticalAlignment = VerticalAlignment.Center;
        classStyle.DataFormat = workbook.CreateDataFormat().GetFormat("General");

        classStyle.SetFont(classFont);

        // Set account style and font
        var accountFont = workbook.CreateFont();
        accountFont.CloneStyleFrom(defaultFont);

        var accountStyle = workbook.CreateCellStyle();
        accountStyle.CloneStyleFrom(defaultStyle);
        accountStyle.Alignment = HorizontalAlignment.Left;
        accountStyle.DataFormat = workbook.CreateDataFormat().GetFormat("General");

        accountStyle.SetFont(accountFont);

        // Set bold default style and font
        var boldDefaultFont = workbook.CreateFont();
        boldDefaultFont.CloneStyleFrom(defaultFont);
        boldDefaultFont.IsBold = true;

        var boldDefaultStyle = workbook.CreateCellStyle();
        boldDefaultStyle.CloneStyleFrom(defaultStyle);

        boldDefaultStyle.SetFont(boldDefaultFont);

        // Set bold account style and font
        var boldAccountFont = workbook.CreateFont();
        boldAccountFont.CloneStyleFrom(accountFont);
        boldAccountFont.IsBold = true;

        var boldAccountStyle = workbook.CreateCellStyle();
        boldAccountStyle.CloneStyleFrom(accountStyle);

        boldAccountStyle.SetFont(boldAccountFont);


        // Iterate data to write
        var currentRowId = _dataStartRow;

        foreach (var accClass in data.Classes)
        {
            // Write class row
            var classRow = sheet.CreateRow(currentRowId);
            for (int i = 0; i <= 6; i++)
            {
                var classCell = classRow.CreateCell(i);
                classCell.CellStyle = classStyle;
            }
            sheet.GetRow(currentRowId).GetCell(0).SetCellValue(accClass.Name);
            sheet.GetRow(currentRowId).HeightInPoints = _classRowHeight;
            sheet.AddMergedRegion(new CellRangeAddress(currentRowId, currentRowId, 0, 6));
            currentRowId++;

            // Get class records grouped by first 2 digits of number
            var groups = accClass.GetGroupedBalanceRecords();
            foreach (var group in groups)
            {
                // Write records of group
                foreach (var balanceRecord in group)
                {
                    var balanceRow = sheet.CreateRow(currentRowId);
                    var firstCell = balanceRow.CreateCell(0);
                    firstCell.CellStyle = accountStyle;
                    for (int i = 1; i <= 6; i++)
                    {
                        var balanceCell = balanceRow.CreateCell(i);
                        balanceCell.CellStyle = defaultStyle;
                    }
                    sheet.GetRow(currentRowId).GetCell(0).SetCellValue(int.Parse(balanceRecord.Number));
                    sheet.GetRow(currentRowId).GetCell(1).SetCellValue((double)balanceRecord.OpeningBalanceActive);
                    sheet.GetRow(currentRowId).GetCell(2).SetCellValue((double)balanceRecord.OpeningBalancePassive);
                    sheet.GetRow(currentRowId).GetCell(3).SetCellValue((double)balanceRecord.TurnoverDebit);
                    sheet.GetRow(currentRowId).GetCell(4).SetCellValue((double)balanceRecord.TurnoverCredit);
                    sheet.GetRow(currentRowId).GetCell(5).SetCellValue((double)balanceRecord.ClosingBalanceActive);
                    sheet.GetRow(currentRowId).GetCell(6).SetCellValue((double)balanceRecord.ClosingBalancePassive);
                    currentRowId++;
                }

                // Write overall data over group
                var suboverallRow = sheet.CreateRow(currentRowId);
                var suboverallTitleCell = suboverallRow.CreateCell(0);
                suboverallTitleCell.CellStyle = boldAccountStyle;
                for (int i = 1; i <= 6; i++)
                {
                    var suboverallCell = suboverallRow.CreateCell(i);
                    suboverallCell.CellStyle = boldDefaultStyle;
                }
                sheet.GetRow(currentRowId).GetCell(0).SetCellValue(int.Parse(group.Key));
                sheet.GetRow(currentRowId).GetCell(1).SetCellValue((double)group.Sum(b => b.OpeningBalanceActive));
                sheet.GetRow(currentRowId).GetCell(2).SetCellValue((double)group.Sum(b => b.OpeningBalancePassive));
                sheet.GetRow(currentRowId).GetCell(3).SetCellValue((double)group.Sum(b => b.TurnoverDebit));
                sheet.GetRow(currentRowId).GetCell(4).SetCellValue((double)group.Sum(b => b.TurnoverCredit));
                sheet.GetRow(currentRowId).GetCell(5).SetCellValue((double)group.Sum(b => b.ClosingBalanceActive));
                sheet.GetRow(currentRowId).GetCell(6).SetCellValue((double)group.Sum(b => b.ClosingBalancePassive));
                currentRowId++;
            }

            // Write overall data over class
            var overallRow = sheet.CreateRow(currentRowId);
            var overalTitleCell = overallRow.CreateCell(0);
            overalTitleCell.CellStyle = boldAccountStyle;
            for (int i = 1; i <= 6; i++)
            {
                var overallCell = overallRow.CreateCell(i);
                overallCell.CellStyle = boldDefaultStyle;
            }
            sheet.GetRow(currentRowId).GetCell(0).SetCellValue(_classOverallRowTitle);
            sheet.GetRow(currentRowId).GetCell(1).SetCellValue((double)accClass.OpeningBalanceActiveSum);
            sheet.GetRow(currentRowId).GetCell(2).SetCellValue((double)accClass.OpeningBalancePassiveSum);
            sheet.GetRow(currentRowId).GetCell(3).SetCellValue((double)accClass.TurnoverDebitSum);
            sheet.GetRow(currentRowId).GetCell(4).SetCellValue((double)accClass.TurnoverCreditSum);
            sheet.GetRow(currentRowId).GetCell(5).SetCellValue((double)accClass.ClosingBalanceActiveSum);
            sheet.GetRow(currentRowId).GetCell(6).SetCellValue((double)accClass.ClosingBalancePassiveSum);
            currentRowId++;
        }
    }
}
