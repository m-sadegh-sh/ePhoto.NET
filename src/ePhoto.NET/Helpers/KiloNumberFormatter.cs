namespace ePhoto.NET.Helpers {
    public static class KiloNumberFormatter {
        private const long KILO_FIFTEEN = (long) (KILO*.05);
        private const long KILO_TEN = (long) (KILO*.1);
        private const long KILO_TWENTY_FIVE = (long) (KILO*.25);
        private const long KILO_FIFTY = (long) (KILO*.5);
        private const long KILO_SEVENTY_FIVE = (long) (KILO*.75);
        private const long KILO = 1000;
        private const long MEGA = KILO*KILO;
        private const long GIGA = KILO*MEGA;

        public static string ToPretty(this int value) {
            if (value > GIGA)
                return (value/GIGA).ToString("0g");

            if (value > MEGA)
                return (value/MEGA).ToString("0m");

            if (value >= KILO)
                return (value/KILO).ToString("0k");

            if (value > KILO_SEVENTY_FIVE)
                return KILO_SEVENTY_FIVE.ToString("0+");

            if (value > KILO_FIFTY)
                return KILO_FIFTY.ToString("0+");

            if (value > KILO_TWENTY_FIVE)
                return KILO_TWENTY_FIVE.ToString("0+");

            if (value > KILO_TEN)
                return KILO_TEN.ToString("0+");

            if (value > KILO_FIFTEEN)
                return KILO_FIFTEEN.ToString("0+");

            return KILO_FIFTEEN.ToString("0~");
        }
    }
}