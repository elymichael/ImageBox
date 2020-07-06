using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ImageBox.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaService))]
namespace ImageBox.Droid
{
    public class MediaService: IMediaService
    {
        private int CalculateInSampleSize(BitmapFactory.Options options, float reqWidth, float reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
                while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return (int)inSampleSize;
        }
        public byte[] ResizeImage(string filename, float width, float height)
        {
            if (File.Exists(filename))
            {
                byte[] imageData = File.ReadAllBytes(filename);
                // Load the bitmap 
                BitmapFactory.Options options = new BitmapFactory.Options();// Create object of bitmapfactory's option method for further option use
                options.InJustDecodeBounds = true;
                Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);

                var originalHeight = options.OutHeight;
                var originalWidth = options.OutWidth;

                // Calculate inSampleSize
                options.InSampleSize = CalculateInSampleSize(options, width, height);

                // Decode bitmap with inSampleSize set
                options.InJustDecodeBounds = false;

                Bitmap resizedImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);                

                using (MemoryStream ms = new MemoryStream())
                {
                    resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);

                    resizedImage.Recycle();

                    return ms.ToArray();
                }
            }
            return null;
        }
        //public byte[] ResizeImage(string filename, float width, float height)
        //{
        //    if (File.Exists(filename))
        //    {
        //        byte[] imageData2 = RecalculateImage(filename, width, height);

        //        byte[] imageData = File.ReadAllBytes(filename);

        //        // Load the bitmap 
        //        BitmapFactory.Options options = new BitmapFactory.Options();// Create object of bitmapfactory's option method for further option use
        //        options.InPurgeable = true; // inPurgeable is used to free up memory while required
        //        Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);

        //        float newHeight = 0;
        //        float newWidth = 0;

        //        var originalHeight = originalImage.Height;
        //        var originalWidth = originalImage.Width;

        //        if (originalHeight > originalWidth)
        //        {
        //            newHeight = height;
        //            float ratio = originalHeight / height;
        //            newWidth = originalWidth / ratio;
        //        }
        //        else
        //        {
        //            newWidth = width;
        //            float ratio = originalWidth / width;
        //            newHeight = originalHeight / ratio;
        //        }

        //        Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, true);

        //        originalImage.Recycle();

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);

        //            resizedImage.Recycle();

        //            return ms.ToArray();
        //        }
        //    }
        //    return null;
        //}
    }
}