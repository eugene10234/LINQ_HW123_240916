using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace Starter
{

    //Notes: LINQ 主要參考 
    //組件 System.Core.dll,
    //namespace {}  System.Linq
    //public static class Enumerable
   
    //public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);

    //1. 泛型 (泛用方法)                                          (ex.  void SwapAnyType<T>(ref T a, ref T b)
    //2. 委派參數 Lambda Expression (匿名方法簡潔版)               (ex.  MyWhere(nums, n => n %2==0);
    //3. Iterator                                                (ex.  MyIterator)
    //4. 擴充方法                                                 (ex. WordCount() Chars(2)
    //
    public partial class FrmLangForLINQ : Form
    {
        public FrmLangForLINQ()
        {
            InitializeComponent();
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //int[] nums = { 1, 2, 3 };
            //nums.Max()

            int n1, n2;
            n1 = 100;
            n2 = 200;

            MessageBox.Show(n1 + "," + n2);

          
            Swap(ref n1, ref n2);  //FrmLangForLINQ.Swap(ref n1, ref n2);
            //MessageBox.Show(SystemInformation.ComputerName);
            //Application.StartupPath

            MessageBox.Show(n1 + "," + n2);

            //==============================

            string s1 = "aaa", s2 = "bbb";
            MessageBox.Show(s1 + "," + s2);

            Swap(ref s1, ref s2);
            MessageBox.Show(s1 + "," + s2);

        }

        private static void Swap(ref int n1, ref int n2)
        {
            int temp = n2;
            n2 = n1;
            n1 = temp;
        }

        private static void Swap(ref string n1, ref string n2)
        {
            string temp = n2;
            n2 = n1;
            n1 = temp;
        }

        //..........
        private static void Swapxxx(ref object n1, ref object n2)
        {
            object temp = n2;
            n2 = n1;
            n1 = temp;
        }

        private static void SwapAnyType<T>(ref T n1, ref T n2)
        {
            T temp = n2;
            n2 = n1;
            n1 = temp;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            int n1, n2;
            n1 = 100;
            n2 = 200;
            MessageBox.Show(n1 + "," + n2);

            SwapAnyType<int>(ref n1, ref n2);
            MessageBox.Show(n1 + "," + n2);
            //==============================
            string s1 = "aaa", s2 = "bbb";
            MessageBox.Show(s1 + "," + s2);

            // SwapAnyType<string>(ref s1, ref s2);
            SwapAnyType(ref s1, ref s2); //推斷型別
            MessageBox.Show(s1 + "," + s2);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //具名方法
            //this.buttonX.Click += new EventHandler( ButtonX_Click);
            this.buttonX.Click += ButtonX_Click;

            // NOTE:           嚴重性 程式碼 說明 專案  檔案 行   隱藏項目狀態
            //錯誤(作用中)    CS0123  'aaa' 沒有任何多載符合委派 'EventHandler' LinqLabs C:\Shared\LINQ\LinqLabs(Solution)\LinqLabs\2.FrmLangForLINQ.cs    102

            this.buttonX.Click += aaa;

            //================================
            //C# 2.0 匿名方法
            this.buttonX.Click += delegate (object sender1, EventArgs e1)
                                          {
                                              MessageBox.Show("匿名方法");
                                          };

            //===============================
            //匿名方法 C# 3.0 lambda => goes to
            this.buttonX.Click += (object sender1, EventArgs e1) =>
                                    {
                                        MessageBox.Show("匿名方法 簡潔版");
                                    };

        }



        private void ButtonX_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ButtonX click");
        }

        private void aaa(object sender, EventArgs e)
        {
            MessageBox.Show("aaa");
        }

         bool Test (int n)
        {
            return n > 5;
        }
        bool Test2(int n, int n2)
        {
            return n > 5;
        }

        bool IsEven(int n)
        {
            return n %2==0;
        }

        bool IsOdd(int n)
        {
            return n % 2 == 1;
        }

        //Step 1: create delegate class
        //Step 2: create delegate object
        //Step 3: Invoke method / call method

        delegate bool MyDelegate(int n);

        private void button9_Click(object sender, EventArgs e)
        {
            bool result;
            result = Test(3);

            //============================
            //具名方法
            MyDelegate delegateObj  =  new MyDelegate(Test);
            result =  delegateObj(7);

            //===========================

            delegateObj = IsEven;
            //result = delegateObj(7);

            result = delegateObj.Invoke(7);

            //=========================
            delegateObj = IsOdd;
            result = delegateObj.Invoke(7);
          

            //================================
            //C# 2.0 匿名方法
            delegateObj = delegate (int n)
                                    {
                                        return n > 5;
                                    };

            result = delegateObj(3);

            //====================================
            //C# 3.0 匿名方法簡潔板 labmda expression =>
            //Lambda 運算式是建立委派最簡單的方法 (參數型別也沒寫 / return 也沒寫 => 非常高階的抽象)

            delegateObj = n => n > 5;
            result = delegateObj(8);

            MessageBox.Show("result = " + result);

        }

        List<int> MyWhere(int[] nums, MyDelegate delegateObj)
        {
            List<int> list = new List<int>();
           foreach (int n in nums)
            {
                if ( delegateObj(n))
                {
                  list.Add(n);
                }
                
            }
            return list;
        }

        int Add(int n1, int n2)
        {
            return n1 + n2;

        }
        private void button10_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 ,11, 13, 101};
            MyWhere(nums, Test);
            //MyWhere(nums, Test2);

           List<int> largeList =   MyWhere(nums, n => n > 5);
            List<int> EvenList = MyWhere(nums, n => n%2==0);
            List<int> OddList = MyWhere(nums, n => n % 2 == 1);

            foreach (int n in EvenList)
            {
                this.listBox1.Items.Add(n);
            }
            foreach (int n in OddList)
            {
                this.listBox2.Items.Add(n);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //IEnumerable<int> q = from n in nums
            //                        where n > 5
            //                        select n;

            IEnumerable<int> q = nums.Where(n => n > 5);

            foreach(int n in q)
            {
                this.listBox1.Items.Add(n);
            }

            //==============================

            string[] words = { "aaa", "bbbbb", "cccc" };

            IEnumerable<string> q2 = words.Where<string>(w => w.Length > 3);

            foreach (string s in q2)
            {
                this.listBox2.Items.Add(s);
            }

            
        }


        IEnumerable<int> MyIterator(int[] nums, MyDelegate delegateObj)
        {
            foreach (int n in nums)
            {
                if (delegateObj(n))
                {
                    yield return n;
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {

            //TODO ......
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            IEnumerable<int> q = MyIterator(nums, n => n > 5);

            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            bool b;
            b = true;
            b = false;
          
            //=========
            bool? result;
            result = true;
            result = false;
            result = null;

            //if (result.HasValue)


        }

       // var x = 100;

        private void button45_Click(object sender, EventArgs e)
        {
            //var 懶得寫(x)
            //========================
            //var 型別難寫
            //var for 匿名型別

            int n = 100;

            var n1 = 200;

            var s = "abc";
            s = s.ToUpper();
         
            MessageBox.Show(s);

            var p = new Point(100, 100);
            MessageBox.Show(p.X + "," + p.Y);

        }

        private void button41_Click(object sender, EventArgs e)
        {
            //Point pt1 = new Point(1, 1);
            Font f = new Font("Arial", 14);
            //==============================

            MyPoint pt1 = new MyPoint(9, 9);
            //pt1.P1 = 100;  //set
            //pt1.P2 = 200;  //set

            List<MyPoint> list = new List<MyPoint>();
            list.Add(pt1);
            list.Add(new MyPoint(11));     //() constructor 建構子方法
            list.Add(new MyPoint(22, 22));

            list.Add(new MyPoint { P1 = 33, P2 = 33, Field1 = 33 });   //{ } 物件初始化
            list.Add(new MyPoint { P1 = 1111, P2=99 });                       //{ } 物件初始化
           

            this.dataGridView1.DataSource = list;

            //=================================
            List<MyPoint> list2 = new List<MyPoint>
                                        {
                                            new MyPoint {P1=3, P2=3 },
                                            new MyPoint {P1=31, P2=3 },
                                            new MyPoint {P1=33, P2=3 },
                                            new MyPoint {P1=3444, P2=3 },
                                        };

            this.dataGridView2.DataSource = list2;
        }

        private void button43_Click(object sender, EventArgs e)
        {
            var  pt1 = new { P1 = 100, P2 = 200, P3=999 };
            var pt2 = new { P1 = 100, P2 = 200, P3 = 999 };
            var pt3 = new {X=7, Y=8 };


            //MessageBox.Show(pt1.P2.ToString());
            //MessageBox.Show(pt3.X.ToString());

            //pt1.P2 = 999;
            //SystemInformation.ComputerName = "xxx";

            //=====================================
            this.listBox1.Items.Add(pt1.GetType());
            this.listBox1.Items.Add(pt2.GetType());

            this.listBox1.Items.Add(pt3.GetType());

            //======================================
            //TODO ,.....
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //var q = from n in nums
            //        where n > 5
            //        select new { N = n, Square = n * n, Cube = n * n * n };

            var q = nums.Where(n => n > 5).Select(n => new { N = n, Square = n * n, Cube = n * n * n });

            this.dataGridView1.DataSource = q.ToList();

            //========================================

            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);

            var q2 = from p in this.nwDataSet1.Products
                     where p.UnitPrice > 30
                     select new 
                     {
                         ID = p.ProductID,
                         產品名稱 = p.ProductName,
                         p.UnitPrice,
                         p.UnitsInStock,
                         TotalPrice =$"{p.UnitPrice * p.UnitsInStock:c2}"  
                     };

            var q2x = this.nwDataSet1.Products.Where(p => p.UnitPrice > 30)
                                              .Select(p => new
                                              {
                                                  ID = p.ProductID,
                                                  p.ProductName,
                                                  p.UnitPrice,
                                                  p.UnitsInStock,
                                                  TotalPrice = $"{p.UnitPrice * p.UnitsInStock:c2}"
                                              });
            this.dataGridView2.DataSource = q2.ToList();

        }

        private void button40_Click(object sender, EventArgs e)
        {
            //具名型別陣列
            Point[] pts = new Point[]{
                                 new Point(10,10),
                                 new Point(20, 20)
                                };

            //匿名型別陣列
            var arr = new [] {
                                new { x = 1, y = 1 },
                                new { x = 2, y = 2 }
                             };


            foreach (var item in arr)
            {
                listBox1.Items.Add(item.x + ", " + item.y);

            }
            this.dataGridView1.DataSource = arr;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            string s = "abcde";

            int count = s.WordCount();

            MessageBox.Show("count = " + count);

            //=================
            string s1 = "123456789";
            MessageBox.Show("s1 count =" + s1.WordCount());

            //========================
            char ch=  s1.Chars(2);
            MessageBox.Show("ch = " + ch);

            //
            // s1.Chars(2)

            ch = MyStringExtend.Chars(s1, 2);

            MessageBox.Show("ch = " + ch);

            //this.nwDataSet1.Products.WriteExcel("xxx.xlx")
            //this.nwDataSet1.Products.WriteXml(....);
        }
    }
}

public static class MyStringExtend
{

     public static int WordCount(this string s)
    {
        return s.Length;
    }

    public static char Chars(this string s,  int index)
    {
        return s[index];
    }
}

//嚴重性 程式碼	說明	專案	檔案	行	隱藏項目狀態
//錯誤 (作用中)	CS0509	'MyString': 無法衍生自密封類型 'string'	LinqLabs	C:\Shared\LINQ\LinqLabs (Solution)\LinqLabs\2. FrmLangForLINQ.cs	477	

//class MyString :String
//{
    
//}


public class MyPoint
{

    public MyPoint()
    {

    }
    public MyPoint(int p1)
    {
        this.P1 = p1;
    }

    public MyPoint(int p1, int p2)
    {
        P1 = p1;
        this.P2 = p2;
    }


    private int m_p1;
    public int P1
    {
        get
        {
            //logic ......
            return m_p1;
        }
        set
        {
            //logic....value.....
            m_p1 = value;
        }
    }

    public int P2 { get; set; }

    //class Variable
    public int Field1 = 999;
    public int Field2 = 888;

}

