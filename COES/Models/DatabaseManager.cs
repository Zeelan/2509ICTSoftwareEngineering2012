using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace COES.Models
{
    /// <summary>
    /// Manages the database connection and queries.
    /// </summary>
    class DatabaseManager
    {
        SQLiteConnection con;


        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseManager"/> class.
        /// </summary>
        public DatabaseManager()
        {
            String dbFile = "database.s3db";
            string dbConnectionString = String.Format("Data Source={0};Version=3",dbFile);
 
            //check if file exits.... else create and populate
            if (File.Exists(dbFile))
            {
                con = new SQLiteConnection(dbConnectionString);
            }
            else
            {
                // automatically creates a blank database file using the same name
                con = new SQLiteConnection(dbConnectionString);

                //now populate....

                String sql = @"

            DROP TABLE IF EXISTS customer;
CREATE TABLE customer (	customer_id			INTEGER 	PRIMARY KEY ASC,
						first_name 			VARCHAR 	NOT NULL,
						last_name			VARCHAR 	NOT NULL,
						phone_number		VARCHAR     NOT NULL UNIQUE,
						credit_card_name	VARCHAR		NULL,
						credit_card_number	INTEGER		NULL,
						credit_card_expiry	VARCHAR		NULL,
						street_no			VARCHAR		NULL,
						street				VARCHAR		NULL,
						suburb_post_code	VARCHAR		NULL,
						status				VARCHAR		NULL,
						status_date			DATE		NULL,
						created_date		DATE		NULL);

DROP INDEX IF EXISTS customer_n1;					
CREATE INDEX customer_n1 ON customer (phone_number);
						
DROP TRIGGER IF EXISTS customer_ai;
CREATE TRIGGER customer_ai AFTER INSERT ON customer
	BEGIN
		UPDATE customer 
		SET created_date = DATETIME('NOW'), 
		    status_date = DATETIME('NOW'),
			status = 'A'
		WHERE rowid = new.rowid;
	END;
DROP TRIGGER IF EXISTS customer_au;
CREATE TRIGGER customer_au AFTER UPDATE ON customer
	BEGIN
		UPDATE customer 
		SET status_date = DATETIME('NOW')
		WHERE rowid = new.rowid;
	END;


DROP TABLE IF EXISTS customer_order;
CREATE TABLE customer_order ( 	customer_order_id	INTEGER 	PRIMARY KEY ASC,
								customer_id			INTEGER  	NULL,
								order_date			DATE		NULL,
								created_date		DATE		NULL,
								delivery_flag		VARCHAR		NULL,
								paid_status			VARCHAR		NULL,
								status				VARCHAR		NULL,
								total_cost			DECIMAL(4,2)NULL);
DROP TRIGGER IF EXISTS cusotmer_order_ai;
CREATE TRIGGER customer_order_ai AFTER INSERT ON customer_order
	BEGIN
		UPDATE customer_order 
		SET created_date = DATETIME('NOW'), 
            status = 'N',
			paid_status = 'N',
			total_cost = 0
		WHERE rowid = new.rowid;
	END;
								
DROP TABLE IF EXISTS order_item;
CREATE TABLE order_item ( 	order_item_id	INTEGER 	PRIMARY KEY ASC,
							order_id		INTEGER  	NOT NULL,
							menu_item_id	INTEGER  	NOT NULL,
							item_cost		DECIMAL(4,2)NULL);
							


DROP TABLE IF EXISTS menu_item;
CREATE TABLE menu_item (	menu_item_id	INTEGER 	PRIMARY KEY ASC,
							menu_item_name	INTEGER  	NOT NULL,
							description     VARCHAR		NOT NULL,
							status_date		DATE		NULL,
							created_date	DATE		NULL,
							item_cost		DECIMAL(4,2)	NULL);
							
DROP TRIGGER IF EXISTS menu_item_ai;
CREATE TRIGGER menu_item_ai AFTER INSERT ON menu_item
	BEGIN
		UPDATE menu_item 
		SET created_date = DATETIME('NOW'), 
		    status_date = DATETIME('NOW')
		WHERE rowid = new.rowid;
	END;
DROP TRIGGER IF EXISTS menu_item_au;
CREATE TRIGGER menu_item_au AFTER UPDATE ON menu_item
	BEGIN
		UPDATE menu_item 
		SET status_date = DATETIME('NOW')
		WHERE rowid = new.rowid;
	END;
	
DROP TABLE IF EXISTS menu;
CREATE TABLE menu (	menu_id			INTEGER 	PRIMARY KEY ASC,
					menu_item_id	INTEGER  	NOT NULL,
					created_date	DATE		NOT NULL);
							
DROP TRIGGER IF EXISTS menu_ai;
CREATE TRIGGER menu_ai AFTER INSERT ON menu
	BEGIN
		UPDATE menu 
		SET created_date = DATETIME('NOW')
		WHERE rowid = new.rowid;
	END;

DROP TABLE IF EXISTS admin_staff;
CREATE TABLE admin_staff (	admin_staff_id		INTEGER 	PRIMARY KEY ASC,
                            admin_staff_name	VARCHAR		NOT NULL,
							p_word				INTEGER  	NOT NULL,
							status_date     	DATE		NULL,
							created_date		DATE		NULL);
							
DROP TRIGGER IF EXISTS admin_staff_ai;
CREATE TRIGGER admin_staff_ai AFTER INSERT ON admin_staff
	BEGIN
		UPDATE admin_staff 
		SET created_date = DATETIME('NOW'), 
		    status_date = DATETIME('NOW')
		WHERE rowid = new.rowid;
	END;
							
                        
	";		    // end of literal string for sql create of database.
		
                 //run as quickquery
                this.quickQuery(sql);


            }
        }

        /// <summary>
        /// Close the connection<see cref="DatabaseManager"/> class.
        /// </summary>
        ~DatabaseManager()
        {
            con.Close();
        }

        /// <summary>
        /// Performs a given query on a database and returns a <see cref="DataTable"/> containing the result.
        /// </summary>
        /// <param name="query">The SQL query.</param>
        /// <returns>The <see cref="DataTable"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataTable query(string query)
        {
            DataTable resultsTable = new DataTable();
            try
            {
                // Open a connection to the database file.
                
                // Setup command to handle sql.
                SQLiteCommand command = new SQLiteCommand(query, this.con);

                // Read the results then transfer using the reader into a generic DataTable.
                SQLiteDataReader reader = command.ExecuteReader();
                resultsTable.Load(reader);
                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return resultsTable;
        }

        /// <summary> 
        /// used to insert a dictionery of values into the specified table
        /// </summary>
        /// <param name="table">Name of the table to insert data into</param>
        /// <param name="tableData">A dictionery of type String,String  key,value, to insert</param>
        /// <returns>true if successful false if not</returns>
        public bool insert(String table, Dictionary<String, String> tableData)
        {
            // build the string to insert into the database

            String keys = "";
            String values = "";

            // grab a list of keys
            List<String> list = new List<String>(tableData.Keys);

            // check we have list values
            if(list.Count()>0)
            {
                //build strings for sql
                for (int i = 0; i < (list.Count()-1); i++)
                {
                    keys += String.Format(" {0}, ", list[i].ToString());
                    values += String.Format(" '{0}' , ", tableData[list[i].ToString()].ToString());
                }
                // don't add the last comma
                keys += String.Format(" {0} ", list[list.Count()-1].ToString());
                values += String.Format(" '{0}'  ", tableData[(list.Count()-1).ToString()].ToString());
            }

            // construct the complete sql statement
            String sql = String.Format("INSERT INTO {0} ({1}) VALUES ({2}); ",table,keys,values);

            try
            {
                this.quickQuery(sql);
            }
            catch (Exception fail)
            {
                
                return false;
            }
            return true;
        }


        ///
        /// <summary> 
        /// used to run queries that only require a return of true of false.
        /// </summary>
        /// <param name="table">Complete SQL statment to run</param>
        /// <returns>true if successful false if not</returns>
        public int quickQuery(String sql)
        {         
            //Setup the sql command
            SQLiteCommand mycom = new SQLiteCommand(sql,this.con);

            //execute the command
            int result = mycom.ExecuteNonQuery();

            return result;
        }

        /// <summary> 
        /// used to update a dictionery of values into the specified table
        /// </summary>
        /// <param name="table">Name of the table to update data in</param>
        /// <param name="tableData">A dictionery of type String,String  key,value, to update</param>
        /// <returns>true if successful false if not</returns>
        public bool update(String table, Dictionary<String, String> tableData, String where = " 1=1 " )
        {
            // build the string to insert into the database

            String keyvalues = "";
           
            // grab a list of keys
            List<String> list = new List<String>(tableData.Keys);

            // check we have list values
            if (list.Count() > 0)
            {
                //build strings for sql
                for (int i = 0; i < (list.Count() - 1); i++)
                {
                    keyvalues += String.Format(" {0} = '{1}] , ", list[i].ToString(), tableData[list[i].ToString()].ToString());
                }
                // don't add the last comma
                keyvalues += String.Format(" {0} = '{1}] , ", list[(list.Count() - 1)].ToString(), tableData[list[(list.Count() - 1)].ToString()].ToString());
            }

            // construct the complete sql statement
            String sql = String.Format("UPDATE {0} SET ({1}) WHERE ({2}) ; ", table, keyvalues, where);

            try
            {
                this.quickQuery(sql);
            }
            catch (Exception fail)
            {
                return false;
            }
            return true;
        }
    }
}


