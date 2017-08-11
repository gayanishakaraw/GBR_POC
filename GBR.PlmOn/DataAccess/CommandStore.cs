using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccess
{
    [Serializable()]
    [XmlRoot("Commands")]
    public class Commands
    {
        [XmlArray("ReadCommands")]
        [XmlArrayItem("ReadCommand", typeof(ReadCommand))]
        public ReadCommand[] ReadCommands { get; set; }
    }

    [Serializable()]
    public class ReadCommand
    {
        [XmlElement("SQLStatement")]
        public SQLStatement SQLCommands { get; set; }

        [XmlAttribute]
        public string name { get; set; }

        [XmlAttribute]
        public string catalog { get; set; }

        [XmlArray("Arguments")]
        [XmlArrayItem("Argument", typeof(Argument))]
        public Argument[] Arguments { get; set; }
    }

    [Serializable()]
    public class Argument
    {
        [XmlAttribute]
        public string name { get; set; }
    }

    [Serializable()]
    public class SQLStatement
    {
        [XmlAttribute]
        public string value { get; set; }
    }
}
