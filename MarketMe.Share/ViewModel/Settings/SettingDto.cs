
using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels.Settings
{
    public class SettingDto: BaseDto
    {
        public bool IsDefault { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public object Value { get; set; }
        public int DataType { get; set; }
        public Guid? Business_Id { get; set; }
        public Guid? Store_Id { get; set; }
    }
}
