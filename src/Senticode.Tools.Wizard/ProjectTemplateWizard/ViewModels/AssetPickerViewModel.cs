using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using ProjectTemplateWizard.Abstractions;
using ProjectTemplateWizard.Constants;
using ProjectTemplateWizard.Core;
using Color = System.Windows.Media.Color;

namespace ProjectTemplateWizard.ViewModels
{
    internal class AssetPickerViewModel : ObservableObjectBase
    {
        private readonly int _assetMaxSize;
        private readonly int _assetMinSize;
        private readonly string _pickerDialogTitle;

        public AssetPickerViewModel()
        {
        }

        public AssetPickerViewModel(string title, string pickerDialogTitle, int assetMaxSize, int assetMinSize)
        {
            _pickerDialogTitle = pickerDialogTitle;
            _assetMaxSize = assetMaxSize;
            _assetMinSize = assetMinSize;
            Title = title;
            SelectedColor = Colors.Transparent;
        }

        public string Title { get; }

        #region SelectedColor: Color

        public Color SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }

        private Color _selectedColor;

        #endregion

        #region AssetPath: string

        public string AssetPath
        {
            get => _assetPath;
            set => SetProperty(ref _assetPath, value);
        }

        private string _assetPath;

        #endregion

        #region PickAsset command

        public Command PickAssetCommand => _pickAssetCommand ??=
            new Command(ExecutePickAsset, () => true);

        private Command _pickAssetCommand;

        private void ExecutePickAsset()
        {
            PickAssetCommand.Disable();

            try
            {
                var dialog = new OpenFileDialog
                {
                    Title = _pickerDialogTitle,
                    InitialDirectory = Locations.MyPictures,
                    Filter = DialogFilters.Png
                };

                if (!(dialog.ShowDialog() ?? false))
                {
                    return;
                }

                var path = dialog.FileName;
                using var bitmap = new Bitmap(path);

                if (bitmap.Width < _assetMinSize || bitmap.Height < _assetMinSize)
                {
                    MessageBox.Show(
                        $"Asset image should be square with min size {_assetMinSize}.",
                        "Too small image",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
                else if (bitmap.Width > _assetMaxSize || bitmap.Height > _assetMaxSize)
                {
                    MessageBox.Show(
                        $"Asset image should be square with max size {_assetMaxSize}.",
                        "Too big image",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
                else
                {
                    AssetPath = path;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{nameof(PickAssetCommand)}: {e.Message}");
            }
            finally
            {
                PickAssetCommand.Enable();
            }
        }

        #endregion
    }
}