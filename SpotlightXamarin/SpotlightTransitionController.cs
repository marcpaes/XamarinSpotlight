﻿using System;
using CoreAnimation;
using UIKit;

namespace Spotlight
{
	public delegate void SpotlightTransitionWillPresentDelegate(SpotlightTransitionController controller, IUIViewControllerContextTransitioning transitionContext);
	public delegate void SpotlightTransitionWillDismissDelegate(SpotlightTransitionController controller, IUIViewControllerContextTransitioning transitionContext);

	public class SpotlightTransitionController : UIViewControllerAnimatedTransitioning
	{
		public bool IsPresent;

		public SpotlightTransitionWillPresentDelegate SpotlightTransitionWillPresent;
		public SpotlightTransitionWillDismissDelegate SpotlightTransitionWillDismiss;


		public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
		{
			if (IsPresent) {
				AnimateTransitionForPresent(transitionContext);
			}
			else
			{
				AnimateTransitionForDismiss(transitionContext);
			}
		}

		public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
		{
			return transitionContext.IsAnimated ? 0.25 : 0;
		}

		private void AnimateTransitionForPresent(IUIViewControllerContextTransitioning transitionContext)
		{
			var source = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
			var destination = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);

			transitionContext.ContainerView.InsertSubviewAbove(destination.View, source.View);
			destination.View.Alpha = 0;
			source.ViewWillDisappear(true);
			destination.ViewWillAppear(true);

			var duration = TransitionDuration(transitionContext);
			CATransaction.Begin();

			CATransaction.CompletionBlock = () =>
			{
				transitionContext.CompleteTransition(true);
			};

			UIView.Animate(duration, 0, new UIViewAnimationOptions(),
				() =>
				{
					destination.View.Alpha = (nfloat)1.0;
				}, () =>
				{
					destination.ViewDidAppear(true);
					source.ViewDidDisappear(true);
				});

			SpotlightTransitionWillPresent?.Invoke(this, transitionContext);
    
			CATransaction.Commit();

		}

		private void AnimateTransitionForDismiss(IUIViewControllerContextTransitioning transitionContext)
		{
			var source = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
			var destination = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);

			source.ViewWillDisappear(true);
			destination.ViewWillAppear(true);

			var duration = TransitionDuration(transitionContext);

			CATransaction.Begin();

			CATransaction.CompletionBlock = () =>
			{
				transitionContext.CompleteTransition(true);
			};

			UIView.Animate(duration, 0, new UIViewAnimationOptions(),
				animation: () =>
				{
                    source.View.Alpha = 0;
				}, completion: () =>
				{
					destination.ViewDidAppear(true);
					source.ViewDidDisappear(true);
				});

			SpotlightTransitionWillDismiss?.Invoke(this, transitionContext);

			CATransaction.Commit();
		}

	}
}