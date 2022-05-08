using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace MarketMe.Share.ViewModels
{
    [Serializable]
    public abstract class BaseViewModel<T>
    {
        public BaseViewModel()
        {
            ErrorList = new List<string>();
        }

        public string Id { get; set; }
        [JsonIgnore]
        public virtual bool HasError
        {
            get
            {
                if (ErrorList.Any())
                    return true;

                return false;
            }
        }

        [JsonIgnore]
        public virtual List<string> ErrorList { get; set; } = new List<string>();
        [JsonIgnore]
        public int TotalCount { get; set; }
        public T Payload { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }

    [Serializable]
    public class BaseViewModel : BaseViewModel<string>, IValidatableObject
    {
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
