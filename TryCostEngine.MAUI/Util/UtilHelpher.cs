using System;

namespace TryCostEngine.MAUI
{
    public static class UtilHelpher
    {
        public static string GenerateSerialNumber(int step = 0)
        {
            return GenerateSerialNumber("", step);
        }

        public static string GenerateSerialNumber(string fix , int step)
        {
            var number = "000";
            step++;
            if (step < 1000)
            {
                var s = step.ToString();
                if (s.Length < 2)
                {
                    number = $"00{s}";
                }
                else if ((s.Length >= 2) && (s.Length < 3))
                {
                    number = $"0{s}";
                }
                else
                {
                    number = $"{s}";
                }
            }
            if (!string.IsNullOrEmpty(fix))
                return $"{fix}{number}";
            else
                return $"{number}";
        }
    }
}
