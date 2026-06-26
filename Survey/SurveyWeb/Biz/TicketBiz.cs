using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models.TicketNotice;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWeb.Biz
{
    public class TicketBiz : RepositoryBase<Ticket>
    {
        public static readonly TicketBiz Instance = new TicketBiz();
        public virtual JqGrid.PagedList<Ticket> GetAllCurrentUserTicket(GridSettings grid, int currentUserId, bool? isRead)
        {
            using (var ctx = new Models.Context())
                return ctx.Ticket.Where(x => x.SenderUserId == currentUserId && (isRead==null || x.IsRead==isRead)).Include(x => x.ReceiverUser).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public async Task<List<Ticket>> GetAllCurrentUserTicket( int currentUserId)
        {
            using (var ctx = new Models.Context())
                return await ctx.Ticket.Where(x => x.SenderUserId == currentUserId && !x.IsRead && x.ReceiverUserId == null).OrderByDescending(x=>x.Id).Take(5).ToListAsync();
        }
        public  PagedList<Ticket> GetAllPagedList(GridSettings grid,bool? isAnswer)
        {
            using (var ctx = new Models.Context())
                return ctx.Ticket.Where(x => isAnswer==null || (isAnswer == true && x.ReceiverUserId != null) || (isAnswer == false && x.ReceiverUserId == null)).Include(x => x.SenderUser).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public async Task<Ticket> Update(Ticket ticket)
        {
            if (string.IsNullOrWhiteSpace(ticket.Answer))
            {
                return null;
            }
            using (var ctx = new Models.Context())
            {
                Ticket t = await ctx.Ticket.FirstOrDefaultAsync(x => x.Id == ticket.Id);
                if (t != null)
                {
                    t.ReceiverUserId = ticket.ReceiverUserId;
                    t.Answer = ticket.Answer;
                    t.UpdateDate = System.DateTime.Now;
                    await ctx.SaveChangesAsync();
                    return t;
                }
                return null;
            }
        }
        public async Task<Ticket> SetIsRead(int id)
        {
            using (var ctx = new Models.Context())
            {
                Ticket t = await ctx.Ticket.FirstOrDefaultAsync(x => x.Id == id);
                if (t != null )
                {
                    if(!t.IsRead)
                        t.IsRead = true;
                    await ctx.SaveChangesAsync();
                    return t;
                }
                return null;
            }
        }
    }
}