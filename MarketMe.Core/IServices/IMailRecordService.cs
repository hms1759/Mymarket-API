using MarketMe.Core.Models;
using MarketMe.Core.ViewModels;
using MarketMe.Share.Enum;
using MarketMe.Share.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.IServices
{
    public interface IMailRecordService : IService<MailRecords>
    {
        Task<bool> Create(MailRecordViewModel model);

        //Task<Page<MailRecordViewModel>> GetAllMailRecords(int pageNumber = 1, int pageSize = 10);
        //Task<Page<MailRecordViewModel>> Filter(NotificationEnum status, int pageNumber = 1, int pageSize = 10);

        //Task<List<MailRecordViewModel>> GetRecordAll();
        //Task<MailRecordViewModel> GetRecordById(Guid Id);
        //Task<bool> UpdateRecord(Guid Id);
    }
}
