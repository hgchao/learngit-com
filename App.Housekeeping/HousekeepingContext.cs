using System;
using System.Collections.Generic;
using System.Text;

namespace App.Housekeeping
{
    public class HousekeepingContext
    {
        public static HousekeepingContext Instance;
        static HousekeepingContext()
        {
            Instance = new HousekeepingContext();
        }
        private HousekeepingContext() { }
        public const string FormNameOfProjectHousekeeping = "文明施工";
    }
}
