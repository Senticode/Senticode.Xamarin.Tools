namespace Senticode.Xamarin.Tools.MVVM.Interfaces
{
    /// <summary>
    ///     Represents a collection of object that can be grouped (hidden or not).
    /// </summary>
    public interface IGroupable
    {
        bool IsHidden { get; set; }
        bool IsActive { get; set; }
    }
}