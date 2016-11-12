using System;

namespace WebLedMatrix.Controllers
{
    public static class NullExpressionsExtensions
    {
        public static void ThrowIf<TExceptionType,TIn>(this TIn toTest,Predicate<TIn> predicate,Func<TExceptionType> exceptionExpression) where TExceptionType : Exception, new()
        {
            if (predicate.Invoke(toTest))
            {
                throw (exceptionExpression.Invoke());
            }
        }

        public static void ThrowIfNull<TExceptionType,TIn>(TIn variableToTest, Func<TExceptionType> exceptionFunc ) where TExceptionType : Exception, new()
        {
            if (variableToTest == null)
            {
                throw exceptionFunc.Invoke();
            }
        }

        public static void ThrowIfNull<TIn>(TIn variableToTest, string nameOfVariable = null) 
        {
            ThrowIfNull<ArgumentNullException,TIn>(variableToTest,()=> new ArgumentNullException(nameOfVariable ?? ""));
        }
    }
}