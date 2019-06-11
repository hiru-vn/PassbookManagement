﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DAO;

namespace MainProgram.Pages.SearchSubPages
{
    /// <summary>
    /// Interaction logic for PassbookViewPage.xaml
    /// </summary>
    public partial class PassbookViewPage : Page
    {
        bool isSearchByName = false;
        public PassbookViewPage()
        {
            InitializeComponent();
        }

        private void TextBox_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty((sender as TextBox).Text))
            {
                this.Button_ClearText.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Button_ClearText.Visibility = Visibility.Visible;
            }
        }

        private void TextBox_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string content = this.Textbox_Search.Text;
                if (!string.IsNullOrEmpty(content))
                {
                    if (isSearchByName)
                        this.ListView.ItemsSource = PassbookDAO.Instance.GetPassInfoByCusName(this.Textbox_Search.Text.Trim()).DefaultView;
                    else
                    {
                        try
                        {
                            int id = int.Parse(this.Textbox_Search.Text.Trim());
                            this.ListView.ItemsSource = PassbookDAO.Instance.GetPassInfoByPassID(id).DefaultView;
                        }
                        catch
                        {
                            this.ListView.Items.Clear();
                        }
                    }
                }
            }
        }

        private void Button_ClearText_Click(object sender, RoutedEventArgs e)
        {
            this.Textbox_Search.Clear();
            (sender as Button).Visibility = Visibility.Hidden;
        }
        private void Search_By_PassbookID(object sender, RoutedEventArgs e)
        {
            HintAssist.SetHint(this.Textbox_Search, "Nhập mã sổ");
            isSearchByName = false;
        }

        private void Search_By_Name(object sender, RoutedEventArgs e)
        {
            HintAssist.SetHint(this.Textbox_Search, "Nhập tên khách hàng");
            isSearchByName = true;
        }
        private void Textbox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Button_LoadList_Click(object sender, RoutedEventArgs e)
        {
            if (isSearchByName)
                this.ListView.ItemsSource = PassbookDAO.Instance.GetPassInfoByCusName(this.Textbox_Search.Text.Trim()).DefaultView;
            else
            {
                try
                {
                    int id = int.Parse(this.Textbox_Search.Text.Trim());
                    this.ListView.ItemsSource = PassbookDAO.Instance.GetPassInfoByPassID(id).DefaultView;
                }
                catch
                {
                    this.ListView.Items.Clear();
                }
            }
        }
    }
}
