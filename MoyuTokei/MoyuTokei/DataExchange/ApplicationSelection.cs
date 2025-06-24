using System;
using System.Collections.Generic;

namespace MoyuTokei.DataExchange
{
    internal sealed class ApplicationSelection
    {
        public List<(string Title, IntPtr Handle)> CurrentAvailableApplications { get; set; }

        public List<IntPtr> SelectedApplicationHandles { get; set; }
    }
}