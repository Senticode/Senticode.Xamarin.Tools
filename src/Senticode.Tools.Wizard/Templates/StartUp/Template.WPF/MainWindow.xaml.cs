using Xamarin.Forms;

namespace _template.WPF
{
    internal partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Forms.SetFlags("RadioButton_Experimental");
            Forms.Init();
            LoadApplication(new MasterDetail.App(WpfInitializer.Instance));
        }
    }
}