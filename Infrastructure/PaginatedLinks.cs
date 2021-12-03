namespace Assignment0.Infrastructure;

/*

The PagedList package you referenced from nuget is intended for .NET Framework, which is the crusty
old version of .NET. We don't know if it will work properly on new .NET (formerly called .NET Core), 
so let's avoid referencing such packages. Here's the compiler warning from the Output window:

    warning NU1701: Package 'PagedList 1.17.0' was restored using '.NETFramework,Version=v4.6.1, .NETFramework,Version=v4.6.2, .NETFramework,Version=v4.7, .NETFramework,Version=v4.7.1, .NETFramework,Version=v4.7.2, .NETFramework,Version=v4.8' 
      instead of the project target framework 'net6.0'. This package may not be fully compatible with your project.

I wrote this class for FFL Scope. Here's how you would use it:

//
// In the MediatR code:
//

// Get the first X pending FFL claims, showing the oldest first.
var claimRequestsQuery = _context.AccountFFLClaimRequests
    .Include(af => af.Account)
    .Include(af => af.FFL)
    .Where(af => af.Status == FFLClaimRequestStatus.Pending)
    .OrderBy(af => af.CreateUtc);

var (pagination, pendingRequests) = await PaginatedLinks.CreateAsync(claimRequestsQuery, message.Page ?? 1, PAGE_SIZE);

command.PendingRequests.AddRange(_mapper.Map<Command.PendingRequestModel[]>(pendingRequests));
command.Pagination = pagination;


//
// In the view:
//

@if (Model.Pagination.TotalPages > 1)
{
    <nav aria-label="...">
        <ul class="pagination">
            <li class="page-item @Html.ClassIf(!Model.Pagination.Previous.Display, "disabled")">
                <a class="page-link" asp-area="@Area.Admin" asp-action="Pending" asp-controller="FFLClaims" asp-route-page="@Model.Pagination.Previous.PageNumber" tabindex="-1">Previous</a>
            </li>
            @foreach (var page in Model.Pagination.Pages)
            {
                <li class="page-item @Html.ClassIf(page.IsCurrent, "disabled")">
                    <a class="page-link" asp-area="@Area.Admin" asp-action="Pending" asp-controller="FFLClaims" asp-route-page="@(page.PageNumber)" tabindex="-1">@(page.PageNumber)</a>
                </li>
            }
            <li class="page-item @Html.ClassIf(!Model.Pagination.Next.Display, "disabled")">
                <a class="page-link" asp-area="@Area.Admin" asp-action="Pending" asp-controller="FFLClaims" asp-route-page="@Model.Pagination.Next.PageNumber" tabindex="-1">Next</a>
            </li>
        </ul>
    </nav>
}


//
// CSS for the links
//

.pagination {
  display: -ms-flexbox;
  display: flex;
  padding-left: 0;
  list-style: none;
  border-radius: 0.25rem;
}

.page-item:first-child .page-link {
  margin-left: 0;
  border-top-left-radius: 0.25rem;
  border-bottom-left-radius: 0.25rem;
}

.page-item:last-child .page-link {
  border-top-right-radius: 0.25rem;
  border-bottom-right-radius: 0.25rem;
}

.page-item.active .page-link {
  z-index: 2;
  color: #fff;
  background-color: #007bff;
  border-color: #007bff;
}

.page-item.disabled .page-link {
  color: #868e96;
  pointer-events: none;
  background-color: #fff;
  border-color: #ddd;
}

.page-link {
  position: relative;
  display: block;
  padding: 0.5rem 0.75rem;
  margin-left: -1px;
  line-height: 1.25;
  color: #007bff;
  background-color: #fff;
  border: 1px solid #ddd;
}

.page-link:focus, .page-link:hover {
  color: #0056b3;
  text-decoration: none;
  background-color: #e9ecef;
  border-color: #ddd;
}

.pagination-lg .page-link {
  padding: 0.75rem 1.5rem;
  font-size: 1.25rem;
  line-height: 1.5;
}

.pagination-lg .page-item:first-child .page-link {
  border-top-left-radius: 0.3rem;
  border-bottom-left-radius: 0.3rem;
}

.pagination-lg .page-item:last-child .page-link {
  border-top-right-radius: 0.3rem;
  border-bottom-right-radius: 0.3rem;
}

.pagination-sm .page-link {
  padding: 0.25rem 0.5rem;
  font-size: 0.875rem;
  line-height: 1.5;
}

.pagination-sm .page-item:first-child .page-link {
  border-top-left-radius: 0.2rem;
  border-bottom-left-radius: 0.2rem;
}

.pagination-sm .page-item:last-child .page-link {
  border-top-right-radius: 0.2rem;
  border-bottom-right-radius: 0.2rem;
}

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represent paging links to display on a page, like in Google search results. Show up to
/// 5 page links at a time, plus Previous and Next.
/// </summary>
public class PaginatedLinks
{
    /// <summary>
    /// Number of records per page. Default value used if the caller passes in an unreasonable value.
    /// </summary>
    private const int DEFAULT_PAGE_SIZE = 10;

    /// <summary>
    /// Max allowable number of records per page.
    /// </summary>
    private const int MAX_PAGE_SIZE = 100;

    /// <summary>
    /// Number of pages visible for pagination. We'll create up to this many pages of data for the caller.
    /// </summary>
    private const int MAX_VISIBLE_PAGES = 5;


    /// <summary>
    /// Factory method that encapsulates counting records that match the query and doing paging with Skip/Take.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="currentPage">1-based page index.</param>
    /// <param name="pageSize">Number of records per page.</param>
    /// <returns>The paginated links, as well as the records returned by the query.</returns>
    public static async Task<(PaginatedLinks, T[] records)> CreateAsync<T>(IQueryable<T> query, int? currentPage, int pageSize)
    {
        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        currentPage = FixOutOfRangeCurrentPage(currentPage);
        pageSize = FixOutOfRangePageSize(pageSize);

        var totalRecords = await query.CountAsync();

        var records = await query
            .Skip((currentPage.Value - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync();

        return (new PaginatedLinks(currentPage.Value, pageSize, totalRecords), records);
    }

    private static int FixOutOfRangeCurrentPage(int? currentPage) => currentPage >= 1 ? currentPage.Value : 1;

    private static int FixOutOfRangePageSize(int pageSize)
    {
        if (pageSize < 1)
        {
            return DEFAULT_PAGE_SIZE;
        }

        if (pageSize > MAX_PAGE_SIZE)
        {
            return MAX_PAGE_SIZE;
        }

        return pageSize;
    }

    private static int FixOutOfRangeTotalRecords(int totalRecords) => totalRecords >= 1 ? totalRecords : 0;


    private readonly int _currentPage;
    private readonly int _pageSize;
    private readonly int _totalPages;
    private readonly int _totalRecords;

    public int CurrentPage => _currentPage;
    public int TotalPages => _totalPages;
    public int TotalRecords => _totalRecords;

    /// <summary>
    /// Data for rendering a &quot;previous&quot; page link.
    /// </summary>
    public PreviousPage Previous { get; private set; }

    /// <summary>
    /// Data to render a sliding window of page links. Displays up to 5 pages.
    /// </summary>
    public List<Page> Pages { get; private set; } = new List<Page>();

    /// <summary>
    /// Data for rendering a &quot;next&quot; page link.
    /// </summary>
    public NextPage Next { get; private set; }


    public PaginatedLinks(int currentPage, int pageSize, int totalRecords)
    {
        _currentPage = FixOutOfRangeCurrentPage(currentPage);
        _pageSize = FixOutOfRangePageSize(pageSize);

        _totalRecords = FixOutOfRangeTotalRecords(totalRecords);
        _totalPages = (int)Math.Ceiling(_totalRecords / (double)_pageSize);

        BuildPagingData();
    }

    private void BuildPagingData()
    {
        (PreviousPage prev, IReadOnlyCollection<Page> pages, NextPage next) pagingData;

        if (_totalRecords == 0)
        {
            // There are no records to page through, and thus nothing further to do.
            pagingData = BuildNoRecordsPagingData();
        }
        else if (_totalPages <= MAX_VISIBLE_PAGES)
        {
            // We have between 1 and 5 pages, inclusive. Start at 1 and show up to _totalPages.
            pagingData = BuildPartialPagingData(_currentPage, _totalPages);
        }
        else if (_currentPage <= (MAX_VISIBLE_PAGES - 2))
        {
            // We have more than MAX_VISIBLE_PAGES pages. We're somewhere in the first three pages. Show
            //   a "front of the list" representation, which is not yet shifted.
            pagingData = BuildFrontOfListPagingData(_currentPage);
        }
        else if (_currentPage > (_totalPages - (MAX_VISIBLE_PAGES - 2)))
        {
            // We have more than MAX_VISIBLE_PAGES pages. We're somewhere in the last three pages. Show
            //   an "end of the list" representation, which no longer shifts to the right.
            pagingData = BuildEndOfListPagingData(_currentPage, _totalPages);
        }
        else
        {
            // We have more than MAX_VISIBLE_PAGES pages. We're somewhere after the first three pages and
            //   before the last three pages (IOW, we're somewhere in the middle of the list). Show a shifted
            //   window of MAX_VISIBLE_PAGES pages.
            pagingData = BuildMiddleOfListPagingData(_currentPage);
        }

        Previous = pagingData.prev;
        Pages.AddRange(pagingData.pages);
        Next = pagingData.next;
    }

    /// <summary>
    /// There are no records; there's nothing to page. 
    /// </summary>
    /// <returns></returns>
    private (PreviousPage prev, IReadOnlyCollection<Page> pages, NextPage next) BuildNoRecordsPagingData()
    {
        var prev = new PreviousPage
        {
            Display = false,
            PageNumber = 1
        };

        var pages = Array.Empty<Page>();

        var next = new NextPage
        {
            Display = false,
            PageNumber = 1
        };

        return (prev, pages, next);
    }

    /// <summary>
    /// We have some data, but the number of pages is &lt;= the number of pages of links that we show. IOW,
    /// we have between 1 and 5 pages, inclusive. Start at 1 and initialize up to _totalPages.
    /// </summary>
    /// <param name="currentPage"></param>
    /// <param name="totalPages">Must be &lt;= <see cref="MAX_VISIBLE_PAGES"/>.</param>
    /// <returns></returns>
    private (PreviousPage prev, IReadOnlyCollection<Page> pages, NextPage next) BuildPartialPagingData(int currentPage, int totalPages)
    {
        // Assertion: totalPages <= MAX_VISIBLE_PAGES

        // Don't try to page beyond the largest number of pages.
        if (currentPage > totalPages)
        {
            currentPage = totalPages;
        }

        // Calculate the previous page index.
        var previousPage = currentPage - 1;
        if (previousPage < 1)
        {
            previousPage = 1;
        }

        var prev = new PreviousPage
        {
            Display = currentPage != previousPage,
            PageNumber = previousPage
        };

        // Create the numbered page links.
        var pages = new List<Page>();
        for (var ixPage = 1; ixPage <= totalPages; ixPage++)
        {
            pages.Add(new Page
            {
                IsCurrent = currentPage == ixPage,
                PageNumber = ixPage
            });
        }

        // Calculate the next page index.
        var nextPage = currentPage + 1;
        if (nextPage > totalPages)
        {
            nextPage = totalPages;
        }

        var next = new NextPage
        {
            Display = currentPage != nextPage,
            PageNumber = nextPage
        };

        return (prev, pages, next);
    }

    /// <summary>
    /// We have more than MAX_VISIBLE_PAGES pages. We're somewhere in the first three pages. Show a 
    /// &quot;front of the list&quot; representation, which is not yet shifted.
    /// </summary>
    /// <param name="currentPage"></param>
    /// <returns></returns>
    private (PreviousPage prev, IReadOnlyCollection<Page> pages, NextPage next) BuildFrontOfListPagingData(int currentPage)
    {
        // Assertion: totalPages > MAX_VISIBLE_PAGES
        // Assertion: currentPage is 1, 2, or 3 (i.e., <= (MAX_VISIBLE_PAGES - 2)).

        // Calculate the previous page index.
        var previousPage = currentPage - 1;
        if (previousPage < 1)
        {
            previousPage = 1;
        }

        var prev = new PreviousPage
        {
            Display = currentPage != previousPage,
            PageNumber = previousPage
        };

        // Create the numbered page links. We know we have at least 6 pages, and that we're at the front of
        //   the list of pages (currentPage is 1, 2, or 3), so just create pages 1-5.
        var pages = new List<Page>();
        for (var ixPage = 1; ixPage <= MAX_VISIBLE_PAGES; ixPage++)
        {
            pages.Add(new Page
            {
                PageNumber = ixPage,
                IsCurrent = ixPage == currentPage
            });
        }

        // Calculate the next page index. We know currentPage is 1, 2, or 3, and that we have at least 6 pages,
        //   so a plain increment is safe here.		
        var next = new NextPage
        {
            Display = true,
            PageNumber = currentPage + 1
        };

        return (prev, pages, next);
    }

    private (PreviousPage prev, IReadOnlyCollection<Page> pages, NextPage next) BuildEndOfListPagingData(int currentPage, int totalPages)
    {
        // Assertion: totalPages > MAX_VISIBLE_PAGES
        // Assertion: currentPage is last-2, last-1, or last

        // Don't try to page beyond the largest number of pages.
        if (currentPage > totalPages)
        {
            currentPage = totalPages;
        }

        // Calculate the previous page index. We know we have at least 6 pages, and that we're within the last
        //   three pages, so a plain decrement is safe here.
        var prev = new PreviousPage
        {
            Display = true,
            PageNumber = currentPage - 1
        };

        // Create the numbered page links. We know we have at least 6 pages, and we're within 3 pages of the end
        //   of the list, so create the last MAX_VISIBLE_PAGES paging links.
        var pages = new List<Page>();
        for (var ixPage = (MAX_VISIBLE_PAGES - 1); ixPage >= 0; ixPage--)
        {
            // Build up the last 5 pages: last-4, last-3, last-2, last-1, and last
            var page = totalPages - ixPage;

            pages.Add(new Page
            {
                PageNumber = page,
                IsCurrent = page == currentPage
            });
        }

        // Calculate the next page index.
        var nextPage = currentPage + 1;
        if (nextPage > totalPages)
        {
            nextPage = totalPages;
        }

        var next = new NextPage
        {
            Display = nextPage != currentPage,
            PageNumber = nextPage
        };

        return (prev, pages, next);
    }

    private (PreviousPage prev, IReadOnlyCollection<Page> pages, NextPage next) BuildMiddleOfListPagingData(int currentPage)
    {
        // Assertion: totalPages > MAX_VISIBLE_PAGES
        // Assertion: currentPage > 3 and currentPage <= [last-3]

        // Calculate the previous page index. We're in the middle of the list, so a 
        //   plain decrement is safe here.
        var prev = new PreviousPage
        {
            Display = true,
            PageNumber = currentPage - 1
        };

        // Display a shifted window of pages:
        //   * For odd window sizes, there are an equal number of leading and trailing pages 
        //     surrounding the current page.
        //   * For even window sizes, there is one more leading page than trailing pages. This
        //     is what Google does with its window size of 10.
        var half = MAX_VISIBLE_PAGES / 2;
        var leadingPages = half;
        var trailingPages = MAX_VISIBLE_PAGES % 2 != 0
            ? half
            : half - 1;

        var pages = new List<Page>(MAX_VISIBLE_PAGES);

        // Insert the leading pages, if any.
        for (var ixLeading = leadingPages; ixLeading > 0; ixLeading--)
        {
            pages.Add(new Page
            {
                PageNumber = currentPage - (ixLeading),
                IsCurrent = false
            });
        }

        // Insert the current page.
        pages.Add(new Page
        {
            PageNumber = currentPage,
            IsCurrent = true
        });

        // Insert the trailing pages, if any.
        for (var ixTrailing = 0; ixTrailing < trailingPages; ixTrailing++)
        {
            pages.Add(new Page
            {
                PageNumber = currentPage + (ixTrailing + 1),
                IsCurrent = false
            });
        }

        // Calculate the next page index. We're in the middle of the list, so a plain
        //   increment is safe here.
        var next = new NextPage
        {
            Display = true,
            PageNumber = currentPage + 1
        };

        return (prev, pages, next);
    }


    //
    // Classes
    //

    public class Page
    {
        public int PageNumber { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class PreviousNextPage
    {
        public int PageNumber { get; set; }
        public bool Display { get; set; }
    }

    public class PreviousPage : PreviousNextPage
    { }

    public class NextPage : PreviousNextPage
    { }
}
