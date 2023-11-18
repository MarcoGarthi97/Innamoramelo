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

        //static private bool CorruptInput(string input)
        //{
        //    Regex regex = new Regex("[^a-zA-Z0-9]");

        //    return regex.IsMatch(input);
        //}
    }
}
