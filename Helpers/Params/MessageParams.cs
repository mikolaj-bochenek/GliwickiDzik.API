namespace GliwickiDzik.API.Helpers
{
    public class MessageParams
    {
        public const int MaxPageSize = 48;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 24;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        
        public int UserId { get; set; }
        public string MessageContainer { get; set; } = "Unread";
    }
}