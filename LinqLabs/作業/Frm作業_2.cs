using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_2 : Form
    {
        int _index_RowEnter;
        IEnumerable<DataRow> _AllBikeq;
        IEnumerable<DataRow> _ConBikeq;
        List<byte[]> _imageList;
        public Frm作業_2()
        {
            InitializeComponent();
            dataGridView1.AllowUserToAddRows = false;
            #region Adapter_Fill_All_To_ad2019DataSet1
            this.productTableAdapter1.Fill(this.ad2019DataSet1.Product);
            this.productSubcategoryTableAdapter1.Fill(this.ad2019DataSet1.ProductSubcategory);
            this.productCategoryTableAdapter1.Fill(this.ad2019DataSet1.ProductCategory);
            this.productProductPhotoTableAdapter1.Fill(this.ad2019DataSet1.ProductProductPhoto);
            this.productPhotoTableAdapter1.Fill(this.ad2019DataSet1.ProductPhoto);
            #endregion
            InitialBikeIEnumPreOP();
            //gdvOrderShowAll(this.nwDataSet1.Orders, this.nwDataSet1.Order_Details);
        }
        private void gdvShowOne(DataGridView dgv, IEnumerable<DataRow> srcq)
        {
            dgv.DataSource = null;
            if (srcq == null) return;
            if (srcq.Count() == 0) return;
            dgv.DataSource = srcq.CopyToDataTable();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            GetBikeIEnumOP(row => true);
            gdvShowOne(dataGridView1, _ConBikeq);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            GetBikeIEnumOP(RangeDate);
            gdvShowOne(dataGridView1, _ConBikeq);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            GetBikeIEnumOP(SelectDate);
            gdvShowOne(dataGridView1, _ConBikeq);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            GetBikeIEnumOP(SeasonSelector(comboBox2));
            gdvShowOne(dataGridView1, _ConBikeq);
        }
        private IEnumerable<DataRow> InitialBikeIEnumPreOP()
        {
            #region GetBikeIEnumPreCode
            List<string> conlist = new List<string> { "Bikes" };
            IEnumerable<DataRow> conpcq = this.ad2019DataSet1.ProductCategory.getIEnumByContains(conlist, "Name");
            List<int> idKListPC = conpcq.getPFKeyList<int>("ProductCategoryID");

            IEnumerable<DataRow> conpscq = this.ad2019DataSet1.ProductSubcategory.getIEnumByContains(idKListPC, "ProductCategoryID");
            List<int> idKListpsc = conpscq.getPFKeyList<int>("ProductSubcategoryID");

            IEnumerable<DataRow> conpq = this.ad2019DataSet1.Product.getIEnumByContains(idKListpsc, "ProductSubcategoryID");
            List<int> idKListp = conpq.getPFKeyList<int>("ProductID");

            IEnumerable<DataRow> conPPPq = this.ad2019DataSet1.ProductProductPhoto.getIEnumByContains(idKListp, "ProductID");
            List<int> idKListppp = conPPPq.getPFKeyList<int>("ProductPhotoID");
            IEnumerable<DataRow> allBikeq = this.ad2019DataSet1.ProductPhoto.getIEnumByContains(idKListppp, "ProductPhotoID");
            #endregion
            SetComboBoxYear(allBikeq);
            _AllBikeq = allBikeq;
            return allBikeq;
        }
        private void GetBikeIEnumOP(Func<DataRow, bool> DFunc)
        {
            //this.ad2019DataSet1.ProductPhoto.getIEnumByContains(idKListppp, "ProductPhotoID");
            var ConBikeq = _AllBikeq.ToList()
                .Where(DFunc);
            List<byte[]> imageList = ConBikeq.getPFKeyList<byte[]>("LargePhoto");
            //List<DateTime> yearList = timeconPPq.getPFKeyList<DateTime>("ModifiedDate");

            _ConBikeq = ConBikeq;
            _imageList = imageList;
        }
        private void SetComboBoxYear(IEnumerable<DataRow> allBikeq)
        {
            List<int> intyearList = allBikeq
                            .Where(row => !Convert.IsDBNull(row["ModifiedDate"]))
                            .Select(row => row.Field<DateTime>("ModifiedDate").Year)
                            .Distinct().OrderBy(x => x).ToList();
            for (int i = 0; i < intyearList.Count; i++)
            {
                //MessageBox.Show(intyearList[i].ToString());
                comboBox3.Items.Add(intyearList[i]);
            }
        }
        private void showLargeImage(List<byte[]> imageList)
        {
            byte[] _image = imageList[_index_RowEnter];
            //MessageBox.Show(_index.ToString());
            if (_image != null)
            {
                Stream streamimage = new MemoryStream((byte[])_image);
                pictureBox1.Image = Bitmap.FromStream(streamimage);
            }
        }
        protected Func<DataRow, bool> SeasonSelector(ComboBox seasonBox)
        {
            //MessageBox.Show(seasonBox.SelectedIndex.ToString());
            switch (seasonBox.SelectedIndex)
            {
                case 0:
                    return SFuncGenerator(3, 5);// s1;
                                                //break;

                case 1:
                    return SFuncGenerator(6, 8);//s2;
                                                //break;

                case 2:
                    return SFuncGenerator(9, 11);//s3;
                                                 //break;

                case 3:
                    return SFuncGenerator(12, 2);//s4;
                                                 //break;

                default:
                    return s4;
                    //break;
            }
        }
        protected Func<DataRow, bool> SFuncGenerator(int min, int max)
        {
            bool check = min <= max;
            (int lhs, int rhs) = (check) ? (min, max) : (max + 1, min - 1);
            return
                (row => (check) ? (row.Field<DateTime>("ModifiedDate").Month >= min &&
                row.Field<DateTime>("ModifiedDate").Month <= max) :
                !(row.Field<DateTime>("ModifiedDate").Month >= min &&
                row.Field<DateTime>("ModifiedDate").Month <= max));
        }

        //private static bool intRangeDateSelect(int min, int max, DataRow row)
        //{
        //    return (row.Field<DateTime>("ModifiedDate").Month >= min &&
        //        row.Field<DateTime>("ModifiedDate").Month <= max);
        //}

        protected bool s1(DataRow row)
        {
            return row.Field<DateTime>("ModifiedDate").Month >= 3 &&
                row.Field<DateTime>("ModifiedDate").Month <= 5;
        }
        protected bool s2(DataRow row)
        {
            return row.Field<DateTime>("ModifiedDate").Month >= 6 &&
                row.Field<DateTime>("ModifiedDate").Month <= 8;
        }
        protected bool s3(DataRow row)
        {
            return row.Field<DateTime>("ModifiedDate").Month >= 9 &&
                row.Field<DateTime>("ModifiedDate").Month <= 11;
        }
        protected bool s4(DataRow row)
        {
            return !(row.Field<DateTime>("ModifiedDate").Month >= 3 &&
                row.Field<DateTime>("ModifiedDate").Month <= 11);
        }
        protected bool SelectDate(DataRow row)
        {
            return row.Field<DateTime>("ModifiedDate").Year.ToString() == comboBox3.Text;
        }
        protected bool RangeDate(DataRow row)
        {
            return (MinDate(row) && MaxDate(row));
        }
        protected bool MaxDate(DataRow row)
        {
            return DateTime.Compare(row.Field<DateTime>("ModifiedDate"), dateTimePicker2.Value) <= 0;
        }
        protected bool MinDate(DataRow row)
        {
            return DateTime.Compare(row.Field<DateTime>("ModifiedDate"), dateTimePicker1.Value) >= 0;
        }
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            _index_RowEnter = e.RowIndex;
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            showLargeImage(_imageList);
        }
    }
}
public static class MyLinqExtenstionHW
{
    public static IEnumerable<DataRow> getIEnumByContains<T>(this IEnumerable<DataRow> source, List<T> list, string FieldName)
    {
        return source
            .Where(pc_row => !Convert.IsDBNull(pc_row[FieldName]) && list.Contains(pc_row.Field<T>(FieldName)));
    }
    public static List<T> getPFKeyList<T>(this IEnumerable<DataRow> source, string PFKeyName)
    {
        return source
            .Where(row => !Convert.IsDBNull(row[PFKeyName]))
            .Select(row => row.Field<T>(PFKeyName)).ToList();
    }
}

