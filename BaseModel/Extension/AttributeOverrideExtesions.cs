using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace BaseModel
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
                if (mode == NationalCodeMode.Person)
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
                            if (false == currentResult)
                            {
                                this.ErrorMessage = $"the index [{index}] of field is invalid nationalCode";
                            }
                        }
                        catch
                        {
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
            if (mode == NationalCodeMode.Person)
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
    public class GenericRequired : RequiredAttribute, IClientValidatable
    {
        public GenericRequired()
        {
            this.ErrorMessage = "وارد کردن {0} الزامی است";
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "required",
            };
            // rule.ValidationParameters.Add("max", "value1");
            yield return rule;
        }
    }
    public class GenericMaxLength : MaxLengthAttribute, IClientValidatable
    {
        public GenericMaxLength(int length) : base(length)
        {
            this.ErrorMessage = "حداکثر طول {0} تعداد {1} می باشد";
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "maxlength",
            };
            rule.ValidationParameters.Add("max", this.Length);
            yield return rule;
        }
    }
    public class GenericMinLength : MinLengthAttribute, IClientValidatable
    {
        public GenericMinLength(int length) : base(length)
        {
            this.ErrorMessage = "حداقل طول {0} تعداد {1} می باشد";
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "minlength",
            };
            rule.ValidationParameters.Add("min", this.Length);
            yield return rule;
        }
    }
    public class GenericStringLength : StringLengthAttribute, IClientValidatable
    {
        public GenericStringLength(int maximumLength) : base(maximumLength)
        {
            this.ErrorMessage = "حداکثر طول {0} تعداد {1} می باشد";
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "length",
            };
            rule.ValidationParameters.Add("max", this.MaximumLength);
            if (this.MinimumLength > 0)
            {
                rule.ValidationParameters.Add("min", this.MinimumLength);
            }
            yield return rule;
        }
    }
}
