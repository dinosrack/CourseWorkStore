using CourseWorkStore.Models;
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
using System.Windows.Shapes;

namespace CourseWorkStore.MessageBoxes
{
    public partial class DeleteRow : Window
    {
        private readonly object _selectedItem;

        public DeleteRow(object selectedItem)
        {
            InitializeComponent();
            _selectedItem = selectedItem; 
            Loaded += DeleteRow_Loaded;
        }

        private void DeleteRow_Loaded(object sender, RoutedEventArgs e)
        {
            var owner = Owner;
            if (owner != null)
            {
                // Используем актуальные размеры и положение главного окна
                double ownerLeft = owner.Left;
                double ownerTop = owner.Top;
                double ownerWidth = owner.ActualWidth;
                double ownerHeight = owner.ActualHeight;

                // Вычисляем положение для окна
                Left = ownerLeft + ownerWidth - Width - 50; // Отступ от правого края
                Top = ownerTop + 70; // Отступ от верхнего края

                // Если окно развернуто на весь экран, корректируем положение
                if (owner.WindowState == WindowState.Maximized)
                {
                    // Получаем размеры рабочей области экрана
                    double screenWidth = SystemParameters.WorkArea.Width;
                    double screenHeight = SystemParameters.WorkArea.Height;

                    Left = screenWidth - Width - 50; // Отступ от правого края экрана
                    Top = 70; // Отступ от верхнего края экрана
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
