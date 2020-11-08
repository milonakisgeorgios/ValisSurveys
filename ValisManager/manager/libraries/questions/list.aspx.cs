using System;
using System.Globalization;
using System.Text;
using Valis.Core;

namespace ValisManager.manager.libraries.questions
{
    public partial class list : ManagerPage
    {
        #region jqGrid support
        /// <summary>
        /// jqGrid's current page
        /// </summary>
        protected string PageNumber
        {
            get
            {
                string pageno = this.Request.Params["pageno"];
                if (string.IsNullOrEmpty(pageno))
                {
                    return "1";
                }
                try
                {
                    return Int32.Parse(pageno, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                }
                catch
                {
                    return "1";
                }
            }
        }
        /// <summary>
        /// jqGrid's current sortname
        /// </summary>
        protected string SortName
        {
            get
            {
                string sortname = this.Request.Params["sortname"];
                if (string.IsNullOrEmpty(sortname))
                {
                    return "QuestionText";
                }
                return sortname;
            }
        }
        /// <summary>
        /// jqGrid's current sortorder
        /// </summary>
        protected string SortOrder
        {
            get
            {
                string sortorder = this.Request.Params["sortorder"];
                if (string.IsNullOrEmpty(sortorder))
                {
                    return "asc";
                }
                return sortorder;
            }
        }
        /// <summary>
        /// jqGrid's current rownum
        /// </summary>
        protected string RowNum
        {
            get
            {
                return "16";
            }
        }
        #endregion


        protected string GetQuestionTypeOptions()
        {
            StringBuilder options = new StringBuilder();

            //SingleLine
            options.AppendFormat("<option value=\"{0}\">SingleLine</option>", QuestionType.SingleLine);
            //MultipleLine
            options.AppendFormat("<option value=\"{0}\">MultipleLine</option>", QuestionType.MultipleLine);
            //Integer
            options.AppendFormat("<option value=\"{0}\">Integer</option>", QuestionType.Integer);
            //Decimal
            options.AppendFormat("<option value=\"{0}\">Decimal</option>", QuestionType.Decimal);
            //Date
            options.AppendFormat("<option value=\"{0}\">Date</option>", QuestionType.Date);
            //Time
            //options.AppendFormat("<option value=\"{0}\">Time</option>", QuestionType.Time);
            //DateTime
            //options.AppendFormat("<option value=\"{0}\">DateTime</option>", QuestionType.DateTime);
            //OneFromMany
            options.AppendFormat("<option value=\"{0}\">OneFromMany</option>", QuestionType.OneFromMany);
            //ManyFromMany
            options.AppendFormat("<option value=\"{0}\">ManyFromMany</option>", QuestionType.ManyFromMany);
            //DropDown
            options.AppendFormat("<option value=\"{0}\">DropDown</option>", QuestionType.DropDown);
            //DescriptiveText
            options.AppendFormat("<option value=\"{0}\">DescriptiveText</option>", QuestionType.DescriptiveText);
            //Slider
            //options.AppendFormat("<option value=\"{0}\">Slider</option>", QuestionType.Slider);
            //Range
            options.AppendFormat("<option value=\"{0}\">Range</option>", QuestionType.Range);
            //MatrixOnePerRow
            options.AppendFormat("<option value=\"{0}\">MatrixOnePerRow</option>", QuestionType.MatrixOnePerRow);
            //MatrixManyPerRow
            options.AppendFormat("<option value=\"{0}\">MatrixManyPerRow</option>", QuestionType.MatrixManyPerRow);
            //MatrixManyPerRowCustom
            //options.AppendFormat("<option value=\"{0}\">MatrixManyPerRowCustom</option>", QuestionType.MatrixManyPerRowCustom);
            //Composite
            //options.AppendFormat("<option value=\"{0}\">Composite</option>", QuestionType.Composite);

            return options.ToString();
        }

    }
}