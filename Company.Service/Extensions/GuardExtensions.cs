/**
* 
* @author : Kai Tam    
*  
*/

using System;

namespace Company.Service.Extensions
{
    public static class GuardExtensions
    {
        public static string ThrowIfArgumentIsNullOrEmpty(this string arg, string argName)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName);
            }

            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentException($"{argName} is empty.", argName);
            }

            return arg;
        }

        public static T ThrowIfArgumentIsNull<T>(this T arg, string argName)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName);
            }

            return arg;
        }
    }
}
