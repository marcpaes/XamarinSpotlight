using UIKit;

namespace SpotlightSample
{
    public partial class MainVC : UIViewController
    {
        public MainVC() : base("MainVC", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            		
			BtnShowHint.TouchUpInside += (sender, e) =>
			{
				PresentViewController(new Layer()
				{
					Alpha = 0.5F
				}, true, null);
			};
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

