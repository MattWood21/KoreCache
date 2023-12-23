using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KoreCache.Models
{
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseContent { get; set; }
        public ApiErrorReason ErrorReason { get; set; }
        public Exception Exception { get; set; }
        public T Result { get; set; }
        public long ExecutionTime { get; set; }
    }

    public class ApiErrorReason
    {
        public string Error { get; set; }
    }
}
