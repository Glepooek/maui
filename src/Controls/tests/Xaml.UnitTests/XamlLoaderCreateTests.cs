using System;
using System.Windows.Input;
using Microsoft.Maui.Controls.Core.UnitTests;
using NUnit.Framework;

namespace Microsoft.Maui.Controls.Xaml.UnitTests
{
#pragma warning disable 0618 //retaining legacy call to obsolete code

	[TestFixture]
	public class XamlLoaderCreateTests
	{
		[TearDown]
		public void TearDown()
		{
			Device.PlatformServices = null;
			XamlLoader.FallbackTypeResolver = null;
			XamlLoader.ValueCreatedCallback = null;
			XamlLoader.InstantiationFailedCallback = null;
			Microsoft.Maui.Controls.Internals.ResourceLoader.ExceptionHandler = null;
			Microsoft.Maui.Controls.Xaml.Internals.XamlLoader.DoNotThrowOnExceptions = false;
		}

		[Test]
		public void CreateFromXaml()
		{
			var xaml = @"
				<ContentView xmlns=""http://schemas.microsoft.com/dotnet/2021/maui""
							 xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml""
						 	 x:Class=""Microsoft.Maui.Controls.Xaml.UnitTests.FOO"">
					<Label Text=""Foo""  x:Name=""label""/>
				</ContentView>";

			var view = XamlLoader.Create(xaml);
			Assert.That(view, Is.TypeOf<ContentView>());
			Assert.AreEqual("Foo", ((Label)((ContentView)view).Content).Text);
		}

		[Test]
		public void CreateFromXamlDoesntFailOnMissingEventHandler()
		{
			var xaml = @"
				<Button xmlns=""http://schemas.microsoft.com/dotnet/2021/maui""
						xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml""
						Clicked=""handleClick"">
				</Button>";

			Button button = null;
			Assert.DoesNotThrow(() => button = XamlLoader.Create(xaml, true) as Button);
			Assert.NotNull(button);
		}
	}
#pragma warning restore 0618
}