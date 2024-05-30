namespace SharedResponse.Parameters
    {
    public class PagenationRequestParameter
        {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PagenationRequestParameter()
            {
            PageNumber = 1;
            PageSize = 20;
            }
        public PagenationRequestParameter(int pageNumber, int pageSize)
            {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;
            }
        }
    }
