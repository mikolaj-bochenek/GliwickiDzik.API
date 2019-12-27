namespace GliwickiDzik.API.Helpers
{
    public class CommentParams
    {
        public int MaxPageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public int TrainingPlanId { get; set; }
        public string OrderBy { get; set; }
    }
}