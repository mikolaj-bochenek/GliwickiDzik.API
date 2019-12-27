namespace GliwickiDzik.API.Helpers
{
    public class UserParams
    {
        public int MaxPageSize { get; set; } = 48;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 24;
        public int PageSize
        {
            get { return _pageSize = 24; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        
    }
}