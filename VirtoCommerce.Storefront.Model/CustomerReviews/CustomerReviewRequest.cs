using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Storefront.Model.CustomerReviews
{
    public class CustomerReviewRequest
    {
        [Required]
        [StringLength(30)]
        public string AuthorNickname { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; }

        [Required]
        public string ProductId { get; set; }

    }
}
