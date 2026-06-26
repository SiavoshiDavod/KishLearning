using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SurveyWeb
{
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