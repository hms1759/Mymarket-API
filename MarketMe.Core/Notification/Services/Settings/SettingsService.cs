using Dapper;
using MarketMe.Share.Extensions;
using MarketMe.Share.Models.Settings;
using Shared.Dapper;
using Shared.Dapper.Interfaces;
using Shared.Enums;
using Shared.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.Settings
{
    public class SettingsService : Service<Setting>, ISettingsService
    {
        public SettingsService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<SettingsViewModel> GetTaxValue(int type)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@type", type);
            var sql = "SELECT TOP 1 * FROM [Setting] WHERE [Type] = @type ORDER BY ModifiedOn DESC";
            var setting = this.SqlQuery<SettingDto>(sql, parameter).FirstOrDefault();

            if (setting == null)
            {
                Results.Add(new ValidationResult($"This setting could not be found."));
                return null;
            }


            return await Task.FromResult((SettingsViewModel)setting);
        }

        public SettingsViewModel SettingDetails(Guid settingId, Guid? businessId = null, Guid? storeId = null)
        {
            var sql = @"SELECT TOP 1 * FROM [Setting] 
                        WHERE Id = @id
                        AND (@businessId IS NULL OR Business_Id = @businessId)
                        AND (@storeId IS NULL OR Store_Id = @storeId)
                        AND IsDeleted <> 1 AND IsDefault <> 1";

            var setting = this.SqlQuery<Setting>(sql,
                new
                {
                    id = settingId,
                    businessId,
                    storeId
                }).FirstOrDefault();

            if (setting == null)
            {
                this.Results.Add(new ValidationResult("Unable to find setting."));
                return default;
            }

            return (SettingsViewModel)setting;
        }

        public async Task DeleteSetting(Guid settingId, Guid? businessId = null, Guid? storeId = null)
        {
            var sql = @"SELECT TOP 1 * FROM [Setting] 
                        WHERE Id = @id
                        AND (@businessId IS NULL OR Business_Id = @businessId)
                        AND (@storeId IS NULL OR Store_Id = @storeId)
                        AND IsDeleted <> 1 AND IsDefault <> 1";

            var deleteSetting = this.SqlQuery<Setting>(sql,
                new
                {
                    id = settingId,
                    businessId,
                    storeId
                }).FirstOrDefault();

            if (deleteSetting == null)
            {
                this.Results.Add(new ValidationResult("Unable to find setting to delete."));
                return;
            }

            await this.DeleteAsync(deleteSetting);
        }

        public async Task<IEnumerable<SettingsViewModel>> SearchSetting(string keyword, int? pageIndex = 1, int? pageSize = 10,
            Guid? businessId = null, Guid? storeId = null)
        {
            pageIndex = (pageIndex <= 0 || !pageIndex.HasValue) ? 1 : pageIndex.Value;
            pageSize = (pageSize <= 1 || !pageSize.HasValue) ? 10 : pageSize.Value;

            var parameters = new DynamicParameters();
            parameters.Add("@Keyword", keyword);
            parameters.Add("@PageIndex", pageIndex);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@BusinessId", businessId);
            parameters.Add("@StoreId", storeId);

            var result = await this.ExecuteStoredProcedure<SettingDto>("[sp_search_settings]", parameters);
            return result.Select(t => (SettingsViewModel)t);
        }

        public async Task UpdateSetting(SettingsViewModel viewModel, Guid? businessId = null, Guid? storeId = null)
        {
            if (!Enum.IsDefined(typeof(SettingTypes), viewModel.Type))
            {
                this.Results.Add(new ValidationResult("Type of settings couldn't be resolved."));
                return;
            }

            var settingValue = SettingsDataTypeConverter.TryConvertToObject(viewModel.Value, (SettingTypes)viewModel.Type);

            if (settingValue == null)
            {
                this.Results.Add(new ValidationResult("Unable to convert value for setting type"));
                return;
            }

            var sql = @"SELECT TOP 1 * FROM [Setting] 
                        WHERE [Type] = @type
                        AND Id <> @id
                        AND (@businessId IS NULL OR Business_Id = @businessId)
                        AND (@storeId IS NULL OR Store_Id = @storeId)
                        AND IsDeleted <> 1
                        AND IsDefault <> 1";

            var result = this.SqlQuery<Setting>(sql,
                new
                {
                    id = viewModel.Id,
                    type = viewModel.Type,
                    businessId,
                    storeId
                });

            if (result.Any())
            {
                this.Results.Add(new ValidationResult("Setting already exist for business."));
                return;
            }

            sql = @"SELECT TOP 1 * FROM [Setting] 
                        WHERE Id = @id
                        AND (@businessId IS NULL OR Business_Id = @businessId)
                        AND (@storeId IS NULL OR Store_Id = @storeId)
                        AND IsDeleted <> 1 AND IsDefault <> 1";

            var editSetting = this.SqlQuery<Setting>(sql,
                new
                {
                    id = viewModel.Id,
                    type = viewModel.Type,
                    businessId,
                    storeId
                }).FirstOrDefault();

            if (editSetting == null)
            {
                this.Results.Add(new ValidationResult("Setting to update couldn't be found"));
                return;
            }

            editSetting.Type = viewModel.Type;

            if (string.IsNullOrWhiteSpace(viewModel.Description))
            {
                var type = (SettingTypes)viewModel.Type;
                editSetting.Description = type.GetDescription();
            }
            else
            {
                editSetting.Description = viewModel.Description;
            }

            editSetting.Value = settingValue.ToString();
            editSetting.ModifiedOn = editSetting.CreatedOn = DateTime.Now.GetDateUtcNow();
            editSetting.ModifiedBy = editSetting.CreatedBy = viewModel.CreatedBy;

            await this.UpdateAsync(editSetting);
        }


        public async Task CreateSetting(SettingsViewModel viewModel, Guid? businessId = null, Guid? storeId = null)
        {
            if (!Enum.IsDefined(typeof(SettingTypes), viewModel.Type))
            {
                this.Results.Add(new ValidationResult("Type of settings couldn't be resolved."));
                return;
            }

            var settingValue = SettingsDataTypeConverter.TryConvertToObject(viewModel.Value, (SettingTypes)viewModel.Type);

            if (settingValue == null)
            {
                this.Results.Add(new ValidationResult("Unable to convert value for setting type"));
                return;
            }

            var sql = @"SELECT TOP 1 * FROM [Setting] 
                        WHERE [Type] = @type
                        AND (@businessId IS NULL OR Business_Id = @businessId)
                        AND (@storeId IS NULL OR Store_Id = @storeId)
                        AND IsDeleted <> 1
                        AND IsDefault <> 1";

            var result = this.SqlQuery<Setting>(sql,
                new
                {
                    type = viewModel.Type,
                    businessId,
                    storeId
                });

            if (result.Any())
            {
                this.Results.Add(new ValidationResult("Setting already exist for business."));
                return;
            }

            var setting = (Setting)viewModel;

            if (string.IsNullOrWhiteSpace(setting.Description))
            {
                var type = (SettingTypes)viewModel.Type;
                setting.Description = type.GetDescription();
            }

            setting.Value = settingValue.ToString();
            setting.ModifiedOn = setting.CreatedOn = DateTime.Now.GetDateUtcNow();
            setting.ModifiedBy = setting.CreatedBy = viewModel.CreatedBy;
            setting.Business_Id = businessId;
            setting.Store_Id = businessId;
            await this.AddAsync(setting);

            viewModel.Id = setting.Id.ToString();
        }

        public async Task<decimal> GetTax(int type = 0, Guid? businessId = null, Guid? storeId = null)
        {
            var sql = @"SELECT TOP 1 * FROM [Setting] 
                        WHERE [Type] = @type
                        AND (@businessId IS NULL OR Business_Id = @businessId)
                        AND (@storeId IS NULL OR Store_Id = @storeId)
                        AND IsDeleted <> 1";

            var result = this.SqlQuery<SettingDto>(sql, new { type, businessId, storeId });
            await Task.CompletedTask;

            var setting = result.FirstOrDefault();

            if (setting == null)
                return default;

            var viewModel = (SettingsViewModel)setting;

            if (!decimal.TryParse(viewModel.Value.ToString(), out decimal tax))
                return default;

            return tax;
        }

        public async Task<IEnumerable<SettingsViewModel>> GetSystemSettings()
        {
            var sql = "SELECT * FROM [setting]";

            var result = this.SqlQuery<SettingDto>(sql, null);
            await Task.CompletedTask;
            return result.Select(s => (SettingsViewModel)s);

        }


        public async Task UpdateSettings(SettingsViewModel[] model)
        {
            foreach (var setting in model)
            {
                Setting s = this.FindById(Guid.Parse(setting.Id));

                var value = SettingsDataTypeConverter.TryConvertToObject(setting.Value, (SettingTypes)s.Type);

                if (value == null)
                {
                    this.Results.Add(new ValidationResult("Unable to convert value for setting type"));
                    return;
                }

                s.Value = value.ToString();
                await this.UpdateAsync(s);
            }

            await Task.CompletedTask;
        }
    }
}
