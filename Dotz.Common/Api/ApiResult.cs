namespace Dotz.Common.Api
{
    #region ObjectResult

    public class ApiResult
    {
        public string Message { get; set; }

        public int StatusCode { get; set; }

        public object Body { get; set; }
    }

    #endregion ObjectResult

    #region GenericResult

    public class ApiResult<T>
    {
        public string Message { get; set; }

        public int StatusCode { get; set; }

        public T Body { get; set; }
    }

    #endregion GenericResult
}