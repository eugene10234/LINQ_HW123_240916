using LinqLabs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static LinqLabs.NWDataSet;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MyHomeWork
{
    public partial class Frm作業_1 : Form
    {
        int _pageindex;
        int _pagesize;
        public Frm作業_1()
        {
            InitializeComponent();
            //dataGridView1 = 
            _pageindex = -1;
            _pagesize = 10;
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            SetComboBoxYear(this.nwDataSet1.Orders);
        }
        private void SetComboBoxYear(IEnumerable<DataRow> allBikeq)
        {
            List<int> intyearList = allBikeq
                            .Where(row => !Convert.IsDBNull(row["OrderDate"]))
                            .Select(row => row.Field<DateTime>("OrderDate").Year).Distinct().OrderBy(x => x).ToList();
            for (int i = 0; i < intyearList.Count; i++)
            {
                //MessageBox.Show(intyearList[i].ToString());
                comboBox1.Items.Add(intyearList[i]);
            }
        }
        private void pNext(int maxcount)
        {
            if ( (_pageindex+1)  * _pagesize < maxcount) _pageindex++;
            else _pageindex = maxcount / _pagesize;
            //MessageBox.Show($"source:{maxcount},page:{_pageindex},size:{_pagesize}");
        }
        private void pPrev(int maxcount)
        {
            if (_pageindex > 0) _pageindex--;
            else _pageindex = 0;
            //MessageBox.Show($"source:{maxcount},page:{_pageindex},size:{_pagesize}");
        }
        private IEnumerable<DataRow> genProductPage(NWDataSet.ProductsDataTable productTable)
        {
            List<DataRow> pList = new List<DataRow>();
            for (int i = _pageindex *_pagesize; i < productTable.Count && i< (_pageindex+1) * _pagesize; i++)
            {
                //DataRow row = productTable[i];
                pList.Add(productTable[i]);
            }
            return pList;
        }
        private void tryGetUserPageSize()
        {
            int tryGetSize = 0;
            if (int.TryParse(textBox1.Text, out tryGetSize))
            {
                if (tryGetSize > 0)
                    _pagesize = tryGetSize;
                else
                    textBox1.Text = _pagesize.ToString();
            }
        }
        private void Product_Clicked(Action<int> pMove_NP)
        {
            //gdvOrderInitail();
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);

            int maxcount = this.nwDataSet1.Products.Count();
            tryGetUserPageSize();
            pMove_NP(maxcount);
            IEnumerable<DataRow> indexPage = genProductPage(this.nwDataSet1.Products);
            gdvShowAll(null, indexPage);
        }
        private void btnNextClick(object sender, EventArgs e)
        {
            //this.nwDataSet1.Products.Take(10);//Top 10 Skip(10)

            //Distinct()           
            //int indexsize = 10;     

            Product_Clicked(pNext);

        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            Product_Clicked(pPrev);
        }
        private void btnFInfoLog_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            //files[0].CreationTime
            this.dgvOrders.DataSource = files;

        }

        private void btn2017_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            IEnumerable<System.IO.FileInfo> q = files
                .Where(finfo => finfo.CreationTime.Year == 2024)
                .OrderBy(finfo => finfo.CreationTime);
            var newinfo = q.ToList();
            this.dgvOrders.DataSource = newinfo;
            //columnHideLine(8); 

        }
        private void button4_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            IEnumerable<System.IO.FileInfo> q = files
                .Where(finfo => finfo.Length >= 10000)
                .OrderByDescending(finfo => finfo.Length);
            var newinfo = q.ToList();
            this.dgvOrders.DataSource = newinfo;
        }
        private void gridInitial(System.IO.FileInfo[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                dgvOrders.Columns[i].Visible = true;
            }
        }
        private void gdvOrderInitail()
        {
            //foreach (Delegate d in this.dgvOrders.Click.GetInvocationList())
            //{
            //    this.dgvOrders.Click -= (FindClickedHandler)d;
            //}
            //this.dgvOrders.Click
            this.dgvOrders.DataSource = null;
            this.dgvO_Details.DataSource = null;
        }
        private void gdvShowOne(DataGridView dgv, IEnumerable<DataRow> srcq)
        {
            if (srcq == null) return;
            if (srcq.Count() == 0) return;
            dgv.DataSource = null;
            dgv.DataSource = srcq.CopyToDataTable();
        }
        private void gdvShowAll(IEnumerable<DataRow> oq, IEnumerable<DataRow> odq)
        {
            gdvShowOne(this.dgvOrders, oq);
            gdvShowOne(this.dgvO_Details, odq);
        }
        private void columnHideLine(int hidenum)
        {
            for (int i = 0; i < hidenum; i++)
            {
                dgvOrders.Columns[i].Visible = false;
            }
            dgvOrders.Columns[8].Width = 200;
        }

        private void btnOrderAll_Click(object sender, EventArgs e)
        {
            //gdvOrderInitail();
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            this.order_DetailsTableAdapter1.Fill(this.nwDataSet1.Order_Details);
            gdvOrderShowAll(this.nwDataSet1.Orders, this.nwDataSet1.Order_Details);

        }
        private static List<int> genOrderList(EnumerableRowCollection<NWDataSet.OrdersRow> oq)
        {
            List<int> orderids = new List<int>();
            foreach (var oq_row in oq)
            {
                //MessageBox.Show(oq_row.OrderID.ToString());
                orderids.Add(oq_row.OrderID);
            }
            return orderids;
        }
        
        private void btnOrderSearch_Click(object sender, EventArgs e)
        {
            //gdvOrderInitail();
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            this.order_DetailsTableAdapter1.Fill(this.nwDataSet1.Order_Details);

            int yearnum = 0;
            if (!int.TryParse(comboBox1.Text, out yearnum))
            {
                gdvOrderShowAll(this.nwDataSet1.Orders, this.nwDataSet1.Order_Details);
                return;
            }
            var oq = this.nwDataSet1.Orders
                     .Where(o_row => o_row.OrderDate.Year == yearnum);
            
            List<int> orderids = genOrderList(oq);

            var odq = this.nwDataSet1.Order_Details
                      .Where(od_row => orderids.Contains(od_row.OrderID));

            gdvOrderShowAll(oq, odq);
        }
        IEnumerable<DataRow> _oq;
        IEnumerable<DataRow> _odq;
        private void gdvOrderShowAll(IEnumerable<DataRow> oq, IEnumerable<DataRow> odq)
        {
            _oq = oq;
            _odq = odq;
            this.dgvOrders.RowEnter += this.dgvOrders_RowEnter;
            this.dgvOrders.Click += this.dgvOrders_Click;
            gdvShowAll(oq, odq);
        }

        private void dgvOrders_Click(object sender, EventArgs e)
        {

            //MessageBox.Show(dgvIndex.ToString());
            //List<DataRow> pList = new List<DataRow>();
            //pList.Add(_oq.CopyToDataTable().Rows[dgvIndex]);
            int id = Convert.ToInt32(_oq.CopyToDataTable().Rows[_dgvIndex]["OrderID"]);
            var idodq = _odq.ToList()
                      .Where(od_row => (int)od_row[0]== id);
            gdvShowOne(this.dgvO_Details, idodq);
            //MessageBox.Show(_oq.CopyToDataTable().Rows[dgvIndex]["OrderID"].ToString());
            //MessageBox.Show(_oq.CopyToDataTable().Rows[dgvIndex]);
            //this.dgvO_Details.DataSource = null;
            //this.dgvO_Details.DataSource = _oq.CopyToDataTable();
        }
        int _dgvIndex;
        private void dgvOrders_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            _dgvIndex = e.RowIndex;
            //MessageBox.Show(e.RowIndex.ToString());_oq
            
        }


        //private void gdvOrderShow1123<T1, T2>(IEnumerable<T1> oq, IEnumerable<T2> oq2)
        //{
        //    this.dgvOrders.DataSource = oq.CopyToDataTable();
        //    this.dgvO_Details.DataSource = oq2.CopyToDataTable();
        //}



    }
}
