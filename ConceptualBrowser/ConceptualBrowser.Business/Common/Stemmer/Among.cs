using System;
using System.Collections.Generic;
using System.Text;

namespace ConceptualBrowser.Business.Common.Stemmer
{
    public class Among
    {
        /// <summary>
        ///   Search string.
        /// </summary>
        /// 
        public string SearchString { get; private set; }

        /// <summary>
        ///   Index to longest matching substring.
        /// </summary>
        /// 
        public int MatchIndex { get; private set; }

        /// <summary>
        ///   Result of the lookup.
        /// </summary>
        /// 
        public int Result { get; private set; }

        /// <summary>
        ///   Action to be invoked.
        /// </summary>
        /// 
        public Func<bool> Action { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Among"/> class.
        /// </summary>
        /// 
        /// <param name="str">The search string.</param>
        /// <param name="index">The index to the longest matching substring.</param>
        /// <param name="result">The result of the lookup.</param>
        /// 
        public Among(String str, int index, int result)
            : this(str, index, result, null)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Among"/> class.
        /// </summary>
        /// 
        /// <param name="str">The search string.</param>
        /// <param name="index">The index to the longest matching substring.</param>
        /// <param name="result">The result of the lookup.</param>
        /// <param name="action">The action to be performed, if any.</param>
        /// 
        public Among(String str, int index, int result, Func<bool> action)
        {
            this.SearchString = str;
            this.MatchIndex = index;
            this.Result = result;
            this.Action = action;
        }

        /// <summary>
        ///   Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// 
        /// <returns>
        ///   A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// 
        public override string ToString()
        {
            return SearchString;
        }
    }
}
