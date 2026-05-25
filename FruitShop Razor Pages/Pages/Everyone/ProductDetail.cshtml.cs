using System.ComponentModel.DataAnnotations;
using FruitShop_Razor_Pages.Extensions;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Service.Customer;
using Service.DTOs.Everyone.Product;
using Service.Everyone;

namespace FruitShop_Razor_Pages.Pages.Everyone;

public class ProductDetail(
    ProductService productService,
    CartService cartService,
    [FromKeyedServices("CustomImage")] MarkdownPipeline customImageMarkdownPipeline) : PageModel
{
    public ProductDetailDto? Product { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Vui lòng nhập số lượng hợp lệ.")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
    public int AddToCartQuantity { get; set; } = 1;

    public MarkdownPipeline CustomImageMarkdownPipeline { get; set; } = customImageMarkdownPipeline;

    public async Task<IActionResult> OnGetAsync(int productId)
    {
        int? customerId = null;
        if (User.IsAuthenticated())
        {
            customerId = User.GetUserId();
        }

        Product = await productService.GetProductDetailAsync(productId, customerId,
            BusinessRuleConstants.ProductDetailPageValue.NumberOfTopProductReview);
        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(int productId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Một số đầu vào không hợp lệ. Vui lòng kiểm tra lại.";
                return RedirectToPage(new { productId });
            }

            if (!User.IsAuthenticated())
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng.";
                return RedirectToPage(new { productId });
            }

            var customerId = User.GetUserId();
            await cartService.AddProductToCartAsync(customerId, productId, AddToCartQuantity);
            TempData["SuccessMessage"] = $"Đã thêm {AddToCartQuantity} sản phẩm vào giỏ hàng.";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { productId });
    }

    public async Task<IActionResult> OnPostBuyNowAsync(int productId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Một số đầu vào không hợp lệ. Vui lòng kiểm tra lại.";
                return RedirectToPage(new { productId });
            }

            if (!User.IsAuthenticated())
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để mua sản phẩm.";
                return RedirectToPage(new { productId });
            }

            var customerId = User.GetUserId();
            await cartService.SelectCartItemForBuyNowAsync(customerId, productId, AddToCartQuantity);
            return RedirectToPage("/Customer/Checkout");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return RedirectToPage(new { productId });
        }
    }
}