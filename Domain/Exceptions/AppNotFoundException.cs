using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class AppNotFoundException:Exception
    {

        public object? Value { get; }
        public  AppNotFoundException(): base(ErrorMessages.ResourceNotFound) 
        {
            
        }
        public AppNotFoundException(object value)
        {
            Value = value;
        }
        public AppNotFoundException(string message):base(message)
        {

        }
    }
}
