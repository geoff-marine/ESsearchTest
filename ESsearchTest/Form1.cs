using Nest;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elasticsearch.Net;

namespace ESsearchTest
{
    public partial class Form1 : Form
    {
        ConnectionSettings connectionSettings;
        ElasticClient elasticClient;
        ElasticLowLevelClient lowLevelClient;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Establishing connection with ES
            connectionSettings = new ConnectionSettings(new Uri("http://10.11.1.70:9200"));          
            elasticClient = new ElasticClient(connectionSettings);

        }

        //Get suggestion under search textbox
        private void tbxName_TextChanged(object sender, EventArgs e)
        {
            //Search query to retrieve info

            var response = elasticClient.Search<Vessel>(s => s
                .Size(30)
                .Index("vesselname")
                .Type("allnames")            
                .Query(q => q
               .MultiMatch(c => c
               .Fields(f => f.Field(p => p.VesselName).Field("ExactName^10"))               
               .Query(tbxName.Text)) || q.Term("CountryCode^10", "IRL")));
                //.Query(q => q.QueryString(qs => qs.Query(tbxName.Text + "*"))));

                //.Query(q => q.QueryString(qs => qs.
                //Fields(f => f.Field(fi => fi.VesselName)).Query(tbxName.Text + "*"))));

                //.Query(q => q.Match(m => m.Field(v => v.VesselName).Query(tbxName.Text))));



            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            foreach (var hit in response.Documents)
            {
                collection.Add(hit.VesselName);
            }

            tbxName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbxName.AutoCompleteMode = AutoCompleteMode.Suggest;
            tbxName.AutoCompleteCustomSource = collection;



            if (tbxName.Text == "")
            {
                listView1.Clear();
            }
        }

        private void tbxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
            {
                listView1.Clear();
            }
        }

        //Retrieve information based on search textbox value
        private void btnSearch_Click(object sender, EventArgs e)
        {

            FillList(listView1);
                   
        }

        private void FillList(ListView component)
        {
            var response = elasticClient.Search<Vessel>(s => s
                .Size(50)
                .Index("vesselname")
                .Type("allnames")
                
                .Query(q => q
               .MultiMatch(c => c
               .Fields(f => f.Field(p => p.VesselName).Field("ExactName^10"))
               .Query(tbxName.Text)) || q.Term("CountryCode^10", "IRL")));


            BuildListView(component);

            foreach(var hit in response.Hits)
            {
                ListViewItem lvwItem = new ListViewItem();

                lvwItem.Text = hit.Source.cfr.ToString();
                lvwItem.SubItems.Add(hit.Source.CountryCode).ToString();
                lvwItem.SubItems.Add(hit.Source.VesselName).ToString();
                lvwItem.SubItems.Add(hit.Source.PortCode).ToString();
                lvwItem.SubItems.Add(hit.Source.PortName).ToString();
                lvwItem.SubItems.Add(hit.Source.Loa).ToString();
                lvwItem.SubItems.Add(hit.Source.TonRef).ToString();
                lvwItem.SubItems.Add(hit.Source.PowerMain).ToString();
                component.Items.Add(lvwItem);
   
            }
        }

        private void BuildListView(ListView component)
        {
            component.Columns.Clear();
            component.Items.Clear();

            component.View = System.Windows.Forms.View.Details;
            component.FullRowSelect = true;
            component.GridLines = true;
            

            ColumnHeader cfr, CountryCode, VesselName, PortCode, PortName, Loa, TonRef, PowerMain;
            cfr = new ColumnHeader();
            CountryCode = new ColumnHeader();
            VesselName = new ColumnHeader();
            PortCode = new ColumnHeader();
            PortName = new ColumnHeader();
            Loa = new ColumnHeader();
            TonRef = new ColumnHeader();
            PowerMain = new ColumnHeader();

            cfr.Text = "CFR";
            cfr.TextAlign = HorizontalAlignment.Left;
            cfr.Width = 100;

            CountryCode.Text = "Country Code";
            CountryCode.TextAlign = HorizontalAlignment.Left;
            CountryCode.Width = 100;

            VesselName.Text = "Vessel Name";
            VesselName.TextAlign = HorizontalAlignment.Left;
            VesselName.Width = 150;

            PortCode.Text = "Port Code";
            PortCode.TextAlign = HorizontalAlignment.Left;
            PortCode.Width = 80;

            PortName.Text = "Port Name";
            PortName.TextAlign = HorizontalAlignment.Left;
            PortName.Width = 100;

            Loa.Text = "Loa";
            Loa.TextAlign = HorizontalAlignment.Left;
            Loa.Width = 100;

            TonRef.Text = "TonRef";
            TonRef.TextAlign = HorizontalAlignment.Left;
            TonRef.Width = 100;

            PowerMain.Text = "PowerMain";
            PowerMain.TextAlign = HorizontalAlignment.Left;
            PowerMain.Width = 100;



            component.Columns.Add(cfr);
            component.Columns.Add(CountryCode);
            component.Columns.Add(VesselName);
            component.Columns.Add(PortCode);
            component.Columns.Add(PortName);
            component.Columns.Add(Loa);
            component.Columns.Add(TonRef);
            component.Columns.Add(PowerMain);

        }


    }
}
