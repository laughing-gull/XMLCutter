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
        private bool cut;
        private XDocument document;
        private XElement root;
        private XElement matchingElements;
        private XElement nonMatchingElements;
        private List<XAttribute> uniqueAttributes;
        private OpenFileDialog openFileDialog;

        public XmlManager(OpenFileDialog openFileDialog)
        {
            this.openFileDialog = openFileDialog;
            this.document = XDocument.Load(this.openFileDialog.FileName);
            this.root = this.document.Root;
            FindUniqueAttributes(this.root);
            this.cut = false;
        }

        public List<XAttribute> GetUniqueAttributes()
        {
            return this.uniqueAttributes;
        }

        public XElement GetRoot()
        {
            return this.root;
        }

        public XElement GetMatchingElements()
        {
#pragma warning disable CS8603 // Possible null reference return.
            return cut ? this.matchingElements : null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public XElement GetNonMatchingElements()
        {
#pragma warning disable CS8603 // Possible null reference return.
            return cut ? this.nonMatchingElements : null;
#pragma warning restore CS8603 // Possible null reference return.
        }


        private void FindUniqueAttributes(XElement root)
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

        public void PopulateMatchingAndNonMatchingElements(List<XAttribute> selectedAttributes)
        {
            int counter = 0;
            this.matchingElements = new XElement(this.root.Name);
            this.nonMatchingElements = new XElement(this.root.Name);

            // Loop through all the parent and child elements in the root element.
            foreach (XElement parent in root.Elements())
            {
                foreach (XElement child in parent.Elements())
                {
                    // Loop through all the attributes of the child element.
                    foreach (XAttribute attribute in child.Attributes())
                    {
                        foreach(XAttribute selectedAttribute in selectedAttributes)
                        {
                            //Deptermine if the current selected attribute matches
                            //the current child attribute
                            if(attribute.ToString().Equals(selectedAttribute.ToString())) counter++;
                        }
                    }
                }
                if (counter == selectedAttributes.Count)
                {
                    this.matchingElements.Add(parent);
                    counter = 0;
                }
                else
                {
                    this.nonMatchingElements.Add(parent);
                    counter = 0;
                }
            }

            this.cut = true;

        }

    }
}
