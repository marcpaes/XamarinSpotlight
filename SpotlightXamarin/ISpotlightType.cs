using CoreGraphics;
using UIKit;

namespace Spotlight
{
    public interface ISpotlightType
    {
        CGRect Frame { get; set; }
        CGPoint Center { get; }
        UIBezierPath Path { get; }
        UIBezierPath InfinitesmalPath { get; }
    }
}