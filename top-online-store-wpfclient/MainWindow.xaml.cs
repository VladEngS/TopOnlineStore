using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
using top_online_store_models.Models;

namespace top_online_store_wpfclient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            using HttpClient client = new();
            var result = await client.GetAsync("http://localhost:5000/api/products");
            var json = await result.Content.ReadAsStringAsync();
            var jObj = JObject.Parse(json);
            if ((string?)jObj["status"] != "ok")
                throw new Exception();

            Product[] products = jObj["projects"]!.ToObject<Product[]>()!;
        }
    }
}
