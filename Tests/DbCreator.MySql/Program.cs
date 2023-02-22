using Chinese.Data;
using System;

namespace DbCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sqlite = ApplicationDbContext.UseSqlite("filename=chinese.db"))
            using (var mysql = ApplicationDbContext.UseMySql("server=127.0.0.1;database=chinese"))
            {
            }

            Console.WriteLine("Complete.");
        }
    }
}
