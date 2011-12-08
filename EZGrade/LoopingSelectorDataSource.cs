using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Phone.Controls.Primitives;

namespace EZGrade
{
    /// <summary>
    /// Provides a data source for a <see cref="LoopingSelector"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LoopingSelectorDataSource<T> : ILoopingSelectorDataSource where T : class
    {
        private readonly IList<T> list;
        private T selectedItem;
        private bool allowInfiniteScrolling;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoopingSelectorDataSource&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public LoopingSelectorDataSource(IList<T> list)
            : this(list, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoopingSelectorDataSource&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="selectedValue">The selected value.</param>
        public LoopingSelectorDataSource(IList<T> list, T selectedValue, bool allowInfiniteScrolling)
        {
            this.list = list;
            this.selectedItem = selectedValue ?? list[0];
            this.allowInfiniteScrolling = allowInfiniteScrolling;
        }

        /// <summary>
        /// Raised when the selection changes.
        /// </summary>
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Get the next datum, relative to an existing datum.
        /// </summary>
        /// <param name="relativeTo">The datum the return value will be relative to.</param>
        /// <returns>The next datum.</returns>
        protected virtual T GetNext(T relativeTo)
        {
            return this.GetRelativeTo(relativeTo, 1);
        }

        /// <summary>
        /// Get the previous datum, relative to an existing datum.
        /// </summary>
        /// <param name="relativeTo">The datum the return value will be relative to.</param>
        /// <returns>The previous datum.</returns>
        protected virtual T GetPrevious(T relativeTo)
        {
            return this.GetRelativeTo(relativeTo, -1);
        }

        /// <summary>
        /// The selected item. Should never be null.
        /// </summary>
        public virtual T SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (value != selectedItem)
                {
                    if ((null == value) || (null == selectedItem) || (value != selectedItem))
                    {
                        object previousSelectedItem = selectedItem;
                        selectedItem = value;
                        EventHandler<SelectionChangedEventArgs> handler = SelectionChanged;
                        if (null != handler)
                        {
                            handler(this, new SelectionChangedEventArgs(new object[] { previousSelectedItem }, new object[] { selectedItem }));
                        }
                    }
                }
            }
        }

        private T GetRelativeTo(T relativeItem, int delta)
        {
            int index = this.list.IndexOf(relativeItem) + delta;

            if (!this.allowInfiniteScrolling && (index < 0 || index > (this.list.Count - 1)))
            {
                return null;
            }

            if (index < 0)
            {
                index = (this.list.Count - 1);
            }
            else if (index > (this.list.Count - 1))
            {
                index = 0;
            }

            return this.list[index];
        }

        /// <summary>
        /// Get the next datum, relative to an existing datum.
        /// </summary>
        /// <param name="relativeTo">The datum the return value will be relative to.</param>
        /// <returns>The next datum.</returns>
        object ILoopingSelectorDataSource.GetNext(object relativeTo)
        {
            return this.GetNext((T)relativeTo);
        }

        /// <summary>
        /// Get the previous datum, relative to an existing datum.
        /// </summary>
        /// <param name="relativeTo">The datum the return value will be relative to.</param>
        /// <returns>The previous datum.</returns>
        object ILoopingSelectorDataSource.GetPrevious(object relativeTo)
        {
            return this.GetPrevious((T)relativeTo);
        }

        /// <summary>
        /// The selected item. Should never be null.
        /// </summary>
        /// <value></value>
        object ILoopingSelectorDataSource.SelectedItem
        {
            get { return this.SelectedItem; }
            set { this.SelectedItem = (T)value; }
        }
    }
}