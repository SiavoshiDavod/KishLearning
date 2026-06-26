using System.ComponentModel;

namespace SenakLearn.Enums
{
    public enum OnlineClassType: byte
    {
        [Description("در حال ثبت نام")]
        Registering,
        [Description("در حال برگزاری")]
        OnPerforming,
        [Description("به پایان رسیده")]
        End,
        [Description("تکمیل ظرفیت")]
        FullCapacity,
        [Description("بایگانی شده")]
        Archived
    }
}