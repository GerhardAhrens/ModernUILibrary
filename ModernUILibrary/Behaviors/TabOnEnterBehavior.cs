//-----------------------------------------------------------------------
// <copyright file="TabOnEnterBehavior.cs" company="Lifeprojects.de">
//     Class: TabOnEnterBehavior
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>09.06.2020</date>
//
// <summary>Definition of Behavior Class for press Enter to next Control</summary>
//-----------------------------------------------------------------------
/*
    <TextBox>
       <i:Interaction.Behaviors>
          <behavior:TabOnEnterBehavior />
       </i:Interaction.Behaviors>
    </TextBox>
 */

namespace ModernIU.Behaviors
{
    using System.Windows.Controls;
    using System.Windows.Input;
    
    using Microsoft.Xaml.Behaviors;

    using ModernIU.Controls;

    public class TabOnEnterBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreviewKeyDown += this.AssociatedObject_PreviewKeyDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewKeyDown -= this.AssociatedObject_PreviewKeyDown;
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                AssociatedObject.MoveFocus(request);
            }
        }
    }

    public class TabOnEnterBehaviorCB : Behavior<ComboBoxEx>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreviewKeyDown += this.AssociatedObject_PreviewKeyDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewKeyDown -= this.AssociatedObject_PreviewKeyDown;
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                AssociatedObject.MoveFocus(request);
            }
        }
    }
}
