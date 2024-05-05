
//-----------------------------------------------------------------------
// <copyright file="TextBoxBehavior.cs" company="Lifeprojects.de">
//     Class: TextBoxBehavior
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.02.2023</date>
//
// <summary>
// Die Behavior Klasse wechselt FontWeight, FontStyle beim wechseln von GotFocus, LostFocus
// </summary>
//-----------------------------------------------------------------------


namespace ModernIU.Behaviors
{
    using System.Runtime.Versioning;
    using System.Windows.Controls;

    using Microsoft.Xaml.Behaviors;

    [SupportedOSPlatform("Windows")]
    public class TextBoxBehavior : Behavior<TextBox>
    {
        /// <summary>
        /// Wird nach dem Anfügen des Verhaltens an das "AssociatedObject" aufgerufen.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.GotFocus += AssociatedObjectGotFocus;
            AssociatedObject.LostFocus += AssociatedObjectLostFocus;

            base.OnAttached();
        }

        /// <summary>
        /// Wird aufgerufen, wenn das Verhalten vom "AssociatedObject" getrennt wird.
        /// Der Aufruf erfolgt vor dem eigentlichen Trennvorgang.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.GotFocus -= AssociatedObjectGotFocus;
            AssociatedObject.LostFocus -= AssociatedObjectLostFocus;

            base.OnDetaching();
        }

        private void AssociatedObjectLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            AssociatedObject.FontWeight = System.Windows.FontWeights.Normal;
            AssociatedObject.FontStyle = System.Windows.FontStyles.Normal;
        }

        private void AssociatedObjectGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            AssociatedObject.FontWeight = System.Windows.FontWeights.Bold;
            AssociatedObject.FontStyle = System.Windows.FontStyles.Italic;
        }
    }
}