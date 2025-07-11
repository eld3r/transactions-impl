namespace System;

public static class DateTimeExtensions
{
    public static DateTime ArrangeWithPgTimePrecision(this DateTime dt)
    {
        var ticks = dt.Ticks;
        var truncatedTicks = ticks - (ticks % 10);
        return new DateTime(truncatedTicks, dt.Kind);
    }
}