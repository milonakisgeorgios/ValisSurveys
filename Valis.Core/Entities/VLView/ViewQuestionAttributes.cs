using System;

namespace Valis.Core
{
    [Flags]
    internal enum ViewQuestionAttributes : int
    {
        None = 0,
        ShowChart = 64,           // 1 << 6
        ShowDataTable = 128,          // 1 << 7
        ShowDataInTheChart = 256,          // 1 << 8
        HideZeroResponseOptions = 512,          // 1 << 9
        SwapRowsAndColumns = 1024,         // 1 << 10
    }
}
