using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;

namespace Senticode.Xamarin.Tools.Core.Abstractions.Base
{
    /// <summary>
    ///     Class that contains settings for the application.
    /// </summary>
    public abstract class AppSettingsBase : INotifyPropertyChanged, IAppComponentLocator
    {
        private const string FileName = "AppSettings.xml";
        private readonly string _file;

        protected AppSettingsBase() : this(ServiceLocator.Container)
        {
        }

        protected AppSettingsBase(IUnityContainer container)
        {
            Container = container;

            _file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), FileName);
        }

        public IUnityContainer Container { get; }

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Saves application settings.
        /// </summary>
        public virtual async Task<bool> SaveAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var writer = new StreamWriter(_file))
                    {
                        new XmlSerializer(GetType()).Serialize(writer, this);
                    }

                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return false;
                }
            });
        }

        /// <summary>
        ///     Loads application settings.
        /// </summary>
        public virtual async Task<bool> LoadAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    if (File.Exists(_file))
                    {
                        var xml = File.ReadAllText(_file);
                        using (var reader = new StringReader(xml))
                        {
                            var settings = (AppSettingsBase) new XmlSerializer(GetType()).Deserialize(reader);
                            return ApplySettings(settings);
                        }
                    }

                    return false;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return false;
                }
            });
        }

        protected abstract bool ApplySettings(AppSettingsBase settings);


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}