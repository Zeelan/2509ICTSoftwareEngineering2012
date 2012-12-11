using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Data;

namespace COES.Models
{
    public class MenuItem : ObservableObject
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private int _id;
        private string _name;
        private string _description;
        private double _cost;
        private DateTime _dateCreated;
        private string _status;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { Set(() => Id, ref _id, value); }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { Set(() => Description, ref _description, value); }
        }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        public double Cost
        {
            get { return _cost; }
            set { Set(() => Cost, ref _cost, value); }
        }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { Set(() => DateCreated, ref _dateCreated, value); }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { Set(() => Status, ref _status, value); }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        public MenuItem()
        {

        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Methods ---
        //----------------------------------------------------------------------
        public override string ToString()
        {
            return String.Format("{0} {1}", Cost, Name);
        }


        public bool loadID(int id)
        {

            DatabaseManager dbm = new DatabaseManager();

            String sql = string.Format("select * from menu_item where menu_item_id = {0} ;",id.ToString());
            DataTable dt = dbm.query(sql);

            if (dt.Rows.Count > 0)
            {
                // ok i'll use a foreach :-(
                foreach (DataRow dr in dt.Rows)
                {
                    this.Cost = double.Parse(dr["item_cost"].ToString());
                    this.Id = int.Parse(dr["menu_item_id"].ToString());
                    this.Description = dr["description"].ToString();
                    this.Name = dr["menu_item_name"].ToString();
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------
    }
}
