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

    }
}
