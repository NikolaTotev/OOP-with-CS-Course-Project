﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Core;

namespace User_Interface
{
    /// <summary>
    /// Interaction logic for RoomsMenu.xaml
    /// </summary>
    public partial class RoomsMenu : IAppView
    {
        private MainWindow m_CurrentMainWindow;
        private IAppView m_PreviousView;
        private readonly List<RoomListItem> m_Rooms;
        private bool m_CommandModeActive;
        public RoomsMenu()
        {
            InitializeComponent();
            Tbl_HeaderText.Text = MallManager.GetInstance().CurrentMall.Name + " - Rental Spaces";

            Lv_RentalSpaces.SelectionMode = SelectionMode.Multiple;
            m_Rooms = new List<RoomListItem>();
            foreach (Room room in RoomManager.GetInstance().Rooms.Values.ToList())
            {
                RoomListItem itemToAdd = new RoomListItem
                {
                    RoomId = room.Id,
                    Type = room.Type,
                    RoomName = room.Name,
                    NumberOfActivities = room.Activities.Count.ToString(),
                    Description = room.Description,
                    RoomFloor = room.Floor.ToString(),
                    RoomNumber = room.RoomNumber.ToString(),
                    DateCreated = room.CreateDate.ToShortDateString(),
                    LastEdited = room.LastEditDate.ToShortDateString()
                };
                m_Rooms.Add(itemToAdd);
            }
            DataContext = m_Rooms;
            m_CommandModeActive = false;
        }

        public void LoadRentalSpaces()
        {
            Lv_RentalSpaces.Items.Clear();
            foreach (var rentalSpace in RoomManager.GetInstance().Rooms)
            {
                ListViewItem itemToAdd = new ListViewItem {Tag = rentalSpace.Key};

                if (rentalSpace.Value is Room currentRoom)
                {
                    itemToAdd.Content = currentRoom.Name;
                }
                Lv_RentalSpaces.Items.Add(itemToAdd);
            }
        }

        public void SetMainWindow(MainWindow currentWindow)
        {
            m_CurrentMainWindow = currentWindow;
        }

        public void SetPreviousView(IAppView previousView)
        {
            m_PreviousView = previousView;
        }

        public void SetNextView(IAppView nextView)
        {
            //Implement as needed
        }

        public IAppView GetPreviousView()
        {
            return m_PreviousView;
        }

        private void Btn_AddRentalSpace_Click(object sender, RoutedEventArgs e)
        {
            AddRoomMenu addRoom = new AddRoomMenu();
            m_CurrentMainWindow.ChangeViewForward(addRoom, this);
        }

        private void Btn_DeleteRentalSpace_OnClick(object sender, RoutedEventArgs e)
        {
            if (Lv_RentalSpaces.SelectedItems.Count <= 0) return;
            for (var i = Lv_RentalSpaces.SelectedItems.Count - 1; i >= 0; i--)
            {
                RoomListItem itemToDelete = (RoomListItem)Lv_RentalSpaces.SelectedItems[i];
                if (RoomManager.GetInstance().DeleteRoom(itemToDelete.RoomId, MallManager.GetInstance().CurrentMall.Name))
                {
                    m_Rooms.Remove(itemToDelete);
                    Lv_RentalSpaces.Items.Refresh();
                    Img_DeleteInfo.Visibility = Visibility.Hidden;
                }
                else
                {
                    Img_DeleteInfo.Visibility = Visibility.Visible;
                }
            }
        }

        private void Btn_SelectAll_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (RoomListItem item in Lv_RentalSpaces.Items)
            {
                item.IsSelected = true;
            }
        }

        private void Btn_DeselectAllSelected_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (RoomListItem item in Lv_RentalSpaces.Items)
            {
                item.IsSelected = false;
            }
        }

        private void Btn_Back_OnClick(object sender, RoutedEventArgs e)
        {
            m_CurrentMainWindow.ChangeViewBackward(m_PreviousView, this);
        }


        private void RoomItem_OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(((ListViewItem)sender).Content is RoomListItem item))
            {
                return;
            }
            var space = new SpacePage(item.RoomId);
            m_CurrentMainWindow.ChangeViewForward(space, this);
        }

        private void Tb_Search_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (m_CommandModeActive)
            {
                if (Tb_Search.Text.Length > 4 && Tb_Search.Text.StartsWith("$"))
                {
                    if (Tb_Search.Text.StartsWith("$ T"))
                    {
                        string commandParam = Tb_Search.Text.Split(' ')[2];
                        Lv_RentalSpaces.ItemsSource = m_Rooms.Where(room => room.Type == commandParam);
                    }

                    if (Tb_Search.Text.StartsWith("$ DC") && Tb_Search.Text.Contains("/"))
                    {
                        string commandParam = Tb_Search.Text.Split(' ')[2];
                        Lv_RentalSpaces.ItemsSource = m_Rooms.Where(room => room.DateCreated == commandParam);
                    }

                    if (Tb_Search.Text.StartsWith("$ DLE") && Tb_Search.Text.Contains("/"))
                    {
                        string commandParam = Tb_Search.Text.Split(' ')[2];
                        Lv_RentalSpaces.ItemsSource = m_Rooms.Where(room => room.LastEdited == commandParam);
                    }

                    if (Tb_Search.Text.StartsWith("$ RF"))
                    {
                        string commandParam = Tb_Search.Text.Split(' ')[2];
                        Lv_RentalSpaces.ItemsSource = m_Rooms.Where(room => room.RoomFloor == commandParam);
                    }

                    if (Tb_Search.Text.StartsWith("$ RN"))
                    {
                        string commandParam = Tb_Search.Text.Split(' ')[2];
                        Lv_RentalSpaces.ItemsSource = m_Rooms.Where(room => room.RoomNumber == commandParam);
                    }

                    if (Tb_Search.Text.StartsWith("$ NA"))
                    {
                        string commandParam = Tb_Search.Text.Split(' ')[2];
                        Lv_RentalSpaces.ItemsSource = m_Rooms.Where(room => room.NumberOfActivities == commandParam);
                    }
                }
                //else if (Tb_Search.Text.StartsWith("$A"))
                //{

                //}
                else
                {
                    Lv_RentalSpaces.ItemsSource = m_Rooms;
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(Tb_Search.Text) && Tb_Search.Text != "Search")
                {
                    Lv_RentalSpaces.ItemsSource = m_Rooms.Where(room => room.RoomName.StartsWith(Tb_Search.Text));
                }
                else
                {
                    if (m_Rooms != null)
                    {
                        Lv_RentalSpaces.ItemsSource = m_Rooms;
                    }
                }
            }
        }

        //private List<RoomListItem> SecondarySorting(List<string> commands, List<string> parameters)
        //{
        //    List<RoomListItem> sortedList = new List<RoomListItem>();

        //    sortedList = m_Rooms.Where(room=>)
        //}

        private void Tb_Search_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Tb_Search.Text = "";
        }

        private void Tb_Search_OnLostFocus(object sender, RoutedEventArgs e)
        {
            Tb_Search.Text = "Search";
        }

        private void Btn_EnterCommandMode_OnClick(object sender, RoutedEventArgs e)
        {
            m_CommandModeActive = !m_CommandModeActive;
            if (m_CommandModeActive)
            {
                Btn_EnterCommandMode.Background = (Brush)Application.Current.Resources["OrangeButtonHighlight"];
                Lb_CommandModeStatus.Visibility = Visibility.Visible;
                Btn_CommandModeHelp.Visibility = Visibility.Visible;
            }
            else
            {
                Lv_RentalSpaces.ItemsSource = m_Rooms;
                Btn_EnterCommandMode.Background = Brushes.White;
                Lb_CommandModeStatus.Visibility = Visibility.Hidden;
                Btn_CommandModeHelp.Visibility = Visibility.Hidden;
            }
        }
    }
}
