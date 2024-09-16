using LinqLabs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Starter
{ 
    // 簡介ORM技術

    //代物件導向程式(Object - oriented programming, OOP)被提出後，物件導向逐漸成為所有程式設計師的共通語言。在這幾十年間，物件導向技術逐漸成熟，舉凡對網路的通訊協定、檔案存取、XML文件等操作都已經物件化，讓IT產業的貢獻者能以更接近自然語言的方式討論與創新各種不同領域的資訊系統。
    //Object - Relational Mapping(ORM, O / RM, or O / R mapping)一詞就是將關聯式資料庫映射至物件導向的資料抽象化技術。其理念是將資料庫的內容映射為物件，讓程式開發人員可以用操作物件的方式對資料庫進行操作，而不直接使用SQL語法對資料庫進行操作。讓程式設計師不用管底層的資料庫系統是哪種廠牌或哪個版本的資料庫(如：SQL Server、Oracle、DB2、MySQL、Sybase、DBMaker…)，僅須用同一套語法撰寫存取資料庫的邏輯。當底層資料庫的實作品變更時，由於程式設計師並不直接對資料庫進行操作，因此程式內容幾乎不用修改，也就是降低了物件導向程式與資料庫之間的耦合關係。

    public partial class FrmLinq_To_Entity : Form
    {
        public FrmLinq_To_Entity()
        {
            InitializeComponent();

            Console.Write("xxx");

           
              this.dbContext.Database.Log = Console.Write;

        }

        //entity data model 特色

        //1. App.config 連接字串
        //2. Package 套件下載, 參考 EntityFramework.dll, EntityFramework.SqlServer.dll
        //3. 導覽屬性 關聯

        //4. DataSet model 需要處理 DBNull; Entity Model  不需要處理 DBNull (DBNull 會被 ignore)
        //5. IQuerable<T> query 執行時會轉成 => T-SQL
 
        
        NorthwindEntities dbContext = new NorthwindEntities();
          
        private void button1_Click(object sender, EventArgs e)
        {

             
            IQueryable<Product> q = from p in dbContext.Products
                    where p.UnitPrice > 30
                    select p;

           this.dataGridView1.DataSource =  q.ToList();

        }

        private void button3_Click(object sender, EventArgs e)
        {
           this.dataGridView1.DataSource =  this.dbContext.Categories.First().Products.ToList();

            MessageBox.Show(this.dbContext.Products.First().Category.CategoryName);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            var q = from p in this.dbContext.Products
                    orderby p.UnitsInStock descending, p.ProductID descending
                    select p;

            this.dataGridView1.DataSource = q.ToList();


            //=================================================
            var q2 = this.dbContext.Products.OrderByDescending(p => p.UnitsInStock)
                                           .ThenByDescending(p => p.ProductID)
                                           .Select(p => new { p.ProductID, p.UnitPrice, p.UnitsInStock, TotalPrice = p.UnitPrice * p.UnitsInStock });

            this.dataGridView2.DataSource = q2.ToList();

        }

        private void button23_Click(object sender, EventArgs e)
        {
            ////============================
            ////自訂 compare logic
            var q3 = dbContext.Products.AsEnumerable().OrderBy(p => p, new MyComparer()).ToList();
            this.dataGridView2.DataSource = q3.ToList();

        }

        class MyComparer : IComparer<Product>
        {

            public int Compare(Product x, Product y)
            {
                if (x.UnitPrice < y.UnitPrice)
                    return -1;
                else if (x.UnitPrice > y.UnitPrice)
                    return 1;
                else
                    return string.Compare(x.ProductName[0].ToString(), y.ProductName[0].ToString(), true);

            }
        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            //===================
            var q2 = from c in this.dbContext.Categories
                     join p in this.dbContext.Products
                     on c.CategoryID equals p.CategoryID
                     select new { c.CategoryID, c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice };
               
            this.dataGridView2.DataSource = q2.ToList();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var q = from p in this.dbContext.Products
                    select new { p.CategoryID, p.Category.CategoryName, p.ProductID, p.ProductName, p.UnitPrice };

            this.dataGridView1.DataSource = q.ToList();

        }

        private void button21_Click(object sender, EventArgs e)
        {
            //inner join
            var q = from c in this.dbContext.Categories
                    from p in c.Products
                    select new { c.CategoryID, c.CategoryName, p.ProductID, p.UnitPrice, p.UnitsInStock };

            this.dataGridView1.DataSource = q.ToList();
            MessageBox.Show("q.count() =" + q.Count());

            //=================================
            this.dbContext.Categories.SelectMany(c => c.Products, (c, p) => new { c.CategoryID, c.CategoryName, p.ProductID, p.UnitPrice, p.UnitsInStock });

            //cross join
            var q2 = from c in this.dbContext.Categories
                     from p in this.dbContext.Products
                     select new { c.CategoryID, c.CategoryName, p.ProductID, p.UnitPrice, p.UnitsInStock };
            MessageBox.Show("q2.count() =" + q2.Count());
            this.dataGridView2.DataSource = q2.ToList();
        }

        private void button14_Click(object sender, EventArgs e)
        {
          
        }
    }
}
