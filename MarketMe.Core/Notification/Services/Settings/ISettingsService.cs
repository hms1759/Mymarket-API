using MarketMe.Share.Models.Settings;
using Shared.Dapper;
using Shared.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.Settings
{
    public interface ISettingsService : IService<Setting>
    {
        Task<SettingsViewModel> GetTaxValue(int type);
        SettingsViewModel SettingDetails(Guid settingId, Guid? businessId = null, Guid? storeId = null);
        Task DeleteSetting(Guid settingId, Guid? businessId = null, Guid? storeId = null);
        Task<IEnumerable<SettingsViewModel>> SearchSetting(string keyword, int? pageIndex = 1, int? pageSize = 10,
            Guid? businessId = null, Guid? storeId = null);
        Task UpdateSetting(SettingsViewModel viewModel, Guid? businessId = null, Guid? storeId = null);
        Task CreateSetting(SettingsViewModel viewModel, Guid? businessId = null, Guid? storeId = null);
        Task<decimal> GetTax(int key = 0, Guid? businessId = null, Guid? storeId = null);
        Task<IEnumerable<SettingsViewModel>> GetSystemSettings();
        Task UpdateSettings(SettingsViewModel[] model);
    }
}
