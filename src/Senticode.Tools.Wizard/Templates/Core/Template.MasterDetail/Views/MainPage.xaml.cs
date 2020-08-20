﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _template.MasterDetail.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            if (IsPresented)
            {
                return base.OnBackButtonPressed();
            }

            return true;
        }
    }
}