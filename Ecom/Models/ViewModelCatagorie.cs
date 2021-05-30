using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecom.Models
{
    public class ViewModelCatagorie
    {
        public Catagorie Category { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> ChildCategories { get; set; }

        public List<SelectListItem> AllCategories
        {
            get
            {
                List<SelectListItem> sli = new List<SelectListItem>();
                foreach (SelectListItem item in Categories)
                {
                    sli.Add(item);
                }
                foreach (SelectListItem item in ChildCategories)
                {
                    int i = item.Text.IndexOf(">");
                    string text = item.Text.Substring(0, i - 1);
                    foreach (SelectListItem listItem in ChildCategories)
                    {
                        int k = listItem.Text.LastIndexOf(">");
                        string text2 = listItem.Text.Substring(k + 1);
                        if (text2.Contains(text))
                        {
                            item.Text = listItem.Text + " > " + item.Text.Substring(i + 2);
                        }
                    }
                    sli.Add(item);
                }
                return sli;
            }
        }
    }
}