using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    //the db entity or database table headings, each product has id and name
    public class Product
    {
        //naming convetion makes the int Id a primary key
        //we added values in the table here by using a query from the sqlite options
        public int Id { get; set; }
        public string Name { get; set; }
    }
}