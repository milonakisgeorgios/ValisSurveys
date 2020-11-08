using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Valis.Core.Export
{
    /// <summary>
    /// The 'All Responses Data' XLS export Class 
    /// <para>Responses Data exports organize survey results by respondent. The XLS export 
    /// includes a spreadsheet where each row contains the answers from a given respondent, 
    /// allowing you to do your own analysis in Excel.</para>
    /// </summary>
    public static class AllResponsesXlsExport
    {
        static ILog Logger = LogManager.GetLogger(typeof(AllResponsesXlsExport));


        public static void CreateExcelDocument(VLSurveyManager surveyManager, VLSurvey survey, Collection<VLSurveyQuestionEx> questions, Collection<VLResponseEx> responses, string excelFilename)
        {
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook))
                {
                    WriteExcelFile(surveyManager, survey, questions, responses, document);
                }
            }
            catch (VLException ex)
            {
                var message = string.Format("An exception occured while calling AllResponsesXlsExport.CreateExcelDocument(), AccessTokenId={0}, surveyId={1}, excelFilename={2}", surveyManager.AccessTokenId, survey, excelFilename);
                Logger.Error(string.Format("RefId={0}, {1}", ex.ReferenceId, message), ex);
                throw new VLException(ex.Message, ex.ReferenceId);
            }
            catch (Exception ex)
            {
                var message = string.Format("An exception occured while calling AllResponsesXlsExport.CreateExcelDocument(), AccessTokenId={0}, surveyId={1}, excelFilename={2}", surveyManager.AccessTokenId, survey, excelFilename);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }
        }

        private static void WriteExcelFile(VLSurveyManager surveyManager, VLSurvey survey, Collection<VLSurveyQuestionEx> questions, Collection<VLResponseEx> responses, SpreadsheetDocument spreadsheet)
        {
            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
            //Stylesheet stylesheet = new Stylesheet();
            //workbookStylesPart.Stylesheet = stylesheet;
            workbookStylesPart.Stylesheet = CreateStylesheet();
            workbookStylesPart.Stylesheet.Save();



            uint worksheetNumber = 1;
            {
                // For each worksheet you want to create
                string workSheetID = "rId" + worksheetNumber.ToString();
                string worksheetName = "Responses";

                WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet();

                // create sheet data
                newWorksheetPart.Worksheet.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.SheetData());

                // save worksheet
                WriteDataTableToExcelWorksheet(surveyManager, survey, questions, responses, newWorksheetPart);
                newWorksheetPart.Worksheet.Save();

                // create the worksheet to workbook relation
                if (worksheetNumber == 1)
                    spreadsheet.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

                spreadsheet.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>().AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = (uint)worksheetNumber,
                    Name = "Responses"
                });

                worksheetNumber++;
            }

            spreadsheet.WorkbookPart.Workbook.Save();
        }
        private static void WriteDataTableToExcelWorksheet(VLSurveyManager surveyManager, VLSurvey survey, Collection<VLSurveyQuestionEx> questions, Collection<VLResponseEx> responses, WorksheetPart worksheetPart)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();


            //Columns columns = new Columns();
            //columns.Append(CreateColumnData(1, 2, 11));
            //columns.Append(CreateColumnData(3, 99, 22));
            //sheetData.Append(columns);



            int rowIndex = 1;
            #region XCompany Surveys
            var titleRow1 = new Row { RowIndex = (uint)rowIndex };
            sheetData.Append(titleRow1);
            AppendTextCell(GetExcelColumnName(rowIndex, 0), "XCompany Surveys", titleRow1, 5);
            #endregion
            rowIndex = 2;
            #region All Responses Data export for survey...
            var titleRow2 = new Row { RowIndex = (uint)rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(titleRow2);
            AppendTextCell(GetExcelColumnName(rowIndex, 1), string.Format("All Responses Data export for survey '{0}'", survey.Title), titleRow2, 5);
            #endregion
            rowIndex = 3;
            #region Recorded Responses...
            var titleRow3 = new Row { RowIndex = (uint)rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(titleRow3);
            AppendTextCell(GetExcelColumnName(rowIndex, 1), string.Format("Recorded Responses = {0}, Accessible responses = {1}", survey.RecordedResponses, responses.Count), titleRow3, 5);
            #endregion


            string cellValue = "";
            int columnIndex = 2;

            rowIndex = 6;
            var firstHeaderRow = new Row { RowIndex = (uint)rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(firstHeaderRow);
            rowIndex = 7;
            var subHeaderRow = new Row { RowIndex = (uint)rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(subHeaderRow);
            {
                #region First Columns
                AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), "SurveyId", firstHeaderRow, 6);
                AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), "Collector", firstHeaderRow, 6);
                AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), "ResponseId", firstHeaderRow, 6);
                AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), "ResponseType", firstHeaderRow, 6);
                AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), "RecipientId", firstHeaderRow, 6);
                AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), "Email", firstHeaderRow, 6);
                AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), "FirstName", firstHeaderRow, 6);
                AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), "LastName", firstHeaderRow, 6);
                AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), "StartDate", firstHeaderRow, 6);
                AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), "EndDate", firstHeaderRow, 6);
                AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), "Time Taken to Complete (Seconds)", firstHeaderRow, 6);
                #endregion

                #region questions
                foreach(var q in questions)
                {
                    if (q.QuestionType == QuestionType.SingleLine)
                    {
                        #region QuestionType.SingleLine
                        AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), q.QuestionText, firstHeaderRow, 8);
                        #endregion
                    }
                    else if (q.QuestionType == QuestionType.MultipleLine)
                    {
                        #region QuestionType.MultipleLine
                        AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), q.QuestionText, firstHeaderRow, 8);
                        #endregion
                    }
                    else if (q.QuestionType == QuestionType.Integer || q.QuestionType == QuestionType.Decimal)
                    {
                        #region QuestionType.Integer, QuestionType.Decimal
                        AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), q.QuestionText, firstHeaderRow, 8);
                        #endregion
                    }
                    else if (q.QuestionType == QuestionType.Date)
                    {
                        #region QuestionType.Date
                        AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), q.QuestionText, firstHeaderRow, 8);
                        #endregion
                    }
                    else if (q.QuestionType == QuestionType.OneFromMany)
                    {
                        #region QuestionType.OneFromMany
                        AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), q.QuestionText, firstHeaderRow, 8);
                        if (q.OptionalInputBox)
                        {
                            AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), q.QuestionText + string.Format(" - Other ({0})", q.OtherFieldLabel), firstHeaderRow, 8);
                        }
                        #endregion
                    }
                    else if (q.QuestionType == QuestionType.ManyFromMany)
                    {
                        #region QuestionType.ManyFromMany
                        AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex), q.QuestionText, firstHeaderRow, 8);

                        foreach (var op in q.Options)
                        {
                            //var cellStringValue = string.Format("{0} - {1}", q.QuestionText, op.OptionText);
                            AppendTextCell(GetExcelColumnName(subHeaderRow, columnIndex++), op.OptionText, subHeaderRow,7);
                        }
                        if (q.OptionalInputBox)
                        {
                            //var cellStringValue = q.QuestionText + string.Format(" - Other ({0})", q.OtherFieldLabel);
                            AppendTextCell(GetExcelColumnName(subHeaderRow, columnIndex++), string.Format("Other ({0})", q.OtherFieldLabel), subHeaderRow, 7);
                        }
                        #endregion
                    }
                    else if (q.QuestionType == QuestionType.DropDown)
                    {
                        #region QuestionType.DropDown
                        AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex++), q.QuestionText, firstHeaderRow, 8);
                        #endregion
                    }
                    else if (q.QuestionType == QuestionType.Range)
                    {
                        #region QuestionType.DropDown
                        AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex), q.QuestionText, firstHeaderRow, 8);

                        AppendTextCell(GetExcelColumnName(subHeaderRow, columnIndex++), string.Format("({0} - {1})", q.RangeStart, q.RangeEnd), subHeaderRow, 7);
                        #endregion
                    }
                    else if (q.QuestionType == QuestionType.MatrixOnePerRow)
                    {
                        #region QuestionType.MatrixOnePerRow
                        AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex), q.QuestionText, firstHeaderRow, 8);

                        foreach (var op in q.Options)
                        {
                            //var cellStringValue = string.Format("{0} - {1}", q.QuestionText, op.OptionText);
                            AppendTextCell(GetExcelColumnName(subHeaderRow, columnIndex++), op.OptionText, subHeaderRow, 7);
                        }
                        #endregion
                    }
                    else if (q.QuestionType == QuestionType.MatrixManyPerRow)
                    {
                        #region QuestionType.MatrixManyPerRow
                        AppendTextCell(GetExcelColumnName(firstHeaderRow, columnIndex), q.QuestionText, firstHeaderRow, 8);

                        foreach (var op in q.Options)
                        {
                            //var cellStringValue = string.Format("{0} - {1}", q.QuestionText, op.OptionText);
                            AppendTextCell(GetExcelColumnName(subHeaderRow, columnIndex++), op.OptionText, subHeaderRow, 10);
                        }
                        #endregion
                    }
                }
                #endregion
            }


            //
            //  Now, step through each response...
            //
            foreach (var rsp in responses)
            {
                ++rowIndex;
                var newExcelRow = new Row { RowIndex = (uint)rowIndex };  // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);
                {
                    #region First Columns
                    columnIndex = 2;//SurveyId
                    AppendIntegerCell(GetExcelColumnName(newExcelRow, columnIndex), survey.PublicId, newExcelRow);
                    
                    columnIndex++;//Collector
                    if (rsp.Collector.HasValue)
                    {
                        AppendIntegerCell(GetExcelColumnName(newExcelRow, columnIndex), rsp.Collector.Value.ToString(CultureInfo.InvariantCulture), newExcelRow);
                    }
                    
                    columnIndex++;//ResponseId
                    AppendIntegerCell(GetExcelColumnName(newExcelRow, columnIndex), rsp.ResponseId.ToString(CultureInfo.InvariantCulture), newExcelRow);
                    
                    columnIndex++;//ResponseType
                    if(rsp.ResponseType == ResponseType.Default)
                    {
                        AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), "Default", newExcelRow);
                    }
                    
                    columnIndex++;//RecipientId
                    if (rsp.Recipient.HasValue)
                    {
                        AppendIntegerCell(GetExcelColumnName(newExcelRow, columnIndex), rsp.Recipient.Value.ToString(CultureInfo.InvariantCulture), newExcelRow);
                    }
                    
                    columnIndex++;//Email
                    //AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), string.Empty, newExcelRow);
                    
                    columnIndex++;//FirstName
                    //AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), string.Empty, newExcelRow);
                    
                    columnIndex++;//LastName
                    // AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), string.Empty, newExcelRow);

                    columnIndex++;//OpenDate
                    //cellValue = rsp.OpenDate.ToString("dd/MM/yyyy HH:mm:ss");
                    cellValue = rsp.OpenDate.ToOADate().ToString();
                    AppendDateCell(GetExcelColumnName(newExcelRow, columnIndex), rsp.OpenDate, newExcelRow);
                    
                    columnIndex++;//CloseDate
                    if(rsp.CloseDate.HasValue)
                    {
                        //cellValue = rsp.CloseDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                        cellValue = rsp.CloseDate.Value.ToOADate().ToString();
                        AppendDateCell(GetExcelColumnName(newExcelRow, columnIndex), rsp.CloseDate.Value, newExcelRow);
                    }
                    
                    columnIndex++;//Duration
                    if (rsp.CloseDate.HasValue)
                    {
                        double seconds = (rsp.CloseDate.Value - rsp.OpenDate).TotalSeconds;
                        cellValue = seconds.ToString(CultureInfo.InvariantCulture);
                        AppendIntegerCell(GetExcelColumnName(newExcelRow, columnIndex), cellValue, newExcelRow);
                    }
                    #endregion


                    #region questions
                    foreach (var q in questions)
                    {

                        if (q.QuestionType == QuestionType.SingleLine)
                        {
                            #region QuestionType.SingleLine
                            /*εχουμε το πολύ ένα μόνο ResponseDetail:*/
                            var detail = rsp.GetResponseDetail(q.QuestionId);
                            
                            columnIndex++;
                            if (detail != null)
                            {
                                AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow,9);
                            }
                            #endregion
                        }
                        else if (q.QuestionType == QuestionType.MultipleLine)
                        {
                            #region QuestionType.MultipleLine
                            /*εχουμε το πολύ ένα μόνο ResponseDetail:*/
                            var detail = rsp.GetResponseDetail(q.QuestionId);

                            columnIndex++;
                            if (detail != null)
                            {
                                AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow,9);
                            }
                            #endregion
                        }
                        else if (q.QuestionType == QuestionType.Integer)
                        {
                            #region QuestionType.Integer
                            /*εχουμε το πολύ ένα μόνο ResponseDetail:*/
                            var detail = rsp.GetResponseDetail(q.QuestionId);

                            columnIndex++;
                            if (detail != null)
                            {
                                Int32 _temp = 0;
                                if(!string.IsNullOrWhiteSpace(detail.UserInput))
                                {
                                    if (Int32.TryParse(detail.UserInput, out _temp))
                                    {
                                        AppendIntegerCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow);
                                    }
                                    else
                                    {
                                        AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow);
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (q.QuestionType == QuestionType.Decimal)
                        {
                            #region QuestionType.Decimal
                            /*εχουμε το πολύ ένα μόνο ResponseDetail:*/
                            var detail = rsp.GetResponseDetail(q.QuestionId);

                            columnIndex++;
                            if (detail != null)
                            {
                                Double _temp = 0;
                                if (!string.IsNullOrWhiteSpace(detail.UserInput))
                                {
                                    if (Double.TryParse(detail.UserInput, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _temp))
                                    {
                                        AppendNumericCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow);
                                    }
                                    else
                                    {
                                        AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow);
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (q.QuestionType == QuestionType.Date)
                        {
                            #region QuestionType.Date
                            /*εχουμε το πολύ ένα μόνο ResponseDetail:*/
                            var detail = rsp.GetResponseDetail(q.QuestionId);

                            columnIndex++;
                            if (detail != null)
                            {
                                //ΣΤΑ RESPONSES ΟΙ ΗΜΕΡΟΜΗΝΙΕΣ ΕΙΝΑΙ ΠΑΝΤΑ MM/DD/YYYY:
                                DateTime _temp;
                                if(!string.IsNullOrWhiteSpace(detail.UserInput))
                                {
                                    if (DateTime.TryParseExact(detail.UserInput, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _temp))
                                    {
                                        AppendDateCell(GetExcelColumnName(newExcelRow, columnIndex), _temp, newExcelRow);
                                    }
                                    else
                                    {
                                        AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow);
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (q.QuestionType == QuestionType.OneFromMany)
                        {
                            #region QuestionType.OneFromMany
                            /*εχουμε το πολύ ένα μόνο ResponseDetail:*/
                            var detail = rsp.GetResponseDetail(q.QuestionId);

                            columnIndex++;
                            if (detail != null)
                            {
                                //Μήπως είναι επιλογή
                                if (detail.SelectedOption.HasValue)
                                {
                                    var option = q.GetQuestionOption(detail.SelectedOption.Value);
                                    if (option != null)
                                    {
                                        AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), option.OptionText, newExcelRow, 9);
                                        detail = null;
                                    }
                                    else
                                    {
                                        if (q.OptionalInputBox == true)
                                        {
                                            AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), "Other", newExcelRow, 9);
                                        }
                                    }
                                }
                            }

                            if (q.OptionalInputBox == true)
                            {
                                columnIndex++;
                                if (detail != null && string.IsNullOrWhiteSpace(detail.UserInput) == false)
                                {
                                    AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow, 9);
                                }
                            }
                            #endregion
                        }
                        else if (q.QuestionType == QuestionType.ManyFromMany)
                        {
                            #region QuestionType.ManyFromMany
                            foreach (var op in q.Options)
                            {
                                columnIndex++;
                                var detail = rsp.GetResponseDetail(q.QuestionId, op.OptionId);
                                if (detail != null)
                                {
                                    //AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), op.OptionText, newExcelRow);
                                    AppendIntegerCell(GetExcelColumnName(newExcelRow, columnIndex), "1", newExcelRow);
                                }
                            }
                            if (q.OptionalInputBox)
                            {
                                columnIndex++;
                                var detail = rsp.GetResponseDetail(q.QuestionId, 0); if (detail != null)
                                {
                                    //AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow);
                                    AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow);
                                }
                            }
                            #endregion
                        }
                        else if (q.QuestionType == QuestionType.DropDown)
                        {
                            #region QuestionType.DropDown
                            /*εχουμε το πολύ ένα μόνο ResponseDetail:*/
                            var detail = rsp.GetResponseDetail(q.QuestionId);

                            columnIndex++;
                            if (detail != null)
                            {
                                //Μήπως είναι επιλογή
                                if (detail.SelectedOption.HasValue)
                                {
                                    var option = q.GetQuestionOption(detail.SelectedOption.Value);
                                    if (option != null)
                                    {
                                        AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), option.OptionText, newExcelRow, 9);
                                        detail = null;
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (q.QuestionType == QuestionType.Range)
                        {
                            #region QuestionType.Integer
                            /*εχουμε το πολύ ένα μόνο ResponseDetail:*/
                            var detail = rsp.GetResponseDetail(q.QuestionId);

                            columnIndex++;
                            if (detail != null)
                            {
                                Int32 _temp = 0;
                                if (Int32.TryParse(detail.UserInput, out _temp))
                                {
                                    AppendIntegerCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow);
                                }
                                else
                                {
                                    AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), detail.UserInput, newExcelRow);
                                }
                            }
                            #endregion
                        }
                        else if (q.QuestionType == QuestionType.MatrixOnePerRow)
                        {
                            #region QuestionType.MatrixOnePerRow
                            /*εχουμε το πολύ ένα μόνο ResponseDetail:*/
                            foreach (var op in q.Options)
                            {
                                columnIndex++;
                                var detail = rsp.GetResponseDetail(q.QuestionId, op.OptionId);
                                if (detail != null)
                                {
                                    if (detail.SelectedColumn.HasValue)
                                    {
                                        var column = q.GetQuestionColumn(detail.SelectedColumn.Value);
                                        if (column != null)
                                        {
                                            AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), column.ColumnText, newExcelRow, 9);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (q.QuestionType == QuestionType.MatrixManyPerRow)
                        {
                            #region QuestionType.MatrixManyPerRow
                            foreach (var op in q.Options)
                            {
                                columnIndex++;
                                var detail = rsp.GetResponseDetail(q.QuestionId, op.OptionId);
                                if (detail != null)
                                {
                                    if (detail.SelectedColumn.HasValue)
                                    {
                                        var column = q.GetQuestionColumn(detail.SelectedColumn.Value);
                                        if (column != null)
                                        {
                                            AppendTextCell(GetExcelColumnName(newExcelRow, columnIndex), column.ColumnText, newExcelRow);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }

            }
        }



        static void AppendTextCell(string cellReference, string cellStringValue, Row excelRow, UInt32? styleIndex = null)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference, DataType = CellValues.String};
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            if(styleIndex.HasValue)
            {
                cell.StyleIndex = styleIndex.Value;
            }
            excelRow.Append(cell);
        }
        static void AppendDateCell(string cellReference, DateTime value, Row excelRow)
        {
            Cell cell = new Cell();
            cell.CellReference = cellReference;
            cell.StyleIndex = 1;
            cell.CellValue = new CellValue(value.ToOADate().ToString(CultureInfo.InvariantCulture));
            excelRow.Append(cell);
        }
        static void AppendNumericCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference};
            cell.StyleIndex = 2;
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);

            excelRow.Append(cell);
        }
        static void AppendIntegerCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference };
            cell.StyleIndex = 3;
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);

            excelRow.Append(cell);
        }


        static string GetExcelColumnName(Row row, int columnIndex)
        {
            return GetExcelColumnName((int)row.RowIndex.Value, columnIndex);
        }
        static string GetExcelColumnName(int rowIndex, int columnIndex)
        {
            //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            //
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString() + rowIndex.ToString(CultureInfo.InvariantCulture);

            char firstChar = (char)('A' + (columnIndex / 26) - 1);
            char secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}{2}", firstChar, secondChar, rowIndex.ToString(CultureInfo.InvariantCulture));
        }

        static Column CreateColumnData(UInt32 StartColumnIndex, UInt32 EndColumnIndex, double ColumnWidth)
        {
            Column column;
            column = new Column();
            column.Min = StartColumnIndex;
            column.Max = EndColumnIndex;
            column.Width = ColumnWidth;
            column.CustomWidth = true;
            return column;
        }

        static Stylesheet CreateStylesheet()
        {
            Stylesheet ss = new Stylesheet();

            #region fts (fonts)
            Fonts fts = new Fonts();
            var ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
            FontName ftn = new FontName();
            ftn.Val = StringValue.FromString("Arial");
            FontSize ftsz = new FontSize();
            ftsz.Val = DoubleValue.FromDouble(11);
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);

            ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
            ftn = new FontName();
            ftn.Val = StringValue.FromString("Verdana");
            ftsz = new FontSize();
            ftsz.Val = DoubleValue.FromDouble(18);
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);

            ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
            ft.Append(new Bold());
            ftn = new FontName();
            ftn.Val = StringValue.FromString("Arial");
            ftsz = new FontSize();
            ftsz.Val = DoubleValue.FromDouble(11);
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);

            fts.Count = UInt32Value.FromUInt32((uint)fts.ChildElements.Count);
            #endregion

            #region fills
            Fills fills = new Fills();
            Fill fill;
            PatternFill patternFill;
            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.None;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Gray125;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Solid;
            patternFill.ForegroundColor = new ForegroundColor();
            patternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("00efffd9");
            patternFill.BackgroundColor = new BackgroundColor();
            patternFill.BackgroundColor.Rgb = patternFill.ForegroundColor.Rgb;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Solid;
            patternFill.ForegroundColor = new ForegroundColor();
            patternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("00f8e8d6");
            patternFill.BackgroundColor = new BackgroundColor();
            patternFill.BackgroundColor.Rgb = patternFill.ForegroundColor.Rgb;
            fill.PatternFill = patternFill;
            fills.Append(fill);

            fills.Count = UInt32Value.FromUInt32((uint)fills.ChildElements.Count);
            #endregion

            #region borders
            Borders borders = new Borders();
            Border border = new Border();
            border.LeftBorder = new LeftBorder();
            border.RightBorder = new RightBorder();
            border.TopBorder = new TopBorder();
            border.BottomBorder = new BottomBorder();
            border.DiagonalBorder = new DiagonalBorder();
            borders.Append(border);

            border = new Border();
            border.LeftBorder = new LeftBorder();
            border.LeftBorder.Style = BorderStyleValues.Thin;
            border.RightBorder = new RightBorder();
            border.RightBorder.Style = BorderStyleValues.Thin;
            border.TopBorder = new TopBorder();
            border.TopBorder.Style = BorderStyleValues.Thin;
            border.BottomBorder = new BottomBorder();
            border.BottomBorder.Style = BorderStyleValues.Thin;
            border.DiagonalBorder = new DiagonalBorder();
            borders.Append(border);
            borders.Count = UInt32Value.FromUInt32((uint)borders.ChildElements.Count);
            #endregion

            #region csfs (CellStyleFormats)
            CellStyleFormats csfs = new CellStyleFormats();
            CellFormat cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            csfs.Append(cf);
            csfs.Count = UInt32Value.FromUInt32((uint)csfs.ChildElements.Count);
            #endregion


            uint iExcelIndex = 164;
            var nfs = new DocumentFormat.OpenXml.Spreadsheet.NumberingFormats();
            CellFormats cfs = new CellFormats();

            cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cfs.Append(cf);

            var nfDateTime = new DocumentFormat.OpenXml.Spreadsheet.NumberingFormat();
            nfDateTime.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nfDateTime.FormatCode = StringValue.FromString("dd/mm/yyyy hh:mm:ss");
            nfs.Append(nfDateTime);

            var nfDate = new DocumentFormat.OpenXml.Spreadsheet.NumberingFormat();
            nfDate.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nfDate.FormatCode = StringValue.FromString("dd/mm/yyyy");
            nfs.Append(nfDate);

            var nf4decimal = new DocumentFormat.OpenXml.Spreadsheet.NumberingFormat();
            nf4decimal.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nf4decimal.FormatCode = StringValue.FromString("#,##0.0000");
            nfs.Append(nf4decimal);

            // #,##0.00 is also Excel style index 4
            var nf2decimal = new DocumentFormat.OpenXml.Spreadsheet.NumberingFormat();
            nf2decimal.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nf2decimal.FormatCode = StringValue.FromString("#,##0.00");
            nfs.Append(nf2decimal);

            // @ is also Excel style index 49
            var nfForcedText = new DocumentFormat.OpenXml.Spreadsheet.NumberingFormat();
            nfForcedText.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nfForcedText.FormatCode = StringValue.FromString("@");
            nfs.Append(nfForcedText);

            // #,##0.00 is also Excel style index 4
            var nf0decimal = new DocumentFormat.OpenXml.Spreadsheet.NumberingFormat();
            nf0decimal.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nf0decimal.FormatCode = StringValue.FromString("#,##0");
            nfs.Append(nf0decimal);

            // index 1
            cf = new CellFormat();
            cf.NumberFormatId = nfDate.NumberFormatId;
            cf.FontId = 0;      //Arial 11
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 2
            cf = new CellFormat();
            cf.NumberFormatId = nf4decimal.NumberFormatId;
            cf.FontId = 0;      //Arial 11
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 3
            cf = new CellFormat();
            cf.NumberFormatId = nf0decimal.NumberFormatId;
            cf.FontId = 0;      //Arial 11
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 4
            cf = new CellFormat();
            cf.NumberFormatId = nfForcedText.NumberFormatId;
            cf.FontId = 0;      //Arial 11
            cf.FillId = 0;      //NO Fill
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 5
            cf = new CellFormat();
            cf.NumberFormatId = nfForcedText.NumberFormatId;
            cf.FontId = 1;      //Verdana 18
            cf.FillId = 0;      //NO Fill
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 6
            // column text
            cf = new CellFormat();
            cf.NumberFormatId = nfForcedText.NumberFormatId;
            cf.FontId = 2;      //Arial 11, Bold
            cf.FillId = 0;      //NO Fill
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 7
            cf = new CellFormat();
            cf.NumberFormatId = nfForcedText.NumberFormatId;
            cf.FontId = 0;      //Arial 11
            cf.FillId = 3;      //Light Orange
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 8
            // column text
            cf = new CellFormat();
            cf.NumberFormatId = nfForcedText.NumberFormatId;
            cf.FontId = 2;      //Arial 11, Bold
            cf.FillId = 2;      //Light Green
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cfs.Append(cf);

            // index 9
            cf = new CellFormat();
            cf.NumberFormatId = nfForcedText.NumberFormatId;
            cf.FontId = 0;      //Arial 11
            cf.FillId = 0;      //NO Fill
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cf.Append(new Alignment() { WrapText = true });
            cfs.Append(cf);

            // index 10
            cf = new CellFormat();
            cf.NumberFormatId = nfForcedText.NumberFormatId;
            cf.FontId = 0;      //Arial 11
            cf.FillId = 3;      //Light Orange
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = BooleanValue.FromBoolean(true);
            cf.Append(new Alignment() { WrapText = true });
            cfs.Append(cf);

            nfs.Count = UInt32Value.FromUInt32((uint)nfs.ChildElements.Count);
            cfs.Count = UInt32Value.FromUInt32((uint)cfs.ChildElements.Count);

            ss.Append(nfs);
            ss.Append(fts);
            ss.Append(fills);
            ss.Append(borders);
            ss.Append(csfs);
            ss.Append(cfs);

            CellStyles css = new CellStyles();
            CellStyle cs = new CellStyle();
            cs.Name = StringValue.FromString("Normal");
            cs.FormatId = 0;
            cs.BuiltinId = 0;
            css.Append(cs);
            css.Count = UInt32Value.FromUInt32((uint)css.ChildElements.Count);
            ss.Append(css);

            DifferentialFormats dfs = new DifferentialFormats();
            dfs.Count = 0;
            ss.Append(dfs);

            TableStyles tss = new TableStyles();
            tss.Count = 0;
            tss.DefaultTableStyle = StringValue.FromString("TableStyleMedium9");
            tss.DefaultPivotStyle = StringValue.FromString("PivotStyleLight16");
            ss.Append(tss);

            return ss;
        }
    }
}
