using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RMDesktopUI.Helpers
{
    class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword",
                typeof(string),
                typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, onBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject d)
        {
            var box = d as PasswordBox;
            if (box != null)
            {
                //this ensures that we've hooked the
                //PasswordChanged event once
                box.PasswordChanged -= PasswordChanged;
                box.PasswordChanged += PasswordChanged;
            }
            return (string)d.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject d, string value)
        {
            if (string.Equals(value, GetBoundPassword(d)))
                return;     //prevent infinite recursion

            d.SetValue(BoundPasswordProperty, value);
        }
        private static void onBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as PasswordBox;

            if (box == null)
                return;

            box.Password = GetBoundPassword(d);
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox password = sender as PasswordBox;

            SetBoundPassword(password, password.Password);
            
            //set cursor pas the last character in the password box
            password.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(password, new object[] { password.Password.Length,0});
        }
    }
}
