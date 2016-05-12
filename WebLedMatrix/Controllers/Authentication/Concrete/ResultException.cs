using System;
using System.Collections.Generic;

public class ResultException : Exception
{
    public IEnumerable<string> ResultErrors { get; set; }
}