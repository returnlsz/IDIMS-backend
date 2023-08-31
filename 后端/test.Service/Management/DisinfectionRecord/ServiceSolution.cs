﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Module.Entities;
using test.Service.Management.DisinfectionRecord;
using SqlSugar;
using test.Common.Db;

namespace test.Service.Management.DisinfectionRecord
{
    public class ServiceSolution : IDisinfectionService
    {

        public bool AddRecord(disinfection_record record)
        {
            try
            {
                var result = DbContext.db.Insertable(record).ExecuteCommand();
                if (result > 0)
                {
                    Console.WriteLine("Insert successful");
                }
                else
                {
                    Console.WriteLine("Insert failed");
                    Console.WriteLine(record);
                }
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public List<disinfection_record> GetAllRecords()
        {
            var data = DbContext.db.Queryable<disinfection_record>().Select(it => new disinfection_record { date=it.date,pos_id=it.pos_id,disinfection=it.disinfection}).ToList();
            return data;
        }
    }
}
