using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.Logic
{
    public class CopyToListGeneric<T>
    {
        public static List<T> CopyDataToList(FacebookObjectCollection<T> i_UserFriends)
        {
            List<T> userFriendToCopy = new List<T>();

            foreach (T userFriend in i_UserFriends)
            {
                userFriendToCopy.Add(userFriend);
            }

            return userFriendToCopy;
        }
    }
}
