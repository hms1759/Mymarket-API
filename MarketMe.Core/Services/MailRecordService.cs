
using MarketMe.Core.IServices;
using MarketMe.Core.Models;
using MarketMe.Core.ViewModels;
using MarketMe.Share.Enum;
using MarketMe.Share.Extensions;
using MarketMe.Share.Utils;
using Shared.Dapper;
using Shared.Dapper.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Walure.Core.Services
{
    public class MailRecordService : Service<MailRecords>, IMailRecordService
    {
        public MailRecordService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<bool> Create(MailRecordViewModel model)
        {
            if (!IsValid(model))
                return default;

           // var data = model.Map();
           var data = new MailRecords();
            data.CreatedOn = data.CreatedOn = DateTime.Now.GetDateUtcNow();

            await this.AddAsync(data);
            //int count = await this.AddAsync(data);
            //if (count < 1)
            //{
            //    return false;
            //}
            // else return true;
            return true;
        }
    
    }
}
