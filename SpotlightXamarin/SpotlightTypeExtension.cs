using CoreGraphics;
using UIKit;

namespace Spotlight
{
    public static class SpotlightTypeExtension
    {
        public static CGPoint Center(this ISpotlightType spot)
        {
            return new CGPoint(x: spot.Frame.GetMidX(), y: spot.Frame.GetMidY());
        }

        public static UIBezierPath InfinitesmalPath(this ISpotlightType spot)
        {
            return UIBezierPath.FromRoundedRect(new CGRect(spot.Center, CGSize.Empty), cornerRadius: 0);
        }
    }
}