using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CustomerReviewConverter
    {
        public static CustomerReview ToShopifyModel(this Storefront.Model.CustomerReviews.CustomerReview review)
        {
            var converter = new ShopifyModelConverter();
            return converter.ToLiquidCustomerReview(review);
        }
    }

    public partial class ShopifyModelConverter
    {
        public virtual CustomerReview ToLiquidCustomerReview(
            Storefront.Model.CustomerReviews.CustomerReview customerReview)
        {
            var result = new CustomerReview
            {
                AuthorNickname = customerReview.AuthorNickname,
                Content = customerReview.Content,
                IsActive = customerReview.IsActive,
                ProductId = customerReview.ProductId,

                CreatedDate = customerReview.CreatedDate
            };

            return result;
        }
    }
}
