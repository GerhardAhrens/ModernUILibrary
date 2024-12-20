
namespace ModernBaseLibrary.Graphics.SVG
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;

    /// <summary>
    ///   A <see cref="MarkupExtension"/> for loading SVG images.
    /// </summary>
    public class SvgImageExtension : MarkupExtension
    {
        private Uri m_Uri = null;
        private bool m_IgnoreEffects = false;

        /// <summary>
        ///   Initializes a new <see cref="SvgImageExtension"/> instance.
        /// </summary>
        public SvgImageExtension()
        {
            // ...
        }

        /// <summary>
        ///   Initializes a new <see cref="SvgImageExtension"/> instance.
        /// </summary>
        /// <param name="uri">
        ///   The location of the SVG document.
        /// </param>
        public SvgImageExtension(Uri uri)
        {
            m_Uri = uri;
        }

        /// <summary>
        ///   Overrides <see cref="MarkupExtension.ProvideValue"/> and returns the 
        ///   <see cref="DrawingImage"/> the SVG document is rendered into.
        /// </summary>
        /// <param name="serviceProvider">
        ///   Object that can provide services for the markup extension; 
        ///   <paramref name="serviceProvider"/> is not used.
        /// </param>
        /// <returns>
        ///   The <see cref="DrawingImage"/> the SVG image is rendered into or 
        ///   <c>null</c> in case there has been an error while parsing or 
        ///   rendering.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                using (Stream stream = Application.GetResourceStream(m_Uri).Stream)
                    return SvgReader.Load(new GZipStream(stream, System.IO.Compression.CompressionMode.Decompress), new SvgReaderOptions { IgnoreEffects = m_IgnoreEffects });
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.GetType() + ": " + exception.Message);
            }

            try
            {
                using (Stream stream = Application.GetResourceStream(m_Uri).Stream)
                    return SvgReader.Load(stream, new SvgReaderOptions { IgnoreEffects = m_IgnoreEffects });
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.GetType() + ": " + exception.Message);
                return null;
            }
        }

        /// <summary>
        ///   Gets or sets the location of the SVG image.
        /// </summary>
        public Uri Uri
        {
            get
            {
                return m_Uri;
            }

            set
            {
                m_Uri = value;
            }
        }

        /// <summary>
        ///   Gets or sets whether SVG filter effects should be transformed into
        ///   WPF bitmap effects.
        /// </summary>
        public bool IgnoreEffects
        {
            get
            {
                return m_IgnoreEffects;
            }

            set
            {
                m_IgnoreEffects = value;
            }
        }
    }
}
