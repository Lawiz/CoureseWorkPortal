using System;

namespace CourseWorksPortal.Models.OtherModels
{
    
    public class OperationResult
    {
        public OperationResult(bool res, string inf)
        {
            Result = res;
            Info = inf;
        }
        public bool Result { get; set; }
        public string Info { get; set; }
        public Exception InnerException { get; set; }
    }
}