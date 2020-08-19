using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EPPlus_Extentions
{
    public static class EPPlusExt
    {
        /// <summary>
        /// Add a calculated field to EPPlus pivot table.
        /// </summary>
        /// <param name="pivotTable"></param>
        /// <param name="name">Name for the created data field</param>
        /// <param name="formula">Formula</param>
        /// <param name="numFmtId">Id for number format
        /// 
        /// Build in ID's
        /// 
        /// 0   General 
        /// 1   0 
        /// 2   0.00 
        /// 3   #,##0 
        /// 4   #,##0.00 
        /// 9   0% 
        /// 10  0.00% 
        /// 11  0.00E+00 
        /// 12  # ?/? 
        /// 13  # ??/?? 
        /// 14  mm-dd-yy 
        /// 15  d-mmm-yy 
        /// 16  d-mmm 
        /// 17  mmm-yy 
        /// 18  h:mm AM/PM 
        /// 19  h:mm:ss AM/PM 
        /// 20  h:mm 
        /// 21  h:mm:ss 
        /// 22  m/d/yy h:mm 
        /// 37  #,##0 ;(#,##0) 
        /// 38  #,##0 ;[Red](#,##0) 
        /// 39  #,##0.00;(#,##0.00) 
        /// 40  #,##0.00;[Red](#,##0.00) 
        /// 45  mm:ss 
        /// 46  [h]:mm:ss 
        /// 47  mmss.0 
        /// 48  ##0.0E+0 
        /// 49  @
        /// </param>
        /// <param name="index">Position of dataField. Zero based.</param>
        /// <param name="caption">Caption if different from name.</param>
        public static void AddCalculatedField(this OfficeOpenXml.Table.PivotTable.ExcelPivotTable pivotTable, string name, string formula, int numFmtId = 10, int? index = 0, string caption = null)
        {
            if (!pivotTable.WorkSheet.Workbook.Styles.NumberFormats.Where(nf => nf.NumFmtId == numFmtId).Any())
                throw new ArgumentOutOfRangeException("Not a valid numFmtId.");

            //First, add the calculated field cacheFields element as a childr of the cacheFields element in the pivotCacheDefinition1.xml
            XmlElement cacheFieldsElement = pivotTable.CacheDefinition.CacheDefinitionXml.GetElementsByTagName("cacheFields")[0] as XmlElement;

            //Add the cacheField element and take note of the index 
            XmlAttribute cacheFieldsCountAttribute = cacheFieldsElement.Attributes["count"];
            int count = Convert.ToInt32(cacheFieldsCountAttribute.Value);
            cacheFieldsElement.InnerXml += String.Format("<cacheField name=\"{0}\" numFmtId=\"0\" formula=\"{1}\" databaseField=\"0\"/>\n", name, formula);
            int cacheFieldIndex = ++count;
            //update cachefields count attribute
            cacheFieldsCountAttribute.Value = count.ToString();

            //Next, update pivotTable1.xml and insert pivotField element as a child of the pivotFields element
            XmlElement pivotFieldsElement = pivotTable.PivotTableXml.GetElementsByTagName("pivotFields")[0] as XmlElement;
            XmlAttribute pivotFieldsCountAttribute = pivotFieldsElement.Attributes["count"];
            pivotFieldsElement.InnerXml += "<pivotField dataField=\"1\" compact=\"0\" outline=\"0\" subtotalTop=\"0\" dragToRow=\"0\" dragToCol=\"0\" dragToPage=\"0\" showAll=\"0\" includeNewItemsInFilter=\"1\" defaultSubtotal=\"0\"/> \n";
            //update pivotFields count attribute
            pivotFieldsCountAttribute.Value = (int.Parse(pivotFieldsCountAttribute.Value) + 1).ToString();

            //Also in pivotTable1.xml, insert the <dataField> to the correct position, the fld here points to cacheField index
            XmlElement dataFields = pivotTable.PivotTableXml.GetElementsByTagName("dataFields")[0] as XmlElement;

            //Create the dataField element with the attributes
            XmlElement dataField = pivotTable.PivotTableXml.CreateElement("dataField", pivotTable.PivotTableXml.DocumentElement.NamespaceURI);
            dataField.RemoveAllAttributes();
            XmlAttribute nameAttrib = pivotTable.PivotTableXml.CreateAttribute("name");

            //cacheField cannot have same name attribute as dataField
            if (caption == null || caption == name)
                nameAttrib.Value = " " + name;
            else
                nameAttrib.Value = caption;
            dataField.Attributes.Append(nameAttrib);

            XmlAttribute fldAttrib = pivotTable.PivotTableXml.CreateAttribute("fld");

            fldAttrib.Value = (cacheFieldIndex - 1).ToString();
            dataField.Attributes.Append(fldAttrib);
            XmlAttribute baseFieldAttrib = pivotTable.PivotTableXml.CreateAttribute("baseField");
            baseFieldAttrib.Value = "0";
            dataField.Attributes.Append(baseFieldAttrib);
            XmlAttribute baseItemAttrib = pivotTable.PivotTableXml.CreateAttribute("baseItem");
            baseItemAttrib.Value = "0";
            dataField.Attributes.Append(baseItemAttrib);
            XmlAttribute numFmtIdAttrib = pivotTable.PivotTableXml.CreateAttribute("numFmtId");
            numFmtIdAttrib.Value = numFmtId.ToString();
            dataField.Attributes.Append(numFmtIdAttrib);

            //Insert dataField element to the correct position.
            if (index <= 0)
            {
                dataFields.PrependChild(dataField);
            }
            else if (index >= dataFields.ChildNodes.Count)
            {
                dataFields.AppendChild(dataField);
            }
            else
            {
                XmlNode insertBeforeThis = dataFields.ChildNodes.Item(index.Value);
                if (insertBeforeThis != null)
                    dataFields.InsertBefore(dataField, insertBeforeThis);
                else
                    dataFields.AppendChild(dataField);
            }

        }



        /// <summary>
        /// Possible values for the ShowDataAs attribute. 
        /// Values got from http://www.ecma-international.org/publications/files/ECMA-ST/ECMA-376,%20Fourth%20Edition,%20Part%201%20-%20Fundamentals%20And%20Markup%20Language%20Reference.zip 
        /// Section 18.8.70
        /// </summary>
        public enum DataFieldShowDataAs
        {
            difference,
            index,
            normal,
            percentDiff,
            percent,
            percentOfCol,
            percentOfRow,
            percentOfTotal,
            runTotal
        }

        /// <summary>
        /// EPPlus doesn't support the Show Data As for pivot data fields. Set the xml for it with this method.
        /// </summary>
        /// <param name="dataField"></param>
        /// <param name="pivot"></param>
        /// <param name="showDataAs"></param>
        public static void SetDataFieldShowDataAsAttribute(this ExcelPivotTableDataField dataField, ExcelPivotTable pivot, DataFieldShowDataAs showDataAs)
        {
            if (pivot != null & pivot.DataFields != null && pivot.DataFields.Contains(dataField))
            {
                string showDataAsAttributeValue = Enum.GetName(typeof(DataFieldShowDataAs), showDataAs);
                var xml = pivot.PivotTableXml;
                XmlNodeList elements = xml.GetElementsByTagName("dataField");

                foreach (XmlNode elem in elements)
                {
                    XmlAttribute fldAttribute = elem.Attributes["fld"];
                    if (fldAttribute != null && fldAttribute.Value == dataField.Index.ToString())
                    {
                        XmlAttribute showDataAsAttribute = elem.Attributes["showDataAs"];
                        if (showDataAsAttribute == null)
                        {
                            showDataAsAttribute = xml.CreateAttribute("showDataAs");
                            elem.Attributes.InsertAfter(showDataAsAttribute, fldAttribute);
                        }
                        showDataAsAttribute.Value = showDataAsAttributeValue;
                    }
                }

            }
        }
    }
}
