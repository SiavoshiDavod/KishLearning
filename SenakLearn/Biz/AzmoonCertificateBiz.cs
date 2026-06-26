using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models.Azmoon;
using SenakLearn.Models.wrapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace SenakLearn.Biz
{
    public class AzmoonCertificateBiz
    {
        public static readonly AzmoonCertificateBiz Instance = new AzmoonCertificateBiz();
        public PagedList<AzmoonEntityWrapper> GetAllPagedList(GridSettings grid, int entityId)
        {
            using (var ctx = new SWEntities())
            {
                var list = (from u in ctx.AzmoonUserAnswer
                            join e in ctx.AzmoonEntity on u.AzmoonEntityId equals e.Id
                            join lu in ctx.learn_user on u.UserId equals lu.id 
                            where u.AzmoonEntityId == entityId
                            select new AzmoonEntityWrapper()
                            {
                                AzmoonUserAnswerId = u.Id,
                                AzmoonEntityName = e.Name,
                                NameFamily = lu.Name + " " + lu.Family,
                                UserName = lu.user_name,
                                UserId = lu.id,
                                TotalScore = u.TotalScore,
                                TotalRank = u.TotalRank,
                                AzmounDate = u.CreatedDate,
                                AzmoonEntityId = e.Id,
                                Accepted = u.AcceptedDate != null ? true : false,
                                AcceptedDate = u.AcceptedDate,
                                TimeDuration = e.TimeDuration
                            });
                try
                {
                    //var slkdfj= list.Where(a => a.AzmoonUserAnswerId == null).ToList();
                    var result = list.FilterAndSortJqGrid<AzmoonEntityWrapper>(grid).ToPagedList<AzmoonEntityWrapper>(grid);
                    result.ForEach(row => {
                        row.AcceptedDatePersian = row.AcceptedDate?.ToPersianDate();
                        row.AzmounDatePersian= row.AzmounDate.ToPersianDate();
                    });
                    return result;
                }
                catch (SqlException ex)
                {

                    throw;
                }
               

            }
        }

        public MemoryStream GetCertificate( int userAnswerId)
        {
            using (var ctx = new SWEntities())
            {
                var list = (from u in ctx.AzmoonUserAnswer
                            join e in ctx.AzmoonEntity on u.AzmoonEntityId equals e.Id
                            join lu in ctx.learn_user on u.UserId equals lu.id into uuu
                            from user in uuu.DefaultIfEmpty()
                            where u.Id == userAnswerId
                            select new AzmoonEntityWrapper()
                            {
                                AzmoonUserAnswerId = u.Id,
                                AzmoonEntityName = e.Name,
                                NameFamily = user.Name + " " + user.Family,
                                UserName = user.user_name,
                                UserId = user.id,
                                TotalScore = u.TotalScore,
                                TotalRank = u.TotalRank,
                                AzmounDate = u.CreatedDate,
                                AzmoonEntityId = e.Id,
                                Accepted = u.AcceptedDate != null ? true : false,
                                AcceptedDate = u.AcceptedDate,
                                TimeDuration = e.TimeDuration,
                               
                            });

                var result = list;
                return new MemoryStream();
            }
        }
    }
}