using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebAPIClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        HttpClient client = new HttpClient();
        BookCollection _user = new BookCollection();
        CancelEventArgs args;
        public MainWindow()
        {
            InitializeComponent();

            client.BaseAddress = new Uri("http://localhost:55179");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            this.DataContext = this;
            this.usergrid.ItemsSource = _user;
            args = new CancelEventArgs();
        }

        private async void GetBooks(object sender, RoutedEventArgs e)
        {
            try
            {
                btnGetBooks.IsEnabled = false;

                var response = await client.GetAsync("api/Book");
                response.EnsureSuccessStatusCode(); // 오류 코드를 던집니다.

                
                RootObject myInstance = JsonConvert.DeserializeObject<RootObject>(
                                            await response.Content.ReadAsStringAsync());

                _user.CopyFrom(myInstance.book);
            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                // 이 예외는 요청 본문을 역직렬화 할 때, 문제가 발생했음을 나타냅니다.
                MessageBox.Show(jEx.Message);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnGetBooks.IsEnabled = true;
            }
        }

        private async void PostBook(object sender, RoutedEventArgs e)
        {
            btnPostBook.IsEnabled = false;

            try
            {

                Book book = new Book()
                {
                    //name = NameTxtBox.Text,
                    //author = AuthorTxtBox.Text,
                    //isbn = ISBNTxtBox.Text
                    name = SelectedBookName,
                    author = SelectedBookAuthor,
                    isbn = SelectedBookISBN

                };

                if (SelectedBookID.HasValue)
                {
                    var req = await client.PostAsJsonAsync("api/Book/" + SelectedBookID, book);
                    req.EnsureSuccessStatusCode();// 오류 코드를 던집니다.
                }
                else
                {
                    var req = await client.PostAsJsonAsync("api/Book", book);
                    req.EnsureSuccessStatusCode();// 오류 코드를 던집니다.
                }
                
               

                var resp = await client.GetAsync("api/Book");
                resp.EnsureSuccessStatusCode(); // 오류 코드를 던집니다.


                RootObject myInstance = JsonConvert.DeserializeObject<RootObject>(
                                            await resp.Content.ReadAsStringAsync());

                _user.CopyFrom(myInstance.book);

            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Price must be a number");
            }
            finally
            {
                btnPostBook.IsEnabled = true;
            }
        }

       

        private async void DeleteBook(object sender, RoutedEventArgs e)
        {
            btnDelSelectedBook.IsEnabled = false;

            try
            {

                


                var req = await client.DeleteAsync("api/Book/"+SelectedBookID);
                req.EnsureSuccessStatusCode(); // 오류 코드를 던집니다.

                var resp = await client.GetAsync("api/Book");
                resp.EnsureSuccessStatusCode(); // 오류 코드를 던집니다.


                RootObject myInstance = JsonConvert.DeserializeObject<RootObject>(
                                            await resp.Content.ReadAsStringAsync());

                _user.CopyFrom(myInstance.book);

            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Price must be a number");
            }
            finally
            {
                btnDelSelectedBook.IsEnabled = true;
            }
        }

        private async void DeleteAll(object sender, RoutedEventArgs e)
        {
            btnDelAllBook.IsEnabled = false;

            try
            {

                


                var req = await client.DeleteAsync("api/Book");
                req.EnsureSuccessStatusCode(); // 오류 코드를 던집니다.

                var resp = await client.GetAsync("api/Book");
                resp.EnsureSuccessStatusCode(); // 오류 코드를 던집니다.


                RootObject myInstance = JsonConvert.DeserializeObject<RootObject>(
                                            await resp.Content.ReadAsStringAsync());

                _user.CopyFrom(myInstance.book);

            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Price must be a number");
            }
            finally
            {
                btnDelAllBook.IsEnabled = true;
            }
        }

        private string _selectedBookName;
        public string SelectedBookName
        {
            get
            {
                return _selectedBookName;
            }

            set
            {
                if (_selectedBookName == value)
                {
                    return;
                }

                _selectedBookName = value;
                OnPropertyChanged();
            }
        }

        private string _selectedBookAuthor;
        public string SelectedBookAuthor
        {
            get
            {
                return _selectedBookAuthor;
            }

            set
            {
                if (_selectedBookAuthor == value)
                {
                    return;
                }

                _selectedBookAuthor = value;
                OnPropertyChanged();
            }
        }

        private string _selectedBookISBN;
        public string SelectedBookISBN
        {
            get
            {
                return _selectedBookISBN;
            }

            set
            {
                if (_selectedBookISBN == value)
                {
                    return;
                }

                _selectedBookISBN = value;
                OnPropertyChanged();
            }
        }

        private int? _selectedBookID;
        public int? SelectedBookID
        {
            get
            {
                return _selectedBookID;
            }

            set
            {
                if (_selectedBookID == value)
                {
                    return;
                }

                _selectedBookID = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
       

        private void usergrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            btnPostBook.Content = "변경";

            
            if (args.Cancel)
            {
                return;
            }

           if(usergrid.SelectedItem == null)
            {
                SelectedBookID = null;
                SelectedBookName = null;
                SelectedBookAuthor = null;
                SelectedBookISBN = null;
                btnPostBook.Content = "추가";
                return;
            }

            Book selectedBook = (Book)usergrid.SelectedItem;
            try
            {
                SelectedBookID = selectedBook.id;
                SelectedBookName = selectedBook.name;
                SelectedBookAuthor = selectedBook.author;
                SelectedBookISBN = selectedBook.isbn;
            }
            catch(Exception ex)
            {
                return;
            }
            
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            args.Cancel = true;
            usergrid.UnselectAllCells();
            usergrid.UnselectAll();
            args.Cancel = false;
        }


        private void btnMakeNullBox_Click(object sender, RoutedEventArgs e)
        {
            SelectedBookID = null;
            SelectedBookName = null;
            SelectedBookAuthor = null;
            SelectedBookISBN = null;
            btnPostBook.Content = "추가";
        }
    }
}
