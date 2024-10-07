using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ECBGatewayLibrary
{
    public class ECBParserXML
    {
        public List<ECBRateModel> ParserXMl(string XmlDoc)
        {
            var xml = XDocument.Parse(XmlDoc);
            XNamespace gesmes = "http://www.gesmes.org/xml/2002-08-01";
            XNamespace ecb = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

            var result = xml.Descendants(ecb + "Cube")
                .Where(c => c.Attribute("currency") != null)
                .Select(c => new ECBRateModel
                {
                    Currency = c.Attribute("currency")?.Value,
                    Rate = decimal.Parse(c.Attribute("rate")?.Value ?? "0.0")
                }).ToList();

            return result;
        }
    }
}
