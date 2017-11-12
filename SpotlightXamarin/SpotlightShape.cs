using System;
using CoreGraphics;
using UIKit;

namespace Spotlight
{
    public class SpotlightShape
    {
        public class Oval : ISpotlightType
        {
            public CGPoint Center => this.Center();
            public UIBezierPath InfinitesmalPath => this.InfinitesmalPath();
            public UIBezierPath Path => UIBezierPath.FromRoundedRect(Frame, cornerRadius: Frame.Width / 2);

            public CGRect Frame { get; set; }

            public Oval(CGRect frame)
            {
                Frame = frame;
            }

            public Oval(CGPoint center, nfloat diameter)
            {
                Frame = new CGRect(x: center.X - diameter / 2, y: center.Y - diameter / 2, width: diameter, height: diameter);
            }

            public Oval(UIView view, nfloat margin)
            {
                var origin = view.Superview?.ConvertPointToCoordinateSpace(new CGPoint(view.Frame.GetMidX(), view.Frame.GetMidY()), view.Window.Screen.FixedCoordinateSpace);
                if (origin == null) return;
                var centerV = new CGPoint(x: origin.Value.X + view.Bounds.Width / 2, y: origin.Value.Y + view.Bounds.Height / 2);
                var diameter = Math.Max(view.Bounds.Width, view.Bounds.Height) + margin * 2;
                Frame = new CGRect(x: centerV.X - diameter / 2, y: centerV.Y - diameter / 2, width: diameter, height: diameter);
            }
        }

        public class Rect : ISpotlightType
        {
            public CGPoint Center => this.Center();
            public UIBezierPath InfinitesmalPath => this.InfinitesmalPath();
            public virtual UIBezierPath Path => UIBezierPath.FromRoundedRect(Frame, cornerRadius: 0);

            public CGRect Frame { get; set; }

            public Rect(CGRect framec)
            {
                Frame = framec;
            }

            public Rect(CGPoint center, CGSize size)
            {
                Frame = new CGRect(x: center.X - size.Width / 2, y: center.Y - size.Height / 2, width: size.Width, height: size.Height);
            }

            public Rect(UIView view, nfloat margin)
            {
                var viewOrigin = view.Superview?.ConvertPointToCoordinateSpace(new CGPoint(view.Frame.GetMidX(), view.Frame.GetMidY()), view.Window.Screen.FixedCoordinateSpace);
                if (viewOrigin == null) return;
                var origin = new CGPoint(x: viewOrigin.Value.X - margin, y: viewOrigin.Value.Y - margin);
                var size = new CGSize(width: view.Bounds.Width + margin * 2, height: view.Bounds.Height + margin * 2);
                Frame = new CGRect(origin, size);
            }

        }

        public class RoundedRect : Rect
        {
            private nfloat CornerRadius { get; set; }

            public RoundedRect(CGPoint center, CGSize size, nfloat cornerRadiusc) : base(center, size)
            {
                CornerRadius = cornerRadiusc;
            }

            public RoundedRect(UIView view, nfloat margin, nfloat cornerRadiusc) : base(view, margin)
            {
                CornerRadius = cornerRadiusc;
            }

            public override UIBezierPath Path => UIBezierPath.FromRoundedRect(Frame, cornerRadius: CornerRadius);

        }
    }
}