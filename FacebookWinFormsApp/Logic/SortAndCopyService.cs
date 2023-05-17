using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicFacebookFeatures.Logic;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures
{
    public class SortAndCopyService
    {
        public static List<User> SortFriendByFirstName(FacebookObjectCollection<User> i_UserFriends)
        {
            List<User> userFriends = CopyToListGeneric<User>.CopyDataToList(i_UserFriends);

            userFriends.Sort(delegate(User firstFriend, User secondFriend)
            {
                return firstFriend.Name.CompareTo(secondFriend.Name);
            });

            return userFriends;
        }

        public static List<Page> CopyFavoriteTeamsToList(Page[] i_UserFavoriteTeams)
        {
            List<Page> userFavoriteTeamsToCopy = new List<Page>();

            foreach (Page team in i_UserFavoriteTeams)
            {
                userFavoriteTeamsToCopy.Add(team);
            }

            return userFavoriteTeamsToCopy;
        }

        public static List<Page> SortFavoriteTeamsByName(Page[] i_UserFavoriteTeams)
        {
            List<Page> userFavoriteTeams = CopyFavoriteTeamsToList(i_UserFavoriteTeams);

            userFavoriteTeams.Sort(delegate(Page firstFavoriteTeam, Page secondFavoriteTeam)
            {
                return firstFavoriteTeam.Name.CompareTo(secondFavoriteTeam.Name);
            });

            return userFavoriteTeams;
        }

        public static List<User> SortFriendByLastName(FacebookObjectCollection<User> i_UserFriends)
        {
            List<User> userFriends = CopyToListGeneric<User>.CopyDataToList(i_UserFriends);

            userFriends.Sort(delegate(User firstFriend, User secondFriend)
            {
                return firstFriend.LastName.CompareTo(secondFriend.LastName);
            });

            return userFriends;
        }

        public static List<Page> sortPages(FacebookObjectCollection<Page> i_UserPages)
        {
            List<Page> userPages = CopyToListGeneric<Page>.CopyDataToList(i_UserPages);
            userPages.Sort(delegate(Page firstPage, Page secondPage)
            {
                return firstPage.Name.CompareTo(secondPage.Name);
            });

            return userPages;
        }

        public static List<Group> sortGroup(FacebookObjectCollection<Group> i_UserGroups)
        {
            List<Group> userPages = CopyToListGeneric<Group>.CopyDataToList(i_UserGroups);
            userPages.Sort(delegate(Group firstGroup, Group secondgroup)
            {
                return firstGroup.Name.CompareTo(secondgroup.Name);
            });

            return userPages;
        }

        public static List<Page> copyLikedPagesToListInOrder(FacebookObjectCollection<Page> i_UserLikedPages)
        {
            List<Page> userPages = CopyToListGeneric<Page>.CopyDataToList(i_UserLikedPages);
            userPages.Sort(delegate(Page firstPage, Page secondPage)
            {
                return firstPage.Name.CompareTo(secondPage.Name);
            });

            return userPages;
        }

        public static List<User> getUserSingleFriendTolist(List<User> i_UserFriends, string i_UserDesireAge)
        {
            List<User> userSingleFriends = new List<User>();

            foreach (User friend in i_UserFriends)
            {
                userSingleFriends.Add(friend);
            }

            return userSingleFriends;
        }
    }
}
