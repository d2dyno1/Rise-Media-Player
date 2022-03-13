﻿using Rise.App.ViewModels;
using Rise.Common.Extensions;
using System;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Rise.App.Views
{


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaylistDetailsPropertiesPage : Page
    {
        private PlaylistViewModel Playlist;

        public PlaylistDetailsPropertiesPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Playlist = e.Parameter as PlaylistViewModel;
        }

        private async void exportPlaylistArt_Click(object sender, RoutedEventArgs e)
        {
            StorageFile picFile =
                await StorageFile.GetFileFromApplicationUriAsync
                (new Uri(Playlist.Icon));

            FolderPicker folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                await picFile.CopyAsync(folder);
            }
        }

        private async void EditPlaylistIcon_Click(Microsoft.UI.Xaml.Controls.SplitButton sender, Microsoft.UI.Xaml.Controls.SplitButtonClickEventArgs args)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                // Get file thumbnail and make a PNG out of it.
                StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 200);
                await thumbnail.SaveToFileAsync($@"modified-artist-{file.Name}.png");

                var uri = new Uri($@"ms-appdata:///local/modified-artist-{file.Name}.png");

                thumbnail?.Dispose();
                Playlist.Icon = uri.ToString();
            }
        }
    }
}
