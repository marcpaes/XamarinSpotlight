﻿using CoreGraphics;
using Spotlight;
using UIKit;

namespace SpotlightSample
{
    public partial class Layer : SpotlightViewController
    {
        private int _stepIndex;

        public Layer() : base("Layer", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SpotlightViewControllerWillPresent += Layer_SpotlightViewControllerWillPresent;
            SpotlightViewControllerTapped += Layer_SpotlightViewControllerTapped;
            SpotlightViewControllerWillDismiss += Layer_SpotlightViewControllerWillDismiss;
        }

        private void Next(bool animated) {
            UpdateAnnotationView(animated);
			var screenSize = UIScreen.MainScreen.Bounds.Size;
			
            switch (_stepIndex) 
            {
                case 0:
                    SpotlightView.Appear(new SpotlightShape.Oval(center: new CGPoint(x: screenSize.Width - 26, y: 42), diameter: 50), 0.25F);
                    break;
                case 1:
                    SpotlightView.Move(new SpotlightShape.Oval(center: new CGPoint(x: screenSize.Width - 75, y: 42), diameter: 50), 0.25F);
					break;
				case 2:
                    SpotlightView.Move(new SpotlightShape.RoundedRect(new CGPoint(x: screenSize.Width / 2 , y: 42), 
                                                                 new CGSize(120, 40), 4F), 0.25F, SpotlightMoveType.Disappear);
					break;
				case 3:
                    SpotlightView.Move(new SpotlightShape.Oval(center: new CGPoint(x: screenSize.Width / 2, y: 200), diameter: 220), 0.25F, SpotlightMoveType.Disappear);
					break;
                case 4:
                    DismissViewController(true, null);
                    break;
			}


            _stepIndex++;
		}

        private void UpdateAnnotationView(bool animated)
        {
            
        }

        private void Layer_SpotlightViewControllerWillPresent(SpotlightViewController controller, bool animated)
        {
            Next(false);
        }

        private void Layer_SpotlightViewControllerTapped(SpotlightViewController controller, bool isInsideSpotlight)
        {
            Next(true);
        }

        private void Layer_SpotlightViewControllerWillDismiss(SpotlightViewController controller, bool animated)
        {
            SpotlightView.Disappear(0.25F);
        }
    }
}

