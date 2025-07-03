using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TotalControl
{
    public static class Animation
    {
        /// <summary>
        /// Fades in the given element over the specified duration.
        /// </summary>
        public static void FadeIn(UIElement element, double durationSeconds)
        {
            DoubleAnimation fadeInAnim = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(durationSeconds),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            Storyboard.SetTarget(fadeInAnim, element);
            Storyboard.SetTargetProperty(fadeInAnim, new PropertyPath("Opacity"));

            Storyboard sb = new Storyboard();
            sb.Children.Add(fadeInAnim);
            sb.Begin();
        }

        /// <summary>
        /// Fades out the given element over the specified duration.
        /// </summary>
        public static void FadeOut(UIElement element, double durationSeconds)
        {
            DoubleAnimation fadeOutAnim = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(durationSeconds),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            Storyboard.SetTarget(fadeOutAnim, element);
            Storyboard.SetTargetProperty(fadeOutAnim, new PropertyPath("Opacity"));

            Storyboard sb = new Storyboard();
            sb.Children.Add(fadeOutAnim);
            sb.Begin();
        }

        /// <summary>
        /// Slides the element from a starting position to a target position while fading in.
        /// </summary>
        public static void SlideAndFadeIn(FrameworkElement element, double fromX, double fromY, double toX, double toY, double durationSeconds)
        {
            if (element == null) return;

            // Store the original margin
            Thickness originalMargin = element.Margin;

            // Set starting margin
            element.Margin = new Thickness(originalMargin.Left + fromX, originalMargin.Top + fromY, originalMargin.Right - fromX, originalMargin.Bottom - fromY);

            // Create margin animation
            ThicknessAnimation marginAnim = new ThicknessAnimation
            {
                From = new Thickness(originalMargin.Left + fromX, originalMargin.Top + fromY, originalMargin.Right - fromX, originalMargin.Bottom - fromY),
                To = originalMargin,
                Duration = TimeSpan.FromSeconds(durationSeconds),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            // Fade in animation
            DoubleAnimation fadeAnim = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(durationSeconds),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            // Create Storyboard
            Storyboard sb = new Storyboard();
            sb.Children.Add(marginAnim);
            sb.Children.Add(fadeAnim);

            // Set animation targets
            Storyboard.SetTarget(marginAnim, element);
            Storyboard.SetTargetProperty(marginAnim, new PropertyPath("Margin"));

            Storyboard.SetTarget(fadeAnim, element);
            Storyboard.SetTargetProperty(fadeAnim, new PropertyPath("Opacity"));

            // Begin the animation
            sb.Begin();
        }

        /// <summary>
        /// Scales in the element from 0 to its full size.
        /// Useful for animating borders or any control that needs a "pop" effect.
        /// </summary>
        public static void ScaleIn(UIElement element, double durationSeconds)
        {
            // Ensure the element has a ScaleTransform with proper origin.
            ScaleTransform scale = new ScaleTransform(0, 0);
            element.RenderTransformOrigin = new Point(0.5, 0.5);
            element.RenderTransform = scale;

            DoubleAnimation scaleXAnim = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(durationSeconds),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            DoubleAnimation scaleYAnim = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(durationSeconds),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            Storyboard sb = new Storyboard();
            sb.Children.Add(scaleXAnim);
            sb.Children.Add(scaleYAnim);

            Storyboard.SetTarget(scaleXAnim, element);
            Storyboard.SetTargetProperty(scaleXAnim, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));

            Storyboard.SetTarget(scaleYAnim, element);
            Storyboard.SetTargetProperty(scaleYAnim, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

            sb.Begin();
        }
    }
}