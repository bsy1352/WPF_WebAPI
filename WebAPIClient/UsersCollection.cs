using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIClient
{
    class BookCollection : ObservableCollection<Book>
    {
       
        public void CopyFrom(IEnumerable<Book> books)
        {
            this.Items.Clear();
            foreach (var p in books)
            {
                this.Items.Add(p);
            }

            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
