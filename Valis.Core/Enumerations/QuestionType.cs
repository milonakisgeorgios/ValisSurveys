namespace Valis.Core
{
    public enum QuestionType : byte
    {
        /// <summary>
        /// One line of text input
        /// <para>Open-ended question type.</para>
        /// </summary>
        SingleLine = 1,
        /// <summary>
        /// Multiple lines of text input
        /// <para>Open-ended question type.</para>
        /// </summary>
        MultipleLine = 2,
        /// <summary>
        /// Numeric input
        /// <para>Open-ended question type.</para>
        /// </summary>
        Integer = 3,
        /// <summary>
        /// Decimal input
        /// <para>Open-ended question type.</para>
        /// </summary>
        Decimal = 4,
        /// <summary>
        /// Date input
        /// <para>Open-ended question type.</para>
        /// </summary>
        Date = 5,
        /// <summary>
        /// 
        /// <para>Open-ended question type.</para>
        /// </summary>
        Time = 6,
        /// <summary>
        /// 
        /// <para>Open-ended question type.</para>
        /// </summary>
        DateTime = 7,
        /// <summary>
        /// List of radio buttons
        /// <para>Radio buttons indicate that respondents can only select one answer 
        /// from the list of choices. When a respondent selects an answer choice, all 
        /// other answer choices are deselected automatically.</para>
        /// <para>Closed-ended question type. Supports skip logic</para>
        /// </summary>
        OneFromMany = 10,
        /// <summary>
        /// List of checkboxes
        /// <para>Checkboxes indicate that respondents are allowed to select multiple 
        /// answers from the list of choices.</para>
        /// <para>Closed-ended question type.</para>
        /// </summary>
        ManyFromMany = 11,
        /// <summary>
        /// DropDown control
        /// <para>Closed-ended question type. Supports skip logic</para>
        /// </summary>
        DropDown = 12,
        /// <summary>
        /// If you want to create text within the body of your survey that presents 
        /// additional information like a section heading, an introduction, a footer, 
        /// etc., use the Descriptive Text option.
        /// </summary>
        DescriptiveText = 16,   
        /// <summary>
        /// 
        /// </summary>
        Slider = 21,
        /// <summary>
        /// 
        /// </summary>
        Range = 22,
        /// <summary>
        /// A matrix with radio controls
        /// </summary>
        MatrixOnePerRow = 31,
        /// <summary>
        /// A matrix with checkboxes
        /// </summary>
        MatrixManyPerRow = 32,
        /// <summary>
        /// A matrix with user defined control
        /// </summary>
        MatrixManyPerRowCustom = 33,
        /// <summary>
        /// 
        /// </summary>
        Composite = 41,
        /// <summary>
        /// 
        /// </summary>
        /// 
    }
}
