using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeCalc
{
    public partial class form_Web : Form
    {
        string url;

        public form_Web(string url)
        {
            InitializeComponent();
            this.url = url;

            webBrowser1.Navigate(url);
            webBrowser1.DocumentCompleted += save;

        }

        void save(object sender, EventArgs e)
        {

            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();

            
            Bitmap bitmap2 = new Bitmap(responseStream);


            Console.WriteLine();


            //string id = "img";

            //var elements = webBrowser1.Document.Images;

            //foreach(HtmlElement foo in elements)
            //{
            //    //HTMLImgElement img = (IHTMLImgElement)e.DomElement;
            //    //Bitmap bmp = new Bitmap(url); //new Bitmap(foo.OffsetRectangle.Width, element.OffsetRectangle.Height);

            //    Console.WriteLine(".");
            //}

            //foreach (HtmlElement foo in webBrowser1.Document.Images)
            //{
            //    File.WriteAllText("test" + i.ToString() + ".txt", foo.All, Encoding.GetEncoding(webBrowser1.Document.Encoding));
            //    i++;
            //}

            //File.WriteAllText("test.txt", webBrowser1.Document.Body.Parent.OuterHtml, Encoding.GetEncoding(webBrowser1.Document.Encoding));
        }

        private void form_Web_Load(object sender, EventArgs e)
        {
        }
    }
}
