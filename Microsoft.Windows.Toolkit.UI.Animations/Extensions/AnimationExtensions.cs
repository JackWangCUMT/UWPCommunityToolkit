﻿using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Windows.Toolkit
{
    /// <summary>
    /// Defines a collection of helper methods for UI <see cref="Storyboard"/> animation.
    /// </summary>
    public static class AnimationExtensions
    {
        /// <summary>
        /// Animates a <see cref="FrameworkElement"/> on the X axis.
        /// </summary>
        /// <param name="element">
        /// The target <see cref="FrameworkElement"/>.
        /// </param>
        /// <param name="x">
        /// The target <see cref="double"/> value on the X axis.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns an await-able task.
        /// </returns>
        public static async Task AnimateXAsync(
            this FrameworkElement element,
            double x,
            double duration = 250,
            EasingFunctionBase easingFunction = null)
        {
            if (element.GetTranslateX() != x)
            {
                await
                    AnimateDoublePropertyAsync(
                        element.GetCompositeTransform(),
                        "TranslateX",
                        element.GetTranslateX(),
                        x,
                        duration,
                        easingFunction);
            }
        }

        /// <summary>
        /// Animates a <see cref="double"/> property on a given target asynchronously.
        /// </summary>
        /// <param name="target">
        /// The target <see cref="DependencyObject"/>.
        /// </param>
        /// <param name="property">
        /// The name of the property to animate.
        /// </param>
        /// <param name="from">
        /// A starting <see cref="double"/> value.
        /// </param>
        /// <param name="to">
        /// The resulting <see cref="double"/> value.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns an await-able task.
        /// </returns>
        public static Task AnimateDoublePropertyAsync(
            this DependencyObject target,
            string property,
            double from,
            double to,
            double duration = 250,
            EasingFunctionBase easingFunction = null)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            Storyboard storyboard = AnimateDoubleProperty(target, property, from, to, duration, easingFunction);
            storyboard.Completed += (sender, e) =>
                {
                    tcs.SetResult(true);
                };
            return tcs.Task;
        }

        /// <summary>
        /// Animates a <see cref="double"/> property on a given target.
        /// </summary>
        /// <param name="target">
        /// The target <see cref="DependencyObject"/>.
        /// </param>
        /// <param name="property">
        /// The name of the property to animate.
        /// </param>
        /// <param name="from">
        /// A starting <see cref="double"/> value.
        /// </param>
        /// <param name="to">
        /// The resulting <see cref="double"/> value.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns the <see cref="Storyboard"/> for the animation.
        /// </returns>
        public static Storyboard AnimateDoubleProperty(
            this DependencyObject target,
            string property,
            double from,
            double to,
            double duration = 250,
            EasingFunctionBase easingFunction = null)
        {
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation
                                {
                                    From = from,
                                    To = to,
                                    Duration = TimeSpan.FromMilliseconds(duration),
                                    EasingFunction = easingFunction ?? new SineEase(),
                                    FillBehavior = FillBehavior.HoldEnd,
                                    EnableDependentAnimation = true
                                };

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, property);

            storyboard.Children.Add(animation);
            storyboard.FillBehavior = FillBehavior.HoldEnd;
            storyboard.Begin();

            return storyboard;
        }
    }
}