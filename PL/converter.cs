using BO;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PL;

public class CalcStatusColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            Status status = (Status)value;
            switch (status)
            {
                case Status.Unscheduled:
                    return Brushes.Black;
                    break;
                case Status.Scheduled:
                    return Brushes.Blue;
                    break;
                case Status.InJeopardy:
                    return Brushes.Yellow;
                    break;
                case Status.OnTrack:
                    return Brushes.Green;
                    break;
                case Status.Completed:
                    return Brushes.Red;
                    break;
                default:
                    return Brushes.Black;
            }
        }
        catch { return Brushes.Black; }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
