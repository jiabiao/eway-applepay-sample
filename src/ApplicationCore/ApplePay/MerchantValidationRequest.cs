using System.ComponentModel.DataAnnotations;

namespace eWAY.Samples.MonkeyStore.Models.ApplePay
{
    public class MerchantValidationRequest
    {
        [Required]
        [DataType(DataType.Url)]
        public string ValidationUrl { get; set; }
    }
}
