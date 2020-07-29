using System.Diagnostics;
using System.Windows.Input;
using Senticode.Xamarin.Tools.MVVM.Abstractions;

namespace Template.MasterDetail.ViewModels.Abstractions
{
    public class ActionViewModel : ViewModelBase<AppCommands, AppSettings>
    {
        private ICommand _command;
        private string _imagePath;
        private string _imagePathToActiveState;
        private string _imagePathToDisabledState;
        private bool _isActive;
        private bool _isEnabled;
        private object _parameter;

        public ActionViewModel(string title)
        {
            Title = title;
            _isEnabled = true;
            _isActive = false;
        }

        #region ImagePath property

        /// <summary>
        ///     Gets or sets the ImagePath value.
        /// </summary>
        public string ImagePath
        {
            get
            {
                if (_imagePath == null)
                {
                    SetImagePath();
                }

                return _imagePath;
            }
            set
            {
                if (value == _imagePath)
                {
                    return;
                }

                SetProperty(ref _imagePath, value);
            }
        }

        #endregion

        #region Parameter property

        /// <summary>
        ///     Gets or sets the Parameter value.
        /// </summary>
        public object Parameter
        {
            get => _parameter;
            set => SetProperty(ref _parameter, value);
        }

        #endregion

        #region Command property

        /// <summary>
        ///     Gets or sets the Command value.
        /// </summary>
        public ICommand Command
        {
            get => _command;
            set => SetProperty(ref _command, value);
        }

        #endregion

        #region IsActive property

        /// <summary>
        ///     Gets or sets the IsActive value.
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (IsEnabled)
                {
                    if (value != _isActive)
                    {
                        SetProperty(ref _isActive, value);
                    }
                }
                else
                {
                    if (value != _isActive)
                    {
                        SetProperty(ref _isActive, value);
                        Debug.WriteLine(
                            $"{nameof(ActionViewModel)} {Title} {nameof(IsActive)} do not set value because {nameof(IsEnabled)}=false");
                    }
                }

                SetImagePath();
            }
        }

        #endregion

        #region IsEnabled property

        /// <summary>
        ///     Gets or sets the IsEnabled value.
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value == _isEnabled)
                {
                    return;
                }

                SetProperty(ref _isEnabled, value);
                SetImagePath();
            }
        }

        #endregion

        #region ImagePathToActiveState

        /// <summary>
        ///     Gets or sets the ImagePathToActiveState value.
        /// </summary>
        public string ImagePathToActiveState
        {
            get => _imagePathToActiveState;
            set => SetProperty(ref _imagePathToActiveState, value, SetImagePath);
        }

        #endregion

        #region ImagePathToDisabledState

        /// <summary>
        ///     Gets or sets the ImagePathToDisabledState value.
        /// </summary>
        public string ImagePathToDisabledState
        {
            get => _imagePathToDisabledState;
            set => SetProperty(ref _imagePathToDisabledState, value, SetImagePath);
        }

        #endregion

        private void SetImagePath()
        {
            if (_isActive)
            {
                if (ImagePathToActiveState != null)
                {
                    ImagePath = ImagePathToActiveState;
                }

                OnPropertyChanged(nameof(ImagePath));
            }
            else
            {
                if (ImagePathToDisabledState != null)
                {
                    ImagePath = ImagePathToDisabledState;
                }

                OnPropertyChanged(nameof(ImagePath));
            }
        }
    }
}