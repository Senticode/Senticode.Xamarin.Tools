using System.Linq;
using Template.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomPageRenderer))]

namespace Template.iOS.Renderer
{
    public class CustomPageRenderer : NavigationRenderer
    {
        private NavigationPage NavPage => (NavigationPage) Element;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UpdateBarTextColor();
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            var toolbar = (e.NewElement as NavigationPage)?.CurrentPage?.ToolbarItems?.FirstOrDefault();

            if (toolbar != null)
            {
                toolbar.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(ToolbarItem.IconImageSource) ||
                        args.PropertyName == nameof(ToolbarItem.Icon))
                    {
                        MakeColorful();
                    }
                };
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            MakeColorful();
        }

        private void MakeColorful()
        {
            try
            {
                var logo = NavigationBar?.TopItem?.RightBarButtonItem?.Image;

                if (logo == null)
                {
                    return;
                }

                if (logo.RenderingMode == UIImageRenderingMode.AlwaysOriginal)
                {
                    return;
                }

                NavigationBar.TopItem.RightBarButtonItem.Image =
                    logo.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            }
            catch
            {
                //Console.WriteLine($"{nameof(CustomPageRenderer)}: {e}");
            }
        }

        private void UpdateBarTextColor()
        {
            var barTextColor = NavPage.BarTextColor;
            var titleTextAttributes = UINavigationBar.Appearance.GetTitleTextAttributes();

            var stringAttributes1 = new UIStringAttributes
            {
                Font = UIFont.BoldSystemFontOfSize(17),
                ForegroundColor = barTextColor.ToUIColor()
            };

            NavigationBar.TitleTextAttributes = stringAttributes1;

            if (!(NavigationBar.Items?.Length > 0))
            {
                return;
            }

            var header = NavigationBar.Items.FirstOrDefault();

            if (header?.LeftBarButtonItem != null)
            {
                //var font = UIFont.BoldSystemFontOfSize(16);
                //header.LeftBarButtonItem.SetTitleTextAttributes(new UITextAttributes
                //    {
                //        Font = font,
                //        TextColor = UIColor.FromRGB(41, 98, 255)
                //    },
                //    UIControlState.Normal);
                //header.LeftBarButtonItem.SetTitleTextAttributes(new UITextAttributes
                //    {
                //        Font = font,
                //        TextColor = UIColor.FromRGB(41, 98, 255).ColorWithAlpha(new nfloat(0.60))
                //    },
                //    UIControlState.Highlighted);
            }
        }
    }
}