using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace PListParserLibrary
{
    public class Parser
    {
        public bool IsFirstDict { get; set; } = false;

        Dictionary<string, object> PropertyDictionary { get; set; }

        public string DictionarySubKey { get; set; } = string.Empty;

        public Property CurrentProperty { get; set; }


        public Dictionary<string, object> Parse(string pListText)
        {
            try
            {
                using (var sr = new StringReader(pListText))
                {
                    XmlReader reader = XmlReader.Create(sr, new XmlReaderSettings()
                    {
                        IgnoreWhitespace = true,
                        DtdProcessing = DtdProcessing.Parse,
                        IgnoreComments = true

                    });
                    while (reader.Read())
                    {
                        if (reader.Name == "key" && reader.NodeType == XmlNodeType.Element)
                        {
                            
                            if (this.CurrentProperty != null && !string.IsNullOrEmpty(this.CurrentProperty.Key) && this.CurrentProperty.Value != null)
                            {
                                if (this.CurrentProperty.ValueKind != ValueType.Dict)
                                {
                                    this.PropertyDictionary.TryAdd(this.CurrentProperty.Key, this.CurrentProperty.Value);
                                    this.CurrentProperty = new Property();
                                }
                                else
                                {
                                    this.DictionarySubKey = String.Empty;
                                }
                            }
                            else
                            {
                                this.CurrentProperty = new Property();
                            }
                            continue;
                        }
                        else if (reader.Name == "dict" && reader.NodeType == XmlNodeType.Element)
                        {
                            if (this.IsFirstDict is not true)
                            {
                                this.IsFirstDict = true;
                                this.PropertyDictionary = new Dictionary<string, object>();
                                continue;
                            }
                            else
                            {
                                this.CurrentProperty.ValueKind = ValueType.Dict;
                                this.CurrentProperty.Value = new Dictionary<string, object>();
                            }
                            continue;
                        }
                        else if (reader.Name == "array" && reader.NodeType == XmlNodeType.Element)
                        {
                            this.CurrentProperty.ValueKind = ValueType.Array;
                            continue;
                        }
                        else if (reader.Name == "string" && reader.NodeType == XmlNodeType.Element)
                        {
                            if (this.CurrentProperty.ValueKind == ValueType.Array)
                            {
                                if (this.CurrentProperty.Value is null)
                                {
                                    this.CurrentProperty.Value = new List<string>();
                                }
                                continue;
                            }
                            else if(this.CurrentProperty.ValueKind == ValueType.Dict)
                            {
                                if (this.CurrentProperty.Value is null)
                                {
                                    this.CurrentProperty.Value = new Dictionary<string, object>();
                                }
                                continue;
                            }
                            else
                            {
                                this.CurrentProperty.ValueKind = ValueType.String;
                                continue;
                            }
                        }
                        else if (reader.Name == "integer" && reader.NodeType == XmlNodeType.Element && this.CurrentProperty.Value == null)
                        {
                            if (this.CurrentProperty.ValueKind == ValueType.Array)
                            {
                                if (this.CurrentProperty.Value is null)
                                {
                                    this.CurrentProperty.Value = new List<int>();
                                }
                                continue;
                            }
                            else if (this.CurrentProperty.ValueKind == ValueType.Dict)
                            {
                                if (this.CurrentProperty.Value is null)
                                {
                                    this.CurrentProperty.Value = new Dictionary<string, object>();
                                }
                                continue;
                            }
                            else
                            {
                                this.CurrentProperty.ValueKind = ValueType.Integer;
                                continue;
                            }
                        }
                        else if (reader.Name == "date" && reader.NodeType == XmlNodeType.Element)
                        {
                            this.CurrentProperty.ValueKind = ValueType.Date;
                            continue;
                        }
                        else if (reader.Name == "data" && reader.NodeType == XmlNodeType.Element)
                        {
                            this.CurrentProperty.ValueKind = ValueType.Data;//CDATA
                            continue;
                        }
                        else if (reader.Name == "real" && reader.NodeType == XmlNodeType.Element)
                        {
                            this.CurrentProperty.ValueKind = ValueType.Real;
                            continue;
                        }
                        else if (reader.Name == "false" || reader.Name == "true" && reader.NodeType == XmlNodeType.Element)
                        {
                            this.CurrentProperty.ValueKind = ValueType.Boolean;
                            _ = bool.TryParse(reader.Value, out bool booli);
                            this.CurrentProperty.Value = booli;
                        }
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.None:
                                break;
                            case XmlNodeType.Element:
                                Debug.WriteLine(reader.Value);
                                break;
                            case XmlNodeType.Attribute:
                                Debug.WriteLine(reader.Value);
                                break;
                            case XmlNodeType.Text:
                                if (string.IsNullOrEmpty(this.CurrentProperty.Key))
                                {
                                    this.CurrentProperty.Key = reader.Value;
                                }
                                else
                                {
                                    switch (this.CurrentProperty.ValueKind)
                                    {
                                        case ValueType.String:
                                        case ValueType.Integer:
                                        case ValueType.Boolean:
                                        case ValueType.Real:
                                            this.CurrentProperty.Value = reader.Value;
                                            break;
                                        case ValueType.Array:

                                            if (this.CurrentProperty.Value is List<string> listOfString)
                                            {
                                                listOfString.Add(reader.Value);
                                            }
                                            else if (this.CurrentProperty.Value is List<int> listOfInts)
                                            {
                                                int.TryParse(reader.Value, out int intValue);
                                                listOfInts.Add(intValue);
                                            }
                                            break;
                                        case ValueType.Dict:
                                            if (string.IsNullOrWhiteSpace(this.DictionarySubKey))
                                            {
                                                this.DictionarySubKey = reader.Value;
                                            }
                                            else if (this.CurrentProperty.Value is Dictionary<string, object> dictionary)
                                            {
                                                dictionary.TryAdd(this.DictionarySubKey, reader.Value);
                                                this.DictionarySubKey  = String.Empty;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                Debug.WriteLine(reader.Value);
                                break;
                            case XmlNodeType.CDATA:
                                break;
                            case XmlNodeType.EntityReference:
                                break;
                            case XmlNodeType.Entity:
                                break;
                            case XmlNodeType.ProcessingInstruction:
                                break;
                            case XmlNodeType.Comment:
                                break;
                            case XmlNodeType.Document:
                                Debug.WriteLine(reader.Value);
                                break;
                            case XmlNodeType.DocumentType:
                                Debug.WriteLine(reader.Value);
                                break;
                            case XmlNodeType.DocumentFragment:
                                Debug.WriteLine(reader.Value);
                                break;
                            case XmlNodeType.Notation:
                                break;
                            case XmlNodeType.Whitespace:
                                break;
                            case XmlNodeType.SignificantWhitespace:
                                break;
                            case XmlNodeType.EndElement:
                                if(reader.Name == "dict")
                                {
                                    this.PropertyDictionary.TryAdd(this.CurrentProperty.Key, this.CurrentProperty.Value);
                                    this.CurrentProperty = new Property();
                                }
                                break;
                            case XmlNodeType.EndEntity:
                                break;
                            case XmlNodeType.XmlDeclaration:
                                Debug.WriteLine(reader.Value);
                                break;
                            default:
                                break;
                        }
                    }
                }
                return this.PropertyDictionary;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return this.PropertyDictionary;
            }
        }
    }
}
