namespace ModernBaseLibrary.Extension
{
    using System.Collections.Generic;

    /// <summary>
    /// Contains methods for querying LinkedList objects.
    /// </summary>
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Ruft alle Knoten in der LinkedList ab.
        /// </summary>
        public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> source)
        {
            var node = source.First;
            while (node != null)
            {
                yield return node;
                node = node.Next;
            }
        }
    }
}
