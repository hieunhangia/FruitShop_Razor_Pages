using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Orders;
using Service.DTOs;
using Service.DTOs.CustomerSupport;

namespace Service.CustomerSupport
{
    public class SupportService(AppDbContext context, SupportMapper mapper)
    {
        public async Task<PagedAndSortedDto<CommentSummaryDto>> GetAssignedCommentListAsync(int supportId, PagedAndSortedRequest<CommentFilterDto> pagedAndSortedRequest)
        {
           
            var query = context.ProductReviews
                .Include(pr => pr.Customer)
                .Where(pr => pr.AssignedCustomerSupportId == supportId);

            var searchContent = pagedAndSortedRequest.Filter.SearchContent?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(searchContent))
            {
                query = query.WhereContainsUnaccent(searchContent, pr => pr.Comment!);
            }

            var startDateFilter = pagedAndSortedRequest.Filter.StartDate ?? DateTime.MinValue;
            var endDateFilter = pagedAndSortedRequest.Filter.EndDate ?? DateTime.MaxValue;
            query = query.Where(pr => pr.CreatedAt >= startDateFilter && pr.CreatedAt <= endDateFilter);

            if (pagedAndSortedRequest.Filter.CommentClassification.HasValue)
            {
                query = query.Where(pr => pr.CommentClassification == pagedAndSortedRequest.Filter.CommentClassification.Value);
            }
            if (pagedAndSortedRequest.Filter.Rating.HasValue)
            {
                query = query.Where(pr => pr.Rating == pagedAndSortedRequest.Filter.Rating.Value);
            }

            pagedAndSortedRequest.SortColumn ??= nameof(ProductReview.CreatedAt);
            pagedAndSortedRequest.SortDirection ??= SortDirection.Descending;

            int totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PagedAndSortedDto<CommentSummaryDto>([], 0, pagedAndSortedRequest.PageIndex,
                pagedAndSortedRequest.PageSize, pagedAndSortedRequest.SortColumn, pagedAndSortedRequest.SortDirection.Value);
            }

            var dbItems = await query
                    .DynamicOrderBy(pagedAndSortedRequest.SortColumn, pagedAndSortedRequest.SortDirection.Value)
                    .ApplyPaging(pagedAndSortedRequest.PageIndex, pagedAndSortedRequest.PageSize)
                    .ToListAsync(); // <-- Dữ liệu thô (List<ProductReview>) đã về tới RAM ở đây

            var finalMappedItems = mapper.ToCommentSummaryDtoList(dbItems);

            return new PagedAndSortedDto<CommentSummaryDto>(finalMappedItems, totalCount, pagedAndSortedRequest.PageIndex,
                pagedAndSortedRequest.PageSize, pagedAndSortedRequest.SortColumn, pagedAndSortedRequest.SortDirection.Value);
        }
    }
}