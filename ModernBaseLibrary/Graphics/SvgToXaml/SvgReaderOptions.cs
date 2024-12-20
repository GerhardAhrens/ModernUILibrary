
namespace ModernBaseLibrary.Graphics.SVG
{
    using System.Windows.Media.Effects;

    /// <summary>
    ///   Defines a set of options to customize rendering repspectively reading 
    ///   of SVG documents.
    /// </summary>
    public class SvgReaderOptions
    {

        private bool m_IgnoreEffects = false;

        /// <summary>
        ///   Initializes a new <see cref="SvgReaderOptions"/> instance.
        /// </summary>
        public SvgReaderOptions()
        {
        }

        /// <summary>
        ///   Initializes a new <see cref="SvgReaderOptions"/> instance.
        /// </summary>
        /// <param name="ignoreEffects">
        ///   Specifies whether filter effects should be applied using WPF bitmap 
        ///   effects.
        /// </param>
        public SvgReaderOptions(bool ignoreEffects)
        {
            m_IgnoreEffects = ignoreEffects;
        }

        /// <summary>
        ///   Gets or sets whether SVG effects should either be ignored or 
        ///   converted to <see cref="BitmapEffect">bitmap effects</see>.
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
