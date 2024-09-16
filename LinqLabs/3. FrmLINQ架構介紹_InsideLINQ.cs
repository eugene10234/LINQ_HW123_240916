using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmLINQ架構介紹_InsideLINQ : Form
    {
        public FrmLINQ架構介紹_InsideLINQ()
        {
            InitializeComponent();

            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);

        }

        private void button30_Click(object sender, EventArgs e)
        {
            System.Collections.ArrayList arrlist = new System.Collections.ArrayList();
            arrlist.Add(12);
            arrlist.Add(3);

            var q = from n in arrlist.Cast<int>()
                    where n > 3
                    select new { N = n };

            this.dataGridView1.DataSource = q.ToList();


            //DataSet
            //    DataTable 
            //int[] nums = { 1, 2, 3 };
            //nums.Where
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //I. 延遲查詢 (deferred execution)
            //定義時不會估算
            //使用執行時才估算


            int[] nums = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            int i = 0;
            var q = from n in nums
                    select ++i;

            //foreach 執行 Query
            foreach (var v in q)
            {
                listBox1.Items.Add(string.Format("v = {0}, i = {1}", v, i));
            }
            listBox1.Items.Add("===========================================");



            //=======================================================

            i = 0;
            var q1 = (from n in nums
                      select ++i).ToList();

            foreach (var v in q1)
            {
                listBox1.Items.Add(string.Format("v = {0}, i = {1}", v, i));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            // this.productsTableAdapter1.Fill(this.nwDataSet1.Products);

            var q = (from p in this.nwDataSet1.Products
                     where p.UnitPrice > 20
                     orderby p.UnitsInStock descending
                     select p).Take(5);

            this.dataGridView1.DataSource = q.ToList();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var q = nums.MyWhere(n => n > 5);

            // nums.ToExcel("a.xls")

            //this.nwDataSet1.Products.MyWhere
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //When execute query
            //1. foreach
            //2. ToXXX()
            //3. Aggregation Sum()....

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            this.listBox1.Items.Add("Sum = " + nums.Sum());
            this.listBox1.Items.Add("Max = " + nums.Max());
            this.listBox1.Items.Add("Min = " + nums.Min());
            this.listBox1.Items.Add("Avg = " + nums.Average());
            this.listBox1.Items.Add("Count = " + nums.Count());

            //nums.Median()

            //py.Median()
            //py.Mean()

            //=====================================
            this.listBox1.Items.Add("Avg UnitsInStock = " + this.nwDataSet1.Products.Average(p => p.UnitsInStock));
            this.listBox1.Items.Add("Max UnitPrice = " + this.nwDataSet1.Products.Max(p => p.UnitPrice));

        }
    }

}


    public static class MyLinqExtenstion
{
    public static IEnumerable<T>  MyWhere<T>(this IEnumerable<T> source, Func<T, bool> delegateObj)
    {
        foreach (T n in source)
        {
            if (delegateObj(n))
            {
                yield return n;
            }
        }
    }
}