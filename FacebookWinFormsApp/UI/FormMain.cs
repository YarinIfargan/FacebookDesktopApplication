using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection.Emit;
using FacebookWrapper.UI;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;
using BasicFacebookFeatures.Logic;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        private static readonly string[] sr_AgeRange = new string[] { "18-21", "22-25", "26-30", "31-40", "41-50", "51-65", "66+" };
        private readonly string[] r_RequiredPermissions =
        {
        "email",
        "user_age_range",
        "user_birthday",
        "public_profile",
        "user_events",
        "user_friends",
        "user_gender",
        "user_hometown",
        "user_likes",
        "user_link",
        "user_location",
        "user_photos",
        "user_posts",
        "user_videos",
        };

        private DiscoverYourLove m_MatchFinder;
        private LikesSorting m_LikesSorting;
        private User m_LoggedInUser;
        private LoginResult m_LoginResult;

        public FormMain()
        {
            InitializeComponent();
            FacebookWrapper.FacebookService.s_CollectionLimit = 200;
        }

        private void mainForm_Load(object i_Sender, EventArgs i_EventArgs)
        {
            this.CenterToScreen();
            this.Location = new Point(this.Location.X, 0);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private string getUserGender()
        {
            string gender = null;

            try
            {
                gender = m_LoggedInUser.Gender.ToString();
            }
            catch
            {
                MessageBox.Show(UIconst.k_ServerNotPoviedeThisInfo);
            }

            return gender;
        }

        private string getBirthdayDate()
        {
            string birthDay = null;

            try
            {
                birthDay = m_LoggedInUser.Birthday.ToString();
            }
            catch
            {
                MessageBox.Show(UIconst.k_ServerNotPoviedeThisInfo);
            }

            return birthDay;
        }

        private string getEmail()
        {
            string mail = null;
            try
            {
                mail = m_LoggedInUser.Email;
            }
            catch
            {
                MessageBox.Show(UIconst.k_ServerNotPoviedeThisInfo);
            }

            return mail;
        }

        private void setTimeAndDate()
        {
            DateUserLabel.Text = DateTime.Now.ToShortDateString();
        }

        private void setCoverPicture()
        {
            try
            {
                if (m_LoggedInUser.Cover.SourceURL != null)
                {
                    CoverPictureBox.LoadAsync(m_LoggedInUser.Cover.SourceURL);
                }
                else
                {
                    CoverPictureBox.BackColor = Color.White;
                }
            }
            catch
            {
                MessageBox.Show(UIconst.k_ServerNotProvideCoverPhoto);
            }
        }

        private void label1_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void label6_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void tabPage2_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void label3_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void label5_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void logoutButton_Click(object i_Sender, EventArgs i_EventArgs)
        {
            if(m_LoggedInUser != null)
            {
                try
                {
                    FacebookService.Logout(null);
                    MessageBox.Show(UIconst.k_LoggedOutMessage);
                }
                catch
                {
                    MessageBox.Show("Error, unable to logout.");
                    Environment.Exit(0);
                }
                finally
                {
                    this.Hide();
                    FormMain mainScreen = new FormMain();
                    mainScreen.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show(UIconst.k_YouAreNotLoggedIn);
            }
        }

        private void label1_Click_1(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void LivesInLabel_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void GenderLabel_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void label9_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void showMatchedFriends()
        {
            if (m_MatchFinder.MatchFriends.Count == 0)
            {
                MessageBox.Show(UIconst.k_ThereIsNoDataToShow);
            }
            else
            {
                try
                {
                    List<User> singleFriends = SortAndCopyService.getUserSingleFriendTolist(m_MatchFinder.MatchFriends, comboBoxAges.Text);
                    ListBoxService<User>.CopyToListBox(listBoxSingleFriends, m_MatchFinder.MatchFriends);
                }
                catch
                {
                    MessageBox.Show(UIconst.k_ServerNotPovideThisFeature);
                }
            }
        }

        private void getSinglesFreindsButton_Click(object sender, EventArgs e)
        {
            listBoxSingleFriends.Items.Clear();
            listBoxSingleFriends.DisplayMember = "Name";
            try
            {
                if (m_MatchFinder.MatchFriends != null)
                {
                    m_MatchFinder.MatchFriends.Clear();
                }
            }
            catch
            {

            }

            if (m_LoggedInUser != null)

            {
                try
                {
                    if ((checkBoxFemale.Checked || checkBoxMale.Checked) && !comboBoxAges.SelectedText.Equals("Age"))
                    {
                        m_MatchFinder.FindYourLove(m_LoggedInUser.Friends);
                        showMatchedFriends();
                    }
                    else
                    {
                        MessageBox.Show(UIconst.k_NotAllParmametersSelected);
                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show(UIconst.k_ServerNotPovideThisFeature);
                }
            }
            else
            {
                MessageBox.Show(UIconst.k_YouAreNotLoggedIn);
            }
        }

        private void fetchAlbumButton_Click(object i_Sender, EventArgs i_EventArgs)
        {
            fetchAlbumsUser();
        }

        private void fetchAlbumsUser()
        {
            List<Album> userAlbums = null;
            if (m_LoggedInUser != null)
            {
                try
                {
                    userAlbums = CopyToListGeneric<Album>.CopyDataToList(m_LoggedInUser.Albums);

                    if (userAlbums != null)
                    {
                        ListBoxService<Album>.CopyToListBox(listBoxAlbums, userAlbums);
                    }
                }
                catch
                {
                    MessageBox.Show(UIconst.k_ServerNotPoviedeThisInfo);
                }
            }
            else
            {
                MessageBox.Show(UIconst.k_YouAreNotLoggedIn, "Error");
            }
        }

        private void dateUserLabel_Click(object sender, EventArgs e)
        {
        }

        private void fetchAlbumInformation()
        {
            Album selectedAlbum = listBoxAlbums.SelectedItem as Album;
            try
            {
                AlbumCreatedAtDateTimePickerAlbum.Value = selectedAlbum.CreatedTime ?? new DateTime(9998, 12, 31);
            }
            catch
            {
            }
        }

        private void displaySelectedAlbumPicture()
        {
            if (listBoxAlbums.SelectedItems.Count == 1)
            {
                Album selectedAlbum = listBoxAlbums.SelectedItem as Album;

                if (selectedAlbum.PictureAlbumURL != null)
                {
                    PictureAlbumBox.LoadAsync(selectedAlbum.PictureAlbumURL);
                }
                else
                {
                    pictureBoxProfilePicture.Image = pictureBoxProfilePicture.ErrorImage;
                }
            }
        }

        private void friendsFirstNameOrderCheckBox_CheckedChanged(object i_Sender, EventArgs i_EventArgs)
        {
            if (OrderByFnameRadioButton.Checked)
            {
                OrderByLnameRadioButton.Checked = false;
            }
        }

        private void friendsLastNameOrderCheckBox_CheckedChanged(object i_Sender, EventArgs i_EventArgs)
        {
            if (OrderByLnameRadioButton.Checked)
            {
                OrderByFnameRadioButton.Checked = false;
            }
        }

        private void fetchFriendButton_Click(object sender, EventArgs e)
        {
            fetchFriendTolistBox();
        }

        private void fetchFriendTolistBox()
        {
            List<User> friendsUser = null;

            if (m_LoggedInUser != null)
            {
                try
                {
                    if (!OrderByFnameRadioButton.Checked && !OrderByLnameRadioButton.Checked)
                    {
                        friendsUser = CopyToListGeneric<User>.CopyDataToList(m_LoggedInUser.Friends);
                    }
                    else
                    {
                        if (OrderByFnameRadioButton.Checked)
                        {
                            friendsUser = SortAndCopyService.SortFriendByFirstName(m_LoggedInUser.Friends);
                        }

                        if (OrderByLnameRadioButton.Checked)
                        {
                            friendsUser = SortAndCopyService.SortFriendByLastName(m_LoggedInUser.Friends);
                        }
                    }

                    if (friendsUser != null)
                    {
                        ListBoxService<User>.CopyToListBox(listBoxFriends, friendsUser);
                    }
                }
                catch
                {
                    MessageBox.Show(UIconst.k_ServerNotPoviedeThisInfo);
                }
            }
            else
            {
                MessageBox.Show(UIconst.k_YouAreNotLoggedIn, "Error");
            }
        }

        private void fetchGroupsButton_Click(object i_Sender, EventArgs i_EventArgs)
        {
            fetchGroupsNameToListBox();
        }

        private void fetchGroupsNameToListBox()
        {
            listBoxGroups.Items.Clear();
            listBoxGroups.DisplayMember = "Name";
            List<Group> groups = null;

            try
            {
                if (m_LoggedInUser != null)
                {
                    if (!OrderByGroupsRadioButton.Checked)
                    {
                        groups = CopyToListGeneric<Group>.CopyDataToList(m_LoggedInUser.Groups);
                    }
                    else
                    {
                        OrderByGroupsRadioButton.Checked = false;
                        groups = fetchGroupsInOrder();
                    }

                    if (groups != null)
                    {
                        ListBoxService<Group>.CopyToListBox(listBoxGroups, groups);
                    }
                }
                else
                {
                    MessageBox.Show(UIconst.k_YouAreNotLoggedIn);
                }
            }
            catch
            {
                MessageBox.Show(UIconst.k_ServerNotPoviedeThisInfo);
            }
        }

        private List<Group> fetchGroupsInOrder()
        {
            return SortAndCopyService.sortGroup(m_LoggedInUser.Groups);
        }

        private void albumListBox_SelectedIndexChanged(object i_Sender, EventArgs i_EventArgs)
        {
            displaySelectedAlbumPicture();
            fetchAlbumInformation();
        }

        private void orderByFnameRadio_CheckedChanged(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void fetchPostsButton_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void fetchLikedPagesButton_Click(object i_Sender, EventArgs i_EventArgs)
        {
            fetchLikedPages();
        }

        private void fetchLikedPages()
        {
            List<Page> likedPages = null;

            if(m_LoggedInUser != null)
            {
                try
                {
                    if (!orderByPgaesRadioButton.Checked)
                    {
                        likedPages = CopyToListGeneric<Page>.CopyDataToList(m_LoggedInUser.LikedPages);
                    }
                    else
                    {
                        likedPages = SortAndCopyService.copyLikedPagesToListInOrder(m_LoggedInUser.LikedPages);
                        orderByPgaesRadioButton.Checked = false;
                    }

                    if (likedPages != null)
                    {
                        ListBoxService<Page>.CopyToListBox(listBoxLikedPages, likedPages);
                    }

                    if (m_LoggedInUser.LikedPages.Count == 0)
                    {
                        MessageBox.Show(UIconst.k_ThereIsNoDataToShow);
                    }
                }
                catch
                {
                    MessageBox.Show(UIconst.k_ServerNotPoviedeThisInfo);
                }
            }
            else
            {
                MessageBox.Show(UIconst.k_YouAreNotLoggedIn, "Error");
            }
        }

        private void GenderComboBox_SelectedIndexChanged(object i_Sender, EventArgs i_EventArgs)
        {
            this.matchInitializer();
            this.m_MatchFinder.AgeRangeSelected = this.comboBoxAges.SelectedItem.ToString();
        }

        private void matchInitializer()
        {
            if (m_MatchFinder == null)
            {
                m_MatchFinder = new DiscoverYourLove(m_LoggedInUser);
            }
        }

        private void PostButton_Click(object sender, EventArgs e)
        {
            if (m_LoggedInUser != null)
            {
                try
                {
                    Status statusToPost = m_LoggedInUser.PostStatus(PostTextBox.Text);

                    PostTextBox.Clear();
                    MessageBox.Show(UIconst.k_StatusPosted);
                }
                catch
                {
                    MessageBox.Show(UIconst.k_ServerNotPovideThisFeature);
                }
            }
            else
            {
                MessageBox.Show(UIconst.k_YouAreNotLoggedIn, "Error");
            }
        }

        private void FormMain_FormClosed(object i_Sender, FormClosedEventArgs i_EventArgs)
        {
            Application.Exit();
        }

        private void UserBirthdayLabel_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void orderByPagesRadioButton_CheckedChanged(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void GroupsTabPage_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void LoginButton_Click(object i_Sender, EventArgs i_EventArgs)
        {
            Clipboard.SetText("design.patterns.22aa"); /// the current password for Desig Patter

            loginAndInit();
        }

        private void loginAndInit()
        {
            m_LoginResult = FacebookService.Login("607397074173939", r_RequiredPermissions);
            string userAccesToken = m_LoginResult.AccessToken;

            if (!string.IsNullOrEmpty(userAccesToken))
            {
                m_LoggedInUser = m_LoginResult.LoggedInUser;

                fetchUserInfo();
            }
            else if (string.IsNullOrEmpty(userAccesToken))
            {
                MessageBox.Show(m_LoginResult.ErrorMessage, "Login Failed");
            }
        }

        private void fetchUserInfo()
        {
            setTimeAndDate();
            setCoverPicture();
            fetchAgeRange();
            labelUserFullName.Text = m_LoggedInUser.Name;
            pictureBoxProfilePicture.LoadAsync(m_LoggedInUser.PictureNormalURL);
            labelUserBirthday.Text = getBirthdayDate();
            labelUserEmail.Text = getEmail();
            labelUserLivesIn.Text = getLocationName();
            labelUserGender.Text = getUserGender();
            labelTimeUser.Text = DateTime.Now.ToShortTimeString();
        }

        private string getLocationName()
        {
            string location = null;

            try
            {
                location = m_LoggedInUser.Location.Name;
            }
            catch
            {
            }

            return location;
        }

        private void fetchAgeRange()
        {
            foreach (string ageRange in sr_AgeRange)
            {
                comboBoxAges.Items.Add(ageRange);
            }
        }

        private void DateLable_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void FriendListBox_SelectedIndexChanged(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void LikedPagesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxLikedPages.SelectedItems.Count == 1)
            {
                Page selectedLikedPage = listBoxLikedPages.SelectedItem as Page;

                if (selectedLikedPage.PictureNormalURL != null)
                {
                    LikedPagePictureBox.LoadAsync(selectedLikedPage.PictureNormalURL);
                }
            }
        }

        private void GroupsListBox_SelectedIndexChanged(object i_Sender, EventArgs i_EventArgs)
        {
            if (listBoxGroups.SelectedItems.Count == 1)
            {
                Group selectedGroup = listBoxGroups.SelectedItem as Group;
                GroupsPictureBox.LoadAsync(selectedGroup.PictureNormalURL);
            }
        }

        private void LivesInUserLabel_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void FavoriteTeamButton_Click(object i_Sender, EventArgs i_EventArgs)
        {
            fetchFavoriteTeam();
        }

        private void fetchFavoriteTeam()
        {
            listBoxFavoriteTeam.Items.Clear();
            listBoxFavoriteTeam.DisplayMember = "Name";
            List<Page> favoriteTeams = null;

            try
            {
                if (m_LoggedInUser != null)
                {
                    if (!FavoriteTeamOrderRadioButton.Checked)
                    {
                        favoriteTeams = SortAndCopyService.CopyFavoriteTeamsToList(m_LoggedInUser.FavofriteTeams);
                    }
                    else
                    {
                        FavoriteTeamOrderRadioButton.Checked = false;
                        favoriteTeams = SortAndCopyService.SortFavoriteTeamsByName(m_LoggedInUser.FavofriteTeams);
                    }

                    if (favoriteTeams != null)
                    {
                        ListBoxService<Page>.CopyToListBox(listBoxFavoriteTeam, favoriteTeams);
                    }

                    if (listBoxFavoriteTeam.Items.Count == 0)
                    {
                        MessageBox.Show(UIconst.k_ThereIsNoDataToShow);
                    }
                }
                else
                {
                    MessageBox.Show(UIconst.k_YouAreNotLoggedIn);
                }
            }
            catch
            {
                MessageBox.Show(UIconst.k_ServerNotPoviedeThisInfo);
            }
        }

        private void FavoriteTeamListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFavoriteTeam.SelectedItems.Count == 1)
            {
                Page selectedFavoriteTeam = listBoxFavoriteTeam.SelectedItem as Page;
                FavoriteTeamPictureBox.LoadAsync(selectedFavoriteTeam.PictureNormalURL);
            }
        }

        private void ProfilePictureBox_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void Timer_Tick(object sender, EventArgs e, Timer timer)
        {
            labelTimeUser.Text = DateTime.Now.ToLongTimeString();
            timer.Start();
        }

        private void createdAtUserLabel_Click(object i_Sender, EventArgs i_EventArgs)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void labelLivesIn_Click(object sender, EventArgs e)
        {
        }

        private void labelUserLivesIn_Click(object sender, EventArgs e)
        {
        }

        private void LikesSortingTabPage_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void buttonSortLikes_Click(object sender, EventArgs e)
        {
            try
            {
                if(m_LoggedInUser != null)
                {
                    m_LikesSorting = new LikesSorting(this.m_LoggedInUser);
                    m_LikesSorting.RunLikeSorter();
                    showSortedFriendsByLikes();
                }
                else
                {
                    MessageBox.Show(UIconst.k_YouAreNotLoggedIn);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(UIconst.k_ServerNotPovideThisFeature);
            }
        }

        private void showSortedFriendsByLikes()
        {
            this.listBoxLikesSorting.Items.Clear();

            if (this.m_LikesSorting.FriendsLikeCounter.Count == 0)
            {
                listBoxLikesSorting.Items.Add("No likes found");
            }
            else
            {
                foreach (User friend in this.m_LikesSorting.FriendsLikeCounter.Keys)
                {
                    this.listBoxLikesSorting.Items.Add(friend.Name);
                }
            }
        }

        private void listBoxLikesSorting_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
        }

        private void checkBoxMale_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMale.Checked)
            {
                checkBoxFemale.Checked = false;
            }
        }

        private void checkBoxFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFemale.Checked)
            {
                checkBoxMale.Checked = false;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
    }
}
