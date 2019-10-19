using System;

namespace ePhoto.NET.Helpers {
    public static class DateTimeFormatter {
        public static string ToPretty(this DateTime value) {
            var subtractedValue = DateTime.Now.Subtract(value);

            if (subtractedValue.TotalSeconds < 60)
                return $"{subtractedValue.Seconds} ثانیه پیش";

            if (subtractedValue.TotalMinutes < 2)
                return "یه دقیقه پیش";

            if (subtractedValue.TotalMinutes < 60)
                return $"{subtractedValue.Minutes} دقیقه پیش";

            if (subtractedValue.TotalHours < 24)
                return $"{subtractedValue.Hours} ساعت پیش";

            if (Math.Abs(subtractedValue.TotalDays - 1) < double.Epsilon)
                return "دیروز";

            if (subtractedValue.TotalDays < 7)
                return $"{subtractedValue.Days} روز پیش";

            if (subtractedValue.TotalDays < 14)
                return "هفته گذشته";

            if (subtractedValue.TotalDays < 21)
                return "دو هفته پیش";

            if (subtractedValue.TotalDays < 28)
                return "سه هفته پیش";

            if (subtractedValue.TotalDays < 60)
                return "ماه گذشته";

            if (subtractedValue.TotalDays < 365)
                return $"{Math.Round(subtractedValue.TotalDays/30)} ماه پیش";

            if (subtractedValue.TotalDays < 730)
                return "سال گذشته";

            return $"{Math.Round(subtractedValue.TotalDays/365)} سال پیش";
        }
    }
}