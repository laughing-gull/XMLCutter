using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLCutter
{
    class XmlManager
    {
        private XDocument document;
        private XElement root;
        private List<XAttribute> uniqueAttributes;
        private OpenFileDialog openFileDialog;

        public XmlManager(OpenFileDialog openFileDialog)
        {
            this.openFileDialog = openFileDialog;
            this.document = XDocument.Load(this.openFileDialog.FileName);
            this.root = this.document.Root;
            findUniqueAttributes(this.root);
        }

        public List<XAttribute> getUniqueAttributes()
        {
            return this.uniqueAttributes;
        }

        public XElement getRoot()
        {
            return this.root;
        }

        private void findUniqueAttributes(XElement root)
        {
            // Create a HashSet to store the string representations of the attributes.
            // The HashSet automatically prevents duplicates.
            HashSet<string> attributeStrings = new HashSet<string>();

            // Loop through all the parent and child elements in the root element.
            foreach (XElement parent in root.Elements())
            {
                foreach (XElement child in parent.Elements())
                {
                    // Loop through all the attributes of the child element.
                    foreach (XAttribute attribute in child.Attributes())
                    {
                        // Add the string representation of the attribute to the HashSet.
                        attributeStrings.Add(attribute.ToString());
                    }
                }
            }

            // Create a list to store the XAttribute objects.
            List<XAttribute> attributes = new List<XAttribute>();

            // Use a regular expression to extract the name and value of the attributes,
            // and create XAttribute objects using the name and value.
            foreach (string attributeString in attributeStrings)
            {
                // Use a regular expression to extract the name and value of the attribute.
                Match match = Regex.Match(attributeString, @"(?<name>\w+)=""(?<value>.*?)""");
                string name = match.Groups["name"].Value;

                string value = match.Groups["value"].Value;

                // Create an XAttribute object using the name and value, and add it to the list.
                attributes.Add(new XAttribute(XName.Get(name), value));
            }


            // Assign the list of XAttribute objects to the uniqueAttributes field.
            this.uniqueAttributes = attributes;
        }





    }
}
