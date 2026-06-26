namespace SurveyWeb.Migrations
{
    using SurveyWeb.Biz;
    using SurveyWeb.Models;
    using SurveyWeb.Models.BaseInfo;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SurveyWeb.Models.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

        }

        protected override void Seed(SurveyWeb.Models.Context context)
        {
            //  This method will be called after migrating to the latest version.
            if (!context.User.Any())
            {
                context.User.Add(new Models.User() { Name = "Admin", Family = "Admin", CreatedDate = DateTime.Now, Pass = "Admin", UserName = "Admin",Email= "Admin@Admin.com", Mobile = "0912", RoleId = Models.Roles.Admin });
                context.SaveChanges();
            }

            if (!context.ResturantType.Any())
            {
                context.ResturantType.Add(new ResturantType() { DropDownTitle = "رستوران" });
                context.ResturantType.Add(new ResturantType() { DropDownTitle = "فست فود" });
                context.ResturantType.Add(new ResturantType() { DropDownTitle = "کافی شاپ" });
                context.SaveChanges();

            }
            if (!context.CheckListType.Any())
            {
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "معرفی نامه از جامعه مراکز پذیرایی" });
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "تاییدیه اداره ایمنی و آتش نشانی کیش" });
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "تاییدیه بهداشت مکان از مرکز توسعه سلامت کیش" });
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "تاییدیه اداره اماکن" });
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "تاییدیه صلاحیت فردی بهره بردار  و پرسنل" });
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "عکس ، کپی شناسنامه و کارت ملی مدیر و بهره بردار" });
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "مدارک بهره بردار جهت صدور مجوز" });
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "کپی برابر اصل سند ملک/اجاره نامه" });
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "بیمه نامه های : آتش سوزی،مسئولیت کارفرما در قبال کارکنان و مراجعین" });
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "تاییدیه منوی غذا  به نرخ نامه غذایی" });
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "تاییدیه منوی غذا  با رسپی (فارسی و لاتین)" });
                context.CheckListType.Add(new CheckListType() { DropDownTitle = "تصویر آخرین مجوز فعالیت اقتصادی('در صورت تمدید مجوز" });
                //context.CheckListType.Add(new CheckListType() { DropDownTitle = "رستوران" });
                context.SaveChanges();

            }
            if (!context.Menu.Any())
            {
                Menu m1 = new Menu() { CreatedDate = DateTime.Now, Order = 1, Status = true, Title = "" };
                m1.MenuSubs.Add(new MenuSub() { Title = "", Order = 1, CreatedDate = DateTime.Now, Status = true });

                Menu m2 = new Menu() { CreatedDate = DateTime.Now, Order = 1, Status = true, Title = "" };
                m2.MenuSubs.Add(new MenuSub() { Title = "", Order = 1, CreatedDate = DateTime.Now, Status = true });

                context.Menu.AddRange(new List<Menu>() { m1, m2 });
                context.SaveChanges();
            }

            if (!context.Educations.Any())
            {
                context.Educations.Add(new Education() { DropDownTitle = "بی سواد" });
                context.Educations.Add(new Education() { DropDownTitle = "خواندن و نوشتن" });
                context.Educations.Add(new Education() { DropDownTitle = "سیکل" });
                context.Educations.Add(new Education() { DropDownTitle = "دیپلم" });
                context.Educations.Add(new Education() { DropDownTitle = "فوق دیپلم" });
                context.Educations.Add(new Education() { DropDownTitle = "کارشناسی" });
                context.Educations.Add(new Education() { DropDownTitle = "کارشناسی ارشد" });
                context.Educations.Add(new Education() { DropDownTitle = "دکتری" });
                context.SaveChanges();

            }
        }
    }
}
