using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;

namespace XMLCutter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private XmlManager xmlManager;
        private OpenFileDialog openFileDialog;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectXML_Click(object sender, RoutedEventArgs e)
        {
            //Create an openFileDialog to select the xml file
            this.openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a XML File";
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog.ShowDialog() == true)
            {
                //create a new XmlManager to manage the xml file
                xmlManager = new XmlManager(openFileDialog);
                foreach (XAttribute xAttribute in this.xmlManager.GetUniqueAttributes())
                {
                    this.selectAttributes.Items.Add(xAttribute);
                }
                this.xmlTextBlock.Text = this.xmlManager.GetRoot().ToString();
            }
        }

        private void SplitXML_Click(object sender, RoutedEventArgs e)
        {
            if(xmlManager == null)
            {
                return;
            }

            List<XAttribute> selectedAttributes = new List<XAttribute>();
            foreach (XAttribute attribute in this.selectAttributes.SelectedItems)
            {
                selectedAttributes.Add(attribute);
            }
            this.xmlManager.PopulateMatchingAndNonMatchingElements(selectedAttributes);

            XDocument docWithMatchingAttributes = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                this.xmlManager.GetMatchingElements()
            );

            File.WriteAllText("MatchingAttributes.xml", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" + docWithMatchingAttributes.ToString());


            XDocument docWithNonMatchingAttributes = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                this.xmlManager.GetNonMatchingElements()
            );

            File.WriteAllText("NonMatchingAttributes.xml", "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" + docWithNonMatchingAttributes.ToString());

        }


    }
}
