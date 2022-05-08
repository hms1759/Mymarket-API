using MarketMe.Share.Models.Settings;
using MarketMe.Share.ViewModels;
using Shared.Enums;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels.Settings
{
    public class SettingsViewModel : BaseViewModel
    {
        public bool IsDefault { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public Guid? Business_Id { get; set; }
        public Guid? Store_Id { get; set; }

        public static explicit operator SettingsViewModel(Setting source)
        {
            var destination = new SettingsViewModel
            {
                Id = source.Id.ToString(),
                Type = source.Type,
                Description = source.Description,
                Business_Id = source.Business_Id,
                Store_Id = source.Store_Id,
                IsDefault = source.IsDefault,
            };

            destination.Value = SettingsDataTypeConverter.TryConvertToObject(source.Value, (SettingTypes)source.Type).ToString();
            destination.ModifiedOn = source.ModifiedOn.HasValue ? source.ModifiedOn.Value.ToString("dd/MM/yyyy") : null;
            destination.CreatedOn = source.CreatedOn.ToString("dd/MM/yyyy");

            return destination;
        }

        public static explicit operator Setting(SettingsViewModel source)
        {
            var destination = new Setting
            {
                Type = source.Type,
                Description = source.Description,
                Business_Id = source.Business_Id,
                Store_Id = source.Store_Id,
                IsDefault = source.IsDefault,
            };

            return destination;
        }

        public static explicit operator SettingsViewModel(SettingDto source)
        {
            var destination = new SettingsViewModel
            {
                Id = source.Id.ToString(),
                Type = source.Type,
                Description = source.Description,
                Business_Id = source.Business_Id,
                Store_Id = source.Store_Id,
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy,
                IsDefault = source.IsDefault,
                TotalCount = source.TotalCount
            };

            destination.Value = SettingsDataTypeConverter.TryConvertToObject(source.Value, (SettingTypes)source.Type).ToString();
            destination.ModifiedOn = source.ModifiedOn.HasValue ? source.ModifiedOn.Value.ToString("dd/MM/yyyy") : null;
            destination.CreatedOn = source.CreatedOn.HasValue ? source.CreatedOn.Value.ToString("dd/MM/yyyy") : null;
            return destination;
        }
    }
}
