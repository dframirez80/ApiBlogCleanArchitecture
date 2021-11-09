using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class Numerical
    {
        public enum StatusUser { Active, Blocked, Pending };
        public const int MinValue = 0;
        public const int MaxValueReaction = 1;
        public const int MinValueReaction = 0;
    }
}
