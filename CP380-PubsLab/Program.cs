using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CP380_PubsLab.Models;

namespace CP380_PubsLab
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbcontext = new Models.PubsDbContext())
            {
                if (dbcontext.Database.CanConnect())
                {
                    Console.WriteLine("Yes, I can connect");
                }


                // 1:Many practice
                //
                // TODO: - Loop through each employee
                //       - For each employee, list their job description (job_desc, in the jobs table)

                var q = dbcontext.Employee;
                Console.WriteLine($"{q.ToQueryString()}");
                foreach (var e in q.Include(e => e.Job))
                {
                    Console.WriteLine($"{e.Emp_id} -> {e.Job.Desc}");
                }

                // TODO: - Loop through all of the jobs
                //       - For each job, list the employees (first name, last name) that have that job
                foreach (var a in dbcontext.Jobs.Include(d => d.Employee))
                {
                    var employeesjob = a.Employee.Select(d => new { Emp_id = d.Emp_id, Fname = d.Firstname, Lname = d.Lastname, Job_Desc = a.Desc }).ToList();
                    var empl_Job = String.Join(",", employeesjob);
                    Console.WriteLine($"{a.Desc} -> {empl_Job}");
                }

                // Many:many practice
                //
                // TODO: - Loop through each Store
                //       - For each store, list all the titles sold at that store
                //
                // e.g.
                //  Bookbeat -> The Gourmet Microwave, The Busy Executive's Database Guide, Cooking with Computers: Surreptitious Balance Sheets, But Is It User Friendly?

                var storeTitles = dbcontext.Stores.Select
                    (
                    s => new { Store = s.stor_name, Titles = s.Sales.Select(sl => sl.Title.title) }).ToList();

                Console.WriteLine("\n#Stores | Titles ");
                foreach (var store in storeTitles)
                {
                    var strJoin = String.Join(",", store.Titles);
                    Console.WriteLine($"\n{store.Store} -. {strJoin}");
                }


                // TODO: - Loop through each Title
                //       - For each title, list all the stores it was sold at
                //
                // e.g.
                //  The Gourmet Microwave -> Doc-U-Mat: Quality Laundry and Books, Bookbeat

                var Titlestore = dbcontext.Titles.Select(t => new { TitleName = t.title, StoreList = t.Sales.Select(sl => sl.Store.stor_name) }).ToList();

                Console.WriteLine("\n#Titles | Stores ");
                foreach (var title in Titlestore)
                {
                    var strJoin = String.Join(",", title.StoreList);
                    Console.WriteLine($"\n{title.TitleName} -. {strJoin}");
                }

            }
        }
            }
        }
    

