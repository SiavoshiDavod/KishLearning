using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace SenakLearn
{
    public enum NationalCodeMode
    {
        Person,
        Company,
        Both
    }
    public class NationalCodeAttribute : ValidationAttribute
    {
        private NationalCodeMode mode;
        private bool isNullable = false;
        public NationalCodeAttribute(string mode, bool isNullable)
        {
            this.mode = (NationalCodeMode)System.Enum.Parse(typeof(NationalCodeMode), mode);
            this.isNullable = isNullable;
        }

        public NationalCodeAttribute(string mode) : this(mode, false)
        {
        }

        public NationalCodeAttribute(bool isNullable) : this("Person", isNullable)
        {
        }

        public NationalCodeAttribute() : this("Person", false)
        {

        }

        public override bool IsValid(object value)
        {
            #region list mode
            //if value is array 
            if (typeof(IEnumerable).IsAssignableFrom(value?.GetType()) && value?.GetType() != typeof(string))
            {
                if(mode == NationalCodeMode.Person)
                {
                    IEnumerable enumerable = value as IEnumerable;
                    bool result = true;
                    int index = 0;
                    foreach (object item in enumerable)
                    {
                        if (string.IsNullOrEmpty(item?.ToString()?.Trim()) && isNullable)
                        {
                            index++;
                            continue;
                        }


                        try
                        {
                            bool currentResult = IsValidPersonNationalCode(item.ToString());
                            result &= currentResult;
                            if(false == currentResult)
                            {
                                this.ErrorMessage = $"the index [{index}] of field is invalid nationalCode";
                            }
                        }
                        catch { 
                            this.ErrorMessage = $"the index [{index}] of field is invalid nationalCode";
                            return false;
                        }
                        
                        index++;
                    }

                    return result;
                }
            }
            #endregion

            #region single object mode

            if (isNullable && value == null)
                return true;


            if (string.IsNullOrEmpty(value?.ToString()?.Trim()) && isNullable)
            {
                return true;
            }

            string nationalCode = value?.ToString();
            if(mode == NationalCodeMode.Person)
            {
                return IsValidPersonNationalCode(nationalCode);
            }

            #endregion

            //no-object or invalid format of object
            return false;
        }

        public static bool IsValidPersonNationalCode(string input)
        {
            if (!Regex.IsMatch(input, @"^\d{10}$"))
                return false;

            var check = Convert.ToInt32(input.Substring(9, 1));
            var sum = Enumerable.Range(0, 9)
                .Select(x => Convert.ToInt32(input.Substring(x, 1)) * (10 - x))
                .Sum() % 11;

            return (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);
        }
    }
}