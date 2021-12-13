using System.Windows;
using System.Windows.Controls;

namespace PROG8050_PROJECT.Styles
{
	public class PasswordBoxMonitor : DependencyObject
	{
		public static readonly DependencyProperty IsMonitoringProperty
			= DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));

		public static readonly DependencyProperty PasswordLengthProperty
			= DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new UIPropertyMetadata(0));

		public static bool GetIsMonitoring(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsMonitoringProperty);
		}

		public static void SetMonitoring(DependencyObject obj, bool value)
		{
			obj.SetValue(IsMonitoringProperty, value);
		}

		public static int GetPasswordLength(DependencyObject obj)
		{
			return (int)obj.GetValue(PasswordLengthProperty);
		}

		public static void SetPasswordLength(DependencyObject obj, int value)
		{
			obj.SetValue(PasswordLengthProperty, value);
		}

		private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = obj as PasswordBox;
			if (passwordBox == null)
			{
				return;
			}

			if ((bool)e.NewValue)
			{
				passwordBox.PasswordChanged += PasswordChanged;
			}
			else
			{
				passwordBox.PasswordChanged -= PasswordChanged;
			}
		}

		private static void PasswordChanged(object sender, RoutedEventArgs e)
		{
			PasswordBox passwordBox = sender as PasswordBox;
			if (passwordBox == null)
			{
				return;
			}
			SetPasswordLength(passwordBox, passwordBox.Password.Length);
		}
	}
}
