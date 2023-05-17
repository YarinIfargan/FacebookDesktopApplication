using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;
using BasicFacebookFeatures.Logic;
using Newtonsoft.Json;

namespace BasicFacebookFeatures
{
    public class DiscoverYourLove
    {
        private const int k_StartYearSubStringIndex = 6;
        private const int k_MaxAgeRange = 120;
        private const string k_DbName = @"\..\..\..\db.json";
        private List<User> m_MatchFriends = new List<User>();

        public DiscoverYourLove(User i_LoggedInUser)
        {
            this.SelectedGender = new Dictionary<string, bool>();
            readDataFromLocalDb();
        }

        public List<DbProperties> DbPropertiesList { get; set; }

        public string AgeRangeSelected { get; set; }

        public Dictionary<string, bool> SelectedGender { get; set; }

        public List<User> MatchFriends
        {
            get
            {
                return m_MatchFriends;
            }
        }

        public void FindYourLove(FacebookObjectCollection<User> i_UserFriends)
        {
            foreach (User friend in i_UserFriends)
            {
                if (isInAgeRange(friend))
                {
                    MatchFriends.Add(friend);
                }
            }
        }

        private bool isInAgeRange(User i_Friend)
        {
            bool isInRange = false;
            int friendAge = parseBirthdayToAge(i_Friend.Birthday);
            int maxAge;
            int minAge;

            this.parseAgeRange(AgeRangeSelected, out minAge, out maxAge);
            isInRange = isFriendInRange(friendAge, minAge, maxAge);
            return isInRange;
        }

        private void parseAgeRange(string i_AgeRange, out int o_MinAge, out int o_MaxAge)
        {
            int index = 0;
            o_MaxAge = 0;
            o_MinAge = 0;

            while (index < i_AgeRange.Length)
            {
                if (i_AgeRange[index] == '-')
                {
                    o_MinAge = int.Parse(i_AgeRange.Substring(0, index));
                    o_MaxAge = int.Parse(i_AgeRange.Substring(index + 1));

                    break;
                }

                if (i_AgeRange[index] == '+')
                {
                    o_MaxAge = int.Parse((i_AgeRange.Substring(0, index)));
                }
                else
                {
                    index++;
                }
            }
        }

        private bool isFriendInRange(int i_FriendAge, int i_MinAge, int i_MaxAge)
        {
            bool isInRange = false;

            if (i_FriendAge >= i_MinAge && i_FriendAge <= i_MaxAge)
            {
                isInRange = true;
            }

            return isInRange;
        }

        private int parseBirthdayToAge(string i_Birthday)
        {
            int age;
            bool isParseSuccessfull = int.TryParse(i_Birthday.Substring(k_StartYearSubStringIndex), out age);

            if (isParseSuccessfull)
            {
                age = DateTime.Now.Year - age;
            }
            else
            {
                throw new Exception("Unsuccessfull parse in parseBirthdayToAge method");
            }

            return age;
        }

        private void readDataFromLocalDb()
        {
            try
            {
                string exeDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string jsonDir = exeDir + k_DbName;
                string json = File.ReadAllText(jsonDir);
                DbPropertiesList = JsonConvert.DeserializeObject<List<DbProperties>>(json);
            }
            catch
            {
                DbPropertiesList = new List<DbProperties>();
            }
        }
    }
}
