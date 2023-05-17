using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    public class LikesSorting
    {
        private const int k_StartAmountOfLikes = 1;

        public LikesSorting(User i_LoggedInUser)
        {
            FriendsLikeCounter = new Dictionary<User, int>();
            LoggedInUser = i_LoggedInUser;
        }

        public Dictionary<User, int> FriendsLikeCounter { get; set; }

        public User LoggedInUser { get; set; }

        public void RunLikeSorter()
        {
            try
            {
                for (int i = 0; i < this.LoggedInUser.Posts.Count; i++)
                {
                    foreach (User user in this.LoggedInUser.Posts[i].LikedBy)
                    {
                        if (this.isFriendInDictionary(user.Name))
                        {
                            this.updateRecordInDictionary(user);
                        }
                        else
                        {
                            this.FriendsLikeCounter.Add(user, k_StartAmountOfLikes);
                        }
                    }
                }

                this.SortedDictionary();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isFriendInDictionary(string i_NameOfFriend)
        {
            bool isExist = false;

            foreach (User user in this.FriendsLikeCounter.Keys)
            {
                if (user.Name.Equals(i_NameOfFriend))
                {
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }

        private void updateRecordInDictionary(User i_UserToUpdate)
        {
            foreach (User userInDictionary in this.FriendsLikeCounter.Keys)
            {
                if (userInDictionary.Name.Equals(i_UserToUpdate.Name))
                {
                    int currentUserAmountOfLikesInDictionary = this.FriendsLikeCounter[userInDictionary];
                    currentUserAmountOfLikesInDictionary++;
                    this.FriendsLikeCounter[userInDictionary] = currentUserAmountOfLikesInDictionary;
                    break;
                }
            }
        }

        private void SortedDictionary()
        {
            try
            {
                this.FriendsLikeCounter.OrderByDescending(likeAmount => likeAmount.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
