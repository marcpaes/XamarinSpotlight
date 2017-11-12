using System;
using System.Linq;
using Foundation;
using UIKit;

namespace Spotlight
{
    public delegate void SpotlightViewControllerWillPresentDelegate(SpotlightViewController controller, bool animated);
    public delegate void SpotlightViewControllerWillDismissDelegate(SpotlightViewController controller, bool animated);
    public delegate void SpotlightViewControllerTappedDelegate(SpotlightViewController controller, bool isInsideSpotlight);

    public class SpotlightViewController : UIViewController, IUIViewControllerTransitioningDelegate
    {
        public SpotlightViewControllerWillPresentDelegate SpotlightViewControllerWillPresent;
        public SpotlightViewControllerWillDismissDelegate SpotlightViewControllerWillDismiss;
        public SpotlightViewControllerTappedDelegate SpotlightViewControllerTapped;

        private SpotlightTransitionController _transitionController;

        public SpotlightView SpotlightView = new SpotlightView();

        private readonly UIView _contentView = new UIView();

        public nfloat Alpha = 0.5F;

        public SpotlightViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            CommonInit();
        }

        public SpotlightViewController(NSCoder coder) : base(coder)
        {
            CommonInit();
        }

        private void CommonInit()
        {
            _transitionController = new SpotlightTransitionController();
            _transitionController.SpotlightTransitionWillPresent += SpotlightTransitionWillPresent;
            _transitionController.SpotlightTransitionWillDismiss += SpotlightTransitionWillDismiss;

            ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;

            TransitioningDelegate = this;
        }

        public override void ViewDidLoad()
        {
            View.Frame = UIScreen.MainScreen.Bounds;
            base.ViewDidLoad();

            SetupSpotlightView(Alpha);
            SetupContentView();
            SetupTapGesture();

            View.BackgroundColor = UIColor.Clear;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            View.SetNeedsLayout();
            View.LayoutIfNeeded();
        }


        private void SetupSpotlightView(nfloat alphap)
        {
            SpotlightView.Frame = View.Bounds;
            SpotlightView.BackgroundColor = new UIColor(red: 0, green: 0, blue: 0, alpha: alphap);
            SpotlightView.UserInteractionEnabled = false;

			View.InsertSubview(SpotlightView, 0);

            var constraints = new[] {
                NSLayoutAttribute.Top,
                NSLayoutAttribute.Bottom,
                NSLayoutAttribute.Left,
                NSLayoutAttribute.Right
            };

            View.AddConstraints(
                constraints
                    .Select(x => NSLayoutConstraint.Create(View, x, NSLayoutRelation.Equal, SpotlightView, x, 1, 0))
                    .ToArray());
        }

        private void SetupContentView()
        {
            _contentView.Frame = View.Bounds;
            _contentView.BackgroundColor = UIColor.Clear;
            View.AddSubview(_contentView);

            var constraints = new[] {
                NSLayoutAttribute.Top,
                NSLayoutAttribute.Bottom,
                NSLayoutAttribute.Left,
                NSLayoutAttribute.Right
            };

            View.AddConstraints(
                constraints
                    .Select(x => NSLayoutConstraint.Create(View, x, NSLayoutRelation.Equal, _contentView, x, 1, 0))
                    .ToArray());
        }

        private void SetupTapGesture()
        {
            var gesture = new UITapGestureRecognizer(ViewTapped);
            View.AddGestureRecognizer(gesture);
        }

        private void ViewTapped(UITapGestureRecognizer gesture)
        {

            var touchPoint = gesture.LocationInView(SpotlightView);
            var isInside = SpotlightView?.Spotlight?.Frame.Contains(touchPoint) ?? false;
            SpotlightViewControllerTapped?.Invoke(this, isInside);
        }

        private void SpotlightTransitionWillDismiss(SpotlightTransitionController controller, IUIViewControllerContextTransitioning transitionContext)
        {
            SpotlightViewControllerWillDismiss?.Invoke(this, transitionContext.IsAnimated);
        }

        private void SpotlightTransitionWillPresent(SpotlightTransitionController controller, IUIViewControllerContextTransitioning transitionContext)
        {
            SpotlightViewControllerWillPresent?.Invoke(this, transitionContext.IsAnimated);
        }

        [Export("animationControllerForPresentedController:presentingController:sourceController:")]
        public IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
        {
            _transitionController.IsPresent = true;
            return _transitionController;
        }

        [Export("animationControllerForDismissedController:")]
        public IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
        {
            _transitionController.IsPresent = false;
            return _transitionController;
        }
    }
}