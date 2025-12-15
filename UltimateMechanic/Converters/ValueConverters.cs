using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UltimateMechanic.Converters
{
    /// <summary>
    /// Converts an integer count to a visibility value.
    /// Returns Visible if count is 0, otherwise returns Collapsed.
    /// </summary>
    public class CountToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a count to visibility.
        /// </summary>
        /// <param name="value">The count value.</param>
        /// <param name="targetType">The target type (Visibility).</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>Visibility.Visible if count is 0, otherwise Visibility.Collapsed.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                return count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts back a visibility value to a count (not implemented).
        /// </summary>
        /// <param name="value">Not used.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns>Throws NotImplementedException.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts bytes to a numeric size value (MB, GB, etc.).
    /// </summary>
    public class BytesToSizeConverter : IValueConverter
    {
        /// <summary>
        /// Converts bytes to a formatted size value.
        /// </summary>
        /// <param name="value">The byte value.</param>
        /// <param name="targetType">The target type (double).</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The size as a double value in appropriate unit.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long bytes)
            {
                double len = bytes;
                int order = 0;
                while (len >= 1024 && order < 4)
                {
                    order++;
                    len = len / 1024;
                }
                return Math.Round(len, 2);
            }
            return 0;
        }

        /// <summary>
        /// Converts back a size value to bytes (not implemented).
        /// </summary>
        /// <param name="value">Not used.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns>Throws NotImplementedException.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts bytes to a size unit string (B, KB, MB, GB, TB).
    /// </summary>
    public class BytesToUnitConverter : IValueConverter
    {
        /// <summary>
        /// Converts bytes to a size unit string.
        /// </summary>
        /// <param name="value">The byte value.</param>
        /// <param name="targetType">The target type (string).</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The size unit as a string (B, KB, MB, GB, or TB).</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long bytes)
            {
                string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                double len = bytes;
                int order = 0;
                while (len >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }
                return sizes[order];
            }
            return "B";
        }

        /// <summary>
        /// Converts back a unit string to bytes (not implemented).
        /// </summary>
        /// <param name="value">Not used.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns>Throws NotImplementedException.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts a boolean value to a visibility value.
    /// Returns Visible if true, otherwise returns Collapsed.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to visibility.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target type (Visibility).</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>Visibility.Visible if true, otherwise Visibility.Collapsed.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts back a visibility value to a boolean (not implemented).
        /// </summary>
        /// <param name="value">Not used.</param>
        /// <param name="targetType">Not used.</param>
        /// <param name="parameter">Not used.</param>
        /// <param name="culture">Not used.</param>
        /// <returns>Throws NotImplementedException.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
