using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicFacebookFeatures
{
    public class ListBoxService<T>
    {
        public static void CopyToListBox(ListBox i_ListBox, IEnumerable<T> i_UserInfoToListBox)
        {
            i_ListBox.Items.Clear();
            i_ListBox.DisplayMember = "Name";

            try
            {
                foreach (T item in i_UserInfoToListBox)
                {
                    i_ListBox.Items.Add(item);
                }
            }
            catch
            {
                MessageBox.Show(UIconst.k_ServerNotPoviedeThisInfo);
            }

            if (i_ListBox.Items.Count == 0)
            {
                MessageBox.Show(UIconst.k_ThereIsNoDataToShow);
            }
        }
    }
}
