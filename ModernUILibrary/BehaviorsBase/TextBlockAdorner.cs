//-----------------------------------------------------------------------
// <copyright file="TextBlockAdorner.cs" company="Lifeprojects.de">
//     Class: TextBlockAdorner
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>18.09.2018</date>
//
// <summary>Class with TextBlockAdorner Definition</summary>
//-----------------------------------------------------------------------

namespace ModernIU.BehaviorsBase
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class TextBlockAdorner : Adorner
    {
        private readonly TextBlock adornerTextBlock;

        public TextBlockAdorner(UIElement adornedElement, string label, Style labelStyle) : base(adornedElement)
        {
            adornerTextBlock = new TextBlock { Style = labelStyle, Text = label };
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            adornerTextBlock.Measure(constraint);
            return constraint;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            adornerTextBlock.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return adornerTextBlock;
        }
    }
}
