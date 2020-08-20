using _template.MasterDetail.Commands.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _template.MasterDetail.Views.Menu.MenuElements
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitleMenuRegion : ContentView
    {
        public TitleMenuRegion()
        {
            InitializeComponent();
        }

        #region PreviousMenu

        /// <summary>
        ///     Gets or sets the PreviousMenu value.
        /// </summary>
        public MenuKind PreviousMenu
        {
            get => (MenuKind) GetValue(PreviousMenuProperty);
            set => SetValue(PreviousMenuProperty, value);
        }

        /// <summary>
        ///     PreviousMenu property data.
        /// </summary>
        public static readonly BindableProperty PreviousMenuProperty =
            BindableProperty.Create(nameof(PreviousMenu), typeof(MenuKind), typeof(TitleMenuRegion), default(MenuKind));

        #endregion
    }
}