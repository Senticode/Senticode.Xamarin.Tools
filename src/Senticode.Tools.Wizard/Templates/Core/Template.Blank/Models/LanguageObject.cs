using System.Globalization;
using Senticode.Xamarin.Tools.MVVM.Abstractions;

namespace Template.Blank.Models
{
    public class LanguageObject : ModelBase
    {
        public LanguageObject(CultureInfo culture)
        {
            Culture = culture;
            Title = culture.DisplayName;
        }

        public CultureInfo Culture { get; }

        public override string ToString()
        {
            if (Title != null)
            {
                return Title;
            }

            return base.ToString();
        }

        #region Title

        /// <summary>
        ///     Gets or sets the Title value.
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        ///     Title property data.
        /// </summary>
        private string _title;

        #endregion

        #region IsChecked

        /// <summary>
        ///     Gets or sets the IsChecked value.
        /// </summary>
        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        /// <summary>
        ///     IsChecked property data.
        /// </summary>
        private bool _isChecked;

        #endregion
    }
}