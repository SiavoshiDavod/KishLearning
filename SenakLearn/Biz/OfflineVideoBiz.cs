using SenakLearn.Models;
using SenakLearn.Models.wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Biz
{
    public class OfflineVideoBiz : RepositoryBaseParentChild<SenakLearn.Models.OfflineVideo>
    {
        public static readonly OfflineVideoBiz Instance = new OfflineVideoBiz();

        public IEnumerable<OfflineVideoWrapper> GetAllVideo(int learn_coursId)
        {
            using (var context = new SWEntities())
            {
                var list = (from c in context.learn_cours
                            join i in context.OfflineVideo on c.id equals i.learn_coursId
                            join v in context.VideoFiles on i.VideoId equals v.VideoId
                            where c.id == learn_coursId
                            select new OfflineVideoWrapper
                            {
                                learn_coursId = c.id,
                                IsFree = i.IsFree,
                                Title = v.titel,
                                Description = v.titel,
                                VideoId = v.VideoId,
                                Id = i.Id,
                                ParentId= i.ParentId,
                            }).ToList();
                return list;
            }
        }
    }
}