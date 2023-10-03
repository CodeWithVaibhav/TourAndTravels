using System.Net;

namespace TourAndTravels.Infrastucture
{
    public class ResponseContext<T>
    {
        public bool IsError { get; set; } = false;
        public string Error { get; private set; }
        public object ErrorObject { get; private set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public T Content { get; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get { return (int)Math.Ceiling((double)TotalCount / PageSize); } }

        public ResponseContext(string error, HttpStatusCode statusCode)
        {
            this.IsError = true;
            this.Error = error;
            this.HttpStatusCode = statusCode;
        }

        public ResponseContext(string error, T content, HttpStatusCode statusCode)
        {
            this.IsError = true;
            this.Error = error;
            this.Content = content;
            this.HttpStatusCode = statusCode;
        }

        public ResponseContext(object content, HttpStatusCode statusCode)
        {
            this.IsError = true;
            this.ErrorObject = content;
            this.HttpStatusCode = statusCode;
        }

        public ResponseContext(T content)
        {
            this.IsError = false;
            this.Content = content;
        }

        public ResponseContext(T content, int pageSize,
            int currentPage, int totalCount)
            : this(content)
        {
            this.PageSize = pageSize;
            this.CurrentPage = currentPage;
            this.TotalCount = totalCount;
        }

        public static ResponseContext<T> Success(T content)
        {
            return new ResponseContext<T>(content);
        }
        
        public static ResponseContext<T> ErrorHttpResponse(HttpStatusCode statusCode, object content)
        {
            return new ResponseContext<T>(content, statusCode);
        }
    }
}
