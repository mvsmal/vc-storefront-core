using VirtoCommerce.Storefront.Model.CustomerReviews;
using ReviewDto = VirtoCommerce.Storefront.AutoRestClients.CustomerReviews.WebModuleApi.Models;
using CustomerReviewModel = VirtoCommerce.Storefront.Model.CustomerReviews.CustomerReview;
using CustomerReviewSearchCriteriaModel = VirtoCommerce.Storefront.Model.CustomerReviews.CustomerReviewSearchCriteria;

namespace VirtoCommerce.Storefront.Domain.CustomerReview
{
    public static class CustomerReviewConverter
    {
        public static CustomerReviewModel ToCustomerReview(this ReviewDto.CustomerReview itemDto)
        {
            var result = new CustomerReviewModel
            {
                Id = itemDto.Id,
                AuthorNickname = itemDto.AuthorNickname,
                Content = itemDto.Content,
                CreatedBy = itemDto.CreatedBy,
                CreatedDate = itemDto.CreatedDate,
                IsActive = itemDto.IsActive,
                ModifiedBy = itemDto.ModifiedBy,
                ModifiedDate = itemDto.ModifiedDate,
                ProductId = itemDto.ProductId
            };

            return result;
        }

        public static ReviewDto.CustomerReview ToCustomerReviewDto(this CustomerReviewRequest model)
        {
            var result = new ReviewDto.CustomerReview
            {
                AuthorNickname = model.AuthorNickname,
                Content = model.Content,
                ProductId = model.ProductId
            };

            return result;
        }

        public static ReviewDto.CustomerReviewSearchCriteria ToSearchCriteriaDto(
            this CustomerReviewSearchCriteriaModel criteria)
        {
            var result = new ReviewDto.CustomerReviewSearchCriteria
            {
                IsActive = criteria.IsActive,
                ProductIds = criteria.ProductIds,

                Skip = criteria.Start,
                Take = criteria.PageSize,
                Sort = criteria.Sort
            };

            return result;
        }
    }
}
