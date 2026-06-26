using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SenakLearn.Biz
{
    public class AzmoonAnswerBiz : RepositoryBaseSurvey<Models.AzmoonAnswer>
    {
        public static readonly AzmoonAnswerBiz Instance = new AzmoonAnswerBiz();

        public async Task<short?> ReCalculate(int? idAzmoonUserAnswer,int azmoonEntityId)
        {
            string sp = "UPDATE [AzmoonUserAnswers] SET [AzmoonUserAnswers].TotalRank = x.R FROM ( SELECT Id, ROW_NUMBER() OVER(ORDER BY [TotalScore] desc) AS R FROM [AzmoonUserAnswers] where [AzmoonEntityId]="+ azmoonEntityId + ") x where x.Id=[AzmoonUserAnswers].Id";
            using (var ctx = new SWEntities())
            {
                await ctx.Database.ExecuteSqlCommandAsync(sp);
                if (idAzmoonUserAnswer != null && idAzmoonUserAnswer > 0)
                    return await ctx.AzmoonUserAnswer.Where(x => x.Id == idAzmoonUserAnswer).Select(x => x.TotalRank).FirstOrDefaultAsync();
            }
            return null;
        }
        public async Task<string> SaveBatch(List<AzmoonAnswer> aswers, string ip, int userId)
        {
            string res = "آزمون شما با موفقيت به پايان رسيد.";
            var azmoonEntityId = aswers.First().AzmoonEntityId;
            List<AzmoonQuestion> questionsCorrectOptions = new List<AzmoonQuestion>();
            using (var ctx = new SWEntities())
                questionsCorrectOptions = ctx.AzmoonQuestion.Include(x => x.AzmoonEntity).Include(x => x.AzmoonQuestionOptions).Where(x => x.AzmoonEntityId == azmoonEntityId).ToList();
            List<AzmoonAnswer> result = new List<AzmoonAnswer>();
            var countAll = questionsCorrectOptions.Count(x => x.AzmoonQuestionOptions != null && x.AzmoonQuestionOptions.Count > 0);
            var countAnswerd = aswers.Count(x => !string.IsNullOrEmpty(x.AzmoonQuestionOptionId));
            var notanswed = countAll - countAnswerd;
            var corectanswed = 0;
            var wronganswed = 0;

            foreach (var item in aswers)
            {
                item.CreatedDate = DateTime.Now;
                var questionOption = questionsCorrectOptions.FirstOrDefault(x => x.Id == item.AzmoonQuestionId);
                if (questionOption != null)
                {
                    var correct = questionOption.AzmoonQuestionOptions?.Where(x => x.IsCorrect).ToList();
                    if (correct?.Count > 0)
                    {
                        if (string.IsNullOrWhiteSpace(item.AzmoonQuestionOptionId))
                        {
                            wronganswed += 1;
                            item.Score = questionOption.AzmoonEntity.ZaribManfi * questionOption.Score;
                        }
                        else if (item.AzmoonQuestionOptionId.Contains(","))
                        {
                            var azmoonQuestionOptionIds = item.AzmoonQuestionOptionId.Split(',').Select(Int32.Parse).ToList();

                            if (correct.Count == 1)
                            {
                                if (azmoonQuestionOptionIds.Any(x => x == correct.First().Id))
                                {
                                    corectanswed += 1;
                                    item.Score = questionOption.Score / azmoonQuestionOptionIds.Count;
                                }
                                else
                                {
                                    wronganswed += 1;
                                    item.Score = questionOption.AzmoonEntity.ZaribManfi * questionOption.Score;
                                }
                            }
                            else
                            {
                                var correctId = correct.Select(z => z.Id).ToList();
                                var dups = correctId.Intersect(azmoonQuestionOptionIds).ToList();
                                var distinctCorrect = correctId.Except(azmoonQuestionOptionIds).ToList();
                                var distinctWrong = azmoonQuestionOptionIds.Except(correctId).ToList();
                                if (dups.Count > 0)
                                {
                                    if (distinctCorrect.Count == 0 && distinctWrong.Count == 0)
                                    {
                                        corectanswed += 1;
                                        item.Score = questionOption.Score;
                                    }
                                    else if (distinctCorrect.Count == 0)
                                    {
                                        corectanswed += 1;
                                        item.Score = questionOption.Score / distinctWrong.Count;
                                    }
                                    else if (distinctWrong.Count == 0)
                                    {
                                        corectanswed += 1;
                                        item.Score = questionOption.Score / distinctCorrect.Count;
                                    }
                                }
                                else
                                {
                                    wronganswed += 1;
                                    item.Score = questionOption.AzmoonEntity.ZaribManfi * questionOption.Score;
                                }
                            }

                        }
                        else
                        {
                            if (correct.Count == 1)
                            {
                                if (correct.First().Id.ToString() == item.AzmoonQuestionOptionId.Trim())
                                {
                                    corectanswed += 1;
                                    item.Score = questionOption.Score;
                                }
                                else
                                {
                                    wronganswed += 1;
                                    item.Score = questionOption.AzmoonEntity.ZaribManfi * questionOption.Score;
                                }
                            }
                            else if (correct.Any(x => x.Id.ToString() == item.AzmoonQuestionOptionId.Trim()))
                            {
                                corectanswed += 1;
                                item.Score = questionOption.Score / correct.Count;
                            }
                            else
                            {
                                wronganswed += 1;
                                item.Score = questionOption.AzmoonEntity.ZaribManfi * questionOption.Score;
                            }
                        }
                    }
                    //else if (questionOption.Score > 0)
                    //{
                    //    tashrihi = true;
                    //}
                }
                try
                {
                    item.CreatedDate = DateTime.Now;
                    item.Validate();
                    result.Add(item);
                }
                catch (Exception)
                {
                }
            }
            if (result.Count <= 0)
            {
                throw new Exception("پاسخی برای دخیره سازی یافت نشد");
            }
            var totalScore = result.Sum(x => x.Score);

            if (!questionsCorrectOptions.First().AzmoonEntity.IsJustOption)
            {
                res += "</br>نمره بعدا تصحیح اعلام خواهد شد. ";
            }
            else
            {
                if (notanswed > 0)
                {
                    res += $"</br>تعداد {notanswed} سوال چندگزينه اي پاسخ داده نشده است.";
                }
                if (corectanswed > 0)
                {
                    res += $"</br>تعداد {corectanswed} سوال چندگزينه اي پاسخ صحيح داده شده است.";
                }
                if (wronganswed > 0)
                {
                    res += $"</br>تعداد {wronganswed} سوال چندگزينه اي پاسخ غلط داده شده است.";
                }


                //if (totalScore > 0)
                {
                    var total = questionsCorrectOptions.Sum(x => x.Score);
                    res += $"</br>نمره شما: {totalScore} از { total}.";
                    if (totalScore > questionsCorrectOptions.First().AzmoonEntity.MinScore)
                    {
                        res += $"</br>در صورت تمایل به گرفتن مدرک دوره به قسمت صدور گواهینامه مراجعه کنید.";
                    }
                    else
                    {
                        res += $"</br>متاسفانه شما حداقل نمره قبولی را کسب نکردید .";
                    }
                }
            }

            var azmmonuserans = new AzmoonUserAnswer() { CorrectAnswerd = corectanswed, NoAnswerd = notanswed, WrongAnswerd = wronganswed, AzmoonEntityId = azmoonEntityId, Ip = ip, UserId = userId > 0 ? userId : (int?)null, CreatedDate = DateTime.Now, AzmoonAnswers = result, TotalScore = totalScore };

            using (var ctx = new SWEntities())
            {
                ctx.AzmoonUserAnswer.Add(azmmonuserans);
                await ctx.SaveChangesAsync();
                if (questionsCorrectOptions.First().AzmoonEntity.IsRanking)
                {
                    short? rank = await ReCalculate(azmmonuserans.Id, azmoonEntityId);
                    if (rank != null)
                    {
                        int coun = await ctx.AzmoonUserAnswer.CountAsync(x => x.AzmoonEntityId == azmoonEntityId);
                        res += $"</br>رتبه شما {rank.Value} از {coun}.";
                    }
                }
            }

            return res;
        }

        internal async Task EditScore(int id, double score)
        {
            using (var ctx = new SWEntities())
            {
                AzmoonAnswer ans = await ctx.AzmoonAnswer.FirstOrDefaultAsync(x => x.Id == id);
                if (ans == null)
                {
                    throw new Exception("رکورد یافت نشد");
                }
                if (ans.Score != score)
                {

                    var userAnswer = await ctx.AzmoonUserAnswer.FirstOrDefaultAsync(x => x.Id == ans.AzmoonUserAnswerId);
                    if (ans.Score == 0 && score > 0)
                    {
                        userAnswer.CorrectAnswerd += 1;
                    }
                    else if (ans.Score >= 0 && score <= 0)
                    {
                        userAnswer.WrongAnswerd += 1;
                    }
                    //else { }
                    userAnswer.TotalScore = userAnswer.TotalScore + (score - ans.Score);
                    await ctx.SaveChangesAsync();

                    await ReCalculate(null,ans.AzmoonEntityId);

                    ans.Score = score;
                    await ctx.SaveChangesAsync();

                }

            }
        }
    }
}