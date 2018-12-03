using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PagedList.Core;
using VirtoCommerce.Storefront.AutoRestClients.CustomerReviews.WebModuleApi;
using VirtoCommerce.Storefront.AutoRestClients.CustomerReviews.WebModuleApi.Models;
using VirtoCommerce.Storefront.Extensions;
using VirtoCommerce.Storefront.Infrastructure;
using VirtoCommerce.Storefront.Model.Caching;
using VirtoCommerce.Storefront.Model.Common.Caching;
using VirtoCommerce.Storefront.Model.CustomerReviews;
using VirtoCommerce.Storefront.Model.CustomerReviews.Services;
using VirtoCommerce.Storefront.Model.Modules.Services;
using CustomerReviewModel = VirtoCommerce.Storefront.Model.CustomerReviews.CustomerReview;
using CustomerReviewSearchCriteria = VirtoCommerce.Storefront.Model.CustomerReviews.CustomerReviewSearchCriteria;
using CustomerReviewDto = VirtoCommerce.Storefront.AutoRestClients.CustomerReviews.WebModuleApi.Models.CustomerReview;

namespace VirtoCommerce.Storefront.Domain.CustomerReview
{
    public class CustomerReviewService : ICustomerReviewService
    {
        private readonly ICustomerReviews _customerReviewsApi;
        private readonly IStorefrontMemoryCache _memoryCache;
        private readonly IApiChangesWatcher _apiChangesWatcher;

        public CustomerReviewService(
            ICustomerReviews customerReviewsApi,
            IStorefrontMemoryCache memoryCache,
            IApiChangesWatcher apiChangesWatcher)
        {
            _customerReviewsApi = customerReviewsApi;
            _memoryCache = memoryCache;
            _apiChangesWatcher = apiChangesWatcher;
        }

        public async Task CreateReviewAsync(CustomerReviewRequest review)
        {
            var reviewDto = review.ToCustomerReviewDto();
            try
            {
                await _customerReviewsApi.UpdateAsync(new List<CustomerReviewDto> {reviewDto});
            }
            catch(Exception ex)
            {
//                _telemetryClient.TrackException(ex);
            }
        }

        public IPagedList<CustomerReviewModel> SearchReviews(CustomerReviewSearchCriteria criteria)
        {
            return SearchReviewsAsync(criteria).GetAwaiter().GetResult();
        }

        public async Task<IPagedList<CustomerReviewModel>> SearchReviewsAsync(CustomerReviewSearchCriteria criteria)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(SearchReviewsAsync), criteria.GetCacheKey());
            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, SearchReviewsFactory(criteria));
        }

        private Func<ICacheEntry, Task<IPagedList<CustomerReviewModel>>> SearchReviewsFactory(
            CustomerReviewSearchCriteria criteria)
        {
            return async cacheEntry =>
            {
                cacheEntry.AddExpirationToken(CustomerReviewCacheRegion.CreateChangeToken());
                cacheEntry.AddExpirationToken(_apiChangesWatcher.CreateChangeToken());

                var response = await TrySearchCustomerReviewsAsync(criteria);
                return new StaticPagedList<CustomerReviewModel>(
                    response.Results.Select(CustomerReviewConverter.ToCustomerReview),
                    criteria.PageNumber,
                    criteria.PageSize,
                    response.TotalCount ?? 0);
            };
        }

        private async Task<GenericSearchResultCustomerReview> TrySearchCustomerReviewsAsync(
            CustomerReviewSearchCriteria criteria)
        {
            try
            {
                return await _customerReviewsApi.SearchCustomerReviewsAsync(criteria.ToSearchCriteriaDto());
            }
            catch
            {
                return new GenericSearchResultCustomerReview(0, new List<CustomerReviewDto>());
            }
        }
    }
}
