using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ViniSandbox.Models;
using ViniSandbox.Modules;

namespace ViniSandbox
{
    public class ProcessMonitorLogHandler : LogHandler
    {
        [Serializable]
        //[XmlRoot("procmon")]    
        public class EventCollection
        {
            [XmlArray]
            [XmlArrayItem("event", typeof(computer_event))]
            public List<computer_event> eventlist
            { get; set; }
        }

        [MethodParserAttribute]
        public List<computer_event> ParseEvents(string logContent)
        {
            XmlAttributeOverrides attO = new XmlAttributeOverrides();

            XmlAttributes attTime = new XmlAttributes();
            attTime.XmlElements.Add(new XmlElementAttribute("Time_of_Day"));                
            attO.Add(typeof(computer_event), "time_of_day", attTime);

            XmlAttributes attProcess = new XmlAttributes();
            attProcess.XmlElements.Add(new XmlElementAttribute("Process_Name"));
            attO.Add(typeof(computer_event), "process_name", attProcess);

            XmlAttributes attPid = new XmlAttributes();
            attPid.XmlElements.Add(new XmlElementAttribute("PID"));
            attO.Add(typeof(computer_event), "pid", attPid);

            XmlAttributes attOp = new XmlAttributes();
            attOp.XmlElements.Add(new XmlElementAttribute("Operation"));
            attO.Add(typeof(computer_event), "operation", attOp);

            XmlAttributes attPath = new XmlAttributes();
            attPath.XmlElements.Add(new XmlElementAttribute("Path"));
            attO.Add(typeof(computer_event), "path", attPath);

            XmlAttributes attRes = new XmlAttributes();
            attRes.XmlElements.Add(new XmlElementAttribute("Result"));
            attO.Add(typeof(computer_event), "result", attRes);

            XmlAttributes attDet = new XmlAttributes();
            attDet.XmlElements.Add(new XmlElementAttribute("Detail"));
            attO.Add(typeof(computer_event), "detail", attDet);

            XmlAttributes attrs = new XmlAttributes();
            attrs.XmlIgnore = true;
            attO.Add(typeof(computer_event), "file_properties", attrs);


            XmlAttributes attrAnaId = new XmlAttributes();
            attrAnaId.XmlIgnore = true;
            attO.Add(typeof(computer_event), "id_analysis", attrAnaId);

            XmlAttributes attrAna = new XmlAttributes();
            attrAna.XmlIgnore = true;
            attO.Add(typeof(computer_event), "analysis", attrAna);
            
            XmlSerializer s = new XmlSerializer(typeof(EventCollection), attO, null, new XmlRootAttribute("procmon"), "");
            EventCollection ds = (EventCollection)s.Deserialize(new StringReader(logContent));
            return ds.eventlist;
        }
    }
}