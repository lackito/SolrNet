﻿#region license
// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Xml;

namespace SolrNet.Impl {
    /// <summary>
    /// Parses a solr result into a dictionary of (string, object)
    /// </summary>
    public class SolrDictionaryDocumentResponseParser: ISolrDocumentResponseParser<Dictionary<string, object>> {
        private readonly ISolrFieldParser fieldParser;

        public SolrDictionaryDocumentResponseParser(ISolrFieldParser fieldParser) {
            this.fieldParser = fieldParser;
        }

        public IList<Dictionary<string, object>> ParseResults(XmlNode parentNode) {
            var results = new List<Dictionary<string, object>>();
            if (parentNode == null)
                return results;
            var nodes = parentNode.SelectNodes("doc");
            if (nodes == null)
                return results;
            foreach (XmlNode docNode in nodes) {
                results.Add(ParseDocument(docNode));
            }
            return results;
        }

        public Dictionary<string, object> ParseDocument(XmlNode node) {
            var doc = new Dictionary<string, object>();
            foreach (XmlNode field in node.ChildNodes) {
                string fieldName = field.Attributes["name"].InnerText;
                doc[fieldName] = fieldParser.Parse(field, typeof(object));
            }
            return doc;
        }
    }
}