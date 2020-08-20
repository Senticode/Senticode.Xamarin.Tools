namespace _template.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadApplication(new MasterDetail.App(UwpInitializer.Instance));
        }
    }
}