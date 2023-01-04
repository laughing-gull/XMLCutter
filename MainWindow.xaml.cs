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
using System.Xml.Linq;

namespace XMLCutter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private XmlManager xmlManager;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectXML_Click(object sender, RoutedEventArgs e)
        {
            //Create an openFileDialog to select the xml file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a XML File";
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog.ShowDialog() == true)
            {
                //create a new XmlManager to manage the xml file
                xmlManager = new XmlManager(openFileDialog);
            }
            foreach(XAttribute xAttribute in this.xmlManager.getUniqueAttributes())
            {
                this.selectAttributes.Items.Add(xAttribute);
            }

            
        }

        private void SplitXML_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
