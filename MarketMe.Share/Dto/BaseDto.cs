using System;

namespace Shared.Dto
{
    public abstract class BaseDto
    {
        public Guid Id { get; set; }
        public int TotalCount { get; set; }
        public long RowNo { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
