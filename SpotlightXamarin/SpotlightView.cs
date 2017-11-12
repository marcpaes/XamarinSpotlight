﻿using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Spotlight
{
	public class SpotlightView : UIView
	{
		public static nfloat DefaultAnimateDuration = (nfloat)0.25;

		private readonly CAShapeLayer _maskLayer = new CAShapeLayer
		{
			FillRule = CAShapeLayer.FillRuleEvenOdd,
			FillColor = UIColor.Black.CGColor
		};


		public ISpotlightType Spotlight;

		public SpotlightView()
		{
			CommonInit();
		}

		public SpotlightView(CGRect frame) : base(frame)
		{
			CommonInit();
		}

		public SpotlightView(NSCoder coder) : base(coder)
		{
			CommonInit();
		}

		private void CommonInit()
		{
			Layer.Mask = _maskLayer;

		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			_maskLayer.Frame = Frame;
		}


		public void Appear(ISpotlightType spotlightp, nfloat duration)
		{
			_maskLayer.AddAnimation(AppearAnimation(duration, spotlightp), null);
			Spotlight = spotlightp;
		}

		public void Disappear(nfloat duration)
		{
			_maskLayer.AddAnimation(DisappearAnimation(duration), null);
		}

		public void Move(ISpotlightType toSpotlight, nfloat duration, SpotlightMoveType moveType = SpotlightMoveType.Direct)
		{
			switch (moveType) {
				case SpotlightMoveType.Direct:
					MoveDirect(toSpotlight, duration: duration);
					break;
				case SpotlightMoveType.Disappear:
					MoveDisappear(toSpotlight, duration: duration);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
			}
		}

		public void MoveDirect(ISpotlightType toSpotlight, nfloat duration) {
			_maskLayer.AddAnimation(MoveAnimation(duration, toSpotlight: toSpotlight), null);
			Spotlight = toSpotlight;
		}

		public void MoveDisappear(ISpotlightType toSpotlight, nfloat duration) {
			CATransaction.Begin();

			CATransaction.CompletionBlock = () =>
			{
				Appear(toSpotlight, duration: duration);
				Spotlight = toSpotlight;
			};

			Disappear(duration);
			CATransaction.Commit();
		}

		private UIBezierPath MaskPath(UIBezierPath path)
		{
            var p2 = UIBezierPath.FromRect(Frame);
			p2.AppendPath(path);
			return p2;
		}

		private CAAnimation AppearAnimation(nfloat duration, ISpotlightType spotlightp)
		{
			var beginPath = MaskPath(spotlightp.InfinitesmalPath);
			var endPath = MaskPath(spotlightp.Path);
			return PathAnimation(duration, beginPath, endPath);

		}

		private CAAnimation DisappearAnimation(nfloat duration)
		{
			var endPath = MaskPath(Spotlight.InfinitesmalPath);
			return PathAnimation(duration, null, endPath);

		}

		private CAAnimation MoveAnimation(nfloat duration, ISpotlightType toSpotlight)
		{
			var endPath = MaskPath(toSpotlight.Path);
			return PathAnimation(duration, beginPath: null, endPath: endPath);
		}

		private static CAAnimation PathAnimation(nfloat duration, UIBezierPath beginPath, UIBezierPath endPath) 
		{
			var animation = CABasicAnimation.FromKeyPath("path");
			animation.Duration = duration;
			animation.TimingFunction = new CAMediaTimingFunction(0.66F, 0, 0.33F, 1);

			var path = beginPath;
			if (path != null)
				animation.SetFrom(path.CGPath);
        

			animation.SetTo(endPath.CGPath);
			animation.RemovedOnCompletion = false;
			animation.FillMode = CAFillMode.Forwards;

			return animation;
		}
	}
}