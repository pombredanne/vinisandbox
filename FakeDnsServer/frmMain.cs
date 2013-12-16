using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using ARSoft.Tools.Net.Dns;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Reflection;

namespace FakeDnsServer
{
    public partial class frmMain : Form
    {
        private static DNSAnwser last = new DNSAnwser();
        private IPAddress ip = null;
        private DnsServer s = null;
        private List<string> filter = new List<string>();
        private const string FILTER = "filter.txt";
        private Mutex filterMutex = new Mutex(false, "FILTER-MUTEX");
        private delegate void DelAddDomain(string domain);

        private string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public frmMain()
        {
            InitializeComponent();
            string path = AssemblyDirectory + "\\" + FILTER;
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while(!sr.EndOfStream)
                        filter.Add(sr.ReadLine());
                }
            }
        }

        public frmMain(string ip, string log) : this()
        {
            txtIp.Text = ip;
            txtLog.Text = log;
            start();
        }

        public void start()
        {            
            ip = IPAddress.Parse(txtIp.Text);
            btnIniciar.Text = "Parar";
            if (!setDNS(new string[] { "127.0.0.1" }))
            {
                Text = "FakeDnsServer - DNS cannot be set. Are you admin?";                 
            }
            try
            {
                if (!string.IsNullOrEmpty(txtLog.Text) && File.Exists(txtLog.Text))
                {
                    File.WriteAllText(txtLog.Text, string.Empty);
                }

                DnsServer s = new DnsServer(IPAddress.Loopback, 1, 0, new ARSoft.Tools.Net.Dns.DnsServer.ProcessQuery(ProcessQuery));
                s.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);                
            }
        }

        private bool setDNS(string[] servers)
        {
            try
            {

                ManagementClass mc =
                new ManagementClass("Win32_NetworkAdapterConfiguration");

                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {

                    if ((bool)mo["IPEnabled"])
                    {

                        ManagementBaseObject objdns = mo.GetMethodParameters(
                        "SetDNSServerSearchOrder");

                        if (objdns != null)
                        {

                            objdns[
                            "DNSServerSearchOrder"] = servers;

                            ManagementBaseObject ret = mo.InvokeMethod(
                            "SetDNSServerSearchOrder", objdns, null);

                            if (ret["returnValue"].ToString() == "91")
                                return false;
                        }

                    }

                }
                return true;
            }
            catch (ManagementException e)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
                return false;
            }
        }

        private DnsMessageBase ProcessQuery(DnsMessageBase message, IPAddress clientAddress, ProtocolType protocol)
        {
            DnsMessage query = message as DnsMessage;

            if ((query != null) && (query.Questions.Count == 1))
            {
                foreach (var item in query.Questions)
                {
                    var aux = new DNSAnwser() { query = item.Name, time = DateTime.Now.TimeOfDay };
                    if (!aux.Equals(last) && !isInFilter(item.Name))
                    {                       
                        if (!string.IsNullOrEmpty(txtLog.Text))
                            using (var sw = new StreamWriter(txtLog.Text, true))
                            {
                                sw.WriteLine(item.Name);
                            }
                        addDomainToList(item.Name);                        
                        last = aux;
                    }
                }
                query.AnswerRecords.Add(new ARecord(query.Questions[0].Name, 64, ip));
            }
            return query;
        }

        private void addDomainToList(string domain)
        {
            if (lstDomains.InvokeRequired)
                lstDomains.BeginInvoke(new DelAddDomain(addDomainToList), domain);
            else
                lstDomains.Items.Add(domain);
        }

        private bool isInFilter(string p)
        {
            foreach (var item in filter)
            {
                if (Regex.IsMatch(p, item))
                    return true;
            }
            return false;
        }

        private class DNSAnwser
        {
            public TimeSpan time
            { get; set; }
            public string query
            { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is DNSAnwser)
                {
                    var a = (DNSAnwser)obj;
                    if (a.query == query && a.time != null && time != null && a.time.Subtract(time).Seconds < 1)
                        return true;
                }
                return false;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (btnIniciar.Text == "Iniciar")
            {                
                start();
            }
            else
            {                
                stop();
            }
        }

        private void stop()
        {
            btnIniciar.Text = "Iniciar";
            if (s != null)
            {
                s.Stop();
                s = null;
            }
            setDNS(new string[0]);
        }

        private void btnFdlg_Click(object sender, EventArgs e)
        {
            sfdLog.FileName = txtLog.Text;
            if (sfdLog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                txtLog.Text = sfdLog.FileName;
            }
        }
    }
}
