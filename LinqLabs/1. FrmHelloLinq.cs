using LinqLabs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.LinkLabel;

namespace Starter
{
    public partial class FrmHelloLinq : Form
    {
        public FrmHelloLinq()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //陣列集合是一個  Iterator 物件 迭代器, 可列舉的

            //public interface IEnumerable<T>

            //摘要: 
            //公開支援指定類型集合上簡單反覆運算的列舉值。

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //syntax sugar, 語法糖  string.Format(.............)=>$"{}"
            foreach (int n in nums)
            {
                this.listBox1.Items.Add(n);
            }

            //===================================
            //C# 內部轉譯
            this.listBox1.Items.Add("=================");
            System.Collections.IEnumerator en=   nums.GetEnumerator();
            while( en.MoveNext())
            {
                this.listBox1.Items.Add(en.Current);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            foreach (int n in list)
            {
               this.listBox1.Items.Add(n);
            }
            //===================================
            //C# 內部轉譯
            this.listBox1.Items.Add("=================");
            List<int>.Enumerator en = list.GetEnumerator();

            while( en.MoveNext())
            {
                this.listBox1.Items.Add(en.Current);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int w = 100;

//            嚴重性 程式碼 說明 專案  檔案 行   隱藏項目狀態
//錯誤(作用中)    CS1579 因為 'int' 不包含 'GetEnumerator' 的公用執行個體或延伸模組定義，所以 foreach 陳述式無法在型別 'int' 的變數上運作 LinqLabs    C:\Shared\LINQ\LinqLabs(Solution)\LinqLabs\1.FrmHelloLinq.cs  70

            //foreach (int n in w)
            //{
            //    this.listBox1.Items.Add(n);
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Step 1: define Data Source object 
            //Step 2: define Query
            //Step 3: execute Query

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            //Step 2: define Query
            //define query  (IEnumerable<int> q 是一個  Iterator 物件)　, 如陣列集合一般 (陣列集合也是一個  Iterator 物件)
            //IEnumerable<int> q -  公開支援指定型別集合上簡單反覆運算的列舉值。
            IEnumerable<int> q = from n in nums
                                 //where n > 5
                                 //where n>5 && n<=10
                                 where n%2==0
                                 //where n<3 || n>10
                                 select n;

            //Step 3: execute Query
            //execute query(執行 iterator - 逐一查看集合的item)
            this.listBox1.Items.Clear();
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }


            //======================================
            //NOTE : DataGridView Binding 找屬性
            IEnumerable<string> q1 = from n in nums
                                 where n % 2 == 0
                                 select n.ToString();

            this.dataGridView1.DataSource = q1.ToList();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            IEnumerable<int> q = from n in nums
                                     //where n % 2 == 0
                                 where IsEven(n)
                                 select n;
            this.listBox1.Items.Clear();
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }

         bool   IsEven(int n)
        {
            //if (n%2==0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            return n % 2 == 0;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            IEnumerable<string> q = from n in nums
                                     //where n % 2 == 0
                                  
                                 where IsEven(n)
                                 select "Hello " + n.ToString();
            this.listBox1.Items.Clear();
           
            foreach (string n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            IEnumerable<Point> q = from n in nums
                                    where n > 5
                                    select new Point(n, n * n);

            this.listBox1.Items.Clear();

            //execute query=> foreach (... in q..)
            foreach (Point p in q)
            {
                this.listBox1.Items.Add(p.X + "," + p.Y);
                
            }

            //execute query=>ToXXX()
            List<Point> list = q.ToList();   //foreach (... in q..) {.....}
            this.dataGridView1.DataSource = list;

            //==================
            //Chart
           this.chart1.DataSource = list; 
            this.chart1.Series[0].XValueMember = "X";  //Point  X 屬性
            this.chart1.Series[0].YValueMembers = "Y"; //Point  Y 屬性
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            this.chart1.Series[0].Color = Color.Red;
            this.chart1.Series[0].BorderWidth = 4;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = 100;
            var n1 = 200;


            string[] names = { "aaa", "Apple", "xxxApple", "piniapple", "bbb", "cccccccc" };

            IEnumerable<string> q = from s in names
                                    where s.Length > 4 && s.ToUpper().Contains("APPLE")
                                    orderby s //descending
                                    select s;//.ToUpper();

            foreach (string s in q)
            {
                this.listBox1.Items.Add(s);
            }

        }

      
        private void button12_Click(object sender, EventArgs e)
        {
            // 簡介ORM技術

            //60年代物件導向程式(Object - oriented programming, OOP)被提出後，物件導向逐漸成為所有程式設計師的共通語言。在這幾十年間，物件導向技術逐漸成熟，舉凡對網路的通訊協定、檔案存取、XML文件等操作都已經物件化，讓IT產業的貢獻者能以更接近自然語言的方式討論與創新各種不同領域的資訊系統。

            //Object - Relational Mapping(ORM, O / RM, or O / R mapping)一詞就是將關聯式資料庫映射至物件導向的資料抽象化技術。其理念是將資料庫的內容映射為物件，讓程式開發人員可以用操作物件的方式對資料庫進行操作，而不直接使用SQL語法對資料庫進行操作。讓程式設計師不用管底層的資料庫系統是哪種廠牌或哪個版本的資料庫(如：SQL Server、Oracle、DB2、MySQL、Sybase、DBMaker…)，僅須用同一套語法撰寫存取資料庫的邏輯。當底層資料庫的實作品變更時，由於程式設計師並不直接對資料庫進行操作，因此程式內容幾乎不用修改，也就是降低了物件導向程式與資料庫之間的耦合關係。

            this.productsTableAdapter1.Fill(this.nwDataSet1.Products); //Auto conn.open()=>command.ExecuteXXX()..=> while( sqlDataRader Read...)=conn.Close()

            var q = from p in this.nwDataSet1.Products
                    where  p.UnitPrice > 30 && p.ProductName.StartsWith("M") 
                    select p;

            this.dataGridView1.DataSource =  q.ToList();

            //new class1.class2()
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);

            IEnumerable<NWDataSet.OrdersRow> q = from o in this.nwDataSet1.Orders
                                                 where ! o.IsShipRegionNull() && o.OrderDate.Year == 1997
                                                 orderby o.OrderDate.Month descending
                                                 select o;
            
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            IEnumerable<int> q = from n in nums
                                 where n % 2 == 0
                                 select n;

            //nums.Where<>(.......);
    //        public static System.Collections.Generic.IEnumerable<TSource> Where<TSource>(this System.Collections.Generic.IEnumerable<TSource> source, System.Func<TSource, bool> predicate)
    //System.Linq.Enumerable 的成員

        }
    }
}

public class class1
{
    public class class2
    {

    }
}