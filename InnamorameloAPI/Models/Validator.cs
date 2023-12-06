using MongoDB.Bson;
using System.Reflection;
using System.Text.RegularExpressions;

namespace InnamorameloAPI.Models
{
    public class Validator
    {
        static public bool ValidateFields<T>(T obj)
        {
            try
            {
                PropertyInfo[] properties = obj.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    // Verifica se il campo è null o vuoto (per stringhe)
                    object value = property.GetValue(obj);
                    if (value == null || (value is string && string.IsNullOrEmpty((string)value)))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        static public void CopyProperties<TSource, TDestination>(TSource source, TDestination destination)
        {
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDestination).GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);

                if (destinationProperty != null && destinationProperty.CanWrite)
                {                    
                    try
                    {
                        var sourceValue = sourceProperty.GetValue(source);
                        Type destinationType = destinationProperty.PropertyType;
                        var destinationValue = ConvertValue(sourceValue, destinationProperty.PropertyType);
                        destinationProperty.SetValue(destination, destinationValue);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }            
        }

        static object ConvertValue(object value, Type destinationType)
        {
            if (value == null)
            {
                return destinationType.IsValueType ? Activator.CreateInstance(destinationType) : null;
            }

            if (destinationType.IsAssignableFrom(value.GetType()))
            {
                return value;
            }

            if (value is string stringValue)
            {
                if (destinationType.FullName.Contains("ObjectId"))
                {
                    return new ObjectId(stringValue);
                }
                else if (destinationType == typeof(Guid))
                {
                    return Guid.Parse(stringValue);
                }
                else if (destinationType.IsEnum)
                {
                    return Enum.Parse(destinationType, stringValue);
                }
                else if (int.TryParse(stringValue, out int intValue))
                {
                    return Convert.ChangeType(intValue, destinationType);
                }
                else if (double.TryParse(stringValue, out double doubleValue))
                {
                    return Convert.ChangeType(doubleValue, destinationType);
                }
                else if (decimal.TryParse(stringValue, out decimal decimalValue))
                {
                    return Convert.ChangeType(decimalValue, destinationType);
                }
            }
            else if (value is ObjectId objectIdValue)
            {
                if (destinationType == typeof(string))
                {
                    return objectIdValue.ToString();
                }
            }
            else if (value is Guid guidValue)
            {
                if (destinationType == typeof(string))
                {
                    return guidValue.ToString();
                }
            }
            else if (value is Enum enumValue)
            {
                if (destinationType == typeof(string))
                {
                    return enumValue.ToString();
                }
            }
            else if (value is IConvertible)
            {
                try
                {
                    return Convert.ChangeType(value, destinationType);
                }
                catch
                {
                    // Intentionally ignore the exception
                }
            }

            return value;
        }
    }
}
