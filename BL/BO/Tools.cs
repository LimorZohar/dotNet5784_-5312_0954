using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace BO
{
    static class Tools
    {
        // A function that returns a string representing the object's contents
        public static string ToStringProperty<T>(this T obj)
        {
            // Get the properties of an object of type T
            PropertyInfo[] properties = typeof(T).GetProperties();

            // Build a string representing the object
            string result = string.Join(", ", properties.Select(property =>
            {
                // Get the value of the current property
                object? value = property.GetValue(obj);
                string valueString;

                // Check if the value is null
                if (value == null)
                {
                    valueString = "null";
                }
                // Check if the value is a collection (IEnumerable)
                else if (value is IEnumerable<object> enumerableValue)
                {
                    // If so, convert each item in the collection to a string and join them together
                    valueString = string.Join(", ", enumerableValue.Select(item => item.ToString()));
                }
                // If not a collection or null, convert the value to a string
                else
                {
                    valueString = value.ToString()!;
                }
                // Build the string representing the property and its value
                return $"{property.Name}: {valueString}";
            }));
            // Return the result
            return result;
        }


        // Function that checks if the number is positive
        public static void ValidatePositiveId(int? id, string paramName)
        {
            if (id <= 0)
                throw new BO.BlInvalidDataException($"Invalid {paramName} ID. Must be a positive number.");
        }

        // Function that checks if the string is not empty or contains only spaces
        public static void ValidateNonEmptyString(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BO.BlInvalidDataException($"{paramName} cannot be empty.");
        }

        // Function that checks if the number is positive
        public static void ValidatePositiveNumber(double? number, string paramName)
        {
            if (number < 0)
                throw new BO.BlInvalidDataException($"Invalid {paramName}. Must be a positive number.");
        }

        // Function that checks the validity of an email address
        public static void ValidateEmail(string? email, string paramName)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email!);
                if (addr.Address != email)
                    throw new BO.BlInvalidDataException($"Invalid email address for {paramName}.");
            }
            catch
            {
                throw new BO.BlInvalidDataException($"Invalid email address for {paramName}.");
            }
        }
        public static Status CalculateStatus(DateTime? startDate, DateTime? SchedualDate, DateTime? deadlineDate, DateTime? completeDate)
        {
            if (startDate != null && completeDate == null) // אם המשימה באמצע להעשות 
                return Status.OnTrack;

            if (completeDate != null) // אם המשימה הושלמה 
                return Status.Completed;

            if (completeDate == null && DateTime.Now > SchedualDate) // אם המשימה עוד לא נגמרה וכבר עבר התאריך המתכונן לסיום
                return Status.InJeopardy;

            if (SchedualDate == null && deadlineDate == null) // אם המשימה עוד לא בלוז
                return Status.Unscheduled;

            if (SchedualDate != null && deadlineDate != null) // אם המשימה כבר בלוז
                return Status.Scheduled;

            return Status.Unscheduled;
        }

    }

}
