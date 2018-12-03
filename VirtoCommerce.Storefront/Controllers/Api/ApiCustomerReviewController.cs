using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Storefront.Infrastructure;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.CustomerReviews;
using VirtoCommerce.Storefront.Model.CustomerReviews.Services;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [ApiController]
    [StorefrontApiRoute("customer-reviews")]
    public class ApiCustomerReviewController : StorefrontControllerBase
    {
        private readonly ICustomerReviewService _customerReviewService;

        public ApiCustomerReviewController(IWorkContextAccessor workContextAccessor, IStorefrontUrlBuilder urlBuilder,
            ICustomerReviewService customerReviewService)
            : base(workContextAccessor, urlBuilder)
        {
            _customerReviewService = customerReviewService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReview([FromBody] [Required] CustomerReviewRequest customerReview)
        {
            await _customerReviewService.CreateReviewAsync(customerReview);
            return Created(string.Empty, null); // TODO Do we need Created or Ok is enough
        }
    }
}
