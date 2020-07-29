namespace Template.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadApplication(new Template.MasterDetail.App(UwpInitializer.Instance));
        }
    }
}