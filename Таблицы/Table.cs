using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    public class Table<TOne, TTwo, TValue>
    {
        public Open<TOne, TTwo, TValue> Open { get; }
        public Existed<TOne, TTwo, TValue> Existed { get; }
        public List<TOne> Rows {  get; }
        public List<TTwo> Columns { get; }
        public Dictionary<TOne, Dictionary<TTwo, TValue>> Dictionary = new Dictionary<TOne, Dictionary<TTwo, TValue>>();

        public Table()
        {
            Open = new Open<TOne, TTwo, TValue>(this);
            Existed = new Existed<TOne, TTwo, TValue>(this);     
            Rows = new List<TOne>();
            Columns = new List<TTwo>();
        }

        public void AddRow(TOne row)
        {
            if (!Rows.Contains(row))
            {
                Rows.Add(row);
            }
        }

        public void AddColumn(TTwo colums)
        {
            if (!Columns.Contains(colums))
            {
                Columns.Add(colums);
            }
        }

        public TValue GettingValueDictionary(Table<TOne, TTwo, TValue> table, TOne row, TTwo colums)
        {
            if (table.Dictionary.ContainsKey(row) && table.Dictionary[row].ContainsKey(colums))
            {
                return table.Dictionary[row][colums];
            }
            else
            {
                return default(TValue);
            }
        }

        public void SetValueDictionary(Table<TOne, TTwo, TValue> table, TOne row, TTwo colums, TValue value)
        {
            if (!table.Dictionary.ContainsKey(row))
            {
                table.Dictionary[row] = new Dictionary<TTwo, TValue>();
            }

            if (!(table.Rows.Contains(row) && table.Columns.Contains(colums)))
            {
                table.Dictionary[row][colums] = value;
                table.Rows.Add(row);
                table.Columns.Add(colums);
            }
            else if (table.Rows.Contains(row) && table.Columns.Contains(colums))
            {
                table.Dictionary[row][colums] = value;
            }
        }
    }
    public class Open<TOne, TTwo, TValue>
    {
        public Table<TOne, TTwo, TValue> Table;
        public Open(Table<TOne, TTwo, TValue> table)
        {
            this.Table = table;
        }

        public TValue this[TOne row, TTwo colums]
        {
            get 
            { 
                return Table.GettingValueDictionary(Table, row, colums);
            }
            set
            {
                Table.SetValueDictionary(Table, row, colums, value);
            }
        }
    }

    public class Existed<TOne, TTwo, TValue>
    {
        public Table<TOne, TTwo, TValue> Table;
        public Existed(Table<TOne, TTwo, TValue> table)
        {
            this.Table = table;
        }

        public TValue this[TOne row, TTwo colums]
        {
            get
            {
                if (!Table.Rows.Contains(row)){ throw new ArgumentException(); }
                else if (!(Table.Columns.Contains(colums))) { throw new ArgumentException(); }
                else { return Table.GettingValueDictionary(Table, row, colums); }
            }

            set 
            {
                if (!Table.Rows.Contains(row)) { throw new ArgumentException(); }
                else if (!(Table.Columns.Contains(colums))) { throw new ArgumentException(); }
                else { Table.SetValueDictionary(Table, row, colums, value); }
            }
        }
    }   
}
