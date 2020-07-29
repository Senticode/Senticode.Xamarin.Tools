using Xamarin.Forms;

namespace Template.WPF
{
    internal partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Forms.SetFlags("RadioButton_Experimental");
            Forms.Init();
            LoadApplication(new Template.MasterDetail.App(WpfInitializer.Instance));
        }
    }
}