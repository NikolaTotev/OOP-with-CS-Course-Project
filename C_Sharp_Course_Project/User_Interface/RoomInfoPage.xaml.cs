﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Core;

namespace User_Interface
{
    /// <summary>
    /// Interaction logic for RoomInfoPage.xaml
    /// </summary>
    public partial class RoomInfoPage : IAppView
    {
        private MainWindow m_CurrentMainWindow;
        private IAppView m_PreviousView;
        private readonly Room m_CurrentRoom;
        private readonly StringBuilder m_StringBuilder = new StringBuilder();
        private bool m_InputOk;
        private int m_Floor;
        private int m_Number;
        public RoomInfoPage(Room currentRoom)
        {
            InitializeComponent();
            m_CurrentRoom = currentRoom;

            m_StringBuilder.Append(m_CurrentRoom.Name);
            m_StringBuilder.Append(" - Information");
            Tbl_HeaderText.Text = m_StringBuilder.ToString();
            Tb_Name.Text = m_CurrentRoom.Name;
            Tb_Desc.Text = m_CurrentRoom.Description;
            Tb_Floor.Text = m_CurrentRoom.Floor.ToString();
            Tb_Number.Text = m_CurrentRoom.RoomNumber.ToString();
            Lb_DateCreated.Content = m_CurrentRoom.CreateDate.ToShortDateString();
            Lb_LastEdited.Content = m_CurrentRoom.LastEditDate.ToShortDateString();

            foreach (var type in RoomManager.GetInstance().GetRoomTypes())
            {
                Cmb_RoomType.Items.Add(type);
            }
            Cmb_RoomType.Text = m_CurrentRoom.Type;
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

        private void Btn_Close_OnClick(object sender, RoutedEventArgs e)
        {
            m_CurrentMainWindow.ChangeViewBackward(m_PreviousView, this);
        }

        private void Btn_Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_InputOk)
            {
                RoomManager.GetInstance().EditRoom(MallManager.GetInstance().CurrentMall.Name, m_CurrentRoom.Id, Tb_Name.Text, Tb_Desc.Text, Cmb_RoomType.SelectionBoxItemStringFormat, m_Floor, m_Number);
                m_CurrentMainWindow.ChangeViewBackward(m_PreviousView, this);
            }
        }

        private void Btn_Back_OnClick(object sender, RoutedEventArgs e)
        {
            m_CurrentMainWindow.ChangeViewBackward(m_PreviousView, this);
        }

        private void Tb_Floor_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(Tb_Floor.Text, out m_Floor))
            {
                Tb_Floor.Background = (Brush)Application.Current.Resources["InputError"];
                Tb_Floor.BorderBrush = Brushes.Transparent;
                m_InputOk = false;
            }
            else
            {
                Tb_Floor.Background = Brushes.White;
                m_InputOk = true;
            }
        }

        private void Tb_Number_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(Tb_Number.Text, out m_Number))
            {
                Tb_Number.Background = (Brush)Application.Current.Resources["InputError"];
                Tb_Number.BorderBrush = Brushes.Transparent;
                m_InputOk = false;
            }
            else
            {
                Tb_Number.Background = Brushes.White;
                m_InputOk = true;
            }
        }
    }
}
