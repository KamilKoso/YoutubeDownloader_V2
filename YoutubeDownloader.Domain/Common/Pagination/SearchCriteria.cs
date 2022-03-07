namespace YoutubeDownloader.Domain.Common.Pagination
{
    public class SearchCriteria
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public bool IsAscending { get; set; }
        public string SearchBy { get; set; }
        public string SearchValue { get; set; }


        public SearchCriteria()
        {
            PageNumber = 1;
        }

        public SearchCriteria(int pageNumber)
        {
            PageNumber = pageNumber;
        }

        public SearchCriteria(int pageNumber, int pageSize) : this(pageNumber)
        {
            PageSize = pageSize;
        }
    }
}
