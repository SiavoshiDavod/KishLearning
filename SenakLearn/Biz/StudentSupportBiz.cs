namespace SenakLearn.Biz
{
    public class StudentSupportBiz : RepositoryBaseParentChild<SenakLearn.Models.StudentSupport>
    {
        public static readonly StudentSupportBiz Instance = new StudentSupportBiz();
    }
    public class TeacherSupportBiz : RepositoryBaseParentChild<SenakLearn.Models.TeacherSupport>
    {
        public static readonly TeacherSupportBiz Instance = new TeacherSupportBiz();
    }

}