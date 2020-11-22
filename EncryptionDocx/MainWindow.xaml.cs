using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;


namespace EncryptionDocx
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = "";
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            if (fileDialog.ShowDialog() == true)
            {
                path = fileDialog.FileName.ToString();
            }
            FlowDocument document = new FlowDocument();
            Paragraph p = new Paragraph();

            txtPath.Text = path;

            Word.Application MSWord = new Word.Application();
            Word.Document Doc = MSWord.Documents.Open(path);
            string text = "";
            for (int i = 0; i < Doc.Paragraphs.Count; i++)
            {
                text += " \r\n " + Doc.Paragraphs[i + 1].Range.Text;
            }
            
            p.Inlines.Add(new Run(text));
            document.Blocks.Add(p);
            richTextBox.Document = document;

            Doc.Close();


        }

        private void BtnEncode_Click(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            try
            {
                var encryptedStringAES = Crypto.EncryptStringAES(richText, txtSharedSecret.Text);

                FlowDocument document = new FlowDocument();
                Paragraph p = new Paragraph();


                p.Inlines.Add(new Run(encryptedStringAES));
                document.Blocks.Add(p);
                richTextBox.Document = document;
            }
            catch
            {
                MessageBox.Show("Принята пустая строка");
            }
            
        }

        private void BtnDecode_Click(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            string text = null;
            try
            {
                text = Crypto.DecryptStringAES(richText, txtSharedSecret.Text);
            }
            catch
            {
                MessageBox.Show("Invalid code word");
            }
            

            FlowDocument document = new FlowDocument();
            Paragraph p = new Paragraph();


            p.Inlines.Add(new Run(text));
            document.Blocks.Add(p);
            richTextBox.Document = document;

        }
    }
}
