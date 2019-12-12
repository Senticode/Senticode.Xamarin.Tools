using System;

namespace Senticode.Xamarin.Tools.MVVM.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiredViewAttribute : Attribute
    {
        public RequiredViewAttribute(Type contractType)
        {
            ContractType = contractType;
        }

        public Type ContractType { get; }
    }
}