using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }
}
